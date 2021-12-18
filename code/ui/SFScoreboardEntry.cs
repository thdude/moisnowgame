
using Sandbox;
using Sandbox.UI;
using Sandbox.UI.Construct;

public partial class SFScoreboardEntry : Panel
{
	public Client Client;

	public Label PlayerName;
	public Label Gifts;
	public Label Kills;
	public Label Deaths;
	public Label Ping;

	public SFScoreboardEntry()
	{
		AddClass( "entry" );

		PlayerName = Add.Label( "PlayerName", "name" );
		Gifts = Add.Label( "", "gifts" );
		Kills = Add.Label( "", "kills" );
		Deaths = Add.Label( "", "deaths" );
		Ping = Add.Label( "", "ping" );
	}

	RealTimeSince TimeSinceUpdate = 0;

	public override void Tick()
	{
		base.Tick();

		if ( !IsVisible )
			return;

		if ( !Client.IsValid() )
			return;

		if ( TimeSinceUpdate < 0.1f )
			return;

		TimeSinceUpdate = 0;
		UpdateData();
	}

	public virtual void UpdateData()
	{
		PlayerName.Text = Client.Name;
		Gifts.Text = Client.GetInt( "gifts" ).ToString();
		Kills.Text = Client.GetInt( "kills" ).ToString();
		Deaths.Text = Client.GetInt( "deaths" ).ToString();
		Ping.Text = Client.Ping.ToString();
		SetClass( "me", Client == Local.Client );
	}

	public virtual void UpdateFrom( Client client )
	{
		Client = client;
		UpdateData();
	}
}
