using System;
using System.Collections.Generic;

// Token: 0x020004FF RID: 1279
public class DataStore : Manager
{
	// Token: 0x060017D8 RID: 6104 RVA: 0x00079F4B File Offset: 0x0007834B
	private void Awake()
	{
		this.m_data = new Dictionary<DataStore.Id, object>(32);
		this.m_listeners = new Dictionary<DataStore.Id, FastList<DataStore.OnChangeNotification>>(32);
	}

	// Token: 0x060017D9 RID: 6105 RVA: 0x00079F67 File Offset: 0x00078367
	private void OnDestroy()
	{
		this.Purge();
	}

	// Token: 0x060017DA RID: 6106 RVA: 0x00079F6F File Offset: 0x0007836F
	public void Purge()
	{
		this.m_data.Clear();
		this.m_listeners.Clear();
	}

	// Token: 0x060017DB RID: 6107 RVA: 0x00079F87 File Offset: 0x00078387
	public void Write(DataStore.Id id, object data)
	{
		this.m_data[id] = data;
		this.Emit(id, data);
	}

	// Token: 0x060017DC RID: 6108 RVA: 0x00079FA0 File Offset: 0x000783A0
	private void Emit(DataStore.Id id, object data)
	{
		if (this.m_listeners.ContainsKey(id))
		{
			FastList<DataStore.OnChangeNotification> fastList = this.m_listeners[id];
			for (int i = 0; i < fastList.Count; i++)
			{
				fastList._items[i](id, data);
			}
		}
	}

	// Token: 0x060017DD RID: 6109 RVA: 0x00079FF1 File Offset: 0x000783F1
	public void Register(DataStore.Id id, DataStore.OnChangeNotification listener)
	{
		if (!this.m_listeners.ContainsKey(id))
		{
			this.m_listeners[id] = new FastList<DataStore.OnChangeNotification>(8);
		}
		this.m_listeners[id].Add(listener);
	}

	// Token: 0x060017DE RID: 6110 RVA: 0x0007A028 File Offset: 0x00078428
	public void Unregister(DataStore.Id id, DataStore.OnChangeNotification listener)
	{
		if (this.m_listeners.ContainsKey(id))
		{
			this.m_listeners[id].Remove(listener);
		}
	}

	// Token: 0x0400118A RID: 4490
	private Dictionary<DataStore.Id, object> m_data;

	// Token: 0x0400118B RID: 4491
	private Dictionary<DataStore.Id, FastList<DataStore.OnChangeNotification>> m_listeners;

	// Token: 0x02000500 RID: 1280
	public struct Id : IEquatable<DataStore.Id>
	{
		// Token: 0x060017DF RID: 6111 RVA: 0x0007A04E File Offset: 0x0007844E
		public Id(int id)
		{
			this.m_id = id;
		}

		// Token: 0x060017E0 RID: 6112 RVA: 0x0007A057 File Offset: 0x00078457
		public Id(object id)
		{
			this.m_id = id.GetHashCode();
		}

		// Token: 0x060017E1 RID: 6113 RVA: 0x0007A065 File Offset: 0x00078465
		public override int GetHashCode()
		{
			return this.m_id;
		}

		// Token: 0x060017E2 RID: 6114 RVA: 0x0007A070 File Offset: 0x00078470
		public override bool Equals(object obj)
		{
			return this.m_id == ((DataStore.Id)obj).m_id;
		}

		// Token: 0x060017E3 RID: 6115 RVA: 0x0007A093 File Offset: 0x00078493
		public bool Equals(DataStore.Id other)
		{
			return this.m_id == other.m_id;
		}

		// Token: 0x060017E4 RID: 6116 RVA: 0x0007A0A4 File Offset: 0x000784A4
		public override string ToString()
		{
			return "Id(" + this.m_id.ToString() + ")";
		}

		// Token: 0x060017E5 RID: 6117 RVA: 0x0007A0C6 File Offset: 0x000784C6
		public static bool operator ==(DataStore.Id lhs, DataStore.Id rhs)
		{
			return lhs.m_id == rhs.m_id;
		}

		// Token: 0x060017E6 RID: 6118 RVA: 0x0007A0D8 File Offset: 0x000784D8
		public static bool operator !=(DataStore.Id lhs, DataStore.Id rhs)
		{
			return lhs.m_id != rhs.m_id;
		}

		// Token: 0x060017E7 RID: 6119 RVA: 0x0007A0ED File Offset: 0x000784ED
		public bool IsValid()
		{
			return this.m_id != 0;
		}

		// Token: 0x0400118C RID: 4492
		public static readonly DataStore.Id Invalid = default(DataStore.Id);

		// Token: 0x0400118D RID: 4493
		private const int InvalidId = 0;

		// Token: 0x0400118E RID: 4494
		private int m_id;
	}

	// Token: 0x02000501 RID: 1281
	// (Invoke) Token: 0x060017EA RID: 6122
	public delegate void OnChangeNotification(DataStore.Id id, object data);
}
