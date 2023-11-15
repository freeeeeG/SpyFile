using System;
using Characters.Operations;
using FX;
using UnityEngine;
using UnityEngine.Rendering;

namespace Characters.Projectiles.Operations
{
	// Token: 0x020007A1 RID: 1953
	public sealed class SummonOperationRunnerOnHitPoint : HitOperation
	{
		// Token: 0x060027EA RID: 10218 RVA: 0x000785A1 File Offset: 0x000767A1
		private void OnDestroy()
		{
			this._operationRunner = null;
		}

		// Token: 0x060027EB RID: 10219 RVA: 0x000785AC File Offset: 0x000767AC
		public override void Run(IProjectile projectile, RaycastHit2D raycastHit)
		{
			Character owner = projectile.owner;
			Vector2 vector = raycastHit.point + this._noise.Evaluate();
			vector = new Vector2(vector.x + this._offsetX.value, vector.y + this._offsetY.value);
			if (this._interpolatedPosition)
			{
				this.GetInterpolatedPosition(projectile, raycastHit, ref vector);
			}
			Vector3 vector2 = new Vector3(0f, 0f, this._angle.value);
			bool flag = this._flipXByLookingDirection && owner.lookingDirection == Character.LookingDirection.Left;
			if (flag)
			{
				vector2.z = (180f - vector2.z) % 360f;
			}
			OperationRunner operationRunner = this._operationRunner.Spawn();
			OperationInfos operationInfos = operationRunner.operationInfos;
			operationInfos.transform.SetPositionAndRotation(vector, Quaternion.Euler(vector2));
			SortingGroup component = operationRunner.GetComponent<SortingGroup>();
			if (component != null)
			{
				SortingGroup sortingGroup = component;
				short num = SummonOperationRunnerOnHitPoint.spriteLayer;
				SummonOperationRunnerOnHitPoint.spriteLayer = num + 1;
				sortingGroup.sortingOrder = (int)num;
			}
			if (flag)
			{
				operationInfos.transform.localScale = new Vector3(1f, -1f, 1f) * this._scale.value;
			}
			else
			{
				operationInfos.transform.localScale = new Vector3(1f, 1f, 1f) * this._scale.value;
			}
			operationInfos.Run(owner);
			if (this._attachToOwner)
			{
				operationInfos.transform.parent = base.transform;
			}
		}

		// Token: 0x060027EC RID: 10220 RVA: 0x00078738 File Offset: 0x00076938
		private void GetInterpolatedPosition(IProjectile projectile, RaycastHit2D hit, ref Vector2 result)
		{
			float num = this._interpolatedSize / 2f;
			Bounds bounds = hit.collider.bounds;
			Vector2 point = hit.point;
			float num2 = projectile.transform.position.x + num;
			float num3 = projectile.transform.position.x - num;
			if (num2 > bounds.max.x)
			{
				result = new Vector2(bounds.max.x - num, result.y);
			}
			if (num3 < bounds.min.x)
			{
				result = new Vector2(bounds.min.x + num, result.y);
			}
		}

		// Token: 0x0400220F RID: 8719
		private static short spriteLayer = short.MinValue;

		// Token: 0x04002210 RID: 8720
		[SerializeField]
		[Tooltip("오퍼레이션 프리팹")]
		private OperationRunner _operationRunner;

		// Token: 0x04002211 RID: 8721
		[SerializeField]
		private CustomFloat _offsetX;

		// Token: 0x04002212 RID: 8722
		[SerializeField]
		private CustomFloat _offsetY;

		// Token: 0x04002213 RID: 8723
		[SerializeField]
		private CustomFloat _scale = new CustomFloat(1f);

		// Token: 0x04002214 RID: 8724
		[SerializeField]
		private CustomAngle _angle;

		// Token: 0x04002215 RID: 8725
		[SerializeField]
		private PositionNoise _noise;

		// Token: 0x04002216 RID: 8726
		[Tooltip("주인이 바라보고 있는 방향에 따라 X축으로 플립")]
		[SerializeField]
		private bool _flipXByLookingDirection;

		// Token: 0x04002217 RID: 8727
		[SerializeField]
		[Tooltip("체크하면 주인에 부착되며, 같이 움직임")]
		private bool _attachToOwner;

		// Token: 0x04002218 RID: 8728
		[Header("Interporlation")]
		[Tooltip("콜라이더 끝에 걸쳤을 때 자연스럽게 보이기 위해 위치 보간")]
		[SerializeField]
		private bool _interpolatedPosition;

		// Token: 0x04002219 RID: 8729
		[SerializeField]
		private float _interpolatedSize;
	}
}
