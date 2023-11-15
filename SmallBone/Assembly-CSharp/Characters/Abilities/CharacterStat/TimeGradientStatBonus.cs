using System;
using UnityEngine;

namespace Characters.Abilities.CharacterStat
{
	// Token: 0x02000C8C RID: 3212
	[Serializable]
	public sealed class TimeGradientStatBonus : Ability
	{
		// Token: 0x06004172 RID: 16754 RVA: 0x000BE652 File Offset: 0x000BC852
		public override IAbilityInstance CreateInstance(Character owner)
		{
			return new TimeGradientStatBonus.Instance(owner, this);
		}

		// Token: 0x04003232 RID: 12850
		[SerializeField]
		private Stat.Values _startStat;

		// Token: 0x04003233 RID: 12851
		[SerializeField]
		private float _updateTime = 1f;

		// Token: 0x04003234 RID: 12852
		[SerializeField]
		private float _delta;

		// Token: 0x02000C8D RID: 3213
		public sealed class Instance : AbilityInstance<TimeGradientStatBonus>
		{
			// Token: 0x06004174 RID: 16756 RVA: 0x000BE66E File Offset: 0x000BC86E
			public Instance(Character owner, TimeGradientStatBonus ability) : base(owner, ability)
			{
				this._stat = ability._startStat.Clone();
				this._remainUpdateTime = ability._updateTime;
			}

			// Token: 0x06004175 RID: 16757 RVA: 0x000BE695 File Offset: 0x000BC895
			protected override void OnAttach()
			{
				this.owner.stat.AttachValues(this._stat);
			}

			// Token: 0x06004176 RID: 16758 RVA: 0x000BE6AD File Offset: 0x000BC8AD
			protected override void OnDetach()
			{
				this.owner.stat.DetachValues(this._stat);
			}

			// Token: 0x06004177 RID: 16759 RVA: 0x000BE6C8 File Offset: 0x000BC8C8
			public override void Refresh()
			{
				base.Refresh();
				for (int i = 0; i < this._stat.values.Length; i++)
				{
					this._stat.values[i].value = this.ability._startStat.values[i].value;
				}
			}

			// Token: 0x06004178 RID: 16760 RVA: 0x000BE71C File Offset: 0x000BC91C
			public override void UpdateTime(float deltaTime)
			{
				base.UpdateTime(deltaTime);
				this._remainUpdateTime -= deltaTime;
				if (this._remainUpdateTime <= 0f)
				{
					this.UpdateStat();
					this._remainUpdateTime = this.ability._updateTime;
				}
			}

			// Token: 0x06004179 RID: 16761 RVA: 0x000BE758 File Offset: 0x000BC958
			private void UpdateStat()
			{
				for (int i = 0; i < this._stat.values.Length; i++)
				{
					this._stat.values[i].value += (double)(this.ability._delta * 0.01f);
				}
				this.owner.stat.SetNeedUpdate();
			}

			// Token: 0x04003235 RID: 12853
			private Stat.Values _stat;

			// Token: 0x04003236 RID: 12854
			private float _remainUpdateTime;
		}
	}
}
