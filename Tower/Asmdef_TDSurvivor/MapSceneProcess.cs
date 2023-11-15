using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI.Extensions;

// Token: 0x020000AD RID: 173
public class MapSceneProcess : MonoBehaviour
{
	// Token: 0x06000397 RID: 919 RVA: 0x0000E5D0 File Offset: 0x0000C7D0
	private void OnEnable()
	{
		EventMgr.Register<MapNode>(eMapSceneEvents.OnClickMapNodeButton, new Action<MapNode>(this.OnClickMapNodeButton));
		EventMgr.Register<MapNode>(eMapSceneEvents.TestMapNodeScroll, new Action<MapNode>(this.OnTestMapNodeScroll));
	}

	// Token: 0x06000398 RID: 920 RVA: 0x0000E601 File Offset: 0x0000C801
	private void OnDisable()
	{
		EventMgr.Remove<MapNode>(eMapSceneEvents.OnClickMapNodeButton, new Action<MapNode>(this.OnClickMapNodeButton));
		EventMgr.Register<MapNode>(eMapSceneEvents.TestMapNodeScroll, new Action<MapNode>(this.OnTestMapNodeScroll));
	}

	// Token: 0x06000399 RID: 921 RVA: 0x0000E632 File Offset: 0x0000C832
	private void OnTestMapNodeScroll(MapNode node)
	{
		this.scrollViewControl.CenterToTarget(node.transform as RectTransform, new Vector2(0.5f, 0.5f));
	}

	// Token: 0x0600039A RID: 922 RVA: 0x0000E659 File Offset: 0x0000C859
	private IEnumerator Start()
	{
		GameplayData gameplayData = GameDataManager.instance.GameplayData;
		if (gameplayData == null || !gameplayData.isInitialized)
		{
			GameDataManager.instance.StartNewGame(Random.Range(0, 99999999));
			gameplayData = GameDataManager.instance.GameplayData;
		}
		if (gameplayData.mapData == null || !gameplayData.mapData.IsGenerated)
		{
			gameplayData.mapData = this.mapManager.GenerateMapData(gameplayData.mapGenerateSetting);
		}
		this.mapManager.VisualizeMap(gameplayData.mapGenerateSetting, gameplayData.mapData);
		this.scrollViewControl.CenterToTarget(this.mapManager.GetMapNode(gameplayData.CurMapNodeIndex).transform as RectTransform, new Vector2(0.5f, 0.5f));
		this.isSelectedNode = false;
		this.previousMapNode = null;
		this.currentMapNode = this.mapManager.GetMapNode(gameplayData.CurMapNodeIndex);
		foreach (MapNode mapNode in this.mapManager.GetMapNodes())
		{
			if (mapNode.mapNodeData.Step <= this.currentMapNode.mapNodeData.Step && mapNode != this.currentMapNode && mapNode.IsState(eMapNodeState.TOO_FAR))
			{
				mapNode.SwitchState(eMapNodeState.NOT_SELECTED);
			}
		}
		SoundManager.PlayMusic("Main", "BGM_MapPage", true, 1f, 0f, 1f);
		yield return new WaitForSeconds(1f);
		EventMgr.SendEvent(eGameEvents.UI_TriggerTransition_Hide);
		yield return new WaitForSeconds(0.45f);
		EventMgr.SendEvent<bool>(eMapSceneEvents.ToggleMapSceneBasicUI, true);
		if (gameplayData.CurMapNodeIndex == 0)
		{
			yield return base.StartCoroutine(this.CR_ShowFullMap());
			yield return base.StartCoroutine(this.CR_ShowMapNodes(false));
		}
		else
		{
			yield return base.StartCoroutine(this.CR_ShowMapNodes(true));
		}
		yield return base.StartCoroutine(this.CR_UpdateMapNodeState());
		Debug.Log(string.Format("目前節點類型: {0}", this.currentMapNode.mapNodeData.MapNodeType));
		if (this.currentMapNode.mapNodeData.MapNodeType == eStageType.BATTLE && GameDataManager.instance.Playerdata.Exp > 10)
		{
			MapSceneProcess.<>c__DisplayClass14_0 CS$<>8__locals1 = new MapSceneProcess.<>c__DisplayClass14_0();
			Debug.Log("觸發天賦的新手教學");
			CS$<>8__locals1.isTutorialFinished = false;
			EventMgr.SendEvent<eTutorialType, float, Action>(eGameEvents.RequestTutorial, eTutorialType.TALENTS, 1f, delegate()
			{
				CS$<>8__locals1.isTutorialFinished = true;
			});
			while (!CS$<>8__locals1.isTutorialFinished)
			{
				yield return null;
			}
			yield return base.StartCoroutine(this.CR_MainProc());
			CS$<>8__locals1 = null;
		}
		else
		{
			yield return base.StartCoroutine(this.CR_MainProc());
		}
		yield break;
	}

