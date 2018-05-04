using System.Net.Http;

namespace DataServer.Services.Paciente
{
	public class PacienteScenarioBase : ScenarioBase
	{

		public string SZPacienteXmlBuilder(SZPACIENTE sZPACIENTE)
		{
			return $@"<![CDATA[<?xml version=""1.0"" encoding=""UTF-8""?>
						<SauPaciente>
						   <SZPACIENTE>
							  <CODCOLIGADA>2</CODCOLIGADA>
							  <CODPACIENTE>{sZPACIENTE.CODPACIENTE}</CODPACIENTE>
							  <NOMEPACIENTE>{sZPACIENTE.NOMEPACIENTE}</NOMEPACIENTE>
							  <SEXO>M</SEXO>
							  <DATANASC>1998-01-01T00:00:00</DATANASC>
							  <NOMEPAI>PAI TESTE 3</NOMEPAI>
							  <NOMEMAE>MAE TESTE 3</NOMEMAE>
							  <NATURALIDADE>PORTO ALEGRE</NATURALIDADE>
							  <UFNATURALIDADE>RS</UFNATURALIDADE>
							  <CODNACIONALIDADE>1</CODNACIONALIDADE>
							  <PROFISSAO>ANALISTA DE TESTES</PROFISSAO>
							  <CODGRAUINSTRUCAO>8</CODGRAUINSTRUCAO>
							  <IDENTIDADE>987654312</IDENTIDADE>
							  <ORGAOEMISSOR>SSP</ORGAOEMISSOR>
							  <UFIDENTIDADE>RS</UFIDENTIDADE>
							  <CPF>{sZPACIENTE.CPF}</CPF>
							  <CODESTADOCIVIL>1</CODESTADOCIVIL>
							  <CEP>90000000</CEP>
							  <ENDERECO>TESTEAUTO</ENDERECO>
							  <NUMERO>9999</NUMERO>
							  <COMPLEMENTO>AP 1</COMPLEMENTO>
							  <BAIRRO>BAIRRO TESTE</BAIRRO>
							  <UFENDERECO>RS</UFENDERECO>
							  <DDDTELRES>51</DDDTELRES>
							  <TELRES>33221144</TELRES>
							  <EMAIL>PATRICKTESTEAUTO@TOTVS.COM.BR</EMAIL>
							  <PISPASEP>62306898403</PISPASEP>
							  <CODRACACOR>2</CODRACACOR>
							  <CLIENTEVIP>F</CLIENTEVIP>
							  <DATAEMISIDENTIDADE>2009-01-20T00:00:00</DATAEMISIDENTIDADE>
							  <TPCERTIDAO>2</TPCERTIDAO>
							  <NUMZONAELEIT>2</NUMZONAELEIT>
							  <NUMSECELEIT>1</NUMSECELEIT>
							  <NUMTITULOELEIT>381520780418</NUMTITULOELEIT>
							  <OBSERVACAO>TESTE DE AUTOMAÇÃO COM API - NÃO ALTERAR DADOS DESSE PACIENTE</OBSERVACAO>
							  <RECCREATEDBY>mestre</RECCREATEDBY>
							  <RECCREATEDON>2018-02-27T17:06:46</RECCREATEDON>
							  <RECMODIFIEDBY>mestre</RECMODIFIEDBY>
							  <RECMODIFIEDON>2018-02-27T17:06:46</RECMODIFIEDON>
							  <CODMUNICIPIO>14902</CODMUNICIPIO>
							  <CODLOGRADOURO>1</CODLOGRADOURO>
							  <TIPOBAIRRO>1</TIPOBAIRRO>
							  <IBGENASC>0</IBGENASC>
							  <NOMESOCIAL>AUTOMACAOSOAP</NOMESOCIAL>
						   </SZPACIENTE>
						</SauPaciente>]]>";
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
