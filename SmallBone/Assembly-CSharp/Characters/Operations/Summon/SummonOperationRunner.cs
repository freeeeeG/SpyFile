using System;
using FX;
using UnityEngine;
using UnityEngine.Rendering;

namespace Characters.Operations.Summon
{
	// Token: 0x02000F44 RID: 3908
	public class SummonOperationRunner : CharacterOperation
	{
		// Token: 0x06004BFC RID: 19452 RVA: 0x000DFD39 File Offset: 0x000DDF39
		protected override void OnDestroy()
		{
			base.OnDestroy();
			this._operationRunner = null;
		}

		// Token: 0x06004BFD RID: 19453 RVA: 0x000DFD48 File Offset: 0x000DDF48
		public override void Initialize()
		{
			this._attackDamage = base.GetComponentInParent<AttackDamage>();
		}

		// Token: 0x06004BFE RID: 19454 RVA: 0x000DFD58 File Offset: 0x000DDF58
		public override void Run(Character owner)
		{
			Vector3 vector = (this._spawnPosition == null) ? owner.transform.position : this._spawnPosition.position;
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
				short num = SummonOperationRunner.spriteLayer;
				SummonOperationRunner.spriteLayer = num + 1;
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

		// Token: 0x04003B3F RID: 15167
		private static short spriteLayer = short.MinValue;

		// Token: 0x04003B40 RID: 15168
		[Tooltip("오퍼레이션 프리팹")]
		[SerializeField]
		internal OperationRunner _operationRunner;

		// Token: 0x04003B41 RID: 15169
		[SerializeField]
		[Space]
		private Transform _spawnPosition;

		// Token: 0x04003B42 RID: 15170
		[SerializeField]
		private CustomFloat _scale = new CustomFloat(1f);

		// Token: 0x04003B43 RID: 15171
		[SerializeField]
		private CustomAngle _angle;

		// Token: 0x04003B44 RID: 15172
		[SerializeField]
		private PositionNoise _noise;

		// Token: 0x04003B45 RID: 15173
		[Space]
		[Tooltip("주인이 바라보고 있는 방향에 따라 X축으로 플립")]
		[SerializeField]
		private bool _flipXByLookingDirection;

		// Token: 0x04003B46 RID: 15174
		[Tooltip("X축 플립")]
		[SerializeField]
		private bool _flipX;

		// Token: 0x04003B47 RID: 15175
		[Space]
		[SerializeField]
		private bool _snapToGround;

		// Token: 0x04003B48 RID: 15176
		[Tooltip("땅을 찾기 위해 소환지점으로부터 아래로 탐색할 거리. 실패 시 그냥 소환 지점에 소환됨")]
		[SerializeField]
		private float _groundFindingDistance = 7f;

		// Token: 0x04003B49 RID: 15177
		[Space]
		[Tooltip("체크하면 주인에 부착되며, 같이 움직임")]
		[SerializeField]
		private bool _attachToOwner;

		// Token: 0x04003B4A RID: 15178
		[SerializeField]
		private bool _copyAttackDamage;

		// Token: 0x04003B4B RID: 15179
		private AttackDamage _attackDamage;
	}
}
