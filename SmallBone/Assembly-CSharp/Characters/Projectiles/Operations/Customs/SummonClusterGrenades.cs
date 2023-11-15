using System;
using Characters.Operations;
using FX;
using Unity.Mathematics;
using UnityEngine;

namespace Characters.Projectiles.Operations.Customs
{
	// Token: 0x020007A5 RID: 1957
	public sealed class SummonClusterGrenades : Operation
	{
		// Token: 0x060027FC RID: 10236 RVA: 0x00078D88 File Offset: 0x00076F88
		public override void Run(IProjectile projectile)
		{
			for (int i = 0; i < this._forces.Length; i++)
			{
				Rigidbody2D component = this.Summon(projectile, this._speeds[i]).GetComponent<Rigidbody2D>();
				Vector2 vector = this._forces[i] * projectile.firedDirection;
				vector.y = math.abs(vector.y);
				component.AddForce(vector * 2f, ForceMode2D.Impulse);
			}
		}

		// Token: 0x060027FD RID: 10237 RVA: 0x00078DF8 File Offset: 0x00076FF8
		private OperationInfos Summon(IProjectile projectile, float speed)
		{
			Character owner = projectile.owner;
			Vector3 position = (this._spawnPosition == null) ? base.transform.position : (this._spawnPosition.position + this._noise.Evaluate());
			Vector3 vector = new Vector3(0f, 0f, this._angle.value);
			bool flag = this._flipXByLookingDirection && owner.lookingDirection == Character.LookingDirection.Left;
			if (flag)
			{
				vector.z = (180f - vector.z) % 360f;
			}
			OperationInfos operationInfos = this._operationRunner.Spawn().operationInfos;
			operationInfos.transform.SetPositionAndRotation(position, Quaternion.Euler(vector));
			if (flag)
			{
				operationInfos.transform.localScale = new Vector3(1f, -1f, 1f) * this._scale.value;
			}
			else
			{
				operationInfos.transform.localScale = new Vector3(1f, 1f, 1f) * this._scale.value;
			}
			operationInfos.Run(owner, speed);
			if (this._attachToOwner)
			{
				operationInfos.transform.parent = base.transform;
			}
			return operationInfos;
		}

		// Token: 0x0400222E RID: 8750
		[SerializeField]
		[Tooltip("오퍼레이션 프리팹")]
		private OperationRunner _operationRunner;

		// Token: 0x0400222F RID: 8751
		[SerializeField]
		private Transform _spawnPosition;

		// Token: 0x04002230 RID: 8752
		[SerializeField]
		private CustomFloat _scale = new CustomFloat(1f);

		// Token: 0x04002231 RID: 8753
		[SerializeField]
		private CustomAngle _angle;

		// Token: 0x04002232 RID: 8754
		[SerializeField]
		private PositionNoise _noise;

		// Token: 0x04002233 RID: 8755
		[SerializeField]
		[Tooltip("주인이 바라보고 있는 방향에 따라 X축으로 플립")]
		private bool _flipXByLookingDirection;

		// Token: 0x04002234 RID: 8756
		[SerializeField]
		[Tooltip("체크하면 주인에 부착되며, 같이 움직임")]
		private bool _attachToOwner;

		// Token: 0x04002235 RID: 8757
		[Space]
		[SerializeField]
		private Vector2[] _forces;

		// Token: 0x04002236 RID: 8758
		[SerializeField]
		private float[] _speeds;
	}
}
