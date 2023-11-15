using System;

namespace System.ArrayExtensions
{
	// Token: 0x02000050 RID: 80
	public static class ArrayExtensions
	{
		// Token: 0x06000412 RID: 1042 RVA: 0x00015870 File Offset: 0x00013A70
		public static void ForEach(this Array array, Action<Array, int[]> action)
		{
			if (array.LongLength == 0L)
			{
				return;
			}
			ArrayTraverse arrayTraverse = new ArrayTraverse(array);
			do
			{
				action(array, arrayTraverse.Position);
			}
			while (arrayTraverse.Step());
		}
	}
}
