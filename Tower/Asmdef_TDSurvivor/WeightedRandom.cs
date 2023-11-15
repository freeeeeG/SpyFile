using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020001B0 RID: 432
public class WeightedRandom<T>
{
	// Token: 0x06000B81 RID: 2945 RVA: 0x0002D294 File Offset: 0x0002B494
	public WeightedRandom()
	{
		this.items = new List<T>();
		this.weights = new List<int>();
		this.totalWeight = 0;
	}

	// Token: 0x06000B82 RID: 2946 RVA: 0x0002D2B9 File Offset: 0x0002B4B9
	public void AddItem(T item, int weight)
	{
		if (weight <= 0)
		{
			Debug.LogWarning("Weight should be greater than 0.");
			return;
		}
		this.items.Add(item);
		this.weights.Add(weight);
		this.totalWeight += weight;
	}

	// Token: 0x06000B83 RID: 2947 RVA: 0x0002D2F0 File Offset: 0x0002B4F0
	public T GetRandomResult()
	{
		if (this.items.Count == 0 || this.totalWeight == 0)
		{
			Debug.LogWarning("No items to get random result from.");
			return default(T);
		}
		int num = Random.Range(0, this.totalWeight);
		int num2 = 0;
		for (int i = 0; i < this.items.Count; i++)
		{
			num2 += this.weights[i];
			if (num < num2)
			{
				return this.items[i];
			}
		}
		return default(T);
	}

	// Token: 0x06000B84 RID: 2948 RVA: 0x0002D374 File Offset: 0x0002B574
	public int GetItemCount()
	{
		return this.items.Count;
	}

	// Token: 0x06000B85 RID: 2949 RVA: 0x0002D381 File Offset: 0x0002B581
	public void Clear()
	{
		this.items.Clear();
		this.weights.Clear();
		this.totalWeight = 0;
	}

	// Token: 0x0400092E RID: 2350
	private List<T> items;

	// Token: 0x0400092F RID: 2351
	private List<int> weights;

	// Token: 0x04000930 RID: 2352
	private int totalWeight;
}
