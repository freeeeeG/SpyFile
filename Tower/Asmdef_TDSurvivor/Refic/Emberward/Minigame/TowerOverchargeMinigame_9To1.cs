using System;
using System.Collections.Generic;

namespace Refic.Emberward.Minigame
{
	// Token: 0x020001CA RID: 458
	public class TowerOverchargeMinigame_9To1 : ATowerOverchargeMinigame
	{
		// Token: 0x06000BF8 RID: 3064 RVA: 0x0002EC9C File Offset: 0x0002CE9C
		protected override void SetupMinigame()
		{
			this.list_Data = new List<OverchargeItemData>();
			for (int i = 0; i < 9; i++)
			{
				this.list_Data.Add(new OverchargeItemData(i + 1, (i + 1).ToString()));
			}
		}

		// Token: 0x06000BF9 RID: 3065 RVA: 0x0002ECDF File Offset: 0x0002CEDF
		public override bool ValidateButtonPress(int index)
		{
			if (this.list_Data[index].contentValue != this.nextCorrectAnswer)
			{
				return false;
			}
			this.nextCorrectAnswer--;
			this.correctCount++;
			return true;
		}

		// Token: 0x06000BFA RID: 3066 RVA: 0x0002ED19 File Offset: 0x0002CF19
		public override bool IsCompleted()
		{
			return this.correctCount == this.MAX_BUTTON_COUNT;
		}

		// Token: 0x0400098F RID: 2447
		private int nextCorrectAnswer = 9;

		// Token: 0x04000990 RID: 2448
		private int correctCount;
	}
}
