using Xunit;
using RM_Stuff;
using System.Net;
using System.Net.Http;
using DataServer_Stuff;
using System.Threading.Tasks;

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
	}
}
