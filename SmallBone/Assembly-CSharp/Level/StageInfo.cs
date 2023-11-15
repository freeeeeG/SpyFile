using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Data;
using GameResources;
using Hardmode;
using Level.Npc;
using Singletons;
using UnityEngine;

namespace Level
{
	// Token: 0x02000529 RID: 1321
	[CreateAssetMenu]
	public class StageInfo : IStageInfo
	{
		// Token: 0x17000539 RID: 1337
		// (get) Token: 0x060019F4 RID: 6644 RVA: 0x000513B8 File Offset: 0x0004F5B8
		public SerializablePathNode entry
		{
			get
			{
				return this._entry;
			}
		}

		// Token: 0x1700053A RID: 1338
		// (get) Token: 0x060019F5 RID: 6645 RVA: 0x000513C0 File Offset: 0x0004F5C0
		public SerializablePathNode terminal
		{
			get
			{
				return this._terminal;
			}
		}

		// Token: 0x060019F6 RID: 6646 RVA: 0x000513C8 File Offset: 0x0004F5C8
		public override void Initialize()
		{
			this._maps = this.maps.ToLookup((MapReference m) => m.type);
		}

		// Token: 0x1700053B RID: 1339
		// (get) Token: 0x060019F7 RID: 6647 RVA: 0x000513FA File Offset: 0x0004F5FA
		[TupleElementNames(new string[]
		{
			"node1",
			"node2"
		})]
		public override ValueTuple<PathNode, PathNode> currentMapPath
		{
			[return: TupleElementNames(new string[]
			{
				"node1",
				"node2"
			})]
			get
			{
				return this.GetPathAt(this.pathIndex);
			}
		}

		// Token: 0x1700053C RID: 1340
		// (get) Token: 0x060019F8 RID: 6648 RVA: 0x00051408 File Offset: 0x0004F608
		[TupleElementNames(new string[]
		{
			"node1",
			"node2"
		})]
		public override ValueTuple<PathNode, PathNode> nextMapPath
		{
			[return: TupleElementNames(new string[]
			{
				"node1",
				"node2"
			})]
			get
			{
				return this.GetPathAt(this.pathIndex + 1);
			}
		}

		// Token: 0x1700053D RID: 1341
		// (get) Token: 0x060019F9 RID: 6649 RVA: 0x00051418 File Offset: 0x0004F618
		public override ParallaxBackground background
		{
			get
			{
				return this._background;
			}
		}

		// Token: 0x060019FA RID: 6650 RVA: 0x00051420 File Offset: 0x0004F620
		[return: TupleElementNames(new string[]
		{
			"node1",
			"node2"
		})]
		private ValueTuple<PathNode, PathNode> GetPathAt(int pathIndex)
		{
			if (pathIndex >= this._path.Length)
			{
				return new ValueTuple<PathNode, PathNode>(PathNode.none, PathNode.none);
			}
			return this._path[pathIndex];
		}

