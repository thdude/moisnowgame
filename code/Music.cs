using Sandbox;
public partial class Music
{
	private Sound music;
	public Music()
	{
		
	}
	public void StopMusicClient()
	{
		music.Stop();
	}


	public void StopMusic()
	{
		using ( Prediction.Off() )
			music.Stop();
	}

	public void StartMusic( string musicPath )
	{
		using ( Prediction.Off() )
		{
			music = Sound.FromScreen( musicPath );
			music.SetVolume( 0.10f );
		}
	}
}
