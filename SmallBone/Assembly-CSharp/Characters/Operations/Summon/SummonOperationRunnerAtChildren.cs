using System;
using FX;
using UnityEngine;
using UnityEngine.Rendering;

namespace Characters.Operations.Summon
{
	// Token: 0x02000F45 RID: 3909
	public sealed class SummonOperationRunnerAtChildren : CharacterOperation
	{
		// Token: 0x06004C01 RID: 19457 RVA: 0x000DFFC1 File Offset: 0x000DE1C1
		protected override void OnDestroy()
		{
			base.OnDestroy();
			this._operationRunner = null;
		}

		// Token: 0x06004C02 RID: 19458 RVA: 0x000DFFD0 File Offset: 0x000DE1D0
		public override void Initialize()
		{
			this._attackDamage = base.GetComponentInParent<AttackDamage>();
		}

		// Token: 0x06004C03 RID: 19459 RVA: 0x000DFFE0 File Offset: 0x000DE1E0
		public override void Run(Character owner)
		{
			foreach (object obj in this._spawnPositionParent)
			{
				Vector3 vector = ((Transform)obj).position;
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
				if (this._flipX)
				{
					vector2.z = (180f - vector2.z) % 360f;
				}
				OperationRunner operationRunner = this._operationRunner.Spawn();
				OperationInfos operationInfos = operationRunner.operationInfos;
				operationInfos.transform.SetPositionAndRotation(vector, Quaternion.Euler(vector2));
				if (this._copyAttackDamage && this._attackDamage != null)
				{
					operationRunner.attackDamage.minAttackDamage = this._attackDamage.minAttackDamage;
					operationRunner.attackDamage.maxAttackDamage = this._attackDamage.maxAttackDamage;
				}
				SortingGroup component = operationRunner.GetComponent<SortingGroup>();
				if (component != null)
				{
					SortingGroup sortingGroup = component;
					short num = SummonOperationRunnerAtChildren.spriteLayer;
					SummonOperationRunnerAtChildren.spriteLayer = num + 1;
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
				if (this._flipX)
				{
					operationInfos.transform.localScale = new Vector3(1f, -1f, 1f) * this._scale.value;
				}
				operationInfos.Run(owner);
				if (this._attachToOwner)
				{
					operationInfos.transform.parent = base.transform;
				}
			}
		}

		// Token: 0x04003B4C RID: 15180
		private static short spriteLayer = short.MinValue;

		// Token: 0x04003B4D RID: 15181
		[SerializeField]
		[Tooltip("오퍼레이션 프리팹")]
		internal OperationRunner _operationRunner;

		// Token: 0x04003B4E RID: 15182
		[SerializeField]
		[Space]
		private Transform _spawnPositionParent;

		// Token: 0x04003B4F RID: 15183
		[SerializeField]
		private CustomFloat _scale = new CustomFloat(1f);

		// Token: 0x04003B50 RID: 15184
		[SerializeField]
		private CustomAngle _angle;

		// Token: 0x04003B51 RID: 15185
		[SerializeField]
		private PositionNoise _noise;

		// Token: 0x04003B52 RID: 15186
		[Space]
		[SerializeField]
		[Tooltip("주인이 바라보고 있는 방향에 따라 X축으로 플립")]
		private bool _flipXByLookingDirection;

		// Token: 0x04003B53 RID: 15187
		[Tooltip("X축 플립")]
		[SerializeField]
		private bool _flipX;

		// Token: 0x04003B54 RID: 15188
		[SerializeField]
		[Space]
		private bool _snapToGround;

		// Token: 0x04003B55 RID: 15189
		[Tooltip("땅을 찾기 위해 소환지점으로부터 아래로 탐색할 거리. 실패 시 그냥 소환 지점에 소환됨")]
		[SerializeField]
		private float _groundFindingDistance = 7f;

		// Token: 0x04003B56 RID: 15190
		[SerializeField]
		[Tooltip("체크하면 주인에 부착되며, 같이 움직임")]
		[Space]
		private bool _attachToOwner;

		// Token: 0x04003B57 RID: 15191
		[SerializeField]
		private bool _copyAttackDamage;

		// Token: 0x04003B58 RID: 15192
		private AttackDamage _attackDamage;
	}
}
