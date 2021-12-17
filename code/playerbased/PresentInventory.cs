using Sandbox;
using System;
using System.Collections.Generic;
using System.Linq;

partial class SFPlayer
{

	[Net]
	public int curPresents { get; set; } = 0;

	public void ClearPresent()
	{
		curPresents = 0;
	}

	public void SetPresent( int amount )
	{
		curPresents = amount;
	}

	public void GivePresents( int amount )
	{
		curPresents += amount;
	}

	public void TakeAmmo( int amount )
	{
		curPresents -= amount;
	}
}

