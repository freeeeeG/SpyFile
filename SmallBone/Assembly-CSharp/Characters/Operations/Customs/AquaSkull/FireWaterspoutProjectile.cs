using System;
using Characters.Projectiles;
using Characters.Utils;
using UnityEngine;

namespace Characters.Operations.Customs.AquaSkull
{
	// Token: 0x0200102A RID: 4138
	public class FireWaterspoutProjectile : CharacterOperation
	{
		// Token: 0x17000FAF RID: 4015
		// (get) Token: 0x06004FBB RID: 20411 RVA: 0x000F0850 File Offset: 0x000EEA50
		public CustomFloat scale
		{
			get
			{
				return this._scale;
			}
		}

		// Token: 0x06004FBC RID: 20412 RVA: 0x000F0858 File Offset: 0x000EEA58
		protected override void OnDestroy()
		{
			base.OnDestroy();
			this._projectile = null;
		}

		// Token: 0x06004FBD RID: 20413 RVA: 0x000F0867 File Offset: 0x000EEA67
		public override void Initialize()
		{
			this._attackDamage = base.GetComponentInParent<IAttackDamage>();
			if (this._fireTransform == null)
			{
				this._fireTransform = base.transform;
			}
		}

		// Token: 0x06004FBE RID: 20414 RVA: 0x000F0890 File Offset: 0x000EEA90
		private float GetProjectileLifeTime()
		{
			int num = 0;
			foreach (Projectile projectile in this._projectilesToCount)
			{
				num += projectile.reusable.spawnedCount;
			}
			int num2 = Mathf.Clamp(num, 0, this._durationsByCount.Length - 1);
			return this._durationsByCount[num2];
		}

		// Token: 0x06004FBF RID: 20415 RVA: 0x000F08E4 File Offset: 0x000EEAE4
		public override void Run(Character owner)
		{
			CustomAngle[] values = this._directions.values;
			float attackDamage = this._attackDamage.amount * this._damageMultiplier.value;
			bool flipX = false;
			bool flipY = false;
			HitHistoryManager hitHistoryManager = this._group ? new HitHistoryManager(15) : null;
			float projectileLifeTime = this.GetProjectileLifeTime();
			for (int i = 0; i < values.Length; i++)
			{
				FireWaterspoutProjectile.DirectionType directionType = this._directionType;
				float num;
				if (directionType != FireWaterspoutProjectile.DirectionType.RotationOfFirePosition)
				{
					if (directionType != FireWaterspoutProjectile.DirectionType.OwnerDirection)
					{
						num = values[i].value;
					}
					else
					{
						num = values[i].value;
						bool flag = owner.lookingDirection == Character.LookingDirection.Left || this._fireTransform.lossyScale.x < 0f;
						flipX = (this._flipXByOwnerDirection && flag);
						flipY = (this._flipYByOwnerDirection && flag);
						num = (flag ? ((180f - num) % 360f) : num);
					}
				}
				else
				{
					num = this._fireTransform.rotation.eulerAngles.z + values[i].value;
					if (this._fireTransform.lossyScale.x < 0f)
					{
						num = (180f - num) % 360f;
					}
				}
				Projectile component = this._projectile.reusable.Spawn(this._fireTransform.position, true).GetComponent<Projectile>();
				component.transform.localScale = Vector3.one * this._scale.value;
				component.maxLifeTime = projectileLifeTime;
				component.Fire(owner, attackDamage, num, flipX, flipY, this._speedMultiplier.value, this._group ? hitHistoryManager : null, 0f);
			}
		}

		// Token: 0x0400402A RID: 16426
		[SerializeField]
		private Projectile _projectile;

		// Token: 0x0400402B RID: 16427
		[SerializeField]
		[Header("Special Setting")]
		private Projectile[] _projectilesToCount;

		// Token: 0x0400402C RID: 16428
		[SerializeField]
		private float[] _durationsByCount;

		// Token: 0x0400402D RID: 16429
		[Space]
		[SerializeField]
		private CustomFloat _speedMultiplier = new CustomFloat(1f);

		// Token: 0x0400402E RID: 16430
		[SerializeField]
		private CustomFloat _damageMultiplier = new CustomFloat(1f);

		// Token: 0x0400402F RID: 16431
		[SerializeField]
		private CustomFloat _scale = new CustomFloat(1f);

		// Token: 0x04004030 RID: 16432
		[Space]
		[SerializeField]
		private Transform _fireTransform;

		// Token: 0x04004031 RID: 16433
		[SerializeField]
		private bool _group;

		// Token: 0x04004032 RID: 16434
		[SerializeField]
		private bool _flipXByOwnerDirection;

		// Token: 0x04004033 RID: 16435
		[SerializeField]
		private bool _flipYByOwnerDirection;

		// Token: 0x04004034 RID: 16436
		[SerializeField]
		[Space]
		private FireWaterspoutProjectile.DirectionType _directionType;

		// Token: 0x04004035 RID: 16437
		[SerializeField]
		private CustomAngle.Reorderable _directions;

		// Token: 0x04004036 RID: 16438
		private IAttackDamage _attackDamage;

		// Token: 0x0200102B RID: 4139
		public enum DirectionType
		{
			// Token: 0x04004038 RID: 16440
			RotationOfFirePosition,
			// Token: 0x04004039 RID: 16441
			OwnerDirection,
			// Token: 0x0400403A RID: 16442
			Constant
		}
	}
}
