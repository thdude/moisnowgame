using Sandbox;

[Library( "weapon_sf_snowgun", Title = "Snow DM: Snow Shotgun" )]
partial class SnowballShotgun : Snowball
{
	public override string WorldModelPath => "models/christmas/snowball.vmdl";
	public override string ViewModelPath => "";
	public override float PrimaryRate => 1.35f;

	public override void Spawn()
	{
		base.Spawn();

		SetModel( WorldModelPath );

		AmmoClip = 4;
	}

	public override void AttackPrimary()
	{
		TimeSincePrimaryAttack = 0;
		TimeSinceSecondaryAttack = 0;

		if ( AmmoClip <= 0 )
		{
			DryFire();
			return;
		}

		(Owner as AnimatedEntity).SetAnimParameter( "b_attack", true );
		//(Owner as ExperimentPlayer).TakeAmmo( AmmoType.SDM_Snowballs, 1 );

		Fire();

		for ( int i = 0; i < 4; i++ )
		{
			ShootBullet( 1.85f, 0.0f, 0.0f, 0.0f );
		}
	}

	public override void ShootBullet( float spread, float force, float damage, float bulletSize )
	{
		var forward = Owner.EyeRotation.Forward;
		forward += (Vector3.Random + Vector3.Random + Vector3.Random + Vector3.Random) * spread * 0.25f;
		forward = forward.Normal;

		if ( IsServer )
		{
			using ( Prediction.Off() )
			{
				var snowball = new SnowballProjctile();
				snowball.Position = Owner.EyePosition + (forward * 35);
				snowball.Rotation = Owner.EyeRotation;
				snowball.Owner = Owner;
				snowball.Velocity = forward * 15;
			}
		}
	}

	[ClientRpc]
	private void Fire()
	{
		Host.AssertClient();

		ViewModelEntity?.SetAnimParameter( "fire", true );
	}
}

