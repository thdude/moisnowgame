using Sandbox;

public class DeathCamera : SpectateRagdollCamera
{
	Vector3 FocusPoint;

	public override void Activated()
	{
		base.Activated();

		FocusPoint = CurrentView.Position;
		FieldOfView = CurrentView.FieldOfView;
	}

	public override void Update()
	{
		var client = Local.Client;
		if ( client == null ) return;

		if ( client.Pawn is SFPlayer player && player.Corpse.IsValid() )
		{
			FocusPoint = player.Corpse.PhysicsGroup.GetBody( 6 ).MassCenter;

			Position = FocusPoint;
			Rotation = player.Corpse.PhysicsGroup.GetBody( 6 ).Rotation + Rotation.Identity;
			FieldOfView = FieldOfView.LerpTo( 50, Time.Delta * 3.0f );

			Viewer = null;
		}
	}
}
