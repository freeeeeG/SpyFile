using System;
using System.Collections.Generic;
using System.Diagnostics;
using STRINGS;

// Token: 0x02000548 RID: 1352
[DebuggerDisplay("{Id}")]
public class ScheduleGroup : Resource
{
	// Token: 0x17000184 RID: 388
	// (get) Token: 0x060020E4 RID: 8420 RVA: 0x000AFAAB File Offset: 0x000ADCAB
	// (set) Token: 0x060020E5 RID: 8421 RVA: 0x000AFAB3 File Offset: 0x000ADCB3
	public int defaultSegments { get; private set; }

	// Token: 0x17000185 RID: 389
	// (get) Token: 0x060020E6 RID: 8422 RVA: 0x000AFABC File Offset: 0x000ADCBC
	// (set) Token: 0x060020E7 RID: 8423 RVA: 0x000AFAC4 File Offset: 0x000ADCC4
	public string description { get; private set; }

	// Token: 0x17000186 RID: 390
	// (get) Token: 0x060020E8 RID: 8424 RVA: 0x000AFACD File Offset: 0x000ADCCD
	// (set) Token: 0x060020E9 RID: 8425 RVA: 0x000AFAD5 File Offset: 0x000ADCD5
	public string notificationTooltip { get; private set; }

	// Token: 0x17000187 RID: 391
	// (get) Token: 0x060020EA RID: 8426 RVA: 0x000AFADE File Offset: 0x000ADCDE
	// (set) Token: 0x060020EB RID: 8427 RVA: 0x000AFAE6 File Offset: 0x000ADCE6
	public List<ScheduleBlockType> allowedTypes { get; private set; }

	// Token: 0x17000188 RID: 392
	// (get) Token: 0x060020EC RID: 8428 RVA: 0x000AFAEF File Offset: 0x000ADCEF
	// (set) Token: 0x060020ED RID: 8429 RVA: 0x000AFAF7 File Offset: 0x000ADCF7
	public bool alarm { get; private set; }

	// Token: 0x060020EE RID: 8430 RVA: 0x000AFB00 File Offset: 0x000ADD00
	public ScheduleGroup(string id, ResourceSet parent, int defaultSegments, string name, string description, string notificationTooltip, List<ScheduleBlockType> allowedTypes, bool alarm = false) : base(id, parent, name)
	{
		this.defaultSegments = defaultSegments;
		this.description = description;
		this.notificationTooltip = notificationTooltip;
		this.allowedTypes = allowedTypes;
		this.alarm = alarm;
	}

	// Token: 0x060020EF RID: 8431 RVA: 0x000AFB33 File Offset: 0x000ADD33
	public bool Allowed(ScheduleBlockType type)
	{
		return this.allowedTypes.Contains(type);
	}

	// Token: 0x060020F0 RID: 8432 RVA: 0x000AFB41 File Offset: 0x000ADD41
	public string GetTooltip()
	{
		return string.Format(UI.SCHEDULEGROUPS.TOOLTIP_FORMAT, this.Name, this.description);
	}
}
