using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace RM.Services
{
	public class Methods
	{
		XmlDocument xmlDocument;

		public Methods()
		{
			xmlDocument = new XmlDocument();
		}

		public object DeserializeXMLByType<T>(string xmlString)
		{
			XmlSerializer serializer = new XmlSerializer(typeof(T));
			TextReader reader = new StringReader(xmlString);
			return (T)serializer.Deserialize(reader);
		}

		public string GetTextbySOAPActionAndTagName(string response_content, string soapAction, string tagName)
		{
			xmlDocument.LoadXml(response_content);
			var innerText = xmlDocument.GetElementsByTagName($"{soapAction}Result").Item(0).InnerText;
			xmlDocument.LoadXml(innerText);
			var _string = xmlDocument.GetElementsByTagName(tagName).Item(0).OuterXml;
			return _string;
		}

		public string GetTextbySaveRecordAndTagName(string response_content, string soapAction, string tagName)
		{
			xmlDocument.LoadXml(response_content);
			var innerText = xmlDocument.GetElementsByTagName(soapAction).Item(0).InnerText;
			return innerText;
		}
	}
}
