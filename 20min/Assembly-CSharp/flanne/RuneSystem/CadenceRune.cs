using System;
using UnityEngine;
using UnityEngine.Events;

namespace flanne.RuneSystem
{
	// Token: 0x02000149 RID: 329
	public class CadenceRune : Rune
	{
		// Token: 0x0600087F RID: 2175 RVA: 0x00023E7A File Offset: 0x0002207A
		protected override void Init()
		{
			this.player.gun.OnShoot.AddListener(new UnityAction(this.IncrementCounter));
		}

		// Token: 0x06000880 RID: 2176 RVA: 0x00023E9D File Offset: 0x0002209D
		private void OnDestroy()
		{
			this.player.gun.OnShoot.RemoveListener(new UnityAction(this.IncrementCounter));
		}

		// Token: 0x06000881 RID: 2177 RVA: 0x00023EC0 File Offset: 0x000220C0
		public void IncrementCounter()
		{
			this._counter++;
			if (this._counter == this.shotsPerBuff - 1)
			{
				this.Activate();
				return;
			}
			if (this._counter >= this.shotsPerBuff)
			{
				this._counter = 0;
				this.Deactivate();
			}
		}

		// Token: 0x06000882 RID: 2178 RVA: 0x00023F10 File Offset: 0x00022110
		private void Activate()
		{
			this.player.stats[StatType.Piercing].AddFlatBonus(99);
			float value = this.bonusStatsPerLevel * (float)this.level;
			this.player.stats[StatType.ProjectileSize].AddMultiplierBonus(value);
			this.player.stats[StatType.BulletDamage].AddMultiplierBonus(value);
		}

		// Token: 0x06000883 RID: 2179 RVA: 0x00023F74 File Offset: 0x00022174
		private void Deactivate()
		{
			this.player.stats[StatType.Piercing].AddFlatBonus(-99);
			float num = this.bonusStatsPerLevel * (float)this.level;
			this.player.stats[StatType.ProjectileSize].AddMultiplierBonus(-1f * num);
			this.player.stats[StatType.BulletDamage].AddMultiplierBonus(-1f * num);
		}

		// Token: 0x0400064C RID: 1612
		[SerializeField]
		private float bonusStatsPerLevel;

		// Token: 0x0400064D RID: 1613
		[SerializeField]
		private int shotsPerBuff;

		// Token: 0x0400064E RID: 1614
		private int _counter;
	}
}
