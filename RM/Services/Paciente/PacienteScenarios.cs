using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using DataServer.Services;
using Xunit;

namespace DataServer.IntegrationTests.Services.Paciente
{
	public class PacienteScenarios : PacienteScenarioBase
	{

		[Fact]
		public async Task Can_Read_Paciente()
		{
			// GIVEN
			var readRecordEnvelopeBody = ReadRecordSZPacienteEnvelopeBody(ScenarioBase.Coligada, "997852");
			var httpRequestMessage = SZPacienteBuilder(readRecordEnvelopeBody, SOAPAction.ReadRecordAuth);

			// WHEN
			var response = await new HttpClient().SendAsync(httpRequestMessage);
			var dbPaciente = ExtractPacienteFromReadRecordResponse(response);

			// THEN
			response.EnsureSuccessStatusCode();
			Assert.Equal(HttpStatusCode.OK, response.StatusCode);
			Assert.Equal(readRecordEnvelopeBody.Content.Split(';')[0], dbPaciente.CODCOLIGADA);
			Assert.Equal(readRecordEnvelopeBody.Content.Split(';')[1], dbPaciente.CODPACIENTE);
		}

		[Fact]
		public async Task Can_Create_Paciente()
		{
			// GIVEN
			var szPacienteXml = SZPacienteXmlBuilder(new SZPACIENTE());
			var saveRecordEnvelopeBody = SaveRecordSZPacienteEnvelopeBody(szPacienteXml);
			var httpRequestMessage = SZPacienteBuilder(saveRecordEnvelopeBody, SOAPAction.SaveRecordAuth);

			// WHEN
			var response = await new HttpClient().SendAsync(httpRequestMessage);
			string xmlContent = Methods.GetInnerTextFromSaveRecordResponse(response);

			// THEN
			response.EnsureSuccessStatusCode();
			Assert.NotEmpty(xmlContent.Split(';')[0]);
			Assert.NotEmpty(xmlContent.Split(';')[1]);
		}

		[Fact]
		public async Task Can_Update_Paciente()
		{
			// GIVEN
			var paciente = new SZPACIENTE() { CODPACIENTE = "997852" };
			var szPacienteXml = SZPacienteXmlBuilder(paciente);
			var saveRecordEnvelopeBody = SaveRecordSZPacienteEnvelopeBody(szPacienteXml);
			var httpRequestMessage = SZPacienteBuilder(saveRecordEnvelopeBody, SOAPAction.SaveRecordAuth);

			// WHEN
			var response = await new HttpClient().SendAsync(httpRequestMessage);
			string xmlContent = Methods.GetInnerTextFromSaveRecordResponse(response);

			// THEN
			Assert.True(response.IsSuccessStatusCode, response.StatusCode + xmlContent);
		}


		[Fact]
		public async Task Can_Update_And_Read_Paciente()
		{
			// GIVEN
			var paciente = new SZPACIENTE() { CODPACIENTE = "997852" };
			var szPacienteXml = SZPacienteXmlBuilder(paciente);
			var saveRecordEnvelopeBody = SaveRecordSZPacienteEnvelopeBody(szPacienteXml);
			var httpRequestMessage_SaveRecord = SZPacienteBuilder(saveRecordEnvelopeBody, SOAPAction.SaveRecordAuth);

			// AND
			var responseUpdate = await new HttpClient().SendAsync(httpRequestMessage_SaveRecord);

			// AND
			var readRecordEnvelopeBody = ReadRecordSZPacienteEnvelopeBody(ScenarioBase.Coligada, "997852");
			var httpRequestMessage_ReadRecord = SZPacienteBuilder(readRecordEnvelopeBody, SOAPAction.ReadRecordAuth);

			// WHEN
			var responseReadRecord = await new HttpClient().SendAsync(httpRequestMessage_ReadRecord);
			var dbPaciente = ExtractPacienteFromReadRecordResponse(responseReadRecord);

			// THEN
			Assert.Equal(paciente.NOMEPACIENTE, dbPaciente.NOMEPACIENTE);
		}


	}
}
