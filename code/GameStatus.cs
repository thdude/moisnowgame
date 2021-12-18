using Sandbox;
using System.Collections.Generic;
using System.Linq;

public partial class SFGame
{
	public static TimeSince timeToBegin = 0;
	public TimeSince timeSinceDrop = 0;

	public enum enumStatus
	{
		Idle,
		Start,
		Active,
		Post
	}

	public static enumStatus gameStatus;
	public static bool CanStartGame()
	{
		if ( SFPlayer.GetRedMembers().Count >= 1 && SFPlayer.GetGreenMembers().Count >= 1 )
			return true;

		return false;
	}

	[Event( "SF_StopGame" )]
	public void StopGame()
	{
		gameStatus = enumStatus.Idle;

		using ( Prediction.Off() )
		{
			SFChatBox.AddInformation( To.Everyone, "Stopped Game" );
			UpdateGameStateClient( To.Everyone, enumStatus.Idle );
		}
	}

	[ServerCmd("SF_AdminStartGame")]
	public static void StartGameCMD()
	{
		Event.Run( "SF_BeginGame" );
	}

	[ServerCmd( "SF_AdminStopGame" )]
	public static void StopGameCMD()
	{
		Event.Run( "SF_StopGame" );
	}

	public static bool ShouldStopGame()
	{
		if ( SFPlayer.GetGreenMembers().Count < 1 || SFPlayer.GetRedMembers().Count < 1 )
			return true;

		return false;
	}

	[Event( "SF_BeginGame" )]
	public void StartGame()
	{
		if ( gameStatus != enumStatus.Idle )
			return;

		gameStatus = enumStatus.Start;

		timeToBegin = 0;

		foreach ( var client in Client.All )
		{
			if( client.Pawn is SFPlayer player)
			{
				player.Respawn();
				player.lockControls = true;
			}
		}

		using ( Prediction.Off() )
		{
			SFChatBox.AddInformation( To.Everyone, "Starting game" );
			UpdateGameStateClient( To.Everyone, enumStatus.Start );
		}
	}

	[Event.Tick.Server]
	public void Tick()
	{
		if ( timeToBegin > 7 && gameStatus == enumStatus.Start )
		{
			foreach ( var client in Client.All )
			{
				if ( client.Pawn is SFPlayer player )
					player.lockControls = false;
			}

			using ( Prediction.Off() )
			{
				UpdateGameStateClient( To.Everyone, enumStatus.Active );
			}
		}

		if ( gameStatus == enumStatus.Active && timeSinceDrop >= Rand.Int( 5, 16 ) )
		{
			List<PresentSpawnpoint> spawnPoints = new();

			foreach ( var point in All.OfType<PresentSpawnpoint>() )
			{
				if( point.IsFromMap )
				{
					spawnPoints.Add( point );
				}
			}

			var presentDrop = new Present();
			presentDrop.Position = spawnPoints[Rand.Int( 0, spawnPoints.Count )].Position;
		}
	}

	[ClientRpc]
	private void UpdateGameStateClient( enumStatus status )
	{
		gameStatus = status;
	}

	public void EndGame()
	{
		if ( IsClient )
			return;

		gameStatus = enumStatus.Post;
	}

	public void DeclareWinner()
	{

	}
}
