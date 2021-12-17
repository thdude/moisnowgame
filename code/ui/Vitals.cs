using Sandbox;
using Sandbox.UI;
using Sandbox.UI.Construct;

public partial class Vitals : Panel
{
	private Panel healthBar;
	private Panel healthEmptyIcon;
	private Panel healthFillIcon;

	public Vitals()
	{
		StyleSheet.Load( "UI/Vitals.scss" );

		healthBar = Add.Panel( "healthEmpty" );
		healthFillIcon = Add.Panel( "healthFill" );
	}

	public override void Tick()
	{
		var pawn = Local.Pawn;
		if ( pawn == null )
			return;

		healthFillIcon.Style.Dirty();
		healthFillIcon.Style.Height = Length.Percent( pawn.Health );

		base.Tick();
	}
}

