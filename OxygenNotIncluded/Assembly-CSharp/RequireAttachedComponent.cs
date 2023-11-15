using System;
using System.Collections.Generic;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x020009E0 RID: 2528
[SerializationConfig(MemberSerialization.OptIn)]
public class RequireAttachedComponent : ProcessCondition
{
	// Token: 0x1700059A RID: 1434
	// (get) Token: 0x06004B80 RID: 19328 RVA: 0x001A81D0 File Offset: 0x001A63D0
	// (set) Token: 0x06004B81 RID: 19329 RVA: 0x001A81D8 File Offset: 0x001A63D8
	public Type RequiredType
	{
		get
		{
			return this.requiredType;
		}
		set
		{
			this.requiredType = value;
			this.typeNameString = this.requiredType.Name;
		}
	}

	// Token: 0x06004B82 RID: 19330 RVA: 0x001A81F2 File Offset: 0x001A63F2
	public RequireAttachedComponent(AttachableBuilding myAttachable, Type required_type, string type_name_string)
	{
		this.myAttachable = myAttachable;
		this.requiredType = required_type;
		this.typeNameString = type_name_string;
	}

	// Token: 0x06004B83 RID: 19331 RVA: 0x001A8210 File Offset: 0x001A6410
	public override ProcessCondition.Status EvaluateCondition()
	{
		if (this.myAttachable != null)
		{
			using (List<GameObject>.Enumerator enumerator = AttachableBuilding.GetAttachedNetwork(this.myAttachable).GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.GetComponent(this.requiredType))
					{
						return ProcessCondition.Status.Ready;
					}
				}
			}
			return ProcessCondition.Status.Failure;
		}
		return ProcessCondition.Status.Failure;
	}

	// Token: 0x06004B84 RID: 19332 RVA: 0x001A8288 File Offset: 0x001A6488
	public override string GetStatusMessage(ProcessCondition.Status status)
	{
		return this.typeNameString;
	}

	// Token: 0x06004B85 RID: 19333 RVA: 0x001A8294 File Offset: 0x001A6494
	public override string GetStatusTooltip(ProcessCondition.Status status)
	{
		if (status == ProcessCondition.Status.Ready)
		{
			return string.Format(UI.STARMAP.LAUNCHCHECKLIST.INSTALLED_TOOLTIP, this.typeNameString.ToLower());
		}
		return string.Format(UI.STARMAP.LAUNCHCHECKLIST.MISSING_TOOLTIP, this.typeNameString.ToLower());
	}

	// Token: 0x06004B86 RID: 19334 RVA: 0x001A82CF File Offset: 0x001A64CF
	public override bool ShowInUI()
	{
		return true;
	}

	// Token: 0x04003153 RID: 12627
	private string typeNameString;

	// Token: 0x04003154 RID: 12628
	private Type requiredType;

	// Token: 0x04003155 RID: 12629
	private AttachableBuilding myAttachable;
}
