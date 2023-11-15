using System;
using System.Collections;
using System.Collections.Generic;
using Team17.Online;
using Team17.Online.Multiplayer.Messaging;
using UnityEngine;
using UnityEngine.SceneManagement;

// Token: 0x02000BE0 RID: 3040
public class ClientWorldMapFlowController : ClientSynchroniserBase
{
	// Token: 0x06003E17 RID: 15895 RVA: 0x00128C04 File Offset: 0x00127004
	public override void StartSynchronising(Component synchronisedObject)
	{
		this.m_allNodes = UnityEngine.Object.FindObjectsOfType<MapNode>();
		this.m_baseObject = (WorldMapFlowController)synchronisedObject;
		for (int i = 0; i < this.m_allNodes.Length; i++)
		{
			this.m_allNodes[i].StartUp();
		}
		this.RegisterPopups();
		if (ConnectionStatus.IsInSession() && !ConnectionStatus.IsHost())
		{
			GameSession gameSession = GameUtils.GetGameSession();
			if (!gameSession.Progress.UseSlaveSlot)
			{
				this.m_savingDisabledIcon = GameUtils.RequireManager<SpinnerIconManager>().Show(SpinnerIconManager.SpinnerIconType.SavingDisabled, this, true);
			}
		}
	}

	// Token: 0x06003E18 RID: 15896 RVA: 0x00128C94 File Offset: 0x00127094
	private IEnumerator StartIntroRoutine(MapNode[] unfoldedMapNodes)
	{
		yield return new WaitForSecondsRealtime(2f);
		this.m_runIntro = this.UnfoldNodesRoutine(unfoldedMapNodes, new VoidGeneric<MapNode>(this.MapNodeUnfolded));
		yield break;
	}

	// Token: 0x06003E19 RID: 15897 RVA: 0x00128CB8 File Offset: 0x001270B8
	private void RegisterPopups()
	{
		if (this.m_baseObject.m_newGamePlusDialogPrefab != null)
		{
			NetworkUtils.RegisterSpawnablePrefab(base.gameObject, this.m_baseObject.m_newGamePlusDialogPrefab.gameObject, new VoidGeneric<GameObject>(this.PopupSpawned));
		}
		if (this.m_baseObject.m_practiceModeDialogPrefab != null)
		{
			NetworkUtils.RegisterSpawnablePrefab(base.gameObject, this.m_baseObject.m_practiceModeDialogPrefab.gameObject, new VoidGeneric<GameObject>(this.PopupSpawned));
		}
		if (this.m_baseObject.m_hordeModeDialogPrefab != null)
		{
			NetworkUtils.RegisterSpawnablePrefab(base.gameObject, this.m_baseObject.m_hordeModeDialogPrefab.gameObject, new VoidGeneric<GameObject>(this.PopupSpawned));
		}
	}

	// Token: 0x06003E1A RID: 15898 RVA: 0x00128D7C File Offset: 0x0012717C
	private void OnGameStateChanged(IOnlineMultiplayerSessionUserId sessionUserId, Serialisable message)
	{
		GameStateMessage gameStateMessage = (GameStateMessage)message;
		GameState state = gameStateMessage.m_State;
		if (state != GameState.RunMapUnfoldRoutine)
		{
			if (state == GameState.InMap)
			{
				this.m_popupRoutine = this.ShowPopupsRoutine();
			}
		}
		else
		{
			GameProgress progress = GameUtils.GetGameSession().Progress;
			int num = progress.SaveData.LastLevelEntered;
			if (num == -1)
			{
				num = progress.SaveData.FarthestProgressedLevel(false, true);
			}
			if (num != -1 && num < progress.GetSceneDirectory().Scenes.Length)
			{
				GameUtils.GetMetaGameProgress().SetLastPlayedTheme(progress.GetSceneDirectory().Scenes[num].Theme);
			}
			MapNode[] unfoldedMapNodes = new MapNode[0];
			PortalMapNode[] array = this.m_allNodes.ConvertAll((MapNode x) => x as PortalMapNode);
			array = array.AllRemoved_Predicate((PortalMapNode x) => x == null);
			for (int i = 0; i < array.Length; i++)
			{
				if (this.m_baseObject.IsLevelUnlocked(array[i]))
				{
					GameProgress.GameProgressData.LevelProgress progress2 = progress.GetProgress(array[i].LevelIndex);
					if (!progress2.Completed && !progress2.Revealed && !DebugManager.Instance.GetOption("Unlock all levels") && !array[i].ForceUnlocked)
					{
						ArrayUtils.PushBack<MapNode>(ref unfoldedMapNodes, array[i]);
					}
					else
					{
						array[i].InstantUnfold();
						array[i].SetAsStatic();
					}
				}
				else
				{
					array[i].SetAsStatic();
				}
			}
			SwitchMapNode[] array2 = this.m_allNodes.ConvertAll((MapNode x) => x as SwitchMapNode);
			array2 = array2.AllRemoved_Predicate((SwitchMapNode x) => x == null);
			for (int j = 0; j < array2.Length; j++)
			{
				if (this.m_baseObject.IsSwitchSwitched(array2[j]))
				{
					array2[j].InstantUnfold();
				}
			}
			TeleportalMapNode[] array3 = this.m_allNodes.ConvertAll((MapNode x) => x as TeleportalMapNode);
			array3 = array3.AllRemoved_Predicate((TeleportalMapNode x) => x == null);
			Array.Sort<TeleportalMapNode>(array3, (TeleportalMapNode x, TeleportalMapNode y) => x.World.CompareTo(y.World));
			foreach (TeleportalMapNode teleportalMapNode in array3)
			{
				if (teleportalMapNode.ShouldFocus())
				{
					ArrayUtils.PushBack<MapNode>(ref unfoldedMapNodes, teleportalMapNode);
				}
				else
				{
					teleportalMapNode.InstantUnfold();
					teleportalMapNode.SetAsStatic();
				}
			}
			base.StartCoroutine(this.StartIntroRoutine(unfoldedMapNodes));
		}
	}

