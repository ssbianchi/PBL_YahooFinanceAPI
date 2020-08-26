using PBL_YahooFinance.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YahooFinanceApi;

namespace PBL_YahooFinance.BLL
{
    public class Converter
    {
        public static HistoricalPriceModel ItemToFileHelperDto(Candle item)
        {
            return new HistoricalPriceModel()
            {
                Date = item.DateTime.ToString("MM/dd/yyyy"),
                OpenPrice = item.Open,
                HighPrice = item.High,
                LowPrice = item.Low,
                ClosePrice = item.Close,
                AdjClosePrice = item.AdjustedClose,
                Volume = item.Volume,
            };
        }
    }
}
