using System;
using System.Collections;
using System.Collections.Generic;
using Team17.Online;
using Team17.Online.Multiplayer.Messaging;
using UnityEngine;
using UnityEngine.Playables;

// Token: 0x0200065F RID: 1631
public class ClientCutsceneController : ClientSynchroniserBase
{
	// Token: 0x06001F0F RID: 7951 RVA: 0x00097868 File Offset: 0x00095C68
	public override EntityType GetEntityType()
	{
		return EntityType.Cutscene;
	}

	// Token: 0x06001F10 RID: 7952 RVA: 0x0009786C File Offset: 0x00095C6C
	public override void StartSynchronising(Component synchronisedObject)
	{
		base.StartSynchronising(synchronisedObject);
		this.m_controller = (CutsceneController)synchronisedObject;
		Mailbox.Client.RegisterForMessageType(MessageType.DestroyChef, new OrderedMessageReceivedCallback(this.OnDestroyChefMessageReceived));
	}

	// Token: 0x06001F11 RID: 7953 RVA: 0x0009789C File Offset: 0x00095C9C
	public override void ApplyServerEvent(Serialisable serialisable)
	{
		CutsceneStateMessage cutsceneStateMessage = (CutsceneStateMessage)serialisable;
		if (cutsceneStateMessage != null)
		{
			this.m_skipped = true;
		}
	}

	// Token: 0x06001F12 RID: 7954 RVA: 0x000978BD File Offset: 0x00095CBD
	protected override void OnDestroy()
	{
		Mailbox.Client.UnregisterForMessageType(MessageType.DestroyChef, new OrderedMessageReceivedCallback(this.OnDestroyChefMessageReceived));
		base.OnDestroy();
	}

	// Token: 0x06001F13 RID: 7955 RVA: 0x000978E0 File Offset: 0x00095CE0
	public IEnumerator StartCutscene(CutsceneController.SetupData _setupData)
	{
		if (this.m_controller == null)
		{
			return null;
		}
		this.m_skippable = _setupData.skippable;
		this.m_uiEnabledPostPlayback = _setupData.postplaybackUIEnabled;
		GameObject[] array = GameObject.FindGameObjectsWithTag("Player");
		for (int i = 0; i < array.Length; i++)
		{
			this.m_players.Add(array[i].RequireComponent<PlayerControls>());
		}
		this.m_hudCanvas = GameUtils.GetNamedCanvas("ScalingHUDCanvas").RequireComponent<Canvas>();
		this.m_hoverIconCanvas = GameUtils.GetNamedCanvas("HoverIconCanvas").RequireComponent<Canvas>();
		if (this.m_controller.m_skipUIPrefab != null)
		{
			GameObject skipCanvas = GameUtils.InstantiateUIController(this.m_controller.m_skipUIPrefab, "UICanvas");
			this.m_skipCanvas = skipCanvas;
			this.m_skipCanvas.SetActive(false);
		}
		return this.RunCutscene(this.m_controller.m_director);
	}

	// Token: 0x06001F14 RID: 7956 RVA: 0x000979C4 File Offset: 0x00095DC4
	public void Shutdown()
	{
		if (this.m_skipCanvas != null)
		{
			UnityEngine.Object.Destroy(this.m_skipCanvas.gameObject);
		}
	}

	// Token: 0x06001F15 RID: 7957 RVA: 0x000979E8 File Offset: 0x00095DE8
	public void OnDestroyChefMessageReceived(IOnlineMultiplayerSessionUserId sessionUserId, Serialisable message)
	{
		if (this.m_players != null)
		{
			DestroyChefMessage destroyChefMessage = (DestroyChefMessage)message;
			EntitySerialisationEntry entry = EntitySerialisationRegistry.GetEntry(destroyChefMessage.m_Chef.m_Header.m_uEntityID);
			List<PlayerControls> list = new List<PlayerControls>();
			for (int i = 0; i < this.m_players.Count; i++)
			{
				if (null != this.m_players[i])
				{
					EntitySerialisationEntry entry2 = EntitySerialisationRegistry.GetEntry(this.m_players[i].gameObject);
					if (entry2 != null && entry2.m_Header.m_uEntityID != destroyChefMessage.m_Chef.m_Header.m_uEntityID)
					{
						list.Add(this.m_players[i]);
					}
				}
			}
			this.m_players = list;
		}
	}

	// Token: 0x06001F16 RID: 7958 RVA: 0x00097AB0 File Offset: 0x00095EB0
	private IEnumerator RunCutscene(PlayableDirector _director)
	{
		if (_director != null)
		{
			IEnumerator setupRoutine = this.RunPreCutsceneRoutine(_director);
			while (setupRoutine.MoveNext())
			{
				yield return null;
			}
			IEnumerator directorRoutine = this.RunCutsceneRoutine(_director);
			IEnumerator skipRoutine = this.RunSkipRoutine();
			while (directorRoutine.MoveNext())
			{
				if (ConnectionStatus.IsHost() || !ConnectionStatus.IsInSession())
				{
					if (!this.m_skipped && !skipRoutine.MoveNext())
					{
						this.m_controller.OnCutsceneSkipped();
						this.m_skipped = true;
					}
					yield return null;
				}
				else
				{
					yield return null;
				}
			}
			IEnumerator shutdownRoutine = this.RunPostCutsceneRoutine(_director);
			while (shutdownRoutine.MoveNext())
			{
				yield return null;
			}
		}
		if (this.m_skipCanvas != null)
		{
			UnityEngine.Object.Destroy(this.m_skipCanvas.gameObject);
		}
		yield break;
	}

