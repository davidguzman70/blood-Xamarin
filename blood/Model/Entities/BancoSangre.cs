using System;
using Microsoft.WindowsAzure.MobileServices;
using Newtonsoft.Json;

namespace blood
{

	public class BancoSangre
	{
		string id;
		string persona;
		string contacto;
		string tipo;
		bool done;

		[JsonProperty(PropertyName = "id")]
		public string Id
		{
			get { return id; }
			set { id = value; }
		}

		[JsonProperty(PropertyName = "persona")]
		public string Persona
		{
			get { return persona; }
			set { persona = value; }
		}

		[JsonProperty(PropertyName = "contacto")]
		public string Contacto
		{
			get { return contacto; }
			set { contacto = value; }
		}

		[JsonProperty(PropertyName = "tipo")]
		public string Tipo
		{
			get { return tipo; }
			set { tipo = value; }
		}

		[JsonProperty(PropertyName = "complete")]
		public bool Done
		{
			get { return done; }
			set { done = value; }
		}


		[Version]
		public string Version { get; set; }
	}




} // end namespace blood









