using System;
using Characters.Abilities;
using Platforms;
using UnityEngine;

namespace Characters.Gear.Synergy.Inscriptions
{
	// Token: 0x020008A5 RID: 2213
	public sealed class Omen : SimpleStatBonusKeyword
	{
		// Token: 0x17000A14 RID: 2580
		// (get) Token: 0x06002F09 RID: 12041 RVA: 0x0008D2F0 File Offset: 0x0008B4F0
		protected override double[] statBonusByStep
		{
			get
			{
				return this._statBonusByStep;
			}
		}

		// Token: 0x17000A15 RID: 2581
		// (get) Token: 0x06002F0A RID: 12042 RVA: 0x00089044 File Offset: 0x00087244
		protected override Stat.Category statCategory
		{
			get
			{
				return Stat.Category.Percent;
			}
		}

		// Token: 0x17000A16 RID: 2582
		// (get) Token: 0x06002F0B RID: 12043 RVA: 0x00088CAD File Offset: 0x00086EAD
		protected override Stat.Kind statKind
		{
			get
			{
				return Stat.Kind.Health;
			}
		}

		// Token: 0x06002F0C RID: 12044 RVA: 0x0008D2F8 File Offset: 0x0008B4F8
		protected override void Initialize()
		{
			base.Initialize();
			this._firstAbilityComponent.Initialize();
			this._maxAbilityComponent.Initialize();
		}

		// Token: 0x06002F0D RID: 12045 RVA: 0x0008D318 File Offset: 0x0008B518
		public override void UpdateBonus(bool wasActive, bool wasOmen)
		{
			base.UpdateStat();
			if (this.keyword.step >= 1)
			{
				if (!base.character.ability.Contains(this._firstAbilityComponent.ability))
				{
					base.character.ability.Add(this._firstAbilityComponent.ability);
				}
			}
			else
			{
				base.character.ability.Remove(this._firstAbilityComponent.ability);
			}
			if (this.keyword.isMaxStep)
			{
				if (!base.character.ability.Contains(this._maxAbilityComponent.ability))
				{
					base.character.ability.Add(this._maxAbilityComponent.ability);
					return;
				}
			}
			else
			{
				base.character.ability.Remove(this._maxAbilityComponent.ability);
			}
		}

		// Token: 0x06002F0E RID: 12046 RVA: 0x0008D3F3 File Offset: 0x0008B5F3
		public override void Attach()
		{
			base.Attach();
			Achievement.Type.TerribleOmen.Set();
		}

		// Token: 0x06002F0F RID: 12047 RVA: 0x0008D402 File Offset: 0x0008B602
		public override void Detach()
		{
			base.Detach();
			base.character.ability.Remove(this._firstAbilityComponent.ability);
			base.character.ability.Remove(this._maxAbilityComponent.ability);
		}

		// Token: 0x040026EE RID: 9966
		[SerializeField]
		private double[] _statBonusByStep;

		// Token: 0x040026EF RID: 9967
		[Header("1세트 효과")]
		[AbilityComponent.SubcomponentAttribute]
		[SerializeField]
		private AbilityComponent _firstAbilityComponent;

		// Token: 0x040026F0 RID: 9968
		[SerializeField]
		[AbilityComponent.SubcomponentAttribute]
		[Header("3세트 효과")]
		private AbilityComponent _maxAbilityComponent;
	}
}
