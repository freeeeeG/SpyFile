using System;
using Runnables.States;
using UnityEngine;

namespace Runnables.Triggers
{
	// Token: 0x02000348 RID: 840
	public class EqualsState : Trigger
	{
		// Token: 0x06000FD8 RID: 4056 RVA: 0x0002F9CE File Offset: 0x0002DBCE
		protected override bool Check()
		{
			return this._stateMachine.currentState == this._state;
		}

		// Token: 0x04000CFB RID: 3323
		[SerializeField]
		private StateMachine _stateMachine;

		// Token: 0x04000CFC RID: 3324
		[SerializeField]
		private State _state;
	}
}
