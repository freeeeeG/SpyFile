using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;

// Token: 0x020000FD RID: 253
[Serializable]
public abstract class SerializableDictionaryBase<TKey, TValue, TValueStorage> : SerializableDictionaryBase, IDictionary<TKey, TValue>, ICollection<KeyValuePair<TKey, TValue>>, IEnumerable<KeyValuePair<TKey, TValue>>, IEnumerable, IDictionary, ICollection, ISerializationCallbackReceiver, IDeserializationCallback, ISerializable
{
	// Token: 0x06000646 RID: 1606 RVA: 0x00017CCF File Offset: 0x00015ECF
	public SerializableDictionaryBase()
	{
		this.m_dict = new SerializableDictionaryBase.Dictionary<TKey, TValue>();
	}

	// Token: 0x06000647 RID: 1607 RVA: 0x00017CE2 File Offset: 0x00015EE2
	public SerializableDictionaryBase(IDictionary<TKey, TValue> dict)
	{
		this.m_dict = new SerializableDictionaryBase.Dictionary<TKey, TValue>(dict);
	}

	// Token: 0x06000648 RID: 1608
	protected abstract void SetValue(TValueStorage[] storage, int i, TValue value);

	// Token: 0x06000649 RID: 1609
	protected abstract TValue GetValue(TValueStorage[] storage, int i);

	// Token: 0x0600064A RID: 1610 RVA: 0x00017CF8 File Offset: 0x00015EF8
	public void CopyFrom(IDictionary<TKey, TValue> dict)
	{
		this.m_dict.Clear();
		foreach (KeyValuePair<TKey, TValue> keyValuePair in dict)
		{
			this.m_dict[keyValuePair.Key] = keyValuePair.Value;
		}
	}

	// Token: 0x0600064B RID: 1611 RVA: 0x00017D60 File Offset: 0x00015F60
	public void OnAfterDeserialize()
	{
		if (this.m_keys != null && this.m_values != null && this.m_keys.Length == this.m_values.Length)
		{
			this.m_dict.Clear();
			int num = this.m_keys.Length;
			for (int i = 0; i < num; i++)
			{
				this.m_dict[this.m_keys[i]] = this.GetValue(this.m_values, i);
			}
			this.m_keys = null;
			this.m_values = null;
		}
	}

	// Token: 0x0600064C RID: 1612 RVA: 0x00017DE4 File Offset: 0x00015FE4
	public void OnBeforeSerialize()
	{
		int count = this.m_dict.Count;
		this.m_keys = new TKey[count];
		this.m_values = new TValueStorage[count];
		int num = 0;
		foreach (KeyValuePair<TKey, TValue> keyValuePair in this.m_dict)
		{
			this.m_keys[num] = keyValuePair.Key;
			this.SetValue(this.m_values, num, keyValuePair.Value);
			num++;
		}
	}

	// Token: 0x1700007B RID: 123
	// (get) Token: 0x0600064D RID: 1613 RVA: 0x00017E84 File Offset: 0x00016084
	public ICollection<TKey> Keys
	{
		get
		{
			return ((IDictionary<TKey, TValue>)this.m_dict).Keys;
		}
	}

	// Token: 0x1700007C RID: 124
	// (get) Token: 0x0600064E RID: 1614 RVA: 0x00017E91 File Offset: 0x00016091
	public ICollection<TValue> Values
	{
		get
		{
			return ((IDictionary<TKey, TValue>)this.m_dict).Values;
		}
	}

	// Token: 0x1700007D RID: 125
	// (get) Token: 0x0600064F RID: 1615 RVA: 0x00017E9E File Offset: 0x0001609E
	public int Count
	{
		get
		{
			return ((ICollection<KeyValuePair<TKey, TValue>>)this.m_dict).Count;
		}
	}

	// Token: 0x1700007E RID: 126
	// (get) Token: 0x06000650 RID: 1616 RVA: 0x00017EAB File Offset: 0x000160AB
	public bool IsReadOnly
	{
		get
		{
			return ((ICollection<KeyValuePair<TKey, TValue>>)this.m_dict).IsReadOnly;
		}
	}

	// Token: 0x1700007F RID: 127
	public TValue this[TKey key]
	{
		get
		{
			return ((IDictionary<TKey, TValue>)this.m_dict)[key];
		}
		set
		{
			((IDictionary<TKey, TValue>)this.m_dict)[key] = value;
		}
	}

	// Token: 0x06000653 RID: 1619 RVA: 0x00017ED5 File Offset: 0x000160D5
	public void Add(TKey key, TValue value)
	{
		((IDictionary<TKey, TValue>)this.m_dict).Add(key, value);
	}

	// Token: 0x06000654 RID: 1620 RVA: 0x00017EE4 File Offset: 0x000160E4
	public bool ContainsKey(TKey key)
	{
		return ((IDictionary<TKey, TValue>)this.m_dict).ContainsKey(key);
	}

	// Token: 0x06000655 RID: 1621 RVA: 0x00017EF2 File Offset: 0x000160F2
	public bool Remove(TKey key)
	{
		return ((IDictionary<TKey, TValue>)this.m_dict).Remove(key);
	}

	// Token: 0x06000656 RID: 1622 RVA: 0x00017F00 File Offset: 0x00016100
	public bool TryGetValue(TKey key, out TValue value)
	{
		return ((IDictionary<TKey, TValue>)this.m_dict).TryGetValue(key, out value);
	}

	// Token: 0x06000657 RID: 1623 RVA: 0x00017F0F File Offset: 0x0001610F
	public void Add(KeyValuePair<TKey, TValue> item)
	{
		((ICollection<KeyValuePair<TKey, TValue>>)this.m_dict).Add(item);
	}

