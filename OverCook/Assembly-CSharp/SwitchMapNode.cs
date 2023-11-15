using System;
using UnityEngine;

// Token: 0x02000BC9 RID: 3017
public class SwitchMapNode : MapNode
{
	// Token: 0x1700042D RID: 1069
	// (get) Token: 0x06003DB6 RID: 15798 RVA: 0x00126564 File Offset: 0x00124964
	public int SwitchID
	{
		get
		{
			return this.m_switchID;
		}
	}

	// Token: 0x06003DB7 RID: 15799 RVA: 0x0012656C File Offset: 0x0012496C
	protected void Awake()
	{
		this.m_switchID = (int)EditorIDManager.GetUniqueID(base.gameObject);
		GameSession gameSession = GameUtils.GetGameSession();
		this.m_gameProgress = gameSession.Progress;
	}

	// Token: 0x06003DB8 RID: 15800 RVA: 0x0012659C File Offset: 0x0012499C
	public bool IsSwitchPressed()
	{
		GameProgress.GameProgressData.SwitchState switchState = this.m_gameProgress.GetSwitchState(this.SwitchID);
		return switchState.Activated;
	}

	// Token: 0x06003DB9 RID: 15801 RVA: 0x001265C4 File Offset: 0x001249C4
	public bool IsSwitchedDueToCompletion()
	{
		GameSession gameSession = GameUtils.GetGameSession();
		for (int i = 0; i < this.m_completionFlippers.Length; i++)
		{
			PortalMapNode portalMapNode = this.m_completionFlippers[i];
			if (portalMapNode != null)
			{
				GameProgress.GameProgressData.LevelProgress levelProgress = gameSession.Progress.SaveData.GetLevelProgress(portalMapNode.LevelIndex);
				if (levelProgress != null && levelProgress.LevelId != -1 && levelProgress.Completed)
				{
					return true;
				}
			}
		}
		return false;
	}

	// Token: 0x06003DBA RID: 15802 RVA: 0x0012663C File Offset: 0x00124A3C
	public bool CanProcessSwitch()
	{
		return !this.IsSwitchPressed();
	}

	// Token: 0x04003186 RID: 12678
	private int m_switchID = -1;

	// Token: 0x04003187 RID: 12679
	[SerializeField]
	private PortalMapNode[] m_completionFlippers = new PortalMapNode[0];

	// Token: 0x04003188 RID: 12680
	private GameProgress m_gameProgress;
}
