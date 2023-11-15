using System;
using UnityEngine;

// Token: 0x020000A2 RID: 162
[Serializable]
public class MapGenerateSetting
{
	// Token: 0x0600035D RID: 861 RVA: 0x0000D808 File Offset: 0x0000BA08
	public MapGenerateSetting(int step, int minNodeInStep, int maxNodeInStep, int weight_Level, int weight_Shop, int weight_Workshop, int weight_SpecialEvent)
	{
		this.Step = step;
		this.MinNodeInStep = minNodeInStep;
		this.MaxNodeInStep = maxNodeInStep;
		this.Weight_Level = weight_Level;
		this.Weight_Level_Corrupted = weight_Level;
		this.Weight_Shop = weight_Shop;
		this.Weight_Workshop = weight_Workshop;
		this.Weight_SpecialEvent = weight_SpecialEvent;
		this.Seed = Random.Range(0, 99999999);
	}

	// Token: 0x0600035E RID: 862 RVA: 0x0000D86C File Offset: 0x0000BA6C
	public MapGenerateSetting(int seed = -1)
	{
		this.Seed = ((seed == -1) ? Random.Range(0, 99999999) : seed);
		Random random = new Random(this.Seed);
		this.Step = random.Next(7, 9);
		this.MinNodeInStep = 2;
		this.MaxNodeInStep = random.Next(this.MinNodeInStep + 1, this.MinNodeInStep + 2);
		this.Weight_Level = random.Next(5, 10);
		this.Weight_Level_Corrupted = random.Next(5, 10);
		this.Weight_Shop = random.Next(3, 5);
		this.Weight_Workshop = random.Next(3, 5);
		this.Weight_SpecialEvent = 0;
	}

	// Token: 0x04000387 RID: 903
	public int Step;

	// Token: 0x04000388 RID: 904
	public int MinNodeInStep;

	// Token: 0x04000389 RID: 905
	public int MaxNodeInStep;

	// Token: 0x0400038A RID: 906
	public int Weight_Level;

	// Token: 0x0400038B RID: 907
	public int Weight_Level_Corrupted;

	// Token: 0x0400038C RID: 908
	public int Weight_Shop;

	// Token: 0x0400038D RID: 909
	public int Weight_Workshop;

	// Token: 0x0400038E RID: 910
	public int Weight_SpecialEvent;

	// Token: 0x0400038F RID: 911
	public int Seed;
}
