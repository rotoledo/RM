using System.IO;
using System.Net.Http;
using System.Text;

namespace DataServer_Stuff
{
	public class ScenarioBase
	{
		public static string Usuario = "mestre";
		public static string Senha = "totvs";
		public static string Coligada = "2";
		public static string ApiUrlBase = "10.51.2.133"; // "poavudev01";  // "10.51.5.89"; Diego  //  // "10.51.5.140"; // "poad040100781";
		public static string ApiUrl = $"http://{ApiUrlBase}/TOTVSBusinessConnect/wsDataServer.asmx";
		public static string contexto = $"CODCOLIGADA={Coligada};CODSISTEMA=O;CODUSUARIO={Usuario}";

		public class EnvelopeBody
		{
			public string DataServerName;
			public string Content;
			public string Contexto;
			public string Usuario;
			public string Senha;

			public EnvelopeBody(string dataServerName, string content)
			{
				DataServerName = dataServerName;
				Content = content;
				Contexto = contexto;
				Usuario = ScenarioBase.Usuario;
				Senha = ScenarioBase.Senha;
			}
		}

		public static string EnvelopeXmlBuilder(EnvelopeBody envelopeBody, string soapAction)
		{
			string attributeName;

			if (soapAction.Contains("ReadRecord"))
				attributeName = "PrimaryKey";
			else if (soapAction.Contains("ReadView"))
				attributeName = "Filtro";
			else attributeName = "XML";

			var envelope = File.ReadAllText(@".\Resources\RequestEnvelope.xml");
			envelope = envelope.Replace("soapAction", soapAction)
				.Replace("attributeName", attributeName)
				.Replace("envelopeBody.DataServerName", envelopeBody.DataServerName)
				.Replace("envelopeBody.Content", envelopeBody.Content)
				.Replace("envelopeBody.Contexto", envelopeBody.Contexto)
				.Replace("envelopeBody.Usuario", envelopeBody.Usuario)
				.Replace("envelopeBody.Senha", envelopeBody.Senha);

			return envelope;
		}

		public static HttpRequestMessage RequestMessageBuider(EnvelopeBody envelopeBody, string soapAction)
		{
			string envelope = EnvelopeXmlBuilder(envelopeBody, soapAction);
			var content = new StringContent(envelope, Encoding.UTF8, "text/xml");
			var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, ApiUrl);
			httpRequestMessage.Headers.Add("SOAPAction", $@"http://www.totvs.com.br/br/{soapAction}");
			httpRequestMessage.Content = content;
			return httpRequestMessage;
		}

	}
}
