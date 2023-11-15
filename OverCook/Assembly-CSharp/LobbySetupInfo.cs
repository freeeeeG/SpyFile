using System;
using Team17.Online;
using UnityEngine;

// Token: 0x02000AD9 RID: 2777
[RequireComponent(typeof(PersistentObject))]
public class LobbySetupInfo : MonoBehaviour
{
	// Token: 0x06003827 RID: 14375 RVA: 0x00108938 File Offset: 0x00106D38
	private void Awake()
	{
		if (LobbySetupInfo.Instance != null)
		{
			UnityEngine.Object.Destroy(base.gameObject);
			return;
		}
		LobbySetupInfo.Instance = this;
		this.m_persist = base.gameObject.RequireComponent<PersistentObject>();
		this.m_persist.m_defaultBehaviour = PersistentObject.PersistType.DontPersist;
		this.m_persist.AddPersistingLevel("Lobbies");
		this.m_persist.AddPersistingLevel("Loading");
	}

	// Token: 0x06003828 RID: 14376 RVA: 0x001089A4 File Offset: 0x00106DA4
	private void OnDestroy()
	{
		if (LobbySetupInfo.Instance == this)
		{
			UnityEngine.Object.Destroy(base.gameObject);
			LobbySetupInfo.Instance = null;
		}
	}

	// Token: 0x04002CE3 RID: 11491
	public static LobbySetupInfo Instance;

	// Token: 0x04002CE4 RID: 11492
	public const string LobbyScene = "Lobbies";

	// Token: 0x04002CE5 RID: 11493
	private PersistentObject m_persist;

	// Token: 0x04002CE6 RID: 11494
	public OnlineMultiplayerConnectionMode m_connectionMode;

	// Token: 0x04002CE7 RID: 11495
	public OnlineMultiplayerSessionVisibility m_visiblity = OnlineMultiplayerSessionVisibility.eClosed;

	// Token: 0x04002CE8 RID: 11496
	public GameSession.GameType m_gameType;

	// Token: 0x04002CE9 RID: 11497
	public NetConnectionState m_originalConnectionState;
}
