using System;
using Characters.Abilities.Weapons.DavyJones;
using Characters.Operations.Attack;
using Characters.Projectiles;
using Characters.Utils;
using UnityEngine;

namespace Characters.Operations.Customs.DavyJones
{
	// Token: 0x0200101A RID: 4122
	public sealed class FireCannonBall : CharacterOperation
	{
		// Token: 0x06004F90 RID: 20368 RVA: 0x000EF6EF File Offset: 0x000ED8EF
		public override void Initialize()
		{
			this._attackDamage = base.GetComponentInParent<IAttackDamage>();
			if (this._fireTransform == null)
			{
				this._fireTransform = base.transform;
			}
		}

		// Token: 0x06004F91 RID: 20369 RVA: 0x000EF718 File Offset: 0x000ED918
		public override void Run(Character owner)
		{
			CustomAngle[] values = this._directions.values;
			bool flipX = false;
			bool flipY = false;
			Projectile projectile = this._selector.GetProjectile(this._passive);
			this._passive.Pop();
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
				Projectile component = projectile.reusable.Spawn(this._fireTransform.position, true).GetComponent<Projectile>();
				component.transform.localScale = Vector3.one * this._scale.value;
				component.Fire(owner, this._attackDamage.amount * this._damageMultiplier.value, num, flipX, flipY, this._speedMultiplier.value, this._group ? hitHistoryManager : null, 0f);
			}
		}

		// Token: 0x04003FC9 RID: 16329
		[SerializeField]
		private DavyJonesPassiveComponent _passive;

		// Token: 0x04003FCA RID: 16330
		[SerializeField]
		private FireCannonBall.ProjectileSelector _selector;

		// Token: 0x04003FCB RID: 16331
		[SerializeField]
		[Space]
		private CustomFloat _speedMultiplier = new CustomFloat(1f);

		// Token: 0x04003FCC RID: 16332
		[SerializeField]
		private CustomFloat _damageMultiplier = new CustomFloat(1f);

		// Token: 0x04003FCD RID: 16333
		[SerializeField]
		private CustomFloat _scale = new CustomFloat(1f);

		// Token: 0x04003FCE RID: 16334
		[SerializeField]
		[Space]
		private Transform _fireTransform;

		// Token: 0x04003FCF RID: 16335
		[SerializeField]
		private bool _group;

		// Token: 0x04003FD0 RID: 16336
		[SerializeField]
		private bool _flipXByOwnerDirection;

		// Token: 0x04003FD1 RID: 16337
		[SerializeField]
		private bool _flipYByOwnerDirection;

		// Token: 0x04003FD2 RID: 16338
		[Space]
		[SerializeField]
		private FireProjectile.DirectionType _directionType;

		// Token: 0x04003FD3 RID: 16339
		[SerializeField]
		private CustomAngle.Reorderable _directions;

		// Token: 0x04003FD4 RID: 16340
		private IAttackDamage _attackDamage;

		// Token: 0x0200101B RID: 4123
		[Serializable]
		private class ProjectileSelector
		{
			// Token: 0x06004F93 RID: 20371 RVA: 0x000EF8F8 File Offset: 0x000EDAF8
			public Projectile GetProjectile(DavyJonesPassiveComponent passive)
			{
				foreach (FireCannonBall.ProjectileSelector.ProjectileByCannonBallType projectileByCannonBallType in this._projectiles)
				{
					if (passive.IsTop(projectileByCannonBallType.type))
					{
						return projectileByCannonBallType.projectilePrefab;
					}
				}
				return null;
			}

			// Token: 0x04003FD5 RID: 16341
			[SerializeField]
			private FireCannonBall.ProjectileSelector.ProjectileByCannonBallType[] _projectiles;

			// Token: 0x0200101C RID: 4124
			[Serializable]
			private struct ProjectileByCannonBallType
			{
				// Token: 0x04003FD6 RID: 16342
				[SerializeField]
				internal CannonBallType type;

				// Token: 0x04003FD7 RID: 16343
				[SerializeField]
				internal Projectile projectilePrefab;
			}
		}
	}
}
