using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// Token: 0x020000D5 RID: 213
public class ResourceManager : Singleton<ResourceManager>
{
	// Token: 0x06000504 RID: 1284 RVA: 0x00014434 File Offset: 0x00012634
	private new void Awake()
	{
		base.Awake();
		this.Initialize();
	}

	// Token: 0x06000505 RID: 1285 RVA: 0x00014444 File Offset: 0x00012644
	private void Initialize()
	{
		if (this.isInitialized)
		{
			return;
		}
		this.list_TowerSettingData = new List<TowerSettingData>();
		this.list_PanelSettingData = new List<PanelSettingData>();
		this.list_BuffSettingData = new List<ABaseBuffSettingData>();
		this.list_TowerItemTypes = new List<eItemType>();
		this.list_PanelItemTypes = new List<eItemType>();
		this.list_BuffItemTypes = new List<eItemType>();
		MonsterSettingData[] array = Resources.LoadAll<MonsterSettingData>("");
		this.list_MonsterSettingData = new List<MonsterSettingData>();
		foreach (MonsterSettingData item in array)
		{
			this.list_MonsterSettingData.Add(item);
		}
		this.dic_MonsterSettingData = new Dictionary<eMonsterType, MonsterSettingData>();
		foreach (MonsterSettingData monsterSettingData in this.list_MonsterSettingData)
		{
			this.dic_MonsterSettingData.Add(monsterSettingData.GetMonsterType(), monsterSettingData);
		}
		AItemSettingData[] array3 = Resources.LoadAll<AItemSettingData>("");
		this.list_ItemSettingData = new List<AItemSettingData>();
		foreach (AItemSettingData aitemSettingData in array3)
		{
			this.list_ItemSettingData.Add(aitemSettingData);
			if (aitemSettingData is TowerSettingData)
			{
				this.list_TowerSettingData.Add(aitemSettingData as TowerSettingData);
			}
			if (aitemSettingData is PanelSettingData)
			{
				this.list_PanelSettingData.Add(aitemSettingData as PanelSettingData);
			}
			if (aitemSettingData is ABaseBuffSettingData)
			{
				this.list_BuffSettingData.Add(aitemSettingData as ABaseBuffSettingData);
			}
		}
		this.dic_ItemSettingData = new Dictionary<eItemType, AItemSettingData>();
		foreach (AItemSettingData aitemSettingData2 in this.list_ItemSettingData)
		{
			this.dic_ItemSettingData.Add(aitemSettingData2.GetItemType(), aitemSettingData2);
		}
		foreach (TowerSettingData towerSettingData in this.list_TowerSettingData)
		{
			this.list_TowerItemTypes.Add(towerSettingData.GetItemType());
		}
		foreach (PanelSettingData panelSettingData in this.list_PanelSettingData)
		{
			this.list_PanelItemTypes.Add(panelSettingData.GetItemType());
		}
		foreach (ABaseBuffSettingData abaseBuffSettingData in this.list_BuffSettingData)
		{
			this.list_BuffItemTypes.Add(abaseBuffSettingData.GetItemType());
		}
		this.isInitialized = true;
	}

	// Token: 0x06000506 RID: 1286 RVA: 0x0001470C File Offset: 0x0001290C
	public MonsterSettingData GetMonsterDataByType(eMonsterType type)
	{
		if (this.dic_MonsterSettingData.ContainsKey(type))
		{
			return this.dic_MonsterSettingData[type];
		}
		Debug.LogError(string.Format("找不到指定類型({0})的怪物", type));
		return null;
	}

	// Token: 0x06000507 RID: 1287 RVA: 0x00014740 File Offset: 0x00012940
	public List<MonsterSettingData> GetMonsterDataByWorld(eWorldType type)
	{
		return (from a in this.list_MonsterSettingData
		where a.IsAvaliableInWorld(type) && a.GetMonsterSize() != eMonsterSize.BOSS
		select a).ToList<MonsterSettingData>();
	}

