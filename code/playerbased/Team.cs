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

	public void SetTeam(SFPlayer player)
	{
		Log.Info( "Called by " + player.Client.Name );
		//curTeam = newTeam;

		//KillFeed.Current?.AddEntry( 0, $"{Client.Name} has joined the {newTeam} team", 0, "", null);

		using ( Prediction.Off() )
		{
			//UpdateTeamClient( To.Single( this ), newTeam );
		}
	}

	[ServerCmd("SF_SetTeam")]
	public static void SetTeamServer(SFTeams newTeam )
	{
		var user = ConsoleSystem.Caller;

		Log.Info( "Called by " + user.Name );
		Log.Info( newTeam );

		//curTeam = newTeam;

		//KillFeed.Current?.AddEntry( 0, $"{Client.Name} has joined the {newTeam} team", 0, "", null);

		using ( Prediction.Off() )
		{
			//UpdateTeamClient( To.Single( this ), newTeam );
		}
	}

	[ClientRpc]
	private void UpdateTeamClient( SFTeams newTeamClient )
	{
		curTeam = newTeamClient;
	}
}
