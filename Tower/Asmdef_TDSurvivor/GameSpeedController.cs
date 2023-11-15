using System;
using UnityEngine;

// Token: 0x02000092 RID: 146
public class GameSpeedController : Singleton<GameSpeedController>
{
	// Token: 0x17000041 RID: 65
	// (get) Token: 0x060002EE RID: 750 RVA: 0x0000BA57 File Offset: 0x00009C57
	public bool IsInBattle
	{
		get
		{
			return this.isInBattle;
		}
	}

	// Token: 0x060002EF RID: 751 RVA: 0x0000BA60 File Offset: 0x00009C60
	private new void Awake()
	{
		EventMgr.Register<float>(eGameEvents.RequestModifySystemGameSpeed, new Action<float>(this.OnRequestModifySystemGameSpeed));
		EventMgr.Register<float>(eGameEvents.RequestModifyBattleGameSpeed, new Action<float>(this.OnRequestModifyBattleGameSpeed));
		EventMgr.Register<float>(eGameEvents.RequestModifyBasicGameSpeed, new Action<float>(this.OnRequestModifyBasicGameSpeed));
		EventMgr.Register<float>(eGameEvents.RequestModifyDebugGameSpeed, new Action<float>(this.OnRequestModifyDebugGameSpeed));
		EventMgr.Register(eGameEvents.OnBattleStart, new Action(this.OnBattleStart));
		EventMgr.Register(eGameEvents.OnBattleEnd, new Action(this.OnBattleEnd));
	}

	// Token: 0x060002F0 RID: 752 RVA: 0x0000BB00 File Offset: 0x00009D00
	private void OnDestroy()
	{
		EventMgr.Remove<float>(eGameEvents.RequestModifySystemGameSpeed, new Action<float>(this.OnRequestModifySystemGameSpeed));
		EventMgr.Remove<float>(eGameEvents.RequestModifyBattleGameSpeed, new Action<float>(this.OnRequestModifyBattleGameSpeed));
		EventMgr.Remove<float>(eGameEvents.RequestModifyBattleGameSpeed, new Action<float>(this.OnRequestModifyBasicGameSpeed));
		EventMgr.Remove<float>(eGameEvents.RequestModifyDebugGameSpeed, new Action<float>(this.OnRequestModifyDebugGameSpeed));
		EventMgr.Remove(eGameEvents.OnBattleStart, new Action(this.OnBattleStart));
		EventMgr.Remove(eGameEvents.OnBattleEnd, new Action(this.OnBattleEnd));
		Time.timeScale = 1f;
	}

	// Token: 0x060002F1 RID: 753 RVA: 0x0000BBA7 File Offset: 0x00009DA7
	private void OnRequestModifySystemGameSpeed(float value)
	{
		this.systemGameSpeed = value;
		this.UpdateTotalSpeed();
	}

	// Token: 0x060002F2 RID: 754 RVA: 0x0000BBB6 File Offset: 0x00009DB6
	private void OnRequestModifyBattleGameSpeed(float value)
	{
		this.battleModeGameSpeed = value;
		this.UpdateTotalSpeed();
	}

	// Token: 0x060002F3 RID: 755 RVA: 0x0000BBC5 File Offset: 0x00009DC5
	private void OnRequestModifyBasicGameSpeed(float value)
	{
		if (value <= 0f)
		{
			value = 0.001f;
		}
		this.basicGameSpeed = value;
		this.UpdateTotalSpeed();
	}

	// Token: 0x060002F4 RID: 756 RVA: 0x0000BBE3 File Offset: 0x00009DE3
	private void OnRequestModifyDebugGameSpeed(float value)
	{
		this.debugGameSpeed = value;
		this.UpdateTotalSpeed();
	}

	// Token: 0x060002F5 RID: 757 RVA: 0x0000BBF2 File Offset: 0x00009DF2
	private void OnBattleStart()
	{
		this.isInBattle = true;
		this.UpdateTotalSpeed();
	}

	// Token: 0x060002F6 RID: 758 RVA: 0x0000BC01 File Offset: 0x00009E01
	private void OnBattleEnd()
	{
		this.isInBattle = false;
		this.UpdateTotalSpeed();
	}

	// Token: 0x060002F7 RID: 759 RVA: 0x0000BC10 File Offset: 0x00009E10
	private void UpdateTotalSpeed()
	{
		if (this.isInBattle)
		{
			Time.timeScale = this.battleModeGameSpeed * this.basicGameSpeed * this.debugGameSpeed * this.systemGameSpeed;
			return;
		}
		Time.timeScale = this.basicGameSpeed * this.debugGameSpeed * this.systemGameSpeed;
	}

	// Token: 0x04000350 RID: 848
	private bool isInBattle;

	// Token: 0x04000351 RID: 849
	private float systemGameSpeed = 1f;

	// Token: 0x04000352 RID: 850
	private float battleModeGameSpeed = 1f;

	// Token: 0x04000353 RID: 851
	private float basicGameSpeed = 1f;

	// Token: 0x04000354 RID: 852
	private float debugGameSpeed = 1f;
}
