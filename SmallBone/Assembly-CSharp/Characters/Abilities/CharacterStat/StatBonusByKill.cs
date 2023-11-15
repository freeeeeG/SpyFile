using System;
using UnityEngine;

namespace Characters.Abilities.CharacterStat
{
	// Token: 0x02000C53 RID: 3155
	[Serializable]
	public class StatBonusByKill : Ability
	{
		// Token: 0x17000D9D RID: 3485
		// (get) Token: 0x06004097 RID: 16535 RVA: 0x000BBB87 File Offset: 0x000B9D87
		// (set) Token: 0x06004098 RID: 16536 RVA: 0x000BBB8F File Offset: 0x000B9D8F
		public int stack
		{
			get
			{
				return this._stack;
			}
			set
			{
				this._stack = value;
				if (this._instance == null)
				{
					Debug.Log("instance null");
					return;
				}
				this._instance.UpdateStack();
			}
		}

		// Token: 0x06004099 RID: 16537 RVA: 0x000BBBB6 File Offset: 0x000B9DB6
		public override void Initialize()
		{
			base.Initialize();
			if (this._maxStack == 0)
			{
				this._maxStack = int.MaxValue;
			}
		}

		// Token: 0x0600409A RID: 16538 RVA: 0x000BBBD4 File Offset: 0x000B9DD4
		public override IAbilityInstance CreateInstance(Character owner)
		{
			return this._instance = new StatBonusByKill.Instance(owner, this);
		}

		// Token: 0x040031A2 RID: 12706
		[SerializeField]
		[Tooltip("비어있지 않을 경우, 해당 키를 가진 공격에만 발동됨")]
		private string _attackKey;

		// Token: 0x040031A3 RID: 12707
		[SerializeField]
		private MotionTypeBoolArray _motionTypeFilter;

		// Token: 0x040031A4 RID: 12708
		[SerializeField]
		private AttackTypeBoolArray _attackTypeFilter;

		// Token: 0x040031A5 RID: 12709
		[SerializeField]
		private CharacterTypeBoolArray _characterTypeFilter = new CharacterTypeBoolArray(new bool[]
		{
			true,
			true,
			true,
			true,
			true,
			false,
			false,
			false
		});

		// Token: 0x040031A6 RID: 12710
		[Space]
		[SerializeField]
		private bool _permanenet;

		// Token: 0x040031A7 RID: 12711
		[SerializeField]
		private int _maxStack;

		// Token: 0x040031A8 RID: 12712
		[SerializeField]
		[Tooltip("스택이 쌓일 때마다 남은 시간을 초기화할지")]
		private bool _refreshRemainTime = true;

		// Token: 0x040031A9 RID: 12713
		[SerializeField]
		[Tooltip("실제 스택 1개당 아이콘 상에 표시할 스택")]
		private float _iconStacksPerStack = 1f;

		// Token: 0x040031AA RID: 12714
		[SerializeField]
		private Stat.Values _statPerStack;

		// Token: 0x040031AB RID: 12715
		private StatBonusByKill.Instance _instance;

		// Token: 0x040031AC RID: 12716
		private int _stack;

		// Token: 0x02000C54 RID: 3156
		public class Instance : AbilityInstance<StatBonusByKill>
		{
			// Token: 0x17000D9E RID: 3486
			// (get) Token: 0x0600409C RID: 16540 RVA: 0x000BBC27 File Offset: 0x000B9E27
			// (set) Token: 0x0600409D RID: 16541 RVA: 0x000BBC48 File Offset: 0x000B9E48
			private int stacks
			{
				get
				{
					if (!this.ability._permanenet)
					{
						return this._stacks;
					}
					return this.ability._stack;
				}
				set
				{
					if (this.ability._permanenet)
					{
						this.ability._stack = value;
						return;
					}
					this._stacks = value;
				}
			}

