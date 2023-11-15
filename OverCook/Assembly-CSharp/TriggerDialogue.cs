using System;
using UnityEngine;

// Token: 0x02000A6A RID: 2666
[RequireComponent(typeof(DialogueController))]
public class TriggerDialogue : MonoBehaviour
{
	// Token: 0x170003B1 RID: 945
	// (set) Token: 0x060034AE RID: 13486 RVA: 0x000F74C8 File Offset: 0x000F58C8
	public string[] DialogueScript
	{
		set
		{
			this.m_dialogue.DialogueScript = value;
		}
	}

	// Token: 0x170003B2 RID: 946
	// (set) Token: 0x060034AF RID: 13487 RVA: 0x000F74D6 File Offset: 0x000F58D6
	public bool AutoStart
	{
		set
		{
			this.m_startOnAwake = value;
		}
	}

	// Token: 0x060034B0 RID: 13488 RVA: 0x000F74DF File Offset: 0x000F58DF
	public void RegisterDialogueFinishedCallback(GenericVoid<DialogueController.Dialogue> _callback)
	{
		this.m_dialogueFinishedCallback = (GenericVoid<DialogueController.Dialogue>)Delegate.Combine(this.m_dialogueFinishedCallback, _callback);
	}

	// Token: 0x060034B1 RID: 13489 RVA: 0x000F74F8 File Offset: 0x000F58F8
	public void UnregisterDialogueFinishedCallback(GenericVoid<DialogueController.Dialogue> _callback)
	{
		this.m_dialogueFinishedCallback = (GenericVoid<DialogueController.Dialogue>)Delegate.Remove(this.m_dialogueFinishedCallback, _callback);
	}

	// Token: 0x060034B2 RID: 13490 RVA: 0x000F7511 File Offset: 0x000F5911
	public void OnDialogueFinished()
	{
		this.m_dialogueFinishedCallback(this.m_dialogue);
	}

	// Token: 0x04002A34 RID: 10804
	[SerializeField]
	public DialogueController.Dialogue m_dialogue;

	// Token: 0x04002A35 RID: 10805
	[Header("Positioning")]
	[SerializeField]
	public Transform m_followObject;

	// Token: 0x04002A36 RID: 10806
	[SerializeField]
	public Vector2 m_anchor = new Vector2(0.5f, 0.5f);

	// Token: 0x04002A37 RID: 10807
	[SerializeField]
	public Vector2 m_pivot = new Vector2(0.5f, 0.5f);

	// Token: 0x04002A38 RID: 10808
	[SerializeField]
	public float m_rotation;

	// Token: 0x04002A39 RID: 10809
	[Header("Settings")]
	[SerializeField]
	public bool m_canMoveDuringDialog;

	// Token: 0x04002A3A RID: 10810
	[SerializeField]
	public bool m_startOnAwake;

	// Token: 0x04002A3B RID: 10811
	[SerializeField]
	public string m_trigger;

	// Token: 0x04002A3C RID: 10812
	[SerializeField]
	public string m_onDialogueEndTrigger;

	// Token: 0x04002A3D RID: 10813
	private GenericVoid<DialogueController.Dialogue> m_dialogueFinishedCallback = delegate(DialogueController.Dialogue _dialogue)
	{
	};
}
