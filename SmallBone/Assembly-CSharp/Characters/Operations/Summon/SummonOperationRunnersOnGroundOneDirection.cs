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
	// Token: 0x02000F55 RID: 3925
	public class SummonOperationRunnersOnGroundOneDirection : CharacterOperation
	{
		// Token: 0x06004C49 RID: 19529 RVA: 0x000E1CD4 File Offset: 0x000DFED4
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
			if (this._summonOrigin == null)
			{
				this._summonOrigin = base.transform;
			}
		}

		// Token: 0x06004C4A RID: 19530 RVA: 0x000E1D40 File Offset: 0x000DFF40
		protected override void OnDestroy()
		{
			base.OnDestroy();
			this._operationRunner = null;
		}

		// Token: 0x06004C4B RID: 19531 RVA: 0x000E1D4F File Offset: 0x000DFF4F
		public override void Initialize()
		{
			this._attackDamage = base.GetComponentInParent<AttackDamage>();
		}

		// Token: 0x06004C4C RID: 19532 RVA: 0x000E1D60 File Offset: 0x000DFF60
		private void FindSurfaces(Character owner)
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
				Bounds bounds = this._overlapper.results[i].bounds;
				if (!this._onlyOwnersTerrain || !(this._overlapper.results[i] != owner.movement.controller.collisionState.lastStandingCollider))
				{
					float2 @float = bounds.GetMostLeftTop();
					float2 float2 = bounds.GetMostRightTop();
					@float.x = Mathf.Max(@float.x, x);
					float2.x = Mathf.Min(float2.x, x2);
					this._surfaces.Add(new ValueTuple<float2, float2>(@float, float2));
				}
			}
		}

		// Token: 0x06004C4D RID: 19533 RVA: 0x000E1EAD File Offset: 0x000E00AD
		public override void Run(Character owner)
		{
			this.FindSurfaces(owner);
			if (this._surfaces.Count == 0)
			{
				return;
			}
			this.SpawnByOrder(owner);
		}

		// Token: 0x06004C4E RID: 19534 RVA: 0x000E1ECC File Offset: 0x000E00CC
		private void SpawnByOrder(Character owner)
		{
			List<ValueTuple<float2, float>> list = new List<ValueTuple<float2, float>>();
			float2 @float = new float2(this._summonOrigin.transform.position.x, this._summonOrigin.transform.position.y);
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
					float2 float2 = item + this._noise.EvaluateAsVector2();
					float2.x += this._width * (float)num3;
					if ((owner.lookingDirection != Character.LookingDirection.Right || @float.x <= float2.x) && (owner.lookingDirection != Character.LookingDirection.Left || @float.x >= float2.x))
					{
						list.Add(new ValueTuple<float2, float>(float2, math.distance(@float, float2)));
					}
					num3++;
				}
			}
			list.Sort(([TupleElementNames(new string[]
			{
				"position",
				"distance"
			})] ValueTuple<float2, float> a, [TupleElementNames(new string[]
			{
				"position",
				"distance"
			})] ValueTuple<float2, float> b) => a.Item2.CompareTo(b.Item2));
			if (list.Count != 0)
			{
				base.StartCoroutine(this.CSpawnByDelay(owner, list));
			}
		}

		// Token: 0x06004C4F RID: 19535 RVA: 0x000E204C File Offset: 0x000E024C
		private IEnumerator CSpawnByDelay(Character owner, [TupleElementNames(new string[]
		{
			"position",
			"distance"
		})] List<ValueTuple<float2, float>> spawnPositions)
		{
			float y = spawnPositions[0].Item2 - this._width;
			foreach (ValueTuple<float2, float> spawnPosition in spawnPositions)
			{
				this.Spawn(owner, spawnPosition.Item1);
				yield return Chronometer.global.WaitForSeconds(math.distance(spawnPosition.Item2, y) / this._width * this._delay);
				y = spawnPosition.Item2;
				spawnPosition = default(ValueTuple<float2, float>);
			}
			List<ValueTuple<float2, float>>.Enumerator enumerator = default(List<ValueTuple<float2, float>>.Enumerator);
			yield break;
			yield break;
		}

		// Token: 0x06004C50 RID: 19536 RVA: 0x000E206C File Offset: 0x000E026C
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
				short num = SummonOperationRunnersOnGroundOneDirection.spriteLayer;
				SummonOperationRunnersOnGroundOneDirection.spriteLayer = num + 1;
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

		// Token: 0x06004C51 RID: 19537 RVA: 0x00048973 File Offset: 0x00046B73
		public override void Stop()
		{
			base.StopAllCoroutines();
		}

		// Token: 0x04003BD8 RID: 15320
		private const int _maxTerrainCount = 16;

		// Token: 0x04003BD9 RID: 15321
		private static short spriteLayer = short.MinValue;

		// Token: 0x04003BDA RID: 15322
		[SerializeField]
		private BoxCollider2D _terrainFindingRange;

		// Token: 0x04003BDB RID: 15323
		[Tooltip("플랫폼도 포함할 것인지")]
		[SerializeField]
		private bool _includePlatform = true;

		// Token: 0x04003BDC RID: 15324
		[SerializeField]
		[Tooltip("오퍼레이션 하나의 너비, 즉 스폰 간격")]
		private float _width;

		// Token: 0x04003BDD RID: 15325
		[SerializeField]
		private Transform _summonOrigin;

		// Token: 0x04003BDE RID: 15326
		[SerializeField]
		private bool _onlyOwnersTerrain;

		// Token: 0x04003BDF RID: 15327
		[SerializeField]
		[Tooltip("Order에 따른 각 요소별 스폰 딜레이")]
		private float _delay;

		// Token: 0x04003BE0 RID: 15328
		[SerializeField]
		[Tooltip("오퍼레이션 프리팹")]
		[Space]
		private OperationRunner _operationRunner;

		// Token: 0x04003BE1 RID: 15329
		[Space]
		[SerializeField]
		private CustomFloat _scale = new CustomFloat(1f);

		// Token: 0x04003BE2 RID: 15330
		[SerializeField]
		private CustomAngle _angle;

		// Token: 0x04003BE3 RID: 15331
		[SerializeField]
		private PositionNoise _noise;

		// Token: 0x04003BE4 RID: 15332
		[SerializeField]
		[Tooltip("주인이 바라보고 있는 방향에 따라 X축으로 플립")]
		[Space]
		private bool _flipXByLookingDirection;

		// Token: 0x04003BE5 RID: 15333
		[SerializeField]
		[Tooltip("X축 플립")]
		private bool _flipX;

		// Token: 0x04003BE6 RID: 15334
		[SerializeField]
		private bool _copyAttackDamage;

		// Token: 0x04003BE7 RID: 15335
		private AttackDamage _attackDamage;

		// Token: 0x04003BE8 RID: 15336
		private NonAllocOverlapper _overlapper;

		// Token: 0x04003BE9 RID: 15337
		[TupleElementNames(new string[]
		{
			"a",
			"b"
		})]
		private List<ValueTuple<float2, float2>> _surfaces = new List<ValueTuple<float2, float2>>(16);
	}
}
