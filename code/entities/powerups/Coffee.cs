using Sandbox;

[Library( "ent_sf_pw_coffee" )]
public partial class Coffee : PowerBase
{
	public override string modelPath => "models/citizen_props/coffeemug01.vmdl_c";

	public override float expireTime => 20f;

}
