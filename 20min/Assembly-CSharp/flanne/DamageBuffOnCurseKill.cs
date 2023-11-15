using System;
using UnityEngine;

namespace flanne
{
	// Token: 0x020000DC RID: 220
	public class DamageBuffOnCurseKill : MonoBehaviour
	{
		// Token: 0x0600069E RID: 1694 RVA: 0x0001DD21 File Offset: 0x0001BF21
		private void OnCurseKill(object sender, object args)
		{
			this._killCounter++;
			if ((float)this._killCounter >= this.numKillsPerBuff)
			{
				this._killCounter = 0;
				this.playerStats[StatType.BulletDamage].AddMultiplierBonus(this.damageBuff);
			}
		}

		// Token: 0x0600069F RID: 1695 RVA: 0x0001DD5E File Offset: 0x0001BF5E
		private void Start()
		{
			this.AddObserver(new Action<object, object>(this.OnCurseKill), CurseSystem.CurseKillNotification);
			this.playerStats = PlayerController.Instance.stats;
		}

		// Token: 0x060006A0 RID: 1696 RVA: 0x0001DD87 File Offset: 0x0001BF87
		private void OnDestroy()
		{
			this.RemoveObserver(new Action<object, object>(this.OnCurseKill), CurseSystem.CurseKillNotification);
		}

		// Token: 0x0400046C RID: 1132
		[SerializeField]
		private float damageBuff;

		// Token: 0x0400046D RID: 1133
		[SerializeField]
		private float numKillsPerBuff;

		// Token: 0x0400046E RID: 1134
		private StatsHolder playerStats;

		// Token: 0x0400046F RID: 1135
		private int _killCounter;
	}
}