	// Token: 0x06000508 RID: 1288 RVA: 0x00014776 File Offset: 0x00012976
	public ABaseTower CreateTower(TowerSettingData data)
	{
		return Singleton<PrefabManager>.Instance.InstantiatePrefab(data.GetPrefab(), Vector3.zero, Quaternion.identity, null).GetComponent<ABaseTower>();
	}

	// Token: 0x06000509 RID: 1289 RVA: 0x00014798 File Offset: 0x00012998
	public List<eItemType> GetRandomItemType(int count, List<eItemType> availableItems, bool preventDuplicate)
	{
		List<eItemType> list = availableItems.ToList<eItemType>();
		List<eItemType> list2 = new List<eItemType>();
		Random random = new Random();
		for (int i = 0; i < count; i++)
		{
			int index = random.Next(list.Count);
			eItemType item = list[index];
			list2.Add(item);
			if (preventDuplicate)
			{
				list.RemoveAt(index);
				if (list.Count == 0)
				{
					list = availableItems.ToList<eItemType>();
				}
			}
		}
		return list2;
	}

	// Token: 0x0600050A RID: 1290 RVA: 0x000147FF File Offset: 0x000129FF
	public List<TowerSettingData> GetAllTowerSettingData()
	{
		return this.list_TowerSettingData.ToList<TowerSettingData>();
	}

	// Token: 0x0600050B RID: 1291 RVA: 0x0001480C File Offset: 0x00012A0C
	public List<eItemType> GetAllTowerItemType()
	{
		return this.list_TowerItemTypes.ToList<eItemType>();
	}

	// Token: 0x0600050C RID: 1292 RVA: 0x00014819 File Offset: 0x00012A19
	public TowerSettingData GetTowerDataByType(eItemType type)
	{
		if (this.dic_ItemSettingData.ContainsKey(type))
		{
			return this.dic_ItemSettingData[type] as TowerSettingData;
		}
		Debug.LogError(string.Format("找不到指定類型({0})的Cannon資料", type));
		return null;
	}

	// Token: 0x0600050D RID: 1293 RVA: 0x00014851 File Offset: 0x00012A51
	public TowerSettingData GetRandomTowerSettingData()
	{
		return this.list_TowerSettingData.RandomItem<TowerSettingData>();
	}

	// Token: 0x0600050E RID: 1294 RVA: 0x0001485E File Offset: 0x00012A5E
	public List<eItemType> GetRandomTowerType(int count, bool preventDuplicate)
	{
		return this.GetRandomItemType(count, this.list_TowerItemTypes, preventDuplicate);
	}

	// Token: 0x0600050F RID: 1295 RVA: 0x00014870 File Offset: 0x00012A70
	public List<TowerSettingData> GetRandomTowerWithTier(eTowerTier tier, int count)
	{
		List<TowerSettingData> list = new List<TowerSettingData>();
		List<TowerSettingData> list2 = this.list_TowerSettingData.FindAll((TowerSettingData a) => a.TowerTier == tier);
		for (int i = 0; i < count; i++)
		{
			TowerSettingData item = list2.RandomItem<TowerSettingData>();
			list.Add(item);
			list2.Remove(item);
		}
		return list;
	}

	// Token: 0x06000510 RID: 1296 RVA: 0x000148CE File Offset: 0x00012ACE
	public List<PanelSettingData> GetAllPanelSettingData()
	{
		return this.list_PanelSettingData.ToList<PanelSettingData>();
	}

	// Token: 0x06000511 RID: 1297 RVA: 0x000148DB File Offset: 0x00012ADB
	public PanelSettingData GetPanelDataByType(eItemType type)
	{
		if (this.dic_PanelSettingData.ContainsKey(type))
		{
			return this.dic_PanelSettingData[type];
		}
		Debug.LogError(string.Format("找不到指定類型({0})的Panel資料", type));
		return null;
	}

