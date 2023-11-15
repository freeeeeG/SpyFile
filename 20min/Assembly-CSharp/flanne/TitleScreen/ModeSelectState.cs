using System;
using UnityEngine.Events;

namespace flanne.TitleScreen
{
	// Token: 0x020001E4 RID: 484
	public class ModeSelectState : TitleScreenState
	{
		// Token: 0x06000A9B RID: 2715 RVA: 0x000293BC File Offset: 0x000275BC
		public void OnClickPlay()
		{
			if (base.modeSelectMenu.currIndex == 0)
			{
				this.owner.ChangeState<MapSelectState>();
				return;
			}
			SelectedMap.MapData = base.modeSelectMenu.toggledData;
			this.owner.ChangeState<WaitToLoadIntoBattleState>();
			base.selectPanel.Hide();
			base.gunMenu.Hide();
		}

		// Token: 0x06000A9C RID: 2716 RVA: 0x00029303 File Offset: 0x00027503
		public void OnClickBack()
		{
			this.owner.ChangeState<GunSelectState>();
		}

		// Token: 0x06000A9D RID: 2717 RVA: 0x00029414 File Offset: 0x00027614
		public override void Enter()
		{
			base.modeSelectPanel.Show();
			base.modeSelectMenu.RefreshDescription();
			base.modeSelectMenu.RefreshToggleData();
			base.difficultyController.RefreshText();
			base.modeSelectStartButton.onClick.AddListener(new UnityAction(this.OnClickPlay));
			base.modeSelectBackButton.onClick.AddListener(new UnityAction(this.OnClickBack));
			base.modeSelectStartButton.Select();
		}

		// Token: 0x06000A9E RID: 2718 RVA: 0x00029490 File Offset: 0x00027690
		public override void Exit()
		{
			base.modeSelectPanel.Hide();
			base.modeSelectStartButton.onClick.RemoveListener(new UnityAction(this.OnClickPlay));
			base.modeSelectBackButton.onClick.RemoveListener(new UnityAction(this.OnClickBack));
		}
	}
}
