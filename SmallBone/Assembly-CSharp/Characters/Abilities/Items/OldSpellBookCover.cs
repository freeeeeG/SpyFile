using System;
using System.Collections.Generic;
using Characters.Operations;
using UnityEngine;
using Utils;

namespace Characters.Abilities.Items
{
	// Token: 0x02000CE0 RID: 3296
	[Serializable]
	public sealed class OldSpellBookCover : Ability
	{
		// Token: 0x17000DE2 RID: 3554
		// (get) Token: 0x060042B0 RID: 17072 RVA: 0x000C238C File Offset: 0x000C058C
		// (set) Token: 0x060042B1 RID: 17073 RVA: 0x000C2394 File Offset: 0x000C0594
		public int stack { get; set; }

		// Token: 0x060042B2 RID: 17074 RVA: 0x000C239D File Offset: 0x000C059D
		public override void Initialize()
		{
			base.Initialize();
			this._onIncreaseStack.Initialize();
			this._onUpgrade.Initialize();
		}

		// Token: 0x060042B3 RID: 17075 RVA: 0x000C23BB File Offset: 0x000C05BB
		public override IAbilityInstance CreateInstance(Character owner)
		{
			return new OldSpellBookCover.Instance(owner, this);
		}

		// Token: 0x04003306 RID: 13062
		[SerializeField]
		private PositionInfo _lastTargetPositionInfo;

		// Token: 0x04003307 RID: 13063
		[SerializeField]
		private Transform _lastTargetPoint;

		// Token: 0x04003308 RID: 13064
		[SerializeField]
		private MotionTypeBoolArray _motionTypeFilter;

		// Token: 0x04003309 RID: 13065
		[SerializeField]
		private CharacterTypeBoolArray _targetTypeFilter;

		// Token: 0x0400330A RID: 13066
		[SerializeField]
		private int _maxStackPerTarget;

		// Token: 0x0400330B RID: 13067
		[SerializeField]
		private int _stackForUpgrade;

		// Token: 0x0400330C RID: 13068
		[CharacterOperation.SubcomponentAttribute]
		[SerializeField]
		private CharacterOperation.Subcomponents _onIncreaseStack;

		// Token: 0x0400330D RID: 13069
		[SerializeField]
		[CharacterOperation.SubcomponentAttribute]
		private CharacterOperation.Subcomponents _onUpgrade;

		// Token: 0x02000CE1 RID: 3297
		public sealed class Instance : AbilityInstance<OldSpellBookCover>
		{
			// Token: 0x17000DE3 RID: 3555
			// (get) Token: 0x060042B5 RID: 17077 RVA: 0x000C23C4 File Offset: 0x000C05C4
			public override Sprite icon
			{
				get
				{
					if (this.ability.stack <= 0)
					{
						return null;
					}
					return base.icon;
				}
			}

			// Token: 0x17000DE4 RID: 3556
			// (get) Token: 0x060042B6 RID: 17078 RVA: 0x000C23DC File Offset: 0x000C05DC
			public override int iconStacks
			{
				get
				{
					return this.ability.stack;
				}
			}

			// Token: 0x060042B7 RID: 17079 RVA: 0x000C23E9 File Offset: 0x000C05E9
			public Instance(Character owner, OldSpellBookCover ability) : base(owner, ability)
			{
				this._history = new Dictionary<Character, int>();
			}

			// Token: 0x060042B8 RID: 17080 RVA: 0x000C23FE File Offset: 0x000C05FE
			protected override void OnAttach()
			{
				this.AttachTrigger();
			}

			// Token: 0x060042B9 RID: 17081 RVA: 0x000C2406 File Offset: 0x000C0606
			protected override void OnDetach()
			{
				this.DetachTrigger();
			}

			// Token: 0x060042BA RID: 17082 RVA: 0x000C240E File Offset: 0x000C060E
			private void AttachTrigger()
			{
				Character owner = this.owner;
				owner.onGaveDamage = (GaveDamageDelegate)Delegate.Combine(owner.onGaveDamage, new GaveDamageDelegate(this.HandleOnGaveDamage));
			}

			// Token: 0x060042BB RID: 17083 RVA: 0x000C2437 File Offset: 0x000C0637
			private void DetachTrigger()
			{
				Character owner = this.owner;
				owner.onGaveDamage = (GaveDamageDelegate)Delegate.Remove(owner.onGaveDamage, new GaveDamageDelegate(this.HandleOnGaveDamage));
			}

			// Token: 0x060042BC RID: 17084 RVA: 0x000C2460 File Offset: 0x000C0660
			private void HandleOnGaveDamage(ITarget target, in Damage originalDamage, in Damage gaveDamage, double damageDealt)
			{
				if (!this.ability._motionTypeFilter[gaveDamage.motionType])
				{
					return;
				}
				if (target.character == null)
				{
					return;
				}
				if (!this.ability._targetTypeFilter[target.character.type])
				{
					return;
				}
				if (target == null || target.character == null)
				{
					return;
				}
				this.Invoke(target.character);
			}

			// Token: 0x060042BD RID: 17085 RVA: 0x000C24D4 File Offset: 0x000C06D4
			private void Invoke(Character character)
			{
				if (!this._history.ContainsKey(character))
				{
					this._history.Add(character, 1);
					this.IncreaseStack(character);
					return;
				}
				int num = this._history[character];
				if (num >= this.ability._maxStackPerTarget)
				{
					return;
				}
				this._history[character] = num + 1;
				this.IncreaseStack(character);
			}

			// Token: 0x060042BE RID: 17086 RVA: 0x000C2538 File Offset: 0x000C0738
			private void IncreaseStack(Character target)
			{
				this.ability._lastTargetPositionInfo.Attach(target, this.ability._lastTargetPoint);
				this.ability._onIncreaseStack.Run(this.owner);
				OldSpellBookCover ability = this.ability;
				int stack = ability.stack;
				ability.stack = stack + 1;
				if (this.ability.stack >= this.ability._stackForUpgrade)
				{
					this.UpgradeItem();
				}
			}

			// Token: 0x060042BF RID: 17087 RVA: 0x000C25AA File Offset: 0x000C07AA
			private void UpgradeItem()
			{
				this.ability._onUpgrade.Run(this.owner);
			}

			// Token: 0x0400330F RID: 13071
			private const int initialStack = 1;

			// Token: 0x04003310 RID: 13072
			private IDictionary<Character, int> _history;
		}
	}
}
