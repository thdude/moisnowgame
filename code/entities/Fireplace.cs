using Sandbox;

[Library("ent_sf_fireplace")]
partial class Fireplace : Prop
{
	private Particles fireEffect;

	public override void Spawn()
	{
		base.Spawn();

		SetModel( "models/citizen_props/crate01.vmdl_c" );

		fireEffect = Particles.Create( "particles/fire/fireplace_base.vpcf" );
		fireEffect.SetPosition(0, LocalPosition + new Vector3(0, 0, 25) );
	}


}
