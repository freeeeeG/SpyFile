using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;

// Token: 0x02000102 RID: 258
[Serializable]
public abstract class SerializableHashSet<T> : SerializableHashSetBase, ISet<T>, ICollection<T>, IEnumerable<T>, IEnumerable, ISerializationCallbackReceiver, IDeserializationCallback, ISerializable
{
	// Token: 0x06000678 RID: 1656 RVA: 0x000180CA File Offset: 0x000162CA
	public SerializableHashSet()
	{
		this.m_hashSet = new SerializableHashSetBase.HashSet<T>();
	}

	// Token: 0x06000679 RID: 1657 RVA: 0x000180DD File Offset: 0x000162DD
	public SerializableHashSet(ISet<T> set)
	{
		this.m_hashSet = new SerializableHashSetBase.HashSet<T>(set);
	}

	// Token: 0x0600067A RID: 1658 RVA: 0x000180F4 File Offset: 0x000162F4
	public void CopyFrom(ISet<T> set)
	{
		this.m_hashSet.Clear();
		foreach (T item in set)
		{
			this.m_hashSet.Add(item);
		}
	}

	// Token: 0x0600067B RID: 1659 RVA: 0x00018150 File Offset: 0x00016350
	public void OnAfterDeserialize()
	{
		if (this.m_keys != null)
		{
			this.m_hashSet.Clear();
			int num = this.m_keys.Length;
			for (int i = 0; i < num; i++)
			{
				this.m_hashSet.Add(this.m_keys[i]);
			}
			this.m_keys = null;
		}
	}

	// Token: 0x0600067C RID: 1660 RVA: 0x000181A4 File Offset: 0x000163A4
	public void OnBeforeSerialize()
	{
		int count = this.m_hashSet.Count;
		this.m_keys = new T[count];
		int num = 0;
		foreach (T t in this.m_hashSet)
		{
			this.m_keys[num] = t;
			num++;
		}
	}

	// Token: 0x17000086 RID: 134
	// (get) Token: 0x0600067D RID: 1661 RVA: 0x0001821C File Offset: 0x0001641C
	public int Count
	{
		get
		{
			return ((ICollection<T>)this.m_hashSet).Count;
		}
	}

	// Token: 0x17000087 RID: 135
	// (get) Token: 0x0600067E RID: 1662 RVA: 0x00018229 File Offset: 0x00016429
	public bool IsReadOnly
	{
		get
		{
			return ((ICollection<T>)this.m_hashSet).IsReadOnly;
		}
	}

	// Token: 0x0600067F RID: 1663 RVA: 0x00018236 File Offset: 0x00016436
	public bool Add(T item)
	{
		return ((ISet<T>)this.m_hashSet).Add(item);
	}

	// Token: 0x06000680 RID: 1664 RVA: 0x00018244 File Offset: 0x00016444
	public void ExceptWith(IEnumerable<T> other)
	{
		((ISet<T>)this.m_hashSet).ExceptWith(other);
	}

	// Token: 0x06000681 RID: 1665 RVA: 0x00018252 File Offset: 0x00016452
	public void IntersectWith(IEnumerable<T> other)
	{
		((ISet<T>)this.m_hashSet).IntersectWith(other);
	}

	// Token: 0x06000682 RID: 1666 RVA: 0x00018260 File Offset: 0x00016460
	public bool IsProperSubsetOf(IEnumerable<T> other)
	{
		return ((ISet<T>)this.m_hashSet).IsProperSubsetOf(other);
	}

	// Token: 0x06000683 RID: 1667 RVA: 0x0001826E File Offset: 0x0001646E
	public bool IsProperSupersetOf(IEnumerable<T> other)
	{
		return ((ISet<T>)this.m_hashSet).IsProperSupersetOf(other);
	}

	// Token: 0x06000684 RID: 1668 RVA: 0x0001827C File Offset: 0x0001647C
	public bool IsSubsetOf(IEnumerable<T> other)
	{
		return ((ISet<T>)this.m_hashSet).IsSubsetOf(other);
	}

