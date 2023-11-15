using System;
using System.Collections.Generic;
using FX;
using PhysicsUtils;
using UnityEngine;
using UnityEngine.Rendering;

namespace Characters.Operations.Summon
{
	// Token: 0x02000F49 RID: 3913
	public class SummonOperationRunnersAtTargetWithinRange : CharacterOperation
	{
		// Token: 0x06004C10 RID: 19472 RVA: 0x000E0667 File Offset: 0x000DE867
		protected override void OnDestroy()
		{
			base.OnDestroy();
			this._operationRunner = null;
		}

		// Token: 0x06004C11 RID: 19473 RVA: 0x000E0676 File Offset: 0x000DE876
		public override void Initialize()
		{
			this._attackDamage = base.GetComponentInParent<AttackDamage>();
		}

		// Token: 0x06004C12 RID: 19474 RVA: 0x000E0684 File Offset: 0x000DE884
		private void Awake()
		{
			this._overlapper = new NonAllocOverlapper(this._maxCount);
			if (this._optimizedCollider)
			{
				this._collider.enabled = false;
			}
		}

		// Token: 0x06004C13 RID: 19475 RVA: 0x000E06AC File Offset: 0x000DE8AC
		public override void Run(Character owner)
		{
			this._overlapper.contactFilter.SetLayerMask(this._layer.Evaluate(owner.gameObject));
			this._collider.enabled = true;
			this._overlapper.OverlapCollider(this._collider);
			Vector3 origin = (this._sortOrigin != null) ? this._sortOrigin.position : this._collider.bounds.center;
			if (this._optimizedCollider)
			{
				this._collider.enabled = false;
			}
			if (this._overlapper.results.Count == 0)
			{
				return;
			}
			List<Character> list = new List<Character>(this._overlapper.results.Count);
			for (int i = 0; i < this._overlapper.results.Count; i++)
			{
				Target component = this._overlapper.results[i].GetComponent<Target>();
				if (!(component == null) && !(component.character == null) && component.character.liveAndActive && !(component.character == owner))
				{
					list.Add(component.character);
				}
			}
			if (list.Count == 0)
			{
				return;
			}
			switch (this._method)
			{
			case SummonOperationRunnersAtTargetWithinRange.FindingMethod.Random:
				list.PseudoShuffle<Character>();
				break;
			case SummonOperationRunnersAtTargetWithinRange.FindingMethod.CloseToFar:
				list.Sort(delegate(Character x, Character y)
				{
					Vector3 vector = origin - x.collider.bounds.center;
					Vector3 vector2 = origin - y.collider.bounds.center;
					return vector.sqrMagnitude.CompareTo(vector2.sqrMagnitude);
				});
				break;
			case SummonOperationRunnersAtTargetWithinRange.FindingMethod.FarToClose:
				list.Sort(delegate(Character x, Character y)
				{
					Vector3 vector = origin - x.collider.bounds.center;
					return (origin - y.collider.bounds.center).sqrMagnitude.CompareTo(vector.sqrMagnitude);
				});
				break;
			}
			int num = this._totalOperationCount;
			for (int j = 0; j < this._maxCountPerUnit; j++)
			{
				for (int k = 0; k < list.Count; k++)
				{
					this.SpawnTo(owner, list[k]);
					num--;
					if (num == 0)
					{
						return;
					}
				}
			}
		}

