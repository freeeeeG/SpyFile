using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using UnityEngine;

// Token: 0x02000AFA RID: 2810
public class EntryDevLog
{
	// Token: 0x060056BE RID: 22206 RVA: 0x001FAF7C File Offset: 0x001F917C
	[Conditional("UNITY_EDITOR")]
	public void AddModificationRecord(EntryDevLog.ModificationRecord.ActionType actionType, string target, object newValue)
	{
		string author = this.TrimAuthor();
		this.modificationRecords.Add(new EntryDevLog.ModificationRecord(actionType, target, newValue, author));
	}

	// Token: 0x060056BF RID: 22207 RVA: 0x001FAFA4 File Offset: 0x001F91A4
	[Conditional("UNITY_EDITOR")]
	public void InsertModificationRecord(int index, EntryDevLog.ModificationRecord.ActionType actionType, string target, object newValue)
	{
		string author = this.TrimAuthor();
		this.modificationRecords.Insert(index, new EntryDevLog.ModificationRecord(actionType, target, newValue, author));
	}

	// Token: 0x060056C0 RID: 22208 RVA: 0x001FAFD0 File Offset: 0x001F91D0
	private string TrimAuthor()
	{
		string text = "";
		string[] array = new string[]
		{
			"Invoke",
			"CreateInstance",
			"AwakeInternal",
			"Internal",
			"<>",
			"YamlDotNet",
			"Deserialize"
		};
		string[] array2 = new string[]
		{
			".ctor",
			"Trigger",
			"AddContentContainerRange",
			"AddContentContainer",
			"InsertContentContainer",
			"KInstantiateUI",
			"Start",
			"InitializeComponentAwake",
			"TrimAuthor",
			"InsertModificationRecord",
			"AddModificationRecord",
			"SetValue",
			"Write"
		};
		StackTrace stackTrace = new StackTrace();
		int i = 0;
		int num = 0;
		int num2 = 3;
		while (i < num2)
		{
			num++;
			if (stackTrace.FrameCount <= num)
			{
				break;
			}
			MethodBase method = stackTrace.GetFrame(num).GetMethod();
			bool flag = false;
			for (int j = 0; j < array.Length; j++)
			{
				flag = (flag || method.Name.Contains(array[j]));
			}
			for (int k = 0; k < array2.Length; k++)
			{
				flag = (flag || method.Name.Contains(array2[k]));
			}
			if (!flag && !stackTrace.GetFrame(num).GetMethod().Name.StartsWith("set_") && !stackTrace.GetFrame(num).GetMethod().Name.StartsWith("Instantiate"))
			{
				if (i != 0)
				{
					text += " < ";
				}
				i++;
				text += stackTrace.GetFrame(num).GetMethod().Name;
			}
		}
		return text;
	}

	// Token: 0x04003A6D RID: 14957
	[SerializeField]
	public List<EntryDevLog.ModificationRecord> modificationRecords = new List<EntryDevLog.ModificationRecord>();

	// Token: 0x02001A0A RID: 6666
	public class ModificationRecord
	{
		// Token: 0x170009EF RID: 2543
		// (get) Token: 0x060095EC RID: 38380 RVA: 0x0033B90F File Offset: 0x00339B0F
		// (set) Token: 0x060095ED RID: 38381 RVA: 0x0033B917 File Offset: 0x00339B17
		public EntryDevLog.ModificationRecord.ActionType actionType { get; private set; }

		// Token: 0x170009F0 RID: 2544
		// (get) Token: 0x060095EE RID: 38382 RVA: 0x0033B920 File Offset: 0x00339B20
		// (set) Token: 0x060095EF RID: 38383 RVA: 0x0033B928 File Offset: 0x00339B28
		public string target { get; private set; }

		// Token: 0x170009F1 RID: 2545
		// (get) Token: 0x060095F0 RID: 38384 RVA: 0x0033B931 File Offset: 0x00339B31
		// (set) Token: 0x060095F1 RID: 38385 RVA: 0x0033B939 File Offset: 0x00339B39
		public object newValue { get; private set; }

		// Token: 0x170009F2 RID: 2546
		// (get) Token: 0x060095F2 RID: 38386 RVA: 0x0033B942 File Offset: 0x00339B42
		// (set) Token: 0x060095F3 RID: 38387 RVA: 0x0033B94A File Offset: 0x00339B4A
		public string author { get; private set; }

		// Token: 0x060095F4 RID: 38388 RVA: 0x0033B953 File Offset: 0x00339B53
		public ModificationRecord(EntryDevLog.ModificationRecord.ActionType actionType, string target, object newValue, string author)
		{
			this.target = target;
			this.newValue = newValue;
			this.author = author;
			this.actionType = actionType;
		}

		// Token: 0x0200222D RID: 8749
		public enum ActionType
		{
			// Token: 0x040098DF RID: 39135
			Created,
			// Token: 0x040098E0 RID: 39136
			ChangeSubEntry,
			// Token: 0x040098E1 RID: 39137
			ChangeContent,
			// Token: 0x040098E2 RID: 39138
			ValueChange,
			// Token: 0x040098E3 RID: 39139
			YAMLData
		}
	}
}
