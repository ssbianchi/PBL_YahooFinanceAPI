using LiveCharts;
using LiveCharts.Wpf;
using PBL_YahooFinance.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YahooFinanceApi;

namespace PBL_YahooFinance.ViewModels
{
    public class LoadHistoricalPriceViewModel : Prism.Mvvm.BindableBase
    {
        private readonly HistoricalPriceModel dto;
        #region Variables/Properties
        public ObservableCollection<HistoricalPriceModel> HistoricalPrice { get; set; }
        public SeriesCollection ChartPriceValues { get; set; }
        public string[] ChartDates { get; set; }
        public Func<double, string> YFormatter { get; set; }
        #endregion

        #region Construtctors
        public LoadHistoricalPriceViewModel()
        {
            dto = new HistoricalPriceModel();
            HistoricalPrice = new ObservableCollection<HistoricalPriceModel>();
        }
        #endregion

        #region Methods
        public static LoadHistoricalPriceViewModel LoadViewModel(Action<Task> onLoaded = null)
        {
            LoadHistoricalPriceViewModel viewModel = new LoadHistoricalPriceViewModel();

            viewModel.Load().ContinueWith(t => onLoaded?.Invoke(t));

            return viewModel;
        }
        public async Task Load()
        {
            var result = await Yahoo.GetHistoricalAsync("AAPL", new DateTime(2017, 1, 3), DateTime.Now, Period.Daily);

            var closePrice = result.Select(a => a.Close).ToList();
            var dateArray = result.Select(a => a.DateTime.ToString()).ToArray();

            await LoadChart(closePrice, dateArray);

            RaiseAllCanExecuteChanged();
        }
        public async Task<bool> LoadChart(List<decimal> closePriceList, string[] dateArray)
        {
            ChartPriceValues = new SeriesCollection
            {
                new LineSeries
                {
                    Title = "ClosePrice",
                    Values = new ChartValues<decimal>(closePriceList),
                    PointGeometry = null
                },
            };

            ChartDates = dateArray;
            YFormatter = value => value.ToString("C");

            return true;
        }
        #endregion
        private void RaiseAllCanExecuteChanged()
        {
            RaisePropertyChanged(nameof(ChartPriceValues));
            RaisePropertyChanged(nameof(YFormatter));
            RaisePropertyChanged(nameof(ChartDates));
        }
    }
}
