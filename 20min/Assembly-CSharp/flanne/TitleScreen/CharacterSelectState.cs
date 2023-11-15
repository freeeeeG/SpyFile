using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace flanne.TitleScreen
{
	// Token: 0x020001DF RID: 479
	public class CharacterSelectState : TitleScreenState
	{
		// Token: 0x06000A7E RID: 2686 RVA: 0x00028DF5 File Offset: 0x00026FF5
		public void OnClickRunes()
		{
			base.selectPanel.interactable = false;
			base.characterMenu.interactable = false;
			SaveSystem.data.checkedRunes = true;
			this.owner.ChangeState<RuneMenuCharacterState>();
		}

		// Token: 0x06000A7F RID: 2687 RVA: 0x00028E25 File Offset: 0x00027025
		private void OnClick(int i)
		{
			Loadout.CharacterSelection = base.characterMenu[i];
			Loadout.CharacterIndex = i;
			base.characterMenu.Hide();
			this.owner.ChangeState<GunSelectState>();
		}

		// Token: 0x06000A80 RID: 2688 RVA: 0x00028E54 File Offset: 0x00027054
		private void OnCancel(InputAction.CallbackContext obj)
		{
			this.OnCancel();
		}

		// Token: 0x06000A81 RID: 2689 RVA: 0x00028E5C File Offset: 0x0002705C
		private void OnCancel()
		{
			base.selectPanel.Hide();
			base.characterMenu.Hide();
			this.owner.ChangeState<TitleMainMenuState>();
		}

		// Token: 0x06000A82 RID: 2690 RVA: 0x00028E7F File Offset: 0x0002707F
		public override void Enter()
		{
			base.StartCoroutine(this.WaitToShowCR());
			base.checkRunesPromptArrow.enabled = (SaveSystem.data.playedGame && !SaveSystem.data.checkedRunes);
		}

		// Token: 0x06000A83 RID: 2691 RVA: 0x00028EB8 File Offset: 0x000270B8
		public override void Exit()
		{
			base.runesButton.onClick.RemoveListener(new UnityAction(this.OnClickRunes));
			base.characterMenu.onClick.RemoveListener(new UnityAction<int>(this.OnClick));
			base.loadoutBackButton.onClick.RemoveListener(new UnityAction(this.OnCancel));
			base.input.FindAction("UI/Cancel", false).canceled -= this.OnCancel;
			base.Save();
		}

		// Token: 0x06000A84 RID: 2692 RVA: 0x00028F41 File Offset: 0x00027141
		private IEnumerator WaitToShowCR()
		{
			yield return new WaitForSeconds(0.2f);
			base.runesButton.onClick.AddListener(new UnityAction(this.OnClickRunes));
			base.characterMenu.Show();
			base.characterMenu.onClick.AddListener(new UnityAction<int>(this.OnClick));
			base.loadoutBackButton.onClick.AddListener(new UnityAction(this.OnCancel));
			base.input.FindAction("UI/Cancel", false).canceled += this.OnCancel;
			yield break;
		}
	}
}
