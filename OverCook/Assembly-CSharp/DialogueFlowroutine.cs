using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200067F RID: 1663
[Serializable]
public class DialogueFlowroutine
{
	// Token: 0x06001FEC RID: 8172 RVA: 0x0009B447 File Offset: 0x00099847
	public void OnValidate()
	{
		if (!this.ReceivesInput)
		{
			Array.Resize<string>(ref this.DialogueScript, 1);
		}
	}

	// Token: 0x06001FED RID: 8173 RVA: 0x0009B460 File Offset: 0x00099860
	public void OnEnable()
	{
		if (this.m_dialogueObject != null)
		{
			this.m_dialogueObject.SetActive(true);
		}
	}

	// Token: 0x06001FEE RID: 8174 RVA: 0x0009B47F File Offset: 0x0009987F
	public void OnDisable()
	{
		if (this.m_dialogueObject != null)
		{
			this.m_dialogueObject.SetActive(false);
		}
	}

	// Token: 0x06001FEF RID: 8175 RVA: 0x0009B4A0 File Offset: 0x000998A0
	public void Setup(Transform _followParent)
	{
		this.m_creatorFunction = ((string _text) => this.CreateDialogue(_text, _followParent));
	}

	// Token: 0x06001FF0 RID: 8176 RVA: 0x0009B4D4 File Offset: 0x000998D4
	public void Setup(Vector2 _anchor, Vector2 _pivot)
	{
		this.m_creatorFunction = ((string _text) => this.CreateDialogue(_text, _anchor, _pivot));
	}

	// Token: 0x06001FF1 RID: 8177 RVA: 0x0009B510 File Offset: 0x00099910
	private SpeechDialogueUIController CreateDialogue(string _text, Transform _followParent)
	{
		GameObject obj = GameUtils.InstantiateHoverIconUIController(this.DialogueUIPrefab.gameObject, _followParent, "HoverIconCanvas", default(Vector3));
		SpeechDialogueUIController speechDialogueUIController = obj.RequireComponent<SpeechDialogueUIController>();
		speechDialogueUIController.Setup(_text, true);
		return speechDialogueUIController;
	}

	// Token: 0x06001FF2 RID: 8178 RVA: 0x0009B550 File Offset: 0x00099950
	private SpeechDialogueUIController CreateDialogue(string _text, Vector2 _anchor, Vector2 _pivot)
	{
		GameObject obj = GameUtils.InstantiateUIController(this.DialogueUIPrefab.gameObject, "UICanvas");
		RectTransform rectTransform = obj.RequireComponent<RectTransform>();
		rectTransform.anchorMin = _anchor;
		rectTransform.anchorMax = _anchor;
		rectTransform.pivot = _pivot;
		SpeechDialogueUIController speechDialogueUIController = obj.RequireComponent<SpeechDialogueUIController>();
		speechDialogueUIController.Setup(_text, true);
		return speechDialogueUIController;
	}

	// Token: 0x06001FF3 RID: 8179 RVA: 0x0009B5A0 File Offset: 0x000999A0
	public IEnumerator Run()
	{
		ILogicalButton skipDialogue = PlayerInputLookup.GetAnyButton(PlayerInputLookup.LogicalButtonID.UISelectNotStart, PadSide.Both);
		int i = 0;
		while (i < this.DialogueScript.Length)
		{
			SpeechDialogueUIController controller = this.m_creatorFunction(this.DialogueScript[i]);
			this.m_dialogueObject = controller.gameObject;
			if (this.ReceivesInput)
			{
				skipDialogue.ClaimPressEvent();
				if (!this.m_ignoreInput)
				{
					GameObject gameObject = this.m_dialogueObject.RequireChild("Icon");
					gameObject.SetActive(true);
				}
				DialogueController.StartDialogueSFX(this.DialogueAudio, LayerMask.NameToLayer("Default"));
				while (this.m_ignoreInput || ((!skipDialogue.JustPressed() || !this.m_dialogueObject.activeInHierarchy) && controller.IsPrinting()))
				{
					if (this.m_moveToNextDialog)
					{
						break;
					}
					yield return null;
				}
				if (this.m_dialogueObject.activeInHierarchy)
				{
					controller.SkipPrinting();
				}
				DialogueController.StopDialogueSFX(this.DialogueAudio);
				while (!skipDialogue.JustPressed() || !this.m_dialogueObject.activeInHierarchy)
				{
					if (this.m_moveToNextDialog)
					{
						break;
					}
					yield return null;
				}
				this.m_moveToNextDialog = false;
				UnityEngine.Object.Destroy(this.m_dialogueObject);
				this.m_dialogueObject = null;
				i++;
			}
			else
			{
				DialogueController.StartDialogueSFX(this.DialogueAudio, LayerMask.NameToLayer("Default"));
				while (this.m_dialogueObject.activeInHierarchy && controller.IsPrinting())
				{
					yield return null;
				}
				DialogueController.StopDialogueSFX(this.DialogueAudio);
				for (;;)
				{
					yield return null;
				}
			}
		}
		yield break;
	}

	// Token: 0x06001FF4 RID: 8180 RVA: 0x0009B5BB File Offset: 0x000999BB
	public void IgnoreInput(bool _ignoreInput)
	{
		this.m_ignoreInput = _ignoreInput;
	}

	// Token: 0x06001FF5 RID: 8181 RVA: 0x0009B5C4 File Offset: 0x000999C4
	public void NextDialog()
	{
		this.m_moveToNextDialog = true;
	}

	// Token: 0x06001FF6 RID: 8182 RVA: 0x0009B5CD File Offset: 0x000999CD
	public void Shutdown()
	{
		DialogueController.StopDialogueSFX(this.DialogueAudio);
		if (this.m_dialogueObject)
		{
			UnityEngine.Object.Destroy(this.m_dialogueObject);
			this.m_dialogueObject = null;
		}
	}

	// Token: 0x04001849 RID: 6217
	[AssignResource("DialogueUI", Editorbility.Editable)]
	public SpeechDialogueUIController DialogueUIPrefab;

	// Token: 0x0400184A RID: 6218
	public bool ReceivesInput = true;

	// Token: 0x0400184B RID: 6219
	public DialogueController.DialogueAudio DialogueAudio;

	// Token: 0x0400184C RID: 6220
	public string[] DialogueScript;

	// Token: 0x0400184D RID: 6221
	private Generic<SpeechDialogueUIController, string> m_creatorFunction;

	// Token: 0x0400184E RID: 6222
	private GameObject m_dialogueObject;

	// Token: 0x0400184F RID: 6223
	private bool m_moveToNextDialog;

	// Token: 0x04001850 RID: 6224
	private bool m_ignoreInput;
}
