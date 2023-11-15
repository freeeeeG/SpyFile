using System;

namespace Klei.CustomSettings
{
	// Token: 0x02000DC9 RID: 3529
	public class SettingLevel
	{
		// Token: 0x06006C8D RID: 27789 RVA: 0x002AE362 File Offset: 0x002AC562
		public SettingLevel(string id, string label, string tooltip, long coordinate_offset = 0L, object userdata = null)
		{
			this.id = id;
			this.label = label;
			this.tooltip = tooltip;
			this.userdata = userdata;
			this.coordinate_offset = coordinate_offset;
		}

		// Token: 0x1700079A RID: 1946
		// (get) Token: 0x06006C8E RID: 27790 RVA: 0x002AE38F File Offset: 0x002AC58F
		// (set) Token: 0x06006C8F RID: 27791 RVA: 0x002AE397 File Offset: 0x002AC597
		public string id { get; private set; }

		// Token: 0x1700079B RID: 1947
		// (get) Token: 0x06006C90 RID: 27792 RVA: 0x002AE3A0 File Offset: 0x002AC5A0
		// (set) Token: 0x06006C91 RID: 27793 RVA: 0x002AE3A8 File Offset: 0x002AC5A8
		public string tooltip { get; private set; }

		// Token: 0x1700079C RID: 1948
		// (get) Token: 0x06006C92 RID: 27794 RVA: 0x002AE3B1 File Offset: 0x002AC5B1
		// (set) Token: 0x06006C93 RID: 27795 RVA: 0x002AE3B9 File Offset: 0x002AC5B9
		public string label { get; private set; }

		// Token: 0x1700079D RID: 1949
		// (get) Token: 0x06006C94 RID: 27796 RVA: 0x002AE3C2 File Offset: 0x002AC5C2
		// (set) Token: 0x06006C95 RID: 27797 RVA: 0x002AE3CA File Offset: 0x002AC5CA
		public object userdata { get; private set; }

		// Token: 0x1700079E RID: 1950
		// (get) Token: 0x06006C96 RID: 27798 RVA: 0x002AE3D3 File Offset: 0x002AC5D3
		// (set) Token: 0x06006C97 RID: 27799 RVA: 0x002AE3DB File Offset: 0x002AC5DB
		public long coordinate_offset { get; private set; }
	}
}
