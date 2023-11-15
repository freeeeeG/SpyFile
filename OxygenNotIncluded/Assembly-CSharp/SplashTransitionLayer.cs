using System;
using UnityEngine;

// Token: 0x0200052F RID: 1327
public class SplashTransitionLayer : TransitionDriver.OverrideLayer
{
	// Token: 0x06001FAB RID: 8107 RVA: 0x000A8F04 File Offset: 0x000A7104
	public SplashTransitionLayer(Navigator navigator) : base(navigator)
	{
		this.lastSplashTime = Time.time;
	}

	// Token: 0x06001FAC RID: 8108 RVA: 0x000A8F18 File Offset: 0x000A7118
	private void RefreshSplashes(Navigator navigator, Navigator.ActiveTransition transition)
	{
		if (navigator == null)
		{
			return;
		}
		if (transition.end == NavType.Tube)
		{
			return;
		}
		Vector3 position = navigator.transform.GetPosition();
		if (this.lastSplashTime + 1f < Time.time && Grid.Element[Grid.PosToCell(position)].IsLiquid)
		{
			this.lastSplashTime = Time.time;
			KBatchedAnimController kbatchedAnimController = FXHelpers.CreateEffect("splash_step_kanim", position + new Vector3(0f, 0.75f, -0.1f), null, false, Grid.SceneLayer.Front, false);
			kbatchedAnimController.Play("fx1", KAnim.PlayMode.Once, 1f, 0f);
			kbatchedAnimController.destroyOnAnimComplete = true;
		}
	}

	// Token: 0x06001FAD RID: 8109 RVA: 0x000A8FC0 File Offset: 0x000A71C0
	public override void BeginTransition(Navigator navigator, Navigator.ActiveTransition transition)
	{
		base.BeginTransition(navigator, transition);
		this.RefreshSplashes(navigator, transition);
	}

	// Token: 0x06001FAE RID: 8110 RVA: 0x000A8FD2 File Offset: 0x000A71D2
	public override void UpdateTransition(Navigator navigator, Navigator.ActiveTransition transition)
	{
		base.UpdateTransition(navigator, transition);
		this.RefreshSplashes(navigator, transition);
	}

	// Token: 0x06001FAF RID: 8111 RVA: 0x000A8FE4 File Offset: 0x000A71E4
	public override void EndTransition(Navigator navigator, Navigator.ActiveTransition transition)
	{
		base.EndTransition(navigator, transition);
		this.RefreshSplashes(navigator, transition);
	}

	// Token: 0x040011B5 RID: 4533
	private float lastSplashTime;

	// Token: 0x040011B6 RID: 4534
	private const float SPLASH_INTERVAL = 1f;
}
