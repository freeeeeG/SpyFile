using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using AssetBundles;
using Team17.Online;
using Team17.Online.Multiplayer.Messaging;
using UnityEngine;
using UnityEngine.SceneManagement;

// Token: 0x020006DC RID: 1756
public class LoadingScreenFlow : MonoBehaviour
{
	// Token: 0x1700028F RID: 655
	// (get) Token: 0x06002125 RID: 8485 RVA: 0x0009E322 File Offset: 0x0009C722
	public static bool IsLoading
	{
		get
		{
			return LoadingScreenFlow.m_loading;
		}
	}

	// Token: 0x17000290 RID: 656
	// (get) Token: 0x06002126 RID: 8486 RVA: 0x0009E329 File Offset: 0x0009C729
	public static bool IsAbortingLoad
	{
		get
		{
			return LoadingScreenFlow.m_abortBackToStartScreen;
		}
	}

	// Token: 0x06002127 RID: 8487 RVA: 0x0009E330 File Offset: 0x0009C730
	public static bool IsLoadingStartScreen()
	{
		return LoadingScreenFlow.s_nextScene == "StartScreen" || LoadingScreenFlow.IsAbortingLoad;
	}

	// Token: 0x17000291 RID: 657
	// (get) Token: 0x06002128 RID: 8488 RVA: 0x0009E34E File Offset: 0x0009C74E
	public string NextScene
	{
		get
		{
			return LoadingScreenFlow.s_nextScene;
		}
	}

	// Token: 0x17000292 RID: 658
	// (get) Token: 0x06002129 RID: 8489 RVA: 0x0009E355 File Offset: 0x0009C755
	public float Progress
	{
		get
		{
			if (LoadingScreenFlow.m_routine != null)
			{
				return LoadingScreenFlow.m_routine.Current;
			}
			if (LoadingScreenFlow.m_sceneLoaderHelper != null)
			{
				return LoadingScreenFlow.m_sceneLoaderHelper.GetProgress();
			}
			return -1f;
		}
	}

	// Token: 0x0600212A RID: 8490 RVA: 0x0009E38D File Offset: 0x0009C78D
	private void Awake()
	{
		UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
		LoadingScreenFlow.m_CachedDisconnectionError = null;
		LoadingScreenFlow.m_CachedConnectionModeError = null;
		Mailbox.Client.RegisterForMessageType(MessageType.GameState, new OrderedMessageReceivedCallback(this.OnGameStateChanged));
	}

	// Token: 0x0600212B RID: 8491 RVA: 0x0009E3C0 File Offset: 0x0009C7C0
	private void OnDestroy()
	{
		Mailbox.Client.UnregisterForMessageType(MessageType.GameState, new OrderedMessageReceivedCallback(this.OnGameStateChanged));
		Delegate sessionConnectionLostEvent = DisconnectionHandler.SessionConnectionLostEvent;
		if (LoadingScreenFlow.<>f__mg$cache0 == null)
		{
			LoadingScreenFlow.<>f__mg$cache0 = new GenericVoid(LoadingScreenFlow.OnSessionConnectionLost);
		}
		DisconnectionHandler.SessionConnectionLostEvent = (GenericVoid)Delegate.Remove(sessionConnectionLostEvent, LoadingScreenFlow.<>f__mg$cache0);
		Delegate connectionModeErrorEvent = DisconnectionHandler.ConnectionModeErrorEvent;
		if (LoadingScreenFlow.<>f__mg$cache1 == null)
		{
			LoadingScreenFlow.<>f__mg$cache1 = new GenericVoid<OnlineMultiplayerReturnCode<OnlineMultiplayerConnectionModeErrorResult>>(LoadingScreenFlow.OnConnectionModeError);
		}
		DisconnectionHandler.ConnectionModeErrorEvent = (GenericVoid<OnlineMultiplayerReturnCode<OnlineMultiplayerConnectionModeErrorResult>>)Delegate.Remove(connectionModeErrorEvent, LoadingScreenFlow.<>f__mg$cache1);
		Delegate kickedFromSessionEvent = DisconnectionHandler.KickedFromSessionEvent;
		if (LoadingScreenFlow.<>f__mg$cache2 == null)
		{
			LoadingScreenFlow.<>f__mg$cache2 = new GenericVoid(LoadingScreenFlow.OnKickedFromSession);
		}
		DisconnectionHandler.KickedFromSessionEvent = (GenericVoid)Delegate.Remove(kickedFromSessionEvent, LoadingScreenFlow.<>f__mg$cache2);
		Delegate localDisconnectionEvent = DisconnectionHandler.LocalDisconnectionEvent;
		if (LoadingScreenFlow.<>f__mg$cache3 == null)
		{
			LoadingScreenFlow.<>f__mg$cache3 = new GenericVoid<OnlineMultiplayerReturnCode<OnlineMultiplayerSessionDisconnectionResult>>(LoadingScreenFlow.OnLocalDisconnection);
		}
		DisconnectionHandler.LocalDisconnectionEvent = (GenericVoid<OnlineMultiplayerReturnCode<OnlineMultiplayerSessionDisconnectionResult>>)Delegate.Remove(localDisconnectionEvent, LoadingScreenFlow.<>f__mg$cache3);
		LoadingScreenFlow.m_CachedDisconnectionError = null;
		LoadingScreenFlow.m_CachedConnectionModeError = null;
	}

