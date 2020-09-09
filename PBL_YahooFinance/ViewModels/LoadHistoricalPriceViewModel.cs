using LiveCharts;
using LiveCharts.Wpf;
using PBL_YahooFinance.Commands;
using PBL_YahooFinance.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using YahooFinanceApi;

namespace PBL_YahooFinance.ViewModels
{
    public class LoadHistoricalPriceViewModel : Prism.Mvvm.BindableBase
    {
        #region Variables/Properties
        public ObservableCollection<HistoricalPriceModel> HistoricalPrice { get; set; }

        public ICommand LoadHistoricalInfoAsyncCommand { get; set; }
        string _loadAsset;
        public string LoadAsset
        {
            get { return _loadAsset; }
            set
            {
                if (_loadAsset == value)
                    return;
                _loadAsset = value;
                RaisePropertyChanged(nameof(LoadAsset));
            }
        }
        private string _statusMessage;
        public string StatusMessage
        {
            get
            {
                return _statusMessage;
            }
            set
            {
                _statusMessage = value;
                RaisePropertyChanged(nameof(StatusMessage));
                OnPropertyChanged(nameof(HasStatusMessage));
            }
        }
        public bool HasStatusMessage => !string.IsNullOrEmpty(StatusMessage);


        DateTime _SelectedStartDate;
        public DateTime SelectedStartDate
        {
            get { return _SelectedStartDate; }
            set
            {
                if (_SelectedStartDate == value)
                    return;
                _SelectedStartDate = value;
                RaisePropertyChanged(nameof(SelectedStartDate));
                RaisePropertyChanged(nameof(SelectedEndDate));
            }
        }
        public DateTime StartDate { get; set; }

        DateTime _selectedEndDate;
        public DateTime SelectedEndDate
        {
            get { return _selectedEndDate; }
            set
            {
                if (_selectedEndDate == value)
                    return;
                _selectedEndDate = value;
                RaisePropertyChanged(nameof(SelectedStartDate));
                RaisePropertyChanged(nameof(SelectedEndDate));
            }
        }
        public DateTime EndDate { get; set; }

        public SeriesCollection ChartPriceValues { get; set; }
        public string[] ChartDates { get; set; }
        public Func<double, string> YFormatter { get; set; }
        #endregion

        #region Construtctors
        public LoadHistoricalPriceViewModel()
        {
            HistoricalPrice = new ObservableCollection<HistoricalPriceModel>();
            LoadHistoricalInfoAsyncCommand = new AsyncRelayCommand(LoadHistoricalPrice, (ex) => StatusMessage = ex.Message);
            StartDate = DateTime.Now.AddMonths(-1);
            EndDate = DateTime.Now.Date;
        }
        #endregion

        #region Methods
        public static LoadHistoricalPriceViewModel LoadViewModel(Action<Task> onLoaded = null)
        {
            LoadHistoricalPriceViewModel viewModel = new LoadHistoricalPriceViewModel();

            //viewModel.Load().ContinueWith(t => onLoaded?.Invoke(t));

            return viewModel;
        }
        //public async Task Load()
        //{
        //"MGLU3.SA"
        //var result = await Yahoo.GetHistoricalAsync("MGLU3.SA", new DateTime(2011, 05, 01), DateTime.Now, Period.Daily);

        //HistoricalPrice.AddRange(result.Where(a => a.Close != 0).ToList().ConvertAll(a => PBL_YahooFinance.BLL.Converter.ItemToFileHelperDto(a)));

        //var closePrice = result.Where(a => a.Close != 0).Select(a => a.Close).ToList();
        //var dateArray = result.Where(a => a.Close != 0).Select(a => a.DateTime.ToString("MM/dd/yyyy")).ToArray();

        //await LoadChart(closePrice, dateArray);

        //RaiseAllCanExecuteChanged();
        //}

        private async Task LoadHistoricalPrice()
        {
            StatusMessage = "Status: Loading...";
            var result = await Yahoo.GetHistoricalAsync(LoadAsset, StartDate, EndDate, Period.Daily);

            HistoricalPrice.Clear();
            HistoricalPrice.AddRange(result.Where(a => a.Close != 0)
                                           .OrderByDescending(a => a.DateTime)
                                           .ToList()
                                           .ConvertAll(a => PBL_YahooFinance.BLL.Converter.ItemToFileHelperDto(a)));

            var closePrice = result.Where(a => a.Close != 0).Select(a => a.Close).ToList();
            var dateArray = result.Where(a => a.Close != 0).Select(a => a.DateTime.ToString("MM/dd/yyyy")).ToArray();

            await LoadChart(closePrice, dateArray);

            RaiseAllCanExecuteChanged();
            StatusMessage = "Status: Success.";
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
            RaisePropertyChanged(nameof(StartDate));
            RaisePropertyChanged(nameof(EndDate));
            RaisePropertyChanged(nameof(ChartPriceValues));
            RaisePropertyChanged(nameof(YFormatter));
            RaisePropertyChanged(nameof(ChartDates));
            RaisePropertyChanged(nameof(StatusMessage));
        }
    }
}
