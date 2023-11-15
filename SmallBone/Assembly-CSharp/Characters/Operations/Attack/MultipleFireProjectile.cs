using System;
using Characters.Projectiles;
using Characters.Utils;
using UnityEngine;

namespace Characters.Operations.Attack
{
	// Token: 0x02000FA5 RID: 4005
	public class MultipleFireProjectile : CharacterOperation
	{
		// Token: 0x06004DC9 RID: 19913 RVA: 0x000E8AC6 File Offset: 0x000E6CC6
		protected override void OnDestroy()
		{
			base.OnDestroy();
			this._projectile = null;
		}

		// Token: 0x06004DCA RID: 19914 RVA: 0x000E8AD5 File Offset: 0x000E6CD5
		public override void Initialize()
		{
			this._attackDamage = base.GetComponentInParent<IAttackDamage>();
		}

		// Token: 0x06004DCB RID: 19915 RVA: 0x000E8AE4 File Offset: 0x000E6CE4
		public override void Run(Character owner)
		{
			CustomAngle[] values = this._directions.values;
			HitHistoryManager hitHistoryManager = this._group ? new HitHistoryManager(15) : null;
			foreach (object obj in this._fireTransformsParent)
			{
				Transform transform = (Transform)obj;
				if (this._directionType == MultipleFireProjectile.DirectionType.RotationOfFirePosition)
				{
					for (int i = 0; i < values.Length; i++)
					{
						this._projectile.reusable.Spawn(transform.position, true).GetComponent<Projectile>().Fire(owner, this._attackDamage.amount, transform.localRotation.eulerAngles.z + values[i].value, transform.lossyScale.x < 0f, false, 1f, null, 0f);
					}
				}
				else if (this._directionType == MultipleFireProjectile.DirectionType.OwnerDirection)
				{
					for (int j = 0; j < values.Length; j++)
					{
						this._projectile.reusable.Spawn(transform.position, true).GetComponent<Projectile>().Fire(owner, this._attackDamage.amount, values[j].value, owner.lookingDirection == Character.LookingDirection.Left, false, 1f, null, 0f);
					}
				}
				else
				{
					for (int k = 0; k < values.Length; k++)
					{
						this._projectile.reusable.Spawn(transform.position, true).GetComponent<Projectile>().Fire(owner, this._attackDamage.amount, values[k].value, false, false, 1f, this._group ? hitHistoryManager : null, 0f);
					}
				}
			}
		}

		// Token: 0x04003DB0 RID: 15792
		[SerializeField]
		private Projectile _projectile;

		// Token: 0x04003DB1 RID: 15793
		[SerializeField]
		private Transform _fireTransformsParent;

		// Token: 0x04003DB2 RID: 15794
		[SerializeField]
		private bool _group;

		// Token: 0x04003DB3 RID: 15795
		[SerializeField]
		private MultipleFireProjectile.DirectionType _directionType;

		// Token: 0x04003DB4 RID: 15796
		[SerializeField]
		private CustomAngle.Reorderable _directions;

		// Token: 0x04003DB5 RID: 15797
		[SerializeField]
		private IAttackDamage _attackDamage;

		// Token: 0x02000FA6 RID: 4006
		public enum DirectionType
		{
			// Token: 0x04003DB7 RID: 15799
			RotationOfFirePosition,
			// Token: 0x04003DB8 RID: 15800
			RotationOfCenter,
			// Token: 0x04003DB9 RID: 15801
			OwnerDirection,
			// Token: 0x04003DBA RID: 15802
			Constant
		}
	}
}
