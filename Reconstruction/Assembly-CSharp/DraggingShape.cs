using System;
using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using UnityEngine;

// Token: 0x0200017D RID: 381
public class DraggingShape : DraggingActions
{
	// Token: 0x17000368 RID: 872
	// (get) Token: 0x060009BD RID: 2493 RVA: 0x0001A323 File Offset: 0x00018523
	// (set) Token: 0x060009BE RID: 2494 RVA: 0x0001A32A File Offset: 0x0001852A
	public static DraggingShape PickingShape
	{
		get
		{
			return DraggingShape.pickingShape;
		}
		set
		{
			DraggingShape.pickingShape = value;
		}
	}

	// Token: 0x17000369 RID: 873
	// (get) Token: 0x060009BF RID: 2495 RVA: 0x0001A332 File Offset: 0x00018532
	// (set) Token: 0x060009C0 RID: 2496 RVA: 0x0001A33A File Offset: 0x0001853A
	public TileShape TileShape
	{
		get
		{
			return this.tileShape;
		}
		set
		{
			this.tileShape = value;
		}
	}

	// Token: 0x1700036A RID: 874
	// (get) Token: 0x060009C1 RID: 2497 RVA: 0x0001A343 File Offset: 0x00018543
	public Transform MenuTrans
	{
		get
		{
			return this.menuTrans;
		}
	}

	// Token: 0x060009C2 RID: 2498 RVA: 0x0001A34B File Offset: 0x0001854B
	public void Initialized(TileShape shape)
	{
		this.menuTrans = base.transform.Find("DragMenu");
		this.TileShape = shape;
		this.mainCam = Camera.main;
	}

	// Token: 0x060009C3 RID: 2499 RVA: 0x0001A378 File Offset: 0x00018578
	private void SetAllColor(Color colorToSet)
	{
		foreach (GameTile gameTile in this.TileShape.tiles)
		{
			gameTile.SetTileColor(colorToSet);
		}
	}

	// Token: 0x060009C4 RID: 2500 RVA: 0x0001A3D0 File Offset: 0x000185D0
	private void SetPreviewColor(Color colorToSet)
	{
		foreach (GameTile gameTile in this.TileShape.tiles)
		{
			gameTile.PreviewRenderer.color = colorToSet;
		}
	}

	// Token: 0x060009C5 RID: 2501 RVA: 0x0001A42C File Offset: 0x0001862C
	private void SetTileColor(Color colorToSet, GameTile tile)
	{
		tile.SetTileColor(colorToSet);
	}

	// Token: 0x060009C6 RID: 2502 RVA: 0x0001A435 File Offset: 0x00018635
	protected override void Update()
	{
		base.Update();
		if (this.tileShape.IsPreviewing && Singleton<InputManager>.Instance.GetKeyDown(KeyBindingActions.Rotate))
		{
			this.RotateShape();
		}
	}

	// Token: 0x060009C7 RID: 2503 RVA: 0x0001A460 File Offset: 0x00018660
	public override void OnDraggingInUpdate()
	{
		base.OnDraggingInUpdate();
		Vector3 vector = base.MouseInWorldCoords();
		base.transform.position = new Vector3(Mathf.Round(vector.x + this.pointerOffset.x), Mathf.Round(vector.y + this.pointerOffset.y), base.transform.position.z);
		if (base.transform.position != this.lastPos && this.CheckCanDrop())
		{
			base.StopAllCoroutines();
			base.StartCoroutine(this.TryFindPath());
		}
		this.lastPos = base.transform.position;
	}

	// Token: 0x060009C8 RID: 2504 RVA: 0x0001A516 File Offset: 0x00018716
	private void LateUpdate()
	{
		this.SizeUpdateWithCam();
	}

	// Token: 0x060009C9 RID: 2505 RVA: 0x0001A51E File Offset: 0x0001871E
	private void SizeUpdateWithCam()
	{
		this.menuTrans.localScale = new Vector3(this.mainCam.orthographicSize / 450f, this.mainCam.orthographicSize / 450f, 1f);
	}

