using Sandbox;
using Sandbox.UI;
using Sandbox.UI.Construct;
using System.Collections.Generic;

public partial class TeamSelection : Panel
{
	private bool isOpen = false;
	private TimeSince lastOpenTime;
	private Label greenBtn;
	private Label redBtn;
	
	public TeamSelection()
	{
		StyleSheet.Load( "/ui/teamselection.scss" );
		
		Panel redPnl = Add.Panel( "redTeamMenu" );
		Panel greenPnl = Add.Panel( "greenTeamMenu" );

		greenBtn = greenPnl.Add.Label( "Join Green Team", "greenBtn" );
		greenBtn.AddEventListener( "onclick", () =>
		{
			ConsoleSystem.Run( "SF_SetTeam", SFPlayer.SFTeams.Green );

			isOpen = false;
		} );

		redBtn = redPnl.Add.Label( "Join Red Team", "redBtn" );
		redBtn.AddEventListener( "onclick", () =>
		{
			ConsoleSystem.Run( "SF_SetTeam", SFPlayer.SFTeams.Red );

			isOpen = false;
		} );

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

