using Sandbox;
using Sandbox.UI;
using Sandbox.UI.Construct;

public partial class SFChatEntry : Panel
{
	public Label NameLabel { get; internal set; }
	public Label Message { get; internal set; }
	public Image Avatar { get; internal set; }

	public RealTimeSince TimeSinceBorn = 0;

	public SFChatEntry()
	{
		Avatar = Add.Image();
		NameLabel = Add.Label( "Name", "name" );
		Message = Add.Label( "Message", "message" );
	}

	public override void Tick() 
	{
		base.Tick();

		var client = Local.Pawn;

		if( client is SFPlayer player )
		{
			NameLabel.SetClass(player.curTeam.ToString(), true);
		}

		if ( TimeSinceBorn > 10 ) 
		{ 
			Delete();
		}
	}
}
