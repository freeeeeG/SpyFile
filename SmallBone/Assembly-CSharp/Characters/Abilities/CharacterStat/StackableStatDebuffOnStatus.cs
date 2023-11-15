using System;
using Unity.Mathematics;
using UnityEngine;

namespace Characters.Abilities.CharacterStat
{
	// Token: 0x02000C3C RID: 3132
	[Serializable]
	public class StackableStatDebuffOnStatus : Ability
	{
		// Token: 0x17000D94 RID: 3476
		// (get) Token: 0x06004041 RID: 16449 RVA: 0x000BA7F1 File Offset: 0x000B89F1
		// (set) Token: 0x06004042 RID: 16450 RVA: 0x000BA7F9 File Offset: 0x000B89F9
		public int stack
		{
			get
			{
				return this._stack;
			}
			set
			{
				this._stack = math.min(value, this._maxStack);
				if (this._instance == null)
				{
					return;
				}
				this._instance.UpdateStack();
			}
		}

		// Token: 0x06004043 RID: 16451 RVA: 0x000BA821 File Offset: 0x000B8A21
		public override void Initialize()
		{
			base.Initialize();
			if (this._maxStack == 0)
			{
				this._maxStack = int.MaxValue;
			}
		}

		// Token: 0x06004044 RID: 16452 RVA: 0x000BA83C File Offset: 0x000B8A3C
		public override IAbilityInstance CreateInstance(Character owner)
		{
			return this._instance = new StackableStatDebuffOnStatus.Instance(owner, this);
		}

		// Token: 0x04003165 RID: 12645
		[SerializeField]
		[Tooltip("다시 획득할 경우 스택을 초기화할지")]
		private bool _resetOnAttach = true;

		// Token: 0x04003166 RID: 12646
		[SerializeField]
		private int _maxStack;

		// Token: 0x04003167 RID: 12647
		[Tooltip("스택이 쌓일 때마다 남은 시간을 초기화할지")]
		[SerializeField]
		private bool _refreshRemainTime = true;

		// Token: 0x04003168 RID: 12648
		[SerializeField]
		[Tooltip("실제 스택 1개당 아이콘 상에 표시할 스택")]
		private float _iconStacksPerStack = 1f;

		// Token: 0x04003169 RID: 12649
		[SerializeField]
		private bool _onRelease;

		// Token: 0x0400316A RID: 12650
		[SerializeField]
		internal CharacterStatus.Kind _kind;

		// Token: 0x0400316B RID: 12651
		[SerializeField]
		private Stat.Values _statPerStack;

		// Token: 0x0400316C RID: 12652
		[SerializeField]
		private Stat.Values _maxStackBounsStat;

		// Token: 0x0400316D RID: 12653
		private StackableStatDebuffOnStatus.Instance _instance;

		// Token: 0x0400316E RID: 12654
		private int _stack;

		// Token: 0x02000C3D RID: 3133
		public class Instance : AbilityInstance<StackableStatDebuffOnStatus>
		{
			// Token: 0x17000D95 RID: 3477
			// (get) Token: 0x06004046 RID: 16454 RVA: 0x000BA87A File Offset: 0x000B8A7A
			public override int iconStacks
			{
				get
				{
					return (int)((float)this.ability.stack * this.ability._iconStacksPerStack);
				}
			}

			// Token: 0x06004047 RID: 16455 RVA: 0x000BA895 File Offset: 0x000B8A95
			public Instance(Character owner, StackableStatDebuffOnStatus ability) : base(owner, ability)
			{
			}

			// Token: 0x06004048 RID: 16456 RVA: 0x000BA8A0 File Offset: 0x000B8AA0
			protected override void OnAttach()
			{
				this._stat = this.ability._statPerStack.Clone();
				this.owner.stat.AttachValues(this._stat);
				if (this.ability._resetOnAttach)
				{
					this.ability.stack = 1;
				}
				this.AttachTrigger();
			}

			// Token: 0x06004049 RID: 16457 RVA: 0x000BA8F8 File Offset: 0x000B8AF8
			protected override void OnDetach()
			{
				this.DetachTrigger();
				this.owner.stat.DetachValues(this._stat);
				this.owner.stat.DetachValues(this.ability._maxStackBounsStat);
			}

