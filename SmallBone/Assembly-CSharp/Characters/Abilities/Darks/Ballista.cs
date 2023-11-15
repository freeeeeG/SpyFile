using System;
using System.Collections.Generic;
using Level;
using UnityEngine;

namespace Characters.Abilities.Darks
{
	// Token: 0x02000BB1 RID: 2993
	[Serializable]
	public sealed class Ballista : Ability
	{
		// Token: 0x06003DB9 RID: 15801 RVA: 0x000B369C File Offset: 0x000B189C
		public override IAbilityInstance CreateInstance(Character owner)
		{
			return new Ballista.Instance(owner, this);
		}

		// Token: 0x04002FB8 RID: 12216
		[SerializeField]
		private Character _toSummonPrefab;

		// Token: 0x04002FB9 RID: 12217
		[SerializeField]
		private float _summonCooldowntime;

		// Token: 0x04002FBA RID: 12218
		[SerializeField]
		private float _summonDistance;

		// Token: 0x04002FBB RID: 12219
		[SerializeField]
		private int _maxCount;

		// Token: 0x02000BB2 RID: 2994
		public sealed class Instance : AbilityInstance<Ballista>
		{
			// Token: 0x06003DBB RID: 15803 RVA: 0x000B36A5 File Offset: 0x000B18A5
			public Instance(Character owner, Ballista ability) : base(owner, ability)
			{
			}

			// Token: 0x06003DBC RID: 15804 RVA: 0x000B36B0 File Offset: 0x000B18B0
			public override void UpdateTime(float deltaTime)
			{
				base.UpdateTime(deltaTime);
				if (this._summons.Count < this.ability._maxCount)
				{
					this._remainCooldownTime -= deltaTime;
					if (this._remainCooldownTime <= 0f)
					{
						this.TryToSummon();
					}
				}
			}

			// Token: 0x06003DBD RID: 15805 RVA: 0x000B3700 File Offset: 0x000B1900
			private void TryToSummon()
			{
				Ballista.Instance.<>c__DisplayClass4_0 CS$<>8__locals1 = new Ballista.Instance.<>c__DisplayClass4_0();
				CS$<>8__locals1.<>4__this = this;
				if (this._summons.Count > this.ability._maxCount)
				{
					return;
				}
				CS$<>8__locals1.summoned = UnityEngine.Object.Instantiate<Character>(this.ability._toSummonPrefab, this.GetSummonPoint(), Quaternion.identity);
				Map.Instance.waveContainer.Attach(CS$<>8__locals1.summoned);
				this._summons.Add(CS$<>8__locals1.summoned);
				CS$<>8__locals1.summoned.ForceToLookAt(this.owner.lookingDirection);
				CS$<>8__locals1.summoned.health.onDiedTryCatch += CS$<>8__locals1.<TryToSummon>g__HandleSummonDied|0;
				this._remainCooldownTime = this.ability._summonCooldowntime;
			}

			// Token: 0x06003DBE RID: 15806 RVA: 0x000B37C4 File Offset: 0x000B19C4
			private Vector2 GetSummonPoint()
			{
				Collider2D collider2D;
				if (!this.owner.movement.TryGetClosestBelowCollider(out collider2D, 8, 100f))
				{
					return this.owner.transform.position;
				}
				float x = this.owner.transform.position.x;
				return new Vector2((this.owner.lookingDirection == Character.LookingDirection.Right) ? (x + this.ability._summonDistance) : (x - this.ability._summonDistance), collider2D.bounds.max.y);
			}

			// Token: 0x06003DBF RID: 15807 RVA: 0x000B385D File Offset: 0x000B1A5D
			protected override void OnAttach()
			{
				this._summons = new List<Character>(this.ability._maxCount);
			}

			// Token: 0x06003DC0 RID: 15808 RVA: 0x000B3878 File Offset: 0x000B1A78
			protected override void OnDetach()
			{
				for (int i = this._summons.Count - 1; i >= 0; i--)
				{
					this._summons[i].health.Kill();
				}
			}

			// Token: 0x04002FBC RID: 12220
			private float _remainCooldownTime;

			// Token: 0x04002FBD RID: 12221
			private List<Character> _summons;
		}
	}
}
