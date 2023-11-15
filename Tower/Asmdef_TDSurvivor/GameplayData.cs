using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// Token: 0x0200005C RID: 92
[Serializable]
public class GameplayData
{
	// Token: 0x17000021 RID: 33
	// (get) Token: 0x06000203 RID: 515 RVA: 0x00008D64 File Offset: 0x00006F64
	public int Gem
	{
		get
		{
			return this.gem;
		}
	}

	// Token: 0x17000022 RID: 34
	// (get) Token: 0x06000204 RID: 516 RVA: 0x00008D6C File Offset: 0x00006F6C
	public int TowerCardLimit
	{
		get
		{
			return this.towerCardLimit;
		}
	}

	// Token: 0x17000023 RID: 35
	// (get) Token: 0x06000205 RID: 517 RVA: 0x00008D74 File Offset: 0x00006F74
	public int ItemCardLimit
	{
		get
		{
			return this.itemCardLimit;
		}
	}

	// Token: 0x17000024 RID: 36
	// (get) Token: 0x06000206 RID: 518 RVA: 0x00008D7C File Offset: 0x00006F7C
	public int DrawCardPerRound
	{
		get
		{
			return this.drawCardPerRound;
		}
	}

	// Token: 0x17000025 RID: 37
	// (get) Token: 0x06000207 RID: 519 RVA: 0x00008D84 File Offset: 0x00006F84
	public bool IsGameStarted
	{
		get
		{
			return this.isGameStarted;
		}
	}

	// Token: 0x17000026 RID: 38
	// (get) Token: 0x06000208 RID: 520 RVA: 0x00008D8C File Offset: 0x00006F8C
	public bool IsGameEnded
	{
		get
		{
			return this.isGameEnded;
		}
	}

	// Token: 0x17000027 RID: 39
	// (get) Token: 0x06000209 RID: 521 RVA: 0x00008D94 File Offset: 0x00006F94
	public eWorldType CurWorld
	{
		get
		{
			return this.curWorld;
		}
	}

	// Token: 0x17000028 RID: 40
	// (get) Token: 0x0600020A RID: 522 RVA: 0x00008D9C File Offset: 0x00006F9C
	public int CurMapNodeIndex
	{
		get
		{
			return this.curMapNodeIndex;
		}
	}

	// Token: 0x17000029 RID: 41
	// (get) Token: 0x0600020B RID: 523 RVA: 0x00008DA4 File Offset: 0x00006FA4
	public int MaxHP
	{
		get
		{
			return this.maxHP;
		}
	}

	// Token: 0x1700002A RID: 42
	// (get) Token: 0x0600020C RID: 524 RVA: 0x00008DAC File Offset: 0x00006FAC
	public int CurHP
	{
		get
		{
			return this.curHP;
		}
	}

	// Token: 0x0600020D RID: 525 RVA: 0x00008DB4 File Offset: 0x00006FB4
	public GameplayData(int seed)
	{
		this.randomSeed = seed;
		this.curMapNodeIndex = 0;
		this.mapGenerateSetting = new MapGenerateSetting(seed);
		this.mapData = null;
		this.towerCardLimit = 8;
		this.itemCardLimit = 10;
		this.drawCardPerRound = 3;
		this.MAX_DRAW_CARD_PER_ROUND = 6;
		this.MAX_TOWER_CARD_LIMIT = 8;
		this.maxHP = 10;
		if (GameDataManager.instance.Playerdata.IsTalentLearned(eTalentType.HP_INCREASE_LVL1))
		{
			this.maxHP += 10;
		}
		if (GameDataManager.instance.Playerdata.IsTalentLearned(eTalentType.HP_INCREASE_LVL2))
		{
			this.maxHP += 10;
		}
		if (GameDataManager.instance.Playerdata.IsTalentLearned(eTalentType.HP_INCREASE_LVL1) && GameDataManager.instance.Playerdata.IsTalentLearned(eTalentType.HP_INCREASE_LVL2))
		{
			this.maxHP += 5;
		}
		this.curHP = this.maxHP;
		this.gem = 0;
		this.isGameEnded = false;
		this.list_LoadoutTowerData = new List<TowerIngameData>();
		this.list_CollectedTowerData = new List<TowerIngameData>();
		this.list_ItemStorage = new List<CardData>();
		this.list_PlayedEnvScene = new List<string>();
		this.SetWorld(eWorldType.WORLD_1_FOREST);
		this.RegisterEvents();
	}

