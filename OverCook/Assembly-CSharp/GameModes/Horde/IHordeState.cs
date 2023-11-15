using System;

namespace GameModes.Horde
{
	// Token: 0x020007D6 RID: 2006
	public interface IHordeState<T> where T : struct, IConvertible, IComparable
	{
		// Token: 0x17000302 RID: 770
		// (get) Token: 0x06002690 RID: 9872
		T StateId { get; }
	}
}
