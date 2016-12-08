using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using System.Collections.Generic;

namespace blood
{
	public partial class BancoPage : ContentPage
	{

		BancoSangreItemManager manager;


		public BancoPage()
		{
			InitializeComponent();
			manager = BancoSangreItemManager.DefaultManager;

			// OnPlatform<T> doesn't currently support the "Windows" target platform, so we have this check here.
			if (manager.IsOfflineEnabled &&
				(Device.OS == TargetPlatform.Windows || Device.OS == TargetPlatform.WinPhone))
			{
				var syncButton = new Button
				{
					Text = "Sync items",
					HeightRequest = 30
				};
				syncButton.Clicked += OnSyncItems;

				buttonsPanel.Children.Add(syncButton);
			}
		}

		protected override async void OnAppearing()
		{
			base.OnAppearing();

			// Set syncItems to true in order to synchronize the data on startup when running in offline mode
			await RefreshItems(true, syncItems: true);
		}

		// Data methods
		async Task AddItem(BancoSangre item)
		{
			await manager.SaveTaskAsync(item);
			bancoSangreList.ItemsSource = await manager.GetTodoItemsAsync();
		}

		async Task CompleteItem(BancoSangre item)
		{
			item.Done = true;
			await manager.SaveTaskAsync(item);
			bancoSangreList.ItemsSource = await manager.GetTodoItemsAsync();
		}

		public async void OnAdd(object sender, EventArgs e)
		{
			var banco = new BancoSangre { Persona = newItemName.Text, Contacto = newItemEmail.Text, Tipo = SangreTipoPicker1.Items[SangreTipoPicker1.SelectedIndex] };
			await AddItem(banco);

			newItemName.Text = string.Empty;
			newItemName.Unfocus();
		}

		// Event handlers
		public async void OnSelected(object sender, SelectedItemChangedEventArgs e)
		{
			var banco = e.SelectedItem as BancoSangre;
			if (Device.OS != TargetPlatform.iOS && banco != null)
			{
				// Not iOS - the swipe-to-delete is discoverable there
				if (Device.OS == TargetPlatform.Android)
				{
					await DisplayAlert(banco.Persona, "Mantenga presionado para eliminar a: " + banco.Persona, "OK!");
				}
				else
				{
					// Windows, not all platforms support the Context Actions yet
					if (await DisplayAlert("Eliminar?", "Esta seguro de eliminar " + banco.Persona + "?", "Eliminar", "Cancel"))
					{
						await CompleteItem(banco);
					}
				}
			}

			// prevents background getting highlighted
			bancoSangreList.SelectedItem = null;
		}

		// http://developer.xamarin.com/guides/cross-platform/xamarin-forms/working-with/listview/#context
		public async void OnComplete(object sender, EventArgs e)
		{
			var mi = ((MenuItem)sender);
			var todo = mi.CommandParameter as BancoSangre;
			await CompleteItem(todo);
		}

		// http://developer.xamarin.com/guides/cross-platform/xamarin-forms/working-with/listview/#pulltorefresh
		public async void OnRefresh(object sender, EventArgs e)
		{
			var list = (ListView)sender;
			Exception error = null;
			try
			{
				await RefreshItems(false, true);
			}
			catch (Exception ex)
			{
				error = ex;
			}
			finally
			{
				list.EndRefresh();
			}

			if (error != null)
			{
				await DisplayAlert("Refresh Error", "Couldn't refresh data (" + error.Message + ")", "OK");
			}
		}

		public async void OnSyncItems(object sender, EventArgs e)
		{
			await RefreshItems(true, true);
		}

		private async Task RefreshItems(bool showActivityIndicator, bool syncItems)
		{
			using (var scope = new ActivityIndicatorScope(syncIndicator, showActivityIndicator))
			{
				bancoSangreList.ItemsSource = await manager.GetTodoItemsAsync(syncItems);
			}
		}

		private class ActivityIndicatorScope : IDisposable
		{
			private bool showIndicator;
			private ActivityIndicator indicator;
			private Task indicatorDelay;

			public ActivityIndicatorScope(ActivityIndicator indicator, bool showIndicator)
			{
				this.indicator = indicator;
				this.showIndicator = showIndicator;

				if (showIndicator)
				{
					indicatorDelay = Task.Delay(2000);
					SetIndicatorActivity(true);
				}
				else
				{
					indicatorDelay = Task.FromResult(0);
				}
			}

			private void SetIndicatorActivity(bool isActive)
			{
				this.indicator.IsVisible = isActive;
				this.indicator.IsRunning = isActive;
			}

			public void Dispose()
			{
				if (showIndicator)
				{
					indicatorDelay.ContinueWith(t => SetIndicatorActivity(false), TaskScheduler.FromCurrentSynchronizationContext());
				}
			}
		}


	}
}




    

