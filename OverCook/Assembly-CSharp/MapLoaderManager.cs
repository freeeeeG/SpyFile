using System;
using UnityEngine;

// Token: 0x02000511 RID: 1297
public class MapLoaderManager : Manager
{
	// Token: 0x06001833 RID: 6195 RVA: 0x0007AED5 File Offset: 0x000792D5
	protected virtual void Awake()
	{
		MapLoaderManager.s_instance = this;
		this.m_multiplayerController = GameUtils.RequestManagerInterface<MultiplayerController>();
	}

	// Token: 0x06001834 RID: 6196 RVA: 0x0007AEE8 File Offset: 0x000792E8
	private void Update()
	{
		if (!this.m_bStarted && ConnectionModeSwitcher.GetStatus().GetProgress() == eConnectionModeSwitchProgress.Complete)
		{
			this.m_multiplayerController.StartWorldMap();
			this.m_bStarted = true;
		}
	}

	// Token: 0x06001835 RID: 6197 RVA: 0x0007AF17 File Offset: 0x00079317
	private void OnDestroy()
	{
		this.m_multiplayerController.StopWorldMap();
	}

	// Token: 0x04001376 RID: 4982
	[SerializeField]
	public static MapLoaderManager s_instance;

	// Token: 0x04001377 RID: 4983
	private MultiplayerController m_multiplayerController;

	// Token: 0x04001378 RID: 4984
	private bool m_bStarted;
}
