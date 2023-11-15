using System;
using System.Collections;
using System.Collections.Generic;
using Characters.Actions;
using Characters.Projectiles;
using FX;
using UnityEngine;

namespace Characters.Operations
{
	// Token: 0x02000DE4 RID: 3556
	public class AbyssSpike : CharacterOperation
	{
		// Token: 0x06004744 RID: 18244 RVA: 0x000CF143 File Offset: 0x000CD343
		public override void Initialize()
		{
			this._attackDamage = base.GetComponentInParent<IAttackDamage>();
		}

		// Token: 0x06004745 RID: 18245 RVA: 0x000CF151 File Offset: 0x000CD351
		public override void Run(Character owner)
		{
			base.StartCoroutine(this.CFire(owner));
		}

		// Token: 0x06004746 RID: 18246 RVA: 0x000CF161 File Offset: 0x000CD361
		private IEnumerator CFire(Character owner)
		{
			int count = (int)((float)(this._projectileCountMax - this._projectileCountMin) * this._chargeAction.chargedPercent) + this._projectileCountMin;
			Projectile projectile = (this._chargeAction.chargedPercent < 1f) ? this._incompleteProjectile : this._completeProjectile;
			Bounds bounds = this._area.bounds;
			Character.LookingDirection lookingDirection = owner.lookingDirection;
			int num;
			for (int i = 0; i < count; i = num + 1)
			{
				this.Fire(owner, projectile, bounds, lookingDirection);
				yield return owner.chronometer.animation.WaitForSeconds(this._fireInterval.value);
				num = i;
			}
			yield break;
		}

		// Token: 0x06004747 RID: 18247 RVA: 0x000CF178 File Offset: 0x000CD378
		private void Fire(Character owner, Projectile projectile, Bounds bounds, Character.LookingDirection lookingDirection)
		{
			CustomAngle[] values = this._directions.values;
			List<Vector2> list = new List<Vector2>(values.Length);
			for (int i = 0; i < values.Length; i++)
			{
				list.Add(MMMaths.RandomPointWithinBounds(bounds));
			}
			if (this._directionType == AbyssSpike.DirectionType.OwnerDirection)
			{
				for (int j = 0; j < values.Length; j++)
				{
					float num = values[j].value;
					if (this._spawnEffect != null)
					{
						this._spawnEffect.Spawn(list[j], owner, num, 1f);
					}
					bool flag = lookingDirection == Character.LookingDirection.Left;
					bool flipX = this._flipXByOwnerDirection && flag;
					bool flipY = this._flipYByOwnerDirection && flag;
					num = (flag ? ((180f - num) % 360f) : num);
					projectile.reusable.Spawn(list[j], true).GetComponent<Projectile>().Fire(owner, this._attackDamage.amount, num, flipX, flipY, 1f, null, 0f);
				}
				return;
			}
			for (int k = 0; k < values.Length; k++)
			{
				float num = values[k].value;
				if (this._spawnEffect != null)
				{
					this._spawnEffect.Spawn(list[k], owner, num, 1f);
				}
				if (this._area.transform.lossyScale.x < 0f)
				{
					num = (180f - num) % 360f;
				}
				projectile.reusable.Spawn(list[k], true).GetComponent<Projectile>().Fire(owner, this._attackDamage.amount, num, false, false, 1f, null, 0f);
			}
		}

		// Token: 0x04003645 RID: 13893
		[SerializeField]
		private ChargeAction _chargeAction;

		// Token: 0x04003646 RID: 13894
		[Tooltip("차지를 하나도 안 했을 때 프로젝타일 개수")]
		[SerializeField]
		[Space]
		private int _projectileCountMin = 1;

		// Token: 0x04003647 RID: 13895
		[SerializeField]
		[Tooltip("풀차지 했을 때 프로젝타일 개수")]
		private int _projectileCountMax = 10;

		// Token: 0x04003648 RID: 13896
		[SerializeField]
		[Space]
		[Tooltip("프로젝타일 발사 간격")]
		private CustomFloat _fireInterval;

		// Token: 0x04003649 RID: 13897
		[SerializeField]
		[Space]
		private EffectInfo _spawnEffect;

		// Token: 0x0400364A RID: 13898
		[SerializeField]
		private Projectile _incompleteProjectile;

		// Token: 0x0400364B RID: 13899
		[SerializeField]
		private Projectile _completeProjectile;

		// Token: 0x0400364C RID: 13900
		[SerializeField]
		private bool _flipXByOwnerDirection;

		// Token: 0x0400364D RID: 13901
		[SerializeField]
		private bool _flipYByOwnerDirection;

		// Token: 0x0400364E RID: 13902
		[SerializeField]
		private AbyssSpike.DirectionType _directionType;

		// Token: 0x0400364F RID: 13903
		[SerializeField]
		private CustomAngle.Reorderable _directions = new CustomAngle.Reorderable(new CustomAngle[]
		{
			new CustomAngle(0f)
		});

		// Token: 0x04003650 RID: 13904
		private IAttackDamage _attackDamage;

		// Token: 0x04003651 RID: 13905
		[SerializeField]
		private Collider2D _area;

		// Token: 0x02000DE5 RID: 3557
		public enum DirectionType
		{
			// Token: 0x04003653 RID: 13907
			OwnerDirection,
			// Token: 0x04003654 RID: 13908
			Constant
		}
	}
}
