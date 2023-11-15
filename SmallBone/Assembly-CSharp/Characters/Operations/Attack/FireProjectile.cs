using System;
using Characters.Projectiles;
using Characters.Utils;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.Serialization;

namespace Characters.Operations.Attack
{
	// Token: 0x02000F7E RID: 3966
	public class FireProjectile : CharacterOperation
	{
		// Token: 0x17000F57 RID: 3927
		// (get) Token: 0x06004CF4 RID: 19700 RVA: 0x000E46BC File Offset: 0x000E28BC
		public CustomFloat scale
		{
			get
			{
				return this._scale;
			}
		}

		// Token: 0x06004CF5 RID: 19701 RVA: 0x000E46C4 File Offset: 0x000E28C4
		private void Awake()
		{
			if (this._projectileReference == null || !this._projectileReference.RuntimeKeyIsValid())
			{
				return;
			}
			this._projectileReferenceHandle = this._projectileReference.LoadAssetAsync<GameObject>();
		}

		// Token: 0x06004CF6 RID: 19702 RVA: 0x000E46ED File Offset: 0x000E28ED
		protected override void OnDestroy()
		{
			base.OnDestroy();
			this._projectile = null;
			if (this._projectileReferenceHandle.IsValid())
			{
				Addressables.Release<GameObject>(this._projectileReferenceHandle);
			}
		}

		// Token: 0x06004CF7 RID: 19703 RVA: 0x000E4714 File Offset: 0x000E2914
		public override void Initialize()
		{
			this._attackDamage = base.GetComponentInParent<IAttackDamage>();
			if (this._attackDamage == null)
			{
				Debug.LogError("AttackDamage가 없습니다");
			}
			if (this._fireTransform == null)
			{
				this._fireTransform = base.transform;
			}
		}

		// Token: 0x06004CF8 RID: 19704 RVA: 0x000E4750 File Offset: 0x000E2950
		public override void Run(Character owner)
		{
			if (this._projectile == null && this._projectileReferenceHandle.IsValid())
			{
				this._projectile = this._projectileReferenceHandle.WaitForCompletion().GetComponent<Projectile>();
			}
			CustomAngle[] values = this._directions.values;
			bool flipX = false;
			bool flipY = false;
			HitHistoryManager hitHistoryManager = this._group ? new HitHistoryManager(15) : null;
			for (int i = 0; i < values.Length; i++)
			{
				FireProjectile.DirectionType directionType = this._directionType;
				float num;
				if (directionType != FireProjectile.DirectionType.RotationOfFirePosition)
				{
					if (directionType != FireProjectile.DirectionType.OwnerDirection)
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
				if (this._attackDamage == null)
				{
					this._attackDamage = base.GetComponentInParent<IAttackDamage>();
					if (this._attackDamage == null)
					{
						Debug.LogError("발사 시점에도 AttackDamage가 없습니다");
						return;
					}
				}
				Projectile component = this._projectile.reusable.Spawn(this._fireTransform.position, true).GetComponent<Projectile>();
				component.transform.localScale = Vector3.one * this._scale.value;
				component.Fire(owner, this._attackDamage.amount * this._damageMultiplier.value, num, flipX, flipY, this._speedMultiplier.value, this._group ? hitHistoryManager : null, 0f);
			}
		}

		// Token: 0x04003C97 RID: 15511
		[SerializeField]
		private Projectile _projectile;

		// Token: 0x04003C98 RID: 15512
		[SerializeField]
		private AssetReference _projectileReference;

		// Token: 0x04003C99 RID: 15513
		private AsyncOperationHandle<GameObject> _projectileReferenceHandle;

		// Token: 0x04003C9A RID: 15514
		[Space]
		[SerializeField]
		private CustomFloat _speedMultiplier = new CustomFloat(1f);

		// Token: 0x04003C9B RID: 15515
		[SerializeField]
		private CustomFloat _damageMultiplier = new CustomFloat(1f);

		// Token: 0x04003C9C RID: 15516
		[SerializeField]
		private CustomFloat _scale = new CustomFloat(1f);

		// Token: 0x04003C9D RID: 15517
		[SerializeField]
		[Space]
		private Transform _fireTransform;

		// Token: 0x04003C9E RID: 15518
		[SerializeField]
		private bool _group;

		// Token: 0x04003C9F RID: 15519
		[SerializeField]
		private bool _flipXByOwnerDirection;

		// Token: 0x04003CA0 RID: 15520
		[FormerlySerializedAs("_flipY")]
		[SerializeField]
		private bool _flipYByOwnerDirection;

		// Token: 0x04003CA1 RID: 15521
		[SerializeField]
		[Space]
		private FireProjectile.DirectionType _directionType;

		// Token: 0x04003CA2 RID: 15522
		[SerializeField]
		private CustomAngle.Reorderable _directions;

		// Token: 0x04003CA3 RID: 15523
		private IAttackDamage _attackDamage;

		// Token: 0x02000F7F RID: 3967
		public enum DirectionType
		{
			// Token: 0x04003CA5 RID: 15525
			RotationOfFirePosition,
			// Token: 0x04003CA6 RID: 15526
			OwnerDirection,
			// Token: 0x04003CA7 RID: 15527
			Constant
		}
	}
}
