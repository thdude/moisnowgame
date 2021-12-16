using Sandbox;

[Library( "weapon_sf_snowball", Title = "Snow DM: Snowball" )]
partial class Snowball : WeaponBase
{
	public override string WorldModelPath => "models/christmas/snowball.vmdl";
	public override string ViewModelPath => "";
	public override float PrimaryRate => 1.05f;
	public override AmmoType AmmoType => AmmoType.SDM_SnowAmmo;

	public override void Spawn()
	{
		base.Spawn();

		SetModel( WorldModelPath );

		AmmoClip = 1;
	}

	public override void OnCarryStart( Entity carrier )
	{
		base.OnCarryStart( carrier );

		PlaySound( pickupSound );
	}
	public override void ActiveStart( Entity ent )
	{
		base.ActiveStart( ent );
		//ViewModelEntity?.SetAnimBool( "idle", false );
	}

	public override void Simulate( Client owner )
	{
		base.Simulate( owner );
	}

	public override void AttackPrimary()
	{
		TimeSincePrimaryAttack = 0;

		if ( (Owner as SFPlayer).AmmoCount( AmmoType.SDM_SnowAmmo ) <= 0 )
			return;


		(Owner as AnimEntity).SetAnimBool( "b_attack", true );

		Fire();
		ThrowBall();

		//(Owner as ExperimentPlayer).TakeAmmo(AmmoType.SDM_Snowballs, 1);
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

