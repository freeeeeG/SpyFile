using System;
using UnityEngine;

namespace Characters.Abilities.CharacterStat
{
	// Token: 0x02000C56 RID: 3158
	[Serializable]
	public class StatBonusByMaxHealth : Ability
	{
		// Token: 0x060040AA RID: 16554 RVA: 0x000BBEFB File Offset: 0x000BA0FB
		public override IAbilityInstance CreateInstance(Character owner)
		{
			return new StatBonusByMaxHealth.Instance(owner, this);
		}

		// Token: 0x040031AF RID: 12719
		[SerializeField]
		private Stat.Values _targetPerStack;

		// Token: 0x040031B0 RID: 12720
		[Range(0f, 1000f)]
		[SerializeField]
		private int _stackHealth;

		// Token: 0x040031B1 RID: 12721
		[SerializeField]
		private int _maxStack;

		// Token: 0x02000C57 RID: 3159
		public class Instance : AbilityInstance<StatBonusByMaxHealth>
		{
			// Token: 0x17000DA2 RID: 3490
			// (get) Token: 0x060040AC RID: 16556 RVA: 0x000BBF04 File Offset: 0x000BA104
			public override int iconStacks
			{
				get
				{
					return this._stack;
				}
			}

			// Token: 0x17000DA3 RID: 3491
			// (get) Token: 0x060040AD RID: 16557 RVA: 0x00071719 File Offset: 0x0006F919
			public override float iconFillAmount
			{
				get
				{
					return 0f;
				}
			}

			// Token: 0x060040AE RID: 16558 RVA: 0x000BBF0C File Offset: 0x000BA10C
			public Instance(Character owner, StatBonusByMaxHealth ability) : base(owner, ability)
			{
				this._stat = ability._targetPerStack.Clone();
			}

			// Token: 0x060040AF RID: 16559 RVA: 0x000BBF27 File Offset: 0x000BA127
			protected override void OnAttach()
			{
				this.owner.stat.AttachValues(this._stat);
				this.UpdateStat();
			}

			// Token: 0x060040B0 RID: 16560 RVA: 0x000BBF45 File Offset: 0x000BA145
			protected override void OnDetach()
			{
				this.owner.stat.DetachValues(this._stat);
			}

			// Token: 0x060040B1 RID: 16561 RVA: 0x000BBF5D File Offset: 0x000BA15D
			public override void UpdateTime(float deltaTime)
			{
				base.UpdateTime(deltaTime);
				this._remainUpdateTime -= deltaTime;
				if (this._remainUpdateTime < 0f)
				{
					this._remainUpdateTime = 0.2f;
					this.UpdateStat();
				}
			}

			// Token: 0x060040B2 RID: 16562 RVA: 0x000BBF94 File Offset: 0x000BA194
			public void UpdateStat()
			{
				int num = (int)(this.owner.health.maximumHealth / (double)this.ability._stackHealth);
				if (num == this._stack)
				{
					return;
				}
				this._stack = Mathf.Min(this.ability._maxStack, num);
				this.SetStat(this._stack);
			}

			// Token: 0x060040B3 RID: 16563 RVA: 0x000BBFF0 File Offset: 0x000BA1F0
			private void SetStat(int stack)
			{
				Stat.Value[] values = this._stat.values;
				for (int i = 0; i < values.Length; i++)
				{
					values[i].value = this.ability._targetPerStack.values[i].value * (double)stack;
				}
				this.owner.stat.SetNeedUpdate();
			}

			// Token: 0x040031B2 RID: 12722
			private const float _updateInterval = 0.2f;

			// Token: 0x040031B3 RID: 12723
			private float _remainUpdateTime;

			// Token: 0x040031B4 RID: 12724
			private Stat.Values _stat;

			// Token: 0x040031B5 RID: 12725
			private int _stack;
		}
	}
}
