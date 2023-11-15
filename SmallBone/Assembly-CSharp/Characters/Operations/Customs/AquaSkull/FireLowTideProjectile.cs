using System;
using Characters.Projectiles;
using Characters.Utils;
using UnityEngine;

namespace Characters.Operations.Customs.AquaSkull
{
	// Token: 0x02001028 RID: 4136
	public class FireLowTideProjectile : CharacterOperation
	{
		// Token: 0x17000FAE RID: 4014
		// (get) Token: 0x06004FB5 RID: 20405 RVA: 0x000F05E2 File Offset: 0x000EE7E2
		public CustomFloat scale
		{
			get
			{
				return this._scale;
			}
		}

		// Token: 0x06004FB6 RID: 20406 RVA: 0x000F05EA File Offset: 0x000EE7EA
		protected override void OnDestroy()
		{
			base.OnDestroy();
			this._projectile = null;
		}

		// Token: 0x06004FB7 RID: 20407 RVA: 0x000F05F9 File Offset: 0x000EE7F9
		public override void Initialize()
		{
			this._attackDamage = base.GetComponentInParent<IAttackDamage>();
			if (this._fireTransform == null)
			{
				this._fireTransform = base.transform;
			}
		}

		// Token: 0x06004FB8 RID: 20408 RVA: 0x000F0624 File Offset: 0x000EE824
		private float GetProjectileScale()
		{
			int num = 0;
			foreach (Projectile projectile in this._projectilesToCount)
			{
				num += projectile.reusable.spawnedCount;
			}
			int num2 = Mathf.Clamp(num, 0, this._scalesByCount.Length - 1);
			return this._scalesByCount[num2];
		}

		// Token: 0x06004FB9 RID: 20409 RVA: 0x000F0678 File Offset: 0x000EE878
		public override void Run(Character owner)
		{
			CustomAngle[] values = this._directions.values;
			float attackDamage = this._attackDamage.amount * this._damageMultiplier.value;
			bool flipX = false;
			bool flipY = false;
			HitHistoryManager hitHistoryManager = this._group ? new HitHistoryManager(15) : null;
			float projectileScale = this.GetProjectileScale();
			for (int i = 0; i < values.Length; i++)
			{
				FireLowTideProjectile.DirectionType directionType = this._directionType;
				float num;
				if (directionType != FireLowTideProjectile.DirectionType.RotationOfFirePosition)
				{
					if (directionType != FireLowTideProjectile.DirectionType.OwnerDirection)
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
				component.transform.localScale = Vector3.one * this._scale.value * projectileScale;
				component.Fire(owner, attackDamage, num, flipX, flipY, this._speedMultiplier.value, this._group ? hitHistoryManager : null, 0f);
			}
		}

		// Token: 0x04004019 RID: 16409
		[SerializeField]
		private Projectile _projectile;

		// Token: 0x0400401A RID: 16410
		[Header("Special Setting")]
		[SerializeField]
		private Projectile[] _projectilesToCount;

		// Token: 0x0400401B RID: 16411
		[SerializeField]
		private float[] _scalesByCount;

		// Token: 0x0400401C RID: 16412
		[Space]
		[SerializeField]
		private CustomFloat _speedMultiplier = new CustomFloat(1f);

		// Token: 0x0400401D RID: 16413
		[SerializeField]
		private CustomFloat _damageMultiplier = new CustomFloat(1f);

		// Token: 0x0400401E RID: 16414
		[SerializeField]
		private CustomFloat _scale = new CustomFloat(1f);

		// Token: 0x0400401F RID: 16415
		[SerializeField]
		[Space]
		private Transform _fireTransform;

		// Token: 0x04004020 RID: 16416
		[SerializeField]
		private bool _group;

		// Token: 0x04004021 RID: 16417
		[SerializeField]
		private bool _flipXByOwnerDirection;

		// Token: 0x04004022 RID: 16418
		[SerializeField]
		private bool _flipYByOwnerDirection;

		// Token: 0x04004023 RID: 16419
		[Space]
		[SerializeField]
		private FireLowTideProjectile.DirectionType _directionType;

		// Token: 0x04004024 RID: 16420
		[SerializeField]
		private CustomAngle.Reorderable _directions;

		// Token: 0x04004025 RID: 16421
		private IAttackDamage _attackDamage;

		// Token: 0x02001029 RID: 4137
		public enum DirectionType
		{
			// Token: 0x04004027 RID: 16423
			RotationOfFirePosition,
			// Token: 0x04004028 RID: 16424
			OwnerDirection,
			// Token: 0x04004029 RID: 16425
			Constant
		}
	}
}