	// Token: 0x06000685 RID: 1669 RVA: 0x0001828A File Offset: 0x0001648A
	public bool IsSupersetOf(IEnumerable<T> other)
	{
		return ((ISet<T>)this.m_hashSet).IsSupersetOf(other);
	}

	// Token: 0x06000686 RID: 1670 RVA: 0x00018298 File Offset: 0x00016498
	public bool Overlaps(IEnumerable<T> other)
	{
		return ((ISet<T>)this.m_hashSet).Overlaps(other);
	}

	// Token: 0x06000687 RID: 1671 RVA: 0x000182A6 File Offset: 0x000164A6
	public bool SetEquals(IEnumerable<T> other)
	{
		return ((ISet<T>)this.m_hashSet).SetEquals(other);
	}

	// Token: 0x06000688 RID: 1672 RVA: 0x000182B4 File Offset: 0x000164B4
	public void SymmetricExceptWith(IEnumerable<T> other)
	{
		((ISet<T>)this.m_hashSet).SymmetricExceptWith(other);
	}

	// Token: 0x06000689 RID: 1673 RVA: 0x000182C2 File Offset: 0x000164C2
	public void UnionWith(IEnumerable<T> other)
	{
		((ISet<T>)this.m_hashSet).UnionWith(other);
	}

	// Token: 0x0600068A RID: 1674 RVA: 0x000182D0 File Offset: 0x000164D0
	void ICollection<!0>.Add(T item)
	{
		((ISet<T>)this.m_hashSet).Add(item);
	}

	// Token: 0x0600068B RID: 1675 RVA: 0x000182DF File Offset: 0x000164DF
	public void Clear()
	{
		((ICollection<!0>)this.m_hashSet).Clear();
	}

	// Token: 0x0600068C RID: 1676 RVA: 0x000182EC File Offset: 0x000164EC
	public bool Contains(T item)
	{
		return ((ICollection<!0>)this.m_hashSet).Contains(item);
	}

	// Token: 0x0600068D RID: 1677 RVA: 0x000182FA File Offset: 0x000164FA
	public void CopyTo(T[] array, int arrayIndex)
	{
		((ICollection<!0>)this.m_hashSet).CopyTo(array, arrayIndex);
	}

	// Token: 0x0600068E RID: 1678 RVA: 0x00018309 File Offset: 0x00016509
	public bool Remove(T item)
	{
		return ((ICollection<!0>)this.m_hashSet).Remove(item);
	}

	// Token: 0x0600068F RID: 1679 RVA: 0x00018317 File Offset: 0x00016517
	public IEnumerator<T> GetEnumerator()
	{
		return ((IEnumerable<T>)this.m_hashSet).GetEnumerator();
	}

	// Token: 0x06000690 RID: 1680 RVA: 0x00018324 File Offset: 0x00016524
	IEnumerator IEnumerable.GetEnumerator()
	{
		return ((IEnumerable<T>)this.m_hashSet).GetEnumerator();
	}

	// Token: 0x06000691 RID: 1681 RVA: 0x00018331 File Offset: 0x00016531
	public void OnDeserialization(object sender)
	{
		((IDeserializationCallback)this.m_hashSet).OnDeserialization(sender);
	}

	// Token: 0x06000692 RID: 1682 RVA: 0x0001833F File Offset: 0x0001653F
	protected SerializableHashSet(SerializationInfo info, StreamingContext context)
	{
		this.m_hashSet = new SerializableHashSetBase.HashSet<T>(info, context);
	}

	// Token: 0x06000693 RID: 1683 RVA: 0x00018354 File Offset: 0x00016554
	public void GetObjectData(SerializationInfo info, StreamingContext context)
	{
		((ISerializable)this.m_hashSet).GetObjectData(info, context);
	}

	// Token: 0x04000552 RID: 1362
	private SerializableHashSetBase.HashSet<T> m_hashSet;

	// Token: 0x04000553 RID: 1363
	[SerializeField]
	private T[] m_keys;
}
