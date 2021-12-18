using Sandbox;
using Sandbox.UI;
using System;
using System.Collections.Generic;

partial class SFPlayer
{
	public enum SFTeams
	{
		Unspecified,
		Green, //Grinch team
		Red //Santa team
	}

	public SFTeams curTeam;

	private TimeSince timeSinceSwitchTeam;

	//Sets team serverside
	public void SetTeam(SFPlayer player, SFTeams newTeam )
	{
		//If already assigned to the team, stop here
		if ( player.curTeam == newTeam )
		{
			using ( Prediction.Off() )
				SFChatBox.AddInformation( To.Single( this ), $"You are already on the {newTeam} team" );

			return;
		}

		//Checks if the player can join that team, if not stop here
		if ( (newTeam == SFTeams.Red && GetRedMembers().Count > GetGreenMembers().Count)
			|| (newTeam == SFTeams.Green && GetGreenMembers().Count > GetRedMembers().Count) )
		{
			using ( Prediction.Off() )
				SFChatBox.AddInformation( To.Single( this ), $"You can't pick the {newTeam} team at this time, select a different one" );

			return;
		}
		//If 30 seconds hasn't passed since last team swap, stop here
		if ( timeSinceSwitchTeam < 30.0f )
		{
			using ( Prediction.Off() )
				SFChatBox.AddInformation( To.Single(this), $"Wait {MathF.Round(30.0f - timeSinceSwitchTeam, 1)}s before switching" );
			
			return;
		}

		curTeam = newTeam;

		//Turn prediction off so that we can update the team on the client
		using ( Prediction.Off() )
		{
			UpdateTeamClient( To.Single( this ), newTeam );
			SFChatBox.AddInformation( To.Everyone, $"{player.Client.Name} has joined the {newTeam} team", $"avatar:{player.Client.PlayerId}" );
		}

		timeSinceSwitchTeam = 0.0f;
		OnKilled();

		if(SFGame.CanStartGame())
			Event.Run( "SF_BeginGame" );

	}

	//Client to Server command, just so we can grab the team selection menu result
	//on who picked which team
	[ServerCmd("SF_SetTeam")]
	public static void SetTeamClientToServer( SFTeams newTeam )
	{
		var user = ConsoleSystem.Caller;

		if ( user.Pawn is SFPlayer player )
			player.SetTeam( player, newTeam );
	}

	//Gets each member for this team
	public static List<SFPlayer> GetGreenMembers()
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
	public static List<SFPlayer> GetRedMembers()
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
