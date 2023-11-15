using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000012 RID: 18
[Serializable]
public abstract class AItemSettingData : ScriptableObject, ICardDataSource
{
	// Token: 0x17000003 RID: 3
	// (get) Token: 0x0600003B RID: 59 RVA: 0x0000270E File Offset: 0x0000090E
	public List<TowerStats> List_Stats
	{
		get
		{
			return this.list_Stats;
		}
	}

	// Token: 0x0600003C RID: 60 RVA: 0x00002716 File Offset: 0x00000916
	public int GetBuildCost(float multiplier = 1f)
	{
		return (int)((float)this.baseCost * multiplier);
	}

	// Token: 0x0600003D RID: 61 RVA: 0x00002722 File Offset: 0x00000922
	public int GetStoreCost()
	{
		return this.storeCost;
	}

	// Token: 0x0600003E RID: 62 RVA: 0x0000272A File Offset: 0x0000092A
	public bool IsInGame()
	{
		return this.isInGame;
	}

	// Token: 0x0600003F RID: 63 RVA: 0x00002732 File Offset: 0x00000932
	public bool IsPurchaseable()
	{
		return this.canPurchaseInStore;
	}

	// Token: 0x06000040 RID: 64 RVA: 0x0000273A File Offset: 0x0000093A
	public eItemType GetItemType()
	{
		return this.itemType;
	}

	// Token: 0x06000041 RID: 65 RVA: 0x00002742 File Offset: 0x00000942
	public Sprite GetCardIcon()
	{
		return this.sprite_Icon;
	}

	// Token: 0x06000042 RID: 66 RVA: 0x0000274A File Offset: 0x0000094A
	public virtual string GetLocNameString(bool isPrefix = true)
	{
		return "";
	}

	// Token: 0x06000043 RID: 67 RVA: 0x00002751 File Offset: 0x00000951
	public virtual string GetLocFlavorTextString()
	{
		return "";
	}

	// Token: 0x06000044 RID: 68 RVA: 0x00002758 File Offset: 0x00000958
	public virtual string GetLocStatsString()
	{
		return "";
	}

	// Token: 0x06000045 RID: 69 RVA: 0x0000275F File Offset: 0x0000095F
	public AItemSettingData GetScriptableObjectData()
	{
		return this;
	}

	// Token: 0x06000046 RID: 70 RVA: 0x00002764 File Offset: 0x00000964
	public TowerStats GetTowerStats(eStatType type)
	{
		foreach (TowerStats towerStats in this.list_Stats)
		{
			if (type == towerStats.StatType)
			{
				return towerStats;
			}
		}
		return null;
	}

	// Token: 0x06000047 RID: 71 RVA: 0x000027C0 File Offset: 0x000009C0
	public void CombineMultiplier(AItemSettingData data)
	{
		int count = this.list_Stats.Count;
		foreach (TowerStats towerStats in data.List_Stats)
		{
			for (int i = 0; i < this.list_Stats.Count; i++)
			{
				if (towerStats.StatType == this.list_Stats[i].StatType)
				{
					using (List<StatModifier>.Enumerator enumerator2 = towerStats.list_Modifiers.GetEnumerator())
					{
						while (enumerator2.MoveNext())
						{
							StatModifier item = enumerator2.Current;
							this.list_Stats[i].list_Modifiers.Add(item);
						}
						break;
					}
				}
			}
		}
		Debug.Log(string.Format("修改前:{0}, 修改後:{1}", count, this.list_Stats.Count));
	}

	// Token: 0x06000048 RID: 72 RVA: 0x000028C8 File Offset: 0x00000AC8
	public void AddBuffMultiplier(TowerStats buffStat)
	{
		for (int i = 0; i < this.list_Stats.Count; i++)
		{
			if (buffStat.StatType == this.list_Stats[i].StatType)
			{
				using (List<StatModifier>.Enumerator enumerator = buffStat.list_Modifiers.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						StatModifier item = enumerator.Current;
						this.list_Stats[i].list_Modifiers.Add(item);
					}
					break;
				}
			}
		}
	}

	// Token: 0x06000049 RID: 73 RVA: 0x0000295C File Offset: 0x00000B5C
	public void RemoveBuffMultiplier(TowerStats buffStat)
	{
		for (int i = 0; i < this.list_Stats.Count; i++)
		{
			if (buffStat.StatType == this.list_Stats[i].StatType)
			{
				for (int j = 0; j < buffStat.list_Modifiers.Count; j++)
				{
					for (int k = buffStat.list_Modifiers.Count - 1; k >= 0; k--)
					{
						if (this.list_Stats[i].list_Modifiers[k].Equals(buffStat.list_Modifiers[j]))
						{
							this.list_Stats[i].list_Modifiers.RemoveAt(k);
							break;
						}
					}
				}
				return;
			}
		}
	}

	// Token: 0x04000036 RID: 54
	[SerializeField]
	[Header("卡片類型")]
	protected eItemType itemType;

	// Token: 0x04000037 RID: 55
	[SerializeField]
	[Header("卡片圖示")]
	protected Sprite sprite_Icon;

	// Token: 0x04000038 RID: 56
	[SerializeField]
	[Header("基本金額")]
	protected int baseCost = 10;

	// Token: 0x04000039 RID: 57
	[SerializeField]
	[Header("商店金額")]
	protected int storeCost = 10;

	// Token: 0x0400003A RID: 58
	[SerializeField]
	[Header("是否可在遊戲中使用")]
	protected bool isInGame;

	// Token: 0x0400003B RID: 59
	[SerializeField]
	[Header("是否可在商店中買到")]
	protected bool canPurchaseInStore = true;

	// Token: 0x0400003C RID: 60
	[SerializeField]
	[Header("屬性列表")]
	protected List<TowerStats> list_Stats;

	// Token: 0x0400003D RID: 61
	protected string loc_AttributeString = "";

	// Token: 0x0400003E RID: 62
	protected string loc_Name = "";

	// Token: 0x0400003F RID: 63
	protected string loc_FlavorText = "";
}
