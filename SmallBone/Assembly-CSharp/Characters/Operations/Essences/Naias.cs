using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using PhysicsUtils;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Rendering;

namespace Characters.Operations.Essences
{
	// Token: 0x02000EB1 RID: 3761
	public sealed class Naias : CharacterOperation
	{
		// Token: 0x060049FF RID: 18943 RVA: 0x000D7F90 File Offset: 0x000D6190
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

		// Token: 0x06004A00 RID: 18944 RVA: 0x000D7FFC File Offset: 0x000D61FC
		public override void Run(Character owner)
		{
			this.FindSurfaces();
			if (this._surfaces.Count == 0)
			{
				return;
			}
			this.SpawnByWorldOrder(owner);
		}

		// Token: 0x06004A01 RID: 18945 RVA: 0x000D801C File Offset: 0x000D621C
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
					float2 position = item;
					position.x += this._width * (float)num3;
					this.Spawn(position, num3);
					num3++;
				}
			}
		}

		// Token: 0x06004A02 RID: 18946 RVA: 0x000D80D4 File Offset: 0x000D62D4
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
					float2 @float = item;
					@float.x += this._width * (float)num3;
					list.Add(new ValueTuple<float2, float>(@float, math.distance(x, @float)));
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
			for (int j = 0; j < list.Count; j++)
			{
				this.Spawn(list[j].Item1, j);
			}
		}

		// Token: 0x06004A03 RID: 18947 RVA: 0x000D8224 File Offset: 0x000D6424
		private void Spawn(float2 position, int index)
		{
			if (index >= this._naiasProps.Length)
			{
				return;
			}
			SortingGroup component = this._naiasProps[index].GetComponent<SortingGroup>();
			if (component != null)
			{
				SortingGroup sortingGroup = component;
				short num = Naias.spriteLayer;
				Naias.spriteLayer = num + 1;
				sortingGroup.sortingOrder = (int)num;
			}
			this._naiasProps[index].transform.position = new Vector3(position.x, position.y);
		}

		// Token: 0x06004A04 RID: 18948 RVA: 0x000D828C File Offset: 0x000D648C
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

		// Token: 0x04003933 RID: 14643
		private const int _maxTerrainCount = 16;

		// Token: 0x04003934 RID: 14644
		private static short spriteLayer = short.MinValue;

		// Token: 0x04003935 RID: 14645
		[SerializeField]
		private BoxCollider2D _terrainFindingRange;

		// Token: 0x04003936 RID: 14646
		[Tooltip("플랫폼도 포함할 것인지")]
		[SerializeField]
		private bool _includePlatform = true;

		// Token: 0x04003937 RID: 14647
		[Tooltip("오퍼레이션 하나의 너비, 즉 스폰 간격")]
		[SerializeField]
		private float _width;

		// Token: 0x04003938 RID: 14648
		[SerializeField]
		private Transform _orderOrigin;

		// Token: 0x04003939 RID: 14649
		[Tooltip("Order에 따른 각 요소별 스폰 딜레이")]
		[SerializeField]
		private float _delay;

		// Token: 0x0400393A RID: 14650
		[SerializeField]
		private Transform[] _naiasProps;

		// Token: 0x0400393B RID: 14651
		private NonAllocOverlapper _overlapper;

		// Token: 0x0400393C RID: 14652
		[TupleElementNames(new string[]
		{
			"a",
			"b"
		})]
		private List<ValueTuple<float2, float2>> _surfaces = new List<ValueTuple<float2, float2>>(16);
	}
}
