using Binance.Net.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TRD2022.Hangfire.Models
{
    public class Position
    {
        public string Symbol { get; set; }
        public decimal Valorization { get; set; }
        public decimal InitialValue { get; set; }
        public decimal InitialPrice { get; set; }
        public decimal LastMaxPrice { get; set; }
        /// <summary>
        /// O último valor total da criptomoeda (quantity * price)
        /// </summary>
        public decimal LastValue { get; set; }
        public decimal LastPrice { get; set; }
        public decimal Quantity { get; set; }
        public decimal Risk { get; set; }
        public RecommendationType Type { get; set; }
        public OperationType OperationType { get; set; }
        public DateTime DtOperation { get; set; }

        public Position()
        { }

        public Position(DateTime dtOperation, OperationType opType, string symbol, decimal initialPrice, decimal finalPrice, decimal initialTotal, decimal finalTotal, decimal valorization, RecommendationType recType)
        {
            DtOperation = dtOperation;
            OperationType = opType;
            Symbol = symbol.Trim();
            InitialPrice = initialPrice;
            LastPrice = finalPrice;
            Quantity = initialTotal / initialPrice;
            InitialValue = initialTotal;
            LastValue = finalTotal;
            Valorization = valorization;
            Type = recType;
        }

    }

    public enum RecommendationType
    {
        Day = 0,
        Hour = 1,
        Minute = 2
    }

    public enum OperationType
    {
        VENDA,
        COMPRA
    }
}

