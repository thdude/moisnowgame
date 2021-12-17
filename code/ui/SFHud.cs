using Sandbox.UI;

public partial class SFHud : Sandbox.HudEntity<RootPanel>
{
	public SFHud()
	{
		RootPanel.AddChild<ChatBox>();
		RootPanel.AddChild<TeamSelection>();
		RootPanel.AddChild<AmmoCounter>();
		RootPanel.AddChild<PresentCounter>();
		RootPanel.AddChild<KillFeed>();
		RootPanel.AddChild<Vitals>();

		RootPanel.AddChild<Deathscreen>();
	}
}
