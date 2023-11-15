using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

// Token: 0x020000FF RID: 255
public class SerializableDictionary<TKey, TValue> : SerializableDictionaryBase<TKey, TValue, TValue>
{
	// Token: 0x0600066D RID: 1645 RVA: 0x00018046 File Offset: 0x00016246
	public SerializableDictionary()
	{
	}

	// Token: 0x0600066E RID: 1646 RVA: 0x0001804E File Offset: 0x0001624E
	public SerializableDictionary(IDictionary<TKey, TValue> dict) : base(dict)
	{
	}

	// Token: 0x0600066F RID: 1647 RVA: 0x00018057 File Offset: 0x00016257
	protected SerializableDictionary(SerializationInfo info, StreamingContext context) : base(info, context)
	{
	}

	// Token: 0x06000670 RID: 1648 RVA: 0x00018061 File Offset: 0x00016261
	protected override TValue GetValue(TValue[] storage, int i)
	{
		return storage[i];
	}

	// Token: 0x06000671 RID: 1649 RVA: 0x0001806A File Offset: 0x0001626A
	protected override void SetValue(TValue[] storage, int i, TValue value)
	{
		storage[i] = value;
	}
}
