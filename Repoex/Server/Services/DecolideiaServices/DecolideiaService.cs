using Dapper;
using Microsoft.Data.SqlClient;

namespace Repoex.Server.Services.DecolideiaServices
{
    public class DecolideiaService : IDecolideiaService
    {
        private readonly IConfiguration _configuration;

        public DecolideiaService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<List<Decolideia>> ObterRelatorio()
        {
            using var connection = new SqlConnection(_configuration.GetConnectionString("WorkflowConnectionString"));

            var decolideias = await connection.QueryAsync<Decolideia>(
                "SET LANGUAGE Portuguese " +
                "SELECT Processo.COD_PROCESSO AS 'Processo', " +
                "Formulario.DESCRICAO AS 'Descricao', " +
                "Solicitante.NOM_USUARIO AS 'Solicitante', " +
                "Colaboradores.COLABORADOR1 AS 'Colaborador', " +
                "Colaboradores.DEPARTAMENTO1 AS 'Departamento', " +
                "DepartamentoResponsavel.DES_DESCRICAO AS 'Etapa', " +
                "Responsavel.NOM_USUARIO AS 'Responsavel', " +
                "CASE " +
                "WHEN Processo.IDE_FINALIZADO LIKE 'P' THEN 'Ideia Premiada' " +
                "WHEN Processo.IDE_FINALIZADO LIKE 'R' THEN 'Ideia Rejeitada' " +
                "WHEN Etapa.TITULO_ETAPA LIKE 'LANCA_IDEIA' THEN 'Abertura da Ideia' " +
                "WHEN Etapa.TITULO_ETAPA LIKE 'ANALISE_GESTOR' THEN 'Analise Gestor' " +
                "WHEN Etapa.TITULO_ETAPA LIKE 'PRE_ANALISE' THEN 'Pre analise Melhoria Continua' " +
                "WHEN Etapa.TITULO_ETAPA LIKE 'REVISAO' THEN 'Revisao da Ideia' " +
                "WHEN Etapa.TITULO_ETAPA LIKE 'ANALISE' THEN 'Analise Avaliador' " +
                "WHEN Etapa.TITULO_ETAPA LIKE 'APOIO_1' THEN 'Analise Suporte' " +
                "WHEN Etapa.TITULO_ETAPA LIKE 'ANALISE_APOIO_1' THEN 'Analise Avaliador - Apos Retorno Suporte' " +
                "WHEN Etapa.TITULO_ETAPA LIKE 'EFETIVACAO_DADOS' THEN 'Efetivacao dos Dados Avaliador' " +
                "WHEN Etapa.TITULO_ETAPA LIKE 'ANALISE_FINANCEIRA' THEN 'Analise Financeira' " +
                "WHEN Etapa.TITULO_ETAPA LIKE 'START' THEN 'Em processo de implementacao' " +
                "WHEN Etapa.TITULO_ETAPA LIKE 'ACOMPANHAMENTO' THEN 'Acompanhamento da Eficacia da Ideia' " +
                "WHEN Etapa.TITULO_ETAPA LIKE 'VALIDACAO_FINAL' THEN 'Validacao Final' " +
                "WHEN Etapa.TITULO_ETAPA LIKE 'A_PREMIAR' THEN 'A Premiar' " +
                "WHEN Etapa.TITULO_ETAPA LIKE 'FINALIZACAO' THEN 'Premiado' " +
                "WHEN Etapa.TITULO_ETAPA LIKE 'FINAL_REJEICAO' THEN 'Finalizada/Rejeitada' " +
                "ELSE '-' " +
                "END AS 'Status', " +
                "YEAR(Processo.DAT_DATA) AS 'Ano', " +
                "CONVERT(VARCHAR(10), Processo.DAT_DATA, 23) AS 'Abertura', " +
                "CONVERT(VARCHAR(10), EtapaAtual.DAT_FINALIZACAO, 23) AS 'Conclusao', " +
                "CONVERT(VARCHAR(10), EtapaAtual.DAT_GRAVACAO, 23) AS 'ParadoDesde', " +
                "DATEDIFF(day, EtapaAtual.DAT_GRAVACAO, GETDATE()) AS 'DiasParado', " +
                "Formulario.DETALHAMENTO AS 'Detalhamento', " +
                "Formulario.BENEFICIO_VAL AS 'BeneficioValorizacao', " +
                "Formulario.CUSTO_VAL AS 'CustoValorizacao', " +
                "(Formulario.BENEFICIO_VAL - Formulario.CUSTO_VAL) AS 'SavingValorizacao', " +
                "Formulario.BENEFICIO_EFIC AS 'BeneficioEficacia', " +
                "Formulario.CUSTO_EFIC AS 'CustoEficacia', " +
                "(Formulario.BENEFICIO_EFIC - Formulario.CUSTO_EFIC) AS 'SavingEficacia', " +
                "Formulario.TIPO_IDEIA AS 'TipoIdeia', " +
                "Colaboradores.VP1 AS 'Leadership' " +
                "FROM PROCESSO AS Processo " +
                "INNER JOIN f_decola AS Formulario " +
                "ON Processo.COD_PROCESSO = Formulario.COD_PROCESSO_F " +
                "AND Processo.COD_ETAPA_ATUAL = Formulario.COD_ETAPA_F " +
                "AND Processo.COD_CICLO_ATUAL = Formulario.COD_CICLO_F " +
                "INNER JOIN USUARIO AS Solicitante " +
                "ON Processo.COD_USUARIO = Solicitante.COD_USUARIO " +
                "INNER JOIN processo_etapa AS EtapaAtual " +
                "ON Processo.COD_PROCESSO = EtapaAtual.COD_PROCESSO " +
                "AND Processo.COD_ETAPA_ATUAL = EtapaAtual.COD_ETAPA " +
                "AND Processo.COD_CICLO_ATUAL = EtapaAtual.COD_CICLO " +
                "INNER JOIN etapa AS Etapa " +
                "ON Etapa.COD_FORM = Processo.COD_FORM " +
                "AND Etapa.COD_ETAPA = Processo.COD_ETAPA_ATUAL " +
                "INNER JOIN USUARIO AS Responsavel " +
                "ON EtapaAtual.COD_USUARIO_ETAPA = Responsavel.COD_USUARIO " +
                "INNER JOIN DEPTO AS DepartamentoResponsavel " +
                "ON Responsavel.COD_DEPTO = DepartamentoResponsavel.COD_DEPTO " +
                "INNER JOIN g_decolaCADASTRO_GRID AS Colaboradores " +
                "ON Processo.COD_PROCESSO = Colaboradores.COD_PROCESSO " +
                "AND Processo.COD_ETAPA_ATUAL = Colaboradores.COD_ETAPA " +
                "AND Processo.COD_CICLO_ATUAL = Colaboradores.COD_CICLO " +
                "WHERE Processo.IDE_FINALIZADO NOT LIKE 'C' " +
                "AND Processo.IDE_BETA_TESTE NOT LIKE 'S' "
                );

            return decolideias.ToList();
        }
    }
}