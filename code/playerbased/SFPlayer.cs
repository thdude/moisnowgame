using Sandbox;
using System.Collections.Generic;
using System.Linq;

public partial class SFPlayer : Player
{
	public bool lockControls = false;
	public SFPlayer()
	{
		Inventory = new Inventory( this );
	}

	ModelEntity hat;

	private TimeSince timeKilled;

	public DamageInfo dmgInfo;

	public TimeSince lastPickup;

	//First time spawn (can be after joining or restarting)
	public void InitialSpawn()
	{
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
		lastPickup = 0;
		SetPresent( 0 );
		Inventory.DeleteContents();

		SetModel( "models/citizen/citizen.vmdl" );

		Controller = new WalkController();
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

	public override void Simulate( Client cl )
	{
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

	public override void OnKilled()
	{
		timeKilled = 0;
		base.OnKilled();
		BecomeRagdollOnClient( Velocity, dmgInfo.Flags, dmgInfo.Position, dmgInfo.Force, GetHitboxBone( dmgInfo.HitboxIndex ) );
		Camera = new DeathCamera();
		EnableDrawing = false;

		for ( int i = 0; i < curPresents; i++ )
		{
			var presentDrop = new Present();

			presentDrop.Position = Position;
			presentDrop.Velocity = Position + (Vector3.Random * 15);
			presentDrop.Spawn();
		}

	}
}
