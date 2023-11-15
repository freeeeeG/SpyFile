using System;
using Characters.Actions;
using Characters.Projectiles;
using UnityEngine;

namespace Characters.Operations
{
	// Token: 0x02000DE2 RID: 3554
	public class AbyssOrb : CharacterOperation
	{
		// Token: 0x17000EBC RID: 3772
		// (get) Token: 0x06004740 RID: 18240 RVA: 0x000CEF17 File Offset: 0x000CD117
		public CustomAngle[] directions
		{
			get
			{
				return this._directions.values;
			}
		}

		// Token: 0x06004741 RID: 18241 RVA: 0x000CEF24 File Offset: 0x000CD124
		public override void Initialize()
		{
			this._attackDamage = base.GetComponentInParent<IAttackDamage>();
			if (this._fireTransform == null)
			{
				this._fireTransform = base.transform;
			}
		}

		// Token: 0x06004742 RID: 18242 RVA: 0x000CEF4C File Offset: 0x000CD14C
		public override void Run(Character owner)
		{
			float d = (this._scaleMax - this._scaleMin) * this._chargeAction.chargedPercent + this._scaleMin;
			float num = (this._damageMultiplierMax - this._damageMultiplierMin) * this._chargeAction.chargedPercent + this._damageMultiplierMin;
			Projectile projectile = (this._chargeAction.chargedPercent < 1f) ? this._incompleteProjectile : this._completeProjectile;
			CustomAngle[] values = this._directions.values;
			float attackDamage = this._attackDamage.amount * num;
			bool flipX = false;
			bool flipY = false;
			for (int i = 0; i < values.Length; i++)
			{
				AbyssOrb.DirectionType directionType = this._directionType;
				float num2;
				if (directionType != AbyssOrb.DirectionType.RotationOfFirePosition)
				{
					if (directionType != AbyssOrb.DirectionType.OwnerDirection)
					{
						num2 = values[i].value;
					}
					else
					{
						num2 = values[i].value;
						bool flag = owner.lookingDirection == Character.LookingDirection.Left || this._fireTransform.lossyScale.x < 0f;
						flipX = (this._flipXByOwnerDirection && flag);
						flipY = (this._flipYByOwnerDirection && flag);
						num2 = (flag ? ((180f - num2) % 360f) : num2);
					}
				}
				else
				{
					num2 = this._fireTransform.rotation.eulerAngles.z + values[i].value;
					if (this._fireTransform.lossyScale.x < 0f)
					{
						num2 = (180f - num2) % 360f;
					}
				}
				Projectile component = projectile.reusable.Spawn(this._fireTransform.position, true).GetComponent<Projectile>();
				component.transform.localScale = Vector3.one * d;
				component.Fire(owner, attackDamage, num2, flipX, flipY, 1f, null, 0f);
			}
		}

		// Token: 0x04003634 RID: 13876
		[SerializeField]
		private ChargeAction _chargeAction;

		// Token: 0x04003635 RID: 13877
		[SerializeField]
		[Space]
		private float _scaleMin = 0.2f;

		// Token: 0x04003636 RID: 13878
		[SerializeField]
		private float _scaleMax = 1f;

		// Token: 0x04003637 RID: 13879
		[SerializeField]
		private float _damageMultiplierMin = 0.2f;

		// Token: 0x04003638 RID: 13880
		[SerializeField]
		private float _damageMultiplierMax = 1f;

		// Token: 0x04003639 RID: 13881
		[SerializeField]
		[Space]
		private Projectile _incompleteProjectile;

		// Token: 0x0400363A RID: 13882
		[SerializeField]
		private Projectile _completeProjectile;

		// Token: 0x0400363B RID: 13883
		[SerializeField]
		[Space]
		private Transform _fireTransform;

		// Token: 0x0400363C RID: 13884
		[SerializeField]
		private bool _flipXByOwnerDirection;

		// Token: 0x0400363D RID: 13885
		[SerializeField]
		private bool _flipYByOwnerDirection;

		// Token: 0x0400363E RID: 13886
		[SerializeField]
		[Space]
		private AbyssOrb.DirectionType _directionType;

		// Token: 0x0400363F RID: 13887
		[SerializeField]
		private CustomAngle.Reorderable _directions;

		// Token: 0x04003640 RID: 13888
		private IAttackDamage _attackDamage;

		// Token: 0x02000DE3 RID: 3555
		public enum DirectionType
		{
			// Token: 0x04003642 RID: 13890
			RotationOfFirePosition,
			// Token: 0x04003643 RID: 13891
			OwnerDirection,
			// Token: 0x04003644 RID: 13892
			Constant
		}
	}
}
