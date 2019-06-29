using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace SQLiteTest
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        public Command SearchCommand => new Command(async () =>
        {
            if (IsBusy) return;

            try
            {
                IsBusy = true;
                await Navigation.PushAsync(new SearchPage());
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.ToString());
                await DisplayAlert("システムエラー", ex.Message, "OK");
            }
            finally
            {
                IsBusy = false;
            }
        });
    }
}
