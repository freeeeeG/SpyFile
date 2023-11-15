using System;
using STRINGS;
using UnityEngine;

// Token: 0x020009CB RID: 2507
public class CargoBayIsEmpty : ProcessCondition
{
	// Token: 0x06004B0D RID: 19213 RVA: 0x001A68D8 File Offset: 0x001A4AD8
	public CargoBayIsEmpty(CommandModule module)
	{
		this.commandModule = module;
	}

	// Token: 0x06004B0E RID: 19214 RVA: 0x001A68E8 File Offset: 0x001A4AE8
	public override ProcessCondition.Status EvaluateCondition()
	{
		foreach (GameObject gameObject in AttachableBuilding.GetAttachedNetwork(this.commandModule.GetComponent<AttachableBuilding>()))
		{
			CargoBay component = gameObject.GetComponent<CargoBay>();
			if (component != null && component.storage.MassStored() != 0f)
			{
				return ProcessCondition.Status.Failure;
			}
		}
		return ProcessCondition.Status.Ready;
	}

	// Token: 0x06004B0F RID: 19215 RVA: 0x001A6968 File Offset: 0x001A4B68
	public override string GetStatusMessage(ProcessCondition.Status status)
	{
		return UI.STARMAP.CARGOEMPTY.NAME;
	}

	// Token: 0x06004B10 RID: 19216 RVA: 0x001A6974 File Offset: 0x001A4B74
	public override string GetStatusTooltip(ProcessCondition.Status status)
	{
		return UI.STARMAP.CARGOEMPTY.TOOLTIP;
	}

	// Token: 0x06004B11 RID: 19217 RVA: 0x001A6980 File Offset: 0x001A4B80
	public override bool ShowInUI()
	{
		return true;
	}

	// Token: 0x04003135 RID: 12597
	private CommandModule commandModule;
}
