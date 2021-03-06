using Sandbox;
using System.Collections.Generic;
using System.Linq;

public partial class SFPlayer : Player
{
	public bool lockControls = false;

	public PowerBase curPowerUp;

	[Net] 
	public float SprintTime { get; set; }

	private TimeSince timeLastSprint;
	public TimeSince timeLastJump;

	public Music musicPlayer;

	private ModelEntity hat;

	private TimeSince timeKilled;

	public DamageInfo dmgInfo;

	public TimeSince lastPickup;
	
	public Clothing.Container Clothing = new();

	public TimeSince powerUpExpire;

	//shitty sound system since i cant fucking code
	[Net]
	public bool hitmarksound  {get; set;}
	[Net]
	public bool killmarksound {get; set;}

	public SFPlayer()
	{
		Inventory = new Inventory( this );
	}

	public SFPlayer( Client cl ) : this()
	{
		// Load clothing from client data
		Clothing.LoadFromClient( cl );
	}

	//First time spawn (can be after joining or restarting)
	public void InitialSpawn()
	{
		musicPlayer = new Music();

		timeSinceSwitchTeam = 30.0f;
		curTeam = SFTeams.Unspecified;
		Respawn();
	}

	//Applies a hat to the player
	public void GiveHat()
	{
		if ( hat.IsValid() )
			hat.Delete();

		hat = new ModelEntity();

		if ( curTeam == SFTeams.Green )
			hat.SetModel( "models/grinch_hat.vmdl_c" );
		else if ( curTeam == SFTeams.Red )
			hat.SetModel( "models/santa_hat.vmdl_c" );

		hat.SetParent( this, true );
		hat.EnableShadowInFirstPerson = true;
		hat.EnableHideInFirstPerson = true;
	}

	//Sets the position based on current team
	public void SetPositionSpawn()
	{
		if ( curTeam != SFTeams.Unspecified )
		{
			Inventory.Add( new Snowball(), true );

			if ( curTeam == SFTeams.Green )
			{
				List<GreenSpawnpoint> spawnpoints = new List<GreenSpawnpoint>();

				foreach ( var greenPoint in All.OfType<GreenSpawnpoint>() )
					spawnpoints.Add( greenPoint );

				int checkedIndex = 0;
				int randomIndex = Rand.Int( checkedIndex, spawnpoints.Count - 1 );

				while ( spawnpoints[randomIndex].Position.IsNaN && checkedIndex < spawnpoints.Count )
				{
					checkedIndex += 1;
					randomIndex = Rand.Int( checkedIndex, spawnpoints.Count - 1 );
				}

				Position = spawnpoints[randomIndex].Position;

			}
			else if ( curTeam == SFTeams.Red )
			{

				List<RedSpawnpoint> spawnpoints = new List<RedSpawnpoint>();

				foreach ( var redPoint in All.OfType<RedSpawnpoint>() )
					spawnpoints.Add( redPoint );

				int checkedIndex = 0;
				int randomIndex = Rand.Int( checkedIndex, spawnpoints.Count - 1 );

				while ( spawnpoints[randomIndex].Position.IsNaN && checkedIndex < spawnpoints.Count )
				{
					checkedIndex += 1;
					randomIndex = Rand.Int( checkedIndex, spawnpoints.Count - 1 );
				}

				Position = spawnpoints[randomIndex].Position;
			}
		}
	}

	public override void Respawn()
	{
		SetPresent( 0 );
		SprintTime = 100f;
		Inventory.DeleteContents();

		SetModel( "models/citizen/citizen.vmdl" );

		Clothing.DressEntity( this );

		//Because the standard walk controller can't be adjusted, we'll make a new one
		Controller = new SFController();
		Animator = new StandardPlayerAnimator();
		Camera = new FirstPersonCamera();

		EnableAllCollisions = true;
		EnableDrawing = true;
		EnableHideInFirstPerson = true;
		EnableShadowInFirstPerson = true;

		base.Respawn();

		SetPositionSpawn();
		GiveHat();
	}

	[ClientRpc]
	private void UpdateSprintClient(float sprint)
	{
		SprintTime = sprint;
	}

	public override void Simulate( Client cl )
	{
		//shitty sound system part 2
		//actually no im making an even shittier solution this one does not work
		//this is moist fellow btw our actual coder is good he just wasnt online when i needed to shove this in last minute
		/*
		if(hitmarksound)
		{
			PlaySound("hitmarker");
			hitmarksound = false;	
		}
		if(killmarksound)
		{
			if(IsClient)
			{
				PlaySound("killmarker");
			}
			killmarksound = false;
		}

		//pain end ^^^^^
		*/
		if ( Health > 0 && Input.Down( InputButton.Run ) && (Input.Forward != 0 || Input.Left != 0 ) && IsServer )
		{
			timeLastSprint = 0;
			if ( SprintTime > 0.0f )
				SprintTime -= 1f;

			using( Prediction.Off() )
				UpdateSprintClient( To.Single( this ), SprintTime );
		}

		if ( timeLastSprint > 4.5f && IsServer )
			if ( SprintTime < 100.0f )
				SprintTime += 0.5f;

		if ( curPowerUp != null && IsServer )
		{
			if ( powerUpExpire > curPowerUp.expireTime )
				curPowerUp = null;
		}

		if ( LifeState == LifeState.Dead )
		{
			if(SFGame.gameStatus == SFGame.enumStatus.Idle && timeKilled > 2 && IsServer ) 
			{
				Respawn();
				return;
			}

			if ( timeKilled > 8 && IsServer )
			{
				Respawn();
			}

			return;
		}

		if ( lockControls && IsServer )
			return;

		base.Simulate( cl );
		SimulateActiveChild( cl, ActiveChild );

		TickPlayerUse();
	}

	public override void StartTouch( Entity other )
	{
		base.StartTouch( other );
	}

	public override void TakeDamage( DamageInfo info )
	{
		base.TakeDamage( info );
	}

	public override void OnKilled()
	{
		timeKilled = 0;
		base.OnKilled();

		curPowerUp = null;

		BecomeRagdollOnClient( Velocity, dmgInfo.Flags, dmgInfo.Position, dmgInfo.Force, GetHitboxBone( dmgInfo.HitboxIndex ) );
		Camera = new DeathCamera();
		EnableDrawing = false;
		EnableAllCollisions = false;

		for ( int i = 0; i < curPresents; i++ )
		{
			var presentDrop = new Present();

			presentDrop.Position = Position;
			presentDrop.Velocity = Position + (Vector3.Random * 15);
			presentDrop.Spawn();
		}

	}
	public void hitmark(bool kill)
	{
		if(IsClient)
		{
			if(kill){Sound.FromScreen("hitmarker");}
			if(!kill){Sound.FromScreen("killmarker");}
		}
	}
}