	// Token: 0x0600020E RID: 526 RVA: 0x00008EFC File Offset: 0x000070FC
	public void LoadDataProcess()
	{
		foreach (CardData cardData in this.list_ItemStorage)
		{
			cardData.data = Object.Instantiate<AItemSettingData>(Singleton<ResourceManager>.Instance.GetItemDataByType(cardData.ItemType));
		}
	}

	// Token: 0x0600020F RID: 527 RVA: 0x00008F64 File Offset: 0x00007164
	public void RegisterEvents()
	{
		if (this.isEventRegistered)
		{
			return;
		}
		Debug.Log("RegisterEvents from GameplayData");
		EventMgr.Register(eGameEvents.RequestClearAllTowerCard, new Action(this.OnRequestClearAllTowerCard));
		EventMgr.Register<TowerIngameData>(eGameEvents.RequestAddTowerCard, new Action<TowerIngameData>(this.OnRequestAddTowerCard));
		EventMgr.Register<eItemType>(eGameEvents.RequestRemoveTowerCard, new Action<eItemType>(this.OnRequestRemoveTowerCard));
		EventMgr.Register<eItemType, TowerIngameData>(eGameEvents.RequestReplaceTowerCard, new Action<eItemType, TowerIngameData>(this.OnRequestReplaceTowerCard));
		EventMgr.Register<eItemType, int>(eGameEvents.RequestLevelUpTowerCard, new Action<eItemType, int>(this.OnRequestLevelUpTowerCard));
		EventMgr.Register<List<TowerIngameData>>(eGameEvents.RequestOverrideTowerLoadout, new Action<List<TowerIngameData>>(this.OnRequestOverrideTowerLoadout));
		EventMgr.Register<int>(eGameEvents.RequestAddTowerCardLimit, new Action<int>(this.OnRequestAddTowerCardLimit));
		EventMgr.Register<int>(eGameEvents.RequestAddItemCardLimit, new Action<int>(this.OnRequestAddItemCardLimit));
		EventMgr.Register<eItemType>(eGameEvents.RequestAddCardToStorage, new Action<eItemType>(this.OnRequestAddCardToStorage));
		EventMgr.Register<CardData>(eGameEvents.RequestRemoveCardFromStorage, new Action<CardData>(this.OnRequestRemoveCardFromStorage));
		EventMgr.Register<int>(eGameEvents.RequestAddDrawCardCount, new Action<int>(this.OnRequestAddDrawCardCount));
		EventMgr.Register<int>(eGameEvents.RequestAddGem, new Action<int>(this.OnRequestAddGem));
		EventMgr.Register<int>(eGameEvents.RequestSetGem, new Action<int>(this.OnRequestSetGem));
		EventMgr.Register<int>(eGameEvents.RequestOverrideMapHP, new Action<int>(this.OnRequestOverrideMapHP));
		this.isEventRegistered = true;
		this.isInitialized = true;
	}

	// Token: 0x06000210 RID: 528 RVA: 0x000090E4 File Offset: 0x000072E4
	public void ClearEvents()
	{
		if (!this.isEventRegistered)
		{
			return;
		}
		EventMgr.Remove(eGameEvents.RequestClearAllTowerCard, new Action(this.OnRequestClearAllTowerCard));
		EventMgr.Remove<TowerIngameData>(eGameEvents.RequestAddTowerCard, new Action<TowerIngameData>(this.OnRequestAddTowerCard));
		EventMgr.Remove<eItemType>(eGameEvents.RequestRemoveTowerCard, new Action<eItemType>(this.OnRequestRemoveTowerCard));
		EventMgr.Remove<eItemType, TowerIngameData>(eGameEvents.RequestReplaceTowerCard, new Action<eItemType, TowerIngameData>(this.OnRequestReplaceTowerCard));
		EventMgr.Remove<eItemType, int>(eGameEvents.RequestLevelUpTowerCard, new Action<eItemType, int>(this.OnRequestLevelUpTowerCard));
		EventMgr.Remove<List<TowerIngameData>>(eGameEvents.RequestOverrideTowerLoadout, new Action<List<TowerIngameData>>(this.OnRequestOverrideTowerLoadout));
		EventMgr.Remove<int>(eGameEvents.RequestAddTowerCardLimit, new Action<int>(this.OnRequestAddTowerCardLimit));
		EventMgr.Remove<int>(eGameEvents.RequestAddItemCardLimit, new Action<int>(this.OnRequestAddItemCardLimit));
		EventMgr.Remove<eItemType>(eGameEvents.RequestAddCardToStorage, new Action<eItemType>(this.OnRequestAddCardToStorage));
		EventMgr.Remove<CardData>(eGameEvents.RequestRemoveCardFromStorage, new Action<CardData>(this.OnRequestRemoveCardFromStorage));
		EventMgr.Remove<int>(eGameEvents.RequestAddDrawCardCount, new Action<int>(this.OnRequestAddDrawCardCount));
		EventMgr.Remove<int>(eGameEvents.RequestAddGem, new Action<int>(this.OnRequestAddGem));
		EventMgr.Remove<int>(eGameEvents.RequestSetGem, new Action<int>(this.OnRequestSetGem));
		EventMgr.Remove<int>(eGameEvents.RequestOverrideMapHP, new Action<int>(this.OnRequestOverrideMapHP));
		this.isEventRegistered = false;
	}

