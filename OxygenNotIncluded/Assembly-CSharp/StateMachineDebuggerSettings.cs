using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000437 RID: 1079
public class StateMachineDebuggerSettings : ScriptableObject
{
	// Token: 0x060016C7 RID: 5831 RVA: 0x00076229 File Offset: 0x00074429
	public IEnumerator<StateMachineDebuggerSettings.Entry> GetEnumerator()
	{
		return this.entries.GetEnumerator();
	}

	// Token: 0x060016C8 RID: 5832 RVA: 0x0007623B File Offset: 0x0007443B
	public static StateMachineDebuggerSettings Get()
	{
		if (StateMachineDebuggerSettings._Instance == null)
		{
			StateMachineDebuggerSettings._Instance = Resources.Load<StateMachineDebuggerSettings>("StateMachineDebuggerSettings");
			StateMachineDebuggerSettings._Instance.Initialize();
		}
		return StateMachineDebuggerSettings._Instance;
	}

	// Token: 0x060016C9 RID: 5833 RVA: 0x00076268 File Offset: 0x00074468
	private void Initialize()
	{
		foreach (Type type in App.GetCurrentDomainTypes())
		{
			if (typeof(StateMachine).IsAssignableFrom(type))
			{
				this.CreateEntry(type);
			}
		}
		this.entries.RemoveAll((StateMachineDebuggerSettings.Entry x) => x.type == null);
	}

	// Token: 0x060016CA RID: 5834 RVA: 0x000762F8 File Offset: 0x000744F8
	public StateMachineDebuggerSettings.Entry CreateEntry(Type type)
	{
		foreach (StateMachineDebuggerSettings.Entry entry in this.entries)
		{
			if (type.FullName == entry.typeName)
			{
				entry.type = type;
				return entry;
			}
		}
		StateMachineDebuggerSettings.Entry entry2 = new StateMachineDebuggerSettings.Entry(type);
		this.entries.Add(entry2);
		return entry2;
	}

	// Token: 0x060016CB RID: 5835 RVA: 0x00076378 File Offset: 0x00074578
	public void Clear()
	{
		this.entries.Clear();
		this.Initialize();
	}

	// Token: 0x04000CA7 RID: 3239
	public List<StateMachineDebuggerSettings.Entry> entries = new List<StateMachineDebuggerSettings.Entry>();

	// Token: 0x04000CA8 RID: 3240
	private static StateMachineDebuggerSettings _Instance;

	// Token: 0x020010C6 RID: 4294
	[Serializable]
	public class Entry
	{
		// Token: 0x06007770 RID: 30576 RVA: 0x002D38DC File Offset: 0x002D1ADC
		public Entry(Type type)
		{
			this.typeName = type.FullName;
			this.type = type;
		}

		// Token: 0x04005A05 RID: 23045
		public Type type;

		// Token: 0x04005A06 RID: 23046
		public string typeName;

		// Token: 0x04005A07 RID: 23047
		public bool breakOnGoTo;

		// Token: 0x04005A08 RID: 23048
		public bool enableConsoleLogging;

		// Token: 0x04005A09 RID: 23049
		public bool saveHistory;
	}
}