	// Token: 0x0600039B RID: 923 RVA: 0x0000E668 File Offset: 0x0000C868
	private IEnumerator CR_MainProc()
	{
		for (;;)
		{
			DebugManager.Log(eDebugKey.MAP_SCENE, "(MAIN) 等待玩家選擇節點...", null);
			while (!this.isSelectedNode)
			{
				yield return null;
			}
			this.isSelectedNode = false;
			if (this.previousMapNode != null)
			{
				foreach (MapNode mapNode in this.mapManager.GetNextStepMapNodes(this.previousMapNode))
				{
					if (!(mapNode == this.currentMapNode))
					{
						mapNode.SwitchState(eMapNodeState.NOT_SELECTED);
					}
				}
			}
			if (this.previousMapNode != null)
			{
				foreach (MapNode mapNode2 in this.mapManager.GetNextStepMapNodes(this.previousMapNode))
				{
					if (!(mapNode2 == this.currentMapNode))
					{
						mapNode2.SwitchState(eMapNodeState.NOT_SELECTED);
					}
				}
				this.previousMapNode.SwitchState(eMapNodeState.COMPLETED);
			}
			DebugManager.Log(eDebugKey.MAP_SCENE, "(MAIN) 玩家已選擇, 等待節點流程結束...", null);
			while (this.isInNodeProcess)
			{
				yield return null;
			}
			DebugManager.Log(eDebugKey.MAP_SCENE, "(MAIN) 節點流程結束, 處理節點狀態變化.", null);
			foreach (MapNode mapNode3 in this.mapManager.GetNextStepMapNodes(this.currentMapNode))
			{
				mapNode3.SwitchState(eMapNodeState.AVALIABLE);
			}
			this.currentMapNode.mapNodeData.SetCleared();
			this.currentMapNode.SwitchState(eMapNodeState.JUST_FINISHED);
			Vector3 position = this.currentMapNode.transform.position;
			this.scrollViewControl.CenterToTarget(this.currentMapNode.transform as RectTransform, new Vector2(0.5f, 0.5f));
			foreach (MapNode mapNode4 in this.mapManager.GetMapNodes())
			{
				if (mapNode4.mapNodeData.Step <= this.currentMapNode.mapNodeData.Step && mapNode4 != this.currentMapNode && mapNode4.IsState(eMapNodeState.TOO_FAR))
				{
					mapNode4.SwitchState(eMapNodeState.NOT_SELECTED);
				}
			}
			GameDataManager.instance.SaveData();
			yield return base.StartCoroutine(this.CR_UpdateMapNodeState());
			if (this.currentMapNode.mapNodeData.MapNodeType == eStageType.BATTLE || this.currentMapNode.mapNodeData.MapNodeType == eStageType.BATTLE_CORRUPTED || this.currentMapNode.mapNodeData.MapNodeType == eStageType.BOSS)
			{
				break;
			}
			this.previousMapNode = this.currentMapNode;
			this.currentMapNode = null;
			GameDataManager.instance.SaveData();
		}
		yield break;
		yield break;
	}

