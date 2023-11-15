using System;
using UnityEngine;

namespace flanne
{
	// Token: 0x020000E5 RID: 229
	public class GainMaxHPOnSmiteKill : MonoBehaviour
	{
		// Token: 0x060006C3 RID: 1731 RVA: 0x0001E43C File Offset: 0x0001C63C
		private void OnSmiteKill(object sender, object args)
		{
			if (this._mhpGainCounter >= this.mhpGainCap)
			{
				return;
			}
			this._killCounter++;
			if (this._killCounter >= this.killsToGainMHP)
			{
				this._killCounter = 0;
				this.playerStats[StatType.MaxHP].AddFlatBonus(1);
				int a = Mathf.FloorToInt(this.playerStats[StatType.MaxHP].Modify((float)this.player.loadedCharacter.startHP));
				this.playerHealth.maxHP = Mathf.Min(a, 20);
				this._mhpGainCounter++;
			}
		}

		// Token: 0x060006C4 RID: 1732 RVA: 0x0001E4D8 File Offset: 0x0001C6D8
		private void Start()
		{
			this.player = base.GetComponentInParent<PlayerController>();
			this.playerStats = this.player.stats;
			this.playerHealth = this.player.playerHealth;
			this.AddObserver(new Action<object, object>(this.OnSmiteKill), SmitePassive.SmiteKillNotification);
		}

		// Token: 0x060006C5 RID: 1733 RVA: 0x0001E52A File Offset: 0x0001C72A
		private void OnDestroy()
		{
			this.RemoveObserver(new Action<object, object>(this.OnSmiteKill), SmitePassive.SmiteKillNotification);
		}

		// Token: 0x0400048D RID: 1165
		[SerializeField]
		private int killsToGainMHP;

		// Token: 0x0400048E RID: 1166
		[SerializeField]
		private int mhpGainCap;

		// Token: 0x0400048F RID: 1167
		private PlayerController player;

		// Token: 0x04000490 RID: 1168
		private StatsHolder playerStats;

		// Token: 0x04000491 RID: 1169
		private PlayerHealth playerHealth;

		// Token: 0x04000492 RID: 1170
		private int _killCounter;

		// Token: 0x04000493 RID: 1171
		private int _mhpGainCounter;
	}
}
