using System;
using System.Xml.Serialization;

namespace DataServer.Services.Paciente
{

	[XmlRoot("SZPACIENTE")]
	public class SZPACIENTE : RMEntity
	{
		public SZPACIENTE()
		{
			this.NOMEPACIENTE = "Nome do Paciente " + DateTime.Now.ToString().Replace(":", " ").Replace(".", " ");
			this.CPF = Methods.GenerateCPF();
			this.CODPACIENTE = "0";
		}

		[XmlElement("CODCOLIGADA")]
		public string CODCOLIGADA { get; set; }

		[XmlElement("CODPACIENTE")]
		public string CODPACIENTE { get; set; }

		[XmlElement("NOMEPACIENTE")]
		public string NOMEPACIENTE { get; set; }

		[XmlElement("CPF")]
		public string CPF { get; set; }

		[XmlElement("SEXO")]
		public string SEXO { get; set; }

		[XmlElement("DATANASC")]
		public string DATANASC { get; set; }

		[XmlElement("NATURALIDADE")]
		public string NATURALIDADE { get; set; }

		[XmlElement("UFNATURALIDADE")]
		public string UFNATURALIDADE { get; set; }

		[XmlElement("PROFISSAO")]
		public string PROFISSAO { get; set; }

		[XmlElement("OBSERVACAO")]
		public string OBSERVACAO { get; set; }

	}

	public class RMEntity
	{
	}
}
