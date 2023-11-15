using System;
using Characters.Operations;
using FX;
using UnityEngine;

namespace Characters.Projectiles.Operations
{
	// Token: 0x020007A0 RID: 1952
	public sealed class SummonOperationRunner : Operation
	{
		// Token: 0x060027E7 RID: 10215 RVA: 0x000783F5 File Offset: 0x000765F5
		private void OnDestroy()
		{
			this._operationRunner = null;
		}

		// Token: 0x060027E8 RID: 10216 RVA: 0x00078400 File Offset: 0x00076600
		public override void Run(IProjectile projectile)
		{
			Character owner = projectile.owner;
			Vector3 vector = (this._spawnPosition == null) ? base.transform.position : this._spawnPosition.position;
			if (this._snapToGround)
			{
				RaycastHit2D hit = Physics2D.Raycast(vector, Vector2.down, this._groundFindingDistance, Layers.groundMask);
				if (hit)
				{
					vector = hit.point;
				}
			}
			vector += this._noise.Evaluate();
			Vector3 vector2 = new Vector3(0f, 0f, this._angle.value);
			bool flag = this._flipXByLookingDirection && owner.lookingDirection == Character.LookingDirection.Left;
			if (flag)
			{
				vector2.z = (180f - vector2.z) % 360f;
			}
			OperationInfos operationInfos = this._operationRunner.Spawn().operationInfos;
			operationInfos.transform.SetPositionAndRotation(vector, Quaternion.Euler(vector2));
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

		// Token: 0x04002206 RID: 8710
		[Tooltip("오퍼레이션 프리팹")]
		[SerializeField]
		private OperationRunner _operationRunner;

		// Token: 0x04002207 RID: 8711
		[SerializeField]
		private Transform _spawnPosition;

		// Token: 0x04002208 RID: 8712
		[SerializeField]
		private CustomFloat _scale = new CustomFloat(1f);

		// Token: 0x04002209 RID: 8713
		[SerializeField]
		private CustomAngle _angle;

		// Token: 0x0400220A RID: 8714
		[SerializeField]
		private PositionNoise _noise;

		// Token: 0x0400220B RID: 8715
		[SerializeField]
		[Space]
		private bool _snapToGround;

		// Token: 0x0400220C RID: 8716
		[Tooltip("땅을 찾기 위해 소환지점으로부터 아래로 탐색할 거리. 실패 시 그냥 소환 지점에 소환됨")]
		[SerializeField]
		private float _groundFindingDistance = 7f;

		// Token: 0x0400220D RID: 8717
		[SerializeField]
		[Tooltip("주인이 바라보고 있는 방향에 따라 X축으로 플립")]
		private bool _flipXByLookingDirection;

		// Token: 0x0400220E RID: 8718
		[SerializeField]
		[Tooltip("체크하면 주인에 부착되며, 같이 움직임")]
		private bool _attachToOwner;
	}
}
