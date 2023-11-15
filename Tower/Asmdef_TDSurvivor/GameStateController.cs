using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000093 RID: 147
public class GameStateController : Singleton<GameStateController>
{
	// Token: 0x17000042 RID: 66
	// (get) Token: 0x060002F9 RID: 761 RVA: 0x0000BC93 File Offset: 0x00009E93
	public eGameState State
	{
		get
		{
			return this.state.Type;
		}
	}

	// Token: 0x17000043 RID: 67
	// (get) Token: 0x060002FA RID: 762 RVA: 0x0000BCA0 File Offset: 0x00009EA0
	public bool IsInBattle
	{
		get
		{
			return this.isInBattle;
		}
	}

	// Token: 0x060002FB RID: 763 RVA: 0x0000BCA8 File Offset: 0x00009EA8
	private new void Awake()
	{
		EventMgr.Register<eGameState>(eGameEvents.RequestChangeGameState, new Action<eGameState>(this.OnRequestChangeGameState));
		EventMgr.Register<bool>(eGameEvents.RequestChangeBattleState, new Action<bool>(this.OnRequestChangeBattleState));
		this.dic_GameState = new Dictionary<eGameState, Type>
		{
			{
				eGameState.NORMAL_MODE,
				typeof(State_NormalMode)
			},
			{
				eGameState.EDIT_MODE,
				typeof(State_EditMode)
			},
			{
				eGameState.BUFF_MODE,
				typeof(State_BuffMode)
			},
			{
				eGameState.PAUSE_GAME,
				typeof(State_PauseGame)
			}
		};
	}

	// Token: 0x060002FC RID: 764 RVA: 0x0000BD34 File Offset: 0x00009F34
	private void OnDestroy()
	{
		EventMgr.Remove<eGameState>(eGameEvents.RequestChangeGameState, new Action<eGameState>(this.OnRequestChangeGameState));
		EventMgr.Remove<bool>(eGameEvents.RequestChangeBattleState, new Action<bool>(this.OnRequestChangeBattleState));
		if (this.state != null)
		{
			this.state.StateEnd();
		}
		Object.Destroy(this.state);
	}

	// Token: 0x060002FD RID: 765 RVA: 0x0000BD94 File Offset: 0x00009F94
	private void Start()
	{
		this.SwitchState(eGameState.NORMAL_MODE);
	}

	// Token: 0x060002FE RID: 766 RVA: 0x0000BD9D File Offset: 0x00009F9D
	private void Update()
	{
		if (this.state != null)
		{
			this.state.StateUpdate(Time.deltaTime);
		}
	}

	// Token: 0x060002FF RID: 767 RVA: 0x0000BDBD File Offset: 0x00009FBD
	private void OnRequestChangeGameState(eGameState targetState)
	{
		this.SwitchState(targetState);
	}

	// Token: 0x06000300 RID: 768 RVA: 0x0000BDC6 File Offset: 0x00009FC6
	private void OnRequestChangeBattleState(bool isInBattle)
	{
		this.isInBattle = isInBattle;
	}

	// Token: 0x06000301 RID: 769 RVA: 0x0000BDD0 File Offset: 0x00009FD0
	public void SwitchState(eGameState targetState)
	{
		if (this.state != null)
		{
			if (targetState == this.state.Type)
			{
				return;
			}
			this.state.StateEnd();
			Object.Destroy(this.state);
		}
		this.state = (AGameState)base.gameObject.AddComponent(this.dic_GameState[targetState]);
		this.state.StateStart(targetState);
		Debug.Log(string.Format("切換遊戲狀態: {0}", targetState));
		EventMgr.SendEvent<eGameState>(eGameEvents.GameStateChanged, targetState);
	}

	// Token: 0x06000302 RID: 770 RVA: 0x0000BE60 File Offset: 0x0000A060
	public eGameState GetCurrentState()
	{
		return this.state.Type;
	}

	// Token: 0x06000303 RID: 771 RVA: 0x0000BE6D File Offset: 0x0000A06D
	public bool IsCurrentState(eGameState targetState)
	{
		return this.state.Type == targetState;
	}

	// Token: 0x04000355 RID: 853
	protected AGameState state;

	// Token: 0x04000356 RID: 854
	protected Dictionary<eGameState, Type> dic_GameState;

	// Token: 0x04000357 RID: 855
	private bool isInBattle;
}
