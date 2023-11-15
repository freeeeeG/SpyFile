using System;
using Data;
using UnityEngine;

namespace Characters.Abilities
{
	// Token: 0x02000A0A RID: 2570
	[Serializable]
	public class CurrencyBonus : Ability
	{
		// Token: 0x0600368E RID: 13966 RVA: 0x00089C49 File Offset: 0x00087E49
		public CurrencyBonus()
		{
		}

		// Token: 0x0600368F RID: 13967 RVA: 0x000A16FD File Offset: 0x0009F8FD
		public CurrencyBonus(GameData.Currency.Type type, float percentPoint)
		{
			this._type = type;
			this._percentPoint = percentPoint;
		}

		// Token: 0x06003690 RID: 13968 RVA: 0x000A1713 File Offset: 0x0009F913
		public override IAbilityInstance CreateInstance(Character owner)
		{
			return new CurrencyBonus.Instance(owner, this);
		}

		// Token: 0x04002BAE RID: 11182
		[SerializeField]
		private GameData.Currency.Type _type;

		// Token: 0x04002BAF RID: 11183
		[SerializeField]
		private float _percentPoint;

		// Token: 0x02000A0B RID: 2571
		public class Instance : AbilityInstance<CurrencyBonus>
		{
			// Token: 0x06003691 RID: 13969 RVA: 0x000A171C File Offset: 0x0009F91C
			public Instance(Character owner, CurrencyBonus ability) : base(owner, ability)
			{
			}

			// Token: 0x06003692 RID: 13970 RVA: 0x000A1726 File Offset: 0x0009F926
			protected override void OnAttach()
			{
				GameData.Currency.currencies[this.ability._type].multiplier.AddOrUpdate(this, (double)this.ability._percentPoint);
			}

			// Token: 0x06003693 RID: 13971 RVA: 0x000A1754 File Offset: 0x0009F954
			protected override void OnDetach()
			{
				GameData.Currency.currencies[this.ability._type].multiplier.Remove(this);
			}
		}
	}
}
