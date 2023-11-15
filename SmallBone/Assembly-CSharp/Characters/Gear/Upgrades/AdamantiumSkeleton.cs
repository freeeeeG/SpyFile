using System;
using Hardmode.Darktech;
using Singletons;
using UnityEngine;

namespace Characters.Gear.Upgrades
{
	// Token: 0x02000848 RID: 2120
	public sealed class AdamantiumSkeleton : UpgradeAbility
	{
		// Token: 0x06002C2A RID: 11306 RVA: 0x0008743C File Offset: 0x0008563C
		public override void Attach(Character target)
		{
			Singleton<DarktechManager>.Instance.setting.건강보조장치스탯증폭량 += (float)this._statValueMultiplier;
		}

		// Token: 0x06002C2B RID: 11307 RVA: 0x0008745B File Offset: 0x0008565B
		public override void Detach()
		{
			Singleton<DarktechManager>.Instance.setting.건강보조장치스탯증폭량 -= (float)this._statValueMultiplier;
		}

		// Token: 0x04002555 RID: 9557
		[SerializeField]
		[Range(0f, 100f)]
		private int _statValueMultiplier;
	}
}