	// Token: 0x06001F17 RID: 7959 RVA: 0x00097AD4 File Offset: 0x00095ED4
	private IEnumerator RunPreCutsceneRoutine(PlayableDirector _director)
	{
		_director.gameObject.SetActive(true);
		_director.Play();
		_director.Pause();
		yield return null;
		this.m_hudCanvas.enabled = false;
		this.m_hoverIconCanvas.enabled = false;
		if (this.m_controller.m_gameCamera != null)
		{
			this.m_controller.m_gameCamera.gameObject.SetActive(false);
		}
		this.m_controlsSuppressors.Clear();
		for (int i = 0; i < this.m_players.Count; i++)
		{
			this.m_controlsSuppressors.Add(this.m_players[i].Suppress(this));
		}
		for (int j = 0; j < this.m_players.Count; j++)
		{
			this.m_players[j].gameObject.SetActive(false);
		}
		yield break;
	}

	// Token: 0x06001F18 RID: 7960 RVA: 0x00097AF8 File Offset: 0x00095EF8
	private IEnumerator RunCutsceneRoutine(PlayableDirector _director)
	{
		_director.Play();
		PlayableGraph graph = _director.playableGraph;
		while (!this.m_skipped && _director.time < _director.duration && graph.IsValid() && !graph.IsDone())
		{
			yield return null;
		}
		_director.Pause();
		yield break;
	}

	// Token: 0x06001F19 RID: 7961 RVA: 0x00097B1C File Offset: 0x00095F1C
	private IEnumerator RunPostCutsceneRoutine(PlayableDirector _director)
	{
		if (this.m_skipped)
		{
			yield return null;
		}
		_director.Stop();
		_director.gameObject.SetActive(false);
		if (this.m_uiEnabledPostPlayback)
		{
			this.m_hudCanvas.enabled = true;
			this.m_hoverIconCanvas.enabled = true;
		}
		if (this.m_controller.m_gameCamera != null)
		{
			this.m_controller.m_gameCamera.gameObject.SetActive(true);
		}
		for (int i = 0; i < this.m_controlsSuppressors.Count; i++)
		{
			this.m_controlsSuppressors[i].Release();
		}
		this.m_controlsSuppressors.Clear();
		for (int j = 0; j < this.m_players.Count; j++)
		{
			if (null != this.m_players[j])
			{
				this.m_players[j].gameObject.SetActive(true);
			}
		}
		if (this.m_skipped)
		{
			yield return null;
		}
		yield break;
	}

	// Token: 0x06001F1A RID: 7962 RVA: 0x00097B40 File Offset: 0x00095F40
	private IEnumerator RunSkipRoutine()
	{
		if (this.m_skipCanvas != null)
		{
			ILogicalButton startSkip = PlayerInputLookup.GetAnyButton(PlayerInputLookup.LogicalButtonID.UISkip, PadSide.Both);
			while (!startSkip.JustReleased())
			{
				yield return null;
			}
		}
		ILogicalButton rawSkipButton = PlayerInputLookup.GetAnyButton(PlayerInputLookup.LogicalButtonID.UISkip, PadSide.Both);
		GateLogicalButton gatedSkipButton = new GateLogicalButton(rawSkipButton, () => !TimeManager.IsPaused(base.gameObject));
		TimedLogicalButton skipButton = new TimedLogicalButton(gatedSkipButton, TimedLogicalButton.Condition.HeldLonger, 1f);
		if (this.m_skipCanvas != null)
		{
			this.m_skipCanvas.SetActive(true);
			TimedInputUIController timedInputUIController = this.m_skipCanvas.gameObject.RequestComponentRecursive<TimedInputUIController>();
			timedInputUIController.SetDisplayInput(skipButton, 1f);
		}
		bool skipped = false;
		while (!skipped)
		{
			if (this.m_skipCanvas != null)
			{
				if (!this.m_skipCanvas.activeSelf)
				{
					while (!rawSkipButton.IsDown())
					{
						yield return null;
					}
				}
				this.m_skipCanvas.SetActive(true);
				IEnumerator timoutRoutine = null;
				while (!skipped && (timoutRoutine == null || timoutRoutine.MoveNext()))
				{
					if (timoutRoutine == null)
					{
						if (!rawSkipButton.IsDown())
						{
							timoutRoutine = CoroutineUtils.TimerRoutine(2f, LayerMask.NameToLayer("UI"));
						}
					}
					else if (rawSkipButton.IsDown())
					{
						timoutRoutine = null;
					}
					if (skipButton.JustPressed())
					{
						skipped = true;
					}
					yield return null;
				}
				this.m_skipCanvas.SetActive(false);
			}
			else
			{
				while (!skipButton.JustPressed())
				{
					yield return null;
				}
				skipped = true;
			}
		}
		yield break;
	}

	// Token: 0x040017BA RID: 6074
	private CutsceneController m_controller;

	// Token: 0x040017BB RID: 6075
	private List<Suppressor> m_controlsSuppressors = new List<Suppressor>();

	// Token: 0x040017BC RID: 6076
	private List<PlayerControls> m_players = new List<PlayerControls>();

	// Token: 0x040017BD RID: 6077
	private Canvas m_hudCanvas;

	// Token: 0x040017BE RID: 6078
	private Canvas m_hoverIconCanvas;

	// Token: 0x040017BF RID: 6079
	private GameObject m_skipCanvas;

	// Token: 0x040017C0 RID: 6080
	private bool m_skippable;

	// Token: 0x040017C1 RID: 6081
	private bool m_skipped;

	// Token: 0x040017C2 RID: 6082
	private const float c_skipHoldDuration = 1f;

	// Token: 0x040017C3 RID: 6083
	private const float c_skipPromptTimout = 2f;

	// Token: 0x040017C4 RID: 6084
	private bool m_uiEnabledPostPlayback = true;
}
