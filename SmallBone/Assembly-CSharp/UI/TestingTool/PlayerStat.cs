using System;
using Characters;
using Services;
using Singletons;
using UnityEngine;

namespace UI.TestingTool
{
	// Token: 0x02000411 RID: 1041
	public sealed class PlayerStat : MonoBehaviour
	{
		// Token: 0x060013C5 RID: 5061 RVA: 0x0003C188 File Offset: 0x0003A388
		private void Awake()
		{
			this._player = Singleton<Service>.Instance.levelManager.player;
			foreach (Stat.Kind kind in this._statList)
			{
				UnityEngine.Object.Instantiate<PlayerStatElement>(this._statElement, this._parent).Set(kind);
			}
		}

		// Token: 0x040010C0 RID: 4288
		[SerializeField]
		private PlayerStatElement _statElement;

		// Token: 0x040010C1 RID: 4289
		[SerializeField]
		private Transform _parent;

		// Token: 0x040010C2 RID: 4290
		private Character _player;

		// Token: 0x040010C3 RID: 4291
		private Stat.Kind[] _statList = new Stat.Kind[]
		{
			Stat.Kind.Health,
			Stat.Kind.TakingDamage,
			Stat.Kind.PhysicalAttackDamage,
			Stat.Kind.MagicAttackDamage,
			Stat.Kind.BasicAttackSpeed,
			Stat.Kind.SkillAttackSpeed,
			Stat.Kind.MovementSpeed,
			Stat.Kind.ChargingSpeed,
			Stat.Kind.SkillCooldownSpeed,
			Stat.Kind.SwapCooldownSpeed,
			Stat.Kind.EssenceCooldownSpeed,
			Stat.Kind.CriticalChance,
			Stat.Kind.CriticalDamage
		};
	}
}
