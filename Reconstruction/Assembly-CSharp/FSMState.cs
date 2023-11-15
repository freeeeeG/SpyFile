using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020000D8 RID: 216
public abstract class FSMState
{
	// Token: 0x17000259 RID: 601
	// (get) Token: 0x0600055E RID: 1374 RVA: 0x0000EAC6 File Offset: 0x0000CCC6
	// (set) Token: 0x0600055F RID: 1375 RVA: 0x0000EACE File Offset: 0x0000CCCE
	public StateID StateID
	{
		get
		{
			return this.stateID;
		}
		set
		{
			this.stateID = value;
		}
	}

	// Token: 0x06000560 RID: 1376 RVA: 0x0000EAD7 File Offset: 0x0000CCD7
	public FSMState(FSMSystem fsm)
	{
		this.fsm = fsm;
	}

	// Token: 0x06000561 RID: 1377 RVA: 0x0000EAF4 File Offset: 0x0000CCF4
	public void AddTransition(Transition trans, StateID id)
	{
		if (trans == Transition.NullTransition)
		{
			Debug.LogError("不允许NullTransition");
			return;
		}
		if (id == StateID.NullStateID)
		{
			Debug.LogError("不允许NullStateID");
			return;
		}
		if (this.map.ContainsKey(trans))
		{
			Debug.LogError("添加转换条件时，" + trans.ToString() + "已经存在于map中");
			return;
		}
		this.map.Add(trans, id);
	}

	// Token: 0x06000562 RID: 1378 RVA: 0x0000EB5C File Offset: 0x0000CD5C
	public void DeleteTransition(Transition trans)
	{
		if (trans == Transition.NullTransition)
		{
			Debug.LogError("不允许NullTransition");
			return;
		}
		if (!this.map.ContainsKey(trans))
		{
			Debug.LogError("添加转换条件时，" + trans.ToString() + "不存在于map中");
			return;
		}
		this.map.Remove(trans);
	}

	// Token: 0x06000563 RID: 1379 RVA: 0x0000EBB4 File Offset: 0x0000CDB4
	public StateID GetStateID(Transition trans)
	{
		if (this.map.ContainsKey(trans))
		{
			return this.map[trans];
		}
		return StateID.NullStateID;
	}

	// Token: 0x06000564 RID: 1380 RVA: 0x0000EBD2 File Offset: 0x0000CDD2
	public virtual void DoBeforeEntering()
	{
	}

	// Token: 0x06000565 RID: 1381 RVA: 0x0000EBD4 File Offset: 0x0000CDD4
	public virtual void DoAfterLeaving()
	{
	}

	// Token: 0x06000566 RID: 1382 RVA: 0x0000EBD6 File Offset: 0x0000CDD6
	public virtual void Act(Aircraft agent)
	{
	}

	// Token: 0x06000567 RID: 1383 RVA: 0x0000EBD8 File Offset: 0x0000CDD8
	public virtual void Reason(Aircraft agent)
	{
	}

	// Token: 0x0400023F RID: 575
	private StateID stateID;

	// Token: 0x04000240 RID: 576
	protected FSMSystem fsm;

	// Token: 0x04000241 RID: 577
	protected Dictionary<Transition, StateID> map = new Dictionary<Transition, StateID>();
}
