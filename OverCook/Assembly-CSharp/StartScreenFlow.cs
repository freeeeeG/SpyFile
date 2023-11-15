using System;
using System.Collections;
using System.Collections.Generic;
using AssetBundles;
using Team17.Online;
using UnityEngine;
using UnityEngine.PostProcessing;

// Token: 0x02000A4B RID: 2635
[AddComponentMenu("Scripts/Game/Flow/FrontEndFlow")]
public class StartScreenFlow : MonoBehaviour
{
	// Token: 0x170003AE RID: 942
	// (get) Token: 0x0600340A RID: 13322 RVA: 0x000F3EF5 File Offset: 0x000F22F5
	public static StartScreenFlow Instance
	{
		get
		{
			return StartScreenFlow.s_Instance;
		}
	}

	// Token: 0x0600340B RID: 13323 RVA: 0x000F3EFC File Offset: 0x000F22FC
	private void Awake()
	{
		if (StartScreenFlow.s_Instance != null)
		{
			UnityEngine.Object.Destroy(this);
		}
		else
		{
			StartScreenFlow.s_Instance = this;
		}
		for (int i = 0; i < this.m_PopupData.m_Popups.Count; i++)
		{
			if (this.m_PopupData.m_Popups[i].IsAvailableOnCurrentPlatform())
			{
				this.m_PopupQueue.Enqueue(this.m_PopupData.m_Popups[i]);
			}
		}
		NewContentPopup newContentPopup = this.m_newContentPopup;
		newContentPopup.OnHide = (BaseMenuBehaviour.BaseMenuBehaviourEvent)Delegate.Combine(newContentPopup.OnHide, new BaseMenuBehaviour.BaseMenuBehaviourEvent(this.OnNewContentPopupHide));
	}

	// Token: 0x0600340C RID: 13324 RVA: 0x000F3FAC File Offset: 0x000F23AC
	private void Start()
	{
		this.m_SaveManager = GameUtils.RequireManagerInterface<ISaveManager>();
		this.m_PlayerManager = GameUtils.RequireManager<PlayerManager>();
		this.m_dlcManager = GameUtils.RequireManager<DLCManager>();
		this.SetupFrontendBackgrounds();
		if (!ConnectionStatus.IsInSession() || ConnectionStatus.IsHost())
		{
			ServerGameSetup.Mode = GameMode.OnlineKitchen;
		}
		ServerUserSystem.UnlockEngagement();
		if ((this.m_PlayerManager.HasPlayer() || this.m_PlayerManager.IsEngagingSlot(EngagementSlot.One)) && this.m_SaveManager.GetMetaGameProgress() != null)
		{
			StartScreenFlow.m_bCheckingForEngagement = false;
			this.m_startScreen.SetActive(false);
			this.m_frontend.SetActive(true);
			this.m_PressACanvasObject.SetActive(false);
			MetaGameProgress metaGameProgress = this.m_SaveManager.GetMetaGameProgress();
			if (!this.SwapFrontendBackground(metaGameProgress.GetLastPlayedTheme()))
			{
			}
			this.m_shutterAnim.SetBool("ForceOpen", true);
			this.m_shutterAnim.SetBool("isOpen", true);
			this.m_FrontendFlow.PullBackCamera();
		}
		else
		{
			this.SwapFrontendBackground(SceneDirectoryData.LevelTheme.Null);
			this.ResetToJustStarted(false);
		}
		RichPresenceManager.SetGameMode(GameMode.OnlineKitchen);
		DisconnectionHandler.KickedFromSessionEvent = (GenericVoid)Delegate.Combine(DisconnectionHandler.KickedFromSessionEvent, new GenericVoid(this.OnSessionKicked));
	}

