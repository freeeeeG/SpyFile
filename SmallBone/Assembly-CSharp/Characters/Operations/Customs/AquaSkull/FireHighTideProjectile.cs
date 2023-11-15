using System;
using Characters.Projectiles;
using Characters.Utils;
using UnityEngine;

namespace Characters.Operations.Customs.AquaSkull
{
	// Token: 0x02001026 RID: 4134
	public class FireHighTideProjectile : CharacterOperation
	{
		// Token: 0x17000FAD RID: 4013
		// (get) Token: 0x06004FAF RID: 20399 RVA: 0x000F0380 File Offset: 0x000EE580
		public CustomFloat scale
		{
			get
			{
				return this._scale;
			}
		}

		// Token: 0x06004FB0 RID: 20400 RVA: 0x000F0388 File Offset: 0x000EE588
		protected override void OnDestroy()
		{
			base.OnDestroy();
			this._projectile = null;
		}

		// Token: 0x06004FB1 RID: 20401 RVA: 0x000F0397 File Offset: 0x000EE597
		public override void Initialize()
		{
			this._attackDamage = base.GetComponentInParent<IAttackDamage>();
			if (this._fireTransform == null)
			{
				this._fireTransform = base.transform;
			}
		}

		// Token: 0x06004FB2 RID: 20402 RVA: 0x000F03C0 File Offset: 0x000EE5C0
		private float GetPorjectileCount()
		{
			int num = 0;
			foreach (Projectile projectile in this._projectilesToCount)
			{
				num += projectile.reusable.spawnedCount;
			}
			int num2 = Mathf.Clamp(num, 0, this._countsByCount.Length - 1);
			return this._countsByCount[num2];
		}

		// Token: 0x06004FB3 RID: 20403 RVA: 0x000F0414 File Offset: 0x000EE614
		public override void Run(Character owner)
		{
			float porjectileCount = this.GetPorjectileCount();
			float attackDamage = this._attackDamage.amount * this._damageMultiplier.value;
			bool flipX = false;
			bool flipY = false;
			HitHistoryManager hitHistoryManager = this._group ? new HitHistoryManager(15) : null;
			int num = 0;
			while ((float)num < porjectileCount)
			{
				FireHighTideProjectile.DirectionType directionType = this._directionType;
				float num2;
				if (directionType != FireHighTideProjectile.DirectionType.RotationOfFirePosition)
				{
					if (directionType != FireHighTideProjectile.DirectionType.OwnerDirection)
					{
						num2 = this._direction.value;
					}
					else
					{
						num2 = this._direction.value;
						bool flag = owner.lookingDirection == Character.LookingDirection.Left || this._fireTransform.lossyScale.x < 0f;
						flipX = (this._flipXByOwnerDirection && flag);
						flipY = (this._flipYByOwnerDirection && flag);
						num2 = (flag ? ((180f - num2) % 360f) : num2);
					}
				}
				else
				{
					num2 = this._fireTransform.rotation.eulerAngles.z + this._direction.value;
					if (this._fireTransform.lossyScale.x < 0f)
					{
						num2 = (180f - num2) % 360f;
					}
				}
				Projectile component = this._projectile.reusable.Spawn(this._fireTransform.position, true).GetComponent<Projectile>();
				component.transform.localScale = Vector3.one * this._scale.value;
				component.Fire(owner, attackDamage, num2, flipX, flipY, this._speedMultiplier.value, this._group ? hitHistoryManager : null, this._fireInterval * (float)num);
				num++;
			}
		}

		// Token: 0x04004007 RID: 16391
		[SerializeField]
		private Projectile _projectile;

		// Token: 0x04004008 RID: 16392
		[Header("Special Setting")]
		[SerializeField]
		private Projectile[] _projectilesToCount;

		// Token: 0x04004009 RID: 16393
		[SerializeField]
		[Tooltip("발사 순서 * _fireInterval 만큼 대기한 후 발사됨")]
		private float _fireInterval;

		// Token: 0x0400400A RID: 16394
		[SerializeField]
		private float[] _countsByCount;

		// Token: 0x0400400B RID: 16395
		[Space]
		[SerializeField]
		private CustomFloat _speedMultiplier = new CustomFloat(1f);

		// Token: 0x0400400C RID: 16396
		[SerializeField]
		private CustomFloat _damageMultiplier = new CustomFloat(1f);

		// Token: 0x0400400D RID: 16397
		[SerializeField]
		private CustomFloat _scale = new CustomFloat(1f);

		// Token: 0x0400400E RID: 16398
		[Space]
		[SerializeField]
		private Transform _fireTransform;

		// Token: 0x0400400F RID: 16399
		[SerializeField]
		private bool _group;

		// Token: 0x04004010 RID: 16400
		[SerializeField]
		private bool _flipXByOwnerDirection;

		// Token: 0x04004011 RID: 16401
		[SerializeField]
		private bool _flipYByOwnerDirection;

		// Token: 0x04004012 RID: 16402
		[Space]
		[SerializeField]
		private FireHighTideProjectile.DirectionType _directionType;

		// Token: 0x04004013 RID: 16403
		[SerializeField]
		private CustomAngle _direction;

		// Token: 0x04004014 RID: 16404
		private IAttackDamage _attackDamage;

		// Token: 0x02001027 RID: 4135
		public enum DirectionType
		{
			// Token: 0x04004016 RID: 16406
			RotationOfFirePosition,
			// Token: 0x04004017 RID: 16407
			OwnerDirection,
			// Token: 0x04004018 RID: 16408
			Constant
		}
	}
}
