using Sandbox;

[Library( "SnowFight" )]
public partial class SFGame : Sandbox.Game
{
	private SFHud hud;

	public SFGame()
	{
		if ( IsServer )
		{

		}

		if ( IsClient )
		{
			hud = new SFHud();
		}
	}

	[Event.Hotload]
	public void UpdateHud()
	{
		if ( !IsClient )
			return;

		hud?.Delete();
		hud = new SFHud();
	}

	public override void ClientJoined( Client client )
	{
		base.ClientJoined( client );

		var player = new SFPlayer();
		client.Pawn = player;

		player.InitialSpawn();
	}

	public override void ClientDisconnect( Client cl, NetworkDisconnectionReason reason )
	{
		base.ClientDisconnect( cl, reason );

		if( ShouldStopGame() )
			StopGame();
	}
}
