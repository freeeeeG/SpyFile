using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000066 RID: 102
public class DiscoverRewardHandler : Singleton<DiscoverRewardHandler>
{
	// Token: 0x06000279 RID: 633 RVA: 0x0000A77F File Offset: 0x0000897F
	private void OnEnable()
	{
		EventMgr.Register(eGameEvents.RequestDiscoverReward, new Action(this.OnRequestDiscoverReward));
		EventMgr.Register<DiscoverRewardData, Vector3>(eGameEvents.OnDiscoverRewardSelected, new Action<DiscoverRewardData, Vector3>(this.OnDiscoverRewardSelected));
	}

	// Token: 0x0600027A RID: 634 RVA: 0x0000A7B1 File Offset: 0x000089B1
	private void OnDisable()
	{
		EventMgr.Remove(eGameEvents.RequestDiscoverReward, new Action(this.OnRequestDiscoverReward));
		EventMgr.Remove<DiscoverRewardData, Vector3>(eGameEvents.OnDiscoverRewardSelected, new Action<DiscoverRewardData, Vector3>(this.OnDiscoverRewardSelected));
	}

	// Token: 0x0600027B RID: 635 RVA: 0x0000A7E4 File Offset: 0x000089E4
	private void OnRequestDiscoverReward()
	{
		List<DiscoverRewardData> arg = this.GenerateReward();
		EventMgr.SendEvent<List<DiscoverRewardData>>(eGameEvents.UI_ShowDiscoverReward, arg);
		Singleton<CameraManager>.Instance.ShakeCamera(0.15f, 0.005f, 0f);
	}

	// Token: 0x0600027C RID: 636 RVA: 0x0000A820 File Offset: 0x00008A20
	private void OnDiscoverRewardSelected(DiscoverRewardData data, Vector3 position)
	{
		switch (data.DiscoverRewardType)
		{
		case eDiscoverRewardType.NONE:
			break;
		case eDiscoverRewardType.HP:
			EventMgr.SendEvent<int>(eGameEvents.RequestAddHP, data.value);
			return;
		case eDiscoverRewardType.COIN:
			EventMgr.SendEvent<int>(eGameEvents.RequestAddCoin, data.value);
			return;
		case eDiscoverRewardType.TOWER_CARD:
		{
			TowerIngameData arg = new TowerIngameData(data.List_RewardContentType[0], 1);
			EventMgr.SendEvent<TowerIngameData>(eGameEvents.RequestAddTowerCard, arg);
			return;
		}
		case eDiscoverRewardType.PANEL_CARD:
		case eDiscoverRewardType.BUFF_CARD:
			for (int i = 0; i < data.List_RewardContentType.Count; i++)
			{
				for (int j = 0; j < data.value; j++)
				{
					eItemType arg2 = data.List_RewardContentType[i];
					EventMgr.SendEvent<eItemType, Vector3>(eGameEvents.RequestAddCardToHandFromPosition, arg2, position);
				}
			}
			return;
		case eDiscoverRewardType.RANDOM_PANEL_CARD:
		case eDiscoverRewardType.RANDOM_BUFF_CARD:
		{
			int num = 0;
			while (num < data.List_RewardContentType.Count && num < data.value)
			{
				eItemType arg3 = data.List_RewardContentType[num];
				EventMgr.SendEvent<eItemType, Vector3>(eGameEvents.RequestAddCardToHandFromPosition, arg3, position);
				num++;
			}
			break;
		}
		default:
			return;
		}
	}

	// Token: 0x0600027D RID: 637 RVA: 0x0000A92E File Offset: 0x00008B2E
	private void Update()
	{
	}

	// Token: 0x0600027E RID: 638 RVA: 0x0000A930 File Offset: 0x00008B30
	public List<DiscoverRewardData> GenerateReward()
	{
		List<DiscoverRewardData> weightedRandomReward = this.settingData.GetWeightedRandomReward(Random.Range(3, 5), 1f, true);
		foreach (DiscoverRewardData discoverRewardData in weightedRandomReward)
		{
			if (discoverRewardData.DiscoverRewardType.IsItemCard())
			{
				int handCardSpace = MainGame.Instance.IngameData.GetHandCardSpace();
				if (handCardSpace != 0 && handCardSpace < discoverRewardData.value)
				{
					discoverRewardData.value = handCardSpace;
				}
			}
		}
		return weightedRandomReward;
	}

	// Token: 0x040001CF RID: 463
	[SerializeField]
	private DiscoverRewardAssetData settingData;
}