		// Token: 0x060019FB RID: 6651 RVA: 0x0005144C File Offset: 0x0004F64C
		protected virtual void GeneratePath()
		{
			System.Random random = new System.Random(GameData.Save.instance.randomSeed);
			Debug.Log(string.Format("[Generate Path] seed {0}", GameData.Save.instance.randomSeed));
			int num = random.Next(this._normalMaps.x, this._normalMaps.y + 1);
			int num2 = random.Next(this._headRewards.x, this._headRewards.y + 1);
			int num3 = random.Next(this._itemRewards.x, this._itemRewards.y + 1);
			if (num2 > num)
			{
				throw new ArgumentOutOfRangeException("headRewards", "headRewards must be less than normalMaps");
			}
			if (num3 > num)
			{
				throw new ArgumentOutOfRangeException("itemRewards", "itemRewards must be less than normalMaps");
			}
			ValueTuple<PathNode, PathNode>[] array = new ValueTuple<PathNode, PathNode>[num];
			for (int i = 0; i < array.Length; i++)
			{
				List<MapReference> list = this._remainMaps[Map.Type.Normal];
				int index = list.RandomIndex(random);
				MapReference reference = list[index];
				list.RemoveAt(index);
				array[i] = new ValueTuple<PathNode, PathNode>(new PathNode(reference, MapReward.Type.Gold, Gate.Type.Normal), new PathNode(reference, MapReward.Type.Gold, Gate.Type.Normal));
			}
			if (num2 > 0)
			{
				foreach (int num4 in MMMaths.MultipleRandomWithoutDuplactes(random, num2, 0, num))
				{
					array[num4].Item1.reward = MapReward.Type.Head;
					array[num4].Item1.gate = Gate.Type.Grave;
				}
			}
			if (num3 > 0)
			{
				foreach (int num5 in MMMaths.MultipleRandomWithoutDuplactes(random, num3, 0, num))
				{
					array[num5].Item2.reward = MapReward.Type.Item;
					array[num5].Item2.gate = Gate.Type.Chest;
				}
			}
			int[] array3 = null;
			if (this._remainMaps[Map.Type.Special] != null)
			{
				List<MapReference> list2 = (from m in this._remainMaps[Map.Type.Special]
				where !SpecialMap.GetEncoutered(m.specialMapType)
				select m).ToList<MapReference>();
				int count = Math.Min(this.GetSpecialMapCount(random), list2.Count);
				array3 = MMMaths.MultipleRandomWithoutDuplactes(random, count, 0, num);
				for (int k = 0; k < array3.Length; k++)
				{
					int index2 = list2.RandomIndex(random);
					MapReference reference2 = list2[index2];
					list2.RemoveAt(index2);
					int num6 = array3[k];
					array[num6].Item1.reference = reference2;
					array[num6].Item2.reference = reference2;
				}
			}
			if (Singleton<HardmodeManager>.Instance.hardmode)
			{
				int num7 = Mathf.Min(num, DarkEnemySelector.instance.SetTargetCountInStage());
				if (num7 > 0 && num > 0)
				{
					int[] array4 = null;
					if (this._remainMaps[Map.Type.Special] == null)
					{
						array4 = MMMaths.MultipleRandomWithoutDuplactes(random, num7, 0, num);
					}
					else
					{
						bool flag = true;
						int num8 = 0;
						int num9 = 100;
						while (flag && num8 < num9)
						{
							array4 = MMMaths.MultipleRandomWithoutDuplactes(random, num7, 0, num);
							flag = false;
							num8++;
							foreach (int num10 in array4)
							{
								foreach (int num11 in array3)
								{
									if (num10 == num11)
									{
										flag = true;
										break;
									}
								}
								if (flag)
								{
									break;
								}
							}
						}
					}
					foreach (int num12 in array4)
					{
						array[num12].Item1.reference.darkEnemy = true;
						array[num12].Item2.reference.darkEnemy = true;
					}
				}
			}
			int minValue = Mathf.RoundToInt((float)(num * this._castleNpc.positionRange.x) * 0.01f);
			int maxValue = Mathf.RoundToInt((float)(num * this._castleNpc.positionRange.y) * 0.01f);
			int num13 = random.Next(minValue, maxValue) + 1;
			if (!this._castleNpc.reference.IsNullOrEmpty() && !GameData.Progress.GetRescued(this._npcType))
			{
				array[num13].Item1 = this._castleNpc;
				array[num13].Item2 = this._castleNpc;
			}
			for (int n = 0; n < array.Length; n++)
			{
				if (random.Next(2) == 0)
				{
					PathNode item = array[n].Item1;
					array[n].Item1 = array[n].Item2;
					array[n].Item2 = item;
				}
			}
			List<ValueTuple<PathNode, PathNode>> list3 = new List<ValueTuple<PathNode, PathNode>>(num + 2);
			list3.AddRange(array);
			if (!this._entry.reference.IsNullOrEmpty())
			{
				list3.Insert(0, new ValueTuple<PathNode, PathNode>(this._entry, PathNode.none));
			}
			if (!this._terminal.reference.IsNullOrEmpty())
			{
				list3.Add(new ValueTuple<PathNode, PathNode>(this._terminal, PathNode.none));
			}
			List<StageInfo.ExtraMapInfo> list4 = new List<StageInfo.ExtraMapInfo>();
			foreach (StageInfo.ExtraMapInfo extraMapInfo in this._extraMaps.values)
			{
				if (MMMaths.Chance(random, extraMapInfo.possibility / 100f))
				{
					list4.Add(extraMapInfo);
				}
			}
			foreach (StageInfo.ExtraMapInfo extraMapInfo2 in list4)
			{
				int minValue2 = Mathf.RoundToInt((float)(num * extraMapInfo2.positionRange.x) * 0.01f);
				int maxValue2 = Mathf.RoundToInt((float)(num * extraMapInfo2.positionRange.y) * 0.01f);
				int index3 = random.Next(minValue2, maxValue2) + 1;
				list3.Insert(index3, new ValueTuple<PathNode, PathNode>(extraMapInfo2, extraMapInfo2));
			}
			PathNode pathNode = new PathNode(null, MapReward.Type.None, this._lastGate);
			list3.Add(new ValueTuple<PathNode, PathNode>(pathNode, pathNode));
			this._path = list3.ToArray();
		}

