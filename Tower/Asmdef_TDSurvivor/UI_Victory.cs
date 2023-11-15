using System;
using System.Collections;
using System.Collections.Generic;
using Febucci.UI;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Token: 0x0200018E RID: 398
public class UI_Victory : APopupWindow
{
	// Token: 0x06000AA6 RID: 2726 RVA: 0x00027FAF File Offset: 0x000261AF
	private void Awake()
	{
		this.button_Continue.enabled = false;
	}

	// Token: 0x06000AA7 RID: 2727 RVA: 0x00027FBD File Offset: 0x000261BD
	private void OnEnable()
	{
		this.button_Continue.onClick.AddListener(new UnityAction(this.OnClickButton));
	}

	// Token: 0x06000AA8 RID: 2728 RVA: 0x00027FDB File Offset: 0x000261DB
	private void OnDisable()
	{
		this.button_Continue.onClick.RemoveListener(new UnityAction(this.OnClickButton));
	}

	// Token: 0x06000AA9 RID: 2729 RVA: 0x00027FF9 File Offset: 0x000261F9
	private void OnClickButton()
	{
		this.isButtonClicked = true;
		this.button_Continue.enabled = false;
	}

	// Token: 0x06000AAA RID: 2730 RVA: 0x00028010 File Offset: 0x00026210
	private void OnPlayerVictory()
	{
		StageRewardData currentStageRewardData = GameDataManager.instance.GameplayData.GetCurrentStageRewardData();
		this.text_ExpValue.text = "+" + currentStageRewardData.Exp.ToString();
		this.text_GemValue.text = "+" + currentStageRewardData.Gem.ToString();
		Debug.Log(string.Format("關卡獎勵: {0} - {1} x {2}", currentStageRewardData.RewardType, currentStageRewardData.ItemType, currentStageRewardData.Gem));
		this.animator_Reward_Exp.gameObject.SetActive(currentStageRewardData.Exp > 0);
		if (currentStageRewardData.Exp > 0)
		{
			int num = currentStageRewardData.Exp;
			if (GameDataManager.instance.Playerdata.IsTalentLearned(eTalentType.EXP_GAIN_INCREASE))
			{
				num = (int)((float)num * 1.2f);
			}
			if (GameDataManager.instance.IntermediateData.isCorrupted)
			{
				num = (int)((float)num * 1.5f);
			}
			EventMgr.SendEvent<int>(eGameEvents.RequestAddExp, num);
		}
		this.animator_Reward_Gem.gameObject.SetActive(currentStageRewardData.Gem > 0);
		if (currentStageRewardData.Gem > 0)
		{
			if (GameDataManager.instance.IntermediateData.isCorrupted)
			{
				currentStageRewardData.Gem = (int)((float)currentStageRewardData.Gem * 1.5f);
			}
			EventMgr.SendEvent<int>(eGameEvents.RequestAddGem, currentStageRewardData.Gem);
		}
		switch (currentStageRewardData.RewardType)
		{
		case eStageRewardType.NONE:
			this.animator_Reward_Card.gameObject.SetActive(false);
			break;
		case eStageRewardType.TETRIS:
		{
			this.animator_Reward_Card.gameObject.SetActive(true);
			AItemSettingData itemDataByType = Singleton<ResourceManager>.Instance.GetItemDataByType(currentStageRewardData.ItemType);
			this.cardFace_Reward.SetupContent(currentStageRewardData.ItemType, currentStageRewardData.ItemType.ToCardType(), itemDataByType.GetCardIcon(), false);
			EventMgr.SendEvent<eItemType>(eGameEvents.RequestAddCardToStorage, currentStageRewardData.ItemType);
			break;
		}
		case eStageRewardType.TOWER:
		{
			this.animator_Reward_Card.gameObject.SetActive(true);
			eItemType itemType = this.GetRandomNewTower().GetItemType();
			AItemSettingData itemDataByType2 = Singleton<ResourceManager>.Instance.GetItemDataByType(itemType);
			this.cardFace_Reward.SetupContent(itemType, itemType.ToCardType(), itemDataByType2.GetCardIcon(), true);
			this.cardFace_Reward.ToggleNameText(true);
			this.cardFace_Reward.SetTowerDetail(itemType, true, false);
			TowerIngameData arg = new TowerIngameData(itemType, 1);
			EventMgr.SendEvent<TowerIngameData>(eGameEvents.RequestAddTowerCard, arg);
			break;
		}
		}
		this.Toggle(true);
		this.isButtonClicked = false;
		base.StartCoroutine(this.CR_VictoryProc());
		SoundManager.PlaySound("UI", "Victory", -1f, -1f, -1f);
		GameDataManager.instance.SaveData();
	}

