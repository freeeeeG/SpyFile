using System;
using System.Text;
using Steamworks;
using UnityEngine;

// Token: 0x0200006C RID: 108
[DisallowMultipleComponent]
public class SteamManager : MonoBehaviour
{
	// Token: 0x170000D4 RID: 212
	// (get) Token: 0x06000405 RID: 1029 RVA: 0x00019CD7 File Offset: 0x00017ED7
	protected static SteamManager Instance
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

	// Token: 0x170000D5 RID: 213
	// (get) Token: 0x06000406 RID: 1030 RVA: 0x00019CFB File Offset: 0x00017EFB
	public static bool Initialized
	{
		get
		{
			return SteamManager.Instance.m_bInitialized;
		}
	}

	// Token: 0x06000407 RID: 1031 RVA: 0x00019D07 File Offset: 0x00017F07
	protected static void SteamAPIDebugTextHook(int nSeverity, StringBuilder pchDebugText)
	{
		Debug.LogWarning(pchDebugText);
	}

	// Token: 0x06000408 RID: 1032 RVA: 0x00019D10 File Offset: 0x00017F10
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
			if (!GameParameters.Inst.ifDemo && !GameParameters.Inst.ifSkipSteamDetect && SteamAPI.RestartAppIfNecessary((AppId_t)1255650U))
			{
				Debug.Log("未通过steam启动，重启");
				Application.Quit();
				return;
			}
		}
		catch (DllNotFoundException arg)
		{
			Debug.LogError("[Steamworks.NET] Could not load [lib]steam_api.dll/so/dylib. It's likely not in the correct location. Refer to the README for more details.\n" + arg, this);
			Application.Quit();
			return;
		}
		this.m_bInitialized = SteamAPI.Init();
		if (!this.m_bInitialized)
		{
			Debug.LogError("[Steamworks.NET] SteamAPI_Init() failed. Refer to Valve's documentation or the comment above this line for more information.", this);
			return;
		}
		SteamManager.s_EverInitialized = true;
	}

	// Token: 0x06000409 RID: 1033 RVA: 0x00019E10 File Offset: 0x00018010
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

	// Token: 0x0600040A RID: 1034 RVA: 0x00019E5E File Offset: 0x0001805E
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
		Debug.Log("SteamAPI.Shutdown()");
		SteamAPI.Shutdown();
	}

	// Token: 0x0600040B RID: 1035 RVA: 0x00019E8C File Offset: 0x0001808C
	protected virtual void Update()
	{
		if (!this.m_bInitialized)
		{
			return;
		}
		SteamAPI.RunCallbacks();
	}

	// Token: 0x0400038C RID: 908
	protected static SteamManager s_instance;

	// Token: 0x0400038D RID: 909
	protected static bool s_EverInitialized;

	// Token: 0x0400038E RID: 910
	protected bool m_bInitialized;

	// Token: 0x0400038F RID: 911
	protected SteamAPIWarningMessageHook_t m_SteamAPIWarningMessageHook;
}
