using System;

namespace GameModes.Horde
{
	// Token: 0x020007D8 RID: 2008
	public class HordeState<T> : IHordeState<T> where T : struct, IConvertible, IComparable
	{
		// Token: 0x06002695 RID: 9877 RVA: 0x000B7EAD File Offset: 0x000B62AD
		public HordeState(T state)
		{
			this.StateId = state;
		}

		// Token: 0x17000305 RID: 773
		// (get) Token: 0x06002696 RID: 9878 RVA: 0x000B7EBC File Offset: 0x000B62BC
		// (set) Token: 0x06002697 RID: 9879 RVA: 0x000B7EC4 File Offset: 0x000B62C4
		public T StateId { get; private set; }
	}
}
