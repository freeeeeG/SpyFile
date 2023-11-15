using System;

namespace Pathfinding.Serialization
{
	// Token: 0x020000B7 RID: 183
	public class SerializeSettings
	{
		// Token: 0x1700011A RID: 282
		// (get) Token: 0x0600081A RID: 2074 RVA: 0x00035C5F File Offset: 0x00033E5F
		public static SerializeSettings Settings
		{
			get
			{
				return new SerializeSettings
				{
					nodes = false
				};
			}
		}

		// Token: 0x040004C8 RID: 1224
		public bool nodes = true;

		// Token: 0x040004C9 RID: 1225
		[Obsolete("There is no support for pretty printing the json anymore")]
		public bool prettyPrint;

		// Token: 0x040004CA RID: 1226
		public bool editorSettings;
	}
}
