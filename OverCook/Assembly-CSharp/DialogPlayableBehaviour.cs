using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Playables;

// Token: 0x02000665 RID: 1637
public class DialogPlayableBehaviour : PlayableBehaviour
{
	// Token: 0x17000279 RID: 633
	// (set) Token: 0x06001F32 RID: 7986 RVA: 0x000989E7 File Offset: 0x00096DE7
	public DialogMixerBehaviour Mixer
	{
		set
		{
			this.m_mixer = value;
		}
	}

	// Token: 0x06001F33 RID: 7987 RVA: 0x000989F0 File Offset: 0x00096DF0
	public void Setup(DialogueController.Dialogue _dialogue, Transform _followTarget)
	{
		this.m_dialogue = _dialogue;
		this.m_followTarget = _followTarget;
	}

	// Token: 0x06001F34 RID: 7988 RVA: 0x00098A00 File Offset: 0x00096E00
	public void Setup(DialogueController.Dialogue _dialogue, Vector2 _anchor, Vector2 _pivot, float _rotation = 0f)
	{
		this.m_dialogue = _dialogue;
		this.m_anchor = _anchor;
		this.m_pivot = _pivot;
		this.m_rotation = _rotation;
	}

	// Token: 0x06001F35 RID: 7989 RVA: 0x00098A1F File Offset: 0x00096E1F
	public override void OnBehaviourPlay(Playable playable, FrameData info)
	{
		base.OnBehaviourPlay(playable, info);
		if (this.m_mixer == null || !this.m_mixer.HasPendingLoop(this))
		{
			this.m_mixer.CompletePendingLoop(this);
			this.OnLoopStarted(playable);
		}
	}

	// Token: 0x06001F36 RID: 7990 RVA: 0x00098A58 File Offset: 0x00096E58
	public override void OnBehaviourPause(Playable playable, FrameData info)
	{
		base.OnBehaviourPause(playable, info);
		if (this.m_mixer == null || !this.m_mixer.HasPendingLoop(this))
		{
			this.OnLoopEnded(playable);
		}
	}

	// Token: 0x06001F37 RID: 7991 RVA: 0x00098A88 File Offset: 0x00096E88
	public void OnLoopStarted(Playable playable)
	{
		if (this.m_dialogue == null || this.m_dialogue.DialogueUIPrefab == null || this.m_dialogue.DialogueScript == null || this.m_dialogue.DialogueScript.Length == 0)
		{
			return;
		}
		if (this.m_controller == null)
		{
			PlayableDirector playableDirector = playable.GetGraph<Playable>().GetResolver() as PlayableDirector;
			this.m_controller = playableDirector.gameObject.RequestComponent<ClientDialogueController>();
			if (this.m_controller == null)
			{
				return;
			}
		}
		this.m_finished = false;
		if (this.m_followTarget != null)
		{
			this.m_dialogueRoutine = this.m_controller.StartDialogue(this.m_dialogue, this.m_followTarget);
		}
		else
		{
			this.m_dialogueRoutine = this.m_controller.StartDialogue(this.m_dialogue, this.m_anchor, this.m_pivot, this.m_rotation);
		}
	}

	// Token: 0x06001F38 RID: 7992 RVA: 0x00098B84 File Offset: 0x00096F84
	public void OnLoopEnded(Playable playable)
	{
		if (this.m_controller != null)
		{
			this.m_controller.Shutdown(this.m_dialogue);
			this.m_dialogueRoutine = null;
		}
	}

	// Token: 0x06001F39 RID: 7993 RVA: 0x00098BAF File Offset: 0x00096FAF
	public bool IsLoopActive()
	{
		return !this.m_finished;
	}

	// Token: 0x06001F3A RID: 7994 RVA: 0x00098BBA File Offset: 0x00096FBA
	public override void ProcessFrame(Playable playable, FrameData info, object playerData)
	{
		if (this.m_dialogueRoutine != null && !this.m_dialogueRoutine.MoveNext())
		{
			this.m_finished = true;
		}
	}

	// Token: 0x040017D8 RID: 6104
	private DialogueController.Dialogue m_dialogue;

	// Token: 0x040017D9 RID: 6105
	private Transform m_followTarget;

	// Token: 0x040017DA RID: 6106
	private Vector2 m_anchor = Vector2.zero;

	// Token: 0x040017DB RID: 6107
	private Vector2 m_pivot = Vector2.zero;

	// Token: 0x040017DC RID: 6108
	private float m_rotation;

	// Token: 0x040017DD RID: 6109
	private DialogMixerBehaviour m_mixer;

	// Token: 0x040017DE RID: 6110
	private ClientDialogueController m_controller;

	// Token: 0x040017DF RID: 6111
	private IEnumerator m_dialogueRoutine;

	// Token: 0x040017E0 RID: 6112
	private bool m_finished;
}
