using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RM.Services.RM_Paciente
{
	public class PacienteScenarioBase
	{
		public static string apiUrlBase = "poad040100781";
		public static string apiUrl = $"http://{apiUrlBase}/TOTVSBusinessConnect/wsDataServer.asmx";
		public static string contexto = "CODCOLIGADA=2;CODSISTEMA=O;CODUSUARIO=mestre";
		public static string usuario = "mestre";
		public static string senha = "totvs";
		
		public static string EnvelopeBuilder(ReadRecordEnvelopeBody readRecordEnvelopeBody)
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
	}
}
