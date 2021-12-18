using Sandbox;
using Sandbox.UI;
using Sandbox.UI.Construct;

public partial class Stamina : Panel
{
	private Panel staminaBar;

	public Stamina()
	{
		StyleSheet.Load( "/UI/Stamina.scss" );

		staminaBar = Add.Panel( "staminaBar" );
	}

	public override void Tick()
	{
		var pawn = Local.Pawn;
		if ( pawn == null )
			return;

		if ( pawn is SFPlayer player )
		{
			Style.Dirty();
			Style.Width = Length.Percent( player.SprintTime / 10);
		}

		base.Tick();
	}
}

