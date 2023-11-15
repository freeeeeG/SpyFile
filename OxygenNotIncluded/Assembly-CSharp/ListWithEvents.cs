using System;
using System.Collections;
using System.Collections.Generic;

// Token: 0x02000388 RID: 904
public class ListWithEvents<T> : IList<T>, ICollection<T>, IEnumerable<T>, IEnumerable
{
	// Token: 0x17000051 RID: 81
	// (get) Token: 0x0600129C RID: 4764 RVA: 0x00063F16 File Offset: 0x00062116
	public int Count
	{
		get
		{
			return this.internalList.Count;
		}
	}

	// Token: 0x17000052 RID: 82
	// (get) Token: 0x0600129D RID: 4765 RVA: 0x00063F23 File Offset: 0x00062123
	public bool IsReadOnly
	{
		get
		{
			return ((ICollection<T>)this.internalList).IsReadOnly;
		}
	}

	// Token: 0x14000002 RID: 2
	// (add) Token: 0x0600129E RID: 4766 RVA: 0x00063F30 File Offset: 0x00062130
	// (remove) Token: 0x0600129F RID: 4767 RVA: 0x00063F68 File Offset: 0x00062168
	public event Action<T> onAdd;

	// Token: 0x14000003 RID: 3
	// (add) Token: 0x060012A0 RID: 4768 RVA: 0x00063FA0 File Offset: 0x000621A0
	// (remove) Token: 0x060012A1 RID: 4769 RVA: 0x00063FD8 File Offset: 0x000621D8
	public event Action<T> onRemove;

	// Token: 0x17000053 RID: 83
	public T this[int index]
	{
		get
		{
			return this.internalList[index];
		}
		set
		{
			this.internalList[index] = value;
		}
	}

	// Token: 0x060012A4 RID: 4772 RVA: 0x0006402A File Offset: 0x0006222A
	public void Add(T item)
	{
		this.internalList.Add(item);
		if (this.onAdd != null)
		{
			this.onAdd(item);
		}
	}

	// Token: 0x060012A5 RID: 4773 RVA: 0x0006404C File Offset: 0x0006224C
	public void Insert(int index, T item)
	{
		this.internalList.Insert(index, item);
		if (this.onAdd != null)
		{
			this.onAdd(item);
		}
	}

	// Token: 0x060012A6 RID: 4774 RVA: 0x00064070 File Offset: 0x00062270
	public void RemoveAt(int index)
	{
		T obj = this.internalList[index];
		this.internalList.RemoveAt(index);
		if (this.onRemove != null)
		{
			this.onRemove(obj);
		}
	}

	// Token: 0x060012A7 RID: 4775 RVA: 0x000640AA File Offset: 0x000622AA
	public bool Remove(T item)
	{
		bool flag = this.internalList.Remove(item);
		if (flag && this.onRemove != null)
		{
			this.onRemove(item);
		}
		return flag;
	}

	// Token: 0x060012A8 RID: 4776 RVA: 0x000640CF File Offset: 0x000622CF
	public void Clear()
	{
		while (this.Count > 0)
		{
			this.RemoveAt(0);
		}
	}

	// Token: 0x060012A9 RID: 4777 RVA: 0x000640E3 File Offset: 0x000622E3
	public int IndexOf(T item)
	{
		return this.internalList.IndexOf(item);
	}

	// Token: 0x060012AA RID: 4778 RVA: 0x000640F1 File Offset: 0x000622F1
	public void CopyTo(T[] array, int arrayIndex)
	{
		this.internalList.CopyTo(array, arrayIndex);
	}

	// Token: 0x060012AB RID: 4779 RVA: 0x00064100 File Offset: 0x00062300
	public bool Contains(T item)
	{
		return this.internalList.Contains(item);
	}

	// Token: 0x060012AC RID: 4780 RVA: 0x0006410E File Offset: 0x0006230E
	public IEnumerator<T> GetEnumerator()
	{
		return this.internalList.GetEnumerator();
	}

	// Token: 0x060012AD RID: 4781 RVA: 0x00064120 File Offset: 0x00062320
	IEnumerator IEnumerable.GetEnumerator()
	{
		return this.internalList.GetEnumerator();
	}

	// Token: 0x04000A34 RID: 2612
	private List<T> internalList = new List<T>();
}
