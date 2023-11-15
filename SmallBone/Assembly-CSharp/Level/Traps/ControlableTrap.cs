using System;

namespace Level.Traps
{
	// Token: 0x02000658 RID: 1624
	public abstract class ControlableTrap : Trap
	{
		// Token: 0x170006D9 RID: 1753
		// (get) Token: 0x060020A6 RID: 8358 RVA: 0x0006292A File Offset: 0x00060B2A
		// (set) Token: 0x060020A7 RID: 8359 RVA: 0x00062932 File Offset: 0x00060B32
		public bool activated { get; protected set; }

		// Token: 0x060020A8 RID: 8360
		public abstract void Activate();

		// Token: 0x060020A9 RID: 8361
		public abstract void Deactivate();
	}
}
