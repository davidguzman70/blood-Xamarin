using Xamarin.Forms;

namespace blood
{
	public partial class bloodPage : ContentPage
	{
		public bloodPage()
		{
			InitializeComponent();
			BindingContext = new BancoSangreVM();
		}
	}
}

