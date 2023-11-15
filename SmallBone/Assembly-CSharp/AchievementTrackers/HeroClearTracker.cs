using System;
using System.Collections.Generic;
using System.Linq;
using Characters;
using Characters.Gear.Weapons;
using Data;
using Level.Npc.FieldNpcs;
using Platforms;
using Services;
using Singletons;
using UnityEngine;

namespace AchievementTrackers
{
	// Token: 0x02001439 RID: 5177
	public class HeroClearTracker : MonoBehaviour
	{
		// Token: 0x06006588 RID: 25992 RVA: 0x00125D68 File Offset: 0x00123F68
		private void Start()
		{
			Singleton<Service>.Instance.levelManager.player.health.onTookDamage += new TookDamageDelegate(this.OnTookDamage);
			this._boss.health.onDied += this.OnTargetDied;
		}

		// Token: 0x06006589 RID: 25993 RVA: 0x00125DB8 File Offset: 0x00123FB8
		private void OnDestroy()
		{
			Singleton<Service>.Instance.levelManager.player.health.onTookDamage -= new TookDamageDelegate(this.OnTookDamage);
			this._boss.health.onDied -= this.OnTargetDied;
		}

		// Token: 0x0600658A RID: 25994 RVA: 0x00125E08 File Offset: 0x00124008
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

		// Token: 0x0600658B RID: 25995 RVA: 0x00125E58 File Offset: 0x00124058
		private void OnTargetDied()
		{
			this._boss.health.onDied -= this.OnTargetDied;
			this._normalAchievement.Set();
			if (!this._tookDamage)
			{
				this._perfectAchievement.Set();
			}
			if (Singleton<Service>.Instance.levelManager.player.playerComponents.inventory.weapon.current.name.Equals(this._littleBone.name))
			{
				Achievement.Type.SkeletonKing.Set();
			}
			if (!GameData.Progress.fieldNpcEncountered.Any((KeyValuePair<NpcType, BoolData> kvp) => kvp.Value.value))
			{
				Achievement.Type.ColdBlood.Set();
			}
		}

		// Token: 0x040051C0 RID: 20928
		[SerializeField]
		private Achievement.Type _normalAchievement;

		// Token: 0x040051C1 RID: 20929
		[SerializeField]
		private Achievement.Type _perfectAchievement;

		// Token: 0x040051C2 RID: 20930
		[SerializeField]
		private Character _boss;

		// Token: 0x040051C3 RID: 20931
		[SerializeField]
		private Weapon _littleBone;

		// Token: 0x040051C4 RID: 20932
		private bool _tookDamage;
	}
}
