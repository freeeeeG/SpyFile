using System;
using UnityEngine;

// Token: 0x0200002F RID: 47
public class MyRandom_ForSaveFile
{
	// Token: 0x06000237 RID: 567 RVA: 0x0000D5E4 File Offset: 0x0000B7E4
	public void InitFromMyRandom(MyRandom origin)
	{
		this.doubles_RoyaltyCard = new double[origin.listFloat1_RoyaltyCard.Count];
		for (int i = 0; i < this.doubles_RoyaltyCard.Length; i++)
		{
			this.doubles_RoyaltyCard[i] = (double)origin.listFloat1_RoyaltyCard[i];
		}
		this.doubles_VipCard = new double[origin.listFloat1_VipCard.Count];
		for (int j = 0; j < this.doubles_VipCard.Length; j++)
		{
			this.doubles_VipCard[j] = (double)origin.listFloat1_VipCard[j];
		}
		this.ints_Other = new int[origin.listInt_Other.Count];
		for (int k = 0; k < this.ints_Other.Length; k++)
		{
			this.ints_Other[k] = origin.listInt_Other[k];
		}
	}

	// Token: 0x040001E5 RID: 485
	[SerializeField]
	public double[] doubles_RoyaltyCard = new double[0];

	// Token: 0x040001E6 RID: 486
	[SerializeField]
	public double[] doubles_VipCard = new double[0];

	// Token: 0x040001E7 RID: 487
	[SerializeField]
	public int[] ints_Other = new int[0];
}
