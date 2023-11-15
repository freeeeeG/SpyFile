using System;
using FX;
using UnityEngine;
using UnityEngine.Rendering;

namespace Characters.Operations.Summon
{
	// Token: 0x02000F46 RID: 3910
	public class SummonOperationRunnerAtTarget : TargetedCharacterOperation
	{
		// Token: 0x06004C06 RID: 19462 RVA: 0x000E028B File Offset: 0x000DE48B
		protected void OnDestroy()
		{
			this._operationRunner = null;
		}

		// Token: 0x06004C07 RID: 19463 RVA: 0x000E0294 File Offset: 0x000DE494
		public override void Initialize()
		{
			this._attackDamage = base.GetComponentInParent<AttackDamage>();
		}

		// Token: 0x06004C08 RID: 19464 RVA: 0x000E02A4 File Offset: 0x000DE4A4
		public override void Run(Character owner, Character target)
		{
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
			OperationInfos operationInfos = operationRunner.operationInfos;
			if (this._copyAttackDamage && this._attackDamage != null)
			{
				operationRunner.attackDamage.minAttackDamage = this._attackDamage.minAttackDamage;
				operationRunner.attackDamage.maxAttackDamage = this._attackDamage.maxAttackDamage;
			}
			Vector3 position = target.transform.position;
			position.x += target.collider.offset.x;
			position.y += target.collider.offset.y;
			Vector3 size = target.collider.bounds.size;
			size.x *= this._positionInfo.pivotValue.x;
			size.y *= this._positionInfo.pivotValue.y;
			operationInfos.transform.SetPositionAndRotation(position + size, Quaternion.Euler(vector));
			SortingGroup component = operationRunner.GetComponent<SortingGroup>();
			if (component != null)
			{
				SortingGroup sortingGroup = component;
				short num = SummonOperationRunnerAtTarget.spriteLayer;
				SummonOperationRunnerAtTarget.spriteLayer = num + 1;
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
			if (this._attachToTarget)
			{
				operationInfos.transform.parent = target.transform;
			}
			operationInfos.Run(owner);
		}

		// Token: 0x04003B59 RID: 15193
		private static short spriteLayer = short.MinValue;

		// Token: 0x04003B5A RID: 15194
		[SerializeField]
		[Tooltip("오퍼레이션 프리팹")]
		private OperationRunner _operationRunner;

		// Token: 0x04003B5B RID: 15195
		[SerializeField]
		private SummonOperationRunnerAtTarget.PositionInfo _positionInfo;

		// Token: 0x04003B5C RID: 15196
		[SerializeField]
		private bool _attachToTarget;

		// Token: 0x04003B5D RID: 15197
		[SerializeField]
		private CustomFloat _scale = new CustomFloat(1f);

		// Token: 0x04003B5E RID: 15198
		[SerializeField]
		private CustomAngle _angle;

		// Token: 0x04003B5F RID: 15199
		[SerializeField]
		private PositionNoise _noise;

		// Token: 0x04003B60 RID: 15200
		[SerializeField]
		[Tooltip("주인이 바라보고 있는 방향에 따라 X축으로 플립")]
		private bool _flipXByLookingDirection;

		// Token: 0x04003B61 RID: 15201
		[SerializeField]
		[Tooltip("X축 플립")]
		private bool _flipX;

		// Token: 0x04003B62 RID: 15202
		[SerializeField]
		private bool _copyAttackDamage;

		// Token: 0x04003B63 RID: 15203
		private AttackDamage _attackDamage;

		// Token: 0x02000F47 RID: 3911
		[Serializable]
		public class PositionInfo
		{
			// Token: 0x17000F32 RID: 3890
			// (get) Token: 0x06004C0B RID: 19467 RVA: 0x000E051E File Offset: 0x000DE71E
			public SummonOperationRunnerAtTarget.PositionInfo.Pivot pivot
			{
				get
				{
					return this._pivot;
				}
			}

			// Token: 0x17000F33 RID: 3891
			// (get) Token: 0x06004C0C RID: 19468 RVA: 0x000E0526 File Offset: 0x000DE726
			public Vector2 pivotValue
			{
				get
				{
					return this._pivotValue;
				}
			}

			// Token: 0x06004C0D RID: 19469 RVA: 0x000E052E File Offset: 0x000DE72E
			public PositionInfo()
			{
				this._pivot = SummonOperationRunnerAtTarget.PositionInfo.Pivot.Center;
				this._pivotValue = Vector2.zero;
			}

			// Token: 0x06004C0E RID: 19470 RVA: 0x000E0548 File Offset: 0x000DE748
			public PositionInfo(bool attach, bool layerOnly, int layerOrderOffset, SummonOperationRunnerAtTarget.PositionInfo.Pivot pivot)
			{
				this._pivot = pivot;
				this._pivotValue = SummonOperationRunnerAtTarget.PositionInfo._pivotValues[pivot];
			}

			// Token: 0x04003B64 RID: 15204
			private static readonly EnumArray<SummonOperationRunnerAtTarget.PositionInfo.Pivot, Vector2> _pivotValues = new EnumArray<SummonOperationRunnerAtTarget.PositionInfo.Pivot, Vector2>(new Vector2[]
			{
				new Vector2(0f, 0f),
				new Vector2(-0.5f, 0.5f),
				new Vector2(0f, 0.5f),
				new Vector2(0.5f, 0.5f),
				new Vector2(-0.5f, 0f),
				new Vector2(0f, 0.5f),
				new Vector2(-0.5f, -0.5f),
				new Vector2(0f, -0.5f),
				new Vector2(0.5f, -0.5f),
				new Vector2(0f, 0f)
			});

			// Token: 0x04003B65 RID: 15205
			[SerializeField]
			private SummonOperationRunnerAtTarget.PositionInfo.Pivot _pivot;

			// Token: 0x04003B66 RID: 15206
			[HideInInspector]
			[SerializeField]
			private Vector2 _pivotValue;

			// Token: 0x02000F48 RID: 3912
			public enum Pivot
			{
				// Token: 0x04003B68 RID: 15208
				Center,
				// Token: 0x04003B69 RID: 15209
				TopLeft,
				// Token: 0x04003B6A RID: 15210
				Top,
				// Token: 0x04003B6B RID: 15211
				TopRight,
				// Token: 0x04003B6C RID: 15212
				Left,
				// Token: 0x04003B6D RID: 15213
				Right,
				// Token: 0x04003B6E RID: 15214
				BottomLeft,
				// Token: 0x04003B6F RID: 15215
				Bottom,
				// Token: 0x04003B70 RID: 15216
				BottomRight,
				// Token: 0x04003B71 RID: 15217
				Custom
			}
		}
	}
}
