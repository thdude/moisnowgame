using Sandbox;
using Sandbox.UI;
using Sandbox.UI.Construct;

partial class TeamPresentCounter : Panel
{
	private Panel greenCount;
	private Label greenText;

	private Panel redCount;
	private Label redText;

	private int lastRedCount = 0;
	private int lastGreenCount = 0;

	private TimeSince lastRedPickup;
	private TimeSince lastGreenPickup;
	public TeamPresentCounter()
	{
		lastRedPickup = 1;
		lastGreenPickup = 1;
		StyleSheet.Load( "/ui/TeamPresentCounter.scss" );

		redCount = Add.Panel( "redTeamPresents" );
		redText = redCount.Add.Label( "0", "countText" );

		greenCount = Add.Panel( "greenTeamPresents" );
		greenText = greenCount.Add.Label( "0", "countText" );
	}

	public override void Tick()
	{
		base.Tick();

		SetClass( "active", SFGame.gameStatus == SFGame.enumStatus.Active );
		
		if( lastRedCount != SFGame.redTotalGifts)
		{
			lastRedPickup = 0;
			lastRedCount = SFGame.redTotalGifts;
		}

		if ( lastGreenCount != SFGame.greenTotalGifts )
		{
			lastGreenPickup = 0;
			lastGreenCount = SFGame.greenTotalGifts;
		}

		redCount.SetClass( "update", lastRedPickup < 0.1f );
		greenCount.SetClass( "update", lastGreenPickup < 0.1f );

		redText.Text = $"{SFGame.redTotalGifts}";
		greenText.Text = $"{SFGame.greenTotalGifts}";
	}
}

