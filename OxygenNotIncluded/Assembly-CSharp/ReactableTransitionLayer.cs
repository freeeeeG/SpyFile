using System;

// Token: 0x02000534 RID: 1332
public class ReactableTransitionLayer : TransitionDriver.InterruptOverrideLayer
{
	// Token: 0x06001FBF RID: 8127 RVA: 0x000A958D File Offset: 0x000A778D
	public ReactableTransitionLayer(Navigator navigator) : base(navigator)
	{
	}

	// Token: 0x06001FC0 RID: 8128 RVA: 0x000A9596 File Offset: 0x000A7796
	protected override bool IsOverrideComplete()
	{
		return !this.reactionMonitor.IsReacting() && base.IsOverrideComplete();
	}

	// Token: 0x06001FC1 RID: 8129 RVA: 0x000A95B0 File Offset: 0x000A77B0
	public override void BeginTransition(Navigator navigator, Navigator.ActiveTransition transition)
	{
		if (this.reactionMonitor == null)
		{
			this.reactionMonitor = navigator.GetSMI<ReactionMonitor.Instance>();
		}
		this.reactionMonitor.PollForReactables(transition);
		if (this.reactionMonitor.IsReacting())
		{
			base.BeginTransition(navigator, transition);
			transition.start = this.originalTransition.start;
			transition.end = this.originalTransition.end;
		}
	}

	// Token: 0x040011BA RID: 4538
	private ReactionMonitor.Instance reactionMonitor;
}
