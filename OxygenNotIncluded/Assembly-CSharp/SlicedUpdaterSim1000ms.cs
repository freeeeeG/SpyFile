using System;
using System.Collections.Generic;
using KSerialization;

// Token: 0x02000979 RID: 2425
public abstract class SlicedUpdaterSim1000ms<T> : KMonoBehaviour, ISim200ms where T : KMonoBehaviour, ISlicedSim1000ms
{
	// Token: 0x06004766 RID: 18278 RVA: 0x0019365B File Offset: 0x0019185B
	protected override void OnPrefabInit()
	{
		this.InitializeSlices();
		base.OnPrefabInit();
		SlicedUpdaterSim1000ms<T>.instance = this;
	}

	// Token: 0x06004767 RID: 18279 RVA: 0x0019366F File Offset: 0x0019186F
	protected override void OnForcedCleanUp()
	{
		SlicedUpdaterSim1000ms<T>.instance = null;
		base.OnForcedCleanUp();
	}

	// Token: 0x06004768 RID: 18280 RVA: 0x00193680 File Offset: 0x00191880
	private void InitializeSlices()
	{
		int num = SlicedUpdaterSim1000ms<T>.NUM_200MS_BUCKETS * this.numSlicesPer200ms;
		this.m_slices = new List<SlicedUpdaterSim1000ms<T>.Slice>();
		for (int i = 0; i < num; i++)
		{
			this.m_slices.Add(new SlicedUpdaterSim1000ms<T>.Slice());
		}
		this.m_nextSliceIdx = 0;
	}

	// Token: 0x06004769 RID: 18281 RVA: 0x001936C8 File Offset: 0x001918C8
	private int GetSliceIdx(T toBeUpdated)
	{
		return toBeUpdated.GetComponent<KPrefabID>().InstanceID % this.m_slices.Count;
	}

	// Token: 0x0600476A RID: 18282 RVA: 0x001936E8 File Offset: 0x001918E8
	public void RegisterUpdate1000ms(T toBeUpdated)
	{
		SlicedUpdaterSim1000ms<T>.Slice slice = this.m_slices[this.GetSliceIdx(toBeUpdated)];
		slice.Register(toBeUpdated);
		DebugUtil.DevAssert(slice.Count < this.maxUpdatesPer200ms, string.Format("The SlicedUpdaterSim1000ms for {0} wants to update no more than {1} instances per 200ms tick, but a slice has grown more than the SlicedUpdaterSim1000ms can support.", typeof(T).Name, this.maxUpdatesPer200ms), null);
	}

	// Token: 0x0600476B RID: 18283 RVA: 0x00193745 File Offset: 0x00191945
	public void UnregisterUpdate1000ms(T toBeUpdated)
	{
		this.m_slices[this.GetSliceIdx(toBeUpdated)].Unregister(toBeUpdated);
	}

	// Token: 0x0600476C RID: 18284 RVA: 0x00193760 File Offset: 0x00191960
	public void Sim200ms(float dt)
	{
		foreach (SlicedUpdaterSim1000ms<T>.Slice slice in this.m_slices)
		{
			slice.IncrementDt(dt);
		}
		int num = 0;
		int i = 0;
		while (i < this.numSlicesPer200ms)
		{
			SlicedUpdaterSim1000ms<T>.Slice slice2 = this.m_slices[this.m_nextSliceIdx];
			num += slice2.Count;
			if (num > this.maxUpdatesPer200ms && i > 0)
			{
				break;
			}
			slice2.Update();
			i++;
			this.m_nextSliceIdx = (this.m_nextSliceIdx + 1) % this.m_slices.Count;
		}
	}

	// Token: 0x04002F4C RID: 12108
	private static int NUM_200MS_BUCKETS = 5;

	// Token: 0x04002F4D RID: 12109
	public static SlicedUpdaterSim1000ms<T> instance;

	// Token: 0x04002F4E RID: 12110
	[Serialize]
	public int maxUpdatesPer200ms = 300;

	// Token: 0x04002F4F RID: 12111
	[Serialize]
	public int numSlicesPer200ms = 3;

	// Token: 0x04002F50 RID: 12112
	private List<SlicedUpdaterSim1000ms<T>.Slice> m_slices;

	// Token: 0x04002F51 RID: 12113
	private int m_nextSliceIdx;

	// Token: 0x020017DE RID: 6110
	private class Slice
	{
		// Token: 0x06008F9C RID: 36764 RVA: 0x003230D2 File Offset: 0x003212D2
		public void Register(T toBeUpdated)
		{
			if (this.m_timeSinceLastUpdate == 0f)
			{
				this.m_updateList.Add(toBeUpdated);
				return;
			}
			this.m_recentlyAdded[toBeUpdated] = 0f;
		}

		// Token: 0x06008F9D RID: 36765 RVA: 0x003230FF File Offset: 0x003212FF
		public void Unregister(T toBeUpdated)
		{
			if (!this.m_updateList.Remove(toBeUpdated))
			{
				this.m_recentlyAdded.Remove(toBeUpdated);
			}
		}

		// Token: 0x1700098B RID: 2443
		// (get) Token: 0x06008F9E RID: 36766 RVA: 0x0032311C File Offset: 0x0032131C
		public int Count
		{
			get
			{
				return this.m_updateList.Count + this.m_recentlyAdded.Count;
			}
		}

		// Token: 0x06008F9F RID: 36767 RVA: 0x00323135 File Offset: 0x00321335
		public List<T> GetUpdateList()
		{
			List<T> list = new List<T>();
			list.AddRange(this.m_updateList);
			list.AddRange(this.m_recentlyAdded.Keys);
			return list;
		}

		// Token: 0x06008FA0 RID: 36768 RVA: 0x0032315C File Offset: 0x0032135C
		public void Update()
		{
			foreach (T t in this.m_updateList)
			{
				t.SlicedSim1000ms(this.m_timeSinceLastUpdate);
			}
			foreach (KeyValuePair<T, float> keyValuePair in this.m_recentlyAdded)
			{
				keyValuePair.Key.SlicedSim1000ms(keyValuePair.Value);
				this.m_updateList.Add(keyValuePair.Key);
			}
			this.m_recentlyAdded.Clear();
			this.m_timeSinceLastUpdate = 0f;
		}

		// Token: 0x06008FA1 RID: 36769 RVA: 0x00323234 File Offset: 0x00321434
		public void IncrementDt(float dt)
		{
			this.m_timeSinceLastUpdate += dt;
			if (this.m_recentlyAdded.Count > 0)
			{
				foreach (T t in new List<T>(this.m_recentlyAdded.Keys))
				{
					Dictionary<T, float> recentlyAdded = this.m_recentlyAdded;
					T key = t;
					recentlyAdded[key] += dt;
				}
			}
		}

		// Token: 0x0400703C RID: 28732
		private float m_timeSinceLastUpdate;

		// Token: 0x0400703D RID: 28733
		private List<T> m_updateList = new List<T>();

		// Token: 0x0400703E RID: 28734
		private Dictionary<T, float> m_recentlyAdded = new Dictionary<T, float>();
	}
}
