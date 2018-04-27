using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.Collections.Generic;

namespace RM.Services.RM_Paciente
{

	[XmlRoot("SZPACIENTE")]
	public class SZPACIENTE : RMEntity
	{
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
