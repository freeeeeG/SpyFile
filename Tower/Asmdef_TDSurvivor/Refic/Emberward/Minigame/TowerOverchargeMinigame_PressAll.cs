using System;
using System.Collections.Generic;

namespace Refic.Emberward.Minigame
{
	// Token: 0x020001CB RID: 459
	public class TowerOverchargeMinigame_PressAll : ATowerOverchargeMinigame
	{
		// Token: 0x06000BFC RID: 3068 RVA: 0x0002ED3C File Offset: 0x0002CF3C
		protected override void SetupMinigame()
		{
			this.list_Data = new List<OverchargeItemData>();
			for (int i = 0; i < 9; i++)
			{
				this.list_Data.Add(new OverchargeItemData(i + 1, (i + 1).ToString()));
			}
		}

		// Token: 0x06000BFD RID: 3069 RVA: 0x0002ED7F File Offset: 0x0002CF7F
		public override bool ValidateButtonPress(int index)
		{
			this.buttonPressedCount++;
			return true;
		}

		// Token: 0x06000BFE RID: 3070 RVA: 0x0002ED90 File Offset: 0x0002CF90
		public override bool IsCompleted()
		{
			return this.buttonPressedCount == this.MAX_BUTTON_COUNT;
		}

		// Token: 0x04000991 RID: 2449
		private int buttonPressedCount;
	}
}
