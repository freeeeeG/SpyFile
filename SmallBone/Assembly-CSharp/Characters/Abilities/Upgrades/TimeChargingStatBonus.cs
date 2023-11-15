using System;
using UnityEngine;

namespace Characters.Abilities.Upgrades
{
	// Token: 0x02000B16 RID: 2838
	[Serializable]
	public sealed class TimeChargingStatBonus : Ability
	{
		// Token: 0x060039AB RID: 14763 RVA: 0x000AA4DA File Offset: 0x000A86DA
		public override IAbilityInstance CreateInstance(Character owner)
		{
			return new TimeChargingStatBonus.Instance(owner, this);
		}

		// Token: 0x04002DC2 RID: 11714
		private int _stack;

		// Token: 0x04002DC3 RID: 11715
		[SerializeField]
		private int _maxStack;

		// Token: 0x04002DC4 RID: 11716
		[SerializeField]
		private float _chargingTime;

		// Token: 0x04002DC5 RID: 11717
		[SerializeField]
		private float _resetTime;

		// Token: 0x04002DC6 RID: 11718
		[SerializeField]
		private float _detachTime;

		// Token: 0x04002DC7 RID: 11719
		[SerializeField]
		private MotionTypeBoolArray _motionFilter;

		// Token: 0x04002DC8 RID: 11720
		[SerializeField]
		private Stat.Values _statPerStack;

		// Token: 0x02000B17 RID: 2839
		public class Instance : AbilityInstance<TimeChargingStatBonus>
		{
			// Token: 0x17000BE0 RID: 3040
			// (get) Token: 0x060039AD RID: 14765 RVA: 0x000AA4E3 File Offset: 0x000A86E3
			public override int iconStacks
			{
				get
				{
					return this.ability._stack;
				}
			}

			// Token: 0x17000BE1 RID: 3041
			// (get) Token: 0x060039AE RID: 14766 RVA: 0x000AA4F0 File Offset: 0x000A86F0
			public override float iconFillAmount
			{
				get
				{
					if (this._remainDetachTime <= 0f || this._remainDetachTime >= this.ability._detachTime - this.ability._resetTime)
					{
						return base.iconFillAmount;
					}
					return 0f;
				}
			}

			// Token: 0x060039AF RID: 14767 RVA: 0x000AA52A File Offset: 0x000A872A
			public Instance(Character owner, TimeChargingStatBonus ability) : base(owner, ability)
			{
				this._stat = ability._statPerStack.Clone();
			}

			// Token: 0x060039B0 RID: 14768 RVA: 0x000AA548 File Offset: 0x000A8748
			protected override void OnAttach()
			{
				this.owner.stat.AttachValues(this._stat);
				Character owner = this.owner;
				owner.onGaveDamage = (GaveDamageDelegate)Delegate.Combine(owner.onGaveDamage, new GaveDamageDelegate(this.HandleOnGaveDamage));
				this.ability._stack = 0;
				this._charging = true;
				this.UpdateStack();
			}

			// Token: 0x060039B1 RID: 14769 RVA: 0x000AA5AB File Offset: 0x000A87AB
			private void HandleOnGaveDamage(ITarget target, in Damage originalDamage, in Damage gaveDamage, double damageDealt)
			{
				if (!this._charging)
				{
					return;
				}
				if (!this.ability._motionFilter[gaveDamage.motionType])
				{
					return;
				}
				this._charging = false;
				this._remainDetachTime = this.ability._detachTime;
			}

			// Token: 0x060039B2 RID: 14770 RVA: 0x000AA5E8 File Offset: 0x000A87E8
			public override void UpdateTime(float deltaTime)
			{
				base.UpdateTime(deltaTime);
				if (this._charging)
				{
					this._remainChargingTime -= deltaTime;
					if (this._remainChargingTime <= 0f)
					{
						this.ability._stack = Mathf.Min(this.ability._maxStack, this.ability._stack + 1);
						this._remainChargingTime = this.ability._chargingTime;
						this.UpdateStack();
						return;
					}
				}
				else
				{
					this._remainDetachTime -= deltaTime;
					if (this._remainDetachTime <= this.ability._detachTime - this.ability._resetTime && this.ability._stack > 0)
					{
						this.ability._stack = 0;
						this.UpdateStack();
					}
					if (this._remainDetachTime <= 0f)
					{
						this._charging = true;
					}
				}
			}

			// Token: 0x060039B3 RID: 14771 RVA: 0x000AA6C3 File Offset: 0x000A88C3
			protected override void OnDetach()
			{
				this.owner.stat.DetachValues(this._stat);
				Character owner = this.owner;
				owner.onGaveDamage = (GaveDamageDelegate)Delegate.Remove(owner.onGaveDamage, new GaveDamageDelegate(this.HandleOnGaveDamage));
			}

			// Token: 0x060039B4 RID: 14772 RVA: 0x000AA704 File Offset: 0x000A8904
			public void UpdateStack()
			{
				for (int i = 0; i < this._stat.values.Length; i++)
				{
					this._stat.values[i].value = this.ability._statPerStack.values[i].GetStackedValue((double)this.ability._stack);
				}
				this.owner.stat.SetNeedUpdate();
			}

			// Token: 0x04002DC9 RID: 11721
			private Stat.Values _stat;

			// Token: 0x04002DCA RID: 11722
			private bool _charging;

			// Token: 0x04002DCB RID: 11723
			private float _remainChargingTime;

			// Token: 0x04002DCC RID: 11724
			private float _remainDetachTime;
		}
	}
}
