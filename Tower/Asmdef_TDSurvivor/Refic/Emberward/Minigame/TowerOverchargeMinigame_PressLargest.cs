using System;
using System.Collections.Generic;
using UnityEngine;

namespace Refic.Emberward.Minigame
{
	// Token: 0x020001CD RID: 461
	public class TowerOverchargeMinigame_PressLargest : ATowerOverchargeMinigame
	{
		// Token: 0x06000C04 RID: 3076 RVA: 0x0002EE74 File Offset: 0x0002D074
		protected override void SetupMinigame()
		{
			this.list_Data = new List<OverchargeItemData>();
			this.correctAnswer = Random.Range(7, 10);
			int contentValue = this.correctAnswer - 1;
			int contentValue2 = this.correctAnswer - 2;
			int contentValue3 = this.correctAnswer - 4;
			int contentValue4 = this.correctAnswer - 6;
			this.list_Data.Add(new OverchargeItemData(this.correctAnswer, this.correctAnswer.ToString()));
			for (int i = 0; i < 2; i++)
			{
				this.list_Data.Add(new OverchargeItemData(contentValue, contentValue.ToString()));
				this.list_Data.Add(new OverchargeItemData(contentValue2, contentValue2.ToString()));
				this.list_Data.Add(new OverchargeItemData(contentValue3, contentValue3.ToString()));
				this.list_Data.Add(new OverchargeItemData(contentValue4, contentValue4.ToString()));
			}
		}

		// Token: 0x06000C05 RID: 3077 RVA: 0x0002EF4F File Offset: 0x0002D14F
		public override bool ValidateButtonPress(int index)
		{
			if (this.list_Data[index].contentValue != this.correctAnswer)
			{
				return false;
			}
			this.correctCount++;
			return true;
		}

		// Token: 0x06000C06 RID: 3078 RVA: 0x0002EF7B File Offset: 0x0002D17B
		public override bool IsCompleted()
		{
			return this.correctCount == 1;
		}

		// Token: 0x04000994 RID: 2452
		private int correctAnswer = -1;

		// Token: 0x04000995 RID: 2453
		private int correctCount;
	}
}
