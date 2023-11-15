using System;
using UnityEngine.Events;

namespace flanne.Core
{
	// Token: 0x020001F5 RID: 501
	public class OptionsState : GameState
	{
		// Token: 0x06000B48 RID: 2888 RVA: 0x0002A5C8 File Offset: 0x000287C8
		public void OnClick(int i)
		{
			if (i == 0)
			{
				this.owner.ChangeState<PauseState>();
			}
		}

		// Token: 0x06000B49 RID: 2889 RVA: 0x0002A5D8 File Offset: 0x000287D8
		private void OnBack()
		{
			this.owner.ChangeState<PauseState>();
		}

		// Token: 0x06000B4A RID: 2890 RVA: 0x0002A5E8 File Offset: 0x000287E8
		public override void Enter()
		{
			base.optionsMenu.Show();
			base.optionsMenu.onClick.AddListener(new UnityAction<int>(this.OnClick));
			base.optionsMenu.onCancel.AddListener(new UnityAction(this.OnBack));
		}

		// Token: 0x06000B4B RID: 2891 RVA: 0x0002A638 File Offset: 0x00028838
		public override void Exit()
		{
			base.optionsMenu.Hide();
			base.optionsMenu.onClick.RemoveListener(new UnityAction<int>(this.OnClick));
			base.optionsMenu.onCancel.RemoveListener(new UnityAction(this.OnBack));
		}
	}
}
