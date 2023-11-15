using System;
using Runnables.States;
using UnityEngine;

namespace Runnables
{
	// Token: 0x02000337 RID: 823
	public class TransitTo : Runnable
	{
		// Token: 0x06000FA9 RID: 4009 RVA: 0x0002F664 File Offset: 0x0002D864
		public override void Run()
		{
			this._stateMachine.TransitTo(this._targetState);
		}

		// Token: 0x04000CE0 RID: 3296
		[SerializeField]
		private StateMachine _stateMachine;

		// Token: 0x04000CE1 RID: 3297
		[SerializeField]
		private State _targetState;
	}
}
