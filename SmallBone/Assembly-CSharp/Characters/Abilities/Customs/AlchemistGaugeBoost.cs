using System;
using UnityEngine;

namespace Characters.Abilities.Customs
{
	// Token: 0x02000D1C RID: 3356
	[Serializable]
	public class AlchemistGaugeBoost : Ability
	{
		// Token: 0x17000E0B RID: 3595
		// (get) Token: 0x060043B2 RID: 17330 RVA: 0x000C5255 File Offset: 0x000C3455
		public bool attached
		{
			get
			{
				AlchemistGaugeBoost.Instance instance = this._instance;
				return instance != null && instance.attached;
			}
		}

		// Token: 0x17000E0C RID: 3596
		// (get) Token: 0x060043B3 RID: 17331 RVA: 0x000C5268 File Offset: 0x000C3468
		public int multiplier
		{
			get
			{
				return this._mutiplier;
			}
		}

		// Token: 0x060043B4 RID: 17332 RVA: 0x000C5270 File Offset: 0x000C3470
		public override IAbilityInstance CreateInstance(Character owner)
		{
			return this._instance = new AlchemistGaugeBoost.Instance(owner, this);
		}

		// Token: 0x040033B8 RID: 13240
		private AlchemistGaugeBoost.Instance _instance;

		// Token: 0x040033B9 RID: 13241
		[SerializeField]
		private int _mutiplier;

		// Token: 0x02000D1D RID: 3357
		public class Instance : AbilityInstance<AlchemistGaugeBoost>
		{
			// Token: 0x060043B6 RID: 17334 RVA: 0x000C528D File Offset: 0x000C348D
			public Instance(Character owner, AlchemistGaugeBoost ability) : base(owner, ability)
			{
			}

			// Token: 0x060043B7 RID: 17335 RVA: 0x00002191 File Offset: 0x00000391
			protected override void OnAttach()
			{
			}

			// Token: 0x060043B8 RID: 17336 RVA: 0x00002191 File Offset: 0x00000391
			protected override void OnDetach()
			{
			}
		}
	}
}