	// Token: 0x06000512 RID: 1298 RVA: 0x0001490E File Offset: 0x00012B0E
	public PanelSettingData GetRandomPanelSettingData()
	{
		return this.list_PanelSettingData.RandomItem<PanelSettingData>();
	}

	// Token: 0x06000513 RID: 1299 RVA: 0x0001491B File Offset: 0x00012B1B
	public List<eItemType> GetRandomPanelType(int count, bool preventDuplicate)
	{
		return this.GetRandomItemType(count, this.list_PanelItemTypes, preventDuplicate);
	}

	// Token: 0x06000514 RID: 1300 RVA: 0x0001492B File Offset: 0x00012B2B
	public ABaseBuffSettingData GetRandomBuffSettingData()
	{
		return this.list_BuffSettingData.RandomItem<ABaseBuffSettingData>();
	}

	// Token: 0x06000515 RID: 1301 RVA: 0x00014938 File Offset: 0x00012B38
	public ABaseBuffSettingData GetRandomBuffSettingDataForStore()
	{
		return this.list_BuffSettingData.FindAll((ABaseBuffSettingData a) => a.IsPurchaseable()).RandomItem<ABaseBuffSettingData>();
	}

	// Token: 0x06000516 RID: 1302 RVA: 0x00014969 File Offset: 0x00012B69
	public List<eItemType> GetRandomBuffType(int count, bool preventDuplicate)
	{
		return this.GetRandomItemType(count, this.list_BuffItemTypes, preventDuplicate);
	}

	// Token: 0x06000517 RID: 1303 RVA: 0x0001497C File Offset: 0x00012B7C
	public AItemSettingData GetItemDataByType(eItemType type)
	{
		if (!this.isInitialized)
		{
			this.Initialize();
		}
		AItemSettingData result;
		try
		{
			if (this.dic_ItemSettingData.ContainsKey(type))
			{
				result = this.dic_ItemSettingData[type];
			}
			else
			{
				Debug.LogError(string.Format("找不到指定類型({0})的Card資料", type));
				result = null;
			}
		}
		catch (Exception arg)
		{
			Debug.LogError(string.Format("GetItemDataByType 在取得{0}時發生錯誤: {1}", type, arg));
			result = null;
		}
		return result;
	}

	// Token: 0x040004C5 RID: 1221
	[SerializeField]
	[Header("怪物資料")]
	private List<MonsterSettingData> list_MonsterSettingData;

	// Token: 0x040004C6 RID: 1222
	private Dictionary<eMonsterType, MonsterSettingData> dic_MonsterSettingData;

	// Token: 0x040004C7 RID: 1223
	[SerializeField]
	[Header("道具資料")]
	private List<AItemSettingData> list_ItemSettingData;

	// Token: 0x040004C8 RID: 1224
	private Dictionary<eItemType, AItemSettingData> dic_ItemSettingData;

	// Token: 0x040004C9 RID: 1225
	private List<eItemType> list_TowerItemTypes;

	// Token: 0x040004CA RID: 1226
	private List<TowerSettingData> list_TowerSettingData;

	// Token: 0x040004CB RID: 1227
	private Dictionary<eItemType, TowerSettingData> dic_TowerSettingData;

	// Token: 0x040004CC RID: 1228
	private List<eItemType> list_PanelItemTypes;

	// Token: 0x040004CD RID: 1229
	private List<PanelSettingData> list_PanelSettingData;

	// Token: 0x040004CE RID: 1230
	private Dictionary<eItemType, PanelSettingData> dic_PanelSettingData;

	// Token: 0x040004CF RID: 1231
	private List<eItemType> list_BuffItemTypes;

	// Token: 0x040004D0 RID: 1232
	private List<ABaseBuffSettingData> list_BuffSettingData;

	// Token: 0x040004D1 RID: 1233
	private Dictionary<eItemType, ABaseBuffSettingData> dic_BuffSettingData;

	// Token: 0x040004D2 RID: 1234
	private bool isInitialized;
}