			// Token: 0x0600404A RID: 16458 RVA: 0x000BA934 File Offset: 0x000B8B34
			public void UpdateStack()
			{
				for (int i = 0; i < this._stat.values.Length; i++)
				{
					this._stat.values[i].value = this.ability._statPerStack.values[i].GetStackedValue((double)this.ability.stack);
				}
				if (this.ability.stack == this.ability._maxStack)
				{
					this.owner.stat.AttachValues(this.ability._maxStackBounsStat);
				}
				else
				{
					this.owner.stat.DetachValues(this.ability._maxStackBounsStat);
				}
				this.owner.stat.SetNeedUpdate();
			}

			// Token: 0x0600404B RID: 16459 RVA: 0x000BA9F0 File Offset: 0x000B8BF0
			private void AttachTrigger()
			{
				switch (this.ability._kind)
				{
				case CharacterStatus.Kind.Stun:
					if (!this.ability._onRelease)
					{
						this.owner.status.onApplyStun += this.IncreaseStack;
						return;
					}
					this.owner.status.onReleaseStun += this.IncreaseStack;
					return;
				case CharacterStatus.Kind.Freeze:
					if (!this.ability._onRelease)
					{
						this.owner.status.onApplyFreeze += this.IncreaseStack;
						return;
					}
					this.owner.status.onReleaseFreeze += this.IncreaseStack;
					return;
				case CharacterStatus.Kind.Burn:
					if (!this.ability._onRelease)
					{
						this.owner.status.onApplyBurn += this.IncreaseStack;
						return;
					}
					this.owner.status.onReleaseBurn += this.IncreaseStack;
					return;
				case CharacterStatus.Kind.Wound:
					if (!this.ability._onRelease)
					{
						this.owner.status.onApplyWound += this.IncreaseStack;
						return;
					}
					this.owner.status.onApplyBleed += this.IncreaseStack;
					return;
				case CharacterStatus.Kind.Poison:
					if (!this.ability._onRelease)
					{
						this.owner.status.onApplyPoison += this.IncreaseStack;
						return;
					}
					this.owner.status.onReleasePoison += this.IncreaseStack;
					return;
				default:
					return;
				}
			}

			// Token: 0x0600404C RID: 16460 RVA: 0x000BAB88 File Offset: 0x000B8D88
			private void DetachTrigger()
			{
				switch (this.ability._kind)
				{
				case CharacterStatus.Kind.Stun:
					if (!this.ability._onRelease)
					{
						this.owner.status.onApplyStun -= this.IncreaseStack;
						return;
					}
					this.owner.status.onReleasePoison -= this.IncreaseStack;
					return;
				case CharacterStatus.Kind.Freeze:
					if (!this.ability._onRelease)
					{
						this.owner.status.onApplyFreeze -= this.IncreaseStack;
						return;
					}
					this.owner.status.onReleaseFreeze -= this.IncreaseStack;
					return;
				case CharacterStatus.Kind.Burn:
					if (!this.ability._onRelease)
					{
						this.owner.status.onApplyBurn -= this.IncreaseStack;
						return;
					}
					this.owner.status.onReleaseBurn -= this.IncreaseStack;
					return;
				case CharacterStatus.Kind.Wound:
					if (!this.ability._onRelease)
					{
						this.owner.status.onApplyWound -= this.IncreaseStack;
						return;
					}
					this.owner.status.onApplyBleed -= this.IncreaseStack;
					return;
				case CharacterStatus.Kind.Poison:
					if (!this.ability._onRelease)
					{
						this.owner.status.onApplyPoison -= this.IncreaseStack;
						return;
					}
					this.owner.status.onReleasePoison -= this.IncreaseStack;
					return;
				default:
					return;
				}
			}

			// Token: 0x0600404D RID: 16461 RVA: 0x000BAD20 File Offset: 0x000B8F20
			private void IncreaseStack(Character owner, Character target)
			{
				StackableStatDebuffOnStatus ability = this.ability;
				int stack = ability.stack;
				ability.stack = stack + 1;
			}

			// Token: 0x0400316F RID: 12655
			private Stat.Values _stat;
		}
	}
}
