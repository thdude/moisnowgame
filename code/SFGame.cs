
using Sandbox;
using Sandbox.UI.Construct;
using System;
using System.IO;
using System.Threading.Tasks;

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
}
