using Xunit;
using System;
using System.Text;
using System.Net.Http;
using System.Threading.Tasks;
using System.Xml;
using System.Net;
using System.Xml.Serialization;
using System.IO;

namespace RM.Services.RM_Paciente
{
	public class PacienteScenarios : PacienteScenarioBase
	{
		HttpClient httpClient;
		XmlDocument xmlDocument;
		HttpRequestMessage httpRequestMessage;
		Methods methods;

		public PacienteScenarios()
		{
			httpClient = new HttpClient();
			xmlDocument = new XmlDocument();
			methods = new Methods();
		}

		[Fact]
		public async Task Get_Paciente()
		{
			// Given
			ReadRecordEnvelopeBody readRecordEnvelopeBody = new ReadRecordEnvelopeBody("SauPacienteData", "2;997852");
			string envelope = ReadRecordEnvelopeBuilder(readRecordEnvelopeBody);
			var content = new StringContent(envelope, Encoding.UTF8, "text/xml");
			httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, apiUrl);
			httpRequestMessage.Headers.Add("SOAPAction", "http://www.totvs.com.br/br/ReadRecordAuth");
			httpRequestMessage.Content = content;

			// When
			var response = await httpClient.SendAsync(httpRequestMessage);

			// Handle Response Content Data  (?)
			var response_content = response.Content.ReadAsStringAsync().Result;
			string xmlContent = methods.GetTextbySOAPActionAndTagName(response_content, "ReadRecordAuth", "SZPACIENTE");
			var rmObject = (SZPACIENTE)methods.DeserializeXMLByType<SZPACIENTE>(xmlContent);

			// Then
			response.EnsureSuccessStatusCode();
			Assert.Equal(HttpStatusCode.OK, response.StatusCode);
			Assert.Equal(readRecordEnvelopeBody.PrimaryKey.Split(';')[0], rmObject.CODCOLIGADA);
			Assert.Equal(readRecordEnvelopeBody.PrimaryKey.Split(';')[1], rmObject.CODPACIENTE);
		}

		[Fact]
		public async Task Create_Paciente()
		{
			// Given
			var szPacienteXml = SZPacienteBuilder();
			string envelope = SaveRecordEnvelopeBuilder(szPacienteXml);
			var content = new StringContent(envelope, Encoding.UTF8, "text/xml" );
			httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, apiUrl);
			httpRequestMessage.Headers.Add("SOAPAction", "http://www.totvs.com.br/br/SaveRecordAuth");
			httpRequestMessage.Content = content;

			// When
			var response = await httpClient.SendAsync(httpRequestMessage);

			// Handle Response Content Data  (?)
			var response_content = response.Content.ReadAsStringAsync().Result;
			string xmlContent = methods.GetTextbySaveRecordAndTagName(response_content, "SaveRecordAuthResponse", "SaveRecordAuthResult");
			
			// Then
			response.EnsureSuccessStatusCode();
			Assert.NotEmpty(xmlContent.Split(';')[0]);
			Assert.NotEmpty(xmlContent.Split(';')[1]);
		}

	}
}
