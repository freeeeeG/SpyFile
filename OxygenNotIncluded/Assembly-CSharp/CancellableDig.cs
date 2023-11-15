using System;

// Token: 0x0200048A RID: 1162
[SkipSaveFileSerialization]
public class CancellableDig : Cancellable
{
	// Token: 0x060019F0 RID: 6640 RVA: 0x000896D8 File Offset: 0x000878D8
	protected override void OnCancel(object data)
	{
		if (data != null && (bool)data)
		{
			this.OnAnimationDone("ScaleDown");
			return;
		}
		EasingAnimations componentInChildren = base.GetComponentInChildren<EasingAnimations>();
		int num = Grid.PosToCell(this);
		if (componentInChildren.IsPlaying && Grid.Element[num].hardness == 255)
		{
			EasingAnimations easingAnimations = componentInChildren;
			easingAnimations.OnAnimationDone = (Action<string>)Delegate.Combine(easingAnimations.OnAnimationDone, new Action<string>(this.DoCancelAnim));
			return;
		}
		EasingAnimations easingAnimations2 = componentInChildren;
		easingAnimations2.OnAnimationDone = (Action<string>)Delegate.Combine(easingAnimations2.OnAnimationDone, new Action<string>(this.OnAnimationDone));
		componentInChildren.PlayAnimation("ScaleDown", 0.1f);
	}

	// Token: 0x060019F1 RID: 6641 RVA: 0x00089780 File Offset: 0x00087980
	private void DoCancelAnim(string animName)
	{
		EasingAnimations componentInChildren = base.GetComponentInChildren<EasingAnimations>();
		componentInChildren.OnAnimationDone = (Action<string>)Delegate.Remove(componentInChildren.OnAnimationDone, new Action<string>(this.DoCancelAnim));
		componentInChildren.OnAnimationDone = (Action<string>)Delegate.Combine(componentInChildren.OnAnimationDone, new Action<string>(this.OnAnimationDone));
		componentInChildren.PlayAnimation("ScaleDown", 0.1f);
	}

	// Token: 0x060019F2 RID: 6642 RVA: 0x000897E6 File Offset: 0x000879E6
	private void OnAnimationDone(string animationName)
	{
		if (animationName != "ScaleDown")
		{
			return;
		}
		EasingAnimations componentInChildren = base.GetComponentInChildren<EasingAnimations>();
		componentInChildren.OnAnimationDone = (Action<string>)Delegate.Remove(componentInChildren.OnAnimationDone, new Action<string>(this.OnAnimationDone));
		this.DeleteObject();
	}
}
