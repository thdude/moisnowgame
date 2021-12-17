using Sandbox;
using System;
using System.Linq;

public partial class SFPlayer : Player
{
	public SFPlayer()
	{
		Inventory = new Inventory( this );
	}

	//First time spawn (can be after joining or restarting)
	public void InitialSpawn()
	{
		timeSinceSwitchTeam = 30.0f;
		curTeam = SFTeams.Unspecified;
		Respawn();
	}

	public override void Respawn()
	{
		Inventory.DeleteContents();

		if ( curTeam != SFTeams.Unspecified )
			Inventory.Add( new Snowball(), true );

		SetModel( "models/citizen/citizen.vmdl" );

		Controller = new WalkController();
		Animator = new StandardPlayerAnimator();
		Camera = new FirstPersonCamera();

		EnableAllCollisions = true;
		EnableDrawing = true;
		EnableHideInFirstPerson = true;
		EnableShadowInFirstPerson = true;

		base.Respawn();
	}

	public override void Simulate( Client cl )
	{
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
		base.OnKilled();

		EnableDrawing = false;
	}
}
