using System;
using Klei.AI;
using UnityEngine;

// Token: 0x0200040E RID: 1038
public class EmoteReactable : Reactable
{
	// Token: 0x060015DA RID: 5594 RVA: 0x0007332C File Offset: 0x0007152C
	public EmoteReactable(GameObject gameObject, HashedString id, ChoreType chore_type, int range_width = 15, int range_height = 8, float globalCooldown = 0f, float localCooldown = 20f, float lifeSpan = float.PositiveInfinity, float max_initial_delay = 0f) : base(gameObject, id, chore_type, range_width, range_height, true, globalCooldown, localCooldown, lifeSpan, max_initial_delay, ObjectLayer.NumLayers)
	{
	}

	// Token: 0x060015DB RID: 5595 RVA: 0x00073358 File Offset: 0x00071558
	public EmoteReactable SetEmote(Emote emote)
	{
		this.emote = emote;
		return this;
	}

	// Token: 0x060015DC RID: 5596 RVA: 0x00073364 File Offset: 0x00071564
	public EmoteReactable RegisterEmoteStepCallbacks(HashedString stepName, Action<GameObject> startedCb, Action<GameObject> finishedCb)
	{
		if (this.callbackHandles == null)
		{
			this.callbackHandles = new HandleVector<EmoteStep.Callbacks>.Handle[this.emote.StepCount];
		}
		int stepIndex = this.emote.GetStepIndex(stepName);
		this.callbackHandles[stepIndex] = this.emote[stepIndex].RegisterCallbacks(startedCb, finishedCb);
		return this;
	}

	// Token: 0x060015DD RID: 5597 RVA: 0x000733BC File Offset: 0x000715BC
	public EmoteReactable SetExpression(Expression expression)
	{
		this.expression = expression;
		return this;
	}

	// Token: 0x060015DE RID: 5598 RVA: 0x000733C6 File Offset: 0x000715C6
	public EmoteReactable SetThought(Thought thought)
	{
		this.thought = thought;
		return this;
	}

	// Token: 0x060015DF RID: 5599 RVA: 0x000733D0 File Offset: 0x000715D0
	public EmoteReactable SetOverideAnimSet(string animSet)
	{
		this.overrideAnimSet = Assets.GetAnim(animSet);
		return this;
	}

	// Token: 0x060015E0 RID: 5600 RVA: 0x000733E4 File Offset: 0x000715E4
	public override bool InternalCanBegin(GameObject new_reactor, Navigator.ActiveTransition transition)
	{
		if (this.reactor != null || new_reactor == null)
		{
			return false;
		}
		Navigator component = new_reactor.GetComponent<Navigator>();
		return !(component == null) && component.IsMoving() && (-257 & 1 << (int)component.CurrentNavType) != 0 && this.gameObject != new_reactor;
	}

	// Token: 0x060015E1 RID: 5601 RVA: 0x00073448 File Offset: 0x00071648
	public override void Update(float dt)
	{
		if (this.emote == null || !this.emote.IsValidStep(this.currentStep))
		{
			return;
		}
		if (this.gameObject != null && this.reactor != null)
		{
			Facing component = this.reactor.GetComponent<Facing>();
			if (component != null)
			{
				component.Face(this.gameObject.transform.GetPosition());
			}
		}
		float timeout = this.emote[this.currentStep].timeout;
		if (timeout > 0f && timeout < this.elapsed)
		{
			this.NextStep(null);
			return;
		}
		this.elapsed += dt;
	}

	// Token: 0x060015E2 RID: 5602 RVA: 0x000734FC File Offset: 0x000716FC
	protected override void InternalBegin()
	{
		this.kbac = this.reactor.GetComponent<KBatchedAnimController>();
		this.emote.ApplyAnimOverrides(this.kbac, this.overrideAnimSet);
		if (this.expression != null)
		{
			this.reactor.GetComponent<FaceGraph>().AddExpression(this.expression);
		}
		if (this.thought != null)
		{
			this.reactor.GetSMI<ThoughtGraph.Instance>().AddThought(this.thought);
		}
		this.NextStep(null);
	}

	// Token: 0x060015E3 RID: 5603 RVA: 0x0007357C File Offset: 0x0007177C
	protected override void InternalEnd()
	{
		if (this.kbac != null)
		{
			this.kbac.onAnimComplete -= this.NextStep;
			this.emote.RemoveAnimOverrides(this.kbac, this.overrideAnimSet);
			this.kbac = null;
		}
		if (this.reactor != null)
		{
			if (this.expression != null)
			{
				this.reactor.GetComponent<FaceGraph>().RemoveExpression(this.expression);
			}
			if (this.thought != null)
			{
				this.reactor.GetSMI<ThoughtGraph.Instance>().RemoveThought(this.thought);
			}
		}
		this.currentStep = -1;
	}

	// Token: 0x060015E4 RID: 5604 RVA: 0x00073620 File Offset: 0x00071820
	protected override void InternalCleanup()
	{
		if (this.emote == null || this.callbackHandles == null)
		{
			return;
		}
		int num = 0;
		while (this.emote.IsValidStep(num))
		{
			this.emote[num].UnregisterCallbacks(this.callbackHandles[num]);
			num++;
		}
	}

	// Token: 0x060015E5 RID: 5605 RVA: 0x00073674 File Offset: 0x00071874
	private void NextStep(HashedString finishedAnim)
	{
		if (this.emote.IsValidStep(this.currentStep) && this.emote[this.currentStep].timeout <= 0f)
		{
			this.kbac.onAnimComplete -= this.NextStep;
			if (this.callbackHandles != null)
			{
				this.emote[this.currentStep].OnStepFinished(this.callbackHandles[this.currentStep], this.reactor);
			}
		}
		this.currentStep++;
		if (!this.emote.IsValidStep(this.currentStep) || this.kbac == null)
		{
			base.End();
			return;
		}
		EmoteStep emoteStep = this.emote[this.currentStep];
		if (emoteStep.anim != HashedString.Invalid)
		{
			this.kbac.Play(emoteStep.anim, emoteStep.mode, 1f, 0f);
			if (this.kbac.IsStopped())
			{
				emoteStep.timeout = 0.25f;
			}
		}
		if (emoteStep.timeout <= 0f)
		{
			this.kbac.onAnimComplete += this.NextStep;
		}
		else
		{
			this.elapsed = 0f;
		}
		if (this.callbackHandles != null)
		{
			emoteStep.OnStepStarted(this.callbackHandles[this.currentStep], this.reactor);
		}
	}

	// Token: 0x04000C31 RID: 3121
	private KBatchedAnimController kbac;

	// Token: 0x04000C32 RID: 3122
	public Expression expression;

	// Token: 0x04000C33 RID: 3123
	public Thought thought;

	// Token: 0x04000C34 RID: 3124
	public Emote emote;

	// Token: 0x04000C35 RID: 3125
	private HandleVector<EmoteStep.Callbacks>.Handle[] callbackHandles;

	// Token: 0x04000C36 RID: 3126
	protected KAnimFile overrideAnimSet;

	// Token: 0x04000C37 RID: 3127
	private int currentStep = -1;

	// Token: 0x04000C38 RID: 3128
	private float elapsed;
}
