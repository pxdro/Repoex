using Dapper;
using Microsoft.Data.SqlClient;

namespace Repoex.Server.Services.CotacaoServices
{
    public class CotacaoService : ICotacaoService
    {
        private readonly IConfiguration _configuration;

        public CotacaoService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<List<Cotacao>> ObterRelatorio()
        {
            using var connection = new SqlConnection(_configuration.GetConnectionString("WorkflowConnectionString"));

            var cotacoes = await connection.QueryAsync<Cotacao>(
                "SELECT " +
                "P.COD_PROCESSO AS 'Fluxo', " +
                "E.TITULO_ETAPA AS 'Etapa', " +
                "PE.COD_CICLO AS 'Ciclo', " +
                "SUBSTRING(CAST(FCC.DATA AS VARCHAR), 1, 11) AS 'Necessidade', " +
                "CAST(PE.DAT_GRAVACAO AS VARCHAR) AS 'AbertoEm', " +
                "ISNULL(CAST(PE.DAT_FINALIZACAO AS VARCHAR), 'Em andamento') AS 'FinalizadoEm', " +
                "IIF(PE.DAT_FINALIZACAO IS NULL AND PE.COD_ETAPA = 3, 'Ainda nao aprovado', CAST(U.NOM_USUARIO AS VARCHAR)) AS 'Executor', " +
                "IIF(PE.VLR_TEMPO_CONSUMIDO IS NULL,'Em andamento',CAST(CAST(PE.VLR_TEMPO_CONSUMIDO / 60 / 1000 AS NUMERIC(36,2)) AS VARCHAR)) AS 'Consumido' " +
                "FROM PROCESSO P " +
                "INNER JOIN processo_etapa PE " +
                "ON P.COD_PROCESSO = PE.COD_PROCESSO " +
                "INNER JOIN etapa E " +
                "ON PE.COD_ETAPA = E.COD_ETAPA AND P.COD_FORM = E.COD_FORM " +
                "INNER JOIN USUARIO U " +
                "ON PE.COD_USUARIO_ETAPA = U.COD_USUARIO " +
                "INNER JOIN f_cota_compra FCC " +
                "ON P.COD_PROCESSO = FCC.COD_PROCESSO_F AND PE.COD_ETAPA = FCC.COD_ETAPA_F AND PE.COD_CICLO = FCC.COD_CICLO_F " +
                "WHERE " +
                "CAST(P.DAT_DATA AS DATE) > CAST(DATEADD(month, -2, GETDATE()) AS DATE) " +
                "AND P.COD_FORM = 182 " +
                "AND P.IDE_FINALIZADO NOT LIKE 'C' " +
                "AND P.IDE_FINALIZADO NOT LIKE 'R' " +
                "AND P.IDE_BETA_TESTE NOT LIKE 'S' " +
                "AND P.COD_PROCESSO NOT IN " +
                "(SELECT P.COD_PROCESSO " +
                "FROM PROCESSO P " +
                "INNER JOIN processo_etapa PE " +
                "ON P.COD_PROCESSO = PE.COD_PROCESSO " +
                "AND P.IDE_FINALIZADO NOT LIKE 'C' " +
                "AND P.COD_FORM = 182 " +
                "GROUP BY P.COD_PROCESSO " +
                "HAVING COUNT(P.COD_PROCESSO)=1) " +
                "ORDER BY P.COD_PROCESSO, PE.DAT_GRAVACAO "
                );

            return cotacoes.ToList();
        }
    }
}
