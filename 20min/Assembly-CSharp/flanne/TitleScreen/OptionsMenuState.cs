using System;
using flanne.UI;
using UnityEngine.Events;

namespace flanne.TitleScreen
{
	// Token: 0x020001E5 RID: 485
	public class OptionsMenuState : TitleScreenState
	{
		// Token: 0x06000AA0 RID: 2720 RVA: 0x000294E0 File Offset: 0x000276E0
		public void OnClick(int i)
		{
			if (i == 0)
			{
				this.owner.ChangeState<TitleMainMenuState>();
			}
		}

		// Token: 0x06000AA1 RID: 2721 RVA: 0x00029216 File Offset: 0x00027416
		public void OnCancel()
		{
			this.owner.ChangeState<TitleMainMenuState>();
		}

		// Token: 0x06000AA2 RID: 2722 RVA: 0x000294F0 File Offset: 0x000276F0
		public override void Enter()
		{
			base.optionsMenu.GetComponent<OptionsSetter>().Refresh();
			base.optionsMenu.Show();
			base.optionsMenu.onClick.AddListener(new UnityAction<int>(this.OnClick));
			base.optionsMenu.onCancel.AddListener(new UnityAction(this.OnCancel));
		}

		// Token: 0x06000AA3 RID: 2723 RVA: 0x00029550 File Offset: 0x00027750
		public override void Exit()
		{
			base.optionsMenu.Hide();
			base.optionsMenu.onClick.RemoveListener(new UnityAction<int>(this.OnClick));
			base.optionsMenu.onCancel.RemoveListener(new UnityAction(this.OnCancel));
		}
	}
}
