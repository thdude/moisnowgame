using Sandbox;
using Sandbox.UI;
using Sandbox.UI.Construct;
using System.Collections.Generic;
using System.Linq;

partial class SFPlayer
{
	public enum SFTeams
	{
		Unspecified,
		Green, //Grinch team
		Red //Santa team
	}

	public SFTeams curTeam;

	//Sets team serverside
	public void SetTeam(SFPlayer player, SFTeams newTeam )
	{
		//Checks if the player can join that team, if not stop here
		if ( (newTeam == SFTeams.Red && GetRedMembers().Count <= GetGreenMembers().Count)
				|| (newTeam == SFTeams.Green && GetGreenMembers().Count <= GetRedMembers().Count) )	
			curTeam = newTeam;
		else
			return;
		
		//Turn prediction off so that we can update the team on the client
		using ( Prediction.Off() )
		{
			UpdateTeamClient( To.Single( this ), newTeam );
		}
	}

	//Client to Server command, just so we can grab the team selection menu result
	//on who picked which team
	[ServerCmd("SF_SetTeam")]
	public static void SetTeamClientToServer( SFTeams newTeam )
	{
		var user = ConsoleSystem.Caller;

		if ( user.Pawn is SFPlayer player )
		{
			//If already assigned to the team, stop here
			if ( player.curTeam == newTeam )
				return;

			player.SetTeam( player, newTeam );
		}

		ChatBox.AddInformation( To.Everyone, $"{user.Name} has joined the {newTeam} team", $"avatar:{user.PlayerId}" );
	}

	//Gets each member for this team
	public List<SFPlayer> GetGreenMembers()
	{
		List<SFPlayer> curGreenMembers = new();

		foreach ( var client in Client.All )
		{
			if ( client.Pawn is SFPlayer player )
			{
				if ( player.curTeam == SFTeams.Green )
					curGreenMembers.Add( player );
			}
		}

		return curGreenMembers;
	}

	//Same as above but for the red team
	public List<SFPlayer> GetRedMembers()
	{
		List<SFPlayer> curRedMembers = new();

		foreach ( var client in Client.All )
		{
			if( client.Pawn is SFPlayer player )
			{
				if (player.curTeam == SFTeams.Red)
				curRedMembers.Add( player );
			}	
		}

		return curRedMembers;
	}

	//Updates clientside teams
	[ClientRpc]
	private void UpdateTeamClient( SFTeams newTeamClient )
	{
		curTeam = newTeamClient;
	}
}
