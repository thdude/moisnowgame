using Sandbox;
using Sandbox.UI;
using Sandbox.UI.Construct;

partial class WinnerAnnounce : Panel
{
	private Label winText;

	public WinnerAnnounce()
	{
		StyleSheet.Load( "ui/WinnerAnnounce.scss" );

		winText = Add.Label( "", "winnerannounce" );
	}

	public override void Tick()
	{
		SetClass( "active", SFGame.gameStatus == SFGame.enumStatus.Post );

		if ( SFGame.redWinner )
			winText.Text = "The Happy Santa elves win!";


		if ( SFGame.greenWinner )
			winText.Text = "The evil Grinch minions win!";

		SetClass( "Red", SFGame.redWinner );
		SetClass( "Green", SFGame.greenWinner );
	}
}
