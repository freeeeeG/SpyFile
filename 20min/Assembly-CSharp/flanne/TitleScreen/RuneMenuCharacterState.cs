using System;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace flanne.TitleScreen
{
	// Token: 0x020001E6 RID: 486
	public class RuneMenuCharacterState : TitleScreenState
	{
		// Token: 0x06000AA5 RID: 2725 RVA: 0x000295A0 File Offset: 0x000277A0
		private void OnConfirmClick()
		{
			this.owner.ChangeState<CharacterSelectState>();
		}

		// Token: 0x06000AA6 RID: 2726 RVA: 0x000295A0 File Offset: 0x000277A0
		private void OnCancel(InputAction.CallbackContext obj)
		{
			this.owner.ChangeState<CharacterSelectState>();
		}

		// Token: 0x06000AA7 RID: 2727 RVA: 0x000295B0 File Offset: 0x000277B0
		public override void Enter()
		{
			base.runeMenuPanel.Show();
			base.runeConfirmButton.onClick.AddListener(new UnityAction(this.OnConfirmClick));
			base.input.FindAction("UI/Cancel", false).canceled += this.OnCancel;
			base.checkRunesPromptArrow.enabled = false;
		}

		// Token: 0x06000AA8 RID: 2728 RVA: 0x00029614 File Offset: 0x00027814
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
