using System;

namespace KMod
{
	// Token: 0x02000D78 RID: 3448
	public struct FileSystemItem
	{
		// Token: 0x04004EAE RID: 20142
		public string name;

		// Token: 0x04004EAF RID: 20143
		public FileSystemItem.ItemType type;

		// Token: 0x02001C4D RID: 7245
		public enum ItemType
		{
			// Token: 0x04008063 RID: 32867
			Directory,
			// Token: 0x04008064 RID: 32868
			File
		}
	}
}
