using System;
using UnityEngine;

namespace flanne.RuneSystem
{
	// Token: 0x0200014A RID: 330
	public class DedicationRune : Rune
	{
		// Token: 0x1700008E RID: 142
		// (get) Token: 0x06000885 RID: 2181 RVA: 0x00023FE3 File Offset: 0x000221E3
		private float summonStatBuff
		{
			get
			{
				return this.summonStatBuffPerLevel * (float)this.level;
			}
		}

		// Token: 0x06000886 RID: 2182 RVA: 0x00023FF3 File Offset: 0x000221F3
		private void OnSummonChanged(object sender, object args)
		{
			this.CheckSummons();
		}

		// Token: 0x06000887 RID: 2183 RVA: 0x00023FFB File Offset: 0x000221FB
		protected override void Init()
		{
			this.CheckSummons();
			this.AddObserver(new Action<object, object>(this.OnSummonChanged), GuardianRune.SummonDestroyedNotification);
			this.AddObserver(new Action<object, object>(this.OnSummonChanged), Powerup.AppliedNotifcation);
		}

		// Token: 0x06000888 RID: 2184 RVA: 0x00024031 File Offset: 0x00022231
		private void OnDestroy()
		{
			this.RemoveObserver(new Action<object, object>(this.OnSummonChanged), GuardianRune.SummonDestroyedNotification);
			this.RemoveObserver(new Action<object, object>(this.OnSummonChanged), Powerup.AppliedNotifcation);
		}

		// Token: 0x06000889 RID: 2185 RVA: 0x00024061 File Offset: 0x00022261
		private void CheckSummons()
		{
			if (Object.FindObjectsOfType<Summon>().Length == 1)
			{
				this.Activate();
				return;
			}
			this.Deactivate();
		}

		// Token: 0x0600088A RID: 2186 RVA: 0x0002407C File Offset: 0x0002227C
		private void Activate()
		{
			if (this.active)
			{
				return;
			}
			this.active = true;
			this.player.stats[StatType.SummonDamage].AddMultiplierBonus(this.summonStatBuff);
			this.player.stats[StatType.SummonAttackSpeed].AddMultiplierBonus(this.summonStatBuff);
		}

		// Token: 0x0600088B RID: 2187 RVA: 0x000240D4 File Offset: 0x000222D4
		private void Deactivate()
		{
			if (!this.active)
			{
				return;
			}
			this.active = false;
			this.player.stats[StatType.SummonDamage].AddMultiplierBonus(-1f * this.summonStatBuff);
			this.player.stats[StatType.SummonAttackSpeed].AddMultiplierBonus(-1f * this.summonStatBuff);
		}

		// Token: 0x0400064F RID: 1615
		[SerializeField]
		private float summonStatBuffPerLevel;

		// Token: 0x04000650 RID: 1616
		private bool active;
	}
}
