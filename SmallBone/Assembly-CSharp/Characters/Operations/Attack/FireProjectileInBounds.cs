using System;
using Characters.Projectiles;
using FX;
using UnityEngine;

namespace Characters.Operations.Attack
{
	// Token: 0x02000F80 RID: 3968
	public class FireProjectileInBounds : CharacterOperation
	{
		// Token: 0x06004CFA RID: 19706 RVA: 0x000E496C File Offset: 0x000E2B6C
		protected override void OnDestroy()
		{
			base.OnDestroy();
			this._projectile = null;
		}

		// Token: 0x06004CFB RID: 19707 RVA: 0x000E497B File Offset: 0x000E2B7B
		public override void Initialize()
		{
			this._attackDamage = base.GetComponentInParent<IAttackDamage>();
			if (this._rotationReference == null)
			{
				this._rotationReference = base.transform;
			}
		}

		// Token: 0x06004CFC RID: 19708 RVA: 0x000E49A4 File Offset: 0x000E2BA4
		public override void Run(Character owner)
		{
			CustomAngle[] values = this._directions.values;
			bool flipX = false;
			bool flipY = false;
			for (int i = 0; i < values.Length; i++)
			{
				FireProjectileInBounds.DirectionType directionType = this._directionType;
				float num;
				if (directionType != FireProjectileInBounds.DirectionType.OwnerDirection)
				{
					if (directionType == FireProjectileInBounds.DirectionType.RotationOfReferenceTransform)
					{
						num = this._rotationReference.rotation.eulerAngles.z + values[i].value;
						if (this._rotationReference.lossyScale.x < 0f)
						{
							num = (180f - num) % 360f;
						}
					}
					else
					{
						num = values[i].value;
					}
				}
				else
				{
					num = values[i].value;
					bool flag = owner.lookingDirection == Character.LookingDirection.Left || this._area.transform.lossyScale.x < 0f;
					flipX = (this._flipXByOwnerDirection && flag);
					flipY = (this._flipYByOwnerDirection && flag);
					num = (flag ? ((180f - num) % 360f) : num);
				}
				Projectile component = this._projectile.reusable.Spawn(MMMaths.RandomPointWithinBounds(this._area.bounds), true).GetComponent<Projectile>();
				component.transform.localScale = Vector3.one * this._scale.value;
				component.Fire(owner, this._attackDamage.amount * this._damageMultiplier.value, num, flipX, flipY, this._speedMultiplier.value, null, 0f);
				this._spawnEffect.Spawn(component.transform.position, owner, 0f, 1f);
			}
		}

		// Token: 0x04003CA8 RID: 15528
		[SerializeField]
		private Projectile _projectile;

		// Token: 0x04003CA9 RID: 15529
		[Space]
		[SerializeField]
		private CustomFloat _speedMultiplier = new CustomFloat(1f);

		// Token: 0x04003CAA RID: 15530
		[SerializeField]
		private CustomFloat _damageMultiplier = new CustomFloat(1f);

		// Token: 0x04003CAB RID: 15531
		[SerializeField]
		private CustomFloat _scale = new CustomFloat(1f);

		// Token: 0x04003CAC RID: 15532
		[Space]
		[SerializeField]
		private Collider2D _area;

		// Token: 0x04003CAD RID: 15533
		[SerializeField]
		private EffectInfo _spawnEffect;

		// Token: 0x04003CAE RID: 15534
		[SerializeField]
		[Space]
		private bool _flipXByOwnerDirection;

		// Token: 0x04003CAF RID: 15535
		[SerializeField]
		private bool _flipYByOwnerDirection;

		// Token: 0x04003CB0 RID: 15536
		[Space]
		[SerializeField]
		private FireProjectileInBounds.DirectionType _directionType;

		// Token: 0x04003CB1 RID: 15537
		[SerializeField]
		[Tooltip("DirectionType을 ReferenceTransform으로 설정했을 경우 이 Transform을 참조합니다.")]
		private Transform _rotationReference;

		// Token: 0x04003CB2 RID: 15538
		[SerializeField]
		private CustomAngle.Reorderable _directions = new CustomAngle.Reorderable(new CustomAngle[]
		{
			new CustomAngle(0f)
		});

		// Token: 0x04003CB3 RID: 15539
		private IAttackDamage _attackDamage;

		// Token: 0x02000F81 RID: 3969
		public enum DirectionType
		{
			// Token: 0x04003CB5 RID: 15541
			OwnerDirection,
			// Token: 0x04003CB6 RID: 15542
			Constant,
			// Token: 0x04003CB7 RID: 15543
			RotationOfReferenceTransform
		}
	}
}
