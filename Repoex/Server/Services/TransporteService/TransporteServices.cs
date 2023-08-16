using Dapper;
using Microsoft.Data.SqlClient;

namespace Repoex.Server.Services.TransporteServices
{
    public class TransporteService : ITransporteService
    {
        private readonly IConfiguration _configuration;

        public TransporteService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<List<Transporte>> ObterRelatorio()
        {
            using var connection = new SqlConnection(_configuration.GetConnectionString("WorkflowConnectionString"));

            var transportes = await connection.QueryAsync<Transporte>(
                "SELECT " +
                "Processo.COD_PROCESSO AS 'Fluxo', " +
                "TRIM(UPPER(Solicitante.NOM_USUARIO)) AS 'Solicitante', " +
                "TRIM(UPPER(GridPassageiro.NOME_PASSAGEIRO_GRID)) AS 'Passageiro', " +
                "GridPassageiro.TEL_PASSAGEIRO_GRID AS 'Telefone', " +
                "TRIM(UPPER(GridPassageiro.RG_GRID)) AS 'RG', " +
                "TRIM(GridPassageiro.EMAIL_GRID) AS 'Email', " +
                "CASE WHEN GridPassageiro.CC_PEP_GRID = 'PEP' THEN GridPassageiro.PEP_GRID " +
                "WHEN GridPassageiro.CC_PEP_GRID = 'CC' THEN GridPassageiro.CC_GRID " +
                "ELSE '' END AS 'PEP_CC', " +
                "CamposDoFormulario.MODALIDADE AS 'Modalidade', " +
                "GridCT.HORA_PARTIDA AS 'Horario', " +
                "GridCT.END_PARTIDA AS 'Partida', " +
                "GridCT.END_DESTINO AS 'Destino', " +
                "CASE WHEN GridCT.AEROPORTO_GRID = 'Sim' " +
                "THEN GridCT.VOO_PROV_GRID " +
                "ELSE '-' END AS 'VooDe', " +
                "CASE WHEN GridCT.AEROPORTO_GRID = 'Sim' " +
                "THEN GridCT.HORA_VOO_GRID " +
                "ELSE '-' END AS 'Embarque', " +
                "CASE WHEN GridCT.AEROPORTO_GRID = 'Sim' " +
                "THEN GridCT.NUM_TERMINAL_GRID " +
                "ELSE '-' END AS 'Terminal', " +
                "CASE WHEN GridCT.AEROPORTO_GRID = 'Sim' " +
                "THEN GridCT.NUM_VOO_GRID " +
                "ELSE '-' END AS 'Numero', " +
                "CASE WHEN GridCT.AEROPORTO_GRID = 'Sim' " +
                "THEN GridCT.NOME_CIA_AEREA_GRID " +
                "ELSE '-' END AS 'Companhia' " +
                "FROM PROCESSO AS Processo " +
                "INNER JOIN f_trans_pes_2 AS CamposDoFormulario " +
                "ON Processo.COD_PROCESSO = CamposDoFormulario.COD_PROCESSO_F " +
                "AND Processo.COD_ETAPA_ATUAL = CamposDoFormulario.COD_ETAPA_F " +
                "AND Processo.COD_CICLO_ATUAL = CamposDoFormulario.COD_CICLO_F " +
                "INNER JOIN g_trans_pes_2GD_PASSAGEIROS AS GridPassageiro " +
                "ON Processo.COD_PROCESSO = GridPassageiro.COD_PROCESSO " +
                "AND Processo.COD_ETAPA_ATUAL = GridPassageiro.COD_ETAPA " +
                "AND Processo.COD_CICLO_ATUAL = GridPassageiro.COD_CICLO " +
                "INNER JOIN g_trans_pes_2GRID_CT_2TURNO AS GridCT " +
                "ON Processo.COD_PROCESSO = GridCT.COD_PROCESSO " +
                "AND Processo.COD_ETAPA_ATUAL = GridCT.COD_ETAPA " +
                "AND Processo.COD_CICLO_ATUAL = GridCT.COD_CICLO " +
                "INNER JOIN USUARIO AS Solicitante " +
                "ON Processo.COD_USUARIO = Solicitante.COD_USUARIO " +
                "WHERE Processo.COD_FORM = 160 " +
                "AND Processo.COD_ETAPA_ATUAL = 5 " +
                "AND CAST(GridCT.DT_PARTIDA AS DATE) = CAST((GETDATE()+1) AS DATE) " +
                "AND Processo.IDE_FINALIZADO NOT LIKE 'C' " +
                "AND Processo.IDE_FINALIZADO NOT LIKE 'R' " +
                "AND Processo.IDE_BETA_TESTE NOT LIKE 'S' " +
                "ORDER BY GridPassageiro.NOME_PASSAGEIRO_GRID "
                );

            return transportes.ToList();
        }
    }
}