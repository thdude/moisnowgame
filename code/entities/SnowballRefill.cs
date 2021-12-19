using Sandbox;
using System;

[Library( "ent_sf_snowpile" )]
[Hammer.EditorModel( "models/citizen_props/crate01.vmdl_c" )]
[Hammer.EntityTool( "Snow pile", "Snow-Fight", "A pile of snowballs" )]
partial class SnowballRefill : Prop, IUse
{
	[Flags]
	public enum Flags
	{
		Green = 1,
		Red = 2,
		All = 3
	}

	private TimeSince lastUse = 0;

	[Property( "teamEnum", Title = "Which team can refill their ammo" )]
	public Flags TeamEnum { get; set; } = Flags.All;

	public override void Spawn()
	{
		base.Spawn();
		SetModel("models/snowpile.vmdl");

		GlowState = GlowStates.On;
		GlowDistanceStart = 0;
		GlowDistanceEnd = 500;
		GlowColor = new Color(150, 150, 150, 1);

	}

	public bool IsUsable( Entity user )
	{
		return true;
	}

	public bool OnUse( Entity user )
	{
		if (user is SFPlayer player)
		{
			if ( lastUse < 0.5 )
				return false;

			lastUse = 0;

			if ( player.curTeam.ToString() != TeamEnum.ToString() && TeamEnum != Flags.All )
				return false;

			if ( player.ActiveChild is WeaponBase weapon )
			{	
				if ( weapon.ToString().Contains("Snowball") && weapon.AmmoClip < weapon.MaxAmmoClip )
				{
					weapon.AmmoClip += 5;

					if ( weapon.AmmoClip > weapon.MaxAmmoClip )
					{
						weapon.AmmoClip = weapon.MaxAmmoClip;
						Sound.FromEntity( "power_pickup", this );
						return true;
					}

					Sound.FromEntity( "power_pickup", this);
					return true;
				}
				else
					return false;

			} else
				return false;
		}

		return false;
	}
}