	// Token: 0x06000211 RID: 529 RVA: 0x00009251 File Offset: 0x00007451
	private void OnRequestOverrideMapHP(int value)
	{
		this.curHP = value;
	}

	// Token: 0x06000212 RID: 530 RVA: 0x0000925A File Offset: 0x0000745A
	private void OnRequestAddDrawCardCount(int value)
	{
		this.drawCardPerRound += value;
	}

	// Token: 0x06000213 RID: 531 RVA: 0x0000926A File Offset: 0x0000746A
	private void OnRequestAddCardToStorage(eItemType type)
	{
		this.AddCardToStorage(type);
	}

	// Token: 0x06000214 RID: 532 RVA: 0x00009274 File Offset: 0x00007474
	private void AddCardToStorage(eItemType type)
	{
		CardData item = new CardData(Singleton<ResourceManager>.Instance.GetItemDataByType(type), true);
		this.list_ItemStorage.Add(item);
	}

	// Token: 0x06000215 RID: 533 RVA: 0x0000929F File Offset: 0x0000749F
	private void OnRequestRemoveCardFromStorage(CardData data)
	{
		throw new NotImplementedException();
	}

	// Token: 0x06000216 RID: 534 RVA: 0x000092A6 File Offset: 0x000074A6
	public void SetWorld(eWorldType world)
	{
		this.curWorld = world;
		EventMgr.SendEvent<eWorldType>(eGameEvents.OnWorldChange, this.curWorld);
	}

	// Token: 0x06000217 RID: 535 RVA: 0x000092C1 File Offset: 0x000074C1
	public void SetCurrentMapNodeIndex(int index)
	{
		this.curMapNodeIndex = index;
	}

	// Token: 0x06000218 RID: 536 RVA: 0x000092CA File Offset: 0x000074CA
	private void OnRequestAddGem(int value)
	{
		this.gem += value;
		EventMgr.SendEvent<int>(eGameEvents.OnGemChanged, this.gem);
	}

	// Token: 0x06000219 RID: 537 RVA: 0x000092ED File Offset: 0x000074ED
	private void OnRequestSetGem(int value)
	{
		this.gem = value;
		EventMgr.SendEvent<int>(eGameEvents.OnGemChanged, this.gem);
	}

	// Token: 0x0600021A RID: 538 RVA: 0x00009309 File Offset: 0x00007509
	private void OnRequestAddTowerCardLimit(int value)
	{
		this.towerCardLimit = Mathf.Clamp(this.towerCardLimit + value, 1, 8);
	}

	// Token: 0x0600021B RID: 539 RVA: 0x00009320 File Offset: 0x00007520
	private void OnRequestAddItemCardLimit(int value)
	{
		this.itemCardLimit = Mathf.Clamp(this.itemCardLimit + value, 1, 10);
	}

	// Token: 0x0600021C RID: 540 RVA: 0x00009338 File Offset: 0x00007538
	private void OnRequestClearAllTowerCard()
	{
		this.list_LoadoutTowerData.Clear();
		this.list_CollectedTowerData.Clear();
		for (int i = 0; i < this.towerCardLimit; i++)
		{
			EventMgr.SendEvent<List<TowerIngameData>, int>(eGameEvents.OnTowerCardChanged, this.list_LoadoutTowerData, i);
		}
	}

	// Token: 0x0600021D RID: 541 RVA: 0x00009380 File Offset: 0x00007580
	private void OnRequestOverrideTowerLoadout(List<TowerIngameData> list_newLoadout)
	{
		this.list_LoadoutTowerData = list_newLoadout;
		for (int i = 0; i < this.towerCardLimit; i++)
		{
			EventMgr.SendEvent<List<TowerIngameData>, int>(eGameEvents.OnTowerCardChanged, this.list_LoadoutTowerData, i);
		}
	}

