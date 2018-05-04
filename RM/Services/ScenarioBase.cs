using System.Net.Http;
using System.Text;

namespace DataServer.Services
{
	public class ScenarioBase
	{
		public static string apiUrlBase = "poad040100781";
		public static string apiUrl = $"http://{apiUrlBase}/TOTVSBusinessConnect/wsDataServer.asmx";
		public static string contexto = "CODCOLIGADA=2;CODSISTEMA=O;CODUSUARIO=mestre";
		public static string usuario = "mestre";
		public static string senha = "totvs";

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
				Usuario = usuario;
				Senha = senha;
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
			var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, apiUrl);
			httpRequestMessage.Headers.Add("SOAPAction", $@"http://www.totvs.com.br/br/{soapAction}");
			httpRequestMessage.Content = content;
			return httpRequestMessage;
		}

	}
}
