using System;
using UnityEngine.Events;

namespace flanne.TitleScreen
{
	// Token: 0x020001E3 RID: 483
	public class MapSelectState : TitleScreenState
	{
		// Token: 0x06000A96 RID: 2710 RVA: 0x000292D0 File Offset: 0x000274D0
		public void OnClickPlay()
		{
			SelectedMap.MapData = base.mapSelectMenu.toggledData;
			this.owner.ChangeState<WaitToLoadIntoBattleState>();
			base.selectPanel.Hide();
			base.gunMenu.Hide();
		}

		// Token: 0x06000A97 RID: 2711 RVA: 0x00029303 File Offset: 0x00027503
		public void OnClickBack()
		{
			this.owner.ChangeState<GunSelectState>();
		}

		// Token: 0x06000A98 RID: 2712 RVA: 0x00029310 File Offset: 0x00027510
		public override void Enter()
		{
			base.mapSelectPanel.Show();
			base.mapSelectStartButton.onClick.AddListener(new UnityAction(this.OnClickPlay));
			base.mapSelectBackButton.onClick.AddListener(new UnityAction(this.OnClickBack));
			base.mapSelectStartButton.Select();
		}

		// Token: 0x06000A99 RID: 2713 RVA: 0x0002936C File Offset: 0x0002756C
		public override void Exit()
		{
			base.mapSelectPanel.Hide();
			base.mapSelectStartButton.onClick.RemoveListener(new UnityAction(this.OnClickPlay));
			base.mapSelectBackButton.onClick.RemoveListener(new UnityAction(this.OnClickBack));
		}
	}
}
