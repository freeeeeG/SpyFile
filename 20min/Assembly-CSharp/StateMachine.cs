using System;
using UnityEngine;

// Token: 0x02000041 RID: 65
public class StateMachine : MonoBehaviour
{
	// Token: 0x1700002D RID: 45
	// (get) Token: 0x060003E9 RID: 1001 RVA: 0x0001511B File Offset: 0x0001331B
	// (set) Token: 0x060003EA RID: 1002 RVA: 0x00015123 File Offset: 0x00013323
	public virtual State CurrentState
	{
		get
		{
			return this._currentState;
		}
		set
		{
			this.Transition(value);
		}
	}

	// Token: 0x060003EB RID: 1003 RVA: 0x0001512C File Offset: 0x0001332C
	public virtual T GetState<T>() where T : State
	{
		T t = base.GetComponent<T>();
		if (t == null)
		{
			t = base.gameObject.AddComponent<T>();
		}
		return t;
	}

	// Token: 0x060003EC RID: 1004 RVA: 0x0001515B File Offset: 0x0001335B
	public virtual void ChangeState<T>() where T : State
	{
		this.CurrentState = this.GetState<T>();
	}

	// Token: 0x060003ED RID: 1005 RVA: 0x0001516E File Offset: 0x0001336E
	protected virtual void Transition(State value)
	{
		if (this._currentState == value)
		{
			return;
		}
		State currentState = this._currentState;
		if (currentState != null)
		{
			currentState.Exit();
		}
		this._currentState = value;
		State currentState2 = this._currentState;
		if (currentState2 == null)
		{
			return;
		}
		currentState2.Enter();
	}

	// Token: 0x040001DC RID: 476
	protected State _currentState;
}
