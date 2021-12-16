using Sandbox;

partial class SFPlayer
{
	public enum SFTeams
	{
		Unspecified,
		Green, //Grinch team
		Red //Santa team
	}

	public SFTeams curTeam;

	public void SetTeam(SFTeams newTeam)
	{
		curTeam = newTeam;

		using (Prediction.Off())
			UpdateTeamClient(To.Single(this), newTeam);
	}

	[ClientRpc]
	private void UpdateTeamClient( SFTeams newTeamClient )
	{
		curTeam = newTeamClient;
	}
}
