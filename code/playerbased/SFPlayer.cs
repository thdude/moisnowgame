using Sandbox;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class SFPlayer : Player
{
	public SFPlayer()
	{
		Inventory = new Inventory( this );
	}

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
					Log.Info( checkedIndex );
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
					Log.Info( checkedIndex );
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
	}

	public override void Simulate( Client cl )
	{
		if ( LifeState == LifeState.Dead )
		{
			if ( timeKilled > 8 && IsServer )
			{
				Respawn();
			}

			return;
		}

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
	}
}
