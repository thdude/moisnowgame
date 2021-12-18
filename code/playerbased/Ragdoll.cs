﻿using Sandbox;


partial class SFPlayer
{
	[ClientRpc]
	private void BecomeRagdollOnClient( Vector3 velocity, DamageFlags damageFlags, Vector3 forcePos, Vector3 force,
		int bone )
	{
		var ent = new ModelEntity();
		ent.Position = Position;
		ent.Rotation = Rotation;
		ent.Scale = Scale;
		ent.MoveType = MoveType.Physics;
		ent.UsePhysicsCollision = true;
		ent.EnableAllCollisions = true;
		ent.CollisionGroup = CollisionGroup.Debris;
		ent.SetModel( GetModelName() );
		ent.CopyBonesFrom( this );
		ent.CopyBodyGroups( this );
		ent.CopyMaterialGroup( this );
		ent.TakeDecalsFrom( this );
		ent.EnableHitboxes = true;
		ent.EnableAllCollisions = true;
		ent.SurroundingBoundsMode = SurroundingBoundsType.Physics;
		ent.RenderColor = RenderColor;
		ent.PhysicsGroup.Velocity = velocity;

		ent.SetInteractsAs( CollisionLayer.Debris );
		ent.SetInteractsWith( CollisionLayer.WORLD_GEOMETRY );
		ent.SetInteractsExclude( CollisionLayer.Player | CollisionLayer.Debris );

		ModelEntity droppedHat = new ModelEntity();

		if ( curTeam == SFTeams.Green )
			droppedHat.SetModel( "models/grinch_hat.vmdl_c" );
		else if ( curTeam == SFTeams.Red )
			droppedHat.SetModel( "models/santa_hat.vmdl_c" );

		droppedHat.SetParent( ent, true);

		if ( damageFlags.HasFlag( DamageFlags.Bullet ) )
		{
			if ( bone >= 0 )
			{
				var body = ent.GetBonePhysicsBody( bone );
				if ( body != null )
				{
					body.ApplyImpulseAt( forcePos, force * body.Mass );
				}
				else
				{
					ent.PhysicsGroup.ApplyImpulse( force );
				}
			}
		}

		if ( damageFlags.HasFlag( DamageFlags.Blast ) )
		{
			if ( ent.PhysicsGroup != null )
			{
				ent.PhysicsGroup.AddVelocity( (Position - (forcePos + Vector3.Down * 100.0f)).Normal *
												(force.Length * 0.2f) );
				var angularDir = (Rotation.FromYaw( 90 ) * force.WithZ( 0 ).Normal).Normal;
				ent.PhysicsGroup.AddAngularVelocity( angularDir * (force.Length * 0.02f) );
			}
		}

		Corpse = ent;

		ent.DeleteAsync( 10.0f );
	}
}
