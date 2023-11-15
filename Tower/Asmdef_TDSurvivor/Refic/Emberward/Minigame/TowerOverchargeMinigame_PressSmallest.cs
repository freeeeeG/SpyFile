using System;
using System.Collections.Generic;
using UnityEngine;

namespace Refic.Emberward.Minigame
{
	// Token: 0x020001CE RID: 462
	public class TowerOverchargeMinigame_PressSmallest : ATowerOverchargeMinigame
	{
		// Token: 0x06000C08 RID: 3080 RVA: 0x0002EF98 File Offset: 0x0002D198
		protected override void SetupMinigame()
		{
			this.list_Data = new List<OverchargeItemData>();
			this.correctAnswer = Random.Range(1, 4);
			int contentValue = this.correctAnswer + 1;
			int contentValue2 = this.correctAnswer + 2;
			int contentValue3 = this.correctAnswer + 4;
			int contentValue4 = this.correctAnswer + 6;
			this.list_Data.Add(new OverchargeItemData(this.correctAnswer, this.correctAnswer.ToString()));
			for (int i = 0; i < 2; i++)
			{
				this.list_Data.Add(new OverchargeItemData(contentValue, contentValue.ToString()));
				this.list_Data.Add(new OverchargeItemData(contentValue2, contentValue2.ToString()));
				this.list_Data.Add(new OverchargeItemData(contentValue3, contentValue3.ToString()));
				this.list_Data.Add(new OverchargeItemData(contentValue4, contentValue4.ToString()));
			}
		}

		// Token: 0x06000C09 RID: 3081 RVA: 0x0002F072 File Offset: 0x0002D272
		public override bool ValidateButtonPress(int index)
		{
			if (this.list_Data[index].contentValue != this.correctAnswer)
			{
				return false;
			}
			this.correctCount++;
			return true;
		}

		// Token: 0x06000C0A RID: 3082 RVA: 0x0002F09E File Offset: 0x0002D29E
		public override bool IsCompleted()
		{
			return this.correctCount == 1;
		}

		// Token: 0x04000996 RID: 2454
		private int correctAnswer = -1;

		// Token: 0x04000997 RID: 2455
		private int correctCount;
	}
}
