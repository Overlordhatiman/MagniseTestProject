using MagniseTask.Data;

namespace MagniseTask.Interfaces;

public interface IFintachartsAuthService
{
	Task<AuthData> AuthenticateAsync();
	
}