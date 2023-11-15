using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// Token: 0x02000023 RID: 35
[CreateAssetMenu(fileName = "Data", menuName = "設定檔/DiscoverRewardAssetData", order = 1)]
public class DiscoverRewardAssetData : ScriptableObject
{
	// Token: 0x060000A9 RID: 169 RVA: 0x000040A4 File Offset: 0x000022A4
	public virtual List<DiscoverRewardData> GetWeightedRandomReward(int count, float multiplier, bool preventSameType)
	{
		List<DiscoverRewardEntry> list = this.rewardEntries.ToList<DiscoverRewardEntry>();
		List<DiscoverRewardEntry> list2 = new List<DiscoverRewardEntry>();
		if (preventSameType && count >= this.rewardEntries.Count)
		{
			count = this.rewardEntries.Count;
			Debug.LogError(string.Format("要求的獎勵種類超過可以給的種類!!, 縮減數量為 {0} 個.", count));
		}
		for (int i = 0; i < count; i++)
		{
			int num = 0;
			foreach (DiscoverRewardEntry discoverRewardEntry in list)
			{
				num += discoverRewardEntry.weight;
			}
			int num2 = Random.Range(0, num);
			int num3 = 0;
			using (List<DiscoverRewardEntry>.Enumerator enumerator = list.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					DiscoverRewardEntry entry = enumerator.Current;
					num3 += entry.weight;
					bool flag = list2.Exists((DiscoverRewardEntry r) => r.rewardType == entry.rewardType);
					if ((!preventSameType || !flag) && num2 < num3)
					{
						DiscoverRewardEntry discoverRewardEntry2 = new DiscoverRewardEntry
						{
							rewardType = entry.rewardType,
							minQuantityMultiplier = entry.minQuantityMultiplier,
							maxQuantityMultiplier = entry.maxQuantityMultiplier,
							weight = entry.weight
						};
						int num4 = Mathf.CeilToInt((float)Random.Range(discoverRewardEntry2.minQuantityMultiplier, discoverRewardEntry2.maxQuantityMultiplier + 1) * multiplier);
						discoverRewardEntry2.quantity = entry.quantityPerServe * num4;
						list2.Add(discoverRewardEntry2);
						if (preventSameType)
						{
							list.Remove(entry);
							break;
						}
						break;
					}
				}
			}
		}
		return this.DiscoverRewardEntryToRewardData(list2);
	}

	// Token: 0x060000AA RID: 170 RVA: 0x00004284 File Offset: 0x00002484
	private List<DiscoverRewardData> DiscoverRewardEntryToRewardData(List<DiscoverRewardEntry> list_DiscoverRewardEntry)
	{
		List<DiscoverRewardData> list = new List<DiscoverRewardData>();
		foreach (DiscoverRewardEntry discoverRewardEntry in list_DiscoverRewardEntry)
		{
			List<eItemType> itemTypes = new List<eItemType>();
			switch (discoverRewardEntry.rewardType)
			{
			case eDiscoverRewardType.HP:
			case eDiscoverRewardType.COIN:
				break;
			case eDiscoverRewardType.TOWER_CARD:
				itemTypes = Singleton<ResourceManager>.Instance.GetRandomTowerType(1, false);
				break;
			case eDiscoverRewardType.PANEL_CARD:
				itemTypes = Singleton<ResourceManager>.Instance.GetRandomPanelType(1, false);
				break;
			case eDiscoverRewardType.RANDOM_PANEL_CARD:
				itemTypes = Singleton<ResourceManager>.Instance.GetRandomPanelType(discoverRewardEntry.quantity, true);
				break;
			case eDiscoverRewardType.BUFF_CARD:
				itemTypes = Singleton<ResourceManager>.Instance.GetRandomBuffType(1, false);
				break;
			case eDiscoverRewardType.RANDOM_BUFF_CARD:
				itemTypes = Singleton<ResourceManager>.Instance.GetRandomBuffType(discoverRewardEntry.quantity, true);
				break;
			default:
				Debug.LogError("獎勵資料有未設定的類型!?");
				break;
			}
			list.Add(new DiscoverRewardData(discoverRewardEntry.rewardType, itemTypes, discoverRewardEntry.quantity));
		}
		return list;
	}

	// Token: 0x0400007D RID: 125
	[SerializeField]
	protected List<DiscoverRewardEntry> rewardEntries;
}
