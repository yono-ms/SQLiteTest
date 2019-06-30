using System;
using System.IO;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SQLiteTest
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new NavigationPage(new MainPage());
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }

        static SQLiteTestDatabase database;

        public static SQLiteTestDatabase Database
        {
            get
            {
                if (database == null)
                {
                    AppLog.Debug($"static変数databaseがnull");
                    database = new SQLiteTestDatabase(
                      Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "SQLiteTest.db3"));
                }
                else
                {
                    AppLog.Debug($"static変数databaseが存在している");
                }
                return database;
            }
        }
    }
}
