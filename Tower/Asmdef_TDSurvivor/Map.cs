using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Splines;

// Token: 0x020000A1 RID: 161
public class Map : MonoBehaviour
{
	// Token: 0x0600034B RID: 843 RVA: 0x0000C7FE File Offset: 0x0000A9FE
	public List<MapNode> GetMapNodes()
	{
		return this.list_MapNodes;
	}

	// Token: 0x0600034C RID: 844 RVA: 0x0000C806 File Offset: 0x0000AA06
	public MapNode GetMapNode(int index)
	{
		return this.list_MapNodes[index];
	}

	// Token: 0x0600034D RID: 845 RVA: 0x0000C814 File Offset: 0x0000AA14
	public List<MapNode> GetNextStepMapNodes(MapNode node)
	{
		return this.GetNextStepMapNodes(node.mapNodeData.Index);
	}

	// Token: 0x0600034E RID: 846 RVA: 0x0000C828 File Offset: 0x0000AA28
	public List<MapNode> GetNextStepMapNodes(int index)
	{
		List<int> connectedIndex = this.list_MapNodes[index].mapNodeData.connectedIndex;
		return this.GetMapNodes(connectedIndex);
	}

	// Token: 0x0600034F RID: 847 RVA: 0x0000C854 File Offset: 0x0000AA54
	public List<MapNode> GetMapNodes(List<int> selectedIndex)
	{
		List<MapNode> list = new List<MapNode>();
		foreach (int index in selectedIndex)
		{
			list.Add(this.list_MapNodes[index]);
		}
		return (from a in list
		orderby a.mapNodeData.Index
		select a).ToList<MapNode>();
	}

	// Token: 0x06000350 RID: 848 RVA: 0x0000C8E0 File Offset: 0x0000AAE0
	public List<MapNode> GetMapNodes(params int[] selectedIndex)
	{
		List<MapNode> list = new List<MapNode>();
		foreach (int index in selectedIndex)
		{
			list.Add(this.list_MapNodes[index]);
		}
		return (from a in list
		orderby a.mapNodeData.Index
		select a).ToList<MapNode>();
	}

	// Token: 0x06000351 RID: 849 RVA: 0x0000C944 File Offset: 0x0000AB44
	public void ClearMap()
	{
		if (this.list_MapNodes == null || this.list_MapNodes.Count == 0)
		{
			return;
		}
		for (int i = this.list_MapNodes.Count - 1; i >= 0; i--)
		{
			Object.Destroy(this.list_MapNodes[i].gameObject);
		}
		this.list_MapNodes.Clear();
	}

