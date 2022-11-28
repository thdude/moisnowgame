using Sandbox;
using System;

[Library( "ent_sf_shotgunpowerup" )]
partial class ShotgunPowerup : Prop, IUse
{
	private TimeSince lastUse = 0;

	private TimeSince powerupStart = 0;

	public override void Spawn()
	{
		base.Spawn();
		SetModel("models/snowpile.vmdl");

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