	// Token: 0x0600212C RID: 8492 RVA: 0x0009E4B8 File Offset: 0x0009C8B8
	public static void LoadScene(string _NextScene, GameState _GameState = GameState.NotSet)
	{
		if (!GameUtils.CanLoadScene(_NextScene))
		{
			return;
		}
		LoadingScreenFlow.s_nextScene = _NextScene;
		LoadingScreenFlow.s_gameState = _GameState;
		LoadingScreenFlow.m_abortBackToStartScreen = false;
		ScreenTransitionManager transitionManager = GameUtils.RequireManager<ScreenTransitionManager>();
		transitionManager.StartTransitionUp(delegate
		{
			string sceneName = SceneManager.GetActiveScene().name;
			AssetBundleManager.LoadLevel("loading", "Loading", false);
			transitionManager.StartTransitionDown(delegate
			{
				LoadingScreenFlow.StartLoading(sceneName);
			});
		});
		InviteMonitor.SwitchHandlerType(InviteMonitor.HandlerType.None);
		Delegate sessionConnectionLostEvent = DisconnectionHandler.SessionConnectionLostEvent;
		if (LoadingScreenFlow.<>f__mg$cache4 == null)
		{
			LoadingScreenFlow.<>f__mg$cache4 = new GenericVoid(LoadingScreenFlow.OnSessionConnectionLost);
		}
		DisconnectionHandler.SessionConnectionLostEvent = (GenericVoid)Delegate.Combine(sessionConnectionLostEvent, LoadingScreenFlow.<>f__mg$cache4);
		Delegate connectionModeErrorEvent = DisconnectionHandler.ConnectionModeErrorEvent;
		if (LoadingScreenFlow.<>f__mg$cache5 == null)
		{
			LoadingScreenFlow.<>f__mg$cache5 = new GenericVoid<OnlineMultiplayerReturnCode<OnlineMultiplayerConnectionModeErrorResult>>(LoadingScreenFlow.OnConnectionModeError);
		}
		DisconnectionHandler.ConnectionModeErrorEvent = (GenericVoid<OnlineMultiplayerReturnCode<OnlineMultiplayerConnectionModeErrorResult>>)Delegate.Combine(connectionModeErrorEvent, LoadingScreenFlow.<>f__mg$cache5);
		Delegate kickedFromSessionEvent = DisconnectionHandler.KickedFromSessionEvent;
		if (LoadingScreenFlow.<>f__mg$cache6 == null)
		{
			LoadingScreenFlow.<>f__mg$cache6 = new GenericVoid(LoadingScreenFlow.OnKickedFromSession);
		}
		DisconnectionHandler.KickedFromSessionEvent = (GenericVoid)Delegate.Combine(kickedFromSessionEvent, LoadingScreenFlow.<>f__mg$cache6);
		Delegate localDisconnectionEvent = DisconnectionHandler.LocalDisconnectionEvent;
		if (LoadingScreenFlow.<>f__mg$cache7 == null)
		{
			LoadingScreenFlow.<>f__mg$cache7 = new GenericVoid<OnlineMultiplayerReturnCode<OnlineMultiplayerSessionDisconnectionResult>>(LoadingScreenFlow.OnLocalDisconnection);
		}
		DisconnectionHandler.LocalDisconnectionEvent = (GenericVoid<OnlineMultiplayerReturnCode<OnlineMultiplayerSessionDisconnectionResult>>)Delegate.Combine(localDisconnectionEvent, LoadingScreenFlow.<>f__mg$cache7);
	}

