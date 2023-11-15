using System;
using System.Collections;
using System.Collections.Generic;
using flanne.RuneSystem;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace flanne.TitleScreen
{
	// Token: 0x020001E0 RID: 480
	public class GunSelectState : TitleScreenState
	{
		// Token: 0x06000A86 RID: 2694 RVA: 0x00028F58 File Offset: 0x00027158
		public void OnClickRunes()
		{
			base.selectPanel.interactable = false;
			base.gunMenu.interactable = false;
			SaveSystem.data.checkedRunes = true;
			this.owner.ChangeState<RuneMenuGunState>();
		}

		// Token: 0x06000A87 RID: 2695 RVA: 0x00028F88 File Offset: 0x00027188
		private void OnClick(int i)
		{
			this.owner.ChangeState<ModeSelectState>();
			Loadout.GunSelection = base.gunMenu[i];
			Loadout.GunIndex = i;
			List<RuneData> activeRunes = base.swordRuneTree.GetActiveRunes();
			activeRunes.AddRange(base.shieldRuneTree.GetActiveRunes());
			Loadout.RuneSelection = activeRunes;
			base.selectPanel.interactable = false;
			base.gunMenu.interactable = false;
		}

		// Token: 0x06000A88 RID: 2696 RVA: 0x00028FF0 File Offset: 0x000271F0
		private void OnCancel(InputAction.CallbackContext obj)
		{
			this.OnCancel();
		}

		// Token: 0x06000A89 RID: 2697 RVA: 0x00028FF8 File Offset: 0x000271F8
		private void OnCancel()
		{
			this.owner.ChangeState<CharacterSelectState>();
			base.gunMenu.Hide();
		}

		// Token: 0x06000A8A RID: 2698 RVA: 0x00029010 File Offset: 0x00027210
		public override void Enter()
		{
			base.selectPanel.interactable = true;
			base.gunMenu.interactable = true;
			base.StartCoroutine(this.WaitToShowCR());
		}

		// Token: 0x06000A8B RID: 2699 RVA: 0x00029038 File Offset: 0x00027238
		public override void Exit()
		{
			base.runesButton.onClick.RemoveListener(new UnityAction(this.OnClickRunes));
			base.gunMenu.onClick.RemoveListener(new UnityAction<int>(this.OnClick));
			base.loadoutBackButton.onClick.RemoveListener(new UnityAction(this.OnCancel));
			base.input.FindAction("UI/Cancel", false).canceled -= this.OnCancel;
			base.Save();
		}

		// Token: 0x06000A8C RID: 2700 RVA: 0x000290C1 File Offset: 0x000272C1
		private IEnumerator WaitToShowCR()
		{
			yield return new WaitForSeconds(0.2f);
			base.runesButton.onClick.AddListener(new UnityAction(this.OnClickRunes));
			base.gunMenu.Show();
			base.gunMenu.onClick.AddListener(new UnityAction<int>(this.OnClick));
			base.loadoutBackButton.onClick.AddListener(new UnityAction(this.OnCancel));
			base.input.FindAction("UI/Cancel", false).canceled += this.OnCancel;
			yield break;
		}
	}
}
