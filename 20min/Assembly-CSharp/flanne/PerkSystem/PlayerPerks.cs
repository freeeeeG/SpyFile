using System;
using System.Collections.Generic;
using UnityEngine;

namespace flanne.PerkSystem
{
	// Token: 0x02000186 RID: 390
	public class PlayerPerks : MonoBehaviour
	{
		// Token: 0x06000980 RID: 2432 RVA: 0x00026A15 File Offset: 0x00024C15
		private void Start()
		{
			this._equippedPerks = new List<Powerup>();
		}

		// Token: 0x06000981 RID: 2433 RVA: 0x00026A24 File Offset: 0x00024C24
		public void Equip(Powerup perk)
		{
			if (!this._equippedPerks.Contains(perk))
			{
				Powerup powerup = perk.Copy<Powerup>();
				powerup.Apply(this.player);
				this._equippedPerks.Add(powerup);
				return;
			}
			perk.Copy<Powerup>().ApplyStack(this.player);
		}

		// Token: 0x06000982 RID: 2434 RVA: 0x00026A70 File Offset: 0x00024C70
		public void Equip(GunEvolution gunEvo)
		{
			gunEvo.Copy<GunEvolution>().Apply(this.player);
		}

		// Token: 0x040006E4 RID: 1764
		[SerializeField]
		private PlayerController player;

		// Token: 0x040006E5 RID: 1765
		private List<Powerup> _equippedPerks;
	}
}