	// Token: 0x0600039C RID: 924 RVA: 0x0000E678 File Offset: 0x0000C878
	private void OnClickMapNodeButton(MapNode clickedNode)
	{
		if (this.currentMapNode != null)
		{
			this.previousMapNode = this.currentMapNode;
		}
		this.isSelectedNode = true;
		this.currentMapNode = clickedNode;
		GameDataManager.instance.GameplayData.SetCurrentMapNodeIndex(this.currentMapNode.mapNodeData.Index);
		this.scrollViewControl.CenterToTarget(clickedNode.transform as RectTransform, new Vector2(0.5f, 0.5f));
		foreach (MapNode mapNode in this.mapManager.GetNextStepMapNodes(this.previousMapNode))
		{
			if (mapNode == clickedNode)
			{
				mapNode.mapNodeData.State = eMapNodeState.JUST_FINISHED;
			}
			else
			{
				mapNode.mapNodeData.State = eMapNodeState.NOT_SELECTED;
			}
		}
		GameDataManager.instance.SaveData();
		switch (clickedNode.mapNodeData.MapNodeType)
		{
		case eStageType.NONE:
		case eStageType.START:
		case eStageType.DIAMOND:
		case eStageType.RECOVER:
		case eStageType.PORTAL:
			break;
		case eStageType.BATTLE:
		case eStageType.BATTLE_CORRUPTED:
			base.StartCoroutine(this.CR_BattleNodeProc());
			return;
		case eStageType.SHOP:
			base.StartCoroutine(this.CR_ShopNodeProc());
			return;
		case eStageType.SPECIAL_EVENT:
			base.StartCoroutine(this.CR_EventNodeProc());
			return;
		case eStageType.BOSS:
			base.StartCoroutine(this.CR_BossNodeProc());
			return;
		case eStageType.TREASURE:
			base.StartCoroutine(this.CR_TreasureNodeProc());
			return;
		case eStageType.WORKSHOP:
			base.StartCoroutine(this.CR_WorkshopNodeProc());
			return;
		case eStageType.ACADEMY:
			base.StartCoroutine(this.CR_AcademyNodeProc());
			break;
		default:
			return;
		}
	}

	// Token: 0x0600039D RID: 925 RVA: 0x0000E810 File Offset: 0x0000CA10
	private void Update()
	{
	}

	// Token: 0x0600039E RID: 926 RVA: 0x0000E812 File Offset: 0x0000CA12
	private IEnumerator CR_DebugResetMapProc()
	{
		MapGenerateSetting setting = new MapGenerateSetting(-1);
		MapData mapData = this.mapManager.GenerateMapData(setting);
		this.mapManager.ClearMap();
		this.mapManager.VisualizeMap(setting, mapData);
		this.previousMapNode = null;
		this.currentMapNode = this.mapManager.GetMapNode(0);
		yield return base.StartCoroutine(this.CR_ShowMapNodes(false));
		yield return base.StartCoroutine(this.CR_UpdateMapNodeState());
		yield break;
	}

