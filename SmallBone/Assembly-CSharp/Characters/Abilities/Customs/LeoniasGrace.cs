using System;
using Characters.Operations;
using UnityEngine;

namespace Characters.Abilities.Customs
{
	// Token: 0x02000D5E RID: 3422
	[Serializable]
	public class LeoniasGrace : Ability
	{
		// Token: 0x060044FD RID: 17661 RVA: 0x000C86F3 File Offset: 0x000C68F3
		public override void Initialize()
		{
			base.Initialize();
			this._operationsOnHit.Initialize();
			this._operationsOnBreak.Initialize();
			this._remainHealth = this._health;
		}

		// Token: 0x060044FE RID: 17662 RVA: 0x000C871D File Offset: 0x000C691D
		public override IAbilityInstance CreateInstance(Character owner)
		{
			return new LeoniasGrace.Instance(owner, this);
		}

		// Token: 0x0400348B RID: 13451
		[SerializeField]
		private OperationInfos _operationInfos;

		// Token: 0x0400348C RID: 13452
		[SerializeField]
		private double _health;

		// Token: 0x0400348D RID: 13453
		[SerializeField]
		private Transform _hitPoint;

		// Token: 0x0400348E RID: 13454
		[SerializeField]
		[CharacterOperation.SubcomponentAttribute]
		private CharacterOperation.Subcomponents _operationsOnHit;

		// Token: 0x0400348F RID: 13455
		[SerializeField]
		[CharacterOperation.SubcomponentAttribute]
		private CharacterOperation.Subcomponents _operationsOnBreak;

		// Token: 0x04003490 RID: 13456
		private double _remainHealth;

		// Token: 0x02000D5F RID: 3423
		public class Instance : AbilityInstance<LeoniasGrace>
		{
			// Token: 0x17000E53 RID: 3667
			// (get) Token: 0x06004500 RID: 17664 RVA: 0x000C8726 File Offset: 0x000C6926
			public override int iconStacks
			{
				get
				{
					return (int)this.ability._remainHealth;
				}
			}

			// Token: 0x06004501 RID: 17665 RVA: 0x000C8734 File Offset: 0x000C6934
			public Instance(Character owner, LeoniasGrace ability) : base(owner, ability)
			{
			}

			// Token: 0x06004502 RID: 17666 RVA: 0x000C873E File Offset: 0x000C693E
			protected override void OnAttach()
			{
				this.owner.health.onTakeDamage.Add(int.MinValue, new TakeDamageDelegate(this.OnOwnerTakeDamage));
			}

			// Token: 0x06004503 RID: 17667 RVA: 0x000C8766 File Offset: 0x000C6966
			protected override void OnDetach()
			{
				this.owner.health.onTakeDamage.Remove(new TakeDamageDelegate(this.OnOwnerTakeDamage));
			}

			// Token: 0x06004504 RID: 17668 RVA: 0x000C878C File Offset: 0x000C698C
			private bool OnOwnerTakeDamage(ref Damage damage)
			{
				this.ability._remainHealth -= damage.amount;
				if (this.ability._hitPoint != null)
				{
					this.ability._hitPoint.position = damage.hitPoint;
				}
				this.ability._operationsOnHit.Run(this.owner);
				if (this.ability._remainHealth < 0.0)
				{
					this.ability._operationsOnBreak.Run(this.owner);
					this.ability._operationInfos.Stop();
				}
				return true;
			}
		}
	}
}
