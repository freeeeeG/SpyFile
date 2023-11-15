using System;
using System.Collections.Generic;

namespace Refic.Emberward.Minigame
{
	// Token: 0x020001C9 RID: 457
	public class TowerOverchargeMinigame_1To9 : ATowerOverchargeMinigame
	{
		// Token: 0x06000BF4 RID: 3060 RVA: 0x0002EC00 File Offset: 0x0002CE00
		protected override void SetupMinigame()
		{
			this.list_Data = new List<OverchargeItemData>();
			for (int i = 0; i < 9; i++)
			{
				this.list_Data.Add(new OverchargeItemData(i + 1, (i + 1).ToString()));
			}
		}

		// Token: 0x06000BF5 RID: 3061 RVA: 0x0002EC43 File Offset: 0x0002CE43
		public override bool ValidateButtonPress(int index)
		{
			if (this.list_Data[index].contentValue != this.nextCorrectAnswer)
			{
				return false;
			}
			this.nextCorrectAnswer++;
			this.correctCount++;
			return true;
		}

		// Token: 0x06000BF6 RID: 3062 RVA: 0x0002EC7D File Offset: 0x0002CE7D
		public override bool IsCompleted()
		{
			return this.correctCount == this.MAX_BUTTON_COUNT;
		}

		// Token: 0x0400098D RID: 2445
		private int nextCorrectAnswer = 1;

		// Token: 0x0400098E RID: 2446
		private int correctCount;
	}
}
