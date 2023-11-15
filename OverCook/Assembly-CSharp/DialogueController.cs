using System;
using UnityEngine;

// Token: 0x02000A59 RID: 2649
public class DialogueController : MonoBehaviour
{
	// Token: 0x06003450 RID: 13392 RVA: 0x000F60CE File Offset: 0x000F44CE
	public void RegisterDialogueStateCallback(GenericVoid<DialogueController.Dialogue, int> _callback)
	{
		this.m_dialogueStateCallback = (GenericVoid<DialogueController.Dialogue, int>)Delegate.Combine(this.m_dialogueStateCallback, _callback);
	}

	// Token: 0x06003451 RID: 13393 RVA: 0x000F60E7 File Offset: 0x000F44E7
	public void UnRegisterDialogueStateCallback(GenericVoid<DialogueController.Dialogue, int> _callback)
	{
		this.m_dialogueStateCallback = (GenericVoid<DialogueController.Dialogue, int>)Delegate.Remove(this.m_dialogueStateCallback, _callback);
	}

	// Token: 0x06003452 RID: 13394 RVA: 0x000F6100 File Offset: 0x000F4500
	public void OnDialogueStateChanged(DialogueController.Dialogue _dialogue, int _state)
	{
		this.m_dialogueStateCallback(_dialogue, _state);
	}

	// Token: 0x06003453 RID: 13395 RVA: 0x000F6110 File Offset: 0x000F4510
	public static void StartDialogueSFX(DialogueController.DialogueAudio _data, int _layer)
	{
		DialogueController.DialogueAudio.SoundType audioType = _data.AudioType;
		if (audioType != DialogueController.DialogueAudio.SoundType.OneShot)
		{
			if (audioType == DialogueController.DialogueAudio.SoundType.Loop)
			{
				GameUtils.StartAudio(_data.LoopTag, _data, _layer);
			}
		}
		else
		{
			GameUtils.TriggerAudio(_data.OneShotTag, _layer);
		}
	}

	// Token: 0x06003454 RID: 13396 RVA: 0x000F615C File Offset: 0x000F455C
	public static void StopDialogueSFX(DialogueController.DialogueAudio _data)
	{
		DialogueController.DialogueAudio.SoundType audioType = _data.AudioType;
		if (audioType == DialogueController.DialogueAudio.SoundType.Loop)
		{
			GameUtils.StopAudio(_data.LoopTag, _data);
		}
	}

	// Token: 0x040029FA RID: 10746
	private GenericVoid<DialogueController.Dialogue, int> m_dialogueStateCallback = delegate(DialogueController.Dialogue _dialogue, int _state)
	{
	};

	// Token: 0x02000A5A RID: 2650
	[Serializable]
	public class Dialogue
	{
		// Token: 0x040029FC RID: 10748
		[SelfAssignID(true)]
		public int UniqueID;

		// Token: 0x040029FD RID: 10749
		[AssignResource("DialogueUI", Editorbility.Editable)]
		public SpeechDialogueUIController DialogueUIPrefab;

		// Token: 0x040029FE RID: 10750
		public DialogueController.DialogueAudio DialogueAudio = new DialogueController.DialogueAudio();

		// Token: 0x040029FF RID: 10751
		public string[] DialogueScript;
	}

	// Token: 0x02000A5B RID: 2651
	[Serializable]
	public class DialogueAudio
	{
		// Token: 0x04002A00 RID: 10752
		public DialogueController.DialogueAudio.SoundType AudioType;

		// Token: 0x04002A01 RID: 10753
		[HideInInspectorTest("AudioType", DialogueController.DialogueAudio.SoundType.OneShot)]
		public GameOneShotAudioTag OneShotTag = GameOneShotAudioTag.Blank;

		// Token: 0x04002A02 RID: 10754
		[HideInInspectorTest("AudioType", DialogueController.DialogueAudio.SoundType.Loop)]
		public GameLoopingAudioTag LoopTag = GameLoopingAudioTag.COUNT;

		// Token: 0x02000A5C RID: 2652
		public enum SoundType
		{
			// Token: 0x04002A04 RID: 10756
			None,
			// Token: 0x04002A05 RID: 10757
			OneShot,
			// Token: 0x04002A06 RID: 10758
			Loop
		}
	}
}
