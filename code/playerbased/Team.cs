using Sandbox;
using Sandbox.UI;
using Sandbox.UI.Construct;

partial class SFPlayer
{
	public enum SFTeams
	{
		Unspecified,
		Green, //Grinch team
		Red //Santa team
	}

	public SFTeams curTeam;

	[Event( "SF_ChangeTeam" )]
	public void SetTeam(SFTeams newTeam)
	{
		curTeam = newTeam;

		KillFeed.Current?.AddEntry( 0, $"{Client.Name} has joined the {newTeam} team", 0, "", null);

		using ( Prediction.Off() )
		{
			UpdateTeamClient( To.Single( this ), newTeam );
		}
	}

	[ClientRpc]
	private void UpdateTeamClient( SFTeams newTeamClient )
	{
		curTeam = newTeamClient;
	}
}
