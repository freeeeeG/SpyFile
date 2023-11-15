using System;
using UnityEngine.Events;

namespace flanne.TitleScreen
{
	// Token: 0x020001E2 RID: 482
	public class LangaugeState : TitleScreenState
	{
		// Token: 0x06000A91 RID: 2705 RVA: 0x00029216 File Offset: 0x00027416
		public void OnClick(int i)
		{
			this.owner.ChangeState<TitleMainMenuState>();
		}

		// Token: 0x06000A92 RID: 2706 RVA: 0x00029216 File Offset: 0x00027416
		public void OnCancel()
		{
			this.owner.ChangeState<TitleMainMenuState>();
		}

		// Token: 0x06000A93 RID: 2707 RVA: 0x00029224 File Offset: 0x00027424
		public override void Enter()
		{
			base.logoPanel.Hide();
			base.languageMenu.Show();
			base.languageMenu.onClick.AddListener(new UnityAction<int>(this.OnClick));
			base.languageMenu.onCancel.AddListener(new UnityAction(this.OnCancel));
		}

		// Token: 0x06000A94 RID: 2708 RVA: 0x00029280 File Offset: 0x00027480
		public override void Exit()
		{
			base.languageMenu.Hide();
			base.languageMenu.onClick.RemoveListener(new UnityAction<int>(this.OnClick));
			base.languageMenu.onCancel.RemoveListener(new UnityAction(this.OnCancel));
		}
	}
}
