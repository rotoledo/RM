using System;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using DataServer_Stuff;
using IntegrationTests.DataServer.Services;
using IntegrationTests.DataServer.Services.Paciente;
using RM_Stuff;
using Xunit;

namespace RM.Web_Service
{
	public class APIScenarios : PacienteScenarioBase
	{

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
			var readRecordEnvelopeBody = ReadRecordSZPacienteEnvelopeBody(ScenarioBase.Coligada, CodPaciente);
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
		[InlineData(1000)]
		[InlineData(2000)]
		[InlineData(3000)]
		[InlineData(5000)]
		public async Task Read_Paciente_Response_Time_Under(int timeout)
		{
			// GIVEN
			var readRecordEnvelopeBody = ReadRecordSZPacienteEnvelopeBody(ScenarioBase.Coligada, CodPaciente);
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
			response.EnsureSuccessStatusCode();
		}


		[Fact]
		public async Task Security()
		{
			ApiUrlBase = "poavudev01";

			// GIVEN
			var szPacienteXml = SZPacienteXmlBuilder(new SZPACIENTE());
			var saveRecordEnvelopeBody = SaveRecordSZPacienteEnvelopeBody(szPacienteXml);
			var httpRequestMessage = SZPacienteBuilder(saveRecordEnvelopeBody, SOAPAction.SaveRecordAuth);

			// WHEN
			var response = await new HttpClient().SendAsync(httpRequestMessage);

			// THEN
			string xmlContent = Methods.GetInnerTextFromResponseBySoapAction(response, SOAPAction.SaveRecordAuth);
			Assert.Equal(HttpStatusCode.InternalServerError, response.StatusCode);


			//	Violação de chave estrangeira

			//Possíveis causas:
			//  -exclusão de registro que possui outros registros associados
			// - inclusão de registro detalhe sem um registro mestre associado
			//=======================================
			//   at RM.Lib.Data.DbServices.QueryExec(String sqlText, Object[] parameters)
			//   at RM.Sau.Utils.SauSeqID.GetId(String codigo, Int32 codColigada)
			//   at RM.Sau.Paciente.SauPacienteData.SavingRow(DataRow row)
			//   at RM.Lib.Server.RMSDataServer.SaveTableRows(DataTable masterTable)
			//   at RM.Lib.Server.RMSDataServer.DoSavingRows(DataSet dataSet)
			//   at RM.Lib.Server.RMSDataServer.InternalSaveRecord(RMSContext context, DataSet& dataSet, Object ownerData, Boolean skipSecurity, Boolean skipSecurityCollumns)
			//   at RM.Lib.Server.RMSDataServer.SaveRecord(RMSContext context, DataSet& dataSet, Object ownerData)
			//   at RM.Con.Conector.ConWSDataServer.SaveRecord(String DataServerName, String XML, String UserName, String UserPassword, String contexto, String emailUsuarioContexto)=======================================
			//The INSERT statement conflicted with the FOREIGN KEY constraint "FKSZSEQID_GCOLIGADA". The conflict occurred in database "RM_SUST_1182", table "dbo.GCOLIGADA", column 'CODCOLIGADA'.
			//The statement has been terminated.
			//=======================================
			//=======================================
			//--SauPacienteData
			//INSERT INTO SZSEQID (CODCOLIGADA, CODIGO, SEQUENCIAL, RECCREATEDBY, RECCREATEDON, RECMODIFIEDBY, RECMODIFIEDON) VALUES (2, 'PSARC001', 1, '', '16/05/2018 17:16:37', '', '16/05/2018 17:16:37')
			//=======================================
		}
	}
}
