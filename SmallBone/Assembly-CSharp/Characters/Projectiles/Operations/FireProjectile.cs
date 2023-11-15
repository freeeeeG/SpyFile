using System;
using Characters.Utils;
using UnityEngine;

namespace Characters.Projectiles.Operations
{
	// Token: 0x0200077B RID: 1915
	public class FireProjectile : Operation
	{
		// Token: 0x17000828 RID: 2088
		// (get) Token: 0x0600276E RID: 10094 RVA: 0x00076423 File Offset: 0x00074623
		public CustomAngle[] directions
		{
			get
			{
				return this._directions.values;
			}
		}

		// Token: 0x0600276F RID: 10095 RVA: 0x00076430 File Offset: 0x00074630
		private void Awake()
		{
			if (this._fireTransform == null)
			{
				this._fireTransform = base.transform;
			}
		}

		// Token: 0x06002770 RID: 10096 RVA: 0x0007644C File Offset: 0x0007464C
		private void OnDestroy()
		{
			this._projectile = null;
		}

		// Token: 0x06002771 RID: 10097 RVA: 0x00076458 File Offset: 0x00074658
		public override void Run(IProjectile projectile)
		{
			Character owner = projectile.owner;
			CustomAngle[] values = this._directions.values;
			float attackDamage = projectile.baseDamage * this._damageMultiplier.value;
			HitHistoryManager hitHistoryManager = this._group ? new HitHistoryManager(15) : null;
			for (int i = 0; i < values.Length; i++)
			{
				FireProjectile.DirectionType directionType = this._directionType;
				float direction;
				bool flipX;
				if (directionType != FireProjectile.DirectionType.RotationOfFirePosition)
				{
					if (directionType != FireProjectile.DirectionType.OwnerDirection)
					{
						direction = values[i].value;
						flipX = false;
					}
					else
					{
						direction = values[i].value;
						flipX = (owner.lookingDirection == Character.LookingDirection.Left);
					}
				}
				else
				{
					direction = this._fireTransform.rotation.eulerAngles.z + values[i].value;
					flipX = (this._fireTransform.lossyScale.x < 0f);
				}
				this._projectile.reusable.Spawn(this._fireTransform.position, true).GetComponent<Projectile>().Fire(owner, attackDamage, direction, flipX, false, this._speedMultiplier.value, this._group ? hitHistoryManager : null, 0f);
			}
		}

		// Token: 0x04002184 RID: 8580
		[SerializeField]
		private Projectile _projectile;

		// Token: 0x04002185 RID: 8581
		[SerializeField]
		private CustomFloat _speedMultiplier = new CustomFloat(1f);

		// Token: 0x04002186 RID: 8582
		[SerializeField]
		private CustomFloat _damageMultiplier = new CustomFloat(1f);

		// Token: 0x04002187 RID: 8583
		[SerializeField]
		private Transform _fireTransform;

		// Token: 0x04002188 RID: 8584
		[SerializeField]
		private bool _group;

		// Token: 0x04002189 RID: 8585
		[SerializeField]
		private FireProjectile.DirectionType _directionType;

		// Token: 0x0400218A RID: 8586
		[SerializeField]
		private CustomAngle.Reorderable _directions;

		// Token: 0x0200077C RID: 1916
		public enum DirectionType
		{
			// Token: 0x0400218C RID: 8588
			RotationOfFirePosition,
			// Token: 0x0400218D RID: 8589
			OwnerDirection,
			// Token: 0x0400218E RID: 8590
			Constant
		}
	}
}