	// Token: 0x0600021E RID: 542 RVA: 0x000093BC File Offset: 0x000075BC
	private void OnRequestAddTowerCard(TowerIngameData data)
	{
		if (this.list_CollectedTowerData.Any((TowerIngameData towerData) => towerData.ItemType == data.ItemType))
		{
			Debug.Log(string.Format("<color=#ffff00>要求增加卡片, 但已經有同種卡片了 ({0})", data.ItemType));
			return;
		}
		Debug.Log(string.Format("增加砲台卡片: {0}", data.ItemType));
		this.list_CollectedTowerData.Add(data);
		if (this.list_LoadoutTowerData.Count < this.MAX_TOWER_CARD_LIMIT)
		{
			this.list_LoadoutTowerData.Add(data);
			EventMgr.SendEvent<List<TowerIngameData>, int>(eGameEvents.OnTowerCardChanged, this.list_LoadoutTowerData, this.list_LoadoutTowerData.Count - 1);
		}
		else
		{
			Debug.Log(string.Format("list_LoadoutTowerData已有{0}張卡 (限制{1})", this.list_LoadoutTowerData.Count, this.MAX_TOWER_CARD_LIMIT));
		}
		EventMgr.SendEvent<eItemType>(eGameEvents.RequestRecordTowerBuilt, data.ItemType);
	}

	// Token: 0x0600021F RID: 543 RVA: 0x000094C8 File Offset: 0x000076C8
	private void OnRequestRemoveTowerCard(eItemType type)
	{
		if (!this.list_CollectedTowerData.Any((TowerIngameData towerData) => towerData.ItemType == type))
		{
			return;
		}
		this.list_CollectedTowerData.Remove(this.list_CollectedTowerData.Find((TowerIngameData a) => a.ItemType == type));
		if (this.list_LoadoutTowerData.Any((TowerIngameData towerData) => towerData.ItemType == type))
		{
			this.list_LoadoutTowerData.Remove(this.list_LoadoutTowerData.Find((TowerIngameData a) => a.ItemType == type));
			EventMgr.SendEvent<List<TowerIngameData>, int>(eGameEvents.OnTowerCardChanged, this.list_LoadoutTowerData, this.list_LoadoutTowerData.Count - 1);
		}
	}

	// Token: 0x06000220 RID: 544 RVA: 0x0000957B File Offset: 0x0000777B
	private void OnRequestReplaceTowerCard(eItemType fromType, TowerIngameData newData)
	{
		throw new NotImplementedException();
	}

	// Token: 0x06000221 RID: 545 RVA: 0x00009584 File Offset: 0x00007784
	private void OnRequestLevelUpTowerCard(eItemType type, int targetLevel)
	{
		TowerIngameData towerIngameData = this.list_LoadoutTowerData.Find((TowerIngameData a) => a.ItemType == type);
		int arg = this.list_LoadoutTowerData.FindIndex((TowerIngameData a) => a.ItemType == type);
		towerIngameData.Level = targetLevel;
		EventMgr.SendEvent<List<TowerIngameData>, int>(eGameEvents.OnTowerCardChanged, this.list_LoadoutTowerData, arg);
	}

	// Token: 0x06000222 RID: 546 RVA: 0x000095E8 File Offset: 0x000077E8
	public bool IsHaveTowerInCollected(eItemType itemType)
	{
		return this.list_CollectedTowerData.Exists((TowerIngameData a) => a.ItemType == itemType);
	}

	// Token: 0x06000223 RID: 547 RVA: 0x0000961C File Offset: 0x0000781C
	public bool IsHaveTowerInLoadout(eItemType itemType)
	{
		return this.list_LoadoutTowerData.Exists((TowerIngameData a) => a.ItemType == itemType);
	}

	// Token: 0x06000224 RID: 548 RVA: 0x00009650 File Offset: 0x00007850
	public bool IsHaveItemInStorage(eItemType itemType)
	{
		return this.list_ItemStorage.Exists((CardData a) => a.ItemType == itemType);
	}

	// Token: 0x06000225 RID: 549 RVA: 0x00009681 File Offset: 0x00007881
	public int GetLoadoutTowerCount()
	{
		return this.list_LoadoutTowerData.Count;
	}

	// Token: 0x06000226 RID: 550 RVA: 0x0000968E File Offset: 0x0000788E
	public List<TowerIngameData> GetLoadoutTowerList()
	{
		return this.list_LoadoutTowerData;
	}

	// Token: 0x06000227 RID: 551 RVA: 0x00009696 File Offset: 0x00007896
	public List<TowerIngameData> GetCollectedTowerList()
	{
		return this.list_CollectedTowerData;
	}

