using System;
using UnityEngine.Events;

namespace flanne.Core
{
	// Token: 0x020001FA RID: 506
	public class RavenUnlockedState : GameState
	{
		// Token: 0x06000B69 RID: 2921 RVA: 0x0002A4BA File Offset: 0x000286BA
		private void OnConfirm()
		{
			this.owner.ChangeState<PlayerSurvivedState>();
		}

		// Token: 0x06000B6A RID: 2922 RVA: 0x0002AF80 File Offset: 0x00029180
		public override void Enter()
		{
			base.ravenUnlockedPanel.Show();
			base.ravenUnlockedConfirmButton.onClick.AddListener(new UnityAction(this.OnConfirm));
			SaveSystem.data.characterUnlocks.unlocks[9] = true;
		}

		// Token: 0x06000B6B RID: 2923 RVA: 0x0002AFBC File Offset: 0x000291BC
		public override void Exit()
		{
			base.ravenUnlockedPanel.Hide();
			base.ravenUnlockedConfirmButton.onClick.RemoveListener(new UnityAction(this.OnConfirm));
		}
	}
}