	// Token: 0x06003E1B RID: 15899 RVA: 0x00129084 File Offset: 0x00127484
	protected void PopupSpawned(GameObject _spawned)
	{
		WorldMapInfoPopup item = _spawned.RequireComponent<WorldMapInfoPopup>();
		this.m_popups.Add(item);
		GameObject namedCanvas = GameUtils.GetNamedCanvas("ScalingHUDCanvas");
		if (namedCanvas != null)
		{
			_spawned.transform.SetParent(namedCanvas.transform);
			_spawned.transform.localPosition = Vector3.zero;
		}
		_spawned.SetActive(false);
	}

	// Token: 0x06003E1C RID: 15900 RVA: 0x001290E4 File Offset: 0x001274E4
	private IEnumerator ShowPopupsRoutine()
	{
		if (this.m_popups.Count == 0)
		{
			yield break;
		}
		Camera mainCamera = Camera.main;
		WorldMapCamera worldMapCamera = mainCamera.gameObject.RequireComponent<WorldMapCamera>();
		MapAvatarControls avatarControls = worldMapCamera.AccessAvatar.RequireComponent<MapAvatarControls>();
		for (int i = 0; i < this.m_popups.Count; i++)
		{
			ClientWorldMapInfoPopup clientPopup = this.m_popups[i].gameObject.RequireComponent<ClientWorldMapInfoPopup>();
			if (clientPopup.CanShow())
			{
				worldMapCamera.enabled = false;
				avatarControls.enabled = false;
				IEnumerator popupRoutine = clientPopup.PopupRoutine();
				while (popupRoutine.MoveNext())
				{
					yield return null;
				}
			}
		}
		worldMapCamera.enabled = true;
		avatarControls.enabled = true;
		yield break;
	}

	// Token: 0x06003E1D RID: 15901 RVA: 0x001290FF File Offset: 0x001274FF
	private void Awake()
	{
		this.m_NetworkErrorDialog.Enable(new T17DialogBox.DialogEvent(this.OnNetworkDisconnectionConfirmed));
		Mailbox.Client.RegisterForMessageType(MessageType.GameState, new OrderedMessageReceivedCallback(this.OnGameStateChanged));
		InviteMonitor.SwitchHandlerType(InviteMonitor.HandlerType.Gameplay);
	}

	// Token: 0x06003E1E RID: 15902 RVA: 0x00129138 File Offset: 0x00127538
	private void Start()
	{
		if (!LoadingScreenFlow.IsLoadingStartScreen())
		{
			GameUtils.LoadScene("InGameMenu", LoadSceneMode.Additive);
		}
		GameSession gameSession = GameUtils.GetGameSession();
		if (!ConnectionStatus.IsInSession() || ConnectionStatus.IsHost())
		{
			gameSession.SaveSession(null);
		}
		else if (gameSession.Progress.UseSlaveSlot)
		{
			gameSession.SaveSession(null);
		}
		else
		{
			GameUtils.RequireManager<SaveManager>().SaveMetaProgress(null);
		}
	}

