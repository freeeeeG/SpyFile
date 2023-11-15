using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200002E RID: 46
[Serializable]
public class MyRandom
{
	// Token: 0x170000B2 RID: 178
	// (get) Token: 0x0600022D RID: 557 RVA: 0x0000D27E File Offset: 0x0000B47E
	public static MyRandom inst
	{
		get
		{
			return TempData.inst.battle.myRandom;
		}
	}

	// Token: 0x0600022E RID: 558 RVA: 0x0000D28F File Offset: 0x0000B48F
	public void InitRandomLists()
	{
		this.InitRandomList_RoyaltyCard();
		this.InitRandomList_VipCard();
		this.InitRandomList_Other();
	}

	// Token: 0x0600022F RID: 559 RVA: 0x0000D2A4 File Offset: 0x0000B4A4
	private void InitRandomList_RoyaltyCard()
	{
		this.listFloat1_RoyaltyCard = new List<float>();
		for (int i = 0; i < 100; i++)
		{
			this.listFloat1_RoyaltyCard.Add(Random.Range(0f, 1f));
		}
	}

	// Token: 0x06000230 RID: 560 RVA: 0x0000D2E4 File Offset: 0x0000B4E4
	private void InitRandomList_VipCard()
	{
		this.listFloat1_VipCard = new List<float>();
		for (int i = 0; i < 100; i++)
		{
			this.listFloat1_VipCard.Add(Random.Range(0f, 1f));
		}
	}

	// Token: 0x06000231 RID: 561 RVA: 0x0000D324 File Offset: 0x0000B524
	private void InitRandomList_Other()
	{
		this.listInt_Other = new List<int>();
		for (int i = 0; i < 100; i++)
		{
			this.listInt_Other.Add(Random.Range(0, 840));
		}
	}

	// Token: 0x06000232 RID: 562 RVA: 0x0000D360 File Offset: 0x0000B560
	public static float GetFloat1_RoyaltyCard()
	{
		if (MyRandom.inst.listFloat1_RoyaltyCard == null || MyRandom.inst.listFloat1_RoyaltyCard.Count == 0)
		{
			MyRandom.inst.InitRandomList_RoyaltyCard();
		}
		float result = MyRandom.inst.listFloat1_RoyaltyCard[0];
		MyRandom.inst.listFloat1_RoyaltyCard.RemoveAt(0);
		if (MyRandom.inst.listFloat1_RoyaltyCard.Count <= 20)
		{
			for (int i = 0; i < 100; i++)
			{
				MyRandom.inst.listFloat1_RoyaltyCard.Add(Random.Range(0f, 1f));
			}
		}
		return result;
	}

	// Token: 0x06000233 RID: 563 RVA: 0x0000D3F4 File Offset: 0x0000B5F4
	public static float GetFloat1_VipCard()
	{
		if (MyRandom.inst.listFloat1_VipCard == null || MyRandom.inst.listFloat1_VipCard.Count == 0)
		{
			MyRandom.inst.InitRandomList_VipCard();
		}
		float result = MyRandom.inst.listFloat1_VipCard[0];
		MyRandom.inst.listFloat1_VipCard.RemoveAt(0);
		if (MyRandom.inst.listFloat1_VipCard.Count <= 20)
		{
			for (int i = 0; i < 100; i++)
			{
				MyRandom.inst.listFloat1_VipCard.Add(Random.Range(0f, 1f));
			}
		}
		return result;
	}

	// Token: 0x06000234 RID: 564 RVA: 0x0000D488 File Offset: 0x0000B688
	public static int GetInt_Other()
	{
		if (MyRandom.inst.listInt_Other == null || MyRandom.inst.listInt_Other.Count == 0)
		{
			MyRandom.inst.InitRandomList_Other();
		}
		int result = MyRandom.inst.listInt_Other[0];
		MyRandom.inst.listInt_Other.RemoveAt(0);
		if (MyRandom.inst.listInt_Other.Count <= 20)
		{
			for (int i = 0; i < 100; i++)
			{
				MyRandom.inst.listInt_Other.Add(Random.Range(0, 840));
			}
		}
		return result;
	}

	// Token: 0x06000235 RID: 565 RVA: 0x0000D518 File Offset: 0x0000B718
	public void ReadFromSaveFile(MyRandom_ForSaveFile origin)
	{
		this.listFloat1_RoyaltyCard = new List<float>();
		for (int i = 0; i < origin.doubles_RoyaltyCard.Length; i++)
		{
			this.listFloat1_RoyaltyCard.Add((float)origin.doubles_RoyaltyCard[i]);
		}
		this.listFloat1_VipCard = new List<float>();
		for (int j = 0; j < origin.doubles_VipCard.Length; j++)
		{
			this.listFloat1_VipCard.Add((float)origin.doubles_VipCard[j]);
		}
		this.listInt_Other = new List<int>();
		for (int k = 0; k < origin.ints_Other.Length; k++)
		{
			this.listInt_Other.Add(origin.ints_Other[k]);
		}
	}

	// Token: 0x040001E2 RID: 482
	[SerializeField]
	public List<float> listFloat1_RoyaltyCard = new List<float>();

	// Token: 0x040001E3 RID: 483
	[SerializeField]
	public List<float> listFloat1_VipCard = new List<float>();

	// Token: 0x040001E4 RID: 484
	[SerializeField]
	public List<int> listInt_Other = new List<int>();
}
