using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace flanne.TitleScreen
{
	// Token: 0x020001E8 RID: 488
	public class TitleMainMenuState : TitleScreenState
	{
		// Token: 0x06000AAF RID: 2735 RVA: 0x00029748 File Offset: 0x00027948
		public void OnClick(int i)
		{
			switch (i)
			{
			case 0:
				base.StartCoroutine(this.TransitionToLoadoutSelect());
				return;
			case 1:
				this.owner.ChangeState<LangaugeState>();
				return;
			case 2:
				this.owner.ChangeState<OptionsMenuState>();
				return;
			case 3:
				Application.Quit();
				return;
			default:
				return;
			}
		}

		// Token: 0x06000AB0 RID: 2736 RVA: 0x00029797 File Offset: 0x00027997
		public override void Enter()
		{
			Cursor.visible = true;
			base.StartCoroutine(this.WaitToShowMenuCR());
		}

		// Token: 0x06000AB1 RID: 2737 RVA: 0x000297AC File Offset: 0x000279AC
		public override void Exit()
		{
			base.mainMenu.Hide();
			base.mainMenu.onClick.RemoveListener(new UnityAction<int>(this.OnClick));
		}

		// Token: 0x06000AB2 RID: 2738 RVA: 0x000297D5 File Offset: 0x000279D5
		private IEnumerator WaitToShowMenuCR()
		{
			base.eyes.SetTrigger("Open");
			yield return new WaitForSeconds(0.3f);
			base.logoPanel.Show();
			base.leavesPanel.Show();
			base.mainMenu.Show();
			base.mainMenu.onClick.AddListener(new UnityAction<int>(this.OnClick));
			base.emberpathPanel.Show();
			yield break;
		}

		// Token: 0x06000AB3 RID: 2739 RVA: 0x000297E4 File Offset: 0x000279E4
		private IEnumerator TransitionToLoadoutSelect()
		{
			base.logoPanel.Hide();
			base.leavesPanel.Hide();
			base.eyes.ResetTrigger("Open");
			base.eyes.SetTrigger("Close");
			base.mainMenu.Hide();
			base.emberpathPanel.Hide();
			yield return new WaitForSeconds(0.2f);
			base.selectPanel.Show();
			this.owner.ChangeState<CharacterSelectState>();
			yield break;
		}
	}
}