	// Token: 0x0600340D RID: 13325 RVA: 0x000F40E8 File Offset: 0x000F24E8
	protected void ResetToJustStarted(bool resetCamera = true)
	{
		T17EventSystemsManager instance = T17EventSystemsManager.Instance;
		StartScreenFlow.m_bCheckingForEngagement = true;
		this.m_startScreen.SetActive(true);
		this.m_frontend.SetActive(false);
		this.m_PressACanvasObject.SetActive(true);
		this.m_shutterAnim.SetBool("ForceOpen", false);
		this.m_shutterAnim.SetBool("isOpen", false);
		if (resetCamera)
		{
			this.m_FrontendFlow.ResetCamera();
		}
		if (this.m_FrontendFlow != null)
		{
			this.m_FrontendFlow.BlockFocusKitchen = false;
		}
		GameObject gameObject = GameObject.Find("EventSystems");
		if (gameObject != null)
		{
			gameObject.SetActive(true);
		}
		this.m_SaveManager.UnloadProfile();
		AchievementManager achievementManager = GameUtils.RequireManager<AchievementManager>();
		if (achievementManager != null)
		{
			achievementManager.Unload();
		}
		this.m_PlayerManager.DisengagePad(EngagementSlot.One);
		if (instance != null)
		{
			instance.ResetAll();
		}
	}

	// Token: 0x0600340E RID: 13326 RVA: 0x000F41CE File Offset: 0x000F25CE
	private void Update()
	{
		if (StartScreenFlow.m_bCheckingForEngagement)
		{
			this.CheckForEngagement();
		}
		else if (this.m_PressACanvasObject != null)
		{
			this.m_PressACanvasObject.SetActive(false);
		}
	}

	// Token: 0x0600340F RID: 13327 RVA: 0x000F4204 File Offset: 0x000F2604
	private void CheckForEngagement()
	{
		EngagmentCircumstances circumstances;
		ControlPadInput.PadNum engagementPad = this.m_PlayerManager.GetEngagementPad(out circumstances);
		if (engagementPad == ControlPadInput.PadNum.Count)
		{
			return;
		}
		this.m_PlayerManager.StartGameownerEngagement(engagementPad, circumstances, new VoidGeneric<GamepadUser>(this.OnEngagementFinished));
		StartScreenFlow.m_bCheckingForEngagement = false;
	}

	// Token: 0x06003410 RID: 13328 RVA: 0x000F4248 File Offset: 0x000F2648
	private void OnEngagementFinished(GamepadUser _param1)
	{
		if (!this.m_PlayerManager.HasPlayer() || !this.m_PlayerManager.HasSavablePlayer())
		{
		}
		if (this.m_PlayerManager.HasPlayer())
		{
			GameUtils.TriggerAudio(GameOneShotAudioTag.UIPressStart, base.gameObject.layer);
			base.StartCoroutine(this.StartLoadProfile(new CallbackVoid(this.OnLoadProfileComplete)));
		}
		else
		{
			this.ResetToJustStarted(true);
		}
	}

	// Token: 0x06003411 RID: 13329 RVA: 0x000F42C0 File Offset: 0x000F26C0
	private IEnumerator StartLoadProfile(CallbackVoid onComplete)
	{
		GamepadUser user = this.m_PlayerManager.GetUser(EngagementSlot.One);
		if (T17EventSystemsManager.Instance.GetEventSystemForGamepadUser(user) == null)
		{
			T17EventSystemsManager.Instance.AssignFreeEventSystemToGamepadUser(user);
		}
		this.m_SaveManager.DestroyMetaSession();
		IEnumerator<SaveLoadResult?> routine = this.m_SaveManager.LoadProfile(user);
		while (routine.MoveNext())
		{
			yield return null;
		}
		if (routine.Current == SaveLoadResult.Exists)
		{
			if (onComplete != null)
			{
				onComplete();
			}
		}
		else if (routine.Current == SaveLoadResult.NotExist || routine.Current == SaveLoadResult.NoSpace)
		{
			this.m_SaveManager.DestroyMetaSession();
			this.m_SaveManager.CreateMetaSession();
			this.m_SaveManager.SaveMetaProgress(delegate(SaveSystemStatus _status)
			{
				if (_status.Status == SaveSystemStatus.SaveStatus.Complete)
				{
					if (_status.Result != SaveLoadResult.Cancel)
					{
						this.StartCoroutine(this.StartLoadProfile(onComplete));
					}
					else
					{
						this.ResetToJustStarted(true);
					}
				}
			});
		}
		else if (routine.Current == SaveLoadResult.Cancel)
		{
			this.ResetToJustStarted(true);
		}
		yield break;
	}

