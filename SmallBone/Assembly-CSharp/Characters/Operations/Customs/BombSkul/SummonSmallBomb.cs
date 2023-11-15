using System;
using Characters.Abilities.Customs;
using FX;
using UnityEngine;
using UnityEngine.Rendering;

namespace Characters.Operations.Customs.BombSkul
{
	// Token: 0x02001025 RID: 4133
	public class SummonSmallBomb : CharacterOperation
	{
		// Token: 0x06004FAB RID: 20395 RVA: 0x000F014D File Offset: 0x000EE34D
		public override void Initialize()
		{
			this._attackDamage = base.GetComponentInParent<AttackDamage>();
		}

		// Token: 0x06004FAC RID: 20396 RVA: 0x000F015C File Offset: 0x000EE35C
		public override void Run(Character owner)
		{
			Vector3 position = (this._spawnPosition == null) ? base.transform.position : (this._spawnPosition.position + this._noise.Evaluate());
			Vector3 vector = new Vector3(0f, 0f, this._angle.value);
			bool flag = this._flipXByLookingDirection && owner.lookingDirection == Character.LookingDirection.Left;
			if (flag)
			{
				vector.z = (180f - vector.z) % 360f;
			}
			if (this._flipX)
			{
				vector.z = (180f - vector.z) % 360f;
			}
			OperationRunner operationRunner = this._operationRunner.Spawn();
			this._passvieComponent.RegisterSmallBomb(operationRunner);
			OperationInfos operationInfos = operationRunner.operationInfos;
			operationInfos.transform.SetPositionAndRotation(position, Quaternion.Euler(vector));
			if (this._copyAttackDamage && this._attackDamage != null)
			{
				operationRunner.attackDamage.minAttackDamage = this._attackDamage.minAttackDamage;
				operationRunner.attackDamage.maxAttackDamage = this._attackDamage.maxAttackDamage;
			}
			SortingGroup component = operationRunner.GetComponent<SortingGroup>();
			if (component != null)
			{
				SortingGroup sortingGroup = component;
				short num = SummonSmallBomb.spriteLayer;
				SummonSmallBomb.spriteLayer = num + 1;
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
			operationRunner.GetComponent<Rigidbody2D>().velocity = MMMaths.RandomVector2(this._minVelocity, this._maxVelocity);
		}

		// Token: 0x04003FFA RID: 16378
		private static short spriteLayer = short.MinValue;

		// Token: 0x04003FFB RID: 16379
		[SerializeField]
		private BombSkulPassiveComponent _passvieComponent;

		// Token: 0x04003FFC RID: 16380
		[SerializeField]
		[Tooltip("오퍼레이션 프리팹")]
		private OperationRunner _operationRunner;

		// Token: 0x04003FFD RID: 16381
		[SerializeField]
		private Transform _spawnPosition;

		// Token: 0x04003FFE RID: 16382
		[SerializeField]
		private CustomFloat _scale = new CustomFloat(1f);

		// Token: 0x04003FFF RID: 16383
		[SerializeField]
		private CustomAngle _angle;

		// Token: 0x04004000 RID: 16384
		[SerializeField]
		private PositionNoise _noise;

		// Token: 0x04004001 RID: 16385
		[Tooltip("주인이 바라보고 있는 방향에 따라 X축으로 플립")]
		[SerializeField]
		private bool _flipXByLookingDirection;

		// Token: 0x04004002 RID: 16386
		[SerializeField]
		[Tooltip("X축 플립")]
		private bool _flipX;

		// Token: 0x04004003 RID: 16387
		[SerializeField]
		private bool _copyAttackDamage;

		// Token: 0x04004004 RID: 16388
		[SerializeField]
		private Vector2 _minVelocity;

		// Token: 0x04004005 RID: 16389
		[SerializeField]
		private Vector2 _maxVelocity;

		// Token: 0x04004006 RID: 16390
		private AttackDamage _attackDamage;
	}
}
