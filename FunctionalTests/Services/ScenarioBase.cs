using System.Net.Http;
using System.Text;

namespace DataServer.FunctionalTests.Services
{
	public class ScenarioBase
	{
		public static string Usuario = "mestre";
		public static string Senha = "totvs";
		public static string Coligada = "2";
		public static string ApiUrlBase = "poad040100781";
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
			else attributeName = "XML";

			return $@"<soap:Envelope xmlns:soap=""http://www.w3.org/2003/05/soap-envelope"" xmlns:br=""http://www.totvs.com.br/br/"">
							<soap:Header/>
								<soap:Body>
								<br:{soapAction}>
									<br:DataServerName>{envelopeBody.DataServerName}</br:DataServerName>
									<br:{attributeName}>{envelopeBody.Content}</br:{attributeName}>
									<br:Contexto>{envelopeBody.Contexto}</br:Contexto>
									<br:Usuario>{envelopeBody.Usuario}</br:Usuario>
									<br:Senha>{envelopeBody.Senha}</br:Senha>
								</br:{soapAction}>
							</soap:Body>
						</soap:Envelope>";
		}

		public HttpRequestMessage RequestMessageBuider(EnvelopeBody envelopeBody, string soapAction)
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
