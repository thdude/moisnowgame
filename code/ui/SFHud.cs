using Sandbox.UI;

public partial class SFHud : Sandbox.HudEntity<RootPanel>
{
	public SFHud()
	{
		RootPanel.AddChild<SFNameTags>();
		RootPanel.AddChild<Scoreboard>();
		RootPanel.AddChild<SFChatBox>();
		RootPanel.AddChild<TeamSelection>();
		RootPanel.AddChild<AmmoCounter>();
		RootPanel.AddChild<PresentCounter>();
		RootPanel.AddChild<KillFeed>();
		RootPanel.AddChild<Vitals>();
		RootPanel.AddChild<Deathscreen>();
		RootPanel.AddChild<TeamPresentCounter>();
	}
}
