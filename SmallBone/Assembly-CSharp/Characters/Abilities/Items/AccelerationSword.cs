using System;
using Characters.Operations;
using UnityEditor;
using UnityEngine;

namespace Characters.Abilities.Items
{
	// Token: 0x02000C9A RID: 3226
	[Serializable]
	public sealed class AccelerationSword : Ability
	{
		// Token: 0x060041A1 RID: 16801 RVA: 0x000BECF3 File Offset: 0x000BCEF3
		public override IAbilityInstance CreateInstance(Character owner)
		{
			return new AccelerationSword.Instance(owner, this);
		}

		// Token: 0x04003248 RID: 12872
		[SerializeField]
		private Collider2D _detectCollider;

		// Token: 0x04003249 RID: 12873
		[Header("쿨타임 = 기본 쿨타임 / (multiplier * 공격속도 스텟[기본값1])")]
		[SerializeField]
		private float _multiplier;

		// Token: 0x0400324A RID: 12874
		[SerializeField]
		private float _cooldownTime;

		// Token: 0x0400324B RID: 12875
		[SerializeField]
		private float _minCooldownTime;

		// Token: 0x0400324C RID: 12876
		[SerializeField]
		private CustomFloat _attackDamage;

		// Token: 0x0400324D RID: 12877
		[SerializeField]
		[Subcomponent(typeof(OperationInfo))]
		private OperationInfo.Subcomponents _operations;

		// Token: 0x02000C9B RID: 3227
		public class Instance : AbilityInstance<AccelerationSword>
		{
			// Token: 0x17000DC7 RID: 3527
			// (get) Token: 0x060041A3 RID: 16803 RVA: 0x000BECFC File Offset: 0x000BCEFC
			public override float iconFillAmount
			{
				get
				{
					return this._remainTime / this._newCooldownTime;
				}
			}

			// Token: 0x060041A4 RID: 16804 RVA: 0x000BED0B File Offset: 0x000BCF0B
			public Instance(Character owner, AccelerationSword ability) : base(owner, ability)
			{
			}

			// Token: 0x060041A5 RID: 16805 RVA: 0x000BED18 File Offset: 0x000BCF18
			protected override void OnAttach()
			{
				this._newCooldownTime = this.ability._cooldownTime / (float)((double)this.ability._multiplier * this.owner.stat.GetFinal(Stat.Kind.BasicAttackSpeed));
				this._remainTime = Mathf.Max(this.ability._minCooldownTime, this._newCooldownTime);
			}

			// Token: 0x060041A6 RID: 16806 RVA: 0x000BED76 File Offset: 0x000BCF76
			public override void UpdateTime(float deltaTime)
			{
				base.UpdateTime(deltaTime);
				this._remainTime -= deltaTime;
				if (this._remainTime < 0f)
				{
					this.Attack();
				}
			}

			// Token: 0x060041A7 RID: 16807 RVA: 0x00002191 File Offset: 0x00000391
			protected override void OnDetach()
			{
			}

			// Token: 0x060041A8 RID: 16808 RVA: 0x000BEDA0 File Offset: 0x000BCFA0
			public void Attack()
			{
				if (TargetFinder.GetRandomTarget(this.ability._detectCollider, 1024) == null)
				{
					this._remainTime = 0.5f;
				}
				this._newCooldownTime = this.ability._cooldownTime / (float)((double)this.ability._multiplier * this.owner.stat.GetFinal(Stat.Kind.BasicAttackSpeed));
				this._remainTime = Mathf.Max(this.ability._minCooldownTime, this._newCooldownTime);
				this.owner.StartCoroutine(this.ability._operations.CRun(this.owner));
			}

			// Token: 0x0400324E RID: 12878
			private float _newCooldownTime;

			// Token: 0x0400324F RID: 12879
			private float _remainTime;
		}
	}
}
