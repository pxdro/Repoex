using Dapper;
using Microsoft.Data.SqlClient;

namespace Repoex.Server.Services.PedidoEngenhariaServices
{
    public class PedidoEngenhariaService : IPedidoEngenhariaService
    {
        private readonly IConfiguration _configuration;

        public PedidoEngenhariaService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<List<PedidoEngenharia>> ObterRelatorio()
        {
            using var connection = new SqlConnection(_configuration.GetConnectionString("WorkflowConnectionString"));

            var pedidosEngenharia = (await connection.QueryAsync<PedidoEngenharia>(
                "SELECT " +
                "pe.cod_processo_f AS 'Fluxo',  " +
                "pe.num_pe AS 'NumeroPE',  " +
                "p.DAT_DATA AS 'AbertoEm',  " +
                "pe.SOLICITANTE AS 'Solicitante',  " +
                "pe.TITULO AS 'Titulo', " +
                "pe.AERONAVE_LISTA AS 'Aeronave',  " +
                "pe.KIT AS 'KitSN',  " +
                "e.DES_ETAPA AS 'Etapa',  " +
                "(SELECT nom_usuario  " +
                "FROM usuario  " +
                "WHERE COD_USUARIO = pet.COD_USUARIO_ETAPA)  " +
                "AS 'Usuario',  " +
                "spe.status AS 'StatusPE', " +
                "'Posicao' =  " +
                "CASE p.IDE_FINALIZADO  " +
                "WHEN 'A' THEN 'Em Andamento'  " +
                "WHEN 'P' THEN 'Aprovado'  " +
                "WHEN 'R' THEN 'Rejeitado'  " +
                "END,  " +
                "pe.ENGE AS 'Engenheiro', " +
                "pe.DATA_1_ANALISE AS 'DataAnalise',  " +
                "pe.SELECIONAR_APOIO AS 'Apoio',  " +
                "pe.CCB AS 'CCB',  " +
                "pe.ATA_CCB AS 'AtaCCB', " +
                "pe.ORCAMENTO_APONT AS 'OrdemApontamento',  " +
                "pe.NUM_CL AS 'NumeroCL',  " +
                "pe.ENGE_CL AS 'CriadorCL',  " +
                "pe.ORIGEM AS 'Origem',  " +
                "rpt.PN_DESENHO AS 'PN', " +
                "rpt.PN_REVISAO AS 'Revisao',  " +
                "'OrcamentoProjeto' =  " +
                "CASE  " +
                "WHEN pe.ORCAMENTO_PROJETO is null THEN 0  " +
                "ELSE pe.ORCAMENTO_PROJETO END,  " +
                "'OrcamentoEngDesenv' =  " +
                "CASE " +
                "WHEN pe.ORCAMENTO_DESENVOLVI is null THEN 0  " +
                "ELSE pe.ORCAMENTO_DESENVOLVI END, " +
                "'OrcamentoEngIndus' =  " +
                "CASE  " +
                "WHEN pe.ORCAMENTO_INDUSTRI is null THEN 0  " +
                "ELSE pe.ORCAMENTO_INDUSTRI END,  " +
                "'OrcamentoProducao' =  " +
                "CASE  " +
                "WHEN pe.ORCAMENTO_PRODUCAO is null THEN 0  " +
                "ELSE pe.ORCAMENTO_PRODUCAO END,  " +
                "'OrcamentoTechPub' =  " +
                "CASE  " +
                "WHEN pe.ORCAMENTO_TECHPUB is null THEN 0  " +
                "ELSE pe.ORCAMENTO_TECHPUB END,  " +
                "'OrcamentoAeronaveg' =  " +
                "CASE " +
                "WHEN pe.ORCAMENTO_AERONAVEGA is null THEN 0  " +
                "ELSE pe.ORCAMENTO_AERONAVEGA END,  " +
                "REPLACE(REPLACE(REPLACE(CAST(pe.DESCRICAO AS VARCHAR(3000)), CHAR(9), ' '), CHAR(10), ' '), CHAR(13), ' ') AS 'Descricao',  " +
                "REPLACE(REPLACE(REPLACE(CAST(pe.MOTIVO AS VARCHAR(3000)), CHAR(9), ' '), CHAR(10), ' '), CHAR(13), ' ') AS 'Motivo',  " +
                "REPLACE(REPLACE(REPLACE(CAST(pe.MOTIVO_RECUSA_PE AS VARCHAR(3000)), CHAR(9), ' '), CHAR(10), ' '), CHAR(13), ' ') AS 'MotivoRecusaEng',  " +
                "REPLACE(REPLACE(REPLACE(CAST(pe.MOTIVO_REPROVACAO_PE AS VARCHAR(3000)), CHAR(9), ' '), CHAR(10), ' '), CHAR(13), ' ') AS 'MotivoRecusaGestConfig',  " +
                "REPLACE(REPLACE(REPLACE(CAST(pe.OBS_ANALISE AS VARCHAR(3000)), CHAR(9), ' '), CHAR(10), ' '), CHAR(13), ' ') AS 'ObsEng',  " +
                "REPLACE(REPLACE(REPLACE(CAST(pe.OBS_APOIO AS VARCHAR(3000)), CHAR(9), ' '), CHAR(10), ' '), CHAR(13), ' ') AS 'ObsApoio',  " +
                "REPLACE(REPLACE(REPLACE(CAST(pe.OBS_APROVACAO AS VARCHAR(3000)), CHAR(9), ' '), CHAR(10), ' '), CHAR(13), ' ') AS 'ObsGestConfig',  " +
                "REPLACE(REPLACE(REPLACE(CAST(pe.OBS_FINAIS AS VARCHAR(3000)), CHAR(9), ' '), CHAR(10), ' '), CHAR(13), ' ') AS 'ObsFinal',  " +
                "REPLACE(REPLACE(REPLACE(CAST(pe.OBS_GESTOR_CONFIG AS VARCHAR(3000)), CHAR(9), ' '), CHAR(10), ' '), CHAR(13), ' ') AS 'ObsGestor'  " +
                "FROM f_eng_pe pe  " +
                "INNER JOIN processo p  " +
                "ON (p.COD_PROCESSO = pe.COD_PROCESSO_F)  " +
                "INNER JOIN etapa e  " +
                "ON (e.COD_FORM = p.COD_FORM AND pe.COD_ETAPA_F = e.COD_ETAPA)   " +
                "INNER JOIN hb_wf_status_pe spe  " +
                "ON(e.DES_ETAPA = spe.etapa)  " +
                "INNER JOIN f_eng_peRPT rpt  " +
                "ON(pe.COD_PROCESSO_F = rpt.COD_PROCESSO_F AND pe.COD_ETAPA_F = rpt.COD_ETAPA_F)  " +
                "INNER JOIN processo_etapa pet  " +
                "ON(pe.COD_PROCESSO_F = pet.COD_PROCESSO AND pe.COD_ETAPA_F = pet.COD_ETAPA)  " +
                "WHERE pe.COD_ETAPA_F = p.COD_ETAPA_ATUAL  " +
                "AND p.DAT_DATA >= '2018-01-01'  " +
                "AND p.IDE_BETA_TESTE = 'N'  " +
                "AND pe.COD_PROCESSO_F  " +
                "NOT IN (215743,216399,219375,224927,230282,230990,234137,238555,238594,239240,244590,245195,248308,248972,249585,250037,250334,253409) " +
                "ORDER BY pe.COD_PROCESSO_F "
            )).ToList();

            pedidosEngenharia.AddRange((await connection.QueryAsync<PedidoEngenharia>(
                "SELECT DISTINCT " +
                "pe.cod_processo_f AS 'Fluxo', " +
                "pe.num_pe AS 'NumeroPE', " +
                "p.DAT_DATA AS 'AbertoEm', " +
                "pe.SOLICITANTE AS 'Solicitante', " +
                "pe.TITULO AS 'Titulo', " +
                "pe.AERONAVE_LISTA AS 'Aeronave', " +
                "pe.KIT AS 'KitSN', " +
                "pe.programa AS 'Programa', " +
                "CONVERT (VARCHAR, pe.EFETIVIDADE) AS 'Configuracao',  " +
                "CONVERT (VARCHAR, pe.INSTALACAO) AS 'Instalacao', " +
                "e.DES_ETAPA AS 'Etapa', " +
                "(SELECT nom_usuario FROM usuario WHERE COD_USUARIO = pet.COD_USUARIO_ETAPA)AS 'Usuario', " +
                "spe.status AS 'StatusPE', " +
                "'Posicao' = CASE p.IDE_FINALIZADO " +
                "WHEN 'A' THEN 'Em Andamento' " +
                "WHEN 'P' THEN 'Aprovado' " +
                "WHEN 'R' THEN 'Rejeitado' " +
                "END, " +
                "pe.SISTEMA AS 'Sistema', " +
                "pe.SUB_SISTEMA AS 'SubSistema', " +
                "pe.ENGE AS 'Engenheiro',  " +
                "pe.DATA_1_ANALISE AS 'DataAnalise', " +
                "pe.SELECIONAR_APOIO AS 'Apoio', " +
                "pe.CCB AS 'CCB', " +
                "pe.ATA_CCB AS 'AtaCCB', " +
                "pe.ORCAMENTO_APONT AS 'OrdemApontamento', " +
                "pe.NUM_CL AS 'NumeroCL', " +
                "pe.ENGE_CL AS 'CriadorCL', " +
                "pe.ORIGEM AS 'Origem', " +
                "pe.tipo_falha AS 'TipoFalha', " +
                "'DocumentoAfetado' = CASE " +
                "WHEN (rpt.DOCUMENTO_AFETADO is null and pe.cod_processo_f < 326170) THEN '' " +
                "WHEN (pe.cod_processo_f < 326170 ) THEN rpt.DOCUMENTO_AFETADO " +
                "WHEN (gddoc.LT_DOCUMENTO_AFETADO is null) THEN '' " +
                "ELSE gddoc.LT_DOCUMENTO_AFETADO " +
                "END, " +
                "'RevDocumentoAfetado' = CASE " +
                "WHEN (rpt.REV_DOC_AFETADO is null and pe.cod_processo_f < 326170) THEN '' " +
                "WHEN (pe.cod_processo_f < 326170) THEN rpt.REV_DOC_AFETADO " +
                "WHEN (gddoc.LT_REV_DOC_AFETADO is null) THEN '' " +
                "ELSE gddoc.LT_REV_DOC_AFETADO " +
                "END, " +
                "'FatorNQ' = CASE " +
                "WHEN (rpt.FATOR_NQ is null and pe.cod_processo_f < 326170) THEN '' " +
                "WHEN (pe.cod_processo_f < 326170) THEN rpt.FATOR_NQ " +
                "WHEN (gddoc.LS_FATOR_NQ is null) THEN '' " +
                "ELSE gddoc.LS_FATOR_NQ " +
                "END, " +
                "'Part Number' = CASE " +
                "WHEN (rpt.PN_DESENHO is null and pe.cod_processo_f < 326170) THEN '' " +
                "WHEN (pe.cod_processo_f < 326170) THEN rpt.PN_DESENHO " +
                "WHEN (gddes.LT_PN_DESENHO is null) THEN '' " +
                "ELSE gddes.LT_PN_DESENHO " +
                "END, " +
                "'Revisao' = CASE " +
                "WHEN (rpt.PN_REVISAO is null and pe.cod_processo_f < 326170) THEN '' " +
                "WHEN (pe.cod_processo_f < 326170) THEN rpt.PN_REVISAO " +
                "WHEN (gddes.LT_PN_REVISAO is null) THEN '' " +
                "ELSE gddes.LT_PN_REVISAO " +
                "END, " +
                "'OrcamentoProjeto' = CASE " +
                "WHEN pe.ORCAMENTO_PROJETO is null THEN 0 " +
                "ELSE pe.ORCAMENTO_PROJETO " +
                "END, " +
                "'OrcamentoEngDesenv' = CASE " +
                "WHEN pe.ORCAMENTO_DESENVOLVI is null THEN 0 " +
                "ELSE pe.ORCAMENTO_DESENVOLVI " +
                "END, " +
                "'OrcamentoEngIndus' = CASE " +
                "WHEN pe.ORCAMENTO_INDUSTRI is null THEN 0 " +
                "ELSE pe.ORCAMENTO_INDUSTRI " +
                "END, " +
                "'OrcamentoProducao' = CASE " +
                "WHEN pe.ORCAMENTO_PRODUCAO is null THEN 0 " +
                "ELSE pe.ORCAMENTO_PRODUCAO " +
                "END, " +
                "'OrcamentoTechPub' = CASE " +
                "WHEN pe.ORCAMENTO_TECHPUB is null THEN 0 " +
                "ELSE pe.ORCAMENTO_TECHPUB " +
                "END, " +
                "'OrcamentoAeronaveg' = CASE " +
                "WHEN pe.ORCAMENTO_AERONAVEGA is null THEN 0 " +
                "ELSE pe.ORCAMENTO_AERONAVEGA " +
                "END, " +
                "REPLACE(REPLACE(REPLACE(CAST(pe.DESCRICAO AS VARCHAR(3000)), CHAR(9), ' '), CHAR(10), ' '), CHAR(13), ' ') AS 'Descricao', " +
                "REPLACE(REPLACE(REPLACE(CAST(pe.MOTIVO AS VARCHAR(3000)), CHAR(9), ' '), CHAR(10), ' '), CHAR(13), ' ') AS 'Motivo', " +
                "REPLACE(REPLACE(REPLACE(CAST(pe.MOTIVO_RECUSA_PE AS VARCHAR(3000)), CHAR(9), ' '), CHAR(10), ' '), CHAR(13), ' ') AS 'MotivoRecusaEng', " +
                "REPLACE(REPLACE(REPLACE(CAST(pe.MOTIVO_REPROVACAO_PE AS VARCHAR(3000)), CHAR(9), ' '), CHAR(10), ' '), CHAR(13), ' ') AS 'MotivoRecusaGestConfig', " +
                "REPLACE(REPLACE(REPLACE(CAST(pe.OBS_ANALISE AS VARCHAR(3000)), CHAR(9), ' '), CHAR(10), ' '), CHAR(13), ' ') AS 'ObsEng', " +
                "REPLACE(REPLACE(REPLACE(CAST(pe.OBS_APOIO AS VARCHAR(3000)), CHAR(9), ' '), CHAR(10), ' '), CHAR(13), ' ') AS 'ObsApoio', " +
                "REPLACE(REPLACE(REPLACE(CAST(pe.OBS_APROVACAO AS VARCHAR(3000)), CHAR(9), ' '), CHAR(10), ' '), CHAR(13), ' ') AS 'ObsGestConfig', " +
                "REPLACE(REPLACE(REPLACE(CAST(pe.OBS_FINAIS AS VARCHAR(3000)), CHAR(9), ' '), CHAR(10), ' '), CHAR(13), ' ') AS 'ObsFinal', " +
                "REPLACE(REPLACE(REPLACE(CAST(pe.OBS_GESTOR_CONFIG AS VARCHAR(3000)), CHAR(9), ' '), CHAR(10), ' '), CHAR(13), ' ') AS 'ObsGestor', " +
                "pe.DATA_REFERENCIA AS 'DataReferencia' " +
                "FROM f_eng_pe_v1 pe " +
                "inner join processo p ON (p.COD_PROCESSO = pe.COD_PROCESSO_F) " +
                "inner join etapa e ON (e.COD_FORM = p.COD_FORM and pe.COD_ETAPA_F = e.COD_ETAPA) " +
                "inner join hb_wf_status_pe spe ON (e.DES_ETAPA = spe.etapa) " +
                "left join f_eng_pe_v1RPT rpt ON (pe.COD_PROCESSO_F = rpt.COD_PROCESSO_F and pe.COD_ETAPA_F = rpt.COD_ETAPA_F) " +
                "left join g_eng_pe_v1GD_DOCUMENTO gddoc ON (pe.COD_PROCESSO_F = gddoc.COD_PROCESSO and pe.COD_ETAPA_F = gddoc.COD_ETAPA) " +
                "left join g_eng_pe_v1GD_DESENHO gddes ON (pe.COD_PROCESSO_F = gddes.COD_PROCESSO and pe.COD_ETAPA_F = gddes.COD_ETAPA) " +
                "inner join processo_etapa pet ON (pe.COD_PROCESSO_F = pet.COD_PROCESSO and pe.COD_ETAPA_F = pet.COD_ETAPA) " +
                "WHERE pe.COD_ETAPA_F = p.COD_ETAPA_ATUAL " +
                "and p.DAT_DATA >= '2018-01-01' " +
                "and pe.num_pe not like '%teste%' " +
                "and pe.SOLICITANTE <> 'Carvalho, Joao' " +
                "and p.IDE_BETA_TESTE = 'N' " +
                "and pe.COD_PROCESSO_F " +
                "not in (215743,216399,219375,224927,230282,230990,234137,238555,238594,239240,244590,245195,248308,248972,249585,250037,250334,253409) " +
                "ORDER BY pe.COD_PROCESSO_F; "
            )).ToList());

            return pedidosEngenharia;
        }
    }
}
