using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Characters.Operations;
using Characters.Operations.Attack;
using Characters.Utils;
using FX;
using PhysicsUtils;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Rendering;

namespace Characters.Projectiles.Operations
{
	// Token: 0x020007A2 RID: 1954
	public class SummonOperationRunnersOnGround : Operation
	{
		// Token: 0x060027EF RID: 10223 RVA: 0x0007880C File Offset: 0x00076A0C
		private void Awake()
		{
			this._overlapper = new NonAllocOverlapper(16);
			if (this._attackGroup)
			{
				this._hitHistoryManager = new HitHistoryManager(512);
			}
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

		// Token: 0x060027F0 RID: 10224 RVA: 0x00078890 File Offset: 0x00076A90
		private void OnDestroy()
		{
			this._operationRunner = null;
		}

		// Token: 0x060027F1 RID: 10225 RVA: 0x0007889C File Offset: 0x00076A9C
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
				Bounds bounds = this._overlapper.results[i].bounds;
				float2 @float = bounds.GetMostLeftTop();
				float2 float2 = bounds.GetMostRightTop();
				@float.x = Mathf.Max(@float.x, x);
				float2.x = Mathf.Min(float2.x, x2);
				this._surfaces.Add(new ValueTuple<float2, float2>(@float, float2));
			}
		}

		// Token: 0x060027F2 RID: 10226 RVA: 0x000789AC File Offset: 0x00076BAC
		public override void Run(IProjectile projectile)
		{
			this.FindSurfaces();
			if (this._surfaces.Count == 0)
			{
				return;
			}
			this.SpawnAtOnce(projectile.owner);
		}

		// Token: 0x060027F3 RID: 10227 RVA: 0x000789D0 File Offset: 0x00076BD0
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

		// Token: 0x060027F4 RID: 10228 RVA: 0x00078A9C File Offset: 0x00076C9C
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
			if (this._attackGroup)
			{
				this._hitHistoryManager.Clear();
				SweepAttack[] components = operationInfos.GetComponents<SweepAttack>();
				for (int i = 0; i < components.Length; i++)
				{
					components[i].collisionDetector.hits = this._hitHistoryManager;
				}
			}
			operationInfos.Run(owner);
		}

		// Token: 0x0400221A RID: 8730
		private const int _maxTerrainCount = 16;

		// Token: 0x0400221B RID: 8731
		private static short spriteLayer = short.MinValue;

		// Token: 0x0400221C RID: 8732
		[SerializeField]
		private BoxCollider2D _terrainFindingRange;

		// Token: 0x0400221D RID: 8733
		[Tooltip("플랫폼도 포함할 것인지")]
		[SerializeField]
		private bool _includePlatform = true;

		// Token: 0x0400221E RID: 8734
		[Tooltip("오퍼레이션 하나의 너비, 즉 스폰 간격")]
		[SerializeField]
		private float _width;

		// Token: 0x0400221F RID: 8735
		[SerializeField]
		private Transform _orderOrigin;

		// Token: 0x04002220 RID: 8736
		[SerializeField]
		[Tooltip("오퍼레이션 프리팹")]
		[Space]
		private OperationRunner _operationRunner;

		// Token: 0x04002221 RID: 8737
		[Space]
		[SerializeField]
		private CustomFloat _scale = new CustomFloat(1f);

		// Token: 0x04002222 RID: 8738
		[SerializeField]
		private CustomAngle _angle;

		// Token: 0x04002223 RID: 8739
		[SerializeField]
		private PositionNoise _noise;

		// Token: 0x04002224 RID: 8740
		[Tooltip("주인이 바라보고 있는 방향에 따라 X축으로 플립")]
		[Space]
		[SerializeField]
		private bool _flipXByLookingDirection;

		// Token: 0x04002225 RID: 8741
		[Tooltip("X축 플립")]
		[SerializeField]
		private bool _flipX;

		// Token: 0x04002226 RID: 8742
		[Header("Sweepattack만 가능")]
		[SerializeField]
		private bool _attackGroup;

		// Token: 0x04002227 RID: 8743
		private NonAllocOverlapper _overlapper;

		// Token: 0x04002228 RID: 8744
		[TupleElementNames(new string[]
		{
			"a",
			"b"
		})]
		private List<ValueTuple<float2, float2>> _surfaces = new List<ValueTuple<float2, float2>>(16);

		// Token: 0x04002229 RID: 8745
		private HitHistoryManager _hitHistoryManager;
	}
}