	// Token: 0x06000658 RID: 1624 RVA: 0x00017F1D File Offset: 0x0001611D
	public void Clear()
	{
		((ICollection<KeyValuePair<TKey, TValue>>)this.m_dict).Clear();
	}

	// Token: 0x06000659 RID: 1625 RVA: 0x00017F2A File Offset: 0x0001612A
	public bool Contains(KeyValuePair<TKey, TValue> item)
	{
		return ((ICollection<KeyValuePair<TKey, TValue>>)this.m_dict).Contains(item);
	}

	// Token: 0x0600065A RID: 1626 RVA: 0x00017F38 File Offset: 0x00016138
	public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
	{
		((ICollection<KeyValuePair<TKey, TValue>>)this.m_dict).CopyTo(array, arrayIndex);
	}

	// Token: 0x0600065B RID: 1627 RVA: 0x00017F47 File Offset: 0x00016147
	public bool Remove(KeyValuePair<TKey, TValue> item)
	{
		return ((ICollection<KeyValuePair<TKey, TValue>>)this.m_dict).Remove(item);
	}

	// Token: 0x0600065C RID: 1628 RVA: 0x00017F55 File Offset: 0x00016155
	public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
	{
		return ((IEnumerable<KeyValuePair<TKey, TValue>>)this.m_dict).GetEnumerator();
	}

	// Token: 0x0600065D RID: 1629 RVA: 0x00017F62 File Offset: 0x00016162
	IEnumerator IEnumerable.GetEnumerator()
	{
		return ((IEnumerable<KeyValuePair<TKey, TValue>>)this.m_dict).GetEnumerator();
	}

	// Token: 0x17000080 RID: 128
	// (get) Token: 0x0600065E RID: 1630 RVA: 0x00017F6F File Offset: 0x0001616F
	public bool IsFixedSize
	{
		get
		{
			return ((IDictionary)this.m_dict).IsFixedSize;
		}
	}

	// Token: 0x17000081 RID: 129
	// (get) Token: 0x0600065F RID: 1631 RVA: 0x00017F7C File Offset: 0x0001617C
	ICollection IDictionary.Keys
	{
		get
		{
			return ((IDictionary)this.m_dict).Keys;
		}
	}

	// Token: 0x17000082 RID: 130
	// (get) Token: 0x06000660 RID: 1632 RVA: 0x00017F89 File Offset: 0x00016189
	ICollection IDictionary.Values
	{
		get
		{
			return ((IDictionary)this.m_dict).Values;
		}
	}

	// Token: 0x17000083 RID: 131
	// (get) Token: 0x06000661 RID: 1633 RVA: 0x00017F96 File Offset: 0x00016196
	public bool IsSynchronized
	{
		get
		{
			return ((ICollection)this.m_dict).IsSynchronized;
		}
	}

	// Token: 0x17000084 RID: 132
	// (get) Token: 0x06000662 RID: 1634 RVA: 0x00017FA3 File Offset: 0x000161A3
	public object SyncRoot
	{
		get
		{
			return ((ICollection)this.m_dict).SyncRoot;
		}
	}

	// Token: 0x17000085 RID: 133
	public object this[object key]
	{
		get
		{
			return ((IDictionary)this.m_dict)[key];
		}
		set
		{
			((IDictionary)this.m_dict)[key] = value;
		}
	}

	// Token: 0x06000665 RID: 1637 RVA: 0x00017FCD File Offset: 0x000161CD
	public void Add(object key, object value)
	{
		((IDictionary)this.m_dict).Add(key, value);
	}

	// Token: 0x06000666 RID: 1638 RVA: 0x00017FDC File Offset: 0x000161DC
	public bool Contains(object key)
	{
		return ((IDictionary)this.m_dict).Contains(key);
	}

	// Token: 0x06000667 RID: 1639 RVA: 0x00017FEA File Offset: 0x000161EA
	IDictionaryEnumerator IDictionary.GetEnumerator()
	{
		return ((IDictionary)this.m_dict).GetEnumerator();
	}

	// Token: 0x06000668 RID: 1640 RVA: 0x00017FF7 File Offset: 0x000161F7
	public void Remove(object key)
	{
		((IDictionary)this.m_dict).Remove(key);
	}

	// Token: 0x06000669 RID: 1641 RVA: 0x00018005 File Offset: 0x00016205
	public void CopyTo(Array array, int index)
	{
		((ICollection)this.m_dict).CopyTo(array, index);
	}

	// Token: 0x0600066A RID: 1642 RVA: 0x00018014 File Offset: 0x00016214
	public void OnDeserialization(object sender)
	{
		((IDeserializationCallback)this.m_dict).OnDeserialization(sender);
	}

	// Token: 0x0600066B RID: 1643 RVA: 0x00018022 File Offset: 0x00016222
	protected SerializableDictionaryBase(SerializationInfo info, StreamingContext context)
	{
		this.m_dict = new SerializableDictionaryBase.Dictionary<TKey, TValue>(info, context);
	}

	// Token: 0x0600066C RID: 1644 RVA: 0x00018037 File Offset: 0x00016237
	public void GetObjectData(SerializationInfo info, StreamingContext context)
	{
		((ISerializable)this.m_dict).GetObjectData(info, context);
	}

	// Token: 0x0400054F RID: 1359
	private SerializableDictionaryBase.Dictionary<TKey, TValue> m_dict;

	// Token: 0x04000550 RID: 1360
	[SerializeField]
	private TKey[] m_keys;

	// Token: 0x04000551 RID: 1361
	[SerializeField]
	private TValueStorage[] m_values;
}
