using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000173 RID: 371
public class TriggerCallback : MonoBehaviour
{
	// Token: 0x06000684 RID: 1668 RVA: 0x0002D1FC File Offset: 0x0002B5FC
	public void RegisterCallback(string _trigger, TriggerCallback.Callback _callback)
	{
		Dictionary<string, TriggerCallback.Callback> callbackRegistry;
		(callbackRegistry = this.m_callbackRegistry)[_trigger] = (TriggerCallback.Callback)Delegate.Combine(callbackRegistry[_trigger], _callback);
	}

	// Token: 0x06000685 RID: 1669 RVA: 0x0002D22C File Offset: 0x0002B62C
	public void UnregisterCallback(string _trigger, TriggerCallback.Callback _callback)
	{
		Dictionary<string, TriggerCallback.Callback> callbackRegistry;
		(callbackRegistry = this.m_callbackRegistry)[_trigger] = (TriggerCallback.Callback)Delegate.Remove(callbackRegistry[_trigger], _callback);
	}

	// Token: 0x06000686 RID: 1670 RVA: 0x0002D25C File Offset: 0x0002B65C
	private void Awake()
	{
		for (int i = 0; i < this.m_allowedTriggers.Length; i++)
		{
			this.m_callbackRegistry.Add(this.m_allowedTriggers[i], delegate
			{
			});
		}
	}

	// Token: 0x06000687 RID: 1671 RVA: 0x0002D2B2 File Offset: 0x0002B6B2
	private void OnTrigger(string _trigger)
	{
		if (this.m_callbackRegistry.ContainsKey(_trigger))
		{
			this.m_callbackRegistry[_trigger]();
		}
	}

	// Token: 0x04000574 RID: 1396
	[SerializeField]
	private string[] m_allowedTriggers;

	// Token: 0x04000575 RID: 1397
	private Dictionary<string, TriggerCallback.Callback> m_callbackRegistry = new Dictionary<string, TriggerCallback.Callback>();

	// Token: 0x02000174 RID: 372
	// (Invoke) Token: 0x0600068A RID: 1674
	public delegate void Callback();
}
