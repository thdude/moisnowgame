using Sandbox;

[Library( "weapon_sf_snowball", Title = "Snow DM: Snowball" )]
partial class Snowball : WeaponBase
{
	public override string WorldModelPath => "models/snowball.vmdl";
	public override string ViewModelPath => "models/viewmodels/snowball.vmdl";
	public override float PrimaryRate => 0.95f;

	public bool empty;

	public override int MaxAmmoClip => 15;

	public override void Spawn()
	{
		base.Spawn();

		SetModel( WorldModelPath );

		AmmoClip = 15;
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

		if (AmmoClip == 0)
			empty = true;
		else
			empty = false;
		ViewModelEntity?.SetAnimParameter("empty", empty);
	}

	public override void AttackPrimary()
	{
		TimeSincePrimaryAttack = 0;

		if ( Owner.LifeState == LifeState.Dead )
			return;

		if ( AmmoClip <= 0 )
			return;

		AmmoClip -= 1;

		( Owner as AnimatedEntity).SetAnimParameter( "b_attack", true );

		Fire();
		ThrowBall();

	}

	[ClientRpc]
	private void Fire()
	{
		Host.AssertClient();

		ViewModelEntity?.SetAnimParameter( "fire", true );
	}

	private void ThrowBall()
	{
		if ( IsServer )
		{
			using ( Prediction.Off() )
			{
				var snowball = new SnowballProjctile();
				snowball.Position = Owner.EyePosition + (Owner.EyeRotation.Forward * 35);
				snowball.Rotation = Owner.EyeRotation;
				snowball.Owner = Owner;
				snowball.Velocity = Owner.EyeRotation.Forward * 15;
			}
		}
		if(IsClient)
		{
			PlaySound("throw");
		}
	}

	public override void SimulateAnimator( PawnAnimator anim )
	{
		if (!empty)
		{
			anim.SetAnimParameter( "holdtype", 1 );
			anim.SetAnimParameter( "aimat_weight", 1.0f );
		}

		else if (empty)
		{
			anim.SetAnimParameter("holdtype", 0);
			anim.SetAnimParameter( "aimat_weight", 1.0f );
		}
	}
}

