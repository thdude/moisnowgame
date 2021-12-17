using Sandbox;

public partial class Game
{
	public enum enumStatus
	{
		Idle,
		Start,
		Active,
		Post
	}

	public enumStatus gameStatus = enumStatus.Idle;

	public void StopGame()
	{
		gameStatus = enumStatus.Idle;
	}

	public void StartGame()
	{
		gameStatus = enumStatus.Start;
	}

	public void EndGame()
	{
		gameStatus = enumStatus.Post;
	}

	public void DeclareWinner()
	{

	}
}
