using System;
using UnityEngine;

// Token: 0x02000425 RID: 1061
public class FPS_No_String_Allocs
{
	// Token: 0x06001338 RID: 4920 RVA: 0x0006BB08 File Offset: 0x00069F08
	public FPS_No_String_Allocs()
	{
		this.m_PreAllocatedReturnStrings = new string[502];
		float num = 1f;
		for (int i = 0; i < 500; i++)
		{
			this.m_PreAllocatedReturnStrings[i] = string.Format("FPS: {0:F2}", num);
			num += 0.13f;
		}
		this.m_PreAllocatedReturnStrings[500] = "FPS: ^^^^^ ";
		this.m_PreAllocatedReturnStrings[501] = "FPS: _____ ";
	}

	// Token: 0x06001339 RID: 4921 RVA: 0x0006BB98 File Offset: 0x00069F98
	public void Update()
	{
		this.m_FPS[this.m_OldFPSSlot] = Time.deltaTime;
		float num = 0f;
		for (int i = 0; i < 10; i++)
		{
			num += this.m_FPS[i];
		}
		num = 10f / num;
		this.m_OldFPSSlot++;
		this.m_AveFPS = num;
		if (this.m_OldFPSSlot >= 10)
		{
			this.m_OldFPSSlot = 0;
		}
	}

	// Token: 0x0600133A RID: 4922 RVA: 0x0006BC0C File Offset: 0x0006A00C
	public string GetString()
	{
		int num = (int)((this.m_AveFPS - 1f) * 7.692308f);
		if (num > 500)
		{
			num = 500;
		}
		else if (num < 0)
		{
			num = 501;
		}
		return this.m_PreAllocatedReturnStrings[num];
	}

	// Token: 0x0600133B RID: 4923 RVA: 0x0006BC58 File Offset: 0x0006A058
	public float AverageFPS()
	{
		return this.m_AveFPS;
	}

	// Token: 0x04000F24 RID: 3876
	private string[] m_PreAllocatedReturnStrings;

	// Token: 0x04000F25 RID: 3877
	private const float MIN_FPS = 1f;

	// Token: 0x04000F26 RID: 3878
	private const float MAX_FPS = 66f;

	// Token: 0x04000F27 RID: 3879
	private const int NUM_VALUES = 500;

	// Token: 0x04000F28 RID: 3880
	private const float STEP = 0.13f;

	// Token: 0x04000F29 RID: 3881
	private const float INV_STEP = 7.692308f;

	// Token: 0x04000F2A RID: 3882
	private const int NUM_FPS_SAMPLES = 10;

	// Token: 0x04000F2B RID: 3883
	private float[] m_FPS = new float[10];

	// Token: 0x04000F2C RID: 3884
	private float m_AveFPS;

	// Token: 0x04000F2D RID: 3885
	private int m_OldFPSSlot;
}
