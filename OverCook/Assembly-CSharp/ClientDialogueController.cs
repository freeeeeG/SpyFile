using System;
using System.Collections;
using System.Collections.Generic;
using Team17.Online.Multiplayer.Messaging;
using UnityEngine;

// Token: 0x02000A56 RID: 2646
public class ClientDialogueController : ClientSynchroniserBase
{
	// Token: 0x06003443 RID: 13379 RVA: 0x000F56E8 File Offset: 0x000F3AE8
	public override EntityType GetEntityType()
	{
		return EntityType.Dialogue;
	}

	// Token: 0x06003444 RID: 13380 RVA: 0x000F56EC File Offset: 0x000F3AEC
	public override void StartSynchronising(Component synchronisedObject)
	{
		base.StartSynchronising(synchronisedObject);
		this.m_dialogueController = (DialogueController)synchronisedObject;
	}

	// Token: 0x06003445 RID: 13381 RVA: 0x000F5704 File Offset: 0x000F3B04
	public override void ApplyServerEvent(Serialisable serialisable)
	{
		DialogueStateMessage data = (DialogueStateMessage)serialisable;
		List<ClientDialogueController.DialogueInstance> list = this.m_instances.FindAll((ClientDialogueController.DialogueInstance x) => x.Dialogue.UniqueID == data.m_dialogueID);
		if (list.Count > 0)
		{
			for (int i = 0; i < list.Count; i++)
			{
				list[i].CurrentIndex = data.m_state;
			}
		}
		else
		{
			ClientDialogueController.PendingInstance pendingInstance = this.m_pendingData.Find((ClientDialogueController.PendingInstance x) => x.DialogueID == data.m_dialogueID);
			if (pendingInstance != null)
			{
				pendingInstance.Index = data.m_state;
			}
			else
			{
				pendingInstance = new ClientDialogueController.PendingInstance();
				pendingInstance.DialogueID = data.m_dialogueID;
				pendingInstance.Index = data.m_state;
				this.m_pendingData.Add(pendingInstance);
			}
		}
	}

	// Token: 0x06003446 RID: 13382 RVA: 0x000F57E4 File Offset: 0x000F3BE4
	public IEnumerator StartDialogue(DialogueController.Dialogue _dialogue, Transform _followTarget)
	{
		ClientDialogueController.DialogueInstance dialogueInstance = new ClientDialogueController.DialogueInstance();
		dialogueInstance.Dialogue = _dialogue;
		dialogueInstance.CreatorFunction = ((string _text) => this.CreateDialogue(_dialogue.DialogueUIPrefab, _text, _followTarget));
		this.m_instances.Add(dialogueInstance);
		return this.RunDialogueRoutine(dialogueInstance);
	}

	// Token: 0x06003447 RID: 13383 RVA: 0x000F5844 File Offset: 0x000F3C44
	public IEnumerator StartDialogue(DialogueController.Dialogue _dialogue, Vector2 _anchor, Vector2 _pivot, float _rotation = 0f)
	{
		ClientDialogueController.DialogueInstance dialogueInstance = new ClientDialogueController.DialogueInstance();
		dialogueInstance.Dialogue = _dialogue;
		dialogueInstance.CreatorFunction = ((string _text) => this.CreateDialogue(_dialogue.DialogueUIPrefab, _text, _anchor, _pivot, _rotation));
		this.m_instances.Add(dialogueInstance);
		return this.RunDialogueRoutine(dialogueInstance);
	}

	// Token: 0x06003448 RID: 13384 RVA: 0x000F58B4 File Offset: 0x000F3CB4
	public void Shutdown(DialogueController.Dialogue _dialogue)
	{
		List<ClientDialogueController.DialogueInstance> list = this.m_instances.FindAll((ClientDialogueController.DialogueInstance x) => x.Dialogue.UniqueID == _dialogue.UniqueID);
		for (int i = 0; i < list.Count; i++)
		{
			if (list[i].Dialogue != null)
			{
				DialogueController.StopDialogueSFX(list[i].Dialogue.DialogueAudio);
			}
			if (list[i].UIObject != null)
			{
				UnityEngine.Object.Destroy(list[i].UIObject);
				list[i].UIObject = null;
			}
		}
		this.m_instances.RemoveAll((ClientDialogueController.DialogueInstance x) => x.Dialogue.UniqueID == _dialogue.UniqueID);
		this.m_pendingData.RemoveAll((ClientDialogueController.PendingInstance x) => x.DialogueID == _dialogue.UniqueID);
	}

	// Token: 0x06003449 RID: 13385 RVA: 0x000F598C File Offset: 0x000F3D8C
	private SpeechDialogueUIController CreateDialogue(SpeechDialogueUIController _uiPrefab, string _text, Transform _followParent)
	{
		GameObject obj = GameUtils.InstantiateHoverIconUIController(_uiPrefab.gameObject, _followParent, "HoverIconCanvas", default(Vector3));
		SpeechDialogueUIController speechDialogueUIController = obj.RequireComponent<SpeechDialogueUIController>();
		speechDialogueUIController.Setup(_text, true);
		return speechDialogueUIController;
	}