	// Token: 0x0600212D RID: 8493 RVA: 0x0009E5D8 File Offset: 0x0009C9D8
	private static void OnSessionConnectionLost()
	{
		LoadingScreenFlow.m_CachedDisconnectionError = new OnlineMultiplayerReturnCode<OnlineMultiplayerSessionDisconnectionResult>
		{
			m_returnCode = OnlineMultiplayerSessionDisconnectionResult.eGeneric
		};
		LoadingScreenFlow.RequestReturnToStartScreen();
	}

	// Token: 0x0600212E RID: 8494 RVA: 0x0009E5FD File Offset: 0x0009C9FD
	private static void OnConnectionModeError(OnlineMultiplayerReturnCode<OnlineMultiplayerConnectionModeErrorResult> result)
	{
		LoadingScreenFlow.m_CachedConnectionModeError = result;
		LoadingScreenFlow.RequestReturnToStartScreen();
	}

	// Token: 0x0600212F RID: 8495 RVA: 0x0009E60C File Offset: 0x0009CA0C
	private static void OnKickedFromSession()
	{
		LoadingScreenFlow.m_CachedDisconnectionError = new OnlineMultiplayerReturnCode<OnlineMultiplayerSessionDisconnectionResult>
		{
			m_returnCode = OnlineMultiplayerSessionDisconnectionResult.eKicked
		};
		LoadingScreenFlow.RequestReturnToStartScreen();
	}

	// Token: 0x06002130 RID: 8496 RVA: 0x0009E631 File Offset: 0x0009CA31
	private static void OnLocalDisconnection(OnlineMultiplayerReturnCode<OnlineMultiplayerSessionDisconnectionResult> result)
	{
		LoadingScreenFlow.m_CachedDisconnectionError = result;
		LoadingScreenFlow.RequestReturnToStartScreen();
	}

	// Token: 0x06002131 RID: 8497 RVA: 0x0009E640 File Offset: 0x0009CA40
	private void OnGameStateChanged(IOnlineMultiplayerSessionUserId sessionUserId, Serialisable message)
	{
		GameStateMessage gameStateMessage = (GameStateMessage)message;
		if (!this.m_finished && SceneManager.GetSceneByName(this.NextScene).isLoaded && gameStateMessage.m_State == LoadingScreenFlow.s_gameState)
		{
			this.Finish();
		}
	}

	// Token: 0x06002132 RID: 8498 RVA: 0x0009E68D File Offset: 0x0009CA8D
	private void Start()
	{
		LoadingScreenFlow.m_sceneLoaderHelper = base.gameObject.AddComponent<SceneLoaderHelper>();
	}

	// Token: 0x06002133 RID: 8499 RVA: 0x0009E6A0 File Offset: 0x0009CAA0
	private static void StartLoading(string activeScene = null)
	{
		if (LoadingScreenFlow.m_sceneLoaderHelper != null)
		{
			if (LoadingScreenFlow.s_nextScene == activeScene)
			{
				AssetBundleManager.UnloadAssetBundle(LoadingScreenFlow.s_nextScene.ToLowerInvariant());
			}
			LoadingScreenFlow.m_sceneLoaderHelper.LoadLevelAsync(LoadingScreenFlow.s_nextScene, false, LoadSceneMode.Single);
			LoadingScreenFlow.m_loading = true;
		}
	}

