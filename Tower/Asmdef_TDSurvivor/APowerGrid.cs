using System;
using System.Collections;
using Lean.Touch;
using UnityEngine;

// Token: 0x0200000E RID: 14
public abstract class APowerGrid : MonoBehaviour
{
	// Token: 0x17000001 RID: 1
	// (get) Token: 0x06000019 RID: 25 RVA: 0x00002219 File Offset: 0x00000419
	public PowerGridSettingData SettingData
	{
		get
		{
			return this.settingData;
		}
	}

	// Token: 0x0600001A RID: 26 RVA: 0x00002221 File Offset: 0x00000421
	private void Start()
	{
		Singleton<GridSystem>.Instance.RegisterPowerGridObject(this);
		this.state = APowerGrid.ePowerGridState.IDLE;
		this.animator.SetBool("isOn", true);
		EventMgr.SendEvent<eTutorialType>(eGameEvents.QueueTutorialForGameStart, eTutorialType.BUFF_GRID);
	}

	// Token: 0x0600001B RID: 27 RVA: 0x00002254 File Offset: 0x00000454
	private void OnDestroy()
	{
		if (Singleton<GridSystem>.HasInstance())
		{
			Singleton<GridSystem>.Instance.UnregisterPowerGridObject(this);
		}
	}

	// Token: 0x0600001C RID: 28 RVA: 0x00002268 File Offset: 0x00000468
	public virtual void OnTetrisPlaced(Obj_TetrisBlock tetris)
	{
		this.state = APowerGrid.ePowerGridState.HAS_TETRIS;
		tetris.OnRemove = (Action<Obj_TetrisBlock>)Delegate.Combine(tetris.OnRemove, new Action<Obj_TetrisBlock>(this.OnTetrisRemoved));
		base.StartCoroutine(this.CR_PlaceTetrisProc(tetris));
	}

	// Token: 0x0600001D RID: 29 RVA: 0x000022A2 File Offset: 0x000004A2
	protected virtual void OnTetrisRemoved(Obj_TetrisBlock tetris)
	{
		this.state = APowerGrid.ePowerGridState.IDLE;
	}

	// Token: 0x0600001E RID: 30 RVA: 0x000022AB File Offset: 0x000004AB
	protected virtual IEnumerator CR_PlaceTetrisProc(Obj_TetrisBlock tetris)
	{
		yield return null;
		yield break;
	}

	// Token: 0x0600001F RID: 31 RVA: 0x000022B4 File Offset: 0x000004B4
	public void OnTowerPlaced(ABaseTower tower)
	{
		tower.OnTowerDespawn = (Action<ABaseTower>)Delegate.Combine(tower.OnTowerDespawn, new Action<ABaseTower>(this.OnTowerRemoved));
		this.animator.SetBool("isTowerOn", true);
		this.ApplyEffectToTower(tower);
		this.state = APowerGrid.ePowerGridState.HAS_TOWER;
		this.collider.enabled = false;
	}

	// Token: 0x06000020 RID: 32 RVA: 0x00002310 File Offset: 0x00000510
	private void OnTowerRemoved(ABaseTower tower)
	{
		tower.OnTowerDespawn = (Action<ABaseTower>)Delegate.Remove(tower.OnTowerDespawn, new Action<ABaseTower>(this.OnTowerRemoved));
		this.animator.SetBool("isTowerOn", false);
		this.RemoveEffectFromTower(tower);
		this.state = APowerGrid.ePowerGridState.HAS_TETRIS;
		this.collider.enabled = true;
	}

	// Token: 0x06000021 RID: 33 RVA: 0x0000236A File Offset: 0x0000056A
	protected virtual void ApplyEffectToTower(ABaseTower tower)
	{
	}

	// Token: 0x06000022 RID: 34 RVA: 0x0000236C File Offset: 0x0000056C
	protected virtual void RemoveEffectFromTower(ABaseTower tower)
	{
	}

	// Token: 0x06000023 RID: 35 RVA: 0x0000236E File Offset: 0x0000056E
	private void Update()
	{
	}

	// Token: 0x06000024 RID: 36 RVA: 0x00002370 File Offset: 0x00000570
	public void OnMouseEnter()
	{
		if (LeanTouch.PointOverGui(Input.mousePosition))
		{
			return;
		}
		if (!Singleton<GameStateController>.Instance.IsCurrentState(eGameState.NORMAL_MODE))
		{
			return;
		}
		string locNameString = this.GetLocNameString(false);
		string locStatsString = this.GetLocStatsString();
		EventMgr.SendEvent<bool>(eGameEvents.UI_ToggleMouseTooltip, true);
		EventMgr.SendEvent<string, string>(eGameEvents.UI_SetMouseTooltipContent, locNameString, locStatsString);
		EventMgr.SendEvent<UI_CursorToolTip.eTargetType, Transform, Vector3>(eGameEvents.UI_SetMouseTooltipTarget, UI_CursorToolTip.eTargetType._3D, base.transform, Vector3.up * 1f);
	}

	// Token: 0x06000025 RID: 37 RVA: 0x000023F5 File Offset: 0x000005F5
	public void OnMouseExit()
	{
		EventMgr.SendEvent<bool>(eGameEvents.UI_ToggleMouseTooltip, false);
	}

	// Token: 0x06000026 RID: 38 RVA: 0x00002408 File Offset: 0x00000608
	public virtual string GetLocNameString(bool isPrefix = true)
	{
		return LocalizationManager.Instance.GetString("PowerGrid", this.SettingData.PowerGridType.ToString(), Array.Empty<object>());
	}

	// Token: 0x06000027 RID: 39 RVA: 0x00002442 File Offset: 0x00000642
	public virtual string GetLocStatsString()
	{
		return "";
	}

	// Token: 0x0400001A RID: 26
	[SerializeField]
	protected PowerGridSettingData settingData;

	// Token: 0x0400001B RID: 27
	[SerializeField]
	protected Animator animator;

	// Token: 0x0400001C RID: 28
	[SerializeField]
	protected Collider collider;

	// Token: 0x0400001D RID: 29
	[SerializeField]
	protected APowerGrid.ePowerGridState state;

	// Token: 0x020001D2 RID: 466
	public enum ePowerGridState
	{
		// Token: 0x0400099F RID: 2463
		IDLE,
		// Token: 0x040009A0 RID: 2464
		HAS_TETRIS,
		// Token: 0x040009A1 RID: 2465
		HAS_TOWER
	}
}
