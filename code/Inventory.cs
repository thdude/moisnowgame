﻿using Sandbox;
using System;
using System.Linq;

partial class Inventory : BaseInventory
{
	public Inventory( Player player ) : base ( player )
	{

	}

	public override bool Add( Entity ent, bool makeActive = false )
	{
		var player = Owner as SFPlayer;
		var weapon = ent as WeaponBase;
		//
		// We don't want to pick up the same weapon twice
		// But we'll take the ammo from it Winky Face
		//
		if ( weapon != null && IsCarryingType( ent.GetType() ) )
		{
			var ammo = weapon.AmmoClip;
			var ammoType = weapon.AmmoType;

			if ( ammo > 0 )
			{
				player.GiveAmmo( ammoType, ammo );
				
			}

			ent.Delete();
			return false;
		}
		
		return base.Add( ent, makeActive );
	}

	public bool IsCarryingType( Type t )
	{
		return List.Any( x => x.GetType() == t );
	}
}
