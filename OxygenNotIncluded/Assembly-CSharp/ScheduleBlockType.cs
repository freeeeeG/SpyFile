using System;
using System.Diagnostics;
using UnityEngine;

// Token: 0x02000547 RID: 1351
[DebuggerDisplay("{Id}")]
public class ScheduleBlockType : Resource
{
	// Token: 0x17000182 RID: 386
	// (get) Token: 0x060020DF RID: 8415 RVA: 0x000AFA6E File Offset: 0x000ADC6E
	// (set) Token: 0x060020E0 RID: 8416 RVA: 0x000AFA76 File Offset: 0x000ADC76
	public Color color { get; private set; }

	// Token: 0x17000183 RID: 387
	// (get) Token: 0x060020E1 RID: 8417 RVA: 0x000AFA7F File Offset: 0x000ADC7F
	// (set) Token: 0x060020E2 RID: 8418 RVA: 0x000AFA87 File Offset: 0x000ADC87
	public string description { get; private set; }

	// Token: 0x060020E3 RID: 8419 RVA: 0x000AFA90 File Offset: 0x000ADC90
	public ScheduleBlockType(string id, ResourceSet parent, string name, string description, Color color) : base(id, parent, name)
	{
		this.color = color;
		this.description = description;
	}
}
