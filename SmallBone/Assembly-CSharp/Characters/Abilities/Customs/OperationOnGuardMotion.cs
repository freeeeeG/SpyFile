using System;
using System.Linq;
using Characters.Actions;
using Characters.Operations;
using UnityEngine;

namespace Characters.Abilities.Customs
{
	// Token: 0x02000D7A RID: 3450
	[Serializable]
	public class OperationOnGuardMotion : Ability
	{
		// Token: 0x06004583 RID: 17795 RVA: 0x000C970D File Offset: 0x000C790D
		public override void Initialize()
		{
			base.Initialize();
			this._operationOnGuardStart.Initialize();
			this._operationOnGuard.Initialize();
			if (this._operationOnGuardMaxCount == 0)
			{
				this._operationOnGuardMaxCount = int.MaxValue;
			}
		}

		// Token: 0x06004584 RID: 17796 RVA: 0x000C973E File Offset: 0x000C793E
		public override IAbilityInstance CreateInstance(Character owner)
		{
			return new OperationOnGuardMotion.Instance(owner, this);
		}

		// Token: 0x040034CE RID: 13518
		[SerializeField]
		private bool _onlyFront = true;

		// Token: 0x040034CF RID: 13519
		[SerializeField]
		private AttackTypeBoolArray _attackType = new AttackTypeBoolArray(new bool[]
		{
			false,
			true,
			true,
			true,
			false
		});

		// Token: 0x040034D0 RID: 13520
		[SerializeField]
		private Characters.Actions.Motion[] _motions;

		// Token: 0x040034D1 RID: 13521
		[SerializeField]
		private BoxCollider2D _operationRange;

		// Token: 0x040034D2 RID: 13522
		[SerializeField]
		private Transform _operationRunPosition;

		// Token: 0x040034D3 RID: 13523
		[SerializeField]
		private int _operationOnGuardMaxCount;

		// Token: 0x040034D4 RID: 13524
		[CharacterOperation.SubcomponentAttribute]
		[SerializeField]
		private CharacterOperation.Subcomponents _operationOnGuardStart;

		// Token: 0x040034D5 RID: 13525
		[SerializeField]
		[CharacterOperation.SubcomponentAttribute]
		private CharacterOperation.Subcomponents _operationOnGuard;

		// Token: 0x02000D7B RID: 3451
		public class Instance : AbilityInstance<OperationOnGuardMotion>
		{
			// Token: 0x06004586 RID: 17798 RVA: 0x000C9772 File Offset: 0x000C7972
			public Instance(Character owner, OperationOnGuardMotion ability) : base(owner, ability)
			{
			}

			// Token: 0x06004587 RID: 17799 RVA: 0x000C977C File Offset: 0x000C797C
			protected override void OnAttach()
			{
				this.count = 0;
				this._guarding = false;
				this.owner.health.onTakeDamage.Add(int.MaxValue, new TakeDamageDelegate(this.Guard));
			}

			// Token: 0x06004588 RID: 17800 RVA: 0x000C97B2 File Offset: 0x000C79B2
			protected override void OnDetach()
			{
				this.owner.health.onTakeDamage.Remove(new TakeDamageDelegate(this.Guard));
				this.ability._operationOnGuard.Stop();
			}

			// Token: 0x06004589 RID: 17801 RVA: 0x000C97E8 File Offset: 0x000C79E8
			private bool Guard(ref Damage damage)
			{
				Characters.Actions.Motion runningMotion = this.owner.runningMotion;
				if (runningMotion == null)
				{
					return false;
				}
				if (this.ability._motions.Length != 0 && !this.ability._motions.Contains(runningMotion))
				{
					return false;
				}
				if (damage.attackType == Damage.AttackType.Additional)
				{
					return false;
				}
				if (!this.owner.invulnerable.value && !damage.@null && damage.amount < 1.0)
				{
					return false;
				}
				if (!this.ability._attackType[damage.attackType])
				{
					return false;
				}
				Vector3 position = this.owner.transform.position;
				Vector3 position2 = damage.attacker.transform.position;
				if (this.ability._onlyFront)
				{
					if (this.owner.lookingDirection == Character.LookingDirection.Right && position.x > position2.x)
					{
						return false;
					}
					if (this.owner.lookingDirection == Character.LookingDirection.Left && position.x < position2.x)
					{
						return false;
					}
				}
				damage.@null = true;
				if (this.ability._operationOnGuard.components.Length == 0)
				{
					return false;
				}
				if (this.count > this.ability._operationOnGuardMaxCount)
				{
					return false;
				}
				Vector3 position3;
				if (damage.attacker.projectile == null)
				{
					position3 = MMMaths.RandomPointWithinBounds(this.ability._operationRange.bounds);
				}
				else
				{
					position3 = this.ability._operationRange.ClosestPoint(damage.hitPoint);
				}
				this.count++;
				this.ability._operationRunPosition.position = position3;
				this.ability._operationOnGuard.Run(this.owner);
				if (!this._guarding)
				{
					this.ability._operationOnGuardStart.Run(this.owner);
					this._guarding = true;
				}
				return false;
			}

			// Token: 0x040034D6 RID: 13526
			private bool _guarding;

			// Token: 0x040034D7 RID: 13527
			private int count;
		}
	}
}
