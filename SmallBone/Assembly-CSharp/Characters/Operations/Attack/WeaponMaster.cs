using System;
using Characters.Operations.Fx;
using Characters.Projectiles;
using FX;
using Level;
using Unity.Mathematics;
using UnityEditor;
using UnityEngine;

namespace Characters.Operations.Attack
{
	// Token: 0x02000FA3 RID: 4003
	public sealed class WeaponMaster : CharacterOperation
	{
		// Token: 0x06004DC6 RID: 19910 RVA: 0x000E8870 File Offset: 0x000E6A70
		public override void Initialize()
		{
			this._attackDamage = base.GetComponentInParent<IAttackDamage>();
			if (this._rotationReference == null)
			{
				this._rotationReference = base.transform;
			}
			this._spawnSound.Initialize();
		}

		// Token: 0x06004DC7 RID: 19911 RVA: 0x000E88A4 File Offset: 0x000E6AA4
		public override void Run(Character owner)
		{
			float num = (float)this._minCount + this._multiplier * (float)math.min(this._maxEnemyCount, Map.Instance.waveContainer.GetAllSpawnedEnemiesCount());
			bool flipX = false;
			bool flipY = false;
			int num2 = 0;
			while ((float)num2 < num)
			{
				WeaponMaster.DirectionType directionType = this._directionType;
				float num3;
				if (directionType != WeaponMaster.DirectionType.OwnerDirection)
				{
					if (directionType == WeaponMaster.DirectionType.RotationOfReferenceTransform)
					{
						num3 = this._rotationReference.rotation.eulerAngles.z + this._direction.value;
						if (this._rotationReference.lossyScale.x < 0f)
						{
							num3 = (180f - num3) % 360f;
						}
					}
					else
					{
						num3 = this._direction.value;
					}
				}
				else
				{
					num3 = this._direction.value;
					bool flag = owner.lookingDirection == Character.LookingDirection.Left || this._area.transform.lossyScale.x < 0f;
					flipX = (this._flipXByOwnerDirection && flag);
					flipY = (this._flipYByOwnerDirection && flag);
					num3 = (flag ? ((180f - num3) % 360f) : num3);
				}
				IProjectile component = this._projectile.reusable.Spawn(MMMaths.RandomPointWithinBounds(this._area.bounds), true).GetComponent<IProjectile>();
				component.transform.localScale = Vector3.one * this._scale.value;
				component.Fire(owner, this._attackDamage.amount * this._damageMultiplier.value, num3, flipX, flipY, this._speedMultiplier.value, null, 0f);
				this._spawnEffect.Spawn(component.transform.position, 0f, 1f);
				this._spawnSound.Run(owner);
				num2++;
			}
		}

		// Token: 0x04003D9C RID: 15772
		[SerializeField]
		private WeaponMasterProjectile _projectile;

		// Token: 0x04003D9D RID: 15773
		[Space]
		[SerializeField]
		private CustomFloat _speedMultiplier = new CustomFloat(1f);

		// Token: 0x04003D9E RID: 15774
		[SerializeField]
		private CustomFloat _damageMultiplier = new CustomFloat(1f);

		// Token: 0x04003D9F RID: 15775
		[SerializeField]
		private CustomFloat _scale = new CustomFloat(1f);

		// Token: 0x04003DA0 RID: 15776
		[SerializeField]
		[Space]
		private Collider2D _area;

		// Token: 0x04003DA1 RID: 15777
		[SerializeField]
		private EffectInfo _spawnEffect;

		// Token: 0x04003DA2 RID: 15778
		[SerializeField]
		[UnityEditor.Subcomponent(typeof(PlaySound))]
		private PlaySound _spawnSound;

		// Token: 0x04003DA3 RID: 15779
		[Space]
		[SerializeField]
		private bool _flipXByOwnerDirection;

		// Token: 0x04003DA4 RID: 15780
		[SerializeField]
		private bool _flipYByOwnerDirection;

		// Token: 0x04003DA5 RID: 15781
		[Space]
		[SerializeField]
		private int _minCount = 1;

		// Token: 0x04003DA6 RID: 15782
		[SerializeField]
		private int _maxEnemyCount = 15;

		// Token: 0x04003DA7 RID: 15783
		[SerializeField]
		private float _multiplier;

		// Token: 0x04003DA8 RID: 15784
		[SerializeField]
		private WeaponMaster.DirectionType _directionType;

		// Token: 0x04003DA9 RID: 15785
		[SerializeField]
		[Tooltip("DirectionType을 ReferenceTransform으로 설정했을 경우 이 Transform을 참조합니다.")]
		private Transform _rotationReference;

		// Token: 0x04003DAA RID: 15786
		[SerializeField]
		private CustomAngle _direction;

		// Token: 0x04003DAB RID: 15787
		private IAttackDamage _attackDamage;

		// Token: 0x02000FA4 RID: 4004
		public enum DirectionType
		{
			// Token: 0x04003DAD RID: 15789
			OwnerDirection,
			// Token: 0x04003DAE RID: 15790
			Constant,
			// Token: 0x04003DAF RID: 15791
			RotationOfReferenceTransform
		}
	}
}
