using System;
using Characters.Usables;
using UnityEngine;

namespace Characters.Abilities.Weapons.EntSkul
{
	// Token: 0x02000C15 RID: 3093
	[Serializable]
	public class StatBonusByEntGrassCount : Ability
	{
		// Token: 0x06003F8D RID: 16269 RVA: 0x000B86CD File Offset: 0x000B68CD
		public override IAbilityInstance CreateInstance(Character owner)
		{
			return new StatBonusByEntGrassCount.Instance(owner, this);
		}

		// Token: 0x040030E8 RID: 12520
		[SerializeField]
		private LiquidMaster _liquidMaster;

		// Token: 0x040030E9 RID: 12521
		[SerializeField]
		private int _maxStack;

		// Token: 0x040030EA RID: 12522
		[Tooltip("잔디 갯수에 이 값을 곱한 숫자가 스택이 됨")]
		[SerializeField]
		private double _stackMultiplier = 1.0;

		// Token: 0x040030EB RID: 12523
		[SerializeField]
		[Tooltip("실제 스택 1개당 아이콘 상에 표시할 스택")]
		private double _iconStacksPerStack = 1.0;

		// Token: 0x040030EC RID: 12524
		[SerializeField]
		private Stat.Values _statPerStack;

		// Token: 0x02000C16 RID: 3094
		public class Instance : AbilityInstance<StatBonusByEntGrassCount>
		{
			// Token: 0x17000D72 RID: 3442
			// (get) Token: 0x06003F8F RID: 16271 RVA: 0x000B86FC File Offset: 0x000B68FC
			public override Sprite icon
			{
				get
				{
					if (this._iconStacks <= 0)
					{
						return null;
					}
					return this.ability.defaultIcon;
				}
			}

			// Token: 0x17000D73 RID: 3443
			// (get) Token: 0x06003F90 RID: 16272 RVA: 0x000B8714 File Offset: 0x000B6914
			public override int iconStacks
			{
				get
				{
					return this._iconStacks;
				}
			}

			// Token: 0x06003F91 RID: 16273 RVA: 0x000B871C File Offset: 0x000B691C
			public Instance(Character owner, StatBonusByEntGrassCount ability) : base(owner, ability)
			{
				this._stat = ability._statPerStack.Clone();
			}

			// Token: 0x06003F92 RID: 16274 RVA: 0x000B8737 File Offset: 0x000B6937
			protected override void OnAttach()
			{
				this.owner.stat.AttachValues(this._stat);
				this.ability._liquidMaster.onChanged += this.UpdateStat;
				this.UpdateStat();
			}

			// Token: 0x06003F93 RID: 16275 RVA: 0x000B8771 File Offset: 0x000B6971
			protected override void OnDetach()
			{
				this.owner.stat.DetachValues(this._stat);
				this.ability._liquidMaster.onChanged -= this.UpdateStat;
			}

			// Token: 0x06003F94 RID: 16276 RVA: 0x000B87A8 File Offset: 0x000B69A8
			public void UpdateStat()
			{
				double num = (double)this.ability._liquidMaster.GetStack() * this.ability._stackMultiplier;
				this._iconStacks = (int)(num * this.ability._iconStacksPerStack);
				for (int i = 0; i < this._stat.values.Length; i++)
				{
					this._stat.values[i].value = this.ability._statPerStack.values[i].GetStackedValue(num);
				}
				this.owner.stat.SetNeedUpdate();
			}

			// Token: 0x040030ED RID: 12525
			private Stat.Values _stat;

			// Token: 0x040030EE RID: 12526
			private int _iconStacks;
		}
	}
}
