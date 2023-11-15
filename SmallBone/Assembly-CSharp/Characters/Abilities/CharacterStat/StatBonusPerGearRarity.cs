using System;
using System.Collections;
using Characters.Operations;
using UnityEditor;
using UnityEngine;

namespace Characters.Abilities.CharacterStat
{
	// Token: 0x02000C6F RID: 3183
	[Serializable]
	public class StatBonusPerGearRarity : Ability
	{
		// Token: 0x17000DAF RID: 3503
		// (get) Token: 0x06004102 RID: 16642 RVA: 0x000BCE22 File Offset: 0x000BB022
		// (set) Token: 0x06004103 RID: 16643 RVA: 0x000BCE2A File Offset: 0x000BB02A
		public int count { get; set; }

		// Token: 0x06004104 RID: 16644 RVA: 0x00089C49 File Offset: 0x00087E49
		public StatBonusPerGearRarity()
		{
		}

		// Token: 0x06004105 RID: 16645 RVA: 0x000BCE33 File Offset: 0x000BB033
		public StatBonusPerGearRarity(Stat.Values stat)
		{
			this._statPerGearTag = stat;
		}

		// Token: 0x06004106 RID: 16646 RVA: 0x000BCE42 File Offset: 0x000BB042
		public override void Initialize()
		{
			base.Initialize();
			this._onMaxStack.Initialize();
		}

		// Token: 0x06004107 RID: 16647 RVA: 0x000BCE55 File Offset: 0x000BB055
		public override IAbilityInstance CreateInstance(Character owner)
		{
			return new StatBonusPerGearRarity.Instance(owner, this);
		}

		// Token: 0x040031DE RID: 12766
		[SerializeField]
		private StatBonusPerGearRarityComponent _component;

		// Token: 0x040031DF RID: 12767
		[SerializeField]
		private Rarity _rarity;

		// Token: 0x040031E0 RID: 12768
		[SerializeField]
		private Stat.Values _statPerGearTag;

		// Token: 0x040031E1 RID: 12769
		[SerializeField]
		private int _maxStack;

		// Token: 0x040031E2 RID: 12770
		[SerializeField]
		private int _operationRunCount;

		// Token: 0x040031E3 RID: 12771
		[Subcomponent(typeof(OperationInfo))]
		[SerializeField]
		private OperationInfo.Subcomponents _onMaxStack;

		// Token: 0x02000C70 RID: 3184
		public class Instance : AbilityInstance<StatBonusPerGearRarity>
		{
			// Token: 0x17000DB0 RID: 3504
			// (get) Token: 0x06004108 RID: 16648 RVA: 0x000BCE5E File Offset: 0x000BB05E
			public override int iconStacks
			{
				get
				{
					return this._stack;
				}
			}

			// Token: 0x06004109 RID: 16649 RVA: 0x000BCE66 File Offset: 0x000BB066
			public Instance(Character owner, StatBonusPerGearRarity ability) : base(owner, ability)
			{
			}

			// Token: 0x0600410A RID: 16650 RVA: 0x000BCE70 File Offset: 0x000BB070
			protected override void OnAttach()
			{
				this._stat = this.ability._statPerGearTag.Clone();
				this.owner.stat.AttachValues(this._stat);
				this.owner.StartCoroutine(this.CWaitForLoadAndUpdateStack());
				this.owner.playerComponents.inventory.onUpdated += this.UpdateStatBonus;
			}

			// Token: 0x0600410B RID: 16651 RVA: 0x000BCEDC File Offset: 0x000BB0DC
			protected override void OnDetach()
			{
				this.owner.playerComponents.inventory.onUpdated -= this.UpdateStatBonus;
				this.owner.stat.DetachValues(this._stat);
			}

			// Token: 0x0600410C RID: 16652 RVA: 0x000BCF18 File Offset: 0x000BB118
			private void UpdateStatBonus()
			{
				this.UpdateStack();
				for (int i = 0; i < this._stat.values.Length; i++)
				{
					this._stat.values[i].value = this.ability._statPerGearTag.values[i].GetStackedValue((double)this._stack);
				}
				this.owner.stat.SetNeedUpdate();
			}

			// Token: 0x0600410D RID: 16653 RVA: 0x000BCF83 File Offset: 0x000BB183
			private IEnumerator CWaitForLoadAndUpdateStack()
			{
				yield return null;
				this.ability._component.loaded = true;
				this.UpdateStatBonus();
				yield break;
			}

			// Token: 0x0600410E RID: 16654 RVA: 0x000BCF94 File Offset: 0x000BB194
			private void UpdateStack()
			{
				if (!this.ability._component.loaded)
				{
					return;
				}
				int itemCountByRarity = this.owner.playerComponents.inventory.item.GetItemCountByRarity(this.ability._rarity);
				int countByRarity = this.owner.playerComponents.inventory.weapon.GetCountByRarity(this.ability._rarity);
				int countByRarity2 = this.owner.playerComponents.inventory.quintessence.GetCountByRarity(this.ability._rarity);
				this._stack = Mathf.Min(this.ability._maxStack, itemCountByRarity + countByRarity + countByRarity2);
				if (this.ability._component.stack < (float)this.ability._operationRunCount && this._stack >= this.ability._maxStack)
				{
					this.owner.StartCoroutine(this.ability._onMaxStack.CRun(this.owner));
					this.ability._component.stack += 1f;
				}
			}

			// Token: 0x040031E5 RID: 12773
			private int _stack;

			// Token: 0x040031E6 RID: 12774
			private Stat.Values _stat;
		}
	}
}
