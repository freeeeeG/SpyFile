using System;
using UnityEngine.Events;

namespace flanne.Core
{
	// Token: 0x020001F2 RID: 498
	public class HasturUnlockedState : GameState
	{
		// Token: 0x06000B3C RID: 2876 RVA: 0x0002A4BA File Offset: 0x000286BA
		private void OnConfirm()
		{
			this.owner.ChangeState<PlayerSurvivedState>();
		}

		// Token: 0x06000B3D RID: 2877 RVA: 0x0002A4C7 File Offset: 0x000286C7
		public override void Enter()
		{
			base.hasturUnlockedPanel.Show();
			base.hasturUnlockedConfirmButton.onClick.AddListener(new UnityAction(this.OnConfirm));
			SaveSystem.data.characterUnlocks.unlocks[8] = true;
		}

		// Token: 0x06000B3E RID: 2878 RVA: 0x0002A502 File Offset: 0x00028702
		public override void Exit()
		{
			base.hasturUnlockedPanel.Hide();
			base.hasturUnlockedConfirmButton.onClick.RemoveListener(new UnityAction(this.OnConfirm));
		}
	}
}
