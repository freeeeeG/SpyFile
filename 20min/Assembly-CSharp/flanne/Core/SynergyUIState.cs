using System;
using UnityEngine.Events;

namespace flanne.Core
{
	// Token: 0x020001FC RID: 508
	public class SynergyUIState : GameState
	{
		// Token: 0x06000B72 RID: 2930 RVA: 0x0002A5D8 File Offset: 0x000287D8
		private void OnBack()
		{
			this.owner.ChangeState<PauseState>();
		}

		// Token: 0x06000B73 RID: 2931 RVA: 0x0002B076 File Offset: 0x00029276
		public override void Enter()
		{
			base.synergiesUIPanel.Show();
			base.synergiesUIBackButton.onClick.AddListener(new UnityAction(this.OnBack));
		}

		// Token: 0x06000B74 RID: 2932 RVA: 0x0002B09F File Offset: 0x0002929F
		public override void Exit()
		{
			base.synergiesUIPanel.Hide();
			base.synergiesUIBackButton.onClick.RemoveListener(new UnityAction(this.OnBack));
		}
	}
}
