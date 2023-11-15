using System;
using UnityEngine;

namespace Characters.Abilities.Upgrades
{
	// Token: 0x02000AD7 RID: 2775
	[Serializable]
	public class GreatMountainForce : Ability
	{
		// Token: 0x060038E5 RID: 14565 RVA: 0x000A7A25 File Offset: 0x000A5C25
		public override IAbilityInstance CreateInstance(Character owner)
		{
			return new GreatMountainForce.Instance(owner, this);
		}

		// Token: 0x04002D33 RID: 11571
		[SerializeField]
		private float _updateInterval;

		// Token: 0x04002D34 RID: 11572
		[SerializeField]
		private int _maxStack;

		// Token: 0x04002D35 RID: 11573
		[SerializeField]
		private Stat.Values _statPerStack;

		// Token: 0x02000AD8 RID: 2776
		public class Instance : AbilityInstance<GreatMountainForce>
		{
			// Token: 0x060038E7 RID: 14567 RVA: 0x000A7A2E File Offset: 0x000A5C2E
			public Instance(Character owner, GreatMountainForce ability) : base(owner, ability)
			{
			}

			// Token: 0x060038E8 RID: 14568 RVA: 0x000A7A38 File Offset: 0x000A5C38
			protected override void OnAttach()
			{
				this._stat = this.ability._statPerStack.Clone();
				this._remainUpdateTime = this.ability._updateInterval;
				this.owner.stat.AttachValues(this._stat);
				Character owner = this.owner;
				owner.onGaveDamage = (GaveDamageDelegate)Delegate.Combine(owner.onGaveDamage, new GaveDamageDelegate(this.HandleOnGaveDamage));
			}

			// Token: 0x060038E9 RID: 14569 RVA: 0x000A7AA9 File Offset: 0x000A5CA9
			private void HandleOnGaveDamage(ITarget target, in Damage originalDamage, in Damage gaveDamage, double damageDealt)
			{
				this._stack = 0;
				this._remainUpdateTime = this.ability._updateInterval;
				this.UpdateStack();
			}

			// Token: 0x060038EA RID: 14570 RVA: 0x000A7ACC File Offset: 0x000A5CCC
			public override void UpdateTime(float deltaTime)
			{
				base.UpdateTime(deltaTime);
				this._remainUpdateTime -= deltaTime;
				if (this._remainUpdateTime < 0f)
				{
					this._stack = Mathf.Min(this._stack + 1, this.ability._maxStack);
					this.UpdateStack();
				}
			}

			// Token: 0x060038EB RID: 14571 RVA: 0x000A7B20 File Offset: 0x000A5D20
			public void UpdateStack()
			{
				for (int i = 0; i < this._stat.values.Length; i++)
				{
					this._stat.values[i].value = this.ability._statPerStack.values[i].GetStackedValue((double)this._stack);
				}
				this.owner.stat.SetNeedUpdate();
			}

			// Token: 0x060038EC RID: 14572 RVA: 0x000A7B85 File Offset: 0x000A5D85
			protected override void OnDetach()
			{
				this.owner.stat.DetachValues(this._stat);
				Character owner = this.owner;
				owner.onGaveDamage = (GaveDamageDelegate)Delegate.Remove(owner.onGaveDamage, new GaveDamageDelegate(this.HandleOnGaveDamage));
			}

			// Token: 0x04002D36 RID: 11574
			private Stat.Values _stat;

			// Token: 0x04002D37 RID: 11575
			private int _stack;

			// Token: 0x04002D38 RID: 11576
			private float _remainUpdateTime;
		}
	}
}
