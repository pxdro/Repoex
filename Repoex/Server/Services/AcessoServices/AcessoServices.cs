using Dapper;
using Microsoft.Data.SqlClient;

namespace Repoex.Server.Services.AcessoServices
{
    public class AcessoService : IAcessoService
    {
        private readonly IConfiguration _configuration;

        public AcessoService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<List<Acesso>> ObterRelatorio()
        {
            using var connection = new SqlConnection(_configuration.GetConnectionString("WorkflowConnectionString"));

            var acessos = await connection.QueryAsync<Acesso>(
                "SELECT " +
                "P.COD_PROCESSO AS 'Fluxo', " +
                "TRIM(UPPER(GD.LT_NOME_VISIT)) AS 'Visitante', " +
                "TRIM(UPPER(GD.LT_DOCUMENTO_RPT)) AS 'Documento', " +
                "TRIM(UPPER(GD.NOME_INST)) AS 'Instituicao', " +
                "AA.LOCAL AS 'Local', " +
                "GD.HORA_INI AS 'Chegada', " +
                "GD.HORA_ENCE AS 'Saida', " +
                "AA.NOME_RESPONSAVEL AS 'Responsavel', " +
                "AA.RAMAL_RESPONSAVEL AS 'RamalResp', " +
                "CONVERT(varchar, GD.DATA_INI, 103) AS 'DataInicial', " +
                "CONVERT(varchar, GD.DATA_ENCE, 103) AS 'DataFinal' " +
                "FROM PROCESSO P " +
                "INNER JOIN f_sol_aces_v3 AS AA " +
                "ON P.COD_PROCESSO = AA.COD_PROCESSO_F " +
                "AND P.COD_ETAPA_ATUAL = AA.COD_ETAPA_F " +
                "AND P.COD_CICLO_ATUAL = AA.COD_CICLO_F " +
                "INNER JOIN g_sol_aces_v3GD_CADAST_VISIT AS GD " +
                "ON P.COD_PROCESSO = GD.COD_PROCESSO " +
                "AND P.COD_ETAPA_ATUAL = GD.COD_ETAPA " +
                "AND P.COD_CICLO_ATUAL = GD.COD_CICLO " +
                "WHERE P.COD_FORM = 155 " +
                "AND CAST(GETDATE() AS DATE) " +
                "BETWEEN CAST(GD.DATA_INI AS DATE) AND CAST(GD.DATA_ENCE AS DATE) " +
                "AND P.COD_ETAPA_ATUAL = 5 " +
                "AND P.IDE_FINALIZADO NOT LIKE 'C' " +
                "AND P.IDE_FINALIZADO NOT LIKE 'R' " +
                "AND P.IDE_BETA_TESTE NOT LIKE 'S' " +
                "ORDER BY GD.LT_NOME_VISIT "
                );

            return acessos.ToList();
        }
    }
}