			// Token: 0x17000D9F RID: 3487
			// (get) Token: 0x0600409E RID: 16542 RVA: 0x000BBC6B File Offset: 0x000B9E6B
			public override int iconStacks
			{
				get
				{
					return (int)((float)this.stacks * this.ability._iconStacksPerStack);
				}
			}

			// Token: 0x17000DA0 RID: 3488
			// (get) Token: 0x0600409F RID: 16543 RVA: 0x000B8C5E File Offset: 0x000B6E5E
			public override Sprite icon
			{
				get
				{
					if (this.iconStacks != 0)
					{
						return base.icon;
					}
					return null;
				}
			}

			// Token: 0x060040A0 RID: 16544 RVA: 0x000BBC81 File Offset: 0x000B9E81
			public Instance(Character owner, StatBonusByKill ability) : base(owner, ability)
			{
			}

			// Token: 0x060040A1 RID: 16545 RVA: 0x000BBC8C File Offset: 0x000B9E8C
			protected override void OnAttach()
			{
				this._stat = this.ability._statPerStack.Clone();
				Character owner = this.owner;
				owner.onKilled = (Character.OnKilledDelegate)Delegate.Combine(owner.onKilled, new Character.OnKilledDelegate(this.OnCharacterKilled));
				this.owner.stat.AttachValues(this._stat);
				this.UpdateStack();
			}

			// Token: 0x060040A2 RID: 16546 RVA: 0x000BBCF2 File Offset: 0x000B9EF2
			protected override void OnDetach()
			{
				Character owner = this.owner;
				owner.onKilled = (Character.OnKilledDelegate)Delegate.Remove(owner.onKilled, new Character.OnKilledDelegate(this.OnCharacterKilled));
				this.owner.stat.DetachValues(this._stat);
			}

			// Token: 0x060040A3 RID: 16547 RVA: 0x000BBD31 File Offset: 0x000B9F31
			public override void Refresh()
			{
				if (this.ability._refreshRemainTime)
				{
					base.Refresh();
				}
			}

			// Token: 0x060040A4 RID: 16548 RVA: 0x000BBD48 File Offset: 0x000B9F48
			private void AddStack()
			{
				if (this.stacks >= this.ability._maxStack)
				{
					return;
				}
				int stacks = this.stacks;
				this.stacks = stacks + 1;
				this.UpdateStack();
			}

			// Token: 0x060040A5 RID: 16549 RVA: 0x000BBD80 File Offset: 0x000B9F80
			public void UpdateStack()
			{
				for (int i = 0; i < this._stat.values.Length; i++)
				{
					this._stat.values[i].value = this.ability._statPerStack.values[i].GetStackedValue((double)this.stacks);
				}
				this.owner.stat.SetNeedUpdate();
			}

			// Token: 0x060040A6 RID: 16550 RVA: 0x000BBDE8 File Offset: 0x000B9FE8
			private void OnCharacterKilled(ITarget target, ref Damage damage)
			{
				Character character = target.character;
				if (character == null)
				{
					return;
				}
				if (!this.ability._characterTypeFilter[target.character.type])
				{
					return;
				}
				if (this.ability._characterTypeFilter[Character.Type.Boss] && character.type == Character.Type.Boss && (target.character.key == Key.FirstHero1 || target.character.key == Key.FirstHero2 || target.character.key == Key.Unspecified))
				{
					return;
				}
				if (!this.ability._motionTypeFilter[damage.motionType])
				{
					return;
				}
				if (!this.ability._attackTypeFilter[damage.attackType])
				{
					return;
				}
				if (!string.IsNullOrWhiteSpace(this.ability._attackKey) && !damage.key.Equals(this.ability._attackKey, StringComparison.OrdinalIgnoreCase))
				{
					return;
				}
				this.AddStack();
			}

			// Token: 0x040031AD RID: 12717
			private int _stacks;

			// Token: 0x040031AE RID: 12718
			private Stat.Values _stat;
		}
	}
}
