using Sandbox;

[ Library("ent_sf_present") ]
partial class Present : Prop
{
	private BaseTrigger trigger;

	public override void Spawn()
	{
		base.Spawn();

		trigger = new BaseTrigger();

		SetModel( "models/citizen_props/crate01.vmdl_c" );

		RenderColor = Color.Red;

		GlowState = GlowStates.On;
		GlowDistanceStart = 0;
		GlowDistanceEnd = 500;
		GlowColor = Color.White;

	}

	public override void StartTouch( Entity other )
	{
		Log.Info( other );

		base.StartTouch( other );

		if( other is SFPlayer player)
		{
			player.GivePresents( 1 );
			Delete();

			using ( Prediction.Off() )
				Sound.FromEntity( "present_pickup", this );
		}
	}

}
