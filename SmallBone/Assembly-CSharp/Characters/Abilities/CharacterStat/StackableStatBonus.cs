using System;
using Unity.Mathematics;
using UnityEngine;

namespace Characters.Abilities.CharacterStat
{
	// Token: 0x02000C33 RID: 3123
	[Serializable]
	public class StackableStatBonus : Ability
	{
		// Token: 0x17000D8B RID: 3467
		// (get) Token: 0x0600401B RID: 16411 RVA: 0x000BA1CC File Offset: 0x000B83CC
		// (set) Token: 0x0600401C RID: 16412 RVA: 0x000BA1D4 File Offset: 0x000B83D4
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

		// Token: 0x17000D8C RID: 3468
		// (get) Token: 0x0600401D RID: 16413 RVA: 0x000BA1FC File Offset: 0x000B83FC
		public int maxStack
		{
			get
			{
				return this._maxStack;
			}
		}

		// Token: 0x0600401E RID: 16414 RVA: 0x000BA204 File Offset: 0x000B8404
		public override void Initialize()
		{
			base.Initialize();
			if (this._maxStack == 0)
			{
				this._maxStack = int.MaxValue;
			}
		}

		// Token: 0x0600401F RID: 16415 RVA: 0x000BA220 File Offset: 0x000B8420
		public override IAbilityInstance CreateInstance(Character owner)
		{
			return this._instance = new StackableStatBonus.Instance(owner, this);
		}

		// Token: 0x0400314E RID: 12622
		[SerializeField]
		[Tooltip("다시 획득할 경우 스택을 초기화할지")]
		private bool _resetOnAttach = true;

		// Token: 0x0400314F RID: 12623
		[SerializeField]
		private int _maxStack;

		// Token: 0x04003150 RID: 12624
		[SerializeField]
		[Tooltip("스택이 쌓일 때마다 남은 시간을 초기화할지")]
		private bool _refreshRemainTime = true;

		// Token: 0x04003151 RID: 12625
		[Tooltip("실제 스택 1개당 아이콘 상에 표시할 스택")]
		[SerializeField]
		private float _iconStacksPerStack = 1f;

		// Token: 0x04003152 RID: 12626
		[SerializeField]
		private Stat.Values _statPerStack;

		// Token: 0x04003153 RID: 12627
		private StackableStatBonus.Instance _instance;

		// Token: 0x04003154 RID: 12628
		private int _stack;

		// Token: 0x02000C34 RID: 3124
		public class Instance : AbilityInstance<StackableStatBonus>
		{
			// Token: 0x17000D8D RID: 3469
			// (get) Token: 0x06004021 RID: 16417 RVA: 0x000BA25E File Offset: 0x000B845E
			public override int iconStacks
			{
				get
				{
					return (int)((float)this.ability.stack * this.ability._iconStacksPerStack);
				}
			}

			// Token: 0x06004022 RID: 16418 RVA: 0x000BA279 File Offset: 0x000B8479
			public Instance(Character owner, StackableStatBonus ability) : base(owner, ability)
			{
			}

			// Token: 0x06004023 RID: 16419 RVA: 0x000BA284 File Offset: 0x000B8484
			protected override void OnAttach()
			{
				this._stat = this.ability._statPerStack.Clone();
				this.owner.stat.AttachValues(this._stat);
				if (this.ability._resetOnAttach)
				{
					this.ability.stack = 1;
				}
			}

			// Token: 0x06004024 RID: 16420 RVA: 0x000BA2D6 File Offset: 0x000B84D6
			protected override void OnDetach()
			{
				this.owner.stat.DetachValues(this._stat);
			}

			// Token: 0x06004025 RID: 16421 RVA: 0x000BA2F0 File Offset: 0x000B84F0
			public override void Refresh()
			{
				if (this.ability._refreshRemainTime)
				{
					base.Refresh();
				}
				StackableStatBonus ability = this.ability;
				int stack = ability.stack;
				ability.stack = stack + 1;
			}

			// Token: 0x06004026 RID: 16422 RVA: 0x000BA328 File Offset: 0x000B8528
			public void UpdateStack()
			{
				for (int i = 0; i < this._stat.values.Length; i++)
				{
					this._stat.values[i].value = this.ability._statPerStack.values[i].GetStackedValue((double)this.ability.stack);
				}
				this.owner.stat.SetNeedUpdate();
			}

			// Token: 0x04003155 RID: 12629
			private Stat.Values _stat;
		}
	}
}
