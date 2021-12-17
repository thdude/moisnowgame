using Sandbox;
public class Music
{
	private Sound music;
	public Music()
	{
	}

	public void StopMusic()
	{
		music.Stop();
	}

	public void StartMusic( string musicPath )
	{
		using ( Prediction.Off() )
		{
			music = Sound.FromScreen( musicPath );
			music.SetVolume( 0.75f );
		}
	}
}
