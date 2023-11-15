using System;
using UnityEngine;

namespace Characters.Abilities.CharacterStat
{
	// Token: 0x02000C35 RID: 3125
	[Serializable]
	public sealed class StackableStatBonusByLostHealth : Ability
	{
		// Token: 0x06004027 RID: 16423 RVA: 0x000BA392 File Offset: 0x000B8592
		public override IAbilityInstance CreateInstance(Character owner)
		{
			return new StackableStatBonusByLostHealth.Instance(owner, this);
		}

		// Token: 0x04003156 RID: 12630
		[SerializeField]
		private Stat.Values _targetPerStack;

		// Token: 0x04003157 RID: 12631
		[SerializeField]
		[Range(0f, 1000f)]
		private int _healthStackUnit;

		// Token: 0x04003158 RID: 12632
		[SerializeField]
		private int _maxStack;

		// Token: 0x02000C36 RID: 3126
		public sealed class Instance : AbilityInstance<StackableStatBonusByLostHealth>
		{
			// Token: 0x17000D8E RID: 3470
			// (get) Token: 0x06004029 RID: 16425 RVA: 0x000BA39B File Offset: 0x000B859B
			public override int iconStacks
			{
				get
				{
					return this._stack;
				}
			}

			// Token: 0x17000D8F RID: 3471
			// (get) Token: 0x0600402A RID: 16426 RVA: 0x000BA3A3 File Offset: 0x000B85A3
			public override float iconFillAmount
			{
				get
				{
					return base.remainTime / this.ability.duration;
				}
			}

			// Token: 0x0600402B RID: 16427 RVA: 0x000BA3B7 File Offset: 0x000B85B7
			public Instance(Character owner, StackableStatBonusByLostHealth ability) : base(owner, ability)
			{
				this._stat = ability._targetPerStack.Clone();
			}

			// Token: 0x0600402C RID: 16428 RVA: 0x000BA3D2 File Offset: 0x000B85D2
			protected override void OnAttach()
			{
				this.owner.stat.AttachValues(this._stat);
				this.owner.health.onChanged += this.UpdateStat;
				this.UpdateStat();
			}

			// Token: 0x0600402D RID: 16429 RVA: 0x000BA40C File Offset: 0x000B860C
			protected override void OnDetach()
			{
				this.owner.health.onChanged -= this.UpdateStat;
				this.owner.stat.DetachValues(this._stat);
			}

			// Token: 0x0600402E RID: 16430 RVA: 0x000BA440 File Offset: 0x000B8640
			public void UpdateStat()
			{
				int num = (int)((this.owner.health.maximumHealth - this.owner.health.currentHealth) / (double)this.ability._healthStackUnit);
				if (num == this._stack)
				{
					return;
				}
				this._stack = Mathf.Min(this.ability._maxStack, num);
				this.SetStat(this._stack);
			}

			// Token: 0x0600402F RID: 16431 RVA: 0x000BA4AC File Offset: 0x000B86AC
			private void SetStat(int stack)
			{
				Stat.Value[] values = this._stat.values;
				for (int i = 0; i < values.Length; i++)
				{
					values[i].value = this.ability._targetPerStack.values[i].GetStackedValue((double)stack);
				}
				this.owner.stat.SetNeedUpdate();
			}

			// Token: 0x04003159 RID: 12633
			private Stat.Values _stat;

			// Token: 0x0400315A RID: 12634
			private int _stack;
		}
	}
}
