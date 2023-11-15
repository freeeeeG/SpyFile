using System;

// Token: 0x02000508 RID: 1288
public interface IGridLocation
{
	// Token: 0x17000252 RID: 594
	// (get) Token: 0x06001815 RID: 6165
	GridIndex GridIndex { get; }

	// Token: 0x17000253 RID: 595
	// (get) Token: 0x06001816 RID: 6166
	GridManager AccessGridManager { get; }
}
