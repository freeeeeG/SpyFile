using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using FX;
using PhysicsUtils;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Rendering;

namespace Characters.Operations.Summon
{
	// Token: 0x02000F51 RID: 3921
	public class SummonOperationRunnersOnGround : CharacterOperation
	{
		// Token: 0x06004C32 RID: 19506 RVA: 0x000E14A8 File Offset: 0x000DF6A8
		private void Awake()
		{
			this._overlapper = new NonAllocOverlapper(16);
			int num = 262144;
			if (this._includePlatform)
			{
				num |= 131072;
			}
			this._overlapper.contactFilter.SetLayerMask(num);
			this._terrainFindingRange.enabled = false;
			if (this._orderOrigin == null)
			{
				this._orderOrigin = base.transform;
			}
		}

		// Token: 0x06004C33 RID: 19507 RVA: 0x000E1514 File Offset: 0x000DF714
		protected override void OnDestroy()
		{
			base.OnDestroy();
			this._operationRunner = null;
		}

		// Token: 0x06004C34 RID: 19508 RVA: 0x000E1523 File Offset: 0x000DF723
		public override void Initialize()
		{
			this._attackDamage = base.GetComponentInParent<AttackDamage>();
		}

		// Token: 0x06004C35 RID: 19509 RVA: 0x000E1534 File Offset: 0x000DF734
		private void FindSurfaces()
		{
			this._terrainFindingRange.enabled = true;
			this._overlapper.OverlapCollider(this._terrainFindingRange);
			float x = this._terrainFindingRange.bounds.min.x;
			float x2 = this._terrainFindingRange.bounds.max.x;
			this._terrainFindingRange.enabled = false;
			this._surfaces.Clear();
			if (this._overlapper.results.Count == 0)
			{
				return;
			}
			for (int i = 0; i < this._overlapper.results.Count; i++)
			{
				Collider2D collider2D = this._overlapper.results[i];
				if (!this._onlySpawnToLastStandingCollider || !(collider2D != this._owner.movement.controller.collisionState.lastStandingCollider))
				{
					Bounds bounds = collider2D.bounds;
					float2 @float = bounds.GetMostLeftTop();
					float2 float2 = bounds.GetMostRightTop();
					@float.x = Mathf.Max(@float.x, x);
					float2.x = Mathf.Min(float2.x, x2);
					this._surfaces.Add(new ValueTuple<float2, float2>(@float, float2));
				}
			}
		}

		// Token: 0x06004C36 RID: 19510 RVA: 0x000E1678 File Offset: 0x000DF878
		public override void Run(Character owner)
		{
			this._owner = owner;
			this.FindSurfaces();
			if (this._surfaces.Count == 0)
			{
				return;
			}
			if (this._order == SummonOperationRunnersOnGround.Order.AtOnce)
			{
				this.SpawnAtOnce(owner);
				return;
			}
			if (this._order == SummonOperationRunnersOnGround.Order.InsideToOutside || this._order == SummonOperationRunnersOnGround.Order.OutsideToInside)
			{
				this.SpawnByWorldOrder(owner);
			}
		}

		// Token: 0x06004C37 RID: 19511 RVA: 0x000E16CC File Offset: 0x000DF8CC
		private void SpawnAtOnce(Character owner)
		{
			for (int i = 0; i < this._surfaces.Count; i++)
			{
				ValueTuple<float2, float2> valueTuple = this._surfaces[i];
				float num = (valueTuple.Item2.x - valueTuple.Item1.x) / this._width;
				float num2 = num - (float)((int)num);
				float2 item = valueTuple.Item1;
				item.x = valueTuple.Item1.x + num2 * this._width / 2f;
				int num3 = 0;
				while ((float)num3 < num)
				{
					float2 position = item + this._noise.EvaluateAsVector2();
					position.x += this._width * (float)num3;
					this.Spawn(owner, position);
					num3++;
				}
			}
		}

		// Token: 0x06004C38 RID: 19512 RVA: 0x000E1798 File Offset: 0x000DF998
		private void SpawnByWorldOrder(Character owner)
		{
			List<ValueTuple<float2, float>> list = new List<ValueTuple<float2, float>>();
			float2 x = new float2(this._orderOrigin.transform.position.x, this._orderOrigin.transform.position.y);
			for (int i = 0; i < this._surfaces.Count; i++)
			{
				ValueTuple<float2, float2> valueTuple = this._surfaces[i];
				float num = (valueTuple.Item2.x - valueTuple.Item1.x) / this._width;
				float num2 = num - (float)((int)num);
				float2 item = valueTuple.Item1;
				item.x = valueTuple.Item1.x + num2 * this._width / 2f;
				int num3 = 0;
				while ((float)num3 < num)
				{
					float2 @float = item + this._noise.EvaluateAsVector2();
					@float.x += this._width * (float)num3;
					list.Add(new ValueTuple<float2, float>(@float, math.distance(x, @float)));
					num3++;
				}
			}
			if (this._order == SummonOperationRunnersOnGround.Order.InsideToOutside)
			{
				list.Sort(([TupleElementNames(new string[]
				{
					"position",
					"distance"
				})] ValueTuple<float2, float> a, [TupleElementNames(new string[]
				{
					"position",
					"distance"
				})] ValueTuple<float2, float> b) => a.Item2.CompareTo(b.Item2));
			}
			else if (this._order == SummonOperationRunnersOnGround.Order.OutsideToInside)
			{
				list.Sort(([TupleElementNames(new string[]
				{
					"position",
					"distance"
				})] ValueTuple<float2, float> a, [TupleElementNames(new string[]
				{
					"position",
					"distance"
				})] ValueTuple<float2, float> b) => b.Item2.CompareTo(a.Item2));
			}
			base.StartCoroutine(this.CSpawnByDelay(owner, list));
		}

