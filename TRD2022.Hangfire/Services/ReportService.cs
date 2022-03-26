using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TRD2022.Hangfire.Infra.DAL;
using TRD2022.Hangfire.Models;
using TRD2022.Hangfire.Models.Cross;

namespace TRD2022.Hangfire.Services
{
    public class ReportService
    {
        // pegar os dados do csv de dias anteriores e que ainda não estão no banco, transformar em classe e mandar para o banco de dados como backup
        private DBCommunication _dbService;
        private FileCommunication _fileService;

        public ReportService()
        {

        }

        public void ExecuteFileBackup()
        {
            try
            {
                _fileService = new FileCommunication(AppSettings.TRDFolder + "\\REPORTS");
                var result = _fileService.GetDataFromCsv($"REPORTS-{DateTime.Now.AddDays(-1).ToString("yyyyMMdd")}").Select(x => TransformLineIntoPosition(x)).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }


        }

        private Position TransformLineIntoPosition(string line)
        {
            string[] values = line.Split(';');
            Position position = new Position(Convert.ToDateTime(values[0]), ConvertOpType(values[1]), values[2], Convert.ToDecimal(values[3]), Convert.ToDecimal(values[4]), Convert.ToDecimal(values[5]), Convert.ToDecimal(values[6]), Convert.ToDecimal(values[7]), ConvertRecType(values[8]));
            return position;
        }

        private static RecommendationType ConvertRecType(string value)
        {
            switch (value.ToLower())
            {
                case "day":
                    return RecommendationType.Day;

                case "hour":
                    return RecommendationType.Hour;

                case "minute":
                    return RecommendationType.Minute;

                default:
                    return RecommendationType.Day;
            }
        }

        private static OperationType ConvertOpType(string value)
        {
            switch (value)
            {
                case "[COMPRA]":
                    return OperationType.COMPRA;
                case "[VENDA]":
                    return OperationType.VENDA;
                default:
                    return OperationType.COMPRA;
            }
        }
    }
}
