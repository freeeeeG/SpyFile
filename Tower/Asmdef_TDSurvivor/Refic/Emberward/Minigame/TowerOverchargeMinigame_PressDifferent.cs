using System;
using System.Collections.Generic;
using UnityEngine;

namespace Refic.Emberward.Minigame
{
	// Token: 0x020001CC RID: 460
	public class TowerOverchargeMinigame_PressDifferent : ATowerOverchargeMinigame
	{
		// Token: 0x06000C00 RID: 3072 RVA: 0x0002EDA8 File Offset: 0x0002CFA8
		protected override void SetupMinigame()
		{
			this.list_Data = new List<OverchargeItemData>();
			this.correctAnswer = Random.Range(1, 10);
			int contentValue = (this.correctAnswer + Random.Range(1, 7)) % this.MAX_BUTTON_COUNT + 1;
			this.list_Data.Add(new OverchargeItemData(this.correctAnswer, this.correctAnswer.ToString()));
			for (int i = 0; i < 8; i++)
			{
				this.list_Data.Add(new OverchargeItemData(contentValue, contentValue.ToString()));
			}
		}

		// Token: 0x06000C01 RID: 3073 RVA: 0x0002EE2B File Offset: 0x0002D02B
		public override bool ValidateButtonPress(int index)
		{
			if (this.list_Data[index].contentValue != this.correctAnswer)
			{
				return false;
			}
			this.correctCount++;
			return true;
		}

		// Token: 0x06000C02 RID: 3074 RVA: 0x0002EE57 File Offset: 0x0002D057
		public override bool IsCompleted()
		{
			return this.correctCount == 1;
		}

		// Token: 0x04000992 RID: 2450
		private int correctAnswer = -1;

		// Token: 0x04000993 RID: 2451
		private int correctCount;
	}
}
