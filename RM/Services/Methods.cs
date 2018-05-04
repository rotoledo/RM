using System;
using System.IO;
using System.Net.Http;
using System.Xml;
using System.Xml.Serialization;

namespace DataServer.Services
{
	public class Methods
	{

		public static object DeserializeXMLByType<T>(string xmlString)
		{
			XmlSerializer serializer = new XmlSerializer(typeof(T));
			TextReader reader = new StringReader(xmlString);
			return (T)serializer.Deserialize(reader);
		}

		public static string GetTextbySOAPActionAndTagName(string response_content, string soapAction, string tagName)
		{
			XmlDocument xmlDocument = new XmlDocument();
			xmlDocument.LoadXml(response_content);
			var innerText = xmlDocument.GetElementsByTagName($"{soapAction}Result").Item(0).InnerText;
			xmlDocument.LoadXml(innerText);
			var _string = xmlDocument.GetElementsByTagName(tagName).Item(0).OuterXml;
			return _string;
		}

		public static string GetInnerTextFromSaveRecordResponse(HttpResponseMessage response)
		{
			var response_content = response.Content.ReadAsStringAsync().Result;
			string innerText;
			XmlDocument xmlDocument = new XmlDocument();
			xmlDocument.LoadXml(response_content);
			try
			{
				innerText = xmlDocument.GetElementsByTagName("SaveRecordAuthResponse").Item(0).InnerText;

			}
			catch
			{
				innerText = " - " + xmlDocument.GetElementsByTagName("faultstring").Item(0).InnerText;
			}

			return innerText;
		}

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
}
