using Sandbox;
using System.Collections.Generic;
using System.Linq;

public partial class SFGame
{
	public static TimeSince timeToBegin;
	public static TimeSince timeToEnd;
	public TimeSince timeSinceDrop;
	public TimeSince timeSinceDropPower;

	public static bool redWinner { get; set; } = false;

	public static bool greenWinner { get; set; } = false;

	private int randomTime = 5;
	private int randomTimePower = 20;

	public static int winGoal { get; set; } = 30;

	public static int redTotalGifts { get; set; } = 0;

	public static int greenTotalGifts { get; set; } = 0;

	List<PresentSpawnpoint> spawnPoints = new();

	public enum enumStatus
	{
		Idle,
		Start,
		Active,
		Post
	}

	public static enumStatus gameStatus;

	//Checks if we can we start the game
	public static bool CanStartGame()
	{
		if ( SFPlayer.GetRedMembers().Count >= 1 && SFPlayer.GetGreenMembers().Count >= 1 )
			return true;

		return false;
	}

	//Stops the game
	[Event( "SF_StopGame" )]
	public void StopGame()
	{
		//If the game is already on idle, stop here
		if ( gameStatus == enumStatus.Idle )
			return;

		gameStatus = enumStatus.Idle;

		foreach ( var client in Client.All )
		{
			if ( client.Pawn is SFPlayer player )
			{
				player.lockControls = false;
				player.musicPlayer.StopMusic();
			}
		}

		foreach ( var oldGift in All.OfType<Present>() )
			oldGift.Delete();

		redTotalGifts = 0;
		greenTotalGifts = 0;

		using ( Prediction.Off() )
		{
			SFChatBox.AddInformation( To.Everyone, "Stopped Game" );
			UpdateGameStateClient( To.Everyone, enumStatus.Idle );

			UpdateTeamScoreClient( To.Everyone, SFPlayer.SFTeams.Red, redTotalGifts );
			UpdateTeamScoreClient( To.Everyone, SFPlayer.SFTeams.Green, greenTotalGifts );
		}
	}

	//Admin commands for starting and stopping the game
	[ConCmd.Server("SF_AdminStartGame")]
	public static void StartGameCMD()
	{
		Event.Run( "SF_BeginGame" );
	}

	[ConCmd.Server( "SF_AdminStopGame" )]
	public static void StopGameCMD()
	{
		Event.Run( "SF_StopGame" );
	}

	//Bool to check if the game should stop
	public static bool ShouldStopGame()
	{
		if ( SFPlayer.GetGreenMembers().Count < 1 || SFPlayer.GetRedMembers().Count < 1 )
			return true;

		return false;
	}


	[Event( "SF_BeginGame" )]
	public void StartGame()
	{
		//If the status is not idle, stop here
		//since we don't want to end up restarting an active game
		//unless necessary
		if ( gameStatus != enumStatus.Idle )
			return;

		redTotalGifts = 0;
		greenTotalGifts = 0;

		gameStatus = enumStatus.Start;

		timeToBegin = 0;

		foreach ( var client in Client.All )
		{
			if( client.Pawn is SFPlayer player)
			{
				player.Respawn();
				player.lockControls = true;

				player.musicPlayer.StartMusic( "halls" );
			}
		}

		foreach ( var oldGift in All.OfType<Present>() )
			oldGift.Delete();

		foreach ( var point in All.OfType<PresentSpawnpoint>() )
		{
			if ( point.IsFromMap )
			{
				spawnPoints.Add( point );
			}
		}

		using ( Prediction.Off() )
		{
			SFChatBox.AddInformation( To.Everyone, "Starting game" );
			UpdateGameStateClient( To.Everyone, gameStatus );

			SetTeamScoreClient( To.Everyone, SFPlayer.SFTeams.Red, 0 );
			SetTeamScoreClient( To.Everyone, SFPlayer.SFTeams.Green, 0 );
		}
	}

