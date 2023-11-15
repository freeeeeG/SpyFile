using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000188 RID: 392
public class BuildingState : BattleOperationState
{
	// Token: 0x1700036D RID: 877
	// (get) Token: 0x06000A02 RID: 2562 RVA: 0x0001B586 File Offset: 0x00019786
	public override StateName StateName
	{
		get
		{
			return StateName.BuildingState;
		}
	}

	// Token: 0x06000A03 RID: 2563 RVA: 0x0001B589 File Offset: 0x00019789
	public BuildingState(GameManager gameManager, BoardSystem gameBoard, FuncUI funcUI, ShapeSelectUI shapeUI, TechSelectUI techUI) : base(gameManager)
	{
		this.m_Board = gameBoard;
		this.m_FuncUI = funcUI;
		this.m_ShapeUI = shapeUI;
		this.m_TechUI = techUI;
	}

	// Token: 0x06000A04 RID: 2564 RVA: 0x0001B5B0 File Offset: 0x000197B0
	public override IEnumerator EnterState()
	{
		this.m_Board.TransparentPath(new Color(0.2f, 1f, 1f, 0.5f), 0.3f);
		Singleton<Sound>.Instance.PlayBg("Music_Preparing");
		yield return new WaitForSeconds(0.3f);
		this.gameManager.OperationState = this;
		if (!this.m_ShapeUI.IsVisible() && !this.m_TechUI.IsVisible())
		{
			this.m_FuncUI.Show();
		}
		yield break;
	}

	// Token: 0x06000A05 RID: 2565 RVA: 0x0001B5BF File Offset: 0x000197BF
	public override IEnumerator ExitState(BattleOperationState newState)
	{
		this.gameManager.StartCoroutine(newState.EnterState());
		yield break;
	}

	// Token: 0x04000552 RID: 1362
	private BoardSystem m_Board;

	// Token: 0x04000553 RID: 1363
	private FuncUI m_FuncUI;

	// Token: 0x04000554 RID: 1364
	private ShapeSelectUI m_ShapeUI;

	// Token: 0x04000555 RID: 1365
	private TechSelectUI m_TechUI;
}
