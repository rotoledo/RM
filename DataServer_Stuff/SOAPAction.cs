namespace DataServer_Stuff
{
	public abstract class SOAPAction
	{
		// Autentica o usuário no ambiente RM. O usuário e senha devem ser informados via SOAP, criando um Token para isto.
		public static string AutenticaAcesso = "AutenticaAcesso";

		// Autentica o usuário no ambiente RM. O usuário e senha devem ser informados por parâmetro.
		public static string AutenticaAcessoAuth = "AutenticaAcessoAuth";

		// Exclui um registro no Datataserver (regra de negócio RM). Este método somente irá exclui o registro se este for válido de acordo com as regras de negócio definidas pelo DataServer.
		public static string DeleteRecord = "DeleteRecord";

		public static string DeleteRecordAuth = "DeleteRecordAuth";
		// Exclui um registro no Datataserver (regra de negócio RM). Este método somente irá exclui o registro se este for válido de acordo com as regras de negócio definidas pelo DataServer.

		// Retorna o esquema (XSD) do DataServer (regra de negócio RM) passado como parâmetro.
		public static string GetSchema = "GetSchema";

		// Retorna o esquema (XSD) do DataServer (regra de negócio RM) passado como parâmetro.
		public static string GetSchemaAuth = "GetSchemaAuth";

		// Faz a leitura do DataServer (regra de negócio RM) e retorna o registro específico (XML), conforme chave primária informada nos parâmetros.
		public static string ReadRecord = "ReadRecord";

		// Faz a leitura do DataServer (regra de negócio RM) e retorna o registro específico (XML), conforme chave primária informada nos parâmetros.
		public static string ReadRecordAuth = "ReadRecordAuth";

		// Faz a leitura do DataServer (regra de negócio RM) e retorna conjunto de registros (XML), conforme filtros informados nos parâmetros.
		public static string ReadView = "ReadView";

		// Faz a leitura do DataServer (regra de negócio RM) e retorna conjunto de registros (XML), conforme filtros informados nos parâmetros.
		public static string ReadViewAuth = "ReadViewAuth";

		// Insere um registro no Datataserver (regra de negócio RM). Este método somente irá incluir o registro se este for válido de acordo com as regras de negócio definidas pelo DataServer.
		public static string SaveRecord = "SaveRecord";

		// Insere um registro no Datataserver (regra de negócio RM). Este método somente irá incluir o registro se este for válido de acordo com as regras de negócio definidas pelo DataServer.
		public static string SaveRecordAuth = "SaveRecordAuth";
	}
}
