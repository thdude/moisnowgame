using Sandbox;
using System;

[Library( "ent_sf_shotgunpowerup" )]
[Hammer.EditorModel( "models/citizen_props/crate01.vmdl_c" )]
[Hammer.EntityTool( "Shotgun Powerup", "Snow-Fight", "A powerup that gives the players a snowball shooting shotgun." )]
partial class ShotgunPowerup : Prop, IUse
{
	private TimeSince lastUse = 0;

	private TimeSince powerupStart = 0;

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

			if ( player.ActiveChild is WeaponBase weapon )
			{	
				if ( weapon.ToString().Contains("Snowball") )
				{
					powerupStart = 0;

					weapon.Delete();
					player.Inventory.Add(new SnowballShotgun());

					if (powerupStart >= 2.5f)
					{
						weapon.Delete();
						player.Inventory.Add(new Snowball());
					}

					Sound.FromEntity( "power_pickup", this);
					Particles.Create("particles/screenspace/powerup_base.vpcf");
					Delete();
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