	// Token: 0x06000AAB RID: 2731 RVA: 0x000282B0 File Offset: 0x000264B0
	private TowerSettingData GetRandomNewTower()
	{
		List<TowerSettingData> allTowerSettingData = Singleton<ResourceManager>.Instance.GetAllTowerSettingData();
		for (int i = allTowerSettingData.Count - 1; i >= 0; i--)
		{
			if (!allTowerSettingData[i].IsPurchaseable())
			{
				allTowerSettingData.RemoveAt(i);
			}
			else if (GameDataManager.instance.GameplayData.IsHaveTowerInCollected(allTowerSettingData[i].GetItemType()))
			{
				allTowerSettingData.RemoveAt(i);
			}
		}
		WeightedRandom<TowerSettingData> weightedRandom = new WeightedRandom<TowerSettingData>();
		bool flag = GameDataManager.instance.GameplayData.IsHaveTowerWithSize(eTowerSizeType._2x2);
		foreach (TowerSettingData towerSettingData in allTowerSettingData)
		{
			int weight = 1;
			if (!flag && towerSettingData.TowerSizeType == eTowerSizeType._2x2)
			{
				weight = 2;
			}
			weightedRandom.AddItem(towerSettingData, weight);
		}
		return weightedRandom.GetRandomResult();
	}

	// Token: 0x06000AAC RID: 2732 RVA: 0x0002838C File Offset: 0x0002658C
	private IEnumerator CR_VictoryProc()
	{
		yield return new WaitForSeconds(2.5f);
		if (this.animator_Reward_Exp.gameObject.activeSelf)
		{
			this.animator_Reward_Exp.SetTrigger("Trigger");
			yield return new WaitForSeconds(0.15f);
		}
		if (this.animator_Reward_Gem.gameObject.activeSelf)
		{
			this.animator_Reward_Gem.SetTrigger("Trigger");
			yield return new WaitForSeconds(0.15f);
		}
		if (this.animator_Reward_Card.gameObject.activeSelf)
		{
			this.animator_Reward_Card.SetTrigger("Trigger");
			yield return new WaitForSeconds(0.15f);
		}
		yield return new WaitForSeconds(this.waitTimeAfterVictory);
		this.button_Continue.enabled = true;
		while (!this.isButtonClicked)
		{
			yield return null;
		}
		EventMgr.SendEvent(eGameEvents.UI_VictoryUICompleted);
		yield break;
	}

	// Token: 0x06000AAD RID: 2733 RVA: 0x0002839B File Offset: 0x0002659B
	public void Toggle(bool isOn)
	{
		this.animator.SetBool("isOn", isOn);
	}

	// Token: 0x06000AAE RID: 2734 RVA: 0x000283AE File Offset: 0x000265AE
	protected override void ShowWindowProc()
	{
		this.OnPlayerVictory();
	}

	// Token: 0x06000AAF RID: 2735 RVA: 0x000283B6 File Offset: 0x000265B6
	protected override void CloseWindowProc()
	{
	}

	// Token: 0x04000829 RID: 2089
	[SerializeField]
	private float waitTimeAfterVictory = 3f;

	// Token: 0x0400082A RID: 2090
	[SerializeField]
	private Button button_Continue;

	// Token: 0x0400082B RID: 2091
	[SerializeField]
	private TMP_Text text_Victory;

	// Token: 0x0400082C RID: 2092
	[SerializeField]
	private TypewriterByCharacter typewriter_Victory;

	// Token: 0x0400082D RID: 2093
	[SerializeField]
	private TMP_Text text_GemValue;

	// Token: 0x0400082E RID: 2094
	[SerializeField]
	private TMP_Text text_ExpValue;

	// Token: 0x0400082F RID: 2095
	[SerializeField]
	private GameObject node_Reward_Card;

	// Token: 0x04000830 RID: 2096
	[SerializeField]
	private UI_CardFace cardFace_Reward;

	// Token: 0x04000831 RID: 2097
	[SerializeField]
	private Animator animator_Reward_Exp;

	// Token: 0x04000832 RID: 2098
	[SerializeField]
	private Animator animator_Reward_Gem;

	// Token: 0x04000833 RID: 2099
	[SerializeField]
	private Animator animator_Reward_Card;

	// Token: 0x04000834 RID: 2100
	private bool isButtonClicked;
}
