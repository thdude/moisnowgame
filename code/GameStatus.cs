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

	public static enumStatus gameStatus = enumStatus.Idle;
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

		timeToBegin = 0;

		foreach ( var client in Client.All )
		{
			if( client.Pawn is SFPlayer player)
			{
				player.Respawn();
				player.lockControls = true;
				Log.Info( "Locking " + player.Client.Name + " controls" );
			}
		}

		using ( Prediction.Off() )
			SFChatBox.AddInformation( To.Everyone, "Starting game" );

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
