using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000061 RID: 97
[Serializable]
public class MapNodeData
{
	// Token: 0x1700002F RID: 47
	// (get) Token: 0x06000260 RID: 608 RVA: 0x0000A353 File Offset: 0x00008553
	public bool IsCleared
	{
		get
		{
			return this.isCleared;
		}
	}

	// Token: 0x06000261 RID: 609 RVA: 0x0000A35C File Offset: 0x0000855C
	public MapNodeData(int index, int step, int indexInStep, eStageType mapNodeType, eMapNodeState state)
	{
		this.Index = index;
		this.Step = step;
		this.IndexInStep = indexInStep;
		this.MapNodeType = mapNodeType;
		this.State = state;
		this.connectedIndex = new List<int>();
		this.connectedFromCount = 0;
		this.stageReward = new StageRewardData(0, 0, eStageRewardType.NONE, eItemType.NONE);
	}

	// Token: 0x06000262 RID: 610 RVA: 0x0000A3B5 File Offset: 0x000085B5
	public void SetStageReward(StageRewardData rewardData)
	{
		this.stageReward = rewardData;
	}

	// Token: 0x06000263 RID: 611 RVA: 0x0000A3BE File Offset: 0x000085BE
	public void SetEnvSceneName(string name, int difficulty)
	{
		this.stageEnvSceneName = name;
		this.difficulty = difficulty;
	}

	// Token: 0x06000264 RID: 612 RVA: 0x0000A3CE File Offset: 0x000085CE
	public void AddConnection(int index)
	{
		if (this.connectedIndex.Contains(index))
		{
			return;
		}
		this.connectedIndex.Add(index);
	}

	// Token: 0x06000265 RID: 613 RVA: 0x0000A3EB File Offset: 0x000085EB
	public bool HasStageReward()
	{
		return this.stageReward != null && this.stageReward.RewardType > eStageRewardType.NONE;
	}

	// Token: 0x06000266 RID: 614 RVA: 0x0000A405 File Offset: 0x00008605
	public void SetCleared()
	{
		this.isCleared = true;
	}

	// Token: 0x040001AD RID: 429
	public int Index;

	// Token: 0x040001AE RID: 430
	public int Step;

	// Token: 0x040001AF RID: 431
	public int IndexInStep;

	// Token: 0x040001B0 RID: 432
	public eStageType MapNodeType;

	// Token: 0x040001B1 RID: 433
	public eMapNodeState State;

	// Token: 0x040001B2 RID: 434
	public StageRewardData stageReward;

	// Token: 0x040001B3 RID: 435
	public int randomSeed;

	// Token: 0x040001B4 RID: 436
	public List<int> connectedIndex;

	// Token: 0x040001B5 RID: 437
	public int connectedFromCount;

	// Token: 0x040001B6 RID: 438
	public int difficulty;

	// Token: 0x040001B7 RID: 439
	public string stageEnvSceneName;

	// Token: 0x040001B8 RID: 440
	[SerializeField]
	private bool isCleared;
}
