using System.Net.Http;
using System.Threading.Tasks;
using static DataServer_Stuff.ScenarioBase;

namespace DataServer_Stuff
{
	public static class IScenarioInterfaceImpl
	{

		public static async Task Read_Paciente_returns_OK_Status_code()
		{
			// GIVEN
			var readRecordEnvelopeBody = new EnvelopeBody("DataServerName", ScenarioBase.Coligada + "Codigo");
			var httpRequestMessage = RequestMessageBuider(readRecordEnvelopeBody, SOAPAction.ReadRecordAuth);

			// WHEN
			var response = await new HttpClient().SendAsync(httpRequestMessage);

			// THEN
			response.EnsureSuccessStatusCode();
		}

	}
}
