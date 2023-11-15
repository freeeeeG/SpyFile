using System;
using System.Diagnostics;

// Token: 0x020007AF RID: 1967
public class CellSolidEvent : CellEvent
{
	// Token: 0x06003692 RID: 13970 RVA: 0x00126DB0 File Offset: 0x00124FB0
	public CellSolidEvent(string id, string reason, bool is_send, bool enable_logging = true) : base(id, reason, is_send, enable_logging)
	{
	}

	// Token: 0x06003693 RID: 13971 RVA: 0x00126DC0 File Offset: 0x00124FC0
	[Conditional("ENABLE_CELL_EVENT_LOGGER")]
	public void Log(int cell, bool solid)
	{
		if (!this.enableLogging)
		{
			return;
		}
		CellEventInstance ev = new CellEventInstance(cell, solid ? 1 : 0, 0, this);
		CellEventLogger.Instance.Add(ev);
	}

	// Token: 0x06003694 RID: 13972 RVA: 0x00126DF4 File Offset: 0x00124FF4
	public override string GetDescription(EventInstanceBase ev)
	{
		if ((ev as CellEventInstance).data == 1)
		{
			return base.GetMessagePrefix() + "Solid=true (" + this.reason + ")";
		}
		return base.GetMessagePrefix() + "Solid=false (" + this.reason + ")";
	}
}
