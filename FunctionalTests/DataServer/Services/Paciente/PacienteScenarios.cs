using Xunit;
using RM_Stuff;
using System.Net.Http;
using DataServer_Stuff;
using System.Threading.Tasks;

namespace FunctionalTests.DataServer.Services.Paciente
{
	public class PacienteScenarios : PacienteScenarioBase
	{

		[Fact]
		public async Task Can_Create_Paciente()
		{
			// GIVEN
			var szPacienteXml = SZPacienteXmlBuilder(new SZPACIENTE());
			var saveRecordEnvelopeBody = SaveRecordSZPacienteEnvelopeBody(szPacienteXml);
			var httpRequestMessage = SZPacienteBuilder(saveRecordEnvelopeBody, SOAPAction.SaveRecordAuth);

			// WHEN
			var response = await new HttpClient().SendAsync(httpRequestMessage);

			// THEN
			string xmlContent = Methods.GetInnerTextFromResponseBySoapAction(response, SOAPAction.SaveRecordAuth);
			Assert.Equal(Coligada, xmlContent.Split(';')[0]);
			Assert.NotEmpty(xmlContent.Split(';')[1]);
		}

		[Fact]
		public async Task Can_Read_Paciente()
		{
			// GIVEN // Arrange
			var readRecordEnvelopeBody = ReadRecordSZPacienteEnvelopeBody(ScenarioBase.Coligada, "997852");
			var httpRequestMessage = SZPacienteBuilder(readRecordEnvelopeBody, SOAPAction.ReadRecordAuth);

			// WHEN // Act
			var response = await new HttpClient().SendAsync(httpRequestMessage);

			// THEN // Assert
			var dbPaciente = ExtractPacienteFromReadRecordResponse(response);
			Assert.Equal("997852", dbPaciente.CODPACIENTE);
			Assert.Equal("TESTE DE AUTOMAÇÃO COM API - NÃO ALTERAR DADOS DESSE PACIENTE", dbPaciente.OBSERVACAO);
		}

		[Fact]
		public async Task Can_Update_And_Read_Paciente()
		{
			// GIVEN
			var paciente = new SZPACIENTE() { CODPACIENTE = CodPaciente };
			var szPacienteXml = SZPacienteXmlBuilder(paciente);
			var saveRecordEnvelopeBody = SaveRecordSZPacienteEnvelopeBody(szPacienteXml);
			var httpRequestMessage_SaveRecord = SZPacienteBuilder(saveRecordEnvelopeBody, SOAPAction.SaveRecordAuth);

			// AND
			var responseUpdate = await new HttpClient().SendAsync(httpRequestMessage_SaveRecord);

			// AND
			var readRecordEnvelopeBody = ReadRecordSZPacienteEnvelopeBody(ScenarioBase.Coligada, CodPaciente);
			var httpRequestMessage_ReadRecord = SZPacienteBuilder(readRecordEnvelopeBody, SOAPAction.ReadRecordAuth);

			// WHEN
			var responseReadRecord = await new HttpClient().SendAsync(httpRequestMessage_ReadRecord);

			// THEN
			var dbPaciente = ExtractPacienteFromReadRecordResponse(responseReadRecord);
			Assert.Equal(paciente.NOMEPACIENTE, dbPaciente.NOMEPACIENTE);
			Assert.Equal(paciente.CPF, dbPaciente.CPF);
			Assert.Equal(paciente.CODPACIENTE, dbPaciente.CODPACIENTE);
		}


	}
}
