using System;
using System.Collections.Generic;

// Token: 0x0200038B RID: 907
public class HashMapObjectPool<PoolKey, PoolValue>
{
	// Token: 0x060012B5 RID: 4789 RVA: 0x00064231 File Offset: 0x00062431
	public HashMapObjectPool(Func<PoolKey, PoolValue> instantiator, int initialCount = 0)
	{
		this.initialCount = initialCount;
		this.instantiator = instantiator;
	}

	// Token: 0x060012B6 RID: 4790 RVA: 0x00064254 File Offset: 0x00062454
	public HashMapObjectPool(HashMapObjectPool<PoolKey, PoolValue>.IPoolDescriptor[] descriptors, int initialCount = 0)
	{
		this.initialCount = initialCount;
		for (int i = 0; i < descriptors.Length; i++)
		{
			if (this.objectPoolMap.ContainsKey(descriptors[i].PoolId))
			{
				Debug.LogWarning(string.Format("HshMapObjectPool alaready contains key of {0}! Skipping!", descriptors[i].PoolId));
			}
			else
			{
				this.objectPoolMap[descriptors[i].PoolId] = new ObjectPool<PoolValue>(new Func<PoolValue>(descriptors[i].GetInstance), initialCount);
			}
		}
	}

	// Token: 0x060012B7 RID: 4791 RVA: 0x000642E4 File Offset: 0x000624E4
	public PoolValue GetInstance(PoolKey poolId)
	{
		ObjectPool<PoolValue> objectPool;
		if (!this.objectPoolMap.TryGetValue(poolId, out objectPool))
		{
			objectPool = (this.objectPoolMap[poolId] = new ObjectPool<PoolValue>(new Func<PoolValue>(this.PoolInstantiator), this.initialCount));
		}
		this.currentPoolId = poolId;
		return objectPool.GetInstance();
	}

	// Token: 0x060012B8 RID: 4792 RVA: 0x00064338 File Offset: 0x00062538
	public void ReleaseInstance(PoolKey poolId, PoolValue inst)
	{
		ObjectPool<PoolValue> objectPool;
		if (inst == null || !this.objectPoolMap.TryGetValue(poolId, out objectPool))
		{
			return;
		}
		objectPool.ReleaseInstance(inst);
	}

	// Token: 0x060012B9 RID: 4793 RVA: 0x00064368 File Offset: 0x00062568
	private PoolValue PoolInstantiator()
	{
		if (this.instantiator == null)
		{
			return default(PoolValue);
		}
		return this.instantiator(this.currentPoolId);
	}

	// Token: 0x04000A39 RID: 2617
	private Dictionary<PoolKey, ObjectPool<PoolValue>> objectPoolMap = new Dictionary<PoolKey, ObjectPool<PoolValue>>();

	// Token: 0x04000A3A RID: 2618
	private int initialCount;

	// Token: 0x04000A3B RID: 2619
	private PoolKey currentPoolId;

	// Token: 0x04000A3C RID: 2620
	private Func<PoolKey, PoolValue> instantiator;

	// Token: 0x02000FB3 RID: 4019
	public interface IPoolDescriptor
	{
		// Token: 0x170007F2 RID: 2034
		// (get) Token: 0x06007306 RID: 29446
		PoolKey PoolId { get; }

		// Token: 0x06007307 RID: 29447
		PoolValue GetInstance();
	}
}
