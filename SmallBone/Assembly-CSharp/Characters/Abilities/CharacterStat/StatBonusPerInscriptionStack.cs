using System;
using Characters.Gear.Synergy;
using Characters.Gear.Synergy.Inscriptions;
using Services;
using Singletons;
using UnityEngine;

namespace Characters.Abilities.CharacterStat
{
	// Token: 0x02000C79 RID: 3193
	[Serializable]
	public class StatBonusPerInscriptionStack : Ability
	{
		// Token: 0x06004135 RID: 16693 RVA: 0x000BD636 File Offset: 0x000BB836
		public override void Initialize()
		{
			base.Initialize();
			if (this._maxStack == 0)
			{
				this._maxStack = int.MaxValue;
			}
		}

		// Token: 0x06004136 RID: 16694 RVA: 0x000BD651 File Offset: 0x000BB851
		public override IAbilityInstance CreateInstance(Character owner)
		{
			return new StatBonusPerInscriptionStack.Instance(owner, this);
		}

		// Token: 0x040031FC RID: 12796
		[SerializeField]
		private StatBonusPerInscriptionStack.Comparer _comparer;

		// Token: 0x040031FD RID: 12797
		[Header("비교 대상 숫자")]
		[SerializeField]
		private int _inscriptionLevelForStackCounting = 1;

		// Token: 0x040031FE RID: 12798
		[SerializeField]
		private int _maxStack;

		// Token: 0x040031FF RID: 12799
		[SerializeField]
		private Stat.Values _statPerStack;

		// Token: 0x02000C7A RID: 3194
		public class Instance : AbilityInstance<StatBonusPerInscriptionStack>
		{
			// Token: 0x17000DBB RID: 3515
			// (get) Token: 0x06004138 RID: 16696 RVA: 0x000BD669 File Offset: 0x000BB869
			public override int iconStacks
			{
				get
				{
					return this._stack;
				}
			}

			// Token: 0x06004139 RID: 16697 RVA: 0x000BD671 File Offset: 0x000BB871
			public Instance(Character owner, StatBonusPerInscriptionStack ability) : base(owner, ability)
			{
			}

			// Token: 0x0600413A RID: 16698 RVA: 0x000BD67C File Offset: 0x000BB87C
			protected override void OnAttach()
			{
				this._stat = this.ability._statPerStack.Clone();
				Singleton<Service>.Instance.levelManager.player.playerComponents.inventory.onUpdatedKeywordCounts += this.UpdateStack;
				this.owner.stat.AttachValues(this._stat);
				this.UpdateStack();
			}

			// Token: 0x0600413B RID: 16699 RVA: 0x000BD6E8 File Offset: 0x000BB8E8
			protected override void OnDetach()
			{
				Singleton<Service>.Instance.levelManager.player.playerComponents.inventory.onUpdatedKeywordCounts -= this.UpdateStack;
				this.owner.stat.DetachValues(this._stat);
			}

			// Token: 0x0600413C RID: 16700 RVA: 0x000BD738 File Offset: 0x000BB938
			public void UpdateStack()
			{
				Synergy synergy = Singleton<Service>.Instance.levelManager.player.playerComponents.inventory.synergy;
				this._stack = 0;
				foreach (Inscription inscription in synergy.inscriptions)
				{
					int count = inscription.count;
					switch (this.ability._comparer)
					{
					case StatBonusPerInscriptionStack.Comparer.LessThan:
						if (count < this.ability._inscriptionLevelForStackCounting)
						{
							this._stack++;
						}
						break;
					case StatBonusPerInscriptionStack.Comparer.LessThanOrEqualTo:
						if (count <= this.ability._inscriptionLevelForStackCounting)
						{
							this._stack++;
						}
						break;
					case StatBonusPerInscriptionStack.Comparer.EqualTo:
						if (count == this.ability._inscriptionLevelForStackCounting)
						{
							this._stack++;
						}
						break;
					case StatBonusPerInscriptionStack.Comparer.NotEqualTo:
						if (count != this.ability._inscriptionLevelForStackCounting)
						{
							this._stack++;
						}
						break;
					case StatBonusPerInscriptionStack.Comparer.GreaterThanOrEqualTo:
						if (count >= this.ability._inscriptionLevelForStackCounting)
						{
							this._stack++;
						}
						break;
					case StatBonusPerInscriptionStack.Comparer.GreaterThan:
						if (count > this.ability._inscriptionLevelForStackCounting)
						{
							this._stack++;
						}
						break;
					}
				}
				this._stack = Mathf.Min(this._stack, this.ability._maxStack);
				for (int i = 0; i < this._stat.values.Length; i++)
				{
					this._stat.values[i].value = this.ability._statPerStack.values[i].GetStackedValue((double)this._stack);
				}
				this.owner.stat.SetNeedUpdate();
			}

			// Token: 0x04003200 RID: 12800
			private int _stack;

			// Token: 0x04003201 RID: 12801
			private Stat.Values _stat;
		}

		// Token: 0x02000C7B RID: 3195
		private enum Comparer
		{
			// Token: 0x04003203 RID: 12803
			LessThan,
			// Token: 0x04003204 RID: 12804
			LessThanOrEqualTo,
			// Token: 0x04003205 RID: 12805
			EqualTo,
			// Token: 0x04003206 RID: 12806
			NotEqualTo,
			// Token: 0x04003207 RID: 12807
			GreaterThanOrEqualTo,
			// Token: 0x04003208 RID: 12808
			GreaterThan
		}
	}
}
