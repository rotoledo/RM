using RM_Stuff;
using System.Net.Http;
using DataServer_Stuff;
using System.IO;

namespace IntegrationTests.DataServer.Services.Paciente
{
	public class PacienteScenarioBase : ScenarioBase
	{

		public static string CodPaciente = "98962";

		public string SZPacienteXmlBuilder(SZPACIENTE sZPACIENTE)
		{
			var file = $@"<![CDATA[{File.ReadAllText(@".\Resources\SZPaciente.xml")}]]>";
			file = file.Replace("sZPACIENTE.CODPACIENTE", sZPACIENTE.CODPACIENTE)
				.Replace("sZPACIENTE.NOMEPACIENTE", sZPACIENTE.NOMEPACIENTE)
				.Replace("sZPACIENTE.CPF", sZPACIENTE.CPF);
			return file;
		}


		public EnvelopeBody ReadRecordSZPacienteEnvelopeBody(string coligada, string codPaciente)
		{
			return new EnvelopeBody("SauPacienteData", coligada + ";" + codPaciente);
		}

		public EnvelopeBody SaveRecordSZPacienteEnvelopeBody(string xml)
		{
			return new EnvelopeBody("SauPacienteData", xml);
		}

		public HttpRequestMessage SZPacienteBuilder(EnvelopeBody readRecordEnvelopeBody, string soapAction)
		{
			return RequestMessageBuider(readRecordEnvelopeBody, soapAction);
		}

		public SZPACIENTE ExtractPacienteFromReadRecordResponse(HttpResponseMessage response)
		{
			var response_content = response.Content.ReadAsStringAsync().Result;
			string xmlContent = Methods.GetTextbySOAPActionAndTagName(response_content, "ReadRecordAuth", "SZPACIENTE");
			var dbPaciente = (SZPACIENTE)Methods.DeserializeXMLByType<SZPACIENTE>(xmlContent);
			return dbPaciente;
		}
	}

}