	// Token: 0x06003412 RID: 13330 RVA: 0x000F42E4 File Offset: 0x000F26E4
	private void OnLoadProfileComplete()
	{
		DisconnectionHandler.KickedFromSessionEvent = (GenericVoid)Delegate.Remove(DisconnectionHandler.KickedFromSessionEvent, new GenericVoid(this.OnSessionKicked));
		if (this.m_PlayerManager.HasPlayer())
		{
			GamepadUser user = this.m_PlayerManager.GetUser(EngagementSlot.One);
			IOption option = GameUtils.GetMetaGameProgress().GetOption(OptionsData.OptionType.HasSetSafeArea);
			if (option.GetOption() == 0 && this.m_safeAreaAdjustmentScreen != null)
			{
				if (!this.m_safeAreaAdjustmentScreen.gameObject.activeSelf)
				{
					this.m_safeAreaAdjustmentScreen.Show(user, null, base.gameObject, true);
					SafeAreaAdjusterMenu safeAreaAdjustmentScreen = this.m_safeAreaAdjustmentScreen;
					safeAreaAdjustmentScreen.OnHide = (BaseMenuBehaviour.BaseMenuBehaviourEvent)Delegate.Combine(safeAreaAdjustmentScreen.OnHide, new BaseMenuBehaviour.BaseMenuBehaviourEvent(delegate(BaseMenuBehaviour A_1)
					{
						GameUtils.TriggerAudio(GameOneShotAudioTag.UIConfirmScreenSize, base.gameObject.layer);
						this.m_frontend.SetActive(true);
						this.m_startScreen.SetActive(false);
						this.m_shutterAnim.SetBool("isOpen", true);
						this.m_FrontendFlow.PullBackCamera();
						base.StartCoroutine(this.ShowOnStartPopups());
					}));
				}
			}
			else
			{
				this.m_frontend.SetActive(true);
				this.m_startScreen.SetActive(false);
				this.m_shutterAnim.SetBool("isOpen", true);
				this.m_FrontendFlow.PullBackCamera();
				base.StartCoroutine(this.ShowOnStartPopups());
			}
		}
		else
		{
			this.ResetToJustStarted(true);
		}
	}

	// Token: 0x06003413 RID: 13331 RVA: 0x000F43FA File Offset: 0x000F27FA
	protected virtual void OnSessionKicked()
	{
	}

	// Token: 0x06003414 RID: 13332 RVA: 0x000F43FC File Offset: 0x000F27FC
	public bool CanCancel()
	{
		return true;
	}

	// Token: 0x06003415 RID: 13333 RVA: 0x000F43FF File Offset: 0x000F27FF
	public void OnCancel()
	{
		if (this.CanCancel())
		{
			GameUtils.TriggerAudio(GameOneShotAudioTag.StartScreenBack, base.gameObject.layer);
			throw new Exception("OnCancel requires implementation - move back?");
		}
	}

	// Token: 0x06003416 RID: 13334 RVA: 0x000F442C File Offset: 0x000F282C
	public virtual void OnDestroy()
	{
		if (StartScreenFlow.s_Instance == this)
		{
			StartScreenFlow.m_bCheckingForEngagement = true;
		}
		DisconnectionHandler.KickedFromSessionEvent = (GenericVoid)Delegate.Remove(DisconnectionHandler.KickedFromSessionEvent, new GenericVoid(this.OnSessionKicked));
		NewContentPopup newContentPopup = this.m_newContentPopup;
		newContentPopup.OnHide = (BaseMenuBehaviour.BaseMenuBehaviourEvent)Delegate.Remove(newContentPopup.OnHide, new BaseMenuBehaviour.BaseMenuBehaviourEvent(this.OnNewContentPopupHide));
	}

