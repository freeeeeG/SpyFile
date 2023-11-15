using System;

// Token: 0x0200014C RID: 332
public static class GameParam
{
	// Token: 0x060008F5 RID: 2293 RVA: 0x0001893A File Offset: 0x00016B3A
	public static void ResetGameParam()
	{
		GameParam.GroundSize = StaticData.GroundSize;
	}

	// Token: 0x040004AC RID: 1196
	public static int GroundSize = 25;

	// Token: 0x040004AD RID: 1197
	public static int TrapCount;
}
