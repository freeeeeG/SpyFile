using System;
using GameModes;
using UnityEngine;

// Token: 0x02000BAC RID: 2988
[ExecutionDependency(typeof(BootstrapManager))]
public class LevelPortalMapNode : PortalMapNode
{
	// Token: 0x06003D1E RID: 15646 RVA: 0x00124410 File Offset: 0x00122810
	protected override void Awake()
	{
		base.Awake();
		this.m_sceneProgress = this.m_gameProgress.GetProgress(this.LevelIndex);
	}

	// Token: 0x06003D1F RID: 15647 RVA: 0x0012442F File Offset: 0x0012282F
	public GameProgress.GameProgressData.LevelProgress GetLevelProgress()
	{
		if (this.m_sceneProgress != null)
		{
			return this.m_sceneProgress;
		}
		return null;
	}

	// Token: 0x06003D20 RID: 15648 RVA: 0x00124444 File Offset: 0x00122844
	public override WorldMapLevelIconUI GetUIPrefab(Kind _kind)
	{
		return this.m_gameModeUIData.m_gameModes[(int)_kind].m_levelPreview;
	}

	// Token: 0x04003127 RID: 12583
	[SerializeField]
	[AssignResource("GameModeUIData", Editorbility.Editable)]
	public GameModeUIData m_gameModeUIData;

	// Token: 0x04003128 RID: 12584
	[HideInInspector]
	public GameProgress.GameProgressData.LevelProgress m_sceneProgress;
}