	// Token: 0x06003417 RID: 13335 RVA: 0x000F4498 File Offset: 0x000F2898
	private void SetupFrontendBackgrounds()
	{
		this.m_ExtraBackgrounds.Clear();
		StartScreenFlow.ThemeBackgroundCollection[] allData = this.m_Backgrounds.AllData;
		for (int i = 0; i < allData.Length; i++)
		{
			if (allData[i] != null)
			{
				SceneDirectoryData.LevelTheme[] themes = allData[i].Themes;
				StartScreenBackgroundData[] backgrounds = allData[i].Backgrounds;
				if (themes != null && backgrounds != null)
				{
					int num = Mathf.Min(themes.Length, backgrounds.Length);
					for (int j = 0; j < num; j++)
					{
						if (backgrounds[j] != null)
						{
							this.m_ExtraBackgrounds.Add((int)themes[j], backgrounds[j]);
						}
					}
				}
			}
		}
		if (this.m_Camera != null && this.m_Camera.gameObject != null)
		{
			PostProcessingBehaviour postProcessingBehaviour = this.m_Camera.gameObject.RequestComponent<PostProcessingBehaviour>();
			if (postProcessingBehaviour != null)
			{
				this.m_DefaultCameraData.PostProcessing = postProcessingBehaviour.profile;
			}
		}
		this.m_DefaultRenderData.SkyboxMaterial = RenderSettings.skybox;
		this.m_DefaultRenderData.AmbientSource = RenderSettings.ambientMode;
		this.m_DefaultRenderData.SkyboxData.SkyboxColour = RenderSettings.ambientSkyColor;
		this.m_DefaultRenderData.GradientData.SkyColour = RenderSettings.ambientSkyColor;
		this.m_DefaultRenderData.GradientData.EquatorColour = RenderSettings.ambientEquatorColor;
		this.m_DefaultRenderData.GradientData.GroundColour = RenderSettings.ambientGroundColor;
		this.m_DefaultRenderData.ColorData.Colour = RenderSettings.ambientSkyColor;
		this.m_DefaultRenderData.ReflectionSource = RenderSettings.defaultReflectionMode;
		this.m_DefaultRenderData.SkyboxReflectionData.Resolution = RenderSettings.defaultReflectionResolution;
		this.m_DefaultRenderData.SkyboxReflectionData.IntensityMultiplier = RenderSettings.reflectionIntensity;
		this.m_DefaultRenderData.SkyboxReflectionData.Bounces = RenderSettings.reflectionBounces;
		this.m_DefaultRenderData.CustomReflectionData.Cubemap = RenderSettings.customReflection;
		this.m_DefaultRenderData.CustomReflectionData.IntensityMultiplier = RenderSettings.reflectionIntensity;
		this.m_DefaultRenderData.CustomReflectionData.Bounces = RenderSettings.reflectionBounces;
		if (this.m_MusicSource != null && this.m_MusicSource.gameObject != null)
		{
			AudioSource audioSource = this.m_MusicSource.gameObject.RequestComponent<AudioSource>();
			if (audioSource != null)
			{
				this.m_DefaultAudioData.Music = audioSource.clip;
			}
		}
		if (this.m_AmbienceSource != null && this.m_AmbienceSource.gameObject != null)
		{
			this.m_DefaultAudioData.Ambience = this.m_AmbienceSource.clip;
		}
	}

	// Token: 0x06003418 RID: 13336 RVA: 0x000F474C File Offset: 0x000F2B4C
	private bool SwapFrontendBackground(SceneDirectoryData.LevelTheme _theme)
	{
		if (_theme == SceneDirectoryData.LevelTheme.Null)
		{
			return this.LoadDefaultBackgroundArt();
		}
		StartScreenBackgroundData startScreenBackgroundData = null;
		if (this.m_ExtraBackgrounds.TryGetValue((int)_theme, out startScreenBackgroundData))
		{
			bool flag = this.LoadBackgroundArt(startScreenBackgroundData.BackgroundScene);
			flag |= StartScreenBackgroundDataUtils.SwapBackgroundAudio(startScreenBackgroundData.AudioSettings, ref this.m_MusicSource, ref this.m_AmbienceSource, true);
			flag |= this.SwapCameraSettings(startScreenBackgroundData.CameraSettings);
			flag |= StartScreenBackgroundDataUtils.SetRenderData(startScreenBackgroundData.SceneSettings);
			return flag | this.SwapChefHats(startScreenBackgroundData.ChefHat);
		}
		return this.LoadDefaultBackgroundArt();
	}

	// Token: 0x06003419 RID: 13337 RVA: 0x000F47D8 File Offset: 0x000F2BD8
	private bool LoadDefaultBackgroundArt()
	{
		bool flag = this.LoadBackgroundArt(this.m_DefaultBackgroundScene);
		flag |= StartScreenBackgroundDataUtils.SwapBackgroundAudio(this.m_DefaultAudioData, ref this.m_MusicSource, ref this.m_AmbienceSource, true);
		flag |= this.SwapCameraSettings(this.m_DefaultCameraData);
		flag |= StartScreenBackgroundDataUtils.SetRenderData(this.m_DefaultRenderData);
		return flag | this.SwapChefHats(HatMeshVisibility.VisState.Fancy);
	}

