using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SQLiteTest
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SearchPage : ContentPage
    {
        public SearchPage()
        {
            InitializeComponent();

            Zipcode = "1234";
        }

        private string _Zipcode;

        [Required]
        public string Zipcode
        {
            get { return _Zipcode; }
            set { _Zipcode = value; OnPropertyChanged(); ZipcodeError = GetPropertyError(value); }
        }

        private string _ZipcodeError;

        public string ZipcodeError
        {
            get { return _ZipcodeError; }
            set { _ZipcodeError = value; OnPropertyChanged(); OnPropertyChanged(nameof(CanSearch)); }
        }

        public bool CanSearch => string.IsNullOrWhiteSpace(ZipcodeError);

        private ObservableCollection<ZipcodeItem> _Items = new ObservableCollection<ZipcodeItem>();

        public ObservableCollection<ZipcodeItem> Items
        {
            get { return _Items; }
            set { _Items = value; OnPropertyChanged(); }
        }

        private string GetPropertyError(object value, [CallerMemberName] string propertyName = null)
        {
            var context = new ValidationContext(this) { MemberName = propertyName };
            var result = new List<ValidationResult>();
            var isValid = Validator.TryValidateProperty(value, context, result);
            return isValid ? string.Empty : string.Join("\n", result.Select(e => e.ErrorMessage));
        }

        public Command SearchCommand => new Command(async () =>
        {
            if (IsBusy) return;
            try
            {
                IsBusy = true;

                Items.Clear();

                var result = await ZipcodeProvider.SearchAsync(Zipcode);
                if (string.IsNullOrWhiteSpace(result.message))
                {
                    foreach (var item in result.zipcodeItems)
                    {
                        Items.Add(item);
                    }
                }
                else
                {
                    await DisplayAlert("サーバーエラー", result.message, "OK");
                }
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