using Dapper;
using Microsoft.Data.SqlClient;

namespace Repoex.Server.Services.ExportacaoServices
{
    public class ExportacaoService : IExportacaoService
    {
        private readonly IConfiguration _configuration;

        public ExportacaoService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<List<Exportacao>> ObterRelatorio()
        {
            using var connection = new SqlConnection(_configuration.GetConnectionString("WorkflowConnectionString"));

            var exportacoes = await connection.QueryAsync<Exportacao>(
                "SELECT Fluxo AS 'Fluxo', " +
                "Solicitante AS 'Solicitante', " +
                "Modalidade AS 'Modalidade', " +
                "Planta AS 'Planta', " +
                "Status AS 'Status', " +
                "IIF(SOLICITACAO IS NULL, '-', SOLICITACAO) AS 'Solicitacao', " +
                "IIF(EXPEDICAO IS NULL, '-', EXPEDICAO) AS 'Expedicao', " +
                "IIF(APROV_COMEX IS NULL, '-', APROV_COMEX) AS 'AprovacaoComex', " +
                "IIF(PROCESSAMENTO_1 IS NULL, '-', PROCESSAMENTO_1) AS 'Processamento1', " +
                "IIF(PROCESSAMENTO_2 IS NULL, '-', PROCESSAMENTO_2) AS 'Processamento2', " +
                "IIF(PREPARACAO_DOCUMENTACAO IS NULL, '-', PREPARACAO_DOCUMENTACAO) AS 'PreparacaoDocumentacao', " +
                "IIF(EXPEDICAO_MATERIAL IS NULL, '-', EXPEDICAO_MATERIAL) AS 'ExpedicaoMaterial', " +
                "IIF(CONFIRMACAO_ENTREGA IS NULL, '-', CONFIRMACAO_ENTREGA) AS 'ConfirmacaoEntrega' " +
                "FROM ( SELECT  " +
                "Processo.COD_PROCESSO AS 'Fluxo', " +
                "Solicitante.NOM_USUARIO AS 'Solicitante', " +
                "Etapa.TITULO_ETAPA AS 'Etapa', " +
                "Formulario.Modalidade AS 'Modalidade', " +
                "Formulario.CENTRO_DIST AS 'Planta', " +
                "CASE " +
                "WHEN Processo.IDE_FINALIZADO LIKE 'P' THEN 'APROVADO' " +
                "WHEN Processo.IDE_FINALIZADO LIKE 'R' THEN 'REJEITADO' " +
                "ELSE EtapaAtual.TITULO_ETAPA  " +
                "END AS 'Status', " +
                "CONVERT(VARCHAR(16), ProcessoEtapa.DAT_FINALIZACAO, 120) AS 'Finalizacao' " +
                "FROM PROCESSO AS Processo " +
                "INNER JOIN processo_etapa AS ProcessoEtapa " +
                "ON Processo.COD_PROCESSO = ProcessoEtapa.COD_PROCESSO " +
                "INNER JOIN etapa AS Etapa " +
                "ON ProcessoEtapa.COD_ETAPA = Etapa.COD_ETAPA " +
                "AND Processo.COD_FORM = Etapa.COD_FORM " +
                "INNER JOIN etapa AS EtapaAtual " +
                "ON Processo.COD_FORM = EtapaAtual.COD_FORM " +
                "AND Processo.COD_ETAPA_ATUAL = EtapaAtual.COD_ETAPA " +
                "AND Processo.COD_VERSAO = EtapaAtual.COD_VERSAO " +
                "INNER JOIN USUARIO AS Solicitante " +
                "ON Processo.COD_USUARIO = Solicitante.COD_USUARIO " +
                "INNER JOIN f_soli_export AS Formulario " +
                "ON Processo.COD_PROCESSO = Formulario.COD_PROCESSO_F " +
                "AND Processo.COD_ETAPA_ATUAL = Formulario.COD_ETAPA_F " +
                "AND Processo.COD_CICLO_ATUAL = Formulario.COD_CICLO_F " +
                "WHERE Processo.IDE_FINALIZADO NOT LIKE 'C' " +
                "AND Processo.IDE_BETA_TESTE NOT LIKE 'S' " +
                "AND Etapa.TITULO_ETAPA NOT LIKE 'N/A' " +
                "AND Etapa.TITULO_ETAPA NOT LIKE 'CANCELAMENTO' " +
                "AND Etapa.TITULO_ETAPA NOT LIKE 'REVISAO' " +
                "AND Processo.COD_ETAPA_ATUAL NOT LIKE 1) " +
                "AS src PIVOT ( MAX(Finalizacao) " +
                "FOR ETAPA IN ([SOLICITACAO], [EXPEDICAO], [APROV_COMEX], [PROCESSAMENTO_1],  " +
                "[PROCESSAMENTO_2], [PREPARACAO_DOCUMENTACAO], [EXPEDICAO_MATERIAL], [CONFIRMACAO_ENTREGA])) " +
                "AS pvt ORDER BY Fluxo; "
                );

            return exportacoes.ToList();
        }
    }
}