	// Token: 0x06000228 RID: 552 RVA: 0x0000969E File Offset: 0x0000789E
	public StageRewardData GetCurrentStageRewardData()
	{
		return this.mapData.list_MapNodeData[this.curMapNodeIndex].stageReward;
	}

	// Token: 0x06000229 RID: 553 RVA: 0x000096BB File Offset: 0x000078BB
	public bool IsGameInProgress()
	{
		return this.isGameStarted && !this.isGameEnded;
	}

	// Token: 0x0600022A RID: 554 RVA: 0x000096D0 File Offset: 0x000078D0
	public void SetGameStarted()
	{
		this.isGameStarted = true;
		GameDataManager.instance.SaveData();
	}

	// Token: 0x0600022B RID: 555 RVA: 0x000096E3 File Offset: 0x000078E3
	public void SetGameEnded()
	{
		this.isGameEnded = true;
		GameDataManager.instance.SaveData();
	}

	// Token: 0x0600022C RID: 556 RVA: 0x000096F8 File Offset: 0x000078F8
	public bool IsHaveTowerWithSize(eTowerSizeType towerSizeType)
	{
		for (int i = 0; i < this.list_ItemStorage.Count; i++)
		{
			CardData cardData = this.list_ItemStorage[i];
			if (cardData.CardType == eCardType.TOWER_CARD && Singleton<ResourceManager>.Instance.GetTowerDataByType(cardData.ItemType).TowerSizeType == towerSizeType)
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x0600022D RID: 557 RVA: 0x0000974C File Offset: 0x0000794C
	public void RecordPlayedScene(string name)
	{
		if (!this.list_PlayedEnvScene.Contains(name))
		{
			this.list_PlayedEnvScene.Add(name);
		}
		GameDataManager.instance.SaveData();
	}

	// Token: 0x0600022E RID: 558 RVA: 0x00009772 File Offset: 0x00007972
	public bool IsPlayedScene(string name)
	{
		return this.list_PlayedEnvScene.Contains(name);
	}

	// Token: 0x0400017F RID: 383
	[Header("是否已初始化")]
	public bool isInitialized;

	// Token: 0x04000180 RID: 384
	[Header("這場遊戲的seed")]
	public int randomSeed = -1;

	// Token: 0x04000181 RID: 385
	public List<TowerIngameData> list_LoadoutTowerData;

	// Token: 0x04000182 RID: 386
	public List<TowerIngameData> list_CollectedTowerData;

	// Token: 0x04000183 RID: 387
	public List<CardData> list_ItemStorage;

	// Token: 0x04000184 RID: 388
	[SerializeField]
	[Header("寶石")]
	private int gem;

	// Token: 0x04000185 RID: 389
	[SerializeField]
	[Header("砲塔數量限制")]
	private int towerCardLimit;

	// Token: 0x04000186 RID: 390
	[SerializeField]
	[Header("手牌數量限制")]
	private int itemCardLimit;

	// Token: 0x04000187 RID: 391
	[SerializeField]
	[Header("每回合開始抽卡數")]
	private int drawCardPerRound = 3;

	// Token: 0x04000188 RID: 392
	[SerializeField]
	[Header("是否已經開始")]
	private bool isGameStarted;

	// Token: 0x04000189 RID: 393
	[SerializeField]
	[Header("是否已經GameOver")]
	private bool isGameEnded;

	// Token: 0x0400018A RID: 394
	[SerializeField]
	[Header("目前在哪一大關")]
	private eWorldType curWorld;

	// Token: 0x0400018B RID: 395
	[Header("目前地圖節點")]
	[SerializeField]
	private int curMapNodeIndex;

	// Token: 0x0400018C RID: 396
	[Header("產生地圖的設定值")]
	public MapGenerateSetting mapGenerateSetting;

	// Token: 0x0400018D RID: 397
	[Header("目前地圖資料")]
	public MapData mapData;

	// Token: 0x0400018E RID: 398
	[Header("已經玩過的環境場景")]
	public List<string> list_PlayedEnvScene;

	// Token: 0x0400018F RID: 399
	[SerializeField]
	private int maxHP;

	// Token: 0x04000190 RID: 400
	[SerializeField]
	private int curHP;

	// Token: 0x04000191 RID: 401
	public int MAX_DRAW_CARD_PER_ROUND = 6;

	// Token: 0x04000192 RID: 402
	public int MAX_TOWER_CARD_LIMIT = 8;

	// Token: 0x04000193 RID: 403
	private bool isEventRegistered;
}
