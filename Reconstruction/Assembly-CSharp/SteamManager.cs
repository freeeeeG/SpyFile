using System;
using System.Text;
using AOT;
using Steamworks;
using UnityEngine;

// Token: 0x0200018F RID: 399
[DisallowMultipleComponent]
public class SteamManager : MonoBehaviour
{
	// Token: 0x17000374 RID: 884
	// (get) Token: 0x06000A1B RID: 2587 RVA: 0x0001B6CE File Offset: 0x000198CE
	public static SteamManager Instance
	{
		get
		{
			if (SteamManager.s_instance == null)
			{
				return new GameObject("SteamManager").AddComponent<SteamManager>();
			}
			return SteamManager.s_instance;
		}
	}

	// Token: 0x17000375 RID: 885
	// (get) Token: 0x06000A1C RID: 2588 RVA: 0x0001B6F2 File Offset: 0x000198F2
	public static bool Initialized
	{
		get
		{
			return SteamManager.Instance.m_bInitialized;
		}
	}

	// Token: 0x06000A1D RID: 2589 RVA: 0x0001B6FE File Offset: 0x000198FE
	[MonoPInvokeCallback(typeof(SteamAPIWarningMessageHook_t))]
	protected static void SteamAPIDebugTextHook(int nSeverity, StringBuilder pchDebugText)
	{
		Debug.LogWarning(pchDebugText);
	}

	// Token: 0x06000A1E RID: 2590 RVA: 0x0001B706 File Offset: 0x00019906
	[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
	private static void InitOnPlayMode()
	{
		SteamManager.s_EverInitialized = false;
		SteamManager.s_instance = null;
	}

	// Token: 0x06000A1F RID: 2591 RVA: 0x0001B714 File Offset: 0x00019914
	protected virtual void Awake()
	{
		if (SteamManager.s_instance != null)
		{
			Object.Destroy(base.gameObject);
			return;
		}
		SteamManager.s_instance = this;
		if (SteamManager.s_EverInitialized)
		{
			throw new Exception("Tried to Initialize the SteamAPI twice in one session!");
		}
		Object.DontDestroyOnLoad(base.gameObject);
		if (!Packsize.Test())
		{
			Debug.LogError("[Steamworks.NET] Packsize Test returned false, the wrong version of Steamworks.NET is being run in this platform.", this);
		}
		if (!DllCheck.Test())
		{
			Debug.LogError("[Steamworks.NET] DllCheck Test returned false, One or more of the Steamworks binaries seems to be the wrong version.", this);
		}
		try
		{
			if (SteamAPI.RestartAppIfNecessary(AppId_t.Invalid))
			{
				Application.Quit();
			}
		}
		catch (DllNotFoundException ex)
		{
			string str = "[Steamworks.NET] Could not load [lib]steam_api.dll/so/dylib. It's likely not in the correct location. Refer to the README for more details.\n";
			DllNotFoundException ex2 = ex;
			Debug.LogError(str + ((ex2 != null) ? ex2.ToString() : null), this);
			Application.Quit();
		}
	}

	// Token: 0x06000A20 RID: 2592 RVA: 0x0001B7CC File Offset: 0x000199CC
	public void Initialize()
	{
		this.m_bInitialized = SteamAPI.Init();
		if (!this.m_bInitialized)
		{
			Debug.LogError("[Steamworks.NET] SteamAPI_Init() failed. Refer to Valve's documentation or the comment above this line for more information.", this);
			return;
		}
		SteamManager.s_EverInitialized = true;
	}

	// Token: 0x06000A21 RID: 2593 RVA: 0x0001B7F4 File Offset: 0x000199F4
	protected virtual void OnEnable()
	{
		if (SteamManager.s_instance == null)
		{
			SteamManager.s_instance = this;
		}
		if (!this.m_bInitialized)
		{
			return;
		}
		if (this.m_SteamAPIWarningMessageHook == null)
		{
			this.m_SteamAPIWarningMessageHook = new SteamAPIWarningMessageHook_t(SteamManager.SteamAPIDebugTextHook);
			SteamClient.SetWarningMessageHook(this.m_SteamAPIWarningMessageHook);
		}
	}

	// Token: 0x06000A22 RID: 2594 RVA: 0x0001B842 File Offset: 0x00019A42
	protected virtual void OnDestroy()
	{
		if (SteamManager.s_instance != this)
		{
			return;
		}
		SteamManager.s_instance = null;
		if (!this.m_bInitialized)
		{
			return;
		}
		SteamAPI.Shutdown();
	}

	// Token: 0x06000A23 RID: 2595 RVA: 0x0001B866 File Offset: 0x00019A66
	protected virtual void Update()
	{
		if (!this.m_bInitialized)
		{
			return;
		}
		SteamAPI.RunCallbacks();
	}

	// Token: 0x04000559 RID: 1369
	protected static bool s_EverInitialized;

	// Token: 0x0400055A RID: 1370
	protected static SteamManager s_instance;

	// Token: 0x0400055B RID: 1371
	protected bool m_bInitialized;

	// Token: 0x0400055C RID: 1372
	protected SteamAPIWarningMessageHook_t m_SteamAPIWarningMessageHook;
}
