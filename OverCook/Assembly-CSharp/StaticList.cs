using System;
using System.Collections.Generic;
using System.Diagnostics;

// Token: 0x02000284 RID: 644
public class StaticList<T>
{
	// Token: 0x14000004 RID: 4
	// (add) Token: 0x06000BD5 RID: 3029 RVA: 0x0003DCCC File Offset: 0x0003C0CC
	// (remove) Token: 0x06000BD6 RID: 3030 RVA: 0x0003DD04 File Offset: 0x0003C104
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event VoidGeneric<int> OnObjectsChanged;

	// Token: 0x14000005 RID: 5
	// (add) Token: 0x06000BD7 RID: 3031 RVA: 0x0003DD3C File Offset: 0x0003C13C
	// (remove) Token: 0x06000BD8 RID: 3032 RVA: 0x0003DD74 File Offset: 0x0003C174
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event VoidGeneric<T> OnObjectAdded;

	// Token: 0x14000006 RID: 6
	// (add) Token: 0x06000BD9 RID: 3033 RVA: 0x0003DDAC File Offset: 0x0003C1AC
	// (remove) Token: 0x06000BDA RID: 3034 RVA: 0x0003DDE4 File Offset: 0x0003C1E4
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event VoidGeneric<T> OnObjectRemoved;

	// Token: 0x17000126 RID: 294
	// (get) Token: 0x06000BDB RID: 3035 RVA: 0x0003DE1A File Offset: 0x0003C21A
	public int Count
	{
		get
		{
			return this.m_objects.Count;
		}
	}

	// Token: 0x06000BDC RID: 3036 RVA: 0x0003DE27 File Offset: 0x0003C227
	public IEnumerable<T> GetContents()
	{
		return this.m_objects;
	}

	// Token: 0x06000BDD RID: 3037 RVA: 0x0003DE30 File Offset: 0x0003C230
	public void Add(T _f)
	{
		this.m_objects.Add(_f);
		if (this.OnObjectsChanged != null)
		{
			this.OnObjectsChanged(this.m_objects.Count);
		}
		if (this.OnObjectAdded != null)
		{
			this.OnObjectAdded(_f);
		}
	}

	// Token: 0x06000BDE RID: 3038 RVA: 0x0003DE84 File Offset: 0x0003C284
	public void Remove(T _f)
	{
		this.m_objects.Remove(_f);
		if (this.OnObjectsChanged != null)
		{
			this.OnObjectsChanged(this.m_objects.Count);
		}
		if (this.OnObjectRemoved != null)
		{
			this.OnObjectRemoved(_f);
		}
	}

	// Token: 0x04000908 RID: 2312
	private List<T> m_objects = new List<T>();
}
