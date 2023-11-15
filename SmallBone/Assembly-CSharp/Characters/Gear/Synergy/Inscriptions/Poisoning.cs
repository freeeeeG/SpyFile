using System;
using Characters.Abilities;
using Characters.Abilities.Upgrades;
using UnityEngine;

namespace Characters.Gear.Synergy.Inscriptions
{
	// Token: 0x020008A6 RID: 2214
	public sealed class Poisoning : SimpleStatBonusKeyword
	{
		// Token: 0x17000A17 RID: 2583
		// (get) Token: 0x06002F11 RID: 12049 RVA: 0x0008D442 File Offset: 0x0008B642
		protected override double[] statBonusByStep
		{
			get
			{
				return this._statBonusByStep;
			}
		}

		// Token: 0x17000A18 RID: 2584
		// (get) Token: 0x06002F12 RID: 12050 RVA: 0x00088ACF File Offset: 0x00086CCF
		protected override Stat.Category statCategory
		{
			get
			{
				return Stat.Category.Constant;
			}
		}

		// Token: 0x17000A19 RID: 2585
		// (get) Token: 0x06002F13 RID: 12051 RVA: 0x0008D44A File Offset: 0x0008B64A
		protected override Stat.Kind statKind
		{
			get
			{
				return Stat.Kind.PoisonTickFrequency;
			}
		}

		// Token: 0x06002F14 RID: 12052 RVA: 0x0008D451 File Offset: 0x0008B651
		protected override void Initialize()
		{
			base.Initialize();
			this._step1Ability.Initialize();
		}

		// Token: 0x06002F15 RID: 12053 RVA: 0x0008D464 File Offset: 0x0008B664
		public override void UpdateBonus(bool wasActive, bool wasOmen)
		{
			base.UpdateStat();
			if (this.keyword.step >= 1)
			{
				if (!base.character.ability.Contains(this._step1Ability.ability) && base.character.ability.GetInstance<KettleOfSwampWitch>() == null)
				{
					base.character.ability.Add(this._step1Ability.ability);
					return;
				}
			}
			else
			{
				base.character.ability.Remove(this._step1Ability.ability);
			}
		}

		// Token: 0x06002F16 RID: 12054 RVA: 0x0008D4ED File Offset: 0x0008B6ED
		public override void Detach()
		{
			base.Detach();
			base.character.ability.Remove(this._step1Ability.ability);
		}

		// Token: 0x040026F1 RID: 9969
		[SerializeField]
		private double[] _statBonusByStep = new double[]
		{
			0.0,
			0.0,
			0.5,
			1.5
		};

		// Token: 0x040026F2 RID: 9970
		[Header("1 Step 효과")]
		[SerializeField]
		private OperationByTriggerComponent _step1Ability;
	}
}
