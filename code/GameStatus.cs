using Sandbox;
using Sandbox.UI;
using System.Linq;

public partial class SFGame
{
	public static TimeSince timeToBegin = 0;

	public enum enumStatus
	{
		Idle,
		Start,
		Active,
		Post
	}

	public enumStatus gameStatus = enumStatus.Idle;
	public static bool CanStartGame()
	{
		if ( SFPlayer.GetRedMembers().Count >= 1 && SFPlayer.GetGreenMembers().Count >= 1 )
			return true;

		return false;
	}

	[Event( "SF_StopGame" )]
	public void StopGame()
	{
		if ( IsClient )
			return;

		gameStatus = enumStatus.Idle;

		using ( Prediction.Off() )
			SFChatBox.AddInformation( To.Everyone, "Stopped Game" );
			
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
		if ( IsClient )
			return;

		if ( gameStatus != enumStatus.Idle )
			return;

		gameStatus = enumStatus.Start;

		foreach ( var pawn in Client.All )
		{
			if( pawn is SFPlayer player)
			{

			} else
			{
				Log.Info( "NOT A PLAYER" );
			}

			//pawn.Respawn();
			//player.lockControls = true;
			//Log.Info( "Locking " + player.Client.Name + " controls" );
		}

		using ( Prediction.Off() )
			SFChatBox.AddInformation( To.Everyone, "Starting game" );

	}

	[Event.Tick.Server]
	public void Tick()
	{
		if ( timeToBegin < 10 && gameStatus == enumStatus.Start )
		{
			foreach ( var player in Client.All.OfType<SFPlayer>() )
				player.lockControls = false;

			gameStatus = enumStatus.Active;
		}
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
