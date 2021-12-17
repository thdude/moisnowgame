using Sandbox;
using Sandbox.UI;
using Sandbox.UI.Construct;

partial class AmmoCounter : Panel	
{
	private Panel ammoPnl;
	private Label ammoCount;

	public AmmoCounter()
	{
		StyleSheet.Load( "/ui/AmmoCounter.scss" );

		ammoCount = Add.Label( "x0", "ammoText" );
	}

	public override void Tick()
	{
		var player = Local.Pawn;
		if ( player == null ) return;

		var weapon = player.ActiveChild as WeaponBase;
		SetClass( "active", weapon != null );
		
		if ( weapon == null ) return;

		ammoCount.Text = $"x{weapon.AmmoClip}";
	}
}
