using Sandbox;
using System;

[Library( "ent_sf_depositbox" )]
[Hammer.EditorModel( "models/citizen_props/crate01.vmdl_c" )]
[Hammer.EntityTool( "Deposit Box", "Snow-Fight", "A box to deposit presents" )]
partial class DepositBox : Prop, IUse
{
	[Flags]
	public enum Flags
	{
		Green = 1,
		Red = 2
	}

	//To prevent other teams depositing gifts in the opposing teams box
	[Property( "teamEnum", Title = "Which team can deposit the presents in" )]
	public Flags TeamEnum { get; set; }

	public override void Spawn()
	{
		base.Spawn();
		SetModel( "models/citizen_props/crate01.vmdl_c" );
	}

	public bool IsUsable( Entity user )
	{
		if ( user is SFPlayer player )
		{
			//If the player has at least a present, let them use the box
			if ( player.curPresents > 0 )
				return true;
		}

		return false;
	}

	public bool OnUse( Entity user )
	{
		if ( user is SFPlayer player )
		{
			//If the player is not on the correct team to deposit the presents
			//return false
			if ( player.curTeam.ToString() != TeamEnum.ToString() )
				return false;

			//If the player has a present at least
			if ( player.curPresents > 0 )
			{
				//Check to see which team they're on and update the team present UI
				if ( player.curTeam == SFPlayer.SFTeams.Red )
				{
					SFGame.redTotalGifts += player.curPresents;
					using ( Prediction.Off() )
						SFGame.UpdateTeamScoreClient( To.Everyone, SFPlayer.SFTeams.Red, player.curPresents );
				}
				else if ( player.curTeam == SFPlayer.SFTeams.Green )
				{
					SFGame.greenTotalGifts += player.curPresents;
					using ( Prediction.Off() )
						SFGame.UpdateTeamScoreClient( To.Everyone, SFPlayer.SFTeams.Green, player.curPresents );
				}
				
				if(SFGame.redTotalGifts >= SFGame.winGoal )
				{
					SFGame.redTotalGifts = SFGame.winGoal;
					SFGame.DeclareWinner( SFPlayer.SFTeams.Red );

					SFGame.SetTeamScoreClient( To.Everyone, SFPlayer.SFTeams.Red, SFGame.winGoal );
				} else if (SFGame.greenTotalGifts >= SFGame.winGoal )
				{
					SFGame.greenTotalGifts = SFGame.winGoal;
					SFGame.DeclareWinner( SFPlayer.SFTeams.Green );

					SFGame.SetTeamScoreClient( To.Everyone, SFPlayer.SFTeams.Green, SFGame.winGoal );
				}

				player.SetPresent( 0 );
			}
				
		}

		return true;
	}
}

