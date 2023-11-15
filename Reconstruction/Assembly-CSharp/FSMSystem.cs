using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020000D9 RID: 217
public class FSMSystem
{
	// Token: 0x06000568 RID: 1384 RVA: 0x0000EBDA File Offset: 0x0000CDDA
	public void Update(AirAttacker agent)
	{
		if (!agent.DamageStrategy.IsDie)
		{
			this.CurrentState.Act(agent);
			this.CurrentState.Reason(agent);
		}
	}

	// Token: 0x06000569 RID: 1385 RVA: 0x0000EC01 File Offset: 0x0000CE01
	public void Update(AirProtector agent)
	{
		if (!agent.DamageStrategy.IsDie)
		{
			this.CurrentState.Act(agent);
			this.CurrentState.Reason(agent);
		}
	}

	// Token: 0x0600056A RID: 1386 RVA: 0x0000EC28 File Offset: 0x0000CE28
	public void AddState(FSMState s)
	{
		if (s == null)
		{
			Debug.LogError("state为空");
			return;
		}
		if (this.CurrentState == null)
		{
			this.CurrentState = s;
			this.CurrentStateID = s.StateID;
		}
		if (this.States.ContainsKey(s.StateID))
		{
			Debug.LogError("状态" + s.StateID.ToString() + "已经存在，无法重复添加");
			return;
		}
		this.States.Add(s.StateID, s);
	}

	// Token: 0x0600056B RID: 1387 RVA: 0x0000ECAC File Offset: 0x0000CEAC
	public void DeleteFSMState(StateID id)
	{
		if (id == StateID.NullStateID)
		{
			Debug.LogError("无法删除空状态");
			return;
		}
		if (!this.States.ContainsKey(id))
		{
			Debug.LogError("无法删除不存在状态" + id.ToString());
			return;
		}
		this.States.Remove(id);
	}

	// Token: 0x0600056C RID: 1388 RVA: 0x0000ED00 File Offset: 0x0000CF00
	public void PerformTransition(Transition trans)
	{
		if (trans == Transition.NullTransition)
		{
			Debug.LogError("无法执行空的转换条件");
			return;
		}
		StateID stateID = this.CurrentState.GetStateID(trans);
		if (stateID == StateID.NullStateID)
		{
			Debug.LogWarning(string.Concat(new string[]
			{
				"当前状态",
				this.CurrentStateID.ToString(),
				"无法根据转换条件",
				trans.ToString(),
				"发生转换"
			}));
			return;
		}
		if (!this.States.ContainsKey(stateID))
		{
			Debug.LogError("状态机内没有包含" + stateID.ToString() + "，无法进行状态转换");
			return;
		}
		FSMState currentState = this.States[stateID];
		this.CurrentState.DoAfterLeaving();
		this.CurrentState = currentState;
		this.CurrentStateID = stateID;
		this.CurrentState.DoBeforeEntering();
	}

	// Token: 0x04000242 RID: 578
	private Dictionary<StateID, FSMState> States = new Dictionary<StateID, FSMState>();

	// Token: 0x04000243 RID: 579
	private StateID CurrentStateID;

	// Token: 0x04000244 RID: 580
	private FSMState CurrentState;
}
