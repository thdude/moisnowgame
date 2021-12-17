using System;
using Sandbox;
using Sandbox.UI;
using Sandbox.UI.Construct;

partial class Timer : Panel
{
	public Label timeleft;

	public Timer()
	{
		StyleSheet.Load( "/ui/Timer.scss" );

		timeleft = Add.Label( "0", "timer" );
	}

	public override void Tick()
	{
		base.Tick();

		SetClass( "active", SFGame.timeToBegin < 5);
	}
}
