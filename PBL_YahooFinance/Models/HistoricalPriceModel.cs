using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PBL_YahooFinance.Models
{
    public class HistoricalPriceModel : Prism.Mvvm.BindableBase
    {
        public HistoricalPriceModel() { }

        public string Date { get; set; }

        public decimal OpenPrice { get; set; }

        public decimal HighPrice { get; set; }

        public decimal LowPrice { get; set; }

        public decimal ClosePrice { get; set; }

        public decimal AdjClosePrice { get; set; }

        public decimal Volume { get; set; }
    }
}