		// Token: 0x060019FC RID: 6652 RVA: 0x00051A54 File Offset: 0x0004FC54
		protected int GetSpecialMapCount(System.Random random)
		{
			double num = random.NextDouble() * (double)this._specialMapWeights.Sum();
			for (int i = 0; i < this._specialMapWeights.Length; i++)
			{
				num -= (double)this._specialMapWeights[i];
				if (num <= 0.0)
				{
					return i;
				}
			}
			return 0;
		}

		// Token: 0x060019FD RID: 6653 RVA: 0x00051AA4 File Offset: 0x0004FCA4
		public override void Reset()
		{
			foreach (IGrouping<Map.Type, MapReference> grouping in this._maps)
			{
				this._remainMaps[grouping.Key] = grouping.ToList<MapReference>();
			}
			this.GeneratePath();
		}

		// Token: 0x060019FE RID: 6654 RVA: 0x00051B08 File Offset: 0x0004FD08
		public override PathNode Next(NodeIndex nodeIndex)
		{
			this.nodeIndex = nodeIndex;
			this.pathIndex++;
			if (this.pathIndex >= this._path.Length)
			{
				return null;
			}
			ValueTuple<PathNode, PathNode> valueTuple = this._path[this.pathIndex];
			PathNode item = valueTuple.Item1;
			PathNode item2 = valueTuple.Item2;
			if (nodeIndex == NodeIndex.Node2)
			{
				return item2;
			}
			return item;
		}

		// Token: 0x060019FF RID: 6655 RVA: 0x00002191 File Offset: 0x00000391
		public override void UpdateReferences()
		{
		}

		// Token: 0x040016B9 RID: 5817
		[SerializeField]
		protected SerializablePathNode _entry;

		// Token: 0x040016BA RID: 5818
		[SerializeField]
		protected SerializablePathNode _terminal;

		// Token: 0x040016BB RID: 5819
		[SerializeField]
		protected Gate.Type _lastGate;

		// Token: 0x040016BC RID: 5820
		[SerializeField]
		protected StageInfo.ExtraMapInfo _castleNpc;

		// Token: 0x040016BD RID: 5821
		[SerializeField]
		protected NpcType _npcType;

		// Token: 0x040016BE RID: 5822
		[SerializeField]
		private ParallaxBackground _background;

		// Token: 0x040016BF RID: 5823
		[Tooltip("일반 전투 맵 개수")]
		[SerializeField]
		protected Vector2Int _normalMaps;

		// Token: 0x040016C0 RID: 5824
		[Tooltip("헤드 보상 맵 개수")]
		[SerializeField]
		protected Vector2Int _headRewards;

		// Token: 0x040016C1 RID: 5825
		[Tooltip("아이템 보상 맵 개수")]
		[SerializeField]
		protected Vector2Int _itemRewards;

		// Token: 0x040016C2 RID: 5826
		[Header("Special Maps")]
		[Tooltip("해당 스테이지에 스페셜 맵이 n개 나올 비중. 예를 들어 [0, 30, 70]이면 30% 확률로 1개 등장, 70% 확률로 2개 등장")]
		[SerializeField]
		private float[] _specialMapWeights;

		// Token: 0x040016C3 RID: 5827
		[SerializeField]
		[Header("Extra Maps")]
		protected StageInfo.ExtraMapInfo.Reorderable _extraMaps;

		// Token: 0x040016C4 RID: 5828
		[TupleElementNames(new string[]
		{
			"type1",
			"type2"
		})]
		public ValueTuple<PathNode, PathNode>[] _path;

		// Token: 0x040016C5 RID: 5829
		private ILookup<Map.Type, MapReference> _maps;

		// Token: 0x040016C6 RID: 5830
		protected EnumArray<Map.Type, List<MapReference>> _remainMaps = new EnumArray<Map.Type, List<MapReference>>();

		// Token: 0x0200052A RID: 1322
		[Serializable]
		public class ExtraMapInfo : SerializablePathNode
		{
			// Token: 0x040016C7 RID: 5831
			[Range(0f, 100f)]
			public float possibility = 100f;

			// Token: 0x040016C8 RID: 5832
			[MinMaxSlider(0f, 100f)]
			public Vector2Int positionRange;

			// Token: 0x0200052B RID: 1323
			[Serializable]
			public new class Reorderable : ReorderableArray<StageInfo.ExtraMapInfo>
			{
			}
		}
	}
}
