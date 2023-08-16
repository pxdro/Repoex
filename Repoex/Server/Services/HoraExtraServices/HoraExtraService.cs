using Dapper;
using Microsoft.Data.SqlClient;

namespace Repoex.Server.Services.HoraExtraServices
{
    public class HoraExtraService : IHoraExtraService
    {
        private readonly IConfiguration _configuration;

        public HoraExtraService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<List<HoraExtra>> ObterRelatorio()
        {
            using var connection = new SqlConnection(_configuration.GetConnectionString("WorkflowConnectionString"));

            var horasExtras = await connection.QueryAsync<HoraExtra>(
                "SET LANGUAGE Portuguese " +
                "(SELECT  " +
                "'Solicitacao' AS 'Tipo', " +
                "Processo.COD_PROCESSO AS 'Fluxo', " +
                "TRIM(GridHoraExtra.REGISTRO_NV) AS 'Registro', " +
                "TRIM(UPPER(GridHoraExtra.NOME_FUN_NV)) AS 'Funcionario', " +
                "DATENAME(WEEKDAY,GridHoraExtra.DATA_HEXTRA) AS 'DiaHE', " +
                "CONVERT(varchar(5), CAST(GridHoraExtra.DATA_HEXTRA AS DATE), 3) AS 'DataHE', " +
                "'-' AS 'DataAusencia', " +
                "GridHoraExtra.HORA_INICIO AS 'Inicio', " +
                "GridHoraExtra.HORA_TERMINO AS 'Termino', " +
                "TRIM(UPPER(Solicitante.NOM_USUARIO)) AS 'Solicitante', " +
                "GridHoraExtra.LIST_LANCHE AS 'Lanche',  " +
                "'Não' AS 'Almoco',  " +
                "'Não' AS 'DescontarAlmoco', " +
                "'-' AS 'HorasSegSex', " +
                "'-' AS 'HorasSegSab', " +
                "'-' AS 'SaldoProxPeriodo', " +
                "CAST(GridHoraExtra.DATA_HEXTRA AS DATE) AS 'DataInteira' " +
                "FROM processo AS Processo " +
                "INNER JOIN g_solhexGD_DEMAND_EXT AS GridHoraExtra " +
                "ON Processo.COD_PROCESSO = GridHoraExtra.COD_PROCESSO " +
                "AND Processo.COD_ETAPA_ATUAL = GridHoraExtra.COD_ETAPA " +
                "AND Processo.COD_CICLO_ATUAL = GridHoraExtra.COD_CICLO " +
                "INNER JOIN USUARIO AS Solicitante " +
                "ON Processo.COD_USUARIO = Solicitante.COD_USUARIO " +
                "WHERE CAST(GridHoraExtra.DATA_HEXTRA AS DATE) " +
                "BETWEEN CAST(CONVERT(DATETIME, '10/' + CONVERT(VARCHAR(2), MONTH(DATEADD(month, -1, GETDATE()))) " +
                "+ '/' + CONVERT(VARCHAR(4), YEAR(GETDATE())), 103) AS DATE) " +
                "AND CAST(CONVERT(DATETIME, '20/' + CONVERT(VARCHAR(2), MONTH(GETDATE()))  " +
                "+ '/' + CONVERT(VARCHAR(4), YEAR(GETDATE())), 103) AS DATE) " +
                "AND Processo.IDE_FINALIZADO LIKE 'A' " +
                "AND Processo.COD_ETAPA_ATUAL = 6 " +
                "AND IDE_BETA_TESTE NOT LIKE 'S' " +
                "UNION " +
                "SELECT  " +
                "'Solicitacao' AS 'Tipo', " +
                "Processo.COD_PROCESSO AS 'Fluxo', " +
                "TRIM(GridHoraExtra.REGISTRO_NV) AS 'Registro', " +
                "TRIM(UPPER(GridHoraExtra.NOME_FUN_NV)) AS 'Funcionario', " +
                "DATENAME(WEEKDAY,GridHoraExtra.DATA_HEXTRA) AS 'DiaHE', " +
                "CONVERT(varchar(5), CAST(GridHoraExtra.DATA_HEXTRA AS DATE), 3) AS 'DataHE', " +
                "'-' AS 'DataAusencia', " +
                "GridHoraExtra.HORA_INICIO AS 'Inicio', " +
                "GridHoraExtra.HORA_TERMINO AS 'Termino', " +
                "TRIM(UPPER(Solicitante.NOM_USUARIO)) AS 'Solicitante', " +
                "'Não' AS 'Lanche',  " +
                "GridHoraExtra.LIST_ALMOCO_FULL AS 'Almoco',  " +
                "GridHoraExtra.LIST_DESC_ALMOCO AS 'DescontarAlmoco', " +
                "'-' AS 'HorasSegSex', " +
                "'-' AS 'HorasSegSab', " +
                "'-' AS 'SaldoProxPeriodo', " +
                "CAST(GridHoraExtra.DATA_HEXTRA AS DATE) AS 'DataInteira' " +
                "FROM processo AS Processo " +
                "INNER JOIN g_he_sabadosGD_DEMAND_EXT AS GridHoraExtra " +
                "ON Processo.COD_PROCESSO = GridHoraExtra.COD_PROCESSO " +
                "AND Processo.COD_ETAPA_ATUAL = GridHoraExtra.COD_ETAPA " +
                "AND Processo.COD_CICLO_ATUAL = GridHoraExtra.COD_CICLO " +
                "INNER JOIN USUARIO AS Solicitante " +
                "ON Processo.COD_USUARIO = Solicitante.COD_USUARIO " +
                "WHERE CAST(GridHoraExtra.DATA_HEXTRA AS DATE) " +
                "BETWEEN CAST(CONVERT(DATETIME, '10/' + CONVERT(VARCHAR(2), MONTH(DATEADD(month, -1, GETDATE())))  " +
                "+ '/' + CONVERT(VARCHAR(4), YEAR(GETDATE())), 103) AS DATE) " +
                "AND CAST(CONVERT(DATETIME, '20/' + CONVERT(VARCHAR(2), MONTH(GETDATE()))  " +
                "+ '/' + CONVERT(VARCHAR(4), YEAR(GETDATE())), 103) AS DATE) " +
                "AND Processo.IDE_FINALIZADO LIKE 'A' " +
                "AND Processo.COD_ETAPA_ATUAL = 6 " +
                "AND IDE_BETA_TESTE NOT LIKE 'S' " +
                "UNION  " +
                "SELECT  " +
                "'Solicitacao' AS 'Tipo', " +
                "Processo.COD_PROCESSO AS 'Fluxo', " +
                "TRIM(GridHoraExtra.REGISTRO_FUN) AS 'Registro', " +
                "TRIM(UPPER(GridHoraExtra.NOME_FUN)) AS 'Funcionario', " +
                "DATENAME(WEEKDAY,GridHoraExtra.DATA_HORA_EXTRA) AS 'DiaHE', " +
                "CONVERT(varchar(5), CAST(GridHoraExtra.DATA_HORA_EXTRA AS DATE), 3) AS 'DataHE', " +
                "'-' AS 'DataAusencia', " +
                "GridHoraExtra.QNTD_HORAS AS 'Inicio', " +
                "GridHoraExtra.QNTD2_HORAS AS 'Termino', " +
                "TRIM(UPPER(Solicitante.NOM_USUARIO)) AS 'Solicitante', " +
                "'Não' AS 'Lanche',  " +
                "GridHoraExtra.NEC_ALMOCO AS 'Almoco',  " +
                "GridHoraExtra.DESCONT_HOR_ALMO AS 'DescontarAlmoco', " +
                "'-' AS 'HorasSegSex', " +
                "'-' AS 'HorasSegSab', " +
                "'-' AS 'SaldoProxPeriodo', " +
                "CAST(GridHoraExtra.DATA_HORA_EXTRA AS DATE) AS 'DataInteira' " +
                "FROM processo AS Processo " +
                "INNER JOIN g_wf_HEXDFIDENT_FUNC AS GridHoraExtra " +
                "ON Processo.COD_PROCESSO = GridHoraExtra.COD_PROCESSO " +
                "AND Processo.COD_ETAPA_ATUAL = GridHoraExtra.COD_ETAPA " +
                "AND Processo.COD_CICLO_ATUAL = GridHoraExtra.COD_CICLO " +
                "INNER JOIN USUARIO AS Solicitante " +
                "ON Processo.COD_USUARIO = Solicitante.COD_USUARIO " +
                "WHERE CAST(GridHoraExtra.DATA_HORA_EXTRA AS DATE) " +
                "BETWEEN CAST(CONVERT(DATETIME, '10/' + CONVERT(VARCHAR(2), MONTH(DATEADD(month, -1, GETDATE())))  " +
                "+ '/' + CONVERT(VARCHAR(4), YEAR(GETDATE())), 103) AS DATE) " +
                "AND CAST(CONVERT(DATETIME, '20/' + CONVERT(VARCHAR(2), MONTH(GETDATE()))  " +
                "+ '/' + CONVERT(VARCHAR(4), YEAR(GETDATE())), 103) AS DATE) " +
                "AND Processo.IDE_FINALIZADO LIKE 'A' " +
                "AND Processo.COD_ETAPA_ATUAL = 10 " +
                "AND IDE_BETA_TESTE NOT LIKE 'S' " +
                "UNION " +
                "SELECT  " +
                "'Compensacao' AS 'Tipo', " +
                "Processo.COD_PROCESSO AS 'Fluxo', " +
                "TRIM(GridHoraExtra.REGISTRO_F) AS 'Registro', " +
                "TRIM(UPPER(GridHoraExtra.NOME_F)) AS 'Funcionario', " +
                "DATENAME(WEEKDAY,GridHoraExtra.DATA_COMP_A) AS 'DiaHE', " +
                "CONVERT(varchar(5), CAST(GridHoraExtra.DATA_COMP_A AS DATE), 3) AS 'DataHE', " +
                "IIF(GridHoraExtra.DATA_AUS IS NULL, '-', CONVERT(varchar(5), CAST(GridHoraExtra.DATA_AUS AS DATE), 3)) AS 'DataAusencia', " +
                "'-' AS 'Inicio', " +
                "'-' AS 'Termino', " +
                "TRIM(UPPER(Solicitante.NOM_USUARIO)) AS 'Solicitante', " +
                "'-' AS 'Lanche',  " +
                "'-' AS 'Almoco',  " +
                "IIF(GridHoraExtra.DESC_ALMOCO IS NULL, '-', GridHoraExtra.DESC_ALMOCO) AS 'DescontarAlmoco', " +
                "IIF(GridHoraExtra.HORAS_SEG_SEX IS NULL, '-', GridHoraExtra.DESC_ALMOCO) AS 'HorasSegSex', " +
                "IIF(GridHoraExtra.HORAS_SEG_SAB IS NULL, '-', GridHoraExtra.DESC_ALMOCO) AS 'HorasSegSab', " +
                "IIF(GridHoraExtra.SALDO IS NULL, '-', GridHoraExtra.DESC_ALMOCO) AS 'SaldoProxPeriodo', " +
                "CAST(GridHoraExtra.DATA_COMP_A AS DATE) AS 'DataInteira' " +
                "FROM processo AS Processo " +
                "INNER JOIN g_comp_jorGD_AUSENCIA AS GridHoraExtra " +
                "ON Processo.COD_PROCESSO = GridHoraExtra.COD_PROCESSO " +
                "AND Processo.COD_ETAPA_ATUAL = GridHoraExtra.COD_ETAPA " +
                "AND Processo.COD_CICLO_ATUAL = GridHoraExtra.COD_CICLO " +
                "INNER JOIN USUARIO AS Solicitante " +
                "ON Processo.COD_USUARIO = Solicitante.COD_USUARIO " +
                "WHERE CAST(GridHoraExtra.DATA_COMP_A AS DATE) " +
                "BETWEEN CAST(CONVERT(DATETIME, '10/' + CONVERT(VARCHAR(2), MONTH(DATEADD(month, -1, GETDATE())))  " +
                "+ '/' + CONVERT(VARCHAR(4), YEAR(GETDATE())), 103) AS DATE) " +
                "AND CAST(CONVERT(DATETIME, '20/' + CONVERT(VARCHAR(2), MONTH(GETDATE()))  " +
                "+ '/' + CONVERT(VARCHAR(4), YEAR(GETDATE())), 103) AS DATE) " +
                "AND Processo.IDE_FINALIZADO LIKE 'A' " +
                "AND Processo.COD_ETAPA_ATUAL = 3 " +
                "AND IDE_BETA_TESTE NOT LIKE 'S') " +
                "ORDER BY 'DataInteira' "
                );

            return horasExtras.ToList();
        }
    }
}