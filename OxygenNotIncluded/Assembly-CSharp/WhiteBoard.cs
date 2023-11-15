using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000A2E RID: 2606
public class WhiteBoard : KGameObjectComponentManager<WhiteBoard.Data>, IKComponentManager
{
	// Token: 0x06004E12 RID: 19986 RVA: 0x001B5CEC File Offset: 0x001B3EEC
	public HandleVector<int>.Handle Add(GameObject go)
	{
		return base.Add(go, new WhiteBoard.Data
		{
			keyValueStore = new Dictionary<HashedString, object>()
		});
	}

	// Token: 0x06004E13 RID: 19987 RVA: 0x001B5D18 File Offset: 0x001B3F18
	protected override void OnCleanUp(HandleVector<int>.Handle h)
	{
		WhiteBoard.Data data = base.GetData(h);
		data.keyValueStore.Clear();
		data.keyValueStore = null;
		base.SetData(h, data);
	}

	// Token: 0x06004E14 RID: 19988 RVA: 0x001B5D48 File Offset: 0x001B3F48
	public bool HasValue(HandleVector<int>.Handle h, HashedString key)
	{
		return h.IsValid() && base.GetData(h).keyValueStore.ContainsKey(key);
	}

	// Token: 0x06004E15 RID: 19989 RVA: 0x001B5D67 File Offset: 0x001B3F67
	public object GetValue(HandleVector<int>.Handle h, HashedString key)
	{
		return base.GetData(h).keyValueStore[key];
	}

	// Token: 0x06004E16 RID: 19990 RVA: 0x001B5D7C File Offset: 0x001B3F7C
	public void SetValue(HandleVector<int>.Handle h, HashedString key, object value)
	{
		if (!h.IsValid())
		{
			return;
		}
		WhiteBoard.Data data = base.GetData(h);
		data.keyValueStore[key] = value;
		base.SetData(h, data);
	}

	// Token: 0x06004E17 RID: 19991 RVA: 0x001B5DB0 File Offset: 0x001B3FB0
	public void RemoveValue(HandleVector<int>.Handle h, HashedString key)
	{
		if (!h.IsValid())
		{
			return;
		}
		WhiteBoard.Data data = base.GetData(h);
		data.keyValueStore.Remove(key);
		base.SetData(h, data);
	}

	// Token: 0x020018B8 RID: 6328
	public struct Data
	{
		// Token: 0x040072C6 RID: 29382
		public Dictionary<HashedString, object> keyValueStore;
	}
}
