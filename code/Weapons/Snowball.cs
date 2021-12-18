using Sandbox;

[Library( "weapon_sf_snowball", Title = "Snow DM: Snowball" )]
partial class Snowball : WeaponBase
{
	public override string WorldModelPath => "models/snowball.vmdl";
	public override string ViewModelPath => "models/viewmodels/snowball.vmdl";
	public override float PrimaryRate => 0.95f;

	public override void Spawn()
	{
		base.Spawn();

		SetModel( WorldModelPath );

		AmmoClip = 10;
	}

	public override void OnCarryStart( Entity carrier )
	{
		base.OnCarryStart( carrier );

		PlaySound( pickupSound );
	}
	public override void ActiveStart( Entity ent )
	{
		base.ActiveStart( ent );
	}

	public override void Simulate( Client owner )
	{
		base.Simulate( owner );
	}

	public override void AttackPrimary()
	{
		TimeSincePrimaryAttack = 0;

		if ( Owner.LifeState == LifeState.Dead )
			return;

		if ( AmmoClip <= 0 )
			return;

		AmmoClip -= 1;

		( Owner as AnimEntity).SetAnimBool( "b_attack", true );

		Fire();
		ThrowBall();

	}

	[ClientRpc]
	private void Fire()
	{
		Host.AssertClient();

		ViewModelEntity?.SetAnimBool( "fire", true );
	}

	private void ThrowBall()
	{
		if ( IsServer )
		{
			using ( Prediction.Off() )
			{
				var snowball = new SnowballProjctile();
				snowball.Position = Owner.EyePos + (Owner.EyeRot.Forward * 35);
				snowball.Rotation = Owner.EyeRot;
				snowball.Owner = Owner;
				snowball.Velocity = Owner.EyeRot.Forward * 15;
			}
		}
	}

	public override void SimulateAnimator( PawnAnimator anim )
	{
		anim.SetParam( "holdtype", 1 );
		anim.SetParam( "aimat_weight", 1.0f );
	}
}

