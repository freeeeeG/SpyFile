using System;
using UnityEngine;

namespace flanne.Player.Buffs
{
	// Token: 0x0200016A RID: 362
	public class AddSummonDamageToDamageMod : IDamageModifier
	{
		// Token: 0x06000925 RID: 2341 RVA: 0x00025F18 File Offset: 0x00024118
		public ValueModifier GetMod()
		{
			if (this._summon == null)
			{
				foreach (ShootingSummon shootingSummon in PlayerController.Instance.GetComponentsInChildren<ShootingSummon>(true))
				{
					if (shootingSummon.SummonTypeID == this.SummonTypeID)
					{
						this._summon = shootingSummon;
						break;
					}
				}
			}
			return new AddValueModifier(1, (float)this._summon.baseDamage * this.multiplier);
		}

		// Token: 0x040006AE RID: 1710
		[SerializeField]
		private string SummonTypeID;

		// Token: 0x040006AF RID: 1711
		[SerializeField]
		private float multiplier;

		// Token: 0x040006B0 RID: 1712
		[NonSerialized]
		private ShootingSummon _summon;
	}
}
