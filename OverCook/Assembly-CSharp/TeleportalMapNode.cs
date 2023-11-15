using System;
using UnityEngine;

// Token: 0x02000BD5 RID: 3029
public class TeleportalMapNode : MapNode
{
	// Token: 0x1700042F RID: 1071
	// (get) Token: 0x06003DE8 RID: 15848 RVA: 0x0012797E File Offset: 0x00125D7E
	public SceneDirectoryData.World World
	{
		get
		{
			return this.m_world;
		}
	}

	// Token: 0x06003DE9 RID: 15849 RVA: 0x00127988 File Offset: 0x00125D88
	protected void Awake()
	{
		GameSession gameSession = GameUtils.GetGameSession();
		this.m_gameProgress = gameSession.Progress;
	}

	// Token: 0x06003DEA RID: 15850 RVA: 0x001279A8 File Offset: 0x00125DA8
	public bool ShouldFocus()
	{
		if (!base.enabled)
		{
			return false;
		}
		GameProgress.GameProgressData.TeleportalState teleportalState = this.m_gameProgress.GetTeleportalState(this.m_world);
		return teleportalState.World == SceneDirectoryData.World.COUNT;
	}

	// Token: 0x040031B2 RID: 12722
	[SerializeField]
	private SceneDirectoryData.World m_world = SceneDirectoryData.World.Invalid;

	// Token: 0x040031B3 RID: 12723
	private GameProgress m_gameProgress;
}