	[Event.Tick.Server]
	public void ServerTick()
	{
		//Begins the game after a certain time
		if ( gameStatus == enumStatus.Start && timeToBegin > 7 )
		{
			foreach ( var client in Client.All )
			{
				if ( client.Pawn is SFPlayer player )
					player.lockControls = false;
			}

			gameStatus = enumStatus.Active;

			using ( Prediction.Off() )
				UpdateGameStateClient( To.Everyone, gameStatus );
		
		}

		//Drop random present at given random time
		if ( gameStatus == enumStatus.Active && timeSinceDrop >= randomTime )
		{
			timeSinceDrop = 0;
			randomTime = Rand.Int( 3, 10 );

			var presentDrop = new Present();
			presentDrop.Position = spawnPoints[Rand.Int( 0, spawnPoints.Count - 1 )].Position;
		}

		//Spawns a powerup
		if ( gameStatus == enumStatus.Active && timeSinceDropPower >= randomTimePower )
		{
			timeSinceDropPower = 0;
			randomTimePower = Rand.Int( 5, 25 );

			switch ( Rand.Int( 1, 2 ) )
			{
				case 1:
					var powerUpDrop = new Coffee();
					powerUpDrop.Position = spawnPoints[Rand.Int( 0, spawnPoints.Count - 1 )].Position;
					break;

				case 2:
					//2nd powerup?
					break;
			}
		}

		if ( gameStatus == enumStatus.Post && timeToEnd >= 10)
		{
			gameStatus = enumStatus.Start;

			foreach ( var client in Client.All )
			{
				if ( client.Pawn is SFPlayer player )
					player.Respawn();
			}

			using ( Prediction.Off() )
			{
				SetTeamScoreClient( To.Everyone, SFPlayer.SFTeams.Red, 0 );
				SetTeamScoreClient( To.Everyone, SFPlayer.SFTeams.Green, 0 );
			}
		}
	}

	[ClientRpc]
	private void UpdateGameStateClient( enumStatus status )
	{
		gameStatus = status;
	}

	//Ends the game
	[Event( "SF_EndGame" )]
	public void EndGame()
	{
		redTotalGifts = 0;
		greenTotalGifts = 0;

		timeToEnd = 0;
		gameStatus = enumStatus.Post;

		using ( Prediction.Off() )
			UpdateGameStateClient( To.Everyone, gameStatus );
		foreach ( var oldGift in All.OfType<Present>() )
			oldGift.Delete();
		
		foreach ( var oldPower in All.OfType<PowerBase>() )
			oldPower.Delete();
	}

	public static void DeclareWinner(SFPlayer.SFTeams winningTeam)
	{
		Event.Run( "SF_EndGame" );

		if ( winningTeam == SFPlayer.SFTeams.Red )
			using ( Prediction.Off() )
				SetWinnerClient( To.Everyone, true, false );

		else if ( winningTeam == SFPlayer.SFTeams.Green )
			using ( Prediction.Off() )
				SetWinnerClient( To.Everyone, false, true );

		foreach ( var client in Client.All )
		{
			if ( client.Pawn is SFPlayer player )
			{
				player.lockControls = true;
			}
		}
	}

	[ClientRpc]
	public static void SetWinnerClient( bool redWon, bool greenWon )
	{
		redWinner = redWon;
		greenWinner = greenWon;

		foreach(var client in Client.All)
		{
			if( client.Pawn is SFPlayer player )
			{
				if ( player.curTeam == SFPlayer.SFTeams.Red && redWinner )
					Sound.FromScreen( "winner.sound" );
				else if( player.curTeam == SFPlayer.SFTeams.Red && !redWinner )
					Sound.FromScreen( "loser.sound" );

				if ( player.curTeam == SFPlayer.SFTeams.Green && greenWinner )
					Sound.FromScreen( "winner.sound" );
				else if ( player.curTeam == SFPlayer.SFTeams.Green && !greenWinner )
					Sound.FromScreen( "loser.sound" );
			}
		}

	}

	[ClientRpc]
	public static void SetTeamScoreClient( SFPlayer.SFTeams teamUpdate, int amount )
	{
		if ( teamUpdate == SFPlayer.SFTeams.Red )
			redTotalGifts = amount;
		else if ( teamUpdate == SFPlayer.SFTeams.Green )
			greenTotalGifts = amount;
		else
		{
			redTotalGifts = amount;
			greenTotalGifts = amount;
		}
	}

	[ClientRpc]
	public static void UpdateTeamScoreClient( SFPlayer.SFTeams teamUpdate, int newAmount )
	{
		if ( teamUpdate == SFPlayer.SFTeams.Red )
			redTotalGifts += newAmount;
		else if ( teamUpdate == SFPlayer.SFTeams.Green )
			greenTotalGifts += newAmount;
	}
}
