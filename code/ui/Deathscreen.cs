using Sandbox;
using Sandbox.UI;
using Sandbox.UI.Construct;
partial class Deathscreen : Panel
{
	private TimeSince timeSinceDeath;
	private Label killerLabel;
	public Deathscreen()
	{
		StyleSheet.Load( "/ui/DeathScreen.scss" );
		killerLabel = Add.Label( " killed you", "killer" );
	}

	public override void Tick()
	{
		base.Tick();

		var pawn = Local.Pawn;
		
		if ( pawn != null )
			return;

	}
}

