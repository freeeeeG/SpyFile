using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// Token: 0x02000025 RID: 37
[CreateAssetMenu(fileName = "EnvSceneCollectionData", menuName = "設定檔/環境場景資料 (EnvSceneCollectionData)", order = 1)]
public class EnvSceneCollectionData : ScriptableObject
{
	// Token: 0x060000AD RID: 173 RVA: 0x000043B0 File Offset: 0x000025B0
	public void ResetStageWeights()
	{
		foreach (EnvSceneCollectionData.EnvSceneDataEntry envSceneDataEntry in this.sceneEntries)
		{
			envSceneDataEntry.weight = 5;
		}
	}

	// Token: 0x060000AE RID: 174 RVA: 0x00004404 File Offset: 0x00002604
	public EnvSceneCollectionData.EnvSceneDataEntry GetRandomScene(eWorldType worldType, bool isBossStage, int difficulty)
	{
		List<EnvSceneCollectionData.EnvSceneDataEntry> list = this.sceneEntries.FindAll((EnvSceneCollectionData.EnvSceneDataEntry entry) => (entry.worldType & worldType) == worldType && entry.isBossStage == isBossStage);
		eStageDifficulty stageDifficulty = eStageDifficulty.NONE;
		if (difficulty <= 3)
		{
			stageDifficulty = eStageDifficulty.EASY;
		}
		else if (difficulty <= 6)
		{
			stageDifficulty = eStageDifficulty.NORMAL;
		}
		else
		{
			stageDifficulty = eStageDifficulty.DIFFICULT;
		}
		Debug.Log(string.Format("關卡難度: {0}", stageDifficulty));
		if (!isBossStage)
		{
			list = list.FindAll((EnvSceneCollectionData.EnvSceneDataEntry entry) => entry.stageLevel == stageDifficulty);
		}
		list = list.FindAll((EnvSceneCollectionData.EnvSceneDataEntry entry) => entry.enableInRandomPick);
		List<EnvSceneCollectionData.EnvSceneDataEntry> list2 = new List<EnvSceneCollectionData.EnvSceneDataEntry>();
		for (int i = 0; i < list.Count; i++)
		{
			if (!GameDataManager.instance.GameplayData.IsPlayedScene(list[i].name))
			{
				list2.Add(list[i]);
			}
		}
		if (list2.Count > 0)
		{
			list = list2;
		}
		WeightedRandom<EnvSceneCollectionData.EnvSceneDataEntry> weightedRandom = new WeightedRandom<EnvSceneCollectionData.EnvSceneDataEntry>();
		foreach (EnvSceneCollectionData.EnvSceneDataEntry envSceneDataEntry in list)
		{
			weightedRandom.AddItem(envSceneDataEntry, envSceneDataEntry.weight);
		}
		EnvSceneCollectionData.EnvSceneDataEntry randomResult = weightedRandom.GetRandomResult();
		if (randomResult == null)
		{
			Debug.LogError("無法取得場景");
			return null;
		}
		GameDataManager.instance.GameplayData.RecordPlayedScene(randomResult.name);
		return randomResult;
	}

	// Token: 0x060000AF RID: 175 RVA: 0x00004594 File Offset: 0x00002794
	public bool IsSceneBossStage(string sceneName)
	{
		EnvSceneCollectionData.EnvSceneDataEntry envSceneDataEntry = this.sceneEntries.Find((EnvSceneCollectionData.EnvSceneDataEntry a) => a.name.Equals(sceneName));
		if (envSceneDataEntry != null)
		{
			return envSceneDataEntry.isBossStage;
		}
		Debug.LogError("找不到指定名稱(" + sceneName + ")的場景資料");
		return false;
	}

	// Token: 0x060000B0 RID: 176 RVA: 0x000045EC File Offset: 0x000027EC
	public eWorldType GetSceneWorldType(string sceneName)
	{
		EnvSceneCollectionData.EnvSceneDataEntry envSceneDataEntry = this.sceneEntries.Find((EnvSceneCollectionData.EnvSceneDataEntry a) => a.name.Equals(sceneName));
		if (envSceneDataEntry != null)
		{
			return envSceneDataEntry.worldType;
		}
		Debug.LogError("找不到指定名稱(" + sceneName + ")的場景資料");
		return eWorldType.NONE;
	}

	// Token: 0x060000B1 RID: 177 RVA: 0x00004644 File Offset: 0x00002844
	public EnvSceneCollectionData.EnvSceneDataEntry GetSceneEntryByName(string name)
	{
		return this.sceneEntries.Find((EnvSceneCollectionData.EnvSceneDataEntry a) => a.name.Equals(name));
	}

	// Token: 0x060000B2 RID: 178 RVA: 0x00004675 File Offset: 0x00002875
	private void SortSceneByName()
	{
		this.sceneEntries = (from entry in this.sceneEntries
		orderby entry.name
		select entry).ToList<EnvSceneCollectionData.EnvSceneDataEntry>();
	}

	// Token: 0x060000B3 RID: 179 RVA: 0x000046AC File Offset: 0x000028AC
	private void SelectAll()
	{
		foreach (EnvSceneCollectionData.EnvSceneDataEntry envSceneDataEntry in this.sceneEntries)
		{
			envSceneDataEntry.enableInDebug = true;
		}
	}

	// Token: 0x060000B4 RID: 180 RVA: 0x00004700 File Offset: 0x00002900
	private void DeselectAll()
	{
		foreach (EnvSceneCollectionData.EnvSceneDataEntry envSceneDataEntry in this.sceneEntries)
		{
			envSceneDataEntry.enableInDebug = false;
		}
	}

	// Token: 0x04000084 RID: 132
	public List<EnvSceneCollectionData.EnvSceneDataEntry> sceneEntries;

	// Token: 0x020001D6 RID: 470
	[Serializable]
	public class EnvSceneDataEntry
	{
		// Token: 0x040009A7 RID: 2471
		public bool enableInDebug = true;

		// Token: 0x040009A8 RID: 2472
		public bool enableInRandomPick = true;

		// Token: 0x040009A9 RID: 2473
		public string name;

		// Token: 0x040009AA RID: 2474
		public eWorldType worldType;

		// Token: 0x040009AB RID: 2475
		public StageSettingData presetStageData;

		// Token: 0x040009AC RID: 2476
		public bool isBossStage;

		// Token: 0x040009AD RID: 2477
		public eStageDifficulty stageLevel;

		// Token: 0x040009AE RID: 2478
		public int weight = 1;
	}
}
