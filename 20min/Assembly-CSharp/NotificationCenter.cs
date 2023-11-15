using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200003E RID: 62
public class NotificationCenter
{
	// Token: 0x060003D6 RID: 982 RVA: 0x00014D8C File Offset: 0x00012F8C
	private NotificationCenter()
	{
	}

	// Token: 0x060003D7 RID: 983 RVA: 0x00014DAA File Offset: 0x00012FAA
	public void AddObserver(Action<object, object> handler, string notificationName)
	{
		this.AddObserver(handler, notificationName, null);
	}

	// Token: 0x060003D8 RID: 984 RVA: 0x00014DB8 File Offset: 0x00012FB8
	public void AddObserver(Action<object, object> handler, string notificationName, object sender)
	{
		if (handler == null)
		{
			Debug.LogError("Can't add a null event handler for notification, " + notificationName);
			return;
		}
		if (string.IsNullOrEmpty(notificationName))
		{
			Debug.LogError("Can't observe an unnamed notification");
			return;
		}
		if (!this._table.ContainsKey(notificationName))
		{
			this._table.Add(notificationName, new Dictionary<object, List<Action<object, object>>>());
		}
		Dictionary<object, List<Action<object, object>>> dictionary = this._table[notificationName];
		object key = (sender != null) ? sender : this;
		if (!dictionary.ContainsKey(key))
		{
			dictionary.Add(key, new List<Action<object, object>>());
		}
		List<Action<object, object>> list = dictionary[key];
		if (this._invoking.Contains(list))
		{
			list = (dictionary[key] = new List<Action<object, object>>(list));
		}
		list.Add(handler);
	}

	// Token: 0x060003D9 RID: 985 RVA: 0x00014E61 File Offset: 0x00013061
	public void RemoveObserver(Action<object, object> handler, string notificationName)
	{
		this.RemoveObserver(handler, notificationName, null);
	}

	// Token: 0x060003DA RID: 986 RVA: 0x00014E6C File Offset: 0x0001306C
	public void RemoveObserver(Action<object, object> handler, string notificationName, object sender)
	{
		if (handler == null)
		{
			Debug.LogError("Can't remove a null event handler for notification, " + notificationName);
			return;
		}
		if (string.IsNullOrEmpty(notificationName))
		{
			Debug.LogError("A notification name is required to stop observation");
			return;
		}
		if (!this._table.ContainsKey(notificationName))
		{
			return;
		}
		Dictionary<object, List<Action<object, object>>> dictionary = this._table[notificationName];
		object key = (sender != null) ? sender : this;
		if (!dictionary.ContainsKey(key))
		{
			return;
		}
		List<Action<object, object>> list = dictionary[key];
		int num = list.IndexOf(handler);
		if (num != -1)
		{
			if (this._invoking.Contains(list))
			{
				list = (dictionary[key] = new List<Action<object, object>>(list));
			}
			list.RemoveAt(num);
		}
	}

	// Token: 0x060003DB RID: 987 RVA: 0x00014F08 File Offset: 0x00013108
	public void Clean()
	{
		string[] array = new string[this._table.Keys.Count];
		this._table.Keys.CopyTo(array, 0);
		for (int i = array.Length - 1; i >= 0; i--)
		{
			string key = array[i];
			Dictionary<object, List<Action<object, object>>> dictionary = this._table[key];
			object[] array2 = new object[dictionary.Keys.Count];
			dictionary.Keys.CopyTo(array2, 0);
			for (int j = array2.Length - 1; j >= 0; j--)
			{
				object key2 = array2[j];
				if (dictionary[key2].Count == 0)
				{
					dictionary.Remove(key2);
				}
			}
			if (dictionary.Count == 0)
			{
				this._table.Remove(key);
			}
		}
	}

	// Token: 0x060003DC RID: 988 RVA: 0x00014FCA File Offset: 0x000131CA
	public void PostNotification(string notificationName)
	{
		this.PostNotification(notificationName, null);
	}

	// Token: 0x060003DD RID: 989 RVA: 0x00014FD4 File Offset: 0x000131D4
	public void PostNotification(string notificationName, object sender)
	{
		this.PostNotification(notificationName, sender, null);
	}

	// Token: 0x060003DE RID: 990 RVA: 0x00014FE0 File Offset: 0x000131E0
	public void PostNotification(string notificationName, object sender, object e)
	{
		if (string.IsNullOrEmpty(notificationName))
		{
			Debug.LogError("A notification name is required");
			return;
		}
		if (!this._table.ContainsKey(notificationName))
		{
			return;
		}
		Dictionary<object, List<Action<object, object>>> dictionary = this._table[notificationName];
		if (sender != null && dictionary.ContainsKey(sender))
		{
			List<Action<object, object>> list = dictionary[sender];
			this._invoking.Add(list);
			for (int i = 0; i < list.Count; i++)
			{
				list[i](sender, e);
			}
			this._invoking.Remove(list);
		}
		if (dictionary.ContainsKey(this))
		{
			List<Action<object, object>> list2 = dictionary[this];
			this._invoking.Add(list2);
			for (int j = 0; j < list2.Count; j++)
			{
				list2[j](sender, e);
			}
			this._invoking.Remove(list2);
		}
	}

	// Token: 0x040001D9 RID: 473
	private Dictionary<string, Dictionary<object, List<Action<object, object>>>> _table = new Dictionary<string, Dictionary<object, List<Action<object, object>>>>();

	// Token: 0x040001DA RID: 474
	private HashSet<List<Action<object, object>>> _invoking = new HashSet<List<Action<object, object>>>();

	// Token: 0x040001DB RID: 475
	public static readonly NotificationCenter instance = new NotificationCenter();
}
