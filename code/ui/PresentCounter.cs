using Sandbox;
using Sandbox.UI;
using Sandbox.UI.Construct;

partial class PresentCounter : Panel
{
	private Label presentCount;
	private int lastCount;

	public PresentCounter()
	{
		StyleSheet.Load("/ui/PresentCounter.scss");
		presentCount = Add.Label( "x0", "presentText" );
	}

	public override void Tick()
	{
		var user = Local.Pawn;
		if ( user == null ) return;

		if( user is SFPlayer player)
		{
			//Log.Info( player.lastPickup );
			SetClass( "pickup", player.lastPickup < 0.5f);
			
			presentCount.Text = $"x{player.curPresents}";
		}
	}
}

