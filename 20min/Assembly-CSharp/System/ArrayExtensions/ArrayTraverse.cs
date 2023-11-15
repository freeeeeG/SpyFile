using System;

namespace System.ArrayExtensions
{
	// Token: 0x02000051 RID: 81
	internal class ArrayTraverse
	{
		// Token: 0x06000413 RID: 1043 RVA: 0x000158A4 File Offset: 0x00013AA4
		public ArrayTraverse(Array array)
		{
			this.maxLengths = new int[array.Rank];
			for (int i = 0; i < array.Rank; i++)
			{
				this.maxLengths[i] = array.GetLength(i) - 1;
			}
			this.Position = new int[array.Rank];
		}

		// Token: 0x06000414 RID: 1044 RVA: 0x000158FC File Offset: 0x00013AFC
		public bool Step()
		{
			for (int i = 0; i < this.Position.Length; i++)
			{
				if (this.Position[i] < this.maxLengths[i])
				{
					this.Position[i]++;
					for (int j = 0; j < i; j++)
					{
						this.Position[j] = 0;
					}
					return true;
				}
			}
			return false;
		}

		// Token: 0x04000219 RID: 537
		public int[] Position;

		// Token: 0x0400021A RID: 538
		private int[] maxLengths;
	}
}
