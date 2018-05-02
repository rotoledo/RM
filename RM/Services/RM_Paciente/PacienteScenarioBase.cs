using System;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace RM.Services.Paciente
{
	public class PacienteScenarioBase
	{
		public static string apiUrlBase = "poad040100781";
		public static string apiUrl = $"http://{apiUrlBase}/TOTVSBusinessConnect/wsDataServer.asmx";
		public static string contexto = "CODCOLIGADA=2;CODSISTEMA=O;CODUSUARIO=mestre";
		public static string usuario = "mestre";
		public static string senha = "totvs";

		public static string ReadRecordEnvelopeBuilder(ReadRecordEnvelopeBody readRecordEnvelopeBody)
		{
			return $@"<soap:Envelope xmlns:soap=""http://www.w3.org/2003/05/soap-envelope"" xmlns:br=""http://www.totvs.com.br/br/"">
							<soap:Header/>
								<soap:Body>
								<br:ReadRecordAuth>
									<br:DataServerName>{readRecordEnvelopeBody.DataServerName}</br:DataServerName>
									<br:PrimaryKey>{readRecordEnvelopeBody.PrimaryKey}</br:PrimaryKey>
									<br:Contexto>{readRecordEnvelopeBody.Contexto}</br:Contexto>
									<br:Usuario>{readRecordEnvelopeBody.Usuario}</br:Usuario>
									<br:Senha>{readRecordEnvelopeBody.Senha}</br:Senha>
								</br:ReadRecordAuth >
							</soap:Body>
						</soap:Envelope>";
		}

		public string SaveRecordEnvelopeBuilder(string szPacienteXml)
		{
			return $@"<soap:Envelope xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" xmlns:soap=""http://schemas.xmlsoap.org/soap/envelope/"">
						<soap:Body>
							<SaveRecordAuth xmlns=""http://www.totvs.com.br/br/"">
								<DataServerName>sauPacienteData</DataServerName>
								<XML>{szPacienteXml}</XML>
								<Contexto>CODCOLIGADA=2;CODSISTEMA=O;CODUSUARIO=mestre</Contexto>
								<Usuario>mestre</Usuario>
								<Senha>totvs</Senha>
							</SaveRecordAuth>
						</soap:Body>
					</soap:Envelope>";
		}

		public string SZPacienteBuilder(SZPACIENTE sZPACIENTE)
		{
			return $@"<![CDATA[<?xml version=""1.0"" encoding=""UTF-8""?>
<SauPaciente>
   <SZPACIENTE>
      <CODCOLIGADA>2</CODCOLIGADA>
      <CODPACIENTE>0</CODPACIENTE>
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

		public class ReadRecordEnvelopeBody
		{
			public string DataServerName;
			public string PrimaryKey;
			public string Contexto;
			public string Usuario;
			public string Senha;

			public ReadRecordEnvelopeBody(string dataServerName, string primaryKey)
			{
				DataServerName = dataServerName;
				PrimaryKey = primaryKey;
				Contexto = contexto;
				Usuario = usuario;
				Senha = senha;
			}


		}
		public static string GenerateCPF()
		{
			int soma = 0, resto = 0;
			int[] multiplicador1 = new int[9] { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
			int[] multiplicador2 = new int[10] { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };

			Random rnd = new Random();
			string semente = rnd.Next(100000000, 999999999).ToString();

			for (int i = 0; i < 9; i++)
				soma += int.Parse(semente[i].ToString()) * multiplicador1[i];

			resto = soma % 11;
			if (resto < 2)
				resto = 0;
			else
				resto = 11 - resto;

			semente = semente + resto;
			soma = 0;

			for (int i = 0; i < 10; i++)
				soma += int.Parse(semente[i].ToString()) * multiplicador2[i];

			resto = soma % 11;

			if (resto < 2)
				resto = 0;
			else
				resto = 11 - resto;

			semente = semente + resto;
			return semente;
		}
	}

}
