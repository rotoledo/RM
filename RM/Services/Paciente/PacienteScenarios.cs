using Xunit;
using RM_Stuff;
using System.Net;
using System.Net.Http;
using DataServer_Stuff;
using System.Threading.Tasks;
using DataServer.Services;
using System.Text.RegularExpressions;
using System.Diagnostics;
using System;
using System.Text;

namespace DataServer.IntegrationTests.Services.Paciente
{
	public class PacienteScenarios : PacienteScenarioBase
	{

		[Fact]
		public async Task Read_Paciente_returns_OK_Status_code()
		{
			// GIVEN
			var readRecordEnvelopeBody = ReadRecordSZPacienteEnvelopeBody(ScenarioBase.Coligada, "997852");
			var httpRequestMessage = SZPacienteBuilder(readRecordEnvelopeBody, SOAPAction.ReadRecordAuth);

			// WHEN
			var response = await new HttpClient().SendAsync(httpRequestMessage);

			// THEN
			response.EnsureSuccessStatusCode();
			string xmlContent = Methods.GetInnerTextFromResponseBySoapAction(response, SOAPAction.ReadRecordAuth);
			Assert.Contains("<SZPACIENTE>", xmlContent);
			Assert.Contains("</SZPACIENTE>", xmlContent);
		}

		[Fact]
		public async Task Create_Paciente_returns_OK_Status_code()
		{
			// GIVEN
			var szPacienteXml = SZPacienteXmlBuilder(new SZPACIENTE());
			var saveRecordEnvelopeBody = SaveRecordSZPacienteEnvelopeBody(szPacienteXml);
			var httpRequestMessage = SZPacienteBuilder(saveRecordEnvelopeBody, SOAPAction.SaveRecordAuth);

			// WHEN
			var response = await new HttpClient().SendAsync(httpRequestMessage);

			// THEN
			response.EnsureSuccessStatusCode();
			string xmlContent = Methods.GetInnerTextFromResponseBySoapAction(response, SOAPAction.SaveRecordAuth);
			Assert.Matches(new Regex("^[0-9]+$"), xmlContent.Split(';')[0]);
			Assert.Matches(new Regex("^[0-9]+$"), xmlContent.Split(';')[1]);
		}

		[Fact]
		public async Task Update_Paciente_returns_OK_Status_code()
		{
			// GIVEN
			var szPacienteXml = SZPacienteXmlBuilder(new SZPACIENTE() { CODPACIENTE = "997852" });
			var saveRecordEnvelopeBody = SaveRecordSZPacienteEnvelopeBody(szPacienteXml);
			var httpRequestMessage = SZPacienteBuilder(saveRecordEnvelopeBody, SOAPAction.SaveRecordAuth);

			// WHEN
			var response = await new HttpClient().SendAsync(httpRequestMessage);

			// THEN
			response.EnsureSuccessStatusCode();
			string xmlContent = Methods.GetInnerTextFromResponseBySoapAction(response, SOAPAction.SaveRecordAuth);
			Assert.Matches(new Regex("^[0-9]+$"), xmlContent.Split(';')[0]);
			Assert.Matches(new Regex("^[0-9]+$"), xmlContent.Split(';')[1]);
		}

		[Fact]
		public async Task Delete_Paciente_returns_400_Status_code()
		{
			// GIVEN
			var szPacienteXml = SZPacienteXmlBuilder(new SZPACIENTE() { CODPACIENTE = "997852" });
			var saveRecordEnvelopeBody = SaveRecordSZPacienteEnvelopeBody(szPacienteXml);
			var httpRequestMessage = SZPacienteBuilder(saveRecordEnvelopeBody, SOAPAction.DeleteRecordAuth);

			// WHEN
			var response = await new HttpClient().SendAsync(httpRequestMessage);

			// THEN
			Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
		}

		[Fact]
		public async Task Update_Non_Existent_Paciente_returns_400_Status_code()
		{
			// GIVEN
			var szPacienteXml = SZPacienteXmlBuilder(new SZPACIENTE() { CODPACIENTE = "kijvbraewivbriewbvn548578934758947t9jorngl" });
			var saveRecordEnvelopeBody = SaveRecordSZPacienteEnvelopeBody(szPacienteXml);
			var httpRequestMessage = SZPacienteBuilder(saveRecordEnvelopeBody, SOAPAction.SaveRecordAuth);

			// WHEN
			var response = await new HttpClient().SendAsync(httpRequestMessage);

			// THEN
			Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
		}

		[Fact]
		public async Task Read_Non_Existent_Paciente_returns_400_Status_code()
		{
			// GIVEN
			var readRecordEnvelopeBody = ReadRecordSZPacienteEnvelopeBody("xxx123456xxx", "xxx123456xxx");
			var httpRequestMessage = SZPacienteBuilder(readRecordEnvelopeBody, SOAPAction.ReadRecordAuth);

			// WHEN
			var response = await new HttpClient().SendAsync(httpRequestMessage);

			// THEN
			Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
		}

		[Fact]
		public async Task Read_Paciente_returns_INVALID_REQUEST()
		{
			// GIVEN
			var readRecordEnvelopeBody = ReadRecordSZPacienteEnvelopeBody(ScenarioBase.Coligada, "997852");
			string envelope = EnvelopeXmlBuilder(readRecordEnvelopeBody, SOAPAction.ReadRecordAuth);
			var content = new StringContent(envelope, Encoding.UTF8, "text/xml");
			var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, ApiUrl);
			httpRequestMessage.Headers.Add("SOAPAction", $@"http://www.totvs.com.br/br/{SOAPAction.ReadRecordAuth}");
			httpRequestMessage.Content = content;

			// WHEN
			//var response = await new HttpClient().SendAsync(httpRequestMessage);
			var response = await new HttpClient().PostAsync(ApiUrl, content);

			// THEN
			Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
		}

		[Theory]
		[InlineData(3000)]
		public async Task Read_Paciente_Time_Response_Under(int timeout)
		{
			// GIVEN
			var readRecordEnvelopeBody = ReadRecordSZPacienteEnvelopeBody(ScenarioBase.Coligada, "997852");
			var httpRequestMessage = SZPacienteBuilder(readRecordEnvelopeBody, SOAPAction.ReadRecordAuth);
			HttpResponseMessage response = new HttpResponseMessage();
			HttpClient httpClient = new HttpClient
			{
				Timeout = TimeSpan.FromMilliseconds(timeout)
			};
			var stopwatch = new Stopwatch();

			// WHEN
			try
			{
				stopwatch.Start();
				response = await httpClient.SendAsync(httpRequestMessage);
				stopwatch.Stop();
			}
			// THEN
			catch (TaskCanceledException)
			{
				throw new Exception($"Tempo de resposta da Request foi de {stopwatch.ElapsedMilliseconds} milisegundos, excedendo o Timeout de {timeout} milisegundos");
			}
		}
	}
}
