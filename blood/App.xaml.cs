using Xamarin.Forms;

namespace blood
{
	public partial class App : Application
	{
		public App()
		{
			InitializeComponent();



			MainPage = new BancoPage();
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
	}
}

