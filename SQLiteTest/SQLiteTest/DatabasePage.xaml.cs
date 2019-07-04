using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SQLiteTest
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DatabasePage : ContentPage
    {
        public DatabasePage()
        {
            InitializeComponent();
        }

        private ObservableCollection<ZipcodeItem> _Items = new ObservableCollection<ZipcodeItem>();

        public ObservableCollection<ZipcodeItem> Items
        {
            get { return _Items; }
            set { _Items = value; OnPropertyChanged(); }
        }

        private string _Prefcode;

        public string Prefcode
        {
            get { return _Prefcode; }
            set { _Prefcode = value; OnPropertyChanged(); }
        }

        public Command SearchCommand => new Command(async () =>
        {
            if (IsBusy) return;
            try
            {
                IsBusy = true;

                Items.Clear();

                var results = await App.Database.GetZipcodeItemsAsync();
                foreach (var item in results)
                {
                    Items.Add(item);
                }
            }
            catch (Exception ex)
            {
                AppLog.Error(ex);
                await DisplayAlert("システムエラー", ex.Message, "OK");
            }
            finally
            {
                IsBusy = false;
            }
        });
        public Command FilterCommand => new Command(async () =>
        {
            if (IsBusy) return;
            try
            {
                IsBusy = true;

                Items.Clear();

                var results = await App.Database.GetZipcodeItemsRecentlyAsync();
                foreach (var item in results)
                {
                    Items.Add(item);
                }
            }
            catch (Exception ex)
            {
                AppLog.Error(ex);
                await DisplayAlert("システムエラー", ex.Message, "OK");
            }
            finally
            {
                IsBusy = false;
            }
        });
        public Command PrefCommand => new Command(async () =>
        {
            if (IsBusy) return;
            try
            {
                IsBusy = true;

                Items.Clear();

                var results = await App.Database.GetZipcodeItemsPrefcodeAsync(Prefcode);
                foreach (var item in results)
                {
                    Items.Add(item);
                }
            }
            catch (Exception ex)
            {
                AppLog.Error(ex);
                await DisplayAlert("システムエラー", ex.Message, "OK");
            }
            finally
            {
                IsBusy = false;
            }
        });
    }
}