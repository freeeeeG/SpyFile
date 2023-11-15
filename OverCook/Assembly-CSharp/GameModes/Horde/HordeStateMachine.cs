using System;
using System.Collections.Generic;

namespace GameModes.Horde
{
	// Token: 0x020007D9 RID: 2009
	public class HordeStateMachine<T> : IHordeStateMachine<T> where T : struct, IConvertible, IComparable
	{
		// Token: 0x06002698 RID: 9880 RVA: 0x000B7ED0 File Offset: 0x000B62D0
		public HordeStateMachine(T startState, IHordeState<T>[] states, HordeStateMachine<T>.OnBegin onBegin, HordeStateMachine<T>.OnEnd onEnd, HordeStateMachine<T>.OnUpdate onUpdate)
		{
			this.StateId = startState;
			this.m_states = new Dictionary<T, IHordeState<T>>();
			for (int i = 0; i < states.Length; i++)
			{
				this.m_states.Add(states[i].StateId, states[i]);
			}
			this.m_onBegin = onBegin;
			this.m_onEnd = onEnd;
			this.m_onUpdate = onUpdate;
		}

		// Token: 0x17000306 RID: 774
		// (get) Token: 0x06002699 RID: 9881 RVA: 0x000B7F36 File Offset: 0x000B6336
		// (set) Token: 0x0600269A RID: 9882 RVA: 0x000B7F3E File Offset: 0x000B633E
		public T StateId { get; private set; }

		// Token: 0x17000307 RID: 775
		// (get) Token: 0x0600269B RID: 9883 RVA: 0x000B7F47 File Offset: 0x000B6347
		public IHordeState<T> State
		{
			get
			{
				return this.m_states[this.StateId];
			}
		}

		// Token: 0x0600269C RID: 9884 RVA: 0x000B7F5C File Offset: 0x000B635C
		public bool Transition(T state)
		{
			if (state.Equals(this.StateId))
			{
				return false;
			}
			T stateId = this.StateId;
			if (this.m_onEnd != null)
			{
				this.m_onEnd(this, stateId, state);
			}
			this.StateId = state;
			if (this.m_onBegin != null)
			{
				this.m_onBegin(this, stateId, this.StateId);
			}
			return true;
		}

		// Token: 0x0600269D RID: 9885 RVA: 0x000B7FCE File Offset: 0x000B63CE
		public void Tick(float dT)
		{
			if (this.m_onUpdate != null)
			{
				this.m_onUpdate(this, this.StateId, dT);
			}
		}

		// Token: 0x04001EA4 RID: 7844
		private Dictionary<T, IHordeState<T>> m_states;

		// Token: 0x04001EA6 RID: 7846
		private HordeStateMachine<T>.OnBegin m_onBegin;

		// Token: 0x04001EA7 RID: 7847
		private HordeStateMachine<T>.OnEnd m_onEnd;

		// Token: 0x04001EA8 RID: 7848
		private HordeStateMachine<T>.OnUpdate m_onUpdate;

		// Token: 0x020007DA RID: 2010
		// (Invoke) Token: 0x0600269F RID: 9887
		public delegate void OnBegin(IHordeStateMachine<T> stateMachine, T fromState, T toState);

		// Token: 0x020007DB RID: 2011
		// (Invoke) Token: 0x060026A3 RID: 9891
		public delegate void OnEnd(IHordeStateMachine<T> stateMachine, T fromState, T toState);

		// Token: 0x020007DC RID: 2012
		// (Invoke) Token: 0x060026A7 RID: 9895
		public delegate void OnUpdate(IHordeStateMachine<T> stateMachine, T state, float dT);
	}
}
