using Sandbox;
using Sandbox.UI;
using Sandbox.UI.Construct;

partial class PresentCounter : Panel
{
	private Label presentCount;
	private int lastCount;
	private TimeSince lastPickup;

	public PresentCounter()
	{
		lastPickup = 1;
		StyleSheet.Load("/ui/PresentCounter.scss");
		presentCount = Add.Label( "x0", "presentText" );
	}

	public override void Tick()
	{
		var user = Local.Pawn;
		if ( user == null ) return;

		if( user is SFPlayer player)
		{
			if( lastCount != player.curPresents)
			{
				lastPickup = 0;
				lastCount = player.curPresents; 
			}
			SetClass( "pickup", lastPickup < 0.1f);
			
			SetClass( "death", player.Health <= 0 );

			presentCount.Text = $"x{player.curPresents}";
		}
	}
}

