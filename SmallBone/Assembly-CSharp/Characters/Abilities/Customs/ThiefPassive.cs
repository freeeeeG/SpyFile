using System;
using Characters.Operations;
using Level;
using UnityEngine;

namespace Characters.Abilities.Customs
{
	// Token: 0x02000D99 RID: 3481
	[Serializable]
	public class ThiefPassive : Ability
	{
		// Token: 0x0600460B RID: 17931 RVA: 0x000CAD89 File Offset: 0x000C8F89
		public override void Initialize()
		{
			base.Initialize();
			this._operationOnGoldDespawn.Initialize();
		}

		// Token: 0x0600460C RID: 17932 RVA: 0x000CAD9C File Offset: 0x000C8F9C
		public override IAbilityInstance CreateInstance(Character owner)
		{
			return new ThiefPassive.Instance(owner, this);
		}

		// Token: 0x04003526 RID: 13606
		[Tooltip("골드가 디스폰 되는 시점에 이 트랜스폼이 그 지점으로 이동됩니다.")]
		[SerializeField]
		private Transform _goldDespawnPosition;

		// Token: 0x04003527 RID: 13607
		[CharacterOperation.SubcomponentAttribute]
		[SerializeField]
		private CharacterOperation.Subcomponents _operationOnGoldDespawn;

		// Token: 0x02000D9A RID: 3482
		public class Instance : AbilityInstance<ThiefPassive>
		{
			// Token: 0x0600460E RID: 17934 RVA: 0x000CADA5 File Offset: 0x000C8FA5
			public Instance(Character owner, ThiefPassive ability) : base(owner, ability)
			{
			}

			// Token: 0x0600460F RID: 17935 RVA: 0x000CADAF File Offset: 0x000C8FAF
			protected override void OnAttach()
			{
				ThiefGold.onDespawn += this.OnThiefGoldDespawn;
			}

			// Token: 0x06004610 RID: 17936 RVA: 0x000CADC2 File Offset: 0x000C8FC2
			protected override void OnDetach()
			{
				ThiefGold.onDespawn -= this.OnThiefGoldDespawn;
			}

			// Token: 0x06004611 RID: 17937 RVA: 0x000CADD5 File Offset: 0x000C8FD5
			private void OnThiefGoldDespawn(double goldAmount, Vector3 position)
			{
				this.ability._goldDespawnPosition.transform.position = position;
				this.ability._operationOnGoldDespawn.Run(this.owner);
			}
		}
	}
}
