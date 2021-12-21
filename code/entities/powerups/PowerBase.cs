using Sandbox;

public partial class PowerBase : Prop
{
	public virtual string modelPath => "models/citizen_props/crate01.vmdl_c";
	public virtual float expireTime => 5f;

	public override void Spawn()
	{
		base.Spawn();
		SetModel( modelPath );

		Scale = 1.5f;
	}

	public override void StartTouch( Entity other )
	{
		base.StartTouch( other );

		if ( other is SFPlayer player )
		{
			if ( player.curPowerUp != null )
				return;

			player.powerUpExpire = 0f;
			player.curPowerUp = this;
			Delete();
		}
	}
}
