using Sandbox;
using Sandbox.UI;
using Sandbox.UI.Construct;

partial class TeamPresentCounter : Panel
{
	private Panel greenCount;
	private Label greenText;

	private Panel redCount;
	private Label redText;

	private int lastCount;
	private TimeSince lastPickup;

	public TeamPresentCounter()
	{
		lastPickup = 1;
		StyleSheet.Load( "/ui/TeamPresentCounter.scss" );

		redCount = Add.Panel( "redTeamPresents" );
		redText = redCount.Add.Label( "x0", "countText" );

		greenCount = Add.Panel( "greenTeamPresents" );
		greenText = greenCount.Add.Label( "x0", "countText" );
	}

	public override void Tick()
	{
		base.Tick();

		SetClass( "active", SFGame.gameStatus == SFGame.enumStatus.Active );
	}
}