		// Token: 0x06004C39 RID: 19513 RVA: 0x000E191A File Offset: 0x000DFB1A
		private IEnumerator CSpawnByDelay(Character owner, [TupleElementNames(new string[]
		{
			"position",
			"distance"
		})] List<ValueTuple<float2, float>> spawnPositions)
		{
			float item = spawnPositions[0].Item2;
			foreach (ValueTuple<float2, float> spawnPosition in spawnPositions)
			{
				this.Spawn(owner, spawnPosition.Item1);
				yield return Chronometer.global.WaitForSeconds(math.distance(spawnPosition.Item2, item) / this._width * this._delay);
				item = spawnPosition.Item2;
				spawnPosition = default(ValueTuple<float2, float>);
			}
			List<ValueTuple<float2, float>>.Enumerator enumerator = default(List<ValueTuple<float2, float>>.Enumerator);
			yield break;
			yield break;
		}

		// Token: 0x06004C3A RID: 19514 RVA: 0x000E1938 File Offset: 0x000DFB38
		private void Spawn(Character owner, float2 position)
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
			operationInfos.transform.SetPositionAndRotation(new Vector3(position.x, position.y), Quaternion.Euler(vector));
			if (this._copyAttackDamage && this._attackDamage != null)
			{
				operationRunner.attackDamage.minAttackDamage = this._attackDamage.minAttackDamage;
				operationRunner.attackDamage.maxAttackDamage = this._attackDamage.maxAttackDamage;
			}
			SortingGroup component = operationRunner.GetComponent<SortingGroup>();
			if (component != null)
			{
				SortingGroup sortingGroup = component;
				short num = SummonOperationRunnersOnGround.spriteLayer;
				SummonOperationRunnersOnGround.spriteLayer = num + 1;
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
		}

		// Token: 0x06004C3B RID: 19515 RVA: 0x00048973 File Offset: 0x00046B73
		public override void Stop()
		{
			base.StopAllCoroutines();
		}

		// Token: 0x04003BB6 RID: 15286
		private const int _maxTerrainCount = 16;

		// Token: 0x04003BB7 RID: 15287
		private static short spriteLayer = short.MinValue;

		// Token: 0x04003BB8 RID: 15288
		[SerializeField]
		private BoxCollider2D _terrainFindingRange;

		// Token: 0x04003BB9 RID: 15289
		[SerializeField]
		[Tooltip("플랫폼도 포함할 것인지")]
		private bool _includePlatform = true;

		// Token: 0x04003BBA RID: 15290
		[Tooltip("오퍼레이션 하나의 너비, 즉 스폰 간격")]
		[SerializeField]
		private float _width;

		// Token: 0x04003BBB RID: 15291
		[SerializeField]
		private SummonOperationRunnersOnGround.Order _order;

		// Token: 0x04003BBC RID: 15292
		[SerializeField]
		private Transform _orderOrigin;

		// Token: 0x04003BBD RID: 15293
		[Tooltip("Order에 따른 각 요소별 스폰 딜레이")]
		[SerializeField]
		private float _delay;

		// Token: 0x04003BBE RID: 15294
		[Tooltip("오퍼레이션 프리팹")]
		[SerializeField]
		[Space]
		private OperationRunner _operationRunner;

		// Token: 0x04003BBF RID: 15295
		[Space]
		[SerializeField]
		private CustomFloat _scale = new CustomFloat(1f);

		// Token: 0x04003BC0 RID: 15296
		[SerializeField]
		private CustomAngle _angle;

		// Token: 0x04003BC1 RID: 15297
		[SerializeField]
		private PositionNoise _noise;

		// Token: 0x04003BC2 RID: 15298
		[Space]
		[Tooltip("주인이 바라보고 있는 방향에 따라 X축으로 플립")]
		[SerializeField]
		private bool _flipXByLookingDirection;

		// Token: 0x04003BC3 RID: 15299
		[Tooltip("X축 플립")]
		[SerializeField]
		private bool _flipX;

		// Token: 0x04003BC4 RID: 15300
		[SerializeField]
		private bool _copyAttackDamage;

		// Token: 0x04003BC5 RID: 15301
		[SerializeField]
		private bool _onlySpawnToLastStandingCollider;

		// Token: 0x04003BC6 RID: 15302
		private AttackDamage _attackDamage;

		// Token: 0x04003BC7 RID: 15303
		private NonAllocOverlapper _overlapper;

		// Token: 0x04003BC8 RID: 15304
		[TupleElementNames(new string[]
		{
			"a",
			"b"
		})]
		private List<ValueTuple<float2, float2>> _surfaces = new List<ValueTuple<float2, float2>>(16);

		// Token: 0x04003BC9 RID: 15305
		private Character _owner;

		// Token: 0x02000F52 RID: 3922
		public enum Order
		{
			// Token: 0x04003BCB RID: 15307
			AtOnce,
			// Token: 0x04003BCC RID: 15308
			InsideToOutside,
			// Token: 0x04003BCD RID: 15309
			OutsideToInside
		}
	}
}
