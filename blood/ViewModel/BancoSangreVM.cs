using System;
using System.Collections.ObjectModel;
using Xamarin.Forms;

namespace blood
{

public class BancoSangreVM : ObservableBaseObject
	{
		public ObservableCollection<BancoSangre> Contacts { get; set; }
		private AzureClient _client;
		public Command RefreshCommand { get; set; }
		public Command PurgeCommand { get; set; }
		public Command agregarPersonaCommand { get; set; }
		private bool isBusy;
		private string _persona;

		public bool IsBusy
		{
			get { return isBusy; }
			set { isBusy = value; OnPropertyChanged(); }
		}

		public string personaTxt
		{ 
			get { return _persona; }
			set { _persona = value; OnPropertyChanged(); }
		}

		public BancoSangreVM()
		{
			RefreshCommand = new Command(() => Load());
			agregarPersonaCommand = new Command(() => agregarDonante());
			PurgeCommand = new Command(() => purgeContacts());
			Contacts = new ObservableCollection<BancoSangre>();
			_client = new AzureClient();


		}

		void purgeContacts()
		{
			_client.PurgeData();
		}

		async void agregarDonante()
		{

/*
			string[] names = { "José Luis", "Miguel Ángel", "José Francisco", "Jesús Antonio", "Jorge", "Alberto",
								"Sofía", "Camila", "Valentina", "Isabella", "Ximena", "Ana"};
			string[] lastNames = { "Hernández", "García", "Martínez", "López", "González", "Méndez", "Castillo", "Corona", "Cruz" };

			Random rdn = new Random(DateTime.Now.Millisecond);
			for (int i = 0; i < 10; i++)
			{
				_client.AddContact(new BancoSangre() { persona = $"{names[rdn.Next(0, 12)]} {lastNames[rdn.Next(0, 8)]}" });

			}
*/	

			_client.AddContact(personaTxt);
		}

		public async void Load()
		{ 
			var result = await _client.GetContacts();

			Contacts.Clear();

			foreach (var item in result)
			{
				Contacts.Add(item);
			}
			IsBusy = false;
		}

	}



} // end namespace blood



	