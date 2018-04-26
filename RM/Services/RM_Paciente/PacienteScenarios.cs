using Xunit;
using System;
using System.Text;
using System.Net.Http;
using System.Threading.Tasks;
using System.Xml;
using System.Net;

namespace RM.Services.RM_Paciente
{
	public class PacienteScenarios : PacienteScenarioBase
	{
		HttpClient httpClient;
		XmlDocument xmlDocument;
		HttpRequestMessage httpRequestMessage;

		public PacienteScenarios()
		{
			httpClient = new HttpClient();
			xmlDocument = new XmlDocument();
		}

		[Fact]
		public async Task Get_Paciente()
		{
			// When
			ReadRecordEnvelopeBody readRecordEnvelopeBody = new ReadRecordEnvelopeBody("SauPacienteData", "2;997852");
			string envelope = EnvelopeBuilder(readRecordEnvelopeBody);
			var content = new StringContent(envelope, Encoding.UTF8, "text/xml");
			httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, apiUrl);
			httpRequestMessage.Headers.Add("SOAPAction", "http://www.totvs.com.br/br/ReadRecordAuth");
			httpRequestMessage.Content = content;

			// When
			var response = await httpClient.SendAsync(httpRequestMessage);
			var response_content = response.Content.ReadAsStringAsync().Result;

			// Then
			response.EnsureSuccessStatusCode();
			Assert.Equal(HttpStatusCode.OK, response.StatusCode);


			xmlDocument.LoadXml(response_content);
			var content02 = xmlDocument.GetElementsByTagName("ReadRecordAuthResult");
			var innerText = content02.Item(0).InnerText;
			xmlDocument.LoadXml(innerText);

		}
	}
}
