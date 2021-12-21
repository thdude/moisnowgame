using Sandbox;

public partial class PowerBase : Prop
{
	public virtual string modelPath => "models/citizen_props/crate01.vmdl_c";
	public virtual float expireTime => 5f;

	public override void Spawn()
	{
		base.Spawn();
		SetModel( modelPath );
	}

	public override void StartTouch( Entity other )
	{
		base.StartTouch( other );

		if ( other is SFPlayer player )
		{
			if ( player.hasPowerUp == true )
				return;

			PowerUpStart( player );
			Delete();
		}
	}

	public virtual void PowerUpStart( SFPlayer player )
	{
		player.hasPowerUp = true;
		player.powerUpExpire = 0f;
		player.Inventory.Add( this );
	}

	public virtual void PowerUpEnd( SFPlayer player )
	{
		player.hasPowerUp = false;
		player.Inventory.Drop( this );
	}
}
