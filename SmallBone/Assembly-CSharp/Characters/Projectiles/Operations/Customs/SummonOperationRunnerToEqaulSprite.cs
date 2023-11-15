using System;
using Characters.Operations;
using FX;
using UnityEngine;

namespace Characters.Projectiles.Operations.Customs
{
	// Token: 0x020007A6 RID: 1958
	public sealed class SummonOperationRunnerToEqaulSprite : Operation
	{
		// Token: 0x060027FF RID: 10239 RVA: 0x00078F4E File Offset: 0x0007714E
		private void OnDestroy()
		{
			this._operationRunner = null;
		}

		// Token: 0x06002800 RID: 10240 RVA: 0x00078F58 File Offset: 0x00077158
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
			operationInfos.GetComponentInChildren<Animator>().Play(this._spriteRenderer.sprite.name);
		}

		// Token: 0x04002237 RID: 8759
		[Tooltip("오퍼레이션 프리팹")]
		[SerializeField]
		private OperationRunner _operationRunner;

		// Token: 0x04002238 RID: 8760
		[SerializeField]
		private SpriteRenderer _spriteRenderer;

		// Token: 0x04002239 RID: 8761
		[SerializeField]
		private Transform _spawnPosition;

		// Token: 0x0400223A RID: 8762
		[SerializeField]
		private CustomFloat _scale = new CustomFloat(1f);

		// Token: 0x0400223B RID: 8763
		[SerializeField]
		private CustomAngle _angle;

		// Token: 0x0400223C RID: 8764
		[SerializeField]
		private PositionNoise _noise;

		// Token: 0x0400223D RID: 8765
		[SerializeField]
		[Space]
		private bool _snapToGround;

		// Token: 0x0400223E RID: 8766
		[Tooltip("땅을 찾기 위해 소환지점으로부터 아래로 탐색할 거리. 실패 시 그냥 소환 지점에 소환됨")]
		[SerializeField]
		private float _groundFindingDistance = 7f;

		// Token: 0x0400223F RID: 8767
		[SerializeField]
		[Tooltip("주인이 바라보고 있는 방향에 따라 X축으로 플립")]
		private bool _flipXByLookingDirection;

		// Token: 0x04002240 RID: 8768
		[Tooltip("체크하면 주인에 부착되며, 같이 움직임")]
		[SerializeField]
		private bool _attachToOwner;
	}
}
