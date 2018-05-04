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

	}
}
