using System;
using UnityEngine;

namespace Runnables.States
{
	// Token: 0x02000341 RID: 833
	public class StateMachine : MonoBehaviour
	{
		// Token: 0x17000345 RID: 837
		// (get) Token: 0x06000FC9 RID: 4041 RVA: 0x0002F943 File Offset: 0x0002DB43
		public State currentState
		{
			get
			{
				return this._state;
			}
		}

		// Token: 0x06000FCA RID: 4042 RVA: 0x0002F94B File Offset: 0x0002DB4B
		public void TransitTo(State state)
		{
			this._state = state;
		}

		// Token: 0x04000CF5 RID: 3317
		[SerializeField]
		[Header("초기값 설정")]
		private State _state;
	}
}
