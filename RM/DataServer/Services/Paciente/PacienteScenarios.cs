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
			// TODO: Validate Schema instead
			//Assert.Matches(new Regex("^[0-9]+$"), xmlContent.Split(';')[0]);
			//Assert.Matches(new Regex("^[0-9]+$"), xmlContent.Split(';')[1]);
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
