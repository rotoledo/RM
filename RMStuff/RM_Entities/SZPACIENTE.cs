using System;
using System.Xml.Serialization;

namespace RM_Stuff
{
	[XmlRoot("SZPACIENTE")]
	public class SZPACIENTE : RMEntity
	{
		public SZPACIENTE()
		{
			this.NOMEPACIENTE = "Nome do Paciente " + DateTime.Now.ToString().Replace(":", " ").Replace(".", " ");
			this.CPF = GenerateCPF();
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


		// TODO: Mover método para outro lugar
		public static string GenerateCPF()
		{
			int soma = 0, resto = 0;
			int[] multiplicador1 = new int[9] { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
			int[] multiplicador2 = new int[10] { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };

			Random rnd = new Random();
			string semente = rnd.Next(100000000, 999999999).ToString();

			for (int i = 0; i < 9; i++)
				soma += int.Parse(semente[i].ToString()) * multiplicador1[i];

			resto = soma % 11;
			if (resto < 2)
				resto = 0;
			else
				resto = 11 - resto;

			semente = semente + resto;
			soma = 0;

			for (int i = 0; i < 10; i++)
				soma += int.Parse(semente[i].ToString()) * multiplicador2[i];

			resto = soma % 11;

			if (resto < 2)
				resto = 0;
			else
				resto = 11 - resto;

			semente = semente + resto;
			return semente;
		}
	}

	public class RMEntity
	{
	}
}
