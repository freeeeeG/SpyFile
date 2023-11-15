using System;
using UnityEngine;

// Token: 0x02000BA3 RID: 2979
public class FlagBaseHandler : MonoBehaviour
{
	// Token: 0x06003D07 RID: 15623 RVA: 0x0012386C File Offset: 0x00121C6C
	private void Start()
	{
		this.m_levelMapNode = base.gameObject.RequireComponent<LevelPortalMapNode>();
		this.m_flowController = GameUtils.RequireManager<WorldMapFlowController>();
		SceneDirectoryData sceneDirectory = this.m_flowController.GetSceneDirectory();
		GameProgress.GameProgressData.LevelProgress levelProgress = this.m_levelMapNode.GetLevelProgress();
		SceneDirectoryData.SceneDirectoryEntry sceneDirectoryEntry = sceneDirectory.Scenes.TryAtIndex(this.m_levelMapNode.LevelIndex);
		if (sceneDirectoryEntry.IsHidden && levelProgress.Completed)
		{
			this.m_locked.SetActive(false);
			this.m_unlocked.SetActive(false);
		}
		else
		{
			this.m_locked.SetActive(true);
			this.m_unlocked.SetActive(true);
		}
	}

	// Token: 0x04003117 RID: 12567
	private LevelPortalMapNode m_levelMapNode;

	// Token: 0x04003118 RID: 12568
	private WorldMapFlowController m_flowController;

	// Token: 0x04003119 RID: 12569
	[SerializeField]
	private GameObject m_locked;

	// Token: 0x0400311A RID: 12570
	[SerializeField]
	private GameObject m_unlocked;
}
