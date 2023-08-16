using Blazored.LocalStorage;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text.Json;

namespace Repoex.Client.Authentication
{
    public class CustomAuthStateProvider : AuthenticationStateProvider
    {
        public ILocalStorageService _localStorageService;
        private readonly HttpClient _httpClient;

        public CustomAuthStateProvider(ILocalStorageService localStorageService, HttpClient httpClient)
        {
            _localStorageService = localStorageService;
            _httpClient = httpClient;
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            string token = await _localStorageService.GetItemAsStringAsync("token");

            if (token == "\"\"")
                token = string.Empty;

            var state = new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));

            var identity = new ClaimsIdentity();
            _httpClient.DefaultRequestHeaders.Authorization = null;

            // Caso token ja exista
            if (!string.IsNullOrEmpty(token))
            {
                // Verifica expiracao do token
                var expiry = DateTimeOffset.FromUnixTimeSeconds(long.Parse(ParseClaimsFromJwt(token).Where(claim => claim.Type.Equals("exp")).FirstOrDefault()!.Value));
                if (expiry.UtcDateTime <= DateTime.UtcNow)
                    return state;

                // Cria a Identidade caso valido
                identity = new ClaimsIdentity(ParseClaimsFromJwt(token), "jwt");
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.Replace("\"", ""));
            }

            // Mapeia as roles caso receba multiplas
            var roles = identity.Claims.Where(claim => claim.Type.Contains("role")).FirstOrDefault();
            if (roles != null && roles.Value.Contains("["))
                identity = MapArrayClaimsToMultipleSeparateClaims(identity);

            var user = new ClaimsPrincipal(identity);
            state = new AuthenticationState(user);

            NotifyAuthenticationStateChanged(Task.FromResult(state));

            return state;
        }


        public ClaimsIdentity MapArrayClaimsToMultipleSeparateClaims(ClaimsIdentity Identity)
        {
            var rolesClaim = Identity.Claims.Where(claim => claim.Type.Contains("role")).FirstOrDefault()!.Value.Replace("[", "").Replace("]", "").Replace("\"", "").Split(",");
            Identity.RemoveClaim(Identity.Claims.Where(claim => claim.Type.Contains("role")).FirstOrDefault());

            for (int i = 0; i < rolesClaim.Length; i++)
            {
                var claim = new Claim(ClaimTypes.Role, rolesClaim[i], "http://www.w3.org/2001/XMLSchema#string");
                Identity.AddClaim(claim);
            }

            return Identity;
        }

        public static IEnumerable<Claim> ParseClaimsFromJwt(string jwt)
        {
            var payload = jwt.Split('.')[1];
            var jsonBytes = ParseBase64WithoutPadding(payload);
            var keyValuePairs = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonBytes);
            return keyValuePairs!.Select(kvp => new Claim(kvp.Key, kvp.Value.ToString()!));
        }

        private static byte[] ParseBase64WithoutPadding(string base64)
        {
            switch (base64.Length % 4)
            {
                case 2: base64 += "=="; break;
                case 3: base64 += "="; break;
            }
            return Convert.FromBase64String(base64);
        }
    }
}
