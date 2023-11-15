using System;
using Characters.Operations;
using UnityEditor;
using UnityEngine;

namespace Characters.Abilities.Customs
{
	// Token: 0x02000D4A RID: 3402
	[Serializable]
	public class EssenceRecruitPassive : Ability
	{
		// Token: 0x0600448E RID: 17550 RVA: 0x000C7187 File Offset: 0x000C5387
		public override IAbilityInstance CreateInstance(Character owner)
		{
			return new EssenceRecruitPassive.Instance(owner, this);
		}

		// Token: 0x04003440 RID: 13376
		[SerializeField]
		private float _cooldownTime;

		// Token: 0x04003441 RID: 13377
		[Subcomponent(typeof(OperationInfo))]
		[SerializeField]
		private OperationInfo.Subcomponents _onHits;

		// Token: 0x04003442 RID: 13378
		[SerializeField]
		private Stat.Values _stat;

		// Token: 0x02000D4B RID: 3403
		public class Instance : AbilityInstance<EssenceRecruitPassive>
		{
			// Token: 0x06004490 RID: 17552 RVA: 0x000C7190 File Offset: 0x000C5390
			public Instance(Character owner, EssenceRecruitPassive ability) : base(owner, ability)
			{
			}

			// Token: 0x06004491 RID: 17553 RVA: 0x000C719C File Offset: 0x000C539C
			protected override void OnAttach()
			{
				this.owner.health.onTakeDamage.Add(int.MaxValue, new TakeDamageDelegate(this.OnTakeDamage));
				this.owner.stat.AttachValues(this.ability._stat);
			}

			// Token: 0x06004492 RID: 17554 RVA: 0x000C71EA File Offset: 0x000C53EA
			protected override void OnDetach()
			{
				this.owner.health.onTakeDamage.Remove(new TakeDamageDelegate(this.OnTakeDamage));
				this.owner.stat.DetachValues(this.ability._stat);
			}

			// Token: 0x06004493 RID: 17555 RVA: 0x000C722C File Offset: 0x000C542C
			private bool OnTakeDamage(ref Damage damage)
			{
				if (this._remainCooldownTime > 0f)
				{
					return true;
				}
				this._remainCooldownTime = this.ability._cooldownTime;
				this.owner.StartCoroutine(this.ability._onHits.CRun(this.owner));
				return true;
			}

			// Token: 0x06004494 RID: 17556 RVA: 0x000C727C File Offset: 0x000C547C
			public override void UpdateTime(float deltaTime)
			{
				base.UpdateTime(deltaTime);
				if (this._remainCooldownTime < 0f)
				{
					return;
				}
				this._remainCooldownTime -= deltaTime;
			}

			// Token: 0x04003443 RID: 13379
			private float _remainCooldownTime;
		}
	}
}