	// Token: 0x06000352 RID: 850 RVA: 0x0000C9A0 File Offset: 0x0000ABA0
	public MapData GenerateMapData(MapGenerateSetting setting)
	{
		MapData mapData = new MapData();
		mapData.randomSeed = setting.Seed;
		UnityEngine.Random.InitState(setting.Seed);
		MapNodeData mapNodeData = mapData.AddNode(eStageType.START, eMapNodeState.JUST_FINISHED, 0, 0);
		mapNodeData.SetCleared();
		new List<MapNodeData>().Add(mapNodeData);
		for (int i = 0; i < setting.Step; i++)
		{
			List<MapNodeData> list = new List<MapNodeData>();
			int num;
			if (i == 0)
			{
				num = 1;
			}
			else
			{
				num = UnityEngine.Random.Range(setting.MinNodeInStep, setting.MaxNodeInStep + 1);
			}
			this.nodeCooldown_Shop--;
			this.nodeCooldown_SpecialEvent--;
			this.nodeCooldown_Workshop--;
			this.nodeCooldown_CorruptedBattle--;
			for (int j = 0; j < num; j++)
			{
				eStageType eStageType = this.RollMapNodeType(setting, i);
				MapNodeData mapNodeData2;
				if (i == 0)
				{
					mapNodeData2 = mapData.AddNode(eStageType, eMapNodeState.AVALIABLE, i + 1, j);
				}
				else
				{
					mapNodeData2 = mapData.AddNode(eStageType, eMapNodeState.TOO_FAR, i + 1, j);
				}
				mapNodeData2.difficulty = Mathf.Clamp(Mathf.FloorToInt((float)i * 1.5f), 1, 9);
				if (eStageType == eStageType.BATTLE)
				{
					StageRewardData stageRewardData = this.RollStageReward();
					stageRewardData.Exp = 10 + i * 5;
					stageRewardData.Gem = 5 * (i + 1);
					mapNodeData2.SetStageReward(stageRewardData);
				}
				else if (eStageType == eStageType.BATTLE_CORRUPTED)
				{
					StageRewardData stageRewardData2 = this.RollStageReward();
					stageRewardData2.Exp = 30 + i * 5;
					stageRewardData2.Gem = 10 * (i + 1);
					mapNodeData2.SetStageReward(stageRewardData2);
				}
				list.Add(mapNodeData2);
			}
		}
		mapData.AddNode(eStageType.BOSS, eMapNodeState.TOO_FAR, setting.Step + 1, 0).SetEnvSceneName("EnvScene_099_Boss", 10);
		int k = 0;
		while (k < mapData.list_MapNodeData.Count - 1)
		{
			MapNodeData curNode = mapData.list_MapNodeData[k];
			if (k == 0)
			{
				using (IEnumerator<MapNodeData> enumerator = (from item in mapData.list_MapNodeData
				where item.Step == 1
				select item).GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						MapNodeData mapNodeData3 = enumerator.Current;
						curNode.connectedIndex.Add(mapNodeData3.Index);
						mapNodeData3.connectedFromCount++;
						mapNodeData3.State = eMapNodeState.AVALIABLE;
					}
					goto IL_2E0;
				}
				goto IL_248;
			}
			goto IL_248;
			IL_2E0:
			k++;
			continue;
			IL_248:
			int randomNumberOfConnections = this.GetRandomNumberOfConnections();
			List<MapNodeData> list2 = (from item in mapData.list_MapNodeData
			where item.Step == curNode.Step + 1
			select item).ToList<MapNodeData>();
			List<int> list3 = new List<int>();
			for (int l = 0; l < randomNumberOfConnections; l++)
			{
				MapNodeData mapNodeData4 = list2[UnityEngine.Random.Range(0, list2.Count)];
				curNode.AddConnection(mapNodeData4.Index);
				mapNodeData4.connectedFromCount++;
				list3.Add(mapNodeData4.Index);
				list2.Remove(mapNodeData4);
				if (list2.Count == 0)
				{
					break;
				}
			}
			goto IL_2E0;
		}
		for (int m = 1; m < mapData.list_MapNodeData.Count; m++)
		{
			MapNodeData curNode = mapData.list_MapNodeData[m];
			if (curNode.connectedFromCount == 0)
			{
				List<MapNodeData> list4 = (from item in mapData.list_MapNodeData
				where item.Step == curNode.Step - 1
				select item).ToList<MapNodeData>();
				list4[UnityEngine.Random.Range(0, list4.Count)].AddConnection(m);
				curNode.connectedFromCount++;
			}
		}
		string text = string.Format("地圖產生完成 (seed{0}):\n", mapData.randomSeed);
		for (int n = 0; n < mapData.list_MapNodeData.Count; n++)
		{
			MapNodeData mapNodeData5 = mapData.list_MapNodeData[n];
			text += string.Format("[{0} (階段{1})] - {2}, 狀態:{3}, 連入:{4}, 連出:{5}", new object[]
			{
				mapNodeData5.Index,
				mapNodeData5.Step,
				mapNodeData5.MapNodeType,
				mapNodeData5.State,
				mapNodeData5.connectedFromCount,
				mapNodeData5.connectedIndex.Count
			});
			text += "\n";
		}
		Debug.Log(text);
		mapData.IsGenerated = true;
		return mapData;
	}

	// Token: 0x06000353 RID: 851 RVA: 0x0000CE20 File Offset: 0x0000B020
	private StageRewardData RollStageReward()
	{
		StageRewardData stageRewardData = new StageRewardData(0, 0, eStageRewardType.NONE, eItemType.NONE);
		WeightedRandom<eStageRewardType> weightedRandom = new WeightedRandom<eStageRewardType>();
		weightedRandom.AddItem(eStageRewardType.TOWER, 5);
		stageRewardData.RewardType = weightedRandom.GetRandomResult();
		if (stageRewardData.RewardType == eStageRewardType.TETRIS)
		{
			stageRewardData.ItemType = Singleton<ResourceManager>.Instance.GetRandomPanelType(1, false).First<eItemType>();
		}
		if (stageRewardData.RewardType == eStageRewardType.TOWER)
		{
			stageRewardData.ItemType = eItemType.NONE;
		}
		return stageRewardData;
	}

	// Token: 0x06000354 RID: 852 RVA: 0x0000CE84 File Offset: 0x0000B084
	public void VisualizeMap(MapGenerateSetting setting, MapData mapData)
	{
		List<MapNodeData> list_MapNodeData = mapData.list_MapNodeData;
		this.list_MapNodes = new List<MapNode>();
		foreach (SplineContainer splineContainer in this.list_Splines)
		{
			if (splineContainer.gameObject.activeSelf)
			{
				splineContainer.gameObject.SetActive(false);
			}
		}
		this.spline = this.list_Splines[setting.Seed % this.list_Splines.Count];
		this.spline.gameObject.SetActive(true);
		foreach (MapNodeData mapNodeData in list_MapNodeData)
		{
			MapNode item = this.CreateMapNode(setting, mapData, mapNodeData.MapNodeType, mapNodeData);
			this.list_MapNodes.Add(item);
		}
		this.SetNodeRandomOffset();
		foreach (MapNode mapNode in this.list_MapNodes)
		{
			List<MapNode> list = new List<MapNode>();
			for (int i = 0; i < mapNode.mapNodeData.connectedIndex.Count; i++)
			{
				int index = mapNode.mapNodeData.connectedIndex[i];
				list.Add(this.list_MapNodes[index]);
			}
			list = (from a in list
			orderby a.mapNodeData.IndexInStep
			select a).ToList<MapNode>();
			mapNode.CalculateLines(list);
		}
		foreach (MapNode mapNode2 in this.list_MapNodes)
		{
			mapNode2.SwitchState(mapNode2.mapNodeData.State);
		}
		List<Vector3> source = (from x in this.list_MapNodes
		select x.transform.localPosition).ToList<Vector3>();
		float num = source.Min((Vector3 x) => x.y);
		float num2 = source.Max((Vector3 x) => x.y);
		float num3 = (num + num2) / 2f;
		float num4 = Mathf.Abs(num) + Mathf.Abs(num2);
		this.anchor_MapElementSpawn.GetComponent<RectTransform>();
		Vector3 localPosition = this.list_MapNodes[this.list_MapNodes.Count - 1].transform.localPosition;
		Vector3 localPosition2 = this.list_MapNodes[0].transform.localPosition;
		this.anchor_MapElementSpawn.GetComponent<RectTransform>().sizeDelta = new Vector2(localPosition.x + this.endNodeBorderPadding.x, num4 + 1080f);
		this.anchor_MapElementSpawn.GetComponent<RectTransform>().anchoredPosition = new Vector2(this.anchor_MapElementSpawn.GetComponent<RectTransform>().anchoredPosition.x, num3);
		foreach (MapNode mapNode3 in this.list_MapNodes)
		{
			mapNode3.transform.localPosition = mapNode3.transform.localPosition + Vector3.down * num3;
		}
	}

	// Token: 0x06000355 RID: 853 RVA: 0x0000D240 File Offset: 0x0000B440
	private MapNode CreateMapNode(MapGenerateSetting setting, MapData mapData, eStageType mapNodeType, MapNodeData mapNodeData)
	{
		GameObject prefabForElement = this.GetPrefabForElement(mapNodeType);
		bool flag = mapData.list_NodeCountInEachStep[mapNodeData.Step] != 0;
		Vector3 zero = Vector3.zero;
		float3 rhs;
		float3 @float;
		float3 float2;
		this.spline.Evaluate(0f, out rhs, out @float, out float2);
		float3 float3;
		float3 float4;
		float3 float5;
		this.spline.Evaluate((float)mapNodeData.Step / (float)(setting.Step + 1), out float3, out float4, out float5);
		float3 -= rhs;
		float3 *= Singleton<UIManager>.Instance.GetCanvasScale();
		float angle = Vector3.SignedAngle(new Vector3(float4.x, float4.y, float4.z), Vector3.right, Vector3.back);
		Vector3 vector = new Vector3((float)mapNodeData.Step * this.stepHorizontalOffset, 0f, 0f);
		vector = float3;
		float num = this.iconRange * (float)(2 * (mapNodeData.IndexInStep / 2) + 1) * (float)((mapNodeData.IndexInStep % 2 == 0) ? 1 : -1);
		if ((flag ? 1 : 0) % 2 == 0)
		{
			num += 0.5f * this.iconRange;
		}
		Vector3 vector2 = vector + new Vector3(0f, num, 0f);
		vector2 = this.RotatePointAroundCenter(vector2, vector, angle);
		GameObject gameObject = Object.Instantiate<GameObject>(prefabForElement, vector2, Quaternion.identity, this.anchor_MapElementSpawn);
		RectTransform rectTransform = gameObject.transform as RectTransform;
		rectTransform.anchorMin = new Vector2(0f, 0.5f);
		rectTransform.anchorMax = new Vector2(0f, 0.5f);
		gameObject.transform.localPosition = vector2.WithZ(0f);
		MapNode component = gameObject.GetComponent<MapNode>();
		component.Initialize(mapNodeData, mapData, this);
		return component;
	}

	// Token: 0x06000356 RID: 854 RVA: 0x0000D3E4 File Offset: 0x0000B5E4
	private Vector3 RotatePointAroundCenter(Vector3 pointToRotate, Vector3 centerPoint, float angle)
	{
		Vector3 point = pointToRotate - centerPoint;
		Vector3 b = Quaternion.Euler(0f, 0f, angle) * point;
		return centerPoint + b;
	}

	// Token: 0x06000357 RID: 855 RVA: 0x0000D418 File Offset: 0x0000B618
	private eStageType RollMapNodeType(MapGenerateSetting setting, int step)
	{
		bool flag = true;
		bool flag2 = true;
		bool flag3 = true;
		bool flag4 = true;
		WeightedRandom<eStageType> weightedRandom = new WeightedRandom<eStageType>();
		if (step == 0)
		{
			return eStageType.ACADEMY;
		}
		if (step <= 1)
		{
			flag2 = false;
			flag3 = false;
			flag4 = false;
		}
		if (step < 3)
		{
			flag2 = false;
		}
		if (flag)
		{
			weightedRandom.AddItem(eStageType.BATTLE, setting.Weight_Level);
			if (step > 1 && this.nodeCooldown_CorruptedBattle < 0)
			{
				weightedRandom.AddItem(eStageType.BATTLE_CORRUPTED, setting.Weight_Level_Corrupted);
			}
		}
		if (this.nodeCooldown_Shop <= 0 && flag2)
		{
			weightedRandom.AddItem(eStageType.SHOP, setting.Weight_Shop);
		}
		if (this.nodeCooldown_Workshop <= 0 && flag4)
		{
			weightedRandom.AddItem(eStageType.WORKSHOP, setting.Weight_Workshop);
		}
		if (this.nodeCooldown_SpecialEvent <= 0 && flag3)
		{
			weightedRandom.AddItem(eStageType.SPECIAL_EVENT, setting.Weight_SpecialEvent);
		}
		if (weightedRandom.GetItemCount() == 0)
		{
			weightedRandom.AddItem(eStageType.BATTLE, setting.Weight_Level);
		}
		eStageType randomResult = weightedRandom.GetRandomResult();
		if (randomResult <= eStageType.SPECIAL_EVENT)
		{
			if (randomResult != eStageType.SHOP)
			{
				if (randomResult == eStageType.SPECIAL_EVENT)
				{
					this.nodeCooldown_SpecialEvent = 2;
				}
			}
			else
			{
				this.nodeCooldown_Shop = 2;
			}
		}
		else if (randomResult != eStageType.WORKSHOP)
		{
			if (randomResult == eStageType.BATTLE_CORRUPTED)
			{
				this.nodeCooldown_CorruptedBattle = 1;
			}
		}
		else
		{
			this.nodeCooldown_Workshop = 2;
		}
		return randomResult;
	}

	// Token: 0x06000358 RID: 856 RVA: 0x0000D534 File Offset: 0x0000B734
	private GameObject GetPrefabForElement(eStageType nodeType)
	{
		switch (nodeType)
		{
		case eStageType.START:
			return this.startNodePrefab;
		case eStageType.BATTLE:
			return this.levelNodePrefab;
		case eStageType.SHOP:
			return this.shopNodePrefab;
		case eStageType.SPECIAL_EVENT:
			return this.specialEventNodePrefab;
		case eStageType.BOSS:
			return this.bossNodePrefab;
		case eStageType.WORKSHOP:
			return this.workshopPrefab;
		case eStageType.ACADEMY:
			return this.academyPrefab;
		case eStageType.BATTLE_CORRUPTED:
			return this.corruptedLevelNodePrefab;
		}
		throw new Exception(string.Format("指定的mapnode類型沒有設定Prefab: {0}", nodeType));
	}

	// Token: 0x06000359 RID: 857 RVA: 0x0000D5C8 File Offset: 0x0000B7C8
	private int GetRandomNumberOfConnections()
	{
		int num = UnityEngine.Random.Range(1, 101);
		if (num <= 80)
		{
			return 1;
		}
		if (num <= 95)
		{
			return 2;
		}
		return 3;
	}

	// Token: 0x0600035A RID: 858 RVA: 0x0000D5F0 File Offset: 0x0000B7F0
	private void SetNodeRandomOffset()
	{
		foreach (MapNode mapNode in this.list_MapNodes)
		{
			Vector3 vector = Vector3.zero;
			if (mapNode.mapNodeData.MapNodeType == eStageType.START || mapNode.mapNodeData.MapNodeType == eStageType.BOSS)
			{
				vector = Vector3.zero;
			}
			else
			{
				vector = new Vector3(UnityEngine.Random.Range(-this.randNodeOffset_X, this.randNodeOffset_X), UnityEngine.Random.Range(-this.randNodeOffset_Y, this.randNodeOffset_Y), 0f);
			}
			vector += this.startNodeOffset;
			mapNode.transform.localPosition += vector;
		}
	}

	// Token: 0x0600035B RID: 859 RVA: 0x0000D6C0 File Offset: 0x0000B8C0
	private void CreateMapObjects()
	{
		foreach (MapNode mapNode in this.list_MapNodes)
		{
			for (int i = 0; i < 10; i++)
			{
				Vector3 position = mapNode.transform.position + UnityEngine.Random.insideUnitCircle.normalized * 2.5f;
				GameObject gameObject = Object.Instantiate<GameObject>(this.mapStampPrefab, position, Quaternion.identity, this.anchor_MapElementSpawn);
				gameObject.transform.localPosition = gameObject.transform.localPosition.WithZ(0f);
				gameObject.GetComponent<UI_MapStamp>().Stamp();
			}
		}
	}

	// Token: 0x04000370 RID: 880
	public GameObject startNodePrefab;

	// Token: 0x04000371 RID: 881
	public GameObject levelNodePrefab;

	// Token: 0x04000372 RID: 882
	public GameObject corruptedLevelNodePrefab;

	// Token: 0x04000373 RID: 883
	public GameObject shopNodePrefab;

	// Token: 0x04000374 RID: 884
	public GameObject specialEventNodePrefab;

	// Token: 0x04000375 RID: 885
	public GameObject bossNodePrefab;

	// Token: 0x04000376 RID: 886
	public GameObject workshopPrefab;

	// Token: 0x04000377 RID: 887
	public GameObject academyPrefab;

	// Token: 0x04000378 RID: 888
	public GameObject mapStampPrefab;

	// Token: 0x04000379 RID: 889
	public List<SplineContainer> list_Splines;

	// Token: 0x0400037A RID: 890
	[SerializeField]
	private Transform anchor_MapElementSpawn;

	// Token: 0x0400037B RID: 891
	public float stepHorizontalOffset = 5f;

	// Token: 0x0400037C RID: 892
	public float iconRange = 2f;

	// Token: 0x0400037D RID: 893
	public float randNodeOffset_X = 0.5f;

	// Token: 0x0400037E RID: 894
	public float randNodeOffset_Y = 0.5f;

	// Token: 0x0400037F RID: 895
	public Vector3 startNodeOffset = new Vector3(0f, 0f, 0f);

	// Token: 0x04000380 RID: 896
	public Vector3 endNodeBorderPadding = new Vector3(0f, 0f, 0f);

	// Token: 0x04000381 RID: 897
	private List<MapNode> list_MapNodes;

	// Token: 0x04000382 RID: 898
	private SplineContainer spline;

	// Token: 0x04000383 RID: 899
	private int nodeCooldown_Shop;

	// Token: 0x04000384 RID: 900
	private int nodeCooldown_Workshop;

	// Token: 0x04000385 RID: 901
	private int nodeCooldown_SpecialEvent;

	// Token: 0x04000386 RID: 902
	private int nodeCooldown_CorruptedBattle;
}
