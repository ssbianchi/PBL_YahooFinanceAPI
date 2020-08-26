using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PBL_YahooFinance.ViewModels
{
    public class MainViewModel
    {
        public LoadHistoricalPriceViewModel LoadHistoricalPriceViewModel { get; set; }

        public MainViewModel()
        {
            LoadHistoricalPriceViewModel = LoadHistoricalPriceViewModel.LoadViewModel();
        }
    }
}
