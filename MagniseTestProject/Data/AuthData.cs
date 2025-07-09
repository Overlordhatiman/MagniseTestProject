namespace MagniseTask.Data;

public class AuthData
{
	
	public string AccessToken { get; set; }
	public int ExpiresIn { get; set; } 
	public int RefreshExpiresIn { get; set; } 
	public string RefreshToken { get; set; }
	public string TokenType { get; set; }
	public int NotBeforePolicy { get; set; }
	public string SessionState { get; set; }
	public string Scope { get; set; }

	public DateTime AccessTokenExpiresAt => DateTime.UtcNow.AddSeconds(ExpiresIn);
	public DateTime RefreshTokenExpiresAt => DateTime.UtcNow.AddSeconds(RefreshExpiresIn);

	public bool IsAccessTokenExpired()
	{
		return DateTime.UtcNow >= AccessTokenExpiresAt;
	}

	public bool IsRefreshTokenExpired()
	{
		return DateTime.UtcNow >= RefreshTokenExpiresAt;
	}
}