		// Token: 0x06004C14 RID: 19476 RVA: 0x000E0884 File Offset: 0x000DEA84
		public void SpawnTo(Character owner, Character target)
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
			Vector3 vector2 = position + size;
			if (this._snapToGround)
			{
				RaycastHit2D hit = Physics2D.Raycast(vector2, Vector2.down, this._groundFindingDistance, Layers.groundMask);
				if (hit)
				{
					vector2 = hit.point;
				}
			}
			vector2 += this._noise.Evaluate();
			operationInfos.transform.SetPositionAndRotation(vector2, Quaternion.Euler(vector));
			SortingGroup component = operationRunner.GetComponent<SortingGroup>();
			if (component != null)
			{
				SortingGroup sortingGroup = component;
				short num = SummonOperationRunnersAtTargetWithinRange.spriteLayer;
				SummonOperationRunnersAtTargetWithinRange.spriteLayer = num + 1;
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

		// Token: 0x04003B72 RID: 15218
		private static short spriteLayer = short.MinValue;

		// Token: 0x04003B73 RID: 15219
		private NonAllocOverlapper _overlapper;

		// Token: 0x04003B74 RID: 15220
		[SerializeField]
		[Tooltip("오퍼레이션 프리팹")]
		private OperationRunner _operationRunner;

		// Token: 0x04003B75 RID: 15221
		[SerializeField]
		private SummonOperationRunnersAtTargetWithinRange.PositionInfo _positionInfo;

		// Token: 0x04003B76 RID: 15222
		[SerializeField]
		private bool _attachToTarget;

		// Token: 0x04003B77 RID: 15223
		[SerializeField]
		private CustomFloat _scale = new CustomFloat(1f);

		// Token: 0x04003B78 RID: 15224
		[SerializeField]
		private CustomAngle _angle;

		// Token: 0x04003B79 RID: 15225
		[SerializeField]
		private PositionNoise _noise;

		// Token: 0x04003B7A RID: 15226
		[SerializeField]
		private bool _snapToGround;

		// Token: 0x04003B7B RID: 15227
		[SerializeField]
		[Tooltip("땅을 찾기 위해 소환지점으로부터 아래로 탐색할 거리. 실패 시 그냥 소환 지점에 소환됨")]
		private float _groundFindingDistance = 7f;

		// Token: 0x04003B7C RID: 15228
		[SerializeField]
		[Tooltip("주인이 바라보고 있는 방향에 따라 X축으로 플립")]
		private bool _flipXByLookingDirection;

		// Token: 0x04003B7D RID: 15229
		[Tooltip("X축 플립")]
		[SerializeField]
		private bool _flipX;

		// Token: 0x04003B7E RID: 15230
		[Header("Special Settings")]
		[SerializeField]
		private Collider2D _collider;

		// Token: 0x04003B7F RID: 15231
		[Tooltip("콜라이더 최적화 여부, Composite Collider등 특별한 경우가 아니면 true로 유지")]
		[SerializeField]
		private bool _optimizedCollider = true;

		// Token: 0x04003B80 RID: 15232
		[SerializeField]
		private TargetLayer _layer = new TargetLayer(0, false, true, false, false);

		// Token: 0x04003B81 RID: 15233
		[SerializeField]
		[Tooltip("범위 내 감지가능한 최대 적의 수, 프롭을 포함하지 않으므로 128로 충분")]
		private int _maxCount = 128;

		// Token: 0x04003B82 RID: 15234
		[Tooltip("스폰될 오퍼레이션러너의 최대 개수")]
		[SerializeField]
		private int _totalOperationCount;

		// Token: 0x04003B83 RID: 15235
		[Tooltip("하나의 적에게 중첩되어 스폰될 수 있는 최대 개수")]
		[SerializeField]
		private int _maxCountPerUnit = 1;

		// Token: 0x04003B84 RID: 15236
		[SerializeField]
		private SummonOperationRunnersAtTargetWithinRange.FindingMethod _method;

		// Token: 0x04003B85 RID: 15237
		[SerializeField]
		[Tooltip("Close To Far, Far To Close 계산 시 기준점이 될 위치, 비워둘 경우 콜라이더의 중심점을 기준으로 함")]
		private Transform _sortOrigin;

		// Token: 0x04003B86 RID: 15238
		[SerializeField]
		[Space]
		private bool _copyAttackDamage;

		// Token: 0x04003B87 RID: 15239
		private AttackDamage _attackDamage;

		// Token: 0x02000F4A RID: 3914
		[Serializable]
		public class PositionInfo
		{
			// Token: 0x17000F34 RID: 3892
			// (get) Token: 0x06004C17 RID: 19479 RVA: 0x000E0B9C File Offset: 0x000DED9C
			public SummonOperationRunnersAtTargetWithinRange.PositionInfo.Pivot pivot
			{
				get
				{
					return this._pivot;
				}
			}

			// Token: 0x17000F35 RID: 3893
			// (get) Token: 0x06004C18 RID: 19480 RVA: 0x000E0BA4 File Offset: 0x000DEDA4
			public Vector2 pivotValue
			{
				get
				{
					return this._pivotValue;
				}
			}

			// Token: 0x06004C19 RID: 19481 RVA: 0x000E0BAC File Offset: 0x000DEDAC
			public PositionInfo()
			{
				this._pivot = SummonOperationRunnersAtTargetWithinRange.PositionInfo.Pivot.Center;
				this._pivotValue = Vector2.zero;
			}

			// Token: 0x06004C1A RID: 19482 RVA: 0x000E0BC6 File Offset: 0x000DEDC6
			public PositionInfo(bool attach, bool layerOnly, int layerOrderOffset, SummonOperationRunnersAtTargetWithinRange.PositionInfo.Pivot pivot)
			{
				this._pivot = pivot;
				this._pivotValue = SummonOperationRunnersAtTargetWithinRange.PositionInfo._pivotValues[pivot];
			}

			// Token: 0x04003B88 RID: 15240
			private static readonly EnumArray<SummonOperationRunnersAtTargetWithinRange.PositionInfo.Pivot, Vector2> _pivotValues = new EnumArray<SummonOperationRunnersAtTargetWithinRange.PositionInfo.Pivot, Vector2>(new Vector2[]
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

			// Token: 0x04003B89 RID: 15241
			[SerializeField]
			private SummonOperationRunnersAtTargetWithinRange.PositionInfo.Pivot _pivot;

			// Token: 0x04003B8A RID: 15242
			[SerializeField]
			[HideInInspector]
			private Vector2 _pivotValue;

			// Token: 0x02000F4B RID: 3915
			public enum Pivot
			{
				// Token: 0x04003B8C RID: 15244
				Center,
				// Token: 0x04003B8D RID: 15245
				TopLeft,
				// Token: 0x04003B8E RID: 15246
				Top,
				// Token: 0x04003B8F RID: 15247
				TopRight,
				// Token: 0x04003B90 RID: 15248
				Left,
				// Token: 0x04003B91 RID: 15249
				Right,
				// Token: 0x04003B92 RID: 15250
				BottomLeft,
				// Token: 0x04003B93 RID: 15251
				Bottom,
				// Token: 0x04003B94 RID: 15252
				BottomRight,
				// Token: 0x04003B95 RID: 15253
				Custom
			}
		}

		// Token: 0x02000F4C RID: 3916
		public enum FindingMethod
		{
			// Token: 0x04003B97 RID: 15255
			Random,
			// Token: 0x04003B98 RID: 15256
			CloseToFar,
			// Token: 0x04003B99 RID: 15257
			FarToClose
		}
	}
}
