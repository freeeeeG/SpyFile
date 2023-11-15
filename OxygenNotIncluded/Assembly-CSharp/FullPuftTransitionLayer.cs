using System;

// Token: 0x02000530 RID: 1328
public class FullPuftTransitionLayer : TransitionDriver.OverrideLayer
{
	// Token: 0x06001FB0 RID: 8112 RVA: 0x000A8FF6 File Offset: 0x000A71F6
	public FullPuftTransitionLayer(Navigator navigator) : base(navigator)
	{
	}

	// Token: 0x06001FB1 RID: 8113 RVA: 0x000A9000 File Offset: 0x000A7200
	public override void BeginTransition(Navigator navigator, Navigator.ActiveTransition transition)
	{
		base.BeginTransition(navigator, transition);
		CreatureCalorieMonitor.Instance smi = navigator.GetSMI<CreatureCalorieMonitor.Instance>();
		if (smi != null && smi.stomach.IsReadyToPoop())
		{
			KAnimControllerBase component = navigator.GetComponent<KBatchedAnimController>();
			string s = HashCache.Get().Get(transition.anim.HashValue) + "_full";
			if (component.HasAnimation(s))
			{
				transition.anim = s;
			}
		}
	}
}
