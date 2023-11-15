using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

// Token: 0x02000BB6 RID: 2998
[ExecutionDependency(typeof(BootstrapManager))]
public class MultiLevelMiniPortalMapNode : MiniLevelPortalMapNode
{
	// Token: 0x17000424 RID: 1060
	// (get) Token: 0x06003D49 RID: 15689 RVA: 0x00124FB4 File Offset: 0x001233B4
	public override int LevelIndex
	{
		get
		{
			if (this.m_altLevelIndex != -1)
			{
				return this.m_altLevelIndex;
			}
			return base.LevelIndex;
		}
	}

	// Token: 0x17000425 RID: 1061
	// (get) Token: 0x06003D4A RID: 15690 RVA: 0x00124FCF File Offset: 0x001233CF
	public int[] AlternateLevelIndexes
	{
		get
		{
			return this.m_altLevels;
		}
	}

	// Token: 0x06003D4B RID: 15691 RVA: 0x00124FD8 File Offset: 0x001233D8
	protected override void Awake()
	{
		base.Awake();
		int[] altLevels = this.m_altLevels;
		if (MultiLevelMiniPortalMapNode.<>f__mg$cache0 == null)
		{
			MultiLevelMiniPortalMapNode.<>f__mg$cache0 = new Generic<float, int>(MultiLevelMiniPortalMapNode.LevelScoreFunction);
		}
		KeyValuePair<int, int> keyValuePair = altLevels.FindHighestScoring(MultiLevelMiniPortalMapNode.<>f__mg$cache0);
		if (keyValuePair.Key != -1)
		{
			this.m_altLevelIndex = keyValuePair.Value;
		}
	}

	// Token: 0x06003D4C RID: 15692 RVA: 0x00125030 File Offset: 0x00123430
	private static float LevelScoreFunction(int _levelIndex)
	{
		GameSession gameSession = GameUtils.GetGameSession();
		if (gameSession.Progress.SaveData.IsLevelUnlocked(_levelIndex, false))
		{
			return (float)_levelIndex;
		}
		return float.MinValue;
	}

	// Token: 0x0400313F RID: 12607
	[Space]
	[SerializeField]
	[LevelIndex]
	private int[] m_altLevels = new int[0];

	// Token: 0x04003140 RID: 12608
	private int m_altLevelIndex = -1;

	// Token: 0x04003141 RID: 12609
	[CompilerGenerated]
	private static Generic<float, int> <>f__mg$cache0;
}
