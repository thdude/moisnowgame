using Sandbox;
using Sandbox.UI;
using Sandbox.UI.Construct;

public partial class Vitals : Panel
{
	private Panel healthBar;
	private Panel healthIcon;

	public Vitals()
	{
		StyleSheet.Load( "UI/Vitals.scss" );

		healthBar = Add.Panel( "healthFill" );

	}

	public override void Tick()
	{
		var pawn = Local.Pawn;
		if ( pawn == null )
			return;

		healthBar.Style.Dirty();
		healthBar.Style.Height = Length.Percent( pawn.Health );

		base.Tick();
	}
}

