using Xunit;
using System;
using RM_Stuff;
using System.Net;
using System.Text;
using System.Net.Http;
using DataServer_Stuff;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Linq;
using System.IO;

namespace IntegrationTests.DataServer.Services.Paciente
{
	public class PacienteScenarios : PacienteScenarioBase
	{

		[Fact]
		public async Task Read_Paciente_returns_OK_Status_code()
		{
			// GIVEN
			var readRecordEnvelopeBody = ReadRecordSZPacienteEnvelopeBody(ScenarioBase.Coligada, CodPaciente);
			var httpRequestMessage = SZPacienteBuilder(readRecordEnvelopeBody, SOAPAction.ReadRecordAuth);

			// WHEN
			var response = await new HttpClient().SendAsync(httpRequestMessage);

			// THEN
			response.EnsureSuccessStatusCode();
			//string xmlContent = Methods.GetInnerTextFromResponseBySoapAction(response, SOAPAction.ReadRecordAuth);
			//Assert.Contains("<SZPACIENTE>", xmlContent);
			//Assert.Contains("</SZPACIENTE>", xmlContent);
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
			var szPacienteXml = SZPacienteXmlBuilder(new SZPACIENTE() { CODPACIENTE = CodPaciente });
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
			var szPacienteXml = SZPacienteXmlBuilder(new SZPACIENTE() { CODPACIENTE = CodPaciente });
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
			response.EnsureSuccessStatusCode();
			string xmlContent = Methods.GetInnerTextFromResponseBySoapAction(response, SOAPAction.SaveRecordAuth);
			Assert.Matches(new Regex("^[0-9]+$"), xmlContent.Split(';')[0]);
			Assert.Matches(new Regex("^[0-9]+$"), xmlContent.Split(';')[1]);

			// < SaveRecordAuthResult > Violação de chave estrangeira Possíveis causas: -exclusão de registro que possui outros registros associados - inclusão de registro detalhe sem um registro mestre associado 
			// ======================================= 
			// at RM.Lib.Data.DbServices.QueryExec(String sqlText, Object[] parameters) at RM.Sau.Utils.SauSeqID.GetId(String codigo, Int32 codColigada) at RM.Sau.Paciente.SauPacienteData.SavingRow(DataRow row) at RM.Lib.Server.RMSDataServer.SaveTableRows(DataTable masterTable) at RM.Lib.Server.RMSDataServer.DoSavingRows(DataSet dataSet) at RM.Lib.Server.RMSDataServer.InternalSaveRecord(RMSContext context, DataSet & dataSet, Object ownerData, Boolean skipSecurity, Boolean skipSecurityCollumns) at RM.Lib.Server.RMSDataServer.SaveRecord(RMSContext context, DataSet & dataSet, Object ownerData) at RM.Con.Conector.ConWSDataServer.SaveRecord(String DataServerName, String XML, String UserName, String UserPassword, String contexto, String emailUsuarioContexto)
			//  ======================================= 
			// The INSERT statement conflicted with the FOREIGN KEY constraint "FKSZSEQID_GCOLIGADA".The conflict occurred in database "RM_SUST_1182", table "dbo.GCOLIGADA", column 'CODCOLIGADA'.The statement has been terminated. 
			// ======================================= ======================================= 
			// --SauPacienteData INSERT INTO SZSEQID(CODCOLIGADA, CODIGO, SEQUENCIAL, RECCREATEDBY, RECCREATEDON, RECMODIFIEDBY, RECMODIFIEDON) VALUES(2, 'PSARC001', 1, '', '14/05/2018 12:02:03', '', '14/05/2018 12:02:03') ======================================= </ SaveRecordAuthResult >
		}

		[Fact]
		public async Task ReadRecordAuth_SchemaValidation()
		{
			// GIVEN
			var readRecordEnvelopeBody = ReadRecordSZPacienteEnvelopeBody(ScenarioBase.Coligada, CodPaciente);
			var httpRequestMessage = SZPacienteBuilder(readRecordEnvelopeBody, SOAPAction.ReadRecordAuth);

			// WHEN
			var response = await new HttpClient().SendAsync(httpRequestMessage);

			// THEN
			response.EnsureSuccessStatusCode();
			var response_content = response.Content.ReadAsStringAsync().Result;


			var path = new Uri(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().CodeBase)).LocalPath;
			XmlSchemaSet schema = new XmlSchemaSet();
			//schema.Add("", @"C:\Users\roberto.firmino\AppData\Local\Temp\ResponseContent.xsd");
			//schema.Add(@"http://www.w3.org/2003/05/soap-envelope", @"C:\Users\roberto.firmino\AppData\Local\Temp\ResponseContent.xsd");
			//schema.Add(@"http://schemas.xmlsoap.org/ws/2004/08/addressing", @"C:\Users\roberto.firmino\AppData\Local\Temp\ResponseContent1.xsd");
			//schema.Add(@"http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd", @"C:\Users\roberto.firmino\AppData\Local\Temp\ResponseContent2.xsd");
			schema.Add(@"http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-utility-1.0.xsd", @"C:\Users\roberto.firmino\AppData\Local\Temp\ResponseContent3.xsd");
			// 

			XmlReader reader = XmlReader.Create(@"C:\Users\roberto.firmino\Documents\Visual Studio 2017\Projects\RM\RM\DataServer\Services\Paciente\ResponseContent.xml");
			XDocument document = XDocument.Load(reader);
			document.Validate(schema, ValidationEventHandler);


		}

		static void ValidationEventHandler(object sender, ValidationEventArgs e)
		{
			XmlSeverityType type = XmlSeverityType.Warning;
			if (Enum.TryParse<XmlSeverityType>("Error", out type))
			{
				if (type == XmlSeverityType.Error) throw new Exception(e.Message);
			}
		}






		//string XML_response = @"C:\Users\roberto.firmino\Desktop\ResponseContent.xml";
		//string XML_response_Schema = @"C:\Users\roberto.firmino\Desktop\ResponseContent.xsd";

		//XmlReaderSettings booksSettings = new XmlReaderSettings();
		//booksSettings.Schemas.Add("http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-utility-1.0.xsd", XML_response_Schema);
		//	booksSettings.ValidationType = ValidationType.Schema;

		//	XmlDocument document = new XmlDocument();
		//document.LoadXml(response_content);

		//	XmlReader reader = XmlReader.Create(XML_response, booksSettings);
		//document.Load(reader);

		//	ValidationEventHandler eventHandler = new ValidationEventHandler(ValidationEventHandler);
		//document.Validate(eventHandler);

		//static void ValidationEventHandler(object sender, ValidationEventArgs e)
		//{
		//	switch (e.Severity)
		//	{
		//		case XmlSeverityType.Error:
		//			Console.WriteLine("Error: {0}", e.Message);
		//			break;
		//		case XmlSeverityType.Warning:
		//			Console.WriteLine("Warning {0}", e.Message);
		//			break;
		//	}

		//}



	}
}
