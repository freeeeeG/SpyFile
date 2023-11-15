using System;

namespace GameModes.Horde
{
	// Token: 0x020007D7 RID: 2007
	public interface IHordeStateMachine<T> where T : struct, IConvertible, IComparable
	{
		// Token: 0x17000303 RID: 771
		// (get) Token: 0x06002691 RID: 9873
		T StateId { get; }

		// Token: 0x17000304 RID: 772
		// (get) Token: 0x06002692 RID: 9874
		IHordeState<T> State { get; }

		// Token: 0x06002693 RID: 9875
		bool Transition(T state);

		// Token: 0x06002694 RID: 9876
		void Tick(float dT);
	}
}