	// Token: 0x0600341A RID: 13338 RVA: 0x000F4838 File Offset: 0x000F2C38
	private bool LoadBackgroundArt(string _backgroundScene)
	{
		string assetBundleName = _backgroundScene.ToLower();
		AssetBundleManager.LoadLevel(assetBundleName, _backgroundScene, true);
		return true;
	}

	// Token: 0x0600341B RID: 13339 RVA: 0x000F4858 File Offset: 0x000F2C58
	private bool SwapCameraSettings(SerializedSceneData.CameraData _settings)
	{
		if (this.m_Camera != null && this.m_Camera.gameObject != null)
		{
			PostProcessingBehaviour postProcessingBehaviour = this.m_Camera.gameObject.RequestComponent<PostProcessingBehaviour>();
			if (postProcessingBehaviour != null)
			{
				postProcessingBehaviour.profile = _settings.PostProcessing;
			}
			FogConfig fogConfig = this.m_Camera.gameObject.RequestComponent<FogConfig>();
			if (fogConfig != null)
			{
				fogConfig.m_fogKind = _settings.Fog.m_kind;
				fogConfig.m_fogOffset = _settings.Fog.m_fogOffset;
				fogConfig.m_fogNear = _settings.Fog.m_fogNear;
				fogConfig.m_fogFar = _settings.Fog.m_fogFar;
				fogConfig.m_fogColour = _settings.Fog.m_fogColour;
				fogConfig.ForceUpdate();
			}
			return true;
		}
		return false;
	}