	// Token: 0x060009CA RID: 2506 RVA: 0x0001A557 File Offset: 0x00018757
	public void ShapeFindPath()
	{
		base.StartCoroutine(this.TryFindPath());
	}

	// Token: 0x060009CB RID: 2507 RVA: 0x0001A566 File Offset: 0x00018766
	public void ShapeSpawned()
	{
		if (this.CheckCanDrop())
		{
			base.StartCoroutine(this.TryFindPath());
		}
		DraggingShape.PickingShape = this;
	}

	// Token: 0x060009CC RID: 2508 RVA: 0x0001A583 File Offset: 0x00018783
	private bool CheckCanDrop()
	{
		this.overLapPoint = false;
		this.skillFull = false;
		this.canDrop = true;
		Physics2D.SyncTransforms();
		this.CheckOverLap();
		this.CheckMapEdge();
		if (!this.canDrop)
		{
			this.SetAllColor(this.wrongColor);
			return false;
		}
		return true;
	}

	// Token: 0x060009CD RID: 2509 RVA: 0x0001A5C4 File Offset: 0x000187C4
	private void CheckMapEdge()
	{
		Vector2Int groundSize = BoardSystem._groundSize;
		int num = (groundSize.x - 1) / 2;
		int num2 = -(groundSize.x - 1) / 2;
		int num3 = (groundSize.y - 1) / 2;
		int num4 = -(groundSize.y - 1) / 2;
		foreach (GameTile gameTile in this.TileShape.tiles)
		{
			if (gameTile.transform.position.x > (float)num || gameTile.transform.position.x < (float)num2 || gameTile.transform.position.y > (float)num3 || gameTile.transform.position.y < (float)num4)
			{
				this.canDrop = false;
				break;
			}
		}
	}

	// Token: 0x060009CE RID: 2510 RVA: 0x0001A6B0 File Offset: 0x000188B0
	private void CheckOverLap()
	{
		BoardSystem.PreviewEquipTile = null;
		foreach (GameTile gameTile in this.TileShape.tiles)
		{
			Collider2D collider2D = StaticData.RaycastCollider(gameTile.transform.position, LayerMask.GetMask(new string[]
			{
				StaticData.ConcreteTileMask
			}));
			if (collider2D == null)
			{
				this.SetTileColor(this.correctColor, gameTile);
			}
			else if (gameTile.Content.ContentType != GameTileContentType.Empty)
			{
				if (collider2D.CompareTag(StaticData.OnlyRefactorTag))
				{
					if (!gameTile.Content.CanEquip)
					{
						this.overLapPoint = true;
						this.canDrop = false;
						break;
					}
					StrategyBase strategy = ((ConcreteContent)gameTile.Content).Strategy;
					GameTile component = collider2D.GetComponent<GameTile>();
					StrategyBase strategy2 = ((ConcreteContent)component.Content).Strategy;
					if (strategy.TurretSkills.Count - 2 <= strategy2.ElementSKillSlot - strategy2.TurretSkills.Count)
					{
						this.SetTileColor(this.equipColor, gameTile);
						BoardSystem.PreviewEquipTile = component;
						break;
					}
					this.canDrop = false;
					this.skillFull = true;
					break;
				}
				else
				{
					if (collider2D.CompareTag(StaticData.UndropablePoint))
					{
						this.canDrop = false;
						this.overLapPoint = true;
						break;
					}
					this.SetTileColor(this.correctColor, gameTile);
				}
			}
			else
			{
				this.SetTileColor(this.transparentColor, gameTile);
			}
		}
	}