	// Token: 0x06002134 RID: 8500 RVA: 0x0009E6F4 File Offset: 0x0009CAF4
	private void Update()
	{
		if (LoadingScreenFlow.m_sceneLoaderHelper != null)
		{
			LoadingScreenFlow.m_sceneLoaderHelper.ActivateSceneWhenLoaded = !TimeManager.IsPaused(TimeManager.PauseLayer.System);
		}
		if (LoadingScreenFlow.m_routine != null)
		{
			LoadingScreenFlow.m_routine.MoveNext();
		}
		if (!this.m_finishCalled)
		{
			if (InviteMonitor.GetAcceptedInvite() != null && !this.NextScene.Equals("StartScreen") && ClientUserSystem.m_Users.Count == 1 && !LoadingScreenFlow.m_abortBackToStartScreen)
			{
				LoadingScreenFlow.RequestReturnToStartScreen();
			}
			else if (!this.m_finished && SceneManager.GetSceneByName(this.NextScene).isLoaded && (LoadingScreenFlow.s_gameState == GameState.NotSet || LoadingScreenFlow.m_abortBackToStartScreen))
			{
				this.Finish();
			}
		}
		if (this.m_finished)
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
	}

	// Token: 0x06002135 RID: 8501 RVA: 0x0009E7DC File Offset: 0x0009CBDC
	private static void RequestReturnToStartScreen()
	{
		if (!LoadingScreenFlow.m_abortBackToStartScreen)
		{
			LoadingScreenFlow.m_abortBackToStartScreen = true;
			NetConnectionState state = NetConnectionState.Offline;
			object data = null;
			if (LoadingScreenFlow.<>f__mg$cache8 == null)
			{
				LoadingScreenFlow.<>f__mg$cache8 = new GenericVoid<IConnectionModeSwitchStatus>(LoadingScreenFlow.OnRequestConnectionStateOfflineComplete);
			}
			ConnectionModeSwitcher.RequestConnectionState(state, data, LoadingScreenFlow.<>f__mg$cache8);
			MultiplayerController multiplayerController = GameUtils.RequireManager<MultiplayerController>();
			if (MultiplayerController.IsSynchronisationActive())
			{
				multiplayerController.StopSynchronisation();
			}
			if (LoadingScreenFlow.s_nextScene != "StartScreen")
			{
				LoadingScreenFlow.m_abortBackToStartScreen = true;
			}
		}
	}

	// Token: 0x06002136 RID: 8502 RVA: 0x0009E84E File Offset: 0x0009CC4E
	private static void OnRequestConnectionStateOfflineComplete(IConnectionModeSwitchStatus _status)
	{
		if (_status.GetResult() == eConnectionModeSwitchResult.Success)
		{
			ServerGameSetup.Mode = GameMode.OnlineKitchen;
		}
	}

	// Token: 0x06002137 RID: 8503 RVA: 0x0009E862 File Offset: 0x0009CC62
	private static void LoadStartScreen()
	{
		LoadingScreenFlow.s_gameState = GameState.NotSet;
		LoadingScreenFlow.m_abortBackToStartScreen = false;
		if (LoadingScreenFlow.s_nextScene != "StartScreen")
		{
			LoadingScreenFlow.s_nextScene = "StartScreen";
			LoadingScreenFlow.StartLoading(null);
		}
	}

