using System;
using Characters.Gear.Weapons;
using Services;
using Singletons;
using UnityEngine;

namespace Runnables.Triggers
{
	// Token: 0x02000359 RID: 857
	public sealed class EqualWeaponName : Trigger
	{
		// Token: 0x06001004 RID: 4100 RVA: 0x0002FDD8 File Offset: 0x0002DFD8
		protected override bool Check()
		{
			Weapon polymorphOrCurrent = Singleton<Service>.Instance.levelManager.player.playerComponents.inventory.weapon.polymorphOrCurrent;
			return !(polymorphOrCurrent == null) && polymorphOrCurrent.name.Equals(this._weaponName, StringComparison.OrdinalIgnoreCase);
		}

		// Token: 0x04000D1A RID: 3354
		[SerializeField]
		private string _weaponName;
	}
}
