using Sandbox;
using System;
using System.Linq;

partial class SFPlayer : Player
{
	public SFPlayer()
	{
		Inventory = new Inventory( this );
	}

	public override void Respawn()
	{
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
