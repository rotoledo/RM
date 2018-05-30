using System.Threading.Tasks;

namespace DataServer_Stuff
{
	public interface IDataServerScenarioInterface
	{
		Task Create_DataServerName_returns_OK_Status_code();

		Task Read_DataServerName_returns_OK_Status_code();

		Task Update_DataServerName_returns_OK_Status_code();

		Task Delete_DataServerName_returns_OK_Status_code();

	}
}
