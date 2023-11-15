using System;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace flanne.TitleScreen
{
	// Token: 0x020001E7 RID: 487
	public class RuneMenuGunState : TitleScreenState
	{
		// Token: 0x06000AAA RID: 2730 RVA: 0x00029303 File Offset: 0x00027503
		private void OnConfirmClick()
		{
			this.owner.ChangeState<GunSelectState>();
		}

		// Token: 0x06000AAB RID: 2731 RVA: 0x00029303 File Offset: 0x00027503
		private void OnCancel(InputAction.CallbackContext obj)
		{
			this.owner.ChangeState<GunSelectState>();
		}

		// Token: 0x06000AAC RID: 2732 RVA: 0x0002967C File Offset: 0x0002787C
		public override void Enter()
		{
			base.runeMenuPanel.Show();
			base.runeConfirmButton.onClick.AddListener(new UnityAction(this.OnConfirmClick));
			base.input.FindAction("UI/Cancel", false).canceled += this.OnCancel;
			base.checkRunesPromptArrow.enabled = false;
		}

		// Token: 0x06000AAD RID: 2733 RVA: 0x000296E0 File Offset: 0x000278E0
		public override void Exit()
		{
			base.runeMenuPanel.Hide();
			base.runeConfirmButton.onClick.RemoveListener(new UnityAction(this.OnConfirmClick));
			base.selectPanel.interactable = true;
			base.input.FindAction("UI/Cancel", false).canceled -= this.OnCancel;
			base.Save();
		}
	}
}