	// Token: 0x06003E1F RID: 15903 RVA: 0x001291A8 File Offset: 0x001275A8
	protected override void OnDestroy()
	{
		base.OnDestroy();
		Mailbox.Client.UnregisterForMessageType(MessageType.GameState, new OrderedMessageReceivedCallback(this.OnGameStateChanged));
		this.m_NetworkErrorDialog.Disable();
		if (this.m_savingDisabledIcon != null)
		{
			this.m_savingDisabledIcon.Release();
			this.m_savingDisabledIcon = null;
		}
	}

	// Token: 0x06003E20 RID: 15904 RVA: 0x001291FC File Offset: 0x001275FC
	public override void UpdateSynchronising()
	{
		if (this.m_runIntro != null && !this.m_runIntro.MoveNext())
		{
			ClientMessenger.GameState(GameState.RanMapUnfoldRoutine);
			this.m_runIntro = null;
		}
		this.m_unfoldRoutines.RemoveAll((IEnumerator x) => !x.MoveNext());
		if (this.m_popupRoutine != null && !this.m_popupRoutine.MoveNext())
		{
			this.m_popupRoutine = null;
		}
	}

	// Token: 0x06003E21 RID: 15905 RVA: 0x00129280 File Offset: 0x00127680
	private void LateUpdate()
	{
		if (this.m_runIntro == null && this.m_allNodes != null && this.m_unfoldRoutines.Count == 0)
		{
			for (int i = 0; i < this.m_allNodes.Length; i++)
			{
				if (!this.m_allNodes[i].IsStatic() && this.m_allNodes[i].IsIdle())
				{
					this.m_allNodes[i].SetAsStatic();
				}
			}
		}
	}

	// Token: 0x06003E22 RID: 15906 RVA: 0x00129300 File Offset: 0x00127700
	private void MapNodeUnfolded(MapNode _node)
	{
		PortalMapNode portalMapNode = _node as PortalMapNode;
		if (portalMapNode != null)
		{
			GameSession gameSession = GameUtils.GetGameSession();
			if (gameSession.Progress != null)
			{
				GameProgress.GameProgressData.LevelProgress progress = gameSession.Progress.GetProgress(portalMapNode.LevelIndex);
				progress.Revealed = true;
			}
			return;
		}
		TeleportalMapNode teleportalMapNode = _node as TeleportalMapNode;
		if (teleportalMapNode != null)
		{
			GameSession gameSession2 = GameUtils.GetGameSession();
			if (gameSession2.Progress != null)
			{
				gameSession2.Progress.RecordTeleportalRevealed(teleportalMapNode.World);
			}
			return;
		}
	}

	// Token: 0x06003E23 RID: 15907 RVA: 0x00129390 File Offset: 0x00127790
	public void UnfoldSwitchMapNode(SwitchMapNode _switch)
	{
		IEnumerator routine = this.UnfoldNodesRoutine(new MapNode[]
		{
			_switch
		}, null);
		base.StartCoroutine(routine);
	}

	// Token: 0x06003E24 RID: 15908 RVA: 0x001293B8 File Offset: 0x001277B8
	private void CalculateTransitionData(float _distance, out float _gradLimit, out float _timeToMax)
	{
		float accelerationTime = this.m_baseObject.m_unfoldSequenceData.AccelerationTime;
		float idealTransitTime = this.m_baseObject.m_unfoldSequenceData.IdealTransitTime;
		float num = Mathf.Min(idealTransitTime / 2f, accelerationTime);
		float num2 = 3.1415927f;
		float num3 = idealTransitTime - num - accelerationTime * Mathf.Sin(num2 * num / accelerationTime) / num2;
		float value = _distance / num3;
		_gradLimit = Mathf.Clamp(value, this.m_baseObject.m_unfoldSequenceData.MinMoveSpeed, this.m_baseObject.m_unfoldSequenceData.MaxMoveSpeed);
		_timeToMax = accelerationTime;
	}

