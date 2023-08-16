using Dapper;
using Microsoft.Data.SqlClient;

namespace Repoex.Server.Services.ViagensCentroTreinamentoServices
{
    public class ViagensCentroTreinamentoService : IViagensCentroTreinamentoService
    {
        private readonly IConfiguration _configuration;

        public ViagensCentroTreinamentoService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<List<ViagensCentroTreinamento>> ObterRelatorio()
        {
            using var connection = new SqlConnection(_configuration.GetConnectionString("WorkflowConnectionString"));

            var transportes = await connection.QueryAsync<ViagensCentroTreinamento>(
                "SELECT " +
                "Processo.COD_PROCESSO AS 'Fluxo', " +
                "Etapa.TITULO_ETAPA AS 'Etapa', " +
                "Responsavel.Nome AS 'Responsavel', " +
                "CamposDoFormulario.NUM_ADV AS 'ADV', " +
                "TRIM(UPPER(Iniciador.NOM_USUARIO)) AS 'Solicitante', " +
                "TRIM(UPPER(CamposDoFormulario.NOME_VIAJANTE)) AS 'Viajante', " +
                "TRIM(UPPER(CamposDoFormulario.REGISTRO_VIAJANTE)) AS 'Registro', " +
                "CamposDoFormulario.VALOR AS 'Valor', " +
                "CamposDoFormulario.DESTINO AS 'Destino', " +
                "CONVERT(varchar, CamposDoFormulario.DATA_IDA, 103) AS 'Ida', " +
                "CONVERT(varchar, CamposDoFormulario.DATA_RETORNO, 103) AS 'Retorno', " +
                "CamposDoFormulario.OBJ_VIAGEM AS 'Objetivo' " +
                "FROM PROCESSO Processo " +
                "INNER JOIN f_adv AS CamposDoFormulario " +
                "ON Processo.COD_PROCESSO = CamposDoFormulario.COD_PROCESSO_F " +
                "AND Processo.COD_ETAPA_ATUAL = CamposDoFormulario.COD_ETAPA_F " +
                "AND Processo.COD_CICLO_ATUAL = CamposDoFormulario.COD_CICLO_F " +
                "INNER JOIN USUARIO AS Iniciador " +
                "ON Processo.COD_USUARIO = Iniciador.COD_USUARIO " +
                "INNER JOIN etapa AS Etapa " +
                "ON Processo.COD_ETAPA_ATUAL = Etapa.COD_ETAPA " +
                "AND Processo.COD_FORM = Etapa.COD_FORM " +
                "AND Processo.COD_VERSAO = Etapa.COD_VERSAO " +
                "INNER JOIN (SELECT  " +
                "ProcessoEtapa.COD_PROCESSO,  " +
                "ProcessoEtapa.COD_ETAPA,  " +
                "ProcessoEtapa.COD_CICLO,  " +
                "STRING_AGG(Responsavel.NOM_USUARIO, '; ') AS Nome " +
                "FROM processo_etapa_usu AS ProcessoEtapa " +
                "INNER JOIN USUARIO AS Responsavel " +
                "ON ProcessoEtapa.COD_USUARIO_ETAPA = Responsavel.COD_USUARIO " +
                "GROUP BY ProcessoEtapa.COD_PROCESSO,  " +
                "ProcessoEtapa.COD_ETAPA, ProcessoEtapa.COD_CICLO) AS Responsavel " +
                "ON Processo.COD_PROCESSO = Responsavel.COD_PROCESSO " +
                "AND Processo.COD_ETAPA_ATUAL = Responsavel.COD_ETAPA " +
                "AND Processo.COD_CICLO_ATUAL = Responsavel.COD_CICLO " +
                "WHERE Processo.COD_FORM = 166 " +
                "AND CamposDoFormulario.NOME_VIAJANTE IS NOT NULL " +
                "AND Iniciador.COD_DEPTO IN (433, 516, 629) " +
                "AND Processo.IDE_FINALIZADO LIKE 'A' " +
                "ORDER BY 'Fluxo' "
                );

            return transportes.ToList();
        }
    }
}