	// Token: 0x06002138 RID: 8504 RVA: 0x0009E894 File Offset: 0x0009CC94
	private void Finish()
	{
		if (LoadingScreenFlow.m_abortBackToStartScreen)
		{
			LoadingScreenFlow.LoadStartScreen();
			return;
		}
		this.m_finishCalled = true;
		PlayerManager playerManager = GameUtils.RequireManager<PlayerManager>();
		if (playerManager != null && playerManager.HasPlayer())
		{
			if (LoadingScreenFlow.m_CachedDisconnectionError != null)
			{
				NetworkErrorDialog.ShowDialog(LoadingScreenFlow.m_CachedDisconnectionError);
			}
			else if (LoadingScreenFlow.m_CachedConnectionModeError != null)
			{
				NetworkErrorDialog.ShowDialog(LoadingScreenFlow.m_CachedConnectionModeError);
			}
		}
		else if (LoadingScreenFlow.m_CachedDisconnectionError != null || LoadingScreenFlow.m_CachedConnectionModeError != null)
		{
			LoadingScreenFlow.m_CachedDisconnectionError = null;
			LoadingScreenFlow.m_CachedConnectionModeError = null;
		}
		ScreenTransitionManager transitionManager = GameUtils.RequireManager<ScreenTransitionManager>();
		transitionManager.StartTransitionUp(delegate
		{
			LoadingScreenFlow.m_loading = false;
			this.m_finished = true;
			this.m_finishCalled = false;
			transitionManager.StartTransitionDown(null);
		});
	}

	// Token: 0x04001940 RID: 6464
	public const string StartScene = "StartScreen";

	// Token: 0x04001941 RID: 6465
	private static string s_nextScene = "StartScreen";

	// Token: 0x04001942 RID: 6466
	private static GameState s_gameState;

	// Token: 0x04001943 RID: 6467
	private static SceneLoaderHelper m_sceneLoaderHelper;

	// Token: 0x04001944 RID: 6468
	private static AsyncOperation m_asyncOperation;

	// Token: 0x04001945 RID: 6469
	private static IEnumerator<float> m_routine;

	// Token: 0x04001946 RID: 6470
	private static bool m_loading;

	// Token: 0x04001947 RID: 6471
	private static bool m_abortBackToStartScreen;

	// Token: 0x04001948 RID: 6472
	private bool m_finished;

	// Token: 0x04001949 RID: 6473
	private bool m_finishCalled;

	// Token: 0x0400194A RID: 6474
	private GameSession m_gameSession;

	// Token: 0x0400194B RID: 6475
	private static OnlineMultiplayerReturnCode<OnlineMultiplayerSessionDisconnectionResult> m_CachedDisconnectionError;

	// Token: 0x0400194C RID: 6476
	private static OnlineMultiplayerReturnCode<OnlineMultiplayerConnectionModeErrorResult> m_CachedConnectionModeError;

	// Token: 0x0400194D RID: 6477
	[CompilerGenerated]
	private static GenericVoid <>f__mg$cache0;

	// Token: 0x0400194E RID: 6478
	[CompilerGenerated]
	private static GenericVoid<OnlineMultiplayerReturnCode<OnlineMultiplayerConnectionModeErrorResult>> <>f__mg$cache1;

	// Token: 0x0400194F RID: 6479
	[CompilerGenerated]
	private static GenericVoid <>f__mg$cache2;

	// Token: 0x04001950 RID: 6480
	[CompilerGenerated]
	private static GenericVoid<OnlineMultiplayerReturnCode<OnlineMultiplayerSessionDisconnectionResult>> <>f__mg$cache3;

	// Token: 0x04001951 RID: 6481
	[CompilerGenerated]
	private static GenericVoid <>f__mg$cache4;

	// Token: 0x04001952 RID: 6482
	[CompilerGenerated]
	private static GenericVoid<OnlineMultiplayerReturnCode<OnlineMultiplayerConnectionModeErrorResult>> <>f__mg$cache5;

	// Token: 0x04001953 RID: 6483
	[CompilerGenerated]
	private static GenericVoid <>f__mg$cache6;

	// Token: 0x04001954 RID: 6484
	[CompilerGenerated]
	private static GenericVoid<OnlineMultiplayerReturnCode<OnlineMultiplayerSessionDisconnectionResult>> <>f__mg$cache7;

	// Token: 0x04001955 RID: 6485
	[CompilerGenerated]
	private static GenericVoid<IConnectionModeSwitchStatus> <>f__mg$cache8;
}
