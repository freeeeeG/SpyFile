using System;
using Characters;
using Platforms;
using Services;
using Singletons;
using UnityEngine;

namespace AchievementTrackers
{
	// Token: 0x02001436 RID: 5174
	public class BossClearTracker : MonoBehaviour
	{
		// Token: 0x0600657C RID: 25980 RVA: 0x00125A38 File Offset: 0x00123C38
		private void Start()
		{
			Singleton<Service>.Instance.levelManager.player.health.onTookDamage += new TookDamageDelegate(this.OnTookDamage);
			this._boss.health.onDied += this.OnTargetDied;
		}

		// Token: 0x0600657D RID: 25981 RVA: 0x00125A88 File Offset: 0x00123C88
		private void OnDestroy()
		{
			if (Service.quitting)
			{
				return;
			}
			if (Singleton<Service>.Instance.levelManager.player != null)
			{
				Singleton<Service>.Instance.levelManager.player.health.onTookDamage -= new TookDamageDelegate(this.OnTookDamage);
			}
			this._boss.health.onDied -= this.OnTargetDied;
		}

		// Token: 0x0600657E RID: 25982 RVA: 0x00125AF8 File Offset: 0x00123CF8
		private void OnTookDamage(in Damage originalDamage, in Damage tookDamage, double damageDealt)
		{
			if (tookDamage.attackType == Damage.AttackType.None)
			{
				return;
			}
			if (damageDealt < 1.0)
			{
				return;
			}
			this._tookDamage = true;
			Singleton<Service>.Instance.levelManager.player.health.onTookDamage -= new TookDamageDelegate(this.OnTookDamage);
		}

		// Token: 0x0600657F RID: 25983 RVA: 0x00125B48 File Offset: 0x00123D48
		private void OnTargetDied()
		{
			this._boss.health.onDied -= this.OnTargetDied;
			if (!this._onlyPerfect)
			{
				this._normalAchievement.Set();
			}
			if (!this._tookDamage)
			{
				this._perfectAchievement.Set();
			}
		}

		// Token: 0x040051B6 RID: 20918
		[SerializeField]
		private Achievement.Type _normalAchievement;

		// Token: 0x040051B7 RID: 20919
		[SerializeField]
		private Achievement.Type _perfectAchievement;

		// Token: 0x040051B8 RID: 20920
		[SerializeField]
		private bool _onlyPerfect;

		// Token: 0x040051B9 RID: 20921
		[SerializeField]
		private Character _boss;

		// Token: 0x040051BA RID: 20922
		private bool _tookDamage;
	}
}
