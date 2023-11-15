using System;
using System.Collections.Generic;

// Token: 0x02000580 RID: 1408
public class Accumulators
{
	// Token: 0x0600221D RID: 8733 RVA: 0x000BB90D File Offset: 0x000B9B0D
	public Accumulators()
	{
		this.elapsedTime = 0f;
		this.accumulated = new KCompactedVector<float>(0);
		this.average = new KCompactedVector<float>(0);
	}

	// Token: 0x0600221E RID: 8734 RVA: 0x000BB938 File Offset: 0x000B9B38
	public HandleVector<int>.Handle Add(string name, KMonoBehaviour owner)
	{
		HandleVector<int>.Handle result = this.accumulated.Allocate(0f);
		this.average.Allocate(0f);
		return result;
	}

	// Token: 0x0600221F RID: 8735 RVA: 0x000BB95B File Offset: 0x000B9B5B
	public HandleVector<int>.Handle Remove(HandleVector<int>.Handle handle)
	{
		if (!handle.IsValid())
		{
			return HandleVector<int>.InvalidHandle;
		}
		this.accumulated.Free(handle);
		this.average.Free(handle);
		return HandleVector<int>.InvalidHandle;
	}

	// Token: 0x06002220 RID: 8736 RVA: 0x000BB98C File Offset: 0x000B9B8C
	public void Sim200ms(float dt)
	{
		this.elapsedTime += dt;
		if (this.elapsedTime < 3f)
		{
			return;
		}
		this.elapsedTime -= 3f;
		List<float> dataList = this.accumulated.GetDataList();
		List<float> dataList2 = this.average.GetDataList();
		int count = dataList.Count;
		float num = 0.33333334f;
		for (int i = 0; i < count; i++)
		{
			dataList2[i] = dataList[i] * num;
			dataList[i] = 0f;
		}
	}

	// Token: 0x06002221 RID: 8737 RVA: 0x000BBA1B File Offset: 0x000B9C1B
	public float GetAverageRate(HandleVector<int>.Handle handle)
	{
		if (!handle.IsValid())
		{
			return 0f;
		}
		return this.average.GetData(handle);
	}

	// Token: 0x06002222 RID: 8738 RVA: 0x000BBA38 File Offset: 0x000B9C38
	public void Accumulate(HandleVector<int>.Handle handle, float amount)
	{
		float data = this.accumulated.GetData(handle);
		this.accumulated.SetData(handle, data + amount);
	}

	// Token: 0x04001368 RID: 4968
	private const float TIME_WINDOW = 3f;

	// Token: 0x04001369 RID: 4969
	private float elapsedTime;

	// Token: 0x0400136A RID: 4970
	private KCompactedVector<float> accumulated;

	// Token: 0x0400136B RID: 4971
	private KCompactedVector<float> average;
}