	// Token: 0x0600039F RID: 927 RVA: 0x0000E821 File Offset: 0x0000CA21
	private IEnumerator CR_ShowMapNodes(bool isImmediate)
	{
		List<MapNode> list_MapNodes = this.mapManager.GetMapNodes();
		if (isImmediate)
		{
			using (List<MapNode>.Enumerator enumerator = list_MapNodes.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					MapNode mapNode = enumerator.Current;
					mapNode.ToggleImmediate(true);
					mapNode.SetPathShowPercentage(1f);
				}
				yield break;
			}
		}
		int num;
		for (int i = 0; i < list_MapNodes.Count; i = num + 1)
		{
			MapNode mapNode2 = list_MapNodes[i];
			mapNode2.Toggle(true, Random.Range(0.6f, 0.8f) + 0.066f * (float)i);
			mapNode2.SetPathShowPercentage(0f);
			base.StartCoroutine(this.CR_ShowNodePath(mapNode2, 0.33f));
			yield return new WaitForSeconds(0.085f);
			num = i;
		}
		yield break;
	}

	// Token: 0x060003A0 RID: 928 RVA: 0x0000E837 File Offset: 0x0000CA37
	private IEnumerator CR_UpdateMapNodeState()
	{
		List<MapNode> mapNodes = this.mapManager.GetMapNodes();
		for (int i = 0; i < mapNodes.Count; i++)
		{
			MapNode mapNode = mapNodes[i];
			if (mapNode.IsState(eMapNodeState.JUST_FINISHED))
			{
				mapNode.SetCompleted(true, false);
			}
			else if (mapNode.IsState(eMapNodeState.COMPLETED))
			{
				mapNode.SetCompleted(true, true);
			}
		}
		if (this.currentMapNode.mapNodeData.IsCleared)
		{
			foreach (MapNode mapNode2 in this.mapManager.GetNextStepMapNodes(this.currentMapNode))
			{
				mapNode2.SwitchState(eMapNodeState.AVALIABLE);
			}
			this.currentMapNode.SwitchState(eMapNodeState.JUST_FINISHED);
		}
		else
		{
			this.currentMapNode.SwitchState(eMapNodeState.AVALIABLE);
		}
		GameDataManager.instance.SaveData();
		yield return null;
		yield break;
	}

	// Token: 0x060003A1 RID: 929 RVA: 0x0000E846 File Offset: 0x0000CA46
	private IEnumerator CR_ShowNodePath(MapNode node, float duration)
	{
		for (float time = 0f; time <= duration; time += Time.deltaTime)
		{
			float pathShowPercentage = time / duration;
			node.SetPathShowPercentage(pathShowPercentage);
			yield return null;
		}
		node.SetPathShowPercentage(1f);
		yield break;
	}

	// Token: 0x060003A2 RID: 930 RVA: 0x0000E85C File Offset: 0x0000CA5C
	private IEnumerator CR_ShowFullMap()
	{
		yield return null;
		yield break;
	}

	// Token: 0x060003A3 RID: 931 RVA: 0x0000E864 File Offset: 0x0000CA64
	private IEnumerator CR_ShowNextStep()
	{
		yield return null;
		yield break;
	}

	// Token: 0x060003A4 RID: 932 RVA: 0x0000E86C File Offset: 0x0000CA6C
	private IEnumerator CR_EventNodeProc()
	{
		DebugManager.Log(eDebugKey.MAP_SCENE, "開始流程: 事件格", null);
		this.isInNodeProcess = true;
		yield return new WaitForSeconds(1f);
		EventMgr.SendEvent<EventStageData>(eMapSceneEvents.StartEventBlockProcess, null);
		SoundManager.PlayMusic("MapScene", "BGM_Event", true, 1f, 0f, 1f);
		SingleEventCapturer sc = new SingleEventCapturer(eMapSceneEvents.MapBlockCompleted, null);
		while (!sc.IsEventReceived)
		{
			yield return null;
		}
		SoundManager.PlayMusic("Main", "BGM_MapPage", true, 1f, 0f, 1f);
		this.isInNodeProcess = false;
		DebugManager.Log(eDebugKey.MAP_SCENE, "結束流程: 事件格", null);
		yield break;
	}

	// Token: 0x060003A5 RID: 933 RVA: 0x0000E87B File Offset: 0x0000CA7B
	private IEnumerator CR_BattleNodeProc()
	{
		DebugManager.Log(eDebugKey.MAP_SCENE, "開始流程: 關卡格", null);
		this.isInNodeProcess = true;
		EventMgr.SendEvent<EventStageData>(eMapSceneEvents.StartBattleBlockProcess, null);
		EventMgr.SendEvent(eGameEvents.UI_TriggerTransition_Show);
		yield return new WaitForSeconds(1f);
		if (this.currentMapNode.mapNodeData.stageEnvSceneName == null)
		{
			EnvSceneCollectionData envSceneCollectionData = Resources.Load<EnvSceneCollectionData>("EnvSceneCollectionData");
			if (this.currentMapNode.mapNodeData.Step == 2 && !GameDataManager.instance.Playerdata.IsTutorialStageFinished)
			{
				this.currentMapNode.mapNodeData.SetEnvSceneName("EnvScene_012", 1);
			}
			else
			{
				EnvSceneCollectionData.EnvSceneDataEntry randomScene = envSceneCollectionData.GetRandomScene(GameDataManager.instance.GameplayData.CurWorld, false, this.currentMapNode.mapNodeData.difficulty);
				this.currentMapNode.mapNodeData.SetEnvSceneName(randomScene.name, this.currentMapNode.mapNodeData.difficulty);
			}
		}
		IntermediateData intermediateData = new IntermediateData
		{
			worldType = GameDataManager.instance.GameplayData.CurWorld,
			missionType = eMissionType.STAY_ALIVE_ROUND,
			difficulty = this.currentMapNode.mapNodeData.difficulty,
			isCorrupted = (this.currentMapNode.mapNodeData.MapNodeType == eStageType.BATTLE_CORRUPTED),
			stageName = this.currentMapNode.mapNodeData.stageEnvSceneName,
			mapNodeData = this.currentMapNode.mapNodeData
		};
		GameDataManager.instance.SetIntermediateData(intermediateData);
		SceneManager.LoadScene("MainGame");
		yield break;
	}

	// Token: 0x060003A6 RID: 934 RVA: 0x0000E88A File Offset: 0x0000CA8A
	private IEnumerator CR_BossNodeProc()
	{
		DebugManager.Log(eDebugKey.MAP_SCENE, "開始流程: 關卡格", null);
		this.isInNodeProcess = true;
		EventMgr.SendEvent<EventStageData>(eMapSceneEvents.StartBattleBlockProcess, null);
		EventMgr.SendEvent(eGameEvents.UI_TriggerTransition_Show);
		yield return new WaitForSeconds(1f);
		IntermediateData intermediateData = new IntermediateData
		{
			worldType = GameDataManager.instance.GameplayData.CurWorld,
			missionType = eMissionType.BOSS,
			difficulty = 10,
			isCorrupted = true,
			stageName = this.currentMapNode.mapNodeData.stageEnvSceneName,
			mapNodeData = this.currentMapNode.mapNodeData
		};
		GameDataManager.instance.SetIntermediateData(intermediateData);
		SceneManager.LoadScene("MainGame");
		yield break;
	}

	// Token: 0x060003A7 RID: 935 RVA: 0x0000E899 File Offset: 0x0000CA99
	private IEnumerator CR_ShopNodeProc()
	{
		DebugManager.Log(eDebugKey.MAP_SCENE, "開始流程: 商店格", null);
		this.isInNodeProcess = true;
		yield return new WaitForSeconds(0.33f);
		List<TowerSettingData> allTowerSettingData = Singleton<ResourceManager>.Instance.GetAllTowerSettingData();
		Singleton<ResourceManager>.Instance.GetAllPanelSettingData();
		GameplayData gamePlayData = GameDataManager.instance.GameplayData;
		List<PanelSettingData> list = new List<PanelSettingData>
		{
			Singleton<ResourceManager>.Instance.GetRandomPanelSettingData(),
			Singleton<ResourceManager>.Instance.GetRandomPanelSettingData(),
			Singleton<ResourceManager>.Instance.GetRandomPanelSettingData()
		};
		List<TowerSettingData> list2 = allTowerSettingData.FindAll((TowerSettingData a) => !gamePlayData.IsHaveTowerInCollected(a.GetItemType()));
		list2 = list2.Take(2).ToList<TowerSettingData>();
		List<ABaseBuffSettingData> list3 = new List<ABaseBuffSettingData>
		{
			Singleton<ResourceManager>.Instance.GetRandomBuffSettingDataForStore(),
			Singleton<ResourceManager>.Instance.GetRandomBuffSettingDataForStore()
		};
		List<CardData> list4 = new List<CardData>();
		for (int i = 0; i < list.Count; i++)
		{
			list4.Add(new CardData(list[i], false));
		}
		for (int j = 0; j < list2.Count; j++)
		{
			list4.Add(new CardData(list2[j], false));
		}
		for (int k = 0; k < list3.Count; k++)
		{
			list4.Add(new CardData(list3[k], false));
		}
		UI_MapScene_Shop_Popup window = APopupWindow.CreateWindow<UI_MapScene_Shop_Popup>(APopupWindow.ePopupWindowLayer.TOP, Singleton<UIManager>.Instance.PopupUIAnchor_TopLevel, false);
		window.SetupContent(list4);
		yield return new WaitForSeconds(1f);
		while (!window.IsWindowFinished)
		{
			yield return null;
		}
		this.isInNodeProcess = false;
		DebugManager.Log(eDebugKey.MAP_SCENE, "結束流程: 商店格", null);
		yield break;
	}

	// Token: 0x060003A8 RID: 936 RVA: 0x0000E8A8 File Offset: 0x0000CAA8
	private IEnumerator CR_TreasureNodeProc()
	{
		DebugManager.Log(eDebugKey.MAP_SCENE, "開始流程: 寶藏格", null);
		this.isInNodeProcess = true;
		List<DiscoverRewardData> weightedRandomReward = this.discoverData_WorkShop.GetWeightedRandomReward(5, 1f, true);
		EventMgr.SendEvent<List<DiscoverRewardData>>(eMapSceneEvents.TriggerWorkshopUI, weightedRandomReward);
		SingleEventCapturer sc = new SingleEventCapturer(eMapSceneEvents.WorkshopUICompleted, null);
		while (!sc.IsEventReceived)
		{
			yield return null;
		}
		yield return null;
		this.isInNodeProcess = false;
		DebugManager.Log(eDebugKey.MAP_SCENE, "結束流程: 寶藏格", null);
		yield break;
	}

	// Token: 0x060003A9 RID: 937 RVA: 0x0000E8B7 File Offset: 0x0000CAB7
	private IEnumerator CR_WorkshopNodeProc()
	{
		DebugManager.Log(eDebugKey.MAP_SCENE, "開始流程: 工坊格", null);
		this.isInNodeProcess = true;
		yield return new WaitForSeconds(0.33f);
		List<ABaseBuffSettingData> list = new List<ABaseBuffSettingData>();
		int num = 0;
		while (list.Count < 3 && num < 50)
		{
			ABaseBuffSettingData randomBuffSettingDataForStore = Singleton<ResourceManager>.Instance.GetRandomBuffSettingDataForStore();
			if (!list.Contains(randomBuffSettingDataForStore))
			{
				list.Add(randomBuffSettingDataForStore);
			}
			else
			{
				num++;
			}
		}
		List<eItemType> list2 = new List<eItemType>();
		for (int i = 0; i < list.Count; i++)
		{
			list2.Add(list[i].GetItemType());
		}
		List<DiscoverRewardData> list3 = new List<DiscoverRewardData>();
		foreach (eItemType item in list2)
		{
			list3.Add(new DiscoverRewardData(eDiscoverRewardType.BUFF_CARD, new List<eItemType>
			{
				item
			}, 1));
		}
		UI_MapScene_Workshop_Popup window = APopupWindow.CreateWindow<UI_MapScene_Workshop_Popup>(APopupWindow.ePopupWindowLayer.TOP, Singleton<UIManager>.Instance.PopupUIAnchor_TopLevel, false);
		window.SetupContent(list3);
		while (!window.IsWindowFinished)
		{
			yield return null;
		}
		this.isInNodeProcess = false;
		DebugManager.Log(eDebugKey.MAP_SCENE, "結束流程: 工坊格", null);
		yield break;
	}

	// Token: 0x060003AA RID: 938 RVA: 0x0000E8C6 File Offset: 0x0000CAC6
	private IEnumerator CR_AcademyNodeProc()
	{
		DebugManager.Log(eDebugKey.MAP_SCENE, "開始流程: 學院格", null);
		this.isInNodeProcess = true;
		bool isTutorialFinished = GameDataManager.instance.Playerdata.IsTutorialStageFinished;
		yield return new WaitForSeconds(0.33f);
		List<TowerSettingData> list = Singleton<ResourceManager>.Instance.GetRandomTowerWithTier(eTowerTier.TIER_1, 2);
		List<TowerSettingData> list2 = Singleton<ResourceManager>.Instance.GetRandomTowerWithTier(eTowerTier.TIER_1, 2);
		List<TowerSettingData> list3 = Singleton<ResourceManager>.Instance.GetRandomTowerWithTier(eTowerTier.TIER_1, 2);
		TowerSettingData item = Singleton<ResourceManager>.Instance.GetRandomTowerWithTier(eTowerTier.TIER_2, 1).First<TowerSettingData>();
		TowerSettingData item2 = Singleton<ResourceManager>.Instance.GetRandomTowerWithTier(eTowerTier.TIER_2, 1).First<TowerSettingData>();
		TowerSettingData item3 = Singleton<ResourceManager>.Instance.GetRandomTowerWithTier(eTowerTier.TIER_2, 1).First<TowerSettingData>();
		if (!isTutorialFinished)
		{
			list.Clear();
			list.Add(Singleton<ResourceManager>.Instance.GetTowerDataByType(eItemType._1000_BASIC_TOWER));
			list.Add(Singleton<ResourceManager>.Instance.GetTowerDataByType(eItemType._1004_LIGHTNING_TOWER));
			item = Singleton<ResourceManager>.Instance.GetTowerDataByType(eItemType._1006_FREEZE_TOWER);
			list2.Clear();
			list2.Add(Singleton<ResourceManager>.Instance.GetTowerDataByType(eItemType._1007_DART_TOWER));
			list2.Add(Singleton<ResourceManager>.Instance.GetTowerDataByType(eItemType._1002_CANNON_TOWER));
			item2 = Singleton<ResourceManager>.Instance.GetTowerDataByType(eItemType._1003_FLAMETHROWER_TOWER);
			list3.Clear();
			list3.Add(Singleton<ResourceManager>.Instance.GetTowerDataByType(eItemType._1001_SCRAP_TOWER));
			list3.Add(Singleton<ResourceManager>.Instance.GetTowerDataByType(eItemType._1011_DRONE_TOWER));
			item3 = Singleton<ResourceManager>.Instance.GetTowerDataByType(eItemType._1004_LIGHTNING_TOWER);
		}
		list.Add(item);
		list2.Add(item2);
		list3.Add(item3);
		list = (from a in list
		orderby a.GetItemType()
		select a).ToList<TowerSettingData>();
		list2 = (from a in list2
		orderby a.GetItemType()
		select a).ToList<TowerSettingData>();
		list3 = (from a in list3
		orderby a.GetItemType()
		select a).ToList<TowerSettingData>();
		List<eItemType> list_TetrisTypes = this.tetrisStarterSetData.GetWeightedRandomStarterSet(isTutorialFinished).list_TetrisTypes;
		List<eItemType> list_TetrisTypes2 = this.tetrisStarterSetData.GetWeightedRandomStarterSet(isTutorialFinished).list_TetrisTypes;
		List<eItemType> list_TetrisTypes3 = this.tetrisStarterSetData.GetWeightedRandomStarterSet(isTutorialFinished).list_TetrisTypes;
		for (int i = 0; i < list_TetrisTypes.Count; i++)
		{
			if (list_TetrisTypes[i] == eItemType.NONE)
			{
				list_TetrisTypes[i] = Singleton<ResourceManager>.Instance.GetRandomPanelType(1, false).First<eItemType>();
			}
			if (list_TetrisTypes2[i] == eItemType.NONE)
			{
				list_TetrisTypes2[i] = Singleton<ResourceManager>.Instance.GetRandomPanelType(1, false).First<eItemType>();
			}
			if (list_TetrisTypes3[i] == eItemType.NONE)
			{
				list_TetrisTypes3[i] = Singleton<ResourceManager>.Instance.GetRandomPanelType(1, false).First<eItemType>();
			}
		}
		List<DiscoverRewardData> list4 = new List<DiscoverRewardData>();
		List<DiscoverRewardData> list5 = new List<DiscoverRewardData>();
		List<DiscoverRewardData> list6 = new List<DiscoverRewardData>();
		for (int j = 0; j < 3; j++)
		{
			List<eItemType> itemTypes = new List<eItemType>
			{
				list[j].GetItemType()
			};
			list4.Add(new DiscoverRewardData(eDiscoverRewardType.TOWER_CARD, itemTypes, 1));
		}
		for (int k = 0; k < list_TetrisTypes.Count; k++)
		{
			List<eItemType> itemTypes2 = new List<eItemType>
			{
				list_TetrisTypes[k]
			};
			list4.Add(new DiscoverRewardData(eDiscoverRewardType.PANEL_CARD, itemTypes2, 1));
		}
		for (int l = 0; l < 3; l++)
		{
			List<eItemType> itemTypes3 = new List<eItemType>
			{
				list2[l].GetItemType()
			};
			list5.Add(new DiscoverRewardData(eDiscoverRewardType.TOWER_CARD, itemTypes3, 1));
		}
		for (int m = 0; m < list_TetrisTypes2.Count; m++)
		{
			List<eItemType> itemTypes4 = new List<eItemType>
			{
				list_TetrisTypes2[m]
			};
			list5.Add(new DiscoverRewardData(eDiscoverRewardType.PANEL_CARD, itemTypes4, 1));
		}
		for (int n = 0; n < 3; n++)
		{
			List<eItemType> itemTypes5 = new List<eItemType>
			{
				list3[n].GetItemType()
			};
			list6.Add(new DiscoverRewardData(eDiscoverRewardType.TOWER_CARD, itemTypes5, 1));
		}
		for (int num = 0; num < list_TetrisTypes3.Count; num++)
		{
			List<eItemType> itemTypes6 = new List<eItemType>
			{
				list_TetrisTypes3[num]
			};
			list6.Add(new DiscoverRewardData(eDiscoverRewardType.PANEL_CARD, itemTypes6, 1));
		}
		UI_MapScene_Academy_Popup window = APopupWindow.CreateWindow<UI_MapScene_Academy_Popup>(APopupWindow.ePopupWindowLayer.TOP, Singleton<UIManager>.Instance.PopupUIAnchor_TopLevel, false);
		window.SetupContent(list4, list5, list6);
		while (!window.IsWindowFinished)
		{
			yield return null;
		}
		this.isInNodeProcess = false;
		DebugManager.Log(eDebugKey.MAP_SCENE, "結束流程: 學院格", null);
		yield break;
	}

	// Token: 0x040003BE RID: 958
	[SerializeField]
	private Map mapManager;

	// Token: 0x040003BF RID: 959
	private MapNode previousMapNode;

	// Token: 0x040003C0 RID: 960
	private MapNode currentMapNode;

	// Token: 0x040003C1 RID: 961
	[SerializeField]
	private DiscoverRewardAssetData discoverData_WorkShop;

	// Token: 0x040003C2 RID: 962
	[SerializeField]
	private TetrisStarterSetData tetrisStarterSetData;

	// Token: 0x040003C3 RID: 963
	[SerializeField]
	private MoveUIByMouse mapMover;

	// Token: 0x040003C4 RID: 964
	[SerializeField]
	private CenterTargetInScrollView scrollViewControl;

	// Token: 0x040003C5 RID: 965
	[SerializeField]
	private Scroller scroller;

	// Token: 0x040003C6 RID: 966
	private bool isInNodeProcess;

	// Token: 0x040003C7 RID: 967
	private bool isSelectedNode;

	// Token: 0x040003C8 RID: 968
	private Vector3 mapResetOrigin = new Vector3(-500f, 25f, 0f);
}
