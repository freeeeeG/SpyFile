using System;
using UnityEngine;

namespace Characters.Abilities.CharacterStat
{
	// Token: 0x02000C86 RID: 3206
	[Serializable]
	public sealed class StatPerCurrentHealth : Ability
	{
		// Token: 0x0600415E RID: 16734 RVA: 0x000BE2BE File Offset: 0x000BC4BE
		public override IAbilityInstance CreateInstance(Character owner)
		{
			return new StatPerCurrentHealth.Instance(owner, this);
		}

		// Token: 0x04003226 RID: 12838
		[SerializeField]
		private Stat.Values _statPerCurrentHealth;

		// Token: 0x04003227 RID: 12839
		[SerializeField]
		private int _healthPerStack;

		// Token: 0x04003228 RID: 12840
		[SerializeField]
		private int _maxStack;

		// Token: 0x02000C87 RID: 3207
		public sealed class Instance : AbilityInstance<StatPerCurrentHealth>
		{
			// Token: 0x17000DC0 RID: 3520
			// (get) Token: 0x06004160 RID: 16736 RVA: 0x000BE2C7 File Offset: 0x000BC4C7
			public override int iconStacks
			{
				get
				{
					return this._stack;
				}
			}

			// Token: 0x06004161 RID: 16737 RVA: 0x000BE2CF File Offset: 0x000BC4CF
			public Instance(Character owner, StatPerCurrentHealth ability) : base(owner, ability)
			{
				this._stat = ability._statPerCurrentHealth.Clone();
			}

			// Token: 0x06004162 RID: 16738 RVA: 0x000BE2EA File Offset: 0x000BC4EA
			protected override void OnAttach()
			{
				this.UpdateStat();
				this.owner.stat.AttachValues(this._stat);
			}

			// Token: 0x06004163 RID: 16739 RVA: 0x000BE308 File Offset: 0x000BC508
			protected override void OnDetach()
			{
				this.owner.stat.DetachValues(this._stat);
			}

			// Token: 0x06004164 RID: 16740 RVA: 0x000BE320 File Offset: 0x000BC520
			public override void UpdateTime(float deltaTime)
			{
				base.UpdateTime(deltaTime);
				this._remainCheckTime -= deltaTime;
				if (this._remainCheckTime < 0f)
				{
					this._remainCheckTime += 0.15f;
					this.TryUpdateStat();
				}
			}

			// Token: 0x06004165 RID: 16741 RVA: 0x000BE35C File Offset: 0x000BC55C
			private void TryUpdateStat()
			{
				if (Mathf.Abs((float)(this.owner.health.currentHealth - this._healthCache)) < 1E-45f)
				{
					return;
				}
				this.UpdateStat();
			}

			// Token: 0x06004166 RID: 16742 RVA: 0x000BE38C File Offset: 0x000BC58C
			private void UpdateStat()
			{
				this._stack = Mathf.Min(this.ability._maxStack, Mathf.FloorToInt((float)this.owner.health.currentHealth / (float)this.ability._healthPerStack));
				for (int i = 0; i < this._stat.values.Length; i++)
				{
					double num = (double)this._stack * this.ability._statPerCurrentHealth.values[i].value;
					if (this.ability._statPerCurrentHealth.values[i].categoryIndex == Stat.Category.Percent.index)
					{
						num += 1.0;
					}
					this._stat.values[i].value = num;
				}
				this.owner.stat.SetNeedUpdate();
			}

			// Token: 0x04003229 RID: 12841
			private const float _checkInterval = 0.15f;

			// Token: 0x0400322A RID: 12842
			private Stat.Values _stat;

			// Token: 0x0400322B RID: 12843
			private int _stack;

			// Token: 0x0400322C RID: 12844
			private double _healthCache;

			// Token: 0x0400322D RID: 12845
			private float _remainCheckTime;
		}
	}
}
