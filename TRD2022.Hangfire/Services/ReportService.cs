using Newtonsoft.Json;
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

        public void ExecuteJsonChange()
        {
            try
            {
                string jsonPath = $"D:\\repos\\TRD2022 Ecosystem\\TRD2022.Hangfire\\TRD2022.Hangfire.Infra\\testefile.json";
                _fileService = new FileCommunication(AppSettings.TRDFolder + "\\REPORTS");
                string json = _fileService.GetJsonContent(jsonPath);

                CustomerSettings jsonObj = JsonConvert.DeserializeObject<CustomerSettings>(json);
                jsonObj.AppConfiguration.ApiConfiguration.Key = "CHANGE TEST";

                string output = JsonConvert.SerializeObject(jsonObj, Formatting.Indented);
                _fileService.WriteOnFile(jsonPath, output);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public void ExecuteFileBackup()
        {
            try
            {
                _fileService = new FileCommunication(AppSettings.TRDFolder + "\\REPORTS");
                //List<Position> positions = _fileService.GetDataFromCsv($"REPORTS-{DateTime.Now.AddDays(-1).ToString("yyyyMMdd")}").Select(x => TransformLineIntoPosition(x)).ToList();

                //SendToDB(positions);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void SendToDB(List<Position> positions)
        {
            _dbService = new DBCommunication(AppSettings.ConnString);
            // TODO: find a bulk copy for mysql
            foreach (Position pos in positions)
            {
                Dictionary<string, object> param = PositionIntoDictionary(pos);
                var res = _dbService.ExecuteProc("TRD2022_InsertPosition", param);
            }
        }

        private Dictionary<string, object> PositionIntoDictionary(Position pos)
        {
            Dictionary<string, object> res = new Dictionary<string, object>();

            res.Add("symbol", pos.Symbol);
            res.Add("type", pos.OperationType);
            res.Add("dtOperation", pos.DtOperation);
            res.Add("initialPrice", pos.InitialPrice);
            res.Add("finalPrice", pos.LastPrice);
            res.Add("initialTotal", pos.InitialValue);
            res.Add("finalTotal", pos.LastValue);
            res.Add("valorization", pos.Valorization);
            res.Add("quantity", pos.Quantity);

            return res;
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
