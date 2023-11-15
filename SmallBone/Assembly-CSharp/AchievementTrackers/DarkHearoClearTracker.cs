using System;
using Characters;
using Data;
using Platforms;
using Services;
using Singletons;
using UnityEngine;

namespace AchievementTrackers
{
	// Token: 0x02001437 RID: 5175
	public sealed class DarkHearoClearTracker : MonoBehaviour
	{
		// Token: 0x06006581 RID: 25985 RVA: 0x00125B98 File Offset: 0x00123D98
		private void Start()
		{
			Singleton<Service>.Instance.levelManager.player.health.onTookDamage += new TookDamageDelegate(this.OnTookDamage);
			this._boss.health.onDied += this.OnTargetDied;
		}

		// Token: 0x06006582 RID: 25986 RVA: 0x00125BE8 File Offset: 0x00123DE8
		private void OnDestroy()
		{
			Singleton<Service>.Instance.levelManager.player.health.onTookDamage -= new TookDamageDelegate(this.OnTookDamage);
			this._boss.health.onDied -= this.OnTargetDied;
		}

		// Token: 0x06006583 RID: 25987 RVA: 0x00125C38 File Offset: 0x00123E38
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

		// Token: 0x06006584 RID: 25988 RVA: 0x00125C88 File Offset: 0x00123E88
		private void OnTargetDied()
		{
			this._boss.health.onDied -= this.OnTargetDied;
			if (!this._tookDamage)
			{
				this._perfectAchievement.Set();
			}
			switch (GameData.HardmodeProgress.hardmodeLevel)
			{
			case 1:
				Achievement.Type.DifficultAdventure.Set();
				return;
			case 2:
				Achievement.Type.CourageousAct.Set();
				return;
			case 3:
				Achievement.Type.ResultOfEffort.Set();
				return;
			case 4:
				Achievement.Type.SelfConfidence.Set();
				return;
			case 5:
				Achievement.Type.PatienceAndPerseverance.Set();
				return;
			case 6:
				Achievement.Type.ChallengeSpirit.Set();
				return;
			case 7:
				Achievement.Type.AmazingStep.Set();
				return;
			case 8:
				Achievement.Type.PhenomenalSuccess.Set();
				return;
			case 9:
				Achievement.Type.GreatAchievement.Set();
				return;
			case 10:
				Achievement.Type.ImpossibleIsNothing.Set();
				return;
			default:
				return;
			}
		}

		// Token: 0x040051BB RID: 20923
		[SerializeField]
		private Achievement.Type _perfectAchievement;

		// Token: 0x040051BC RID: 20924
		[SerializeField]
		private Character _boss;

		// Token: 0x040051BD RID: 20925
		private bool _tookDamage;
	}
}
