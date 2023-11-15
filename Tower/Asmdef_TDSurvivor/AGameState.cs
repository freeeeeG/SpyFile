using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000090 RID: 144
public abstract class AGameState : MonoBehaviour
{
	// Token: 0x17000040 RID: 64
	// (get) Token: 0x060002E4 RID: 740 RVA: 0x0000B9BB File Offset: 0x00009BBB
	public eGameState Type
	{
		get
		{
			return this.type;
		}
	}

	// Token: 0x060002E5 RID: 741 RVA: 0x0000B9C3 File Offset: 0x00009BC3
	public void StateStart(eGameState _stateType)
	{
		this.type = _stateType;
		Debug.Log(string.Format("State Start: {0}", this.type));
		this.StateInitProc();
		this.StateProcess();
	}

	// Token: 0x060002E6 RID: 742
	protected abstract void StateInitProc();

	// Token: 0x060002E7 RID: 743 RVA: 0x0000B9F2 File Offset: 0x00009BF2
	private void StateProcess()
	{
		this.coroutine_StateProcess = base.StartCoroutine(this.StateProc());
	}

	// Token: 0x060002E8 RID: 744 RVA: 0x0000BA06 File Offset: 0x00009C06
	protected virtual IEnumerator StateProc()
	{
		yield break;
	}

	// Token: 0x060002E9 RID: 745 RVA: 0x0000BA0E File Offset: 0x00009C0E
	public void StateUpdate(float deltaTime)
	{
		this.StateUpdateProc(deltaTime);
	}

	// Token: 0x060002EA RID: 746 RVA: 0x0000BA17 File Offset: 0x00009C17
	protected virtual void StateUpdateProc(float deltaTime)
	{
	}

	// Token: 0x060002EB RID: 747 RVA: 0x0000BA19 File Offset: 0x00009C19
	public void StateEnd()
	{
		Debug.Log(string.Format("State End: {0}", this.type));
		if (this.coroutine_StateProcess != null)
		{
			base.StopCoroutine(this.coroutine_StateProcess);
		}
		this.StateEndProc();
	}

	// Token: 0x060002EC RID: 748
	protected abstract void StateEndProc();

	// Token: 0x04000348 RID: 840
	[SerializeField]
	protected eGameState type;

	// Token: 0x04000349 RID: 841
	private Coroutine coroutine_StateProcess;
}
