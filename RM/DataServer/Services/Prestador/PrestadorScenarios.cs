using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using DataServer_Stuff;
using Xunit;

namespace RM.DataServer.Services.Prestador
{
	public class PrestadorScenarios : ScenarioBase, IDataServerScenarioInterface
	{
		[Fact]
		public async Task Create_DataServerName_returns_OK_Status_code()
		{
			// GIVEN
			var SZCadGeral = SZPrestadorXmlBuilder();
			var readRecordEnvelopeBody = new EnvelopeBody("SauCadGeralData", SZCadGeral);
			var httpRequestMessage = RequestMessageBuider(readRecordEnvelopeBody, SOAPAction.SaveRecordAuth);

			// WHEN
			var response = await new HttpClient().SendAsync(httpRequestMessage);

			// THEN
			response.EnsureSuccessStatusCode();
		}

		public string SZPrestadorXmlBuilder()
		{
			var file = $@"<![CDATA[{File.ReadAllText(@".\Resources\SZCadGeral.xml")}]]>";
			return file;
		}

		[Fact]
		public Task Delete_DataServerName_returns_OK_Status_code()
		{
			throw new NotImplementedException();
		}

		[Fact]
		public async Task Read_DataServerName_returns_OK_Status_code()
		{
			// GIVEN
			var readRecordEnvelopeBody = new EnvelopeBody("SauCadGeralData", "2;2");
			var httpRequestMessage = RequestMessageBuider(readRecordEnvelopeBody, SOAPAction.ReadRecordAuth);

			// WHEN
			var response = await new HttpClient().SendAsync(httpRequestMessage);

			// THEN
			response.EnsureSuccessStatusCode();
		}

		[Fact]
		public Task Update_DataServerName_returns_OK_Status_code()
		{
			throw new NotImplementedException();
		}
	}
}
