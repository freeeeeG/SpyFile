using System;
using System.Linq;
using Characters.Abilities.Constraints;
using PhysicsUtils;
using UnityEngine;

namespace Characters.Abilities.Customs
{
	// Token: 0x02000D0F RID: 3343
	[Serializable]
	public class AbyssMonarchPassive : Ability
	{
		// Token: 0x06004375 RID: 17269 RVA: 0x000C484C File Offset: 0x000C2A4C
		public override IAbilityInstance CreateInstance(Character owner)
		{
			return new AbyssMonarchPassive.Instance(owner, this);
		}

		// Token: 0x04003387 RID: 13191
		[SerializeField]
		[Space]
		private Collider2D _findRange;

		// Token: 0x04003388 RID: 13192
		[SerializeField]
		private float _findInterval;

		// Token: 0x04003389 RID: 13193
		[SerializeField]
		private float _speedBonusPerTarget;

		// Token: 0x0400338A RID: 13194
		[Constraint.SubcomponentAttribute]
		[SerializeField]
		[Space]
		private Constraint.Subcomponents _constraints;

		// Token: 0x02000D10 RID: 3344
		public class Instance : AbilityInstance<AbyssMonarchPassive>
		{
			// Token: 0x06004377 RID: 17271 RVA: 0x000C4855 File Offset: 0x000C2A55
			static Instance()
			{
				AbyssMonarchPassive.Instance._enemyOverlapper = new NonAllocOverlapper(AbyssMonarchPassive.Instance._maximum);
				AbyssMonarchPassive.Instance._enemyOverlapper.contactFilter.SetLayerMask(1024);
			}

			// Token: 0x06004378 RID: 17272 RVA: 0x000C4889 File Offset: 0x000C2A89
			public Instance(Character owner, AbyssMonarchPassive ability) : base(owner, ability)
			{
				this._statPerStack = new Stat.Values(new Stat.Value[]
				{
					new Stat.Value(Stat.Category.PercentPoint, Stat.Kind.ChargingSpeed, (double)ability._speedBonusPerTarget)
				});
			}

			// Token: 0x06004379 RID: 17273 RVA: 0x000C48BD File Offset: 0x000C2ABD
			protected override void OnAttach()
			{
				this._stat = this._statPerStack.Clone();
				this.owner.stat.AttachValues(this._stat);
			}

			// Token: 0x0600437A RID: 17274 RVA: 0x000C48E6 File Offset: 0x000C2AE6
			protected override void OnDetach()
			{
				this.owner.stat.DetachValues(this._stat);
			}

			// Token: 0x0600437B RID: 17275 RVA: 0x000C4900 File Offset: 0x000C2B00
			public override void UpdateTime(float deltaTime)
			{
				base.UpdateTime(deltaTime);
				if (!this.ability._constraints.Pass())
				{
					return;
				}
				this._remainFindTime -= deltaTime;
				if (this._remainFindTime <= 0f)
				{
					this.UpdateStack(this.GetAbyssEnemyCountAround());
					this._remainFindTime = this.ability._findInterval;
				}
			}

			// Token: 0x0600437C RID: 17276 RVA: 0x000C4960 File Offset: 0x000C2B60
			private int GetAbyssEnemyCountAround()
			{
				Collider2D findRange = this.ability._findRange;
				findRange.enabled = true;
				int result = (from character in AbyssMonarchPassive.Instance._enemyOverlapper.OverlapCollider(findRange).GetComponents<Character>(true)
				where character.ability.GetInstance<Abyss>() != null
				select character).Count<Character>();
				findRange.enabled = false;
				return result;
			}

			// Token: 0x0600437D RID: 17277 RVA: 0x000C49C4 File Offset: 0x000C2BC4
			private void UpdateStack(int stack)
			{
				for (int i = 0; i < this._stat.values.Length; i++)
				{
					this._stat.values[i].value = this._statPerStack.values[i].GetStackedValue((double)stack);
				}
				this.owner.stat.SetNeedUpdate();
			}

			// Token: 0x0400338B RID: 13195
			private readonly Stat.Values _statPerStack;

			// Token: 0x0400338C RID: 13196
			private static readonly NonAllocOverlapper _enemyOverlapper;

			// Token: 0x0400338D RID: 13197
			private static readonly int _maximum = 255;

			// Token: 0x0400338E RID: 13198
			private Stat.Values _stat;

			// Token: 0x0400338F RID: 13199
			private float _remainFindTime;
		}
	}
}
