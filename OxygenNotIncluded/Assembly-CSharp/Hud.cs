using System;

// Token: 0x02000B1A RID: 2842
public class Hud : KScreen
{
	// Token: 0x0600578B RID: 22411 RVA: 0x00200D86 File Offset: 0x001FEF86
	public override void OnKeyDown(KButtonEvent e)
	{
		if (e.TryConsume(global::Action.Help))
		{
			GameScreenManager.Instance.StartScreen(ScreenPrefabs.Instance.ControlsScreen.gameObject, null, GameScreenManager.UIRenderTarget.ScreenSpaceOverlay);
		}
	}
}