	// Token: 0x060009CF RID: 2511 RVA: 0x0001A848 File Offset: 0x00018A48
	private IEnumerator TryFindPath()
	{
		this.waitingForPath = true;
		yield return new WaitForSeconds(0.1f);
		Singleton<Sound>.Instance.PlayUISound("Sound_Shape");
		this.ChangeAstarPath();
		Singleton<GameEvents>.Instance.SeekPath();
		yield return new WaitForSeconds(0.1f);
		this.waitingForPath = false;
		yield break;
	}

	// Token: 0x060009D0 RID: 2512 RVA: 0x0001A858 File Offset: 0x00018A58
	private void ChangeAstarPath()
	{
		GridGraph grid = AstarPath.active.data.gridGraph;
		this.ResetChangeNode(grid);
		using (List<GameTile>.Enumerator enumerator = this.TileShape.tiles.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				GameTile tile = enumerator.Current;
				StaticData.CorrectTileCoord(tile);
				AstarPath.active.AddWorkItem(delegate(IWorkItemContext ctx)
				{
					int x = tile.OffsetCoord.x;
					int y = tile.OffsetCoord.y;
					if (y * GameRes.GroundSize + x > GameRes.GroundSize * GameRes.GroundSize - 1 || y * GameRes.GroundSize + x < 0)
					{
						return;
					}
					GridNodeBase gridNodeBase = grid.nodes[y * grid.width + x];
					if (!gridNodeBase.ChangeAbleNode)
					{
						return;
					}
					if (gridNodeBase.Walkable != tile.isWalkable)
					{
						gridNodeBase.Walkable = !gridNodeBase.Walkable;
						this.ChangeNodes.Add(gridNodeBase);
						grid.CalculateConnectionsForCellAndNeighbours(x, y);
					}
				});
			}
		}
	}

	// Token: 0x060009D1 RID: 2513 RVA: 0x0001A90C File Offset: 0x00018B0C
	private void ResetChangeNode(GridGraph grid)
	{
		foreach (GridNodeBase gridNodeBase in this.ChangeNodes)
		{
			gridNodeBase.Walkable = !gridNodeBase.Walkable;
			grid.CalculateConnectionsForCellAndNeighbours(gridNodeBase.XCoordinateInGrid, gridNodeBase.ZCoordinateInGrid);
		}
		this.ChangeNodes.Clear();
	}

	// Token: 0x060009D2 RID: 2514 RVA: 0x0001A984 File Offset: 0x00018B84
	public void RotateShape()
	{
		base.transform.Rotate(0f, 0f, -90f);
		this.menuTrans.Rotate(0f, 0f, 90f);
		foreach (GameTile gameTile in this.TileShape.tiles)
		{
			gameTile.CorrectRotation();
		}
		if (this.CheckCanDrop())
		{
			base.StopAllCoroutines();
			base.StartCoroutine(this.TryFindPath());
			this.SetPreviewColor(this.dropColor);
		}
	}

	// Token: 0x060009D3 RID: 2515 RVA: 0x0001AA34 File Offset: 0x00018C34
	public void ConfirmShape()
	{
		if (this.waitingForPath)
		{
			Singleton<TipsManager>.Instance.ShowMessage("你点的太快了");
			return;
		}
		if (!GameRes.CheckForcePlacement(base.transform.position, base.transform.up))
		{
			Singleton<TipsManager>.Instance.ShowMessage(GameMultiLang.GetTraduction("TUTORIALPLACE"));
			return;
		}
		if (this.canDrop)
		{
			if (!BoardSystem.FindPath)
			{
				Singleton<TipsManager>.Instance.ShowMessage(GameMultiLang.GetTraduction("NOROUTE"));
				return;
			}
			Singleton<Sound>.Instance.PlayUISound("Sound_ConfirmShape");
			foreach (GameTile gameTile in this.TileShape.tiles)
			{
				gameTile.TileLanded();
			}
			Singleton<GameManager>.Instance.ConfirmShape();
			DraggingShape.PickingShape = null;
			Object.Destroy(base.gameObject);
			return;
		}
		else
		{
			if (this.overLapPoint)
			{
				Singleton<TipsManager>.Instance.ShowMessage(GameMultiLang.GetTraduction("NOOVERLAP"));
				return;
			}
			if (this.skillFull)
			{
				Singleton<TipsManager>.Instance.ShowMessage(GameMultiLang.GetTraduction("SKILLFULL"));
			}
			return;
		}
	}

	// Token: 0x060009D4 RID: 2516 RVA: 0x0001AB68 File Offset: 0x00018D68
	public void UndoShape()
	{
		if (StaticData.LockKeyboard)
		{
			Singleton<TipsManager>.Instance.ShowMessage(GameMultiLang.GetTraduction("TUTORIALPLEASE"));
			return;
		}
		BoardSystem.PreviewEquipTile = null;
		base.StartCoroutine(this.UndoShapeCor());
	}

	// Token: 0x060009D5 RID: 2517 RVA: 0x0001AB99 File Offset: 0x00018D99
	private IEnumerator UndoShapeCor()
	{
		if (this.tileShape.shapeType == ShapeType.D)
		{
			this.tileShape.tiles[0].Content.OnUndo();
		}
		else
		{
			Singleton<GameManager>.Instance.UndoShape();
		}
		GridGraph gridGraph = AstarPath.active.data.gridGraph;
		this.ResetChangeNode(gridGraph);
		base.transform.position = Vector3.one * 1000f;
		this.ShapeFindPath();
		while (this.waitingForPath)
		{
			yield return null;
		}
		this.tileShape.ReclaimTiles();
		yield break;
	}

	// Token: 0x060009D6 RID: 2518 RVA: 0x0001ABA8 File Offset: 0x00018DA8
	public override void StartDragging()
	{
		this.isDragging = true;
		DraggingActions.DraggingThis = this;
		this.pointerOffset = base.transform.position - base.MouseInWorldCoords();
		this.pointerOffset.z = 0f;
		this.SetPreviewColor(this.holdColor);
	}

	// Token: 0x060009D7 RID: 2519 RVA: 0x0001ABFA File Offset: 0x00018DFA
	public override void EndDragging()
	{
		if (this.isDragging)
		{
			if (this.CheckCanDrop())
			{
				this.SetPreviewColor(this.dropColor);
			}
			else
			{
				this.SetPreviewColor(this.wrongColor);
			}
			this.isDragging = false;
			DraggingActions.DraggingThis = null;
		}
	}

	// Token: 0x04000512 RID: 1298
	private Vector2 lastPos;

	// Token: 0x04000513 RID: 1299
	private Transform menuTrans;

	// Token: 0x04000514 RID: 1300
	private Camera mainCam;

	// Token: 0x04000515 RID: 1301
	private static DraggingShape pickingShape;

	// Token: 0x04000516 RID: 1302
	private TileShape tileShape;

	// Token: 0x04000517 RID: 1303
	private bool canDrop;

	// Token: 0x04000518 RID: 1304
	private bool overLapPoint;

	// Token: 0x04000519 RID: 1305
	private bool skillFull;

	// Token: 0x0400051A RID: 1306
	private bool waitingForPath;

	// Token: 0x0400051B RID: 1307
	private Collider2D[] attachedResult = new Collider2D[10];

	// Token: 0x0400051C RID: 1308
	[SerializeField]
	private Color wrongColor;

	// Token: 0x0400051D RID: 1309
	[SerializeField]
	private Color correctColor;

	// Token: 0x0400051E RID: 1310
	[SerializeField]
	private Color transparentColor;

	// Token: 0x0400051F RID: 1311
	[SerializeField]
	private Color holdColor;

	// Token: 0x04000520 RID: 1312
	[SerializeField]
	private Color dropColor;

	// Token: 0x04000521 RID: 1313
	[SerializeField]
	private Color equipColor;

	// Token: 0x04000522 RID: 1314
	private List<GridNodeBase> ChangeNodes = new List<GridNodeBase>();
}
