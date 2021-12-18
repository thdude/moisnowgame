using Sandbox;
using System;

[Library( "ent_sf_depositbox" )]
[Hammer.EditorModel( "models/citizen_props/crate01.vmdl_c" )]
[Hammer.EntityTool( "Deposit Box", "Snow-Fight", "A box to deposit presents" )]
partial class DepositBox : Prop, IUse
{
	[Flags]
	public enum Flags
	{
		Green = 1,
		Red = 2
	}

	[Property( "teamEnum", Title = "Which team can deposit the presents in" )]
	public Flags TeamEnum { get; set; }

	public override void Spawn()
	{
		base.Spawn();
		SetModel( "models/citizen_props/crate01.vmdl_c" );
	}

	public bool IsUsable( Entity user )
	{
		if ( user is SFPlayer player )
		{
			if ( player.curPresents > 0 )
				return true;
		}

		return false;
	}

	public bool OnUse( Entity user )
	{
		if ( user is SFPlayer player )
		{
			if ( player.curTeam.ToString() != TeamEnum.ToString() )
				return false;

			if ( player.curPresents > 0 )
			{
				player.SetPresent( 0 );
			}
				
		}

		return true;
	}
}

