
using Sandbox;
using Sandbox.UI;
using Sandbox.UI.Construct;
using System.Collections.Generic;
using System.Linq;

public partial class SFScoreboard<T> : Panel where T : SFScoreboardEntry, new()
{
	public Panel Canvas { get; protected set; }
	Dictionary<Client, T> Rows = new();

	public Panel Header { get; protected set; }

	public SFScoreboard()
	{
		StyleSheet.Load( "/ui/SFScoreboard.scss" );
		AddClass( "scoreboard" );

		AddHeader();

		Canvas = Add.Panel( "canvas" );
	}

	public override void Tick()
	{
		base.Tick();

		SetClass( "open", Input.Down( InputButton.Score ) );

		if ( !IsVisible )
			return;

		//
		// Clients that were added
		//
		foreach ( var client in Client.All.Except( Rows.Keys ) )
		{
			var entry = AddClient( client );
			Rows[client] = entry;
		}

		foreach ( var client in Rows.Keys.Except( Client.All ) )
		{
			if ( Rows.TryGetValue( client, out var row ) )
			{
				row?.Delete();
				Rows.Remove( client );
			}
		}
	}


	protected virtual void AddHeader()
	{
		Header = Add.Panel( "header" );
		Header.Add.Label( "Name", "name" );
		Header.Add.Label( "Gifts", "gifts" );
		Header.Add.Label( "Kills", "kills" );
		Header.Add.Label( "Deaths", "deaths" );
		Header.Add.Label( "Ping", "ping" );
	}

	protected virtual T AddClient( Client entry )
	{
		var p = Canvas.AddChild<T>();
		p.Client = entry;
		return p;
	}
}
