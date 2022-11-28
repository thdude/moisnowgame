﻿
using Sandbox;
using Sandbox.UI;
using Sandbox.UI.Construct;


public partial class SFChatBox : Panel
{
	static SFChatBox Current;

	public Panel Canvas { get; protected set; }
	public TextEntry Input { get; protected set; }

	public SFChatBox()
	{
		Current = this;

		StyleSheet.Load("/ui/chat/ChatBox.scss");

		Canvas = Add.Panel("chat_canvas");

		Input = Add.TextEntry("");
		Input.AddEventListener("onsubmit", () => Submit());
		Input.AddEventListener("onblur", () => Close());
		Input.AcceptsFocus = true;
		Input.AllowEmojiReplace = true;
	}

	void Open()
	{
		AddClass("open");
		Input.Focus();
	}

	void Close()
	{
		RemoveClass("open");
		Input.Blur();
	}

	public override void Tick()
	{
		base.Tick();

		if (Sandbox.Input.Pressed(InputButton.Chat))
		{
			Open();
		}
	}

	void Submit()
	{
		Close();

		var msg = Input.Text.Trim();
		Input.Text = "";

		if (string.IsNullOrWhiteSpace(msg))
			return;

		Say(msg);
	}

	public void AddEntry(string name, string message, string avatar, string lobbyState = null)
	{
		var e = Canvas.AddChild<ChatEntry>();

		e.Message.Text = message;
		e.NameLabel.Text = name;
		e.Avatar.SetTexture(avatar);

		e.SetClass("noname", string.IsNullOrEmpty(name));
		e.SetClass("noavatar", string.IsNullOrEmpty(avatar));

		if (lobbyState == "ready" || lobbyState == "staging")
		{
			e.SetClass("is-lobby", true);
		}
	}


	[ConCmd.Client("chat_add", CanBeCalledFromServer = true)]
	public static void AddChatEntry(string name, string message, string avatar = null, string lobbyState = null)
	{
		Current?.AddEntry(name, message, avatar, lobbyState);

		// Only log clientside if we're not the listen server host
		if (!Global.IsListenServer)
		{
			Log.Info($"{name}: {message}");
		}
	}

	[ConCmd.Client("chat_addinfo", CanBeCalledFromServer = true)]
	public static void AddInformation(string message, string avatar = null)
	{
		Current?.AddEntry(null, message, avatar);
	}

	[ConCmd.Server("say")]
	public static void Say(string message)
	{
		// todo - reject more stuff
		if (message.Contains('\n') || message.Contains('\r'))
			return;

		Log.Info($"{ConsoleSystem.Caller}: {message}");
		AddChatEntry(To.Everyone, ConsoleSystem.Caller?.Name ?? "Server", message, $"avatar:{ConsoleSystem.Caller?.PlayerId}");
	}
}