	// Token: 0x06003E25 RID: 15909 RVA: 0x00129444 File Offset: 0x00127844
	private IEnumerator BlendCameraToPosition(Vector3 _position)
	{
		Camera mainCamera = Camera.main;
		float distanceToIdeal = (_position - mainCamera.transform.position).magnitude;
		float currentGradient = 0f;
		float gradLimit;
		float accelTime;
		this.CalculateTransitionData(distanceToIdeal, out gradLimit, out accelTime);
		for (;;)
		{
			distanceToIdeal = (_position - mainCamera.transform.position).magnitude;
			float deltaTime = TimeManager.GetDeltaTime(base.gameObject);
			MathUtils.AdvanceToTarget_Sinusoidal(ref distanceToIdeal, ref currentGradient, 0f, gradLimit, accelTime, deltaTime);
			Vector3 pos = _position - (_position - mainCamera.transform.position).SafeNormalised(Vector3.zero) * distanceToIdeal;
			mainCamera.transform.position = pos;
			if (distanceToIdeal < 0.1f)
			{
				break;
			}
			yield return null;
		}
		yield break;
	}

	// Token: 0x06003E26 RID: 15910 RVA: 0x00129468 File Offset: 0x00127868
	private IEnumerator UnfoldNodesRoutine(MapNode[] _mapNodes, VoidGeneric<MapNode> _onShown = null)
	{
		if (_mapNodes.Length == 0)
		{
			yield break;
		}
		Camera mainCamera = Camera.main;
		WorldMapCamera worldMapCamera = mainCamera.gameObject.RequireComponent<WorldMapCamera>();
		worldMapCamera.enabled = false;
		MapAvatarControls avatarControls = worldMapCamera.AccessAvatar.RequireComponent<MapAvatarControls>();
		avatarControls.enabled = false;
		Vector3 idealOffset = worldMapCamera.AccessIdealOffset;
		foreach (MapNode node in _mapNodes)
		{
			IEnumerator blendTo = this.BlendCameraToPosition(node.transform.position + idealOffset);
			while (blendTo.MoveNext())
			{
				yield return null;
			}
			IEnumerator unfoldRoutine = node.UnfoldFlow();
			this.m_unfoldRoutines.Add(unfoldRoutine);
			IEnumerator wait = CoroutineUtils.TimerRoutine(this.m_baseObject.m_unfoldSequenceData.TimePerNode, base.gameObject.layer);
			while (wait.MoveNext())
			{
				yield return null;
			}
		}
		foreach (MapNode node2 in _mapNodes)
		{
			List<ClientWorldMapInfoPopup.InfoPopupShowRequest> popups = node2.GetPopups();
			for (int k = 0; k < popups.Count; k++)
			{
				ClientWorldMapInfoPopup.InfoPopupShowRequest request = popups[k];
				if (request.m_popup.CanShow())
				{
					IEnumerator blendToPopup = this.BlendCameraToPosition(request.m_requester.position + idealOffset);
					while (blendToPopup.MoveNext())
					{
						yield return null;
					}
					IEnumerator popupRoutine = request.m_popup.PopupRoutine();
					while (popupRoutine.MoveNext())
					{
						yield return null;
					}
				}
			}
		}
		foreach (MapNode param in _mapNodes)
		{
			if (_onShown != null)
			{
				_onShown(param);
			}
		}
		IEnumerator blendBack = this.BlendCameraToPosition(worldMapCamera.GetIdealLocation());
		while (blendBack.MoveNext())
		{
			yield return null;
		}
		worldMapCamera.enabled = true;
		avatarControls.enabled = true;
		yield break;
	}

	// Token: 0x06003E27 RID: 15911 RVA: 0x00129491 File Offset: 0x00127891
	private void OnNetworkDisconnectionConfirmed()
	{
		ServerGameSetup.Mode = GameMode.OnlineKitchen;
		LoadingScreenFlow.LoadScene("StartScreen", GameState.NotSet);
	}

	// Token: 0x040031DC RID: 12764
	private IEnumerator m_popupRoutine;

	// Token: 0x040031DD RID: 12765
	private IEnumerator m_runIntro;

	// Token: 0x040031DE RID: 12766
	private List<IEnumerator> m_unfoldRoutines = new List<IEnumerator>();

	// Token: 0x040031DF RID: 12767
	private MapNode[] m_allNodes;

	// Token: 0x040031E0 RID: 12768
	private WorldMapFlowController m_baseObject;

	// Token: 0x040031E1 RID: 12769
	private NetworkErrorDialog m_NetworkErrorDialog = new NetworkErrorDialog();

	// Token: 0x040031E2 RID: 12770
	private List<WorldMapInfoPopup> m_popups = new List<WorldMapInfoPopup>();

	// Token: 0x040031E3 RID: 12771
	private Suppressor m_savingDisabledIcon;
}
