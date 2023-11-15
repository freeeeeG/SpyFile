using System;

namespace Level
{
	// Token: 0x0200051D RID: 1309
	[Serializable]
	public class SerializablePathNode : PathNode
	{
		// Token: 0x0200051E RID: 1310
		[Serializable]
		internal class Reorderable : ReorderableArray<SerializablePathNode>
		{
		}
	}
}
