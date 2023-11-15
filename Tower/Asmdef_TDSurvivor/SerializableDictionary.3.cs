using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

// Token: 0x02000100 RID: 256
public class SerializableDictionary<TKey, TValue, TValueStorage> : SerializableDictionaryBase<TKey, TValue, TValueStorage> where TValueStorage : SerializableDictionary.Storage<TValue>, new()
{
	// Token: 0x06000672 RID: 1650 RVA: 0x00018074 File Offset: 0x00016274
	public SerializableDictionary()
	{
	}

	// Token: 0x06000673 RID: 1651 RVA: 0x0001807C File Offset: 0x0001627C
	public SerializableDictionary(IDictionary<TKey, TValue> dict) : base(dict)
	{
	}

	// Token: 0x06000674 RID: 1652 RVA: 0x00018085 File Offset: 0x00016285
	protected SerializableDictionary(SerializationInfo info, StreamingContext context) : base(info, context)
	{
	}

	// Token: 0x06000675 RID: 1653 RVA: 0x0001808F File Offset: 0x0001628F
	protected override TValue GetValue(TValueStorage[] storage, int i)
	{
		return storage[i].data;
	}

	// Token: 0x06000676 RID: 1654 RVA: 0x000180A2 File Offset: 0x000162A2
	protected override void SetValue(TValueStorage[] storage, int i, TValue value)
	{
		storage[i] = Activator.CreateInstance<TValueStorage>();
		storage[i].data = value;
	}
}
