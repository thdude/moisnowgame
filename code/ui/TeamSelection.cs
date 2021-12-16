using Sandbox;
using Sandbox.UI;
using Sandbox.UI.Construct;
using System.Collections.Generic;

public partial class TeamSelection : Panel
{
	private bool isOpen = false;
	private TimeSince lastOpenTime;

	public TeamSelection()
	{
		StyleSheet.Load( "/ui/teamselection.scss" );
		Panel redPnl = Add.Panel( "redTeamMenu" );
		Panel greenPnl = Add.Panel( "greenTeamMenu" );
	}

	public override void Tick()
	{
		base.Tick();

		if(Input.Pressed(InputButton.Menu) && lastOpenTime >= 0.1f)
		{
			isOpen = !isOpen;
			lastOpenTime = 0.0f;
		}

		SetClass( "open", isOpen );
	}
}

