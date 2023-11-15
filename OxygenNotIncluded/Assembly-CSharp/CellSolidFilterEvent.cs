using System;
using System.Diagnostics;

// Token: 0x020007B0 RID: 1968
public class CellSolidFilterEvent : CellEvent
{
	// Token: 0x06003695 RID: 13973 RVA: 0x00126E46 File Offset: 0x00125046
	public CellSolidFilterEvent(string id, bool enable_logging = true) : base(id, "filtered", false, enable_logging)
	{
	}

	// Token: 0x06003696 RID: 13974 RVA: 0x00126E58 File Offset: 0x00125058
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

	// Token: 0x06003697 RID: 13975 RVA: 0x00126E8C File Offset: 0x0012508C
	public override string GetDescription(EventInstanceBase ev)
	{
		CellEventInstance cellEventInstance = ev as CellEventInstance;
		return base.GetMessagePrefix() + "Filtered Solid Event solid=" + cellEventInstance.data.ToString();
	}
}
