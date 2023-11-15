using System;
using TMPro;
using UnityEngine;

namespace flanne
{
	// Token: 0x02000137 RID: 311
	public class VictoryAchievedUI : MonoBehaviour
	{
		// Token: 0x1700008A RID: 138
		// (get) Token: 0x0600083E RID: 2110 RVA: 0x00022CFB File Offset: 0x00020EFB
		// (set) Token: 0x0600083F RID: 2111 RVA: 0x00022D03 File Offset: 0x00020F03
		public int[] victories { get; private set; }

		// Token: 0x06000840 RID: 2112 RVA: 0x00022D0C File Offset: 0x00020F0C
		private void Start()
		{
			this.achievedUIs = new TMP_Text[this.uiObjs.Length];
			for (int i = 0; i < this.uiObjs.Length; i++)
			{
				this.achievedUIs[i] = this.uiObjs[i].GetComponentInChildren<TMP_Text>();
			}
		}

		// Token: 0x06000841 RID: 2113 RVA: 0x00022D54 File Offset: 0x00020F54
		public void SetProperties(int[] victoryAchieved)
		{
			if (victoryAchieved == null)
			{
				victoryAchieved = new int[this.achievedUIs.Length];
				for (int i = 0; i < this.achievedUIs.Length; i++)
				{
					victoryAchieved[i] = -1;
				}
			}
			if (victoryAchieved.Length < this.achievedUIs.Length)
			{
				int num = victoryAchieved.Length;
				Array.Resize<int>(ref victoryAchieved, this.achievedUIs.Length);
				for (int j = num; j < this.achievedUIs.Length; j++)
				{
					victoryAchieved[j] = -1;
				}
			}
			for (int k = 0; k < this.achievedUIs.Length; k++)
			{
				this.uiObjs[k].SetActive(victoryAchieved[k] != -1);
				this.achievedUIs[k].text = victoryAchieved[k].ToString();
				if (victoryAchieved[k] == this.maxVictoryPossible)
				{
					this.achievedUIs[k].color = this.maxVictoryTextColor;
				}
				else
				{
					this.achievedUIs[k].color = this.normalTextColor;
				}
			}
			this.victories = victoryAchieved;
		}

		// Token: 0x04000611 RID: 1553
		[SerializeField]
		private GameObject[] uiObjs;

		// Token: 0x04000612 RID: 1554
		[SerializeField]
		private Color normalTextColor;

		// Token: 0x04000613 RID: 1555
		[SerializeField]
		private Color maxVictoryTextColor;

		// Token: 0x04000614 RID: 1556
		[SerializeField]
		private int maxVictoryPossible;

		// Token: 0x04000615 RID: 1557
		private TMP_Text[] achievedUIs;
	}
}