	// Token: 0x0600344A RID: 13386 RVA: 0x000F59C4 File Offset: 0x000F3DC4
	private SpeechDialogueUIController CreateDialogue(SpeechDialogueUIController _uiPrefab, string _text, Vector2 _anchor, Vector2 _pivot, float _rotation)
	{
		GameObject obj = GameUtils.InstantiateUIController(_uiPrefab.gameObject, "UICanvas");
		RectTransform rectTransform = obj.RequireComponent<RectTransform>();
		rectTransform.anchorMin = _anchor;
		rectTransform.anchorMax = _anchor;
		rectTransform.pivot = _pivot;
		rectTransform.rotation = Quaternion.Euler(0f, 0f, _rotation);
		SpeechDialogueUIController speechDialogueUIController = obj.RequireComponent<SpeechDialogueUIController>();
		speechDialogueUIController.Setup(_text, true);
		return speechDialogueUIController;
	}

	// Token: 0x0600344B RID: 13387 RVA: 0x000F5A28 File Offset: 0x000F3E28
	private IEnumerator RunDialogueRoutine(ClientDialogueController.DialogueInstance _instance)
	{
		ClientDialogueController.PendingInstance pending = this.m_pendingData.Find((ClientDialogueController.PendingInstance x) => x.DialogueID == _instance.Dialogue.UniqueID);
		if (pending != null)
		{
			_instance.CurrentIndex = pending.Index;
			this.m_pendingData.RemoveAll((ClientDialogueController.PendingInstance x) => x.DialogueID == _instance.Dialogue.UniqueID);
			if (_instance.CurrentIndex == _instance.Dialogue.DialogueScript.Length)
			{
				yield break;
			}
		}
		ILogicalButton skipDialogue = PlayerInputLookup.GetAnyButton(PlayerInputLookup.LogicalButtonID.UISelectNotStart, PadSide.Both);
		for (int i = 0; i < _instance.Dialogue.DialogueScript.Length; i++)
		{
			skipDialogue.ClaimReleaseEvent();
			string text = _instance.Dialogue.DialogueScript[_instance.CurrentIndex];
			SpeechDialogueUIController controller = _instance.CreatorFunction(text);
			_instance.UIObject = controller.gameObject;
			if (ConnectionStatus.IsHost() || !ConnectionStatus.IsInSession())
			{
				GameObject icon = _instance.UIObject.RequireChild("Icon");
				icon.SetActive(true);
				DialogueController.StartDialogueSFX(_instance.Dialogue.DialogueAudio, base.gameObject.layer);
				IEnumerator networkTimeout = CoroutineUtils.TimerRoutine(10f, LayerMask.NameToLayer("UI"));
				while (controller.IsPrinting() && (!this.ShouldSkipDialogue(skipDialogue) || !_instance.UIObject.activeInHierarchy))
				{
					if (!networkTimeout.MoveNext())
					{
						break;
					}
					yield return null;
				}
				if (_instance.UIObject.activeInHierarchy)
				{
					controller.SkipPrinting();
				}
				DialogueController.StopDialogueSFX(_instance.Dialogue.DialogueAudio);
				while (!this.ShouldSkipDialogue(skipDialogue) || !_instance.UIObject.activeInHierarchy)
				{
					if (!networkTimeout.MoveNext())
					{
						break;
					}
					yield return null;
				}
				this.m_dialogueController.OnDialogueStateChanged(_instance.Dialogue, _instance.CurrentIndex + 1);
				while (_instance.CurrentIndex == i)
				{
					yield return null;
				}
			}
			else
			{
				DialogueController.StartDialogueSFX(_instance.Dialogue.DialogueAudio, base.gameObject.layer);
				while (controller.IsPrinting() && _instance.CurrentIndex == i)
				{
					yield return null;
				}
				DialogueController.StopDialogueSFX(_instance.Dialogue.DialogueAudio);
				while (_instance.CurrentIndex == i)
				{
					yield return null;
				}
			}
			UnityEngine.Object.Destroy(_instance.UIObject);
			_instance.UIObject = null;
		}
		yield break;
	}

	// Token: 0x0600344C RID: 13388 RVA: 0x000F5A4A File Offset: 0x000F3E4A
	private bool ShouldSkipDialogue(ILogicalButton skipDialogue)
	{
		return skipDialogue.JustPressed() && (T17InGameFlow.Instance == null || !T17InGameFlow.Instance.WasPauseMenuOpen);
	}

	// Token: 0x040029F0 RID: 10736
	private DialogueController m_dialogueController;

	// Token: 0x040029F1 RID: 10737
	private List<ClientDialogueController.PendingInstance> m_pendingData = new List<ClientDialogueController.PendingInstance>();

	// Token: 0x040029F2 RID: 10738
	private List<ClientDialogueController.DialogueInstance> m_instances = new List<ClientDialogueController.DialogueInstance>();

	// Token: 0x040029F3 RID: 10739
	private const float c_autoAdvanceTime = 10f;

	// Token: 0x02000A57 RID: 2647
	public class PendingInstance
	{
		// Token: 0x040029F4 RID: 10740
		public int DialogueID;

		// Token: 0x040029F5 RID: 10741
		public int Index;
	}

	// Token: 0x02000A58 RID: 2648
	public class DialogueInstance
	{
		// Token: 0x040029F6 RID: 10742
		public DialogueController.Dialogue Dialogue;

		// Token: 0x040029F7 RID: 10743
		public Generic<SpeechDialogueUIController, string> CreatorFunction;

		// Token: 0x040029F8 RID: 10744
		public GameObject UIObject;

		// Token: 0x040029F9 RID: 10745
		public int CurrentIndex;
	}
}