	// Token: 0x0600341C RID: 13340 RVA: 0x000F4930 File Offset: 0x000F2D30
	private bool SwapChefHats(HatMeshVisibility.VisState _hat)
	{
		FrontendPlayerLobby[] componentsInChildren = this.m_FrontendFlow.GetComponentsInChildren<FrontendPlayerLobby>(true);
		if (componentsInChildren.Length > 0)
		{
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				componentsInChildren[i].m_ChefHat = _hat;
			}
			return true;
		}
		return false;
	}

	// Token: 0x0600341D RID: 13341 RVA: 0x000F4974 File Offset: 0x000F2D74
	private IEnumerator ShowOnStartPopups()
	{
		T17EventSystem eventSys = T17EventSystemsManager.Instance.GetEventSystemForEngagementSlot(EngagementSlot.One);
		Suppressor suppressor = null;
		if (eventSys != null)
		{
			suppressor = eventSys.Disable(this);
		}
		yield return null;
		while (T17FrontendFlow.Instance.IsCameraTransitioning())
		{
			yield return null;
		}
		yield return null;
		T17FrontendFlow.Instance.BlockFocusKitchen = true;
		if (suppressor != null)
		{
			suppressor.Release();
		}
		GlobalSave saveGame = GameUtils.GetMetaGameProgress().SaveData;
		long utcNowTicks = DateTime.UtcNow.Ticks;
		while (this.m_PopupQueue.Count != 0)
		{
			if (this.m_currentPopup == null)
			{
				yield return null;
				PopupData popupData = this.m_PopupQueue.Dequeue();
				bool hasSeen = false;
				saveGame.Get(popupData.m_saveGameString, out hasSeen, false);
				if (!hasSeen && utcNowTicks < popupData.m_disableTicks)
				{
					switch (popupData.m_kind)
					{
					case PopupData.Kind.SwitchKitchenTutorial:
						this.m_currentPopup = UnityEngine.Object.Instantiate<GameObject>(popupData.m_prefab, this.m_PopupCanvas.transform);
						break;
					case PopupData.Kind.DLC:
					case PopupData.Kind.Update:
						if (T17FrontendFlow.Instance != null)
						{
							this.m_currentPopup = this.m_newContentPopup.gameObject;
							this.m_newContentPopup.m_popupData = popupData;
							T17FrontendFlow.Instance.m_Rootmenu.OpenFrontendMenu(this.m_newContentPopup);
						}
						break;
					}
					saveGame.Set(popupData.m_saveGameString, true);
				}
			}
			yield return null;
		}
		if (this.m_SaveManager != null)
		{
			this.m_SaveManager.RegisterOnIdle(delegate
			{
				this.m_SaveManager.SaveMetaProgress(null);
			});
		}
		while (this.m_currentPopup != null)
		{
			yield return null;
		}
		T17FrontendFlow.Instance.BlockFocusKitchen = false;
		yield break;
	}

	// Token: 0x0600341E RID: 13342 RVA: 0x000F498F File Offset: 0x000F2D8F
	private void OnNewContentPopupHide(BaseMenuBehaviour menu)
	{
		this.m_currentPopup = null;
	}

	// Token: 0x040029BD RID: 10685
	private static StartScreenFlow s_Instance;

	// Token: 0x040029BE RID: 10686
	[SerializeField]
	private T17FrontendFlow m_FrontendFlow;

	// Token: 0x040029BF RID: 10687
	[SerializeField]
	private GameObject m_PressACanvasObject;

	// Token: 0x040029C0 RID: 10688
	[SerializeField]
	private GameObject m_frontend;

	// Token: 0x040029C1 RID: 10689
	[SerializeField]
	private GameObject m_startScreen;

	// Token: 0x040029C2 RID: 10690
	[SerializeField]
	private SafeAreaAdjusterMenu m_safeAreaAdjustmentScreen;

	// Token: 0x040029C3 RID: 10691
	[SerializeField]
	private Canvas m_PopupCanvas;

	// Token: 0x040029C4 RID: 10692
	[SerializeField]
	private PopupDataScriptableObject m_PopupData;

	// Token: 0x040029C5 RID: 10693
	[SerializeField]
	private NewContentPopup m_newContentPopup;

	// Token: 0x040029C6 RID: 10694
	[Header("Art")]
	[SerializeField]
	private Animator m_shutterAnim;

	// Token: 0x040029C7 RID: 10695
	[SerializeField]
	private Camera m_Camera;

	// Token: 0x040029C8 RID: 10696
	[SerializeField]
	private string m_DefaultBackgroundScene;

	// Token: 0x040029C9 RID: 10697
	[Space]
	[SerializeField]
	private PersistentMusic m_MusicSource;

	// Token: 0x040029CA RID: 10698
	[SerializeField]
	private AudioSource m_AmbienceSource;

	// Token: 0x040029CB RID: 10699
	[Space]
	[SerializeField]
	private StartScreenFlow.DLCSerializedBackgroundData m_Backgrounds = new StartScreenFlow.DLCSerializedBackgroundData();

	// Token: 0x040029CC RID: 10700
	private ISaveManager m_SaveManager;

	// Token: 0x040029CD RID: 10701
	private PlayerManager m_PlayerManager;

	// Token: 0x040029CE RID: 10702
	private DLCManager m_dlcManager;

	// Token: 0x040029CF RID: 10703
	private Dictionary<int, StartScreenBackgroundData> m_ExtraBackgrounds = new Dictionary<int, StartScreenBackgroundData>();

	// Token: 0x040029D0 RID: 10704
	private SerializedSceneData.CameraData m_DefaultCameraData = new SerializedSceneData.CameraData();

	// Token: 0x040029D1 RID: 10705
	private SerializedSceneData.RenderData m_DefaultRenderData = new SerializedSceneData.RenderData();

	// Token: 0x040029D2 RID: 10706
	private SerializedSceneData.AudioData m_DefaultAudioData = new SerializedSceneData.AudioData();

	// Token: 0x040029D3 RID: 10707
	private Queue<PopupData> m_PopupQueue = new Queue<PopupData>();

	// Token: 0x040029D4 RID: 10708
	private GameObject m_currentPopup;

	// Token: 0x040029D5 RID: 10709
	private static bool m_bCheckingForEngagement = true;

	// Token: 0x040029D6 RID: 10710
	private string m_CurrentBackgroundScene;

	// Token: 0x02000A4C RID: 2636
	[Serializable]
	public class ThemeBackgroundCollection
	{
		// Token: 0x040029D7 RID: 10711
		[SerializeField]
		public SceneDirectoryData.LevelTheme[] Themes = new SceneDirectoryData.LevelTheme[0];

		// Token: 0x040029D8 RID: 10712
		[SerializeField]
		public StartScreenBackgroundData[] Backgrounds = new StartScreenBackgroundData[0];
	}

	// Token: 0x02000A4D RID: 2637
	[Serializable]
	private class DLCSerializedBackgroundData : DLCSerializedData<StartScreenFlow.ThemeBackgroundCollection>
	{
	}
}
