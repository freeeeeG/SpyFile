using System;
using System.Collections.Generic;
using System.Linq;
using Data;
using GameResources;
using UnityEngine;

namespace Level
{
	// Token: 0x020004F4 RID: 1268
	[CreateAssetMenu]
	public sealed class HardmodeStageInfo : StageInfo
	{
		// Token: 0x060018D7 RID: 6359 RVA: 0x0004DDD4 File Offset: 0x0004BFD4
		protected override void GeneratePath()
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
			int num4 = DarkEnemySelector.instance.SetTargetCountInStage();
			if (num4 > 0 && num > 0)
			{
				Debug.Log(string.Format("DarkEnemies : {0}", num4));
				Debug.Log(string.Format("Normal Maps : {0}", num));
				foreach (int num5 in MMMaths.MultipleRandomWithoutDuplactes(random, num4, 0, num))
				{
					array[num5].Item1.reference.darkEnemy = true;
					array[num5].Item2.reference.darkEnemy = true;
				}
			}
			if (num2 > 0)
			{
				foreach (int num6 in MMMaths.MultipleRandomWithoutDuplactes(random, num2, 0, num))
				{
					array[num6].Item1.reward = MapReward.Type.Head;
					array[num6].Item1.gate = Gate.Type.Grave;
				}
			}
			if (num3 > 0)
			{
				foreach (int num7 in MMMaths.MultipleRandomWithoutDuplactes(random, num3, 0, num))
				{
					array[num7].Item2.reward = MapReward.Type.Item;
					array[num7].Item2.gate = Gate.Type.Chest;
				}
			}
			if (this._remainMaps[Map.Type.Special] != null)
			{
				List<MapReference> list2 = (from m in this._remainMaps[Map.Type.Special]
				where !SpecialMap.GetEncoutered(m.specialMapType)
				select m).ToList<MapReference>();
				int count = Math.Min(base.GetSpecialMapCount(random), list2.Count);
				int[] array3 = MMMaths.MultipleRandomWithoutDuplactes(random, count, 0, num);
				for (int k = 0; k < array3.Length; k++)
				{
					int index2 = list2.RandomIndex(random);
					MapReference reference2 = list2[index2];
					list2.RemoveAt(index2);
					int num8 = array3[k];
					array[num8].Item1.reference = reference2;
					array[num8].Item2.reference = reference2;
				}
			}
			int minValue = Mathf.RoundToInt((float)(num * this._castleNpc.positionRange.x) * 0.01f);
			int maxValue = Mathf.RoundToInt((float)(num * this._castleNpc.positionRange.y) * 0.01f);
			int num9 = random.Next(minValue, maxValue) + 1;
			if (!this._castleNpc.reference.IsNullOrEmpty() && !GameData.Progress.GetRescued(this._npcType))
			{
				array[num9].Item1 = this._castleNpc;
				array[num9].Item2 = this._castleNpc;
			}
			for (int l = 0; l < array.Length; l++)
			{
				if (random.Next(2) == 0)
				{
					PathNode item = array[l].Item1;
					array[l].Item1 = array[l].Item2;
					array[l].Item2 = item;
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

		// Token: 0x040015B1 RID: 5553
		[Header("HardmodeData")]
		[SerializeField]
		private int a;
	}
}
