using System;
using Characters.Projectiles;
using Characters.Utils;
using UnityEngine;
using UnityEngine.Serialization;

namespace Characters.Operations.Attack
{
	// Token: 0x02000F7C RID: 3964
	public class FireBouncyProjectile : CharacterOperation
	{
		// Token: 0x17000F56 RID: 3926
		// (get) Token: 0x06004CED RID: 19693 RVA: 0x000E4431 File Offset: 0x000E2631
		public CustomFloat scale
		{
			get
			{
				return this._scale;
			}
		}

		// Token: 0x06004CEE RID: 19694 RVA: 0x000E4439 File Offset: 0x000E2639
		private void Awake()
		{
			this._firePosition = this._fireTransform.localPosition;
		}

		// Token: 0x06004CEF RID: 19695 RVA: 0x000E4451 File Offset: 0x000E2651
		protected override void OnDestroy()
		{
			base.OnDestroy();
			this._projectile = null;
		}

		// Token: 0x06004CF0 RID: 19696 RVA: 0x000E4460 File Offset: 0x000E2660
		public override void Initialize()
		{
			this._attackDamage = base.GetComponentInParent<IAttackDamage>();
		}

		// Token: 0x06004CF1 RID: 19697 RVA: 0x000E4470 File Offset: 0x000E2670
		private bool FindFirePosition(Vector2 offset)
		{
			ContactFilter2D contactFilter = default(ContactFilter2D);
			contactFilter.SetLayerMask(Layers.groundMask);
			Collider2D[] results = new Collider2D[1];
			this._fireTransform.localPosition = this._firePosition + offset;
			Physics2D.SyncTransforms();
			return Physics2D.OverlapCollider(this._collider, contactFilter, results) == 0;
		}

		// Token: 0x06004CF2 RID: 19698 RVA: 0x000E44CC File Offset: 0x000E26CC
		public override void Run(Character owner)
		{
			int num = 0;
			while (num < this._positionsToInterpolate.Length && !this.FindFirePosition(this._positionsToInterpolate[num]))
			{
				num++;
			}
			CustomAngle[] values = this._directions.values;
			bool flipX = false;
			bool flipY = false;
			HitHistoryManager hitHistoryManager = this._group ? new HitHistoryManager(15) : null;
			for (int i = 0; i < values.Length; i++)
			{
				FireBouncyProjectile.DirectionType directionType = this._directionType;
				float num2;
				if (directionType != FireBouncyProjectile.DirectionType.RotationOfFirePosition)
				{
					if (directionType != FireBouncyProjectile.DirectionType.OwnerDirection)
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
				BouncyProjectile component = this._projectile.reusable.Spawn(this._fireTransform.position, true).GetComponent<BouncyProjectile>();
				component.transform.localScale = Vector3.one * this._scale.value;
				component.Fire(owner, this._attackDamage.amount * this._damageMultiplier.value, num2, flipX, flipY, this._speedMultiplier.value, this._group ? hitHistoryManager : null, 0f);
			}
		}

		// Token: 0x04003C85 RID: 15493
		[SerializeField]
		private BouncyProjectile _projectile;

		// Token: 0x04003C86 RID: 15494
		[Space]
		[SerializeField]
		private CustomFloat _speedMultiplier = new CustomFloat(1f);

		// Token: 0x04003C87 RID: 15495
		[SerializeField]
		private CustomFloat _damageMultiplier = new CustomFloat(1f);

		// Token: 0x04003C88 RID: 15496
		[SerializeField]
		private CustomFloat _scale = new CustomFloat(1f);

		// Token: 0x04003C89 RID: 15497
		[SerializeField]
		[Space]
		private bool _group;

		// Token: 0x04003C8A RID: 15498
		[SerializeField]
		private bool _flipXByOwnerDirection;

		// Token: 0x04003C8B RID: 15499
		[FormerlySerializedAs("_flipY")]
		[SerializeField]
		private bool _flipYByOwnerDirection;

		// Token: 0x04003C8C RID: 15500
		[SerializeField]
		[Space]
		private FireBouncyProjectile.DirectionType _directionType;

		// Token: 0x04003C8D RID: 15501
		[SerializeField]
		private CustomAngle.Reorderable _directions;

		// Token: 0x04003C8E RID: 15502
		[SerializeField]
		[Space]
		private Transform _fireTransform;

		// Token: 0x04003C8F RID: 15503
		[SerializeField]
		private Collider2D _collider;

		// Token: 0x04003C90 RID: 15504
		[SerializeField]
		private Vector2[] _positionsToInterpolate;

		// Token: 0x04003C91 RID: 15505
		private Vector2 _firePosition;

		// Token: 0x04003C92 RID: 15506
		private IAttackDamage _attackDamage;

		// Token: 0x02000F7D RID: 3965
		public enum DirectionType
		{
			// Token: 0x04003C94 RID: 15508
			RotationOfFirePosition,
			// Token: 0x04003C95 RID: 15509
			OwnerDirection,
			// Token: 0x04003C96 RID: 15510
			Constant
		}
	}
}
