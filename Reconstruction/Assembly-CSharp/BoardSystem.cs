using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Pathfinding;
using UnityEngine;

// Token: 0x02000025 RID: 37
public class BoardSystem : IGameSystem
{
	// Token: 0x17000002 RID: 2
	// (get) Token: 0x0600008E RID: 142 RVA: 0x000048E6 File Offset: 0x00002AE6
	// (set) Token: 0x0600008F RID: 143 RVA: 0x000048F0 File Offset: 0x00002AF0
	public static TileBase PreviewEquipTile
	{
		get
		{
			return BoardSystem.previewEquipTile;
		}
		set
		{
			BoardSystem.previewEquipTile = value;
			if (BoardSystem.previewEquipTile != null)
			{
				BoardSystem.equipAnim.transform.position = BoardSystem.previewEquipTile.transform.position;
			}
			BoardSystem.equipAnim.SetActive(BoardSystem.previewEquipTile != null);
		}
	}

	// Token: 0x17000003 RID: 3
	// (get) Token: 0x06000090 RID: 144 RVA: 0x00004943 File Offset: 0x00002B43
	public bool IsLongPress
	{
		get
		{
			return this.pressCounter >= 0.2f;
		}
	}

	// Token: 0x17000004 RID: 4
	// (get) Token: 0x06000091 RID: 145 RVA: 0x00004955 File Offset: 0x00002B55
	// (set) Token: 0x06000092 RID: 146 RVA: 0x0000495C File Offset: 0x00002B5C
	public static TileBase SelectingTile
	{
		get
		{
			return BoardSystem.selectingTile;
		}
		set
		{
			if (BoardSystem.selectingTile != null)
			{
				BoardSystem.selectingTile.OnTileSelected(false);
				BoardSystem.selectingTile = ((BoardSystem.selectingTile == value) ? null : value);
				Singleton<TipsManager>.Instance.HideTips();
			}
			else
			{
				BoardSystem.selectingTile = value;
			}
			if (BoardSystem.selectingTile != null)
			{
				BoardSystem.selection.transform.position = BoardSystem.selectingTile.transform.position;
				BoardSystem.selectingTile.OnTileSelected(true);
			}
			BoardSystem.selection.SetActive(BoardSystem.selectingTile != null);
			Singleton<Sound>.Instance.PlayUISound("Sound_Click");
		}
	}

	// Token: 0x17000005 RID: 5
	// (get) Token: 0x06000093 RID: 147 RVA: 0x00004A03 File Offset: 0x00002C03
	public static int PathLength
	{
		get
		{
			return BoardSystem.shortestPoints.Count;
		}
	}

	// Token: 0x17000006 RID: 6
	// (get) Token: 0x06000094 RID: 148 RVA: 0x00004A0F File Offset: 0x00002C0F
	// (set) Token: 0x06000095 RID: 149 RVA: 0x00004A17 File Offset: 0x00002C17
	public GameTile SpawnPoint
	{
		get
		{
			return this.spawnPoint;
		}
		set
		{
			this.spawnPoint = value;
		}
	}

	// Token: 0x17000007 RID: 7
	// (get) Token: 0x06000096 RID: 150 RVA: 0x00004A20 File Offset: 0x00002C20
	// (set) Token: 0x06000097 RID: 151 RVA: 0x00004A28 File Offset: 0x00002C28
	public GameTile DestinationPoint
	{
		get
		{
			return this.destinationPoint;
		}
		set
		{
			this.destinationPoint = value;
		}
	}

	// Token: 0x17000008 RID: 8
	// (get) Token: 0x06000098 RID: 152 RVA: 0x00004A31 File Offset: 0x00002C31
	// (set) Token: 0x06000099 RID: 153 RVA: 0x00004A38 File Offset: 0x00002C38
	public static bool FindPath { get; set; }

	// Token: 0x0600009A RID: 154 RVA: 0x00004A40 File Offset: 0x00002C40
	public override void Initialize()
	{
		BoardSystem.selection = base.transform.Find("Selection").gameObject;
		BoardSystem.equipAnim = base.transform.Find("EquipAnim").gameObject;
		Singleton<GameEvents>.Instance.onSeekPath += this.SeekPath;
		Singleton<GameEvents>.Instance.onTileClick += this.TileClick;
		Singleton<GameEvents>.Instance.onTileUp += this.TileUp;
	}

	// Token: 0x0600009B RID: 155 RVA: 0x00004AC4 File Offset: 0x00002CC4
	public override void Release()
	{
		Singleton<GameEvents>.Instance.onSeekPath -= this.SeekPath;
		Singleton<GameEvents>.Instance.onTileClick -= this.TileClick;
		Singleton<GameEvents>.Instance.onTileUp -= this.TileUp;
	}

	// Token: 0x0600009C RID: 156 RVA: 0x00004B14 File Offset: 0x00002D14
	public override void GameUpdate()
	{
		this.pathProgress += Time.deltaTime * 0.8f;
		foreach (PathArrow pathArrow in this.pathArrows)
		{
			pathArrow.PathArrowUpdate(this.pathProgress);
		}
		if (this.pathProgress >= 1f)
		{
			this.pathProgress = 0f;
		}
	}

	// Token: 0x0600009D RID: 157 RVA: 0x00004B9C File Offset: 0x00002D9C
	private void TileClick()
	{
		this.startPos = Input.mousePosition;
	}

	// Token: 0x0600009E RID: 158 RVA: 0x00004BA9 File Offset: 0x00002DA9
	private void TileUp(TileBase tile)
	{
		this.moveDis = Vector2.SqrMagnitude(Input.mousePosition - this.startPos);
		if (this.moveDis < 1000f)
		{
			BoardSystem.SelectingTile = tile;
		}
	}

	// Token: 0x0600009F RID: 159 RVA: 0x00004BE0 File Offset: 0x00002DE0
	private void SetGameBoard()
	{
		BoardSystem._groundSize = new Vector2Int(GameRes.GroundSize, GameRes.GroundSize);
		this.sizeOffset = new Vector2Int((int)((float)(this._startSize.x - 1) * 0.5f), (int)((float)(this._startSize.y - 1) * 0.5f));
		StaticData.BoardOffset = new Vector2Int((int)((float)(BoardSystem._groundSize.x - 1) * 0.5f), (int)((float)(BoardSystem._groundSize.y - 1) * 0.5f));
		this.groundBg.size = BoardSystem._groundSize;
		this.GenerateGroundTiles(BoardSystem._groundSize);
		this.GenereteIntentLine();
		Physics2D.SyncTransforms();
		GridGraph gridGraph = AstarPath.active.data.gridGraph;
		gridGraph.SetDimensions(BoardSystem._groundSize.x, BoardSystem._groundSize.y, 1f);
		gridGraph.Scan();
	}

	// Token: 0x060000A0 RID: 160 RVA: 0x00004CC6 File Offset: 0x00002EC6
	private void PathCheck()
	{
		Physics2D.SyncTransforms();
		this.SeekPath();
		this.ShowPath();
		this.CheckPathTrap();
	}

	// Token: 0x060000A1 RID: 161 RVA: 0x00004CDF File Offset: 0x00002EDF
	public void FirstGameSet()
	{
		this.SetGameBoard();
		this.GenerateStartTiles(this._startSize, this.sizeOffset);
		this.GenerateTrapTiles(this.sizeOffset, this._startSize);
		this.PathCheck();
	}

	// Token: 0x060000A2 RID: 162 RVA: 0x00004D14 File Offset: 0x00002F14
	public void LoadSaveGame()
	{
		this.SetGameBoard();
		try
		{
			foreach (ContentStruct contentStruct in Singleton<LevelManager>.Instance.LastGameSave.SaveContents)
			{
				Vector2Int pos = contentStruct.Pos;
				GameTile gameTile;
				switch (contentStruct.ContentType)
				{
				default:
					gameTile = ConstructHelper.GetNormalTile(GameTileContentType.Empty);
					break;
				case 1:
					gameTile = ConstructHelper.GetDestinationPoint();
					this.DestinationPoint = gameTile;
					break;
				case 2:
					gameTile = ConstructHelper.GetSpawnPoint();
					this.SpawnPoint = gameTile;
					break;
				case 3:
					gameTile = ConstructHelper.GetElementTurret(contentStruct);
					break;
				case 4:
					gameTile = ConstructHelper.GetRefactorTurret(contentStruct);
					break;
				case 5:
					gameTile = ConstructHelper.GetTrap(contentStruct.ContentName, contentStruct.TrapRevealed);
					break;
				}
				gameTile.SetRotation(contentStruct.Direction);
				gameTile.transform.position = (Vector3Int)pos;
				gameTile.TileLanded();
				Physics2D.SyncTransforms();
			}
		}
		catch
		{
			Debug.Log("Error when load game");
		}
		this.PathCheck();
		this.ShowPath();
	}

	// Token: 0x060000A3 RID: 163 RVA: 0x00004E44 File Offset: 0x00003044
	private void GenerateFixedTiles(List<Vector2> pathPoints)
	{
		for (int i = 0; i < pathPoints.Count; i++)
		{
			Vector2Int v = new Vector2Int((int)pathPoints[i].x, (int)pathPoints[i].y);
			GameTile normalTile;
			if (v.x == -1 && v.y == 0)
			{
				normalTile = ConstructHelper.GetSpawnPoint();
				this.SpawnPoint = normalTile;
			}
			else if (v.x == 1 && v.y == 0)
			{
				normalTile = ConstructHelper.GetDestinationPoint();
				this.DestinationPoint = normalTile;
			}
			else
			{
				normalTile = ConstructHelper.GetNormalTile(GameTileContentType.Empty);
			}
			normalTile.transform.position = (Vector3Int)v;
			normalTile.TileLanded();
			Physics2D.SyncTransforms();
		}
	}

	// Token: 0x060000A4 RID: 164 RVA: 0x00004EF8 File Offset: 0x000030F8
	private void GenerateStartTiles(Vector2Int size, Vector2Int offset)
	{
		int num = 0;
		for (int i = 0; i < size.y; i++)
		{
			int j = 0;
			while (j < size.x)
			{
				Vector2Int v = new Vector2Int(j, i) - offset;
				if (v.x != 0 || v.y == 0)
				{
					GameTile normalTile;
					if (v.x == -1 && v.y == 0)
					{
						normalTile = ConstructHelper.GetSpawnPoint();
						this.SpawnPoint = normalTile;
					}
					else if (v.x == 1 && v.y == 0)
					{
						normalTile = ConstructHelper.GetDestinationPoint();
						this.DestinationPoint = normalTile;
					}
					else
					{
						normalTile = ConstructHelper.GetNormalTile(GameTileContentType.Empty);
					}
					normalTile.transform.position = (Vector3Int)v;
					normalTile.TileLanded();
					Physics2D.SyncTransforms();
				}
				j++;
				num++;
			}
		}
	}

	// Token: 0x060000A5 RID: 165 RVA: 0x00004FCB File Offset: 0x000031CB
	private void SeekPath()
	{
		ABPath abpath = ABPath.Construct(this.SpawnPoint.transform.position, this.DestinationPoint.transform.position, new OnPathDelegate(this.OnPathComplete));
		AstarPath.StartPath(abpath, false);
		AstarPath.BlockUntilCalculated(abpath);
	}

	// Token: 0x060000A6 RID: 166 RVA: 0x0000500A File Offset: 0x0000320A
	private IEnumerator SeekPathCor()
	{
		yield return new WaitForSeconds(0.1f);
		ABPath abpath = ABPath.Construct(this.SpawnPoint.transform.position, this.DestinationPoint.transform.position, new OnPathDelegate(this.OnPathComplete));
		AstarPath.StartPath(abpath, false);
		AstarPath.BlockUntilCalculated(abpath);
		yield break;
	}

	// Token: 0x060000A7 RID: 167 RVA: 0x0000501C File Offset: 0x0000321C
	private void OnPathComplete(Path p)
	{
		if (p.error)
		{
			BoardSystem.path = p;
			this.HidePath();
			BoardSystem.FindPath = false;
			return;
		}
		BoardSystem.FindPath = true;
		if (BoardSystem.path != null && p.vectorPath.SequenceEqual(BoardSystem.path.vectorPath))
		{
			return;
		}
		BoardSystem.path = p;
		this.ShowPath();
	}

	// Token: 0x060000A8 RID: 168 RVA: 0x00005078 File Offset: 0x00003278
	public static List<PathPoint> GetReversePath()
	{
		List<PathPoint> list = new List<PathPoint>();
		PathPoint item = default(PathPoint);
		for (int i = BoardSystem.path.vectorPath.Count - 1; i >= 0; i--)
		{
			if (i > 0)
			{
				Direction direction = DirectionExtensions.GetDirection(BoardSystem.path.vectorPath[i], BoardSystem.path.vectorPath[i - 1]);
				item = new PathPoint(BoardSystem.path.vectorPath[i], direction, BoardSystem.path.vectorPath[i] + direction.GetHalfVector());
			}
			else
			{
				Direction direction = DirectionExtensions.GetDirection(BoardSystem.path.vectorPath[i + 1], BoardSystem.path.vectorPath[i]);
				item = new PathPoint(BoardSystem.path.vectorPath[i], direction, BoardSystem.path.vectorPath[i]);
			}
			list.Add(item);
		}
		return list;
	}

	// Token: 0x060000A9 RID: 169 RVA: 0x00005184 File Offset: 0x00003384
	public void GetPathPoints()
	{
		BoardSystem.shortestPoints.Clear();
		this.arrowPoints.Clear();
		for (int i = 0; i < BoardSystem.path.vectorPath.Count; i++)
		{
			PathPoint item;
			if (i < BoardSystem.path.vectorPath.Count - 1)
			{
				Direction direction = DirectionExtensions.GetDirection(BoardSystem.path.vectorPath[i], BoardSystem.path.vectorPath[i + 1]);
				item = new PathPoint(BoardSystem.path.vectorPath[i], direction, BoardSystem.path.vectorPath[i] + direction.GetHalfVector());
			}
			else
			{
				Direction direction = DirectionExtensions.GetDirection(BoardSystem.path.vectorPath[i - 1], BoardSystem.path.vectorPath[i]);
				item = new PathPoint(BoardSystem.path.vectorPath[i], direction, BoardSystem.path.vectorPath[i]);
			}
			this.arrowPoints.Add(item);
			BoardSystem.shortestPoints.Add(item);
		}
		if (GameRes.Reverse)
		{
			for (int j = BoardSystem.path.vectorPath.Count - 1; j >= 0; j--)
			{
				PathPoint item;
				if (j == BoardSystem.path.vectorPath.Count - 1)
				{
					Direction direction = DirectionExtensions.GetDirection(BoardSystem.path.vectorPath[j], BoardSystem.path.vectorPath[j - 1]);
					item = new PathPoint(BoardSystem.path.vectorPath[j], direction, BoardSystem.path.vectorPath[j]);
					BoardSystem.shortestPoints.Add(item);
					item = new PathPoint(BoardSystem.path.vectorPath[j], direction, BoardSystem.path.vectorPath[j] + direction.GetHalfVector());
				}
				else if (j > 0)
				{
					Direction direction = DirectionExtensions.GetDirection(BoardSystem.path.vectorPath[j], BoardSystem.path.vectorPath[j - 1]);
					item = new PathPoint(BoardSystem.path.vectorPath[j], direction, BoardSystem.path.vectorPath[j] + direction.GetHalfVector());
				}
				else
				{
					Direction direction = DirectionExtensions.GetDirection(BoardSystem.path.vectorPath[1], BoardSystem.path.vectorPath[0]);
					item = new PathPoint(BoardSystem.path.vectorPath[j], direction, BoardSystem.path.vectorPath[j]);
				}
				BoardSystem.shortestPoints.Add(item);
			}
		}
	}

	// Token: 0x060000AA RID: 170 RVA: 0x0000546C File Offset: 0x0000366C
	public void ShowPath()
	{
		this.GetPathPoints();
		this.HidePath();
		for (int i = 0; i < this.arrowPoints.Count - 1; i++)
		{
			PathArrow pathArrow = Singleton<ObjectPool>.Instance.Spawn(this.pathArrowPrefab) as PathArrow;
			pathArrow.SpawnOn(i, this.arrowPoints);
			this.pathArrows.Add(pathArrow);
		}
		this.pathProgress = 0.5f;
	}

	// Token: 0x060000AB RID: 171 RVA: 0x000054D8 File Offset: 0x000036D8
	private void HidePath()
	{
		foreach (PathArrow go in this.pathArrows)
		{
			Singleton<ObjectPool>.Instance.UnSpawn(go);
		}
		this.pathArrows.Clear();
	}

	// Token: 0x060000AC RID: 172 RVA: 0x0000553C File Offset: 0x0000373C
	public void TransparentPath(Color color, float time)
	{
		this.pathArrowMat.DOColor(color, time);
	}

	// Token: 0x060000AD RID: 173 RVA: 0x0000554C File Offset: 0x0000374C
	public void GetPathTiles()
	{
		foreach (BasicTile basicTile in BoardSystem.shortestPath)
		{
			basicTile.isPath = false;
		}
		BoardSystem.shortestPath.Clear();
		foreach (PathPoint pathPoint in BoardSystem.shortestPoints)
		{
			BasicTile component = StaticData.RaycastCollider(pathPoint.PathPos, LayerMask.GetMask(new string[]
			{
				StaticData.ConcreteTileMask
			})).GetComponent<BasicTile>();
			component.isPath = true;
			BoardSystem.shortestPath.Add(component);
		}
		foreach (TrapContent trapContent in this.battleTraps)
		{
			trapContent.ClearTurnData();
		}
	}

	// Token: 0x060000AE RID: 174 RVA: 0x00005658 File Offset: 0x00003858
	private void GenerateTrapTiles(Vector2Int offset, Vector2Int size)
	{
		List<Vector2Int> list = new List<Vector2Int>();
		List<Vector2Int> list2 = this.tilePoss.ToList<Vector2Int>();
		for (int i = 0; i < size.y; i++)
		{
			for (int j = 0; j < size.x; j++)
			{
				Vector2Int item = new Vector2Int(j, i) - offset;
				list2.Remove(item);
			}
		}
		for (int k = 0; k < Singleton<LevelManager>.Instance.CurrentLevel.TrapCount; k++)
		{
			int index = Random.Range(0, list2.Count);
			Vector2Int item2 = list2[index];
			list.Add(item2);
			List<Vector2Int> circlePoints = StaticData.GetCirclePoints(4 - GameRes.TrapDistanceAdjust, 0);
			for (int l = 0; l < circlePoints.Count; l++)
			{
				circlePoints[l] += list2[index];
			}
			list2 = list2.Except(circlePoints).ToList<Vector2Int>();
			list2.Remove(item2);
			if (list2.Count <= 0)
			{
				break;
			}
		}
		foreach (Vector2Int v in list)
		{
			GameTile randomTrap = ConstructHelper.GetRandomTrap();
			randomTrap.transform.position = (Vector3Int)v;
			randomTrap.TileLanded();
		}
	}

	// Token: 0x060000AF RID: 175 RVA: 0x000057B8 File Offset: 0x000039B8
	private void GenerateGroundTiles(Vector2Int groundSize)
	{
		int num = 0;
		for (int i = 0; i < groundSize.y; i++)
		{
			int j = 0;
			while (j < groundSize.x)
			{
				GroundTile groundTile = ConstructHelper.GetGroundTile();
				Vector2Int vector2Int = new Vector2Int(j, i) - StaticData.BoardOffset;
				groundTile.transform.position = (Vector3Int)vector2Int;
				groundTile.transform.position += Vector3.forward * 0.1f;
				StaticData.CorrectTileCoord(groundTile);
				this.tilePoss.Add(vector2Int);
				j++;
				num++;
			}
		}
	}

	// Token: 0x060000B0 RID: 176 RVA: 0x00005858 File Offset: 0x00003A58
	public void BuyOneEmptyTile()
	{
		if (StaticData.LockKeyboard)
		{
			Singleton<TipsManager>.Instance.ShowMessage(GameMultiLang.GetTraduction("TUTORIALPLEASE"));
			return;
		}
		if (Singleton<GameManager>.Instance.OperationState.StateName == StateName.WaveState)
		{
			Singleton<TipsManager>.Instance.ShowMessage(GameMultiLang.GetTraduction("NOTBUILDSTATE"));
			return;
		}
		if (StaticData.GetNodeWalkable(BoardSystem.SelectingTile))
		{
			Singleton<TipsManager>.Instance.ShowMessage(GameMultiLang.GetTraduction("ALREADYGROUND"));
			return;
		}
		if (GameRes.FreeGroundTileCount > 0)
		{
			GameRes.FreeGroundTileCount--;
		}
		else
		{
			if (!Singleton<GameManager>.Instance.ConsumeMoney(GameRes.BuyGroundCost))
			{
				return;
			}
			GameRes.BuyGroundCost += Singleton<StaticData>.Instance.BuyGroundCostMultyply;
		}
		GameTile normalTile = ConstructHelper.GetNormalTile(GameTileContentType.Empty);
		normalTile.transform.position = BoardSystem.SelectingTile.transform.position;
		normalTile.TileLanded();
		Physics2D.SyncTransforms();
		if (DraggingShape.PickingShape != null)
		{
			DraggingShape.PickingShape.ShapeFindPath();
		}
		else
		{
			this.PathCheck();
		}
		Singleton<TipsManager>.Instance.HideTips();
	}

	// Token: 0x060000B1 RID: 177 RVA: 0x0000595C File Offset: 0x00003B5C
	public void CheckPathTrap()
	{
		this.battleTraps.Clear();
		foreach (PathPoint pathPoint in BoardSystem.shortestPoints)
		{
			Collider2D collider2D = StaticData.RaycastCollider(pathPoint.PathPos, LayerMask.GetMask(new string[]
			{
				StaticData.TrapMask
			}));
			if (collider2D != null)
			{
				TrapContent component = collider2D.GetComponent<TrapContent>();
				component.RevealTrap();
				this.battleTraps.Add(component);
			}
		}
	}

	// Token: 0x060000B2 RID: 178 RVA: 0x000059F8 File Offset: 0x00003BF8
	public void SwitchContent(GameTileContent content)
	{
		Singleton<LevelManager>.Instance.GameSaveContents.Remove(content);
		content.OnSwitch();
		ConstructHelper.GetShapeByContent(content);
	}

	// Token: 0x060000B3 RID: 179 RVA: 0x00005A18 File Offset: 0x00003C18
	public void SetTutorialPoss(bool value)
	{
		if (!value)
		{
			this.DestoryTutorialPoss();
			return;
		}
		if (GameRes.ForcePlace != null)
		{
			foreach (Vector2 v in GameRes.ForcePlace.GuidePos)
			{
				GameObject item = Object.Instantiate<GameObject>(Singleton<StaticData>.Instance.TileFactory.GetTutorialPrefab(), v, Quaternion.identity, base.transform);
				this.tutorialObjs.Add(item);
			}
		}
	}

	// Token: 0x060000B4 RID: 180 RVA: 0x00005AAC File Offset: 0x00003CAC
	public void HighlightEmptySlotTurrets(bool value)
	{
		if (value)
		{
			using (List<IGameBehavior>.Enumerator enumerator = Singleton<GameManager>.Instance.refactorTurrets.behaviors.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					IGameBehavior gameBehavior = enumerator.Current;
					StrategyBase strategy = ((RefactorTurret)gameBehavior).Strategy;
					if (strategy.TurretSkills.Count < strategy.ElementSKillSlot + 1)
					{
						strategy.Concrete.m_GameTile.Highlight(true);
						this.previewSlotTiles.Add(strategy.Concrete.m_GameTile);
					}
				}
				return;
			}
		}
		foreach (GameTile gameTile in this.previewSlotTiles)
		{
			gameTile.Highlight(false);
		}
		this.previewSlotTiles.Clear();
	}

	// Token: 0x060000B5 RID: 181 RVA: 0x00005B98 File Offset: 0x00003D98
	private void DestoryTutorialPoss()
	{
		foreach (GameObject obj in this.tutorialObjs)
		{
			Object.Destroy(obj);
		}
		this.tutorialObjs.Clear();
	}

	// Token: 0x060000B6 RID: 182 RVA: 0x00005BF4 File Offset: 0x00003DF4
	private void GenereteIntentLine()
	{
		if (GameRes.IntentLineID == 0)
		{
			return;
		}
		Color color = (GameRes.IntentLineID == 1) ? Color.red : Color.cyan;
		color.a = 0.5f;
		int num = (Mathf.CeilToInt((float)(GameRes.GroundSize / 3)) % 2 == 0) ? (GameRes.GroundSize / 3 + 1) : (GameRes.GroundSize / 3);
		for (int i = -num / 2; i < num / 2 + 2; i += num)
		{
			IntentLine intentLine = Object.Instantiate<IntentLine>(this.intentLinePrefab, new Vector2((float)i - 0.5f, 0f), Quaternion.Euler(0f, 0f, 0f));
			intentLine.GetComponent<SpriteRenderer>().size = new Vector2(0.2f, (float)GameRes.GroundSize);
			intentLine.GetComponent<BoxCollider2D>().size = new Vector2(0.1f, (float)GameRes.GroundSize);
			intentLine.GetComponent<SpriteRenderer>().color = color;
			intentLine.IntensifyValue = ((GameRes.IntentLineID == 1) ? -1f : 1f);
			IntentLine intentLine2 = Object.Instantiate<IntentLine>(this.intentLinePrefab, new Vector2(0f, (float)i - 0.5f), Quaternion.Euler(0f, 0f, 90f));
			intentLine2.GetComponent<SpriteRenderer>().size = new Vector2(0.2f, (float)GameRes.GroundSize);
			intentLine2.GetComponent<BoxCollider2D>().size = new Vector2(0.1f, (float)GameRes.GroundSize);
			intentLine2.GetComponent<SpriteRenderer>().color = color;
			intentLine2.IntensifyValue = ((GameRes.IntentLineID == 1) ? -1f : 1f);
		}
	}

	// Token: 0x060000B7 RID: 183 RVA: 0x00005D89 File Offset: 0x00003F89
	private void OnApplicationFocus(bool focus)
	{
		if (BoardSystem.FindPath)
		{
			this.ShowPath();
		}
	}

	// Token: 0x040000CF RID: 207
	private static GameObject equipAnim;

	// Token: 0x040000D0 RID: 208
	private static TileBase previewEquipTile;

	// Token: 0x040000D1 RID: 209
	private static GameObject selection;

	// Token: 0x040000D2 RID: 210
	private float pressCounter;

	// Token: 0x040000D3 RID: 211
	public bool IsPressingTile;

	// Token: 0x040000D4 RID: 212
	private float moveDis;

	// Token: 0x040000D5 RID: 213
	private Vector3 startPos;

	// Token: 0x040000D6 RID: 214
	private Vector2Int sizeOffset;

	// Token: 0x040000D7 RID: 215
	private static TileBase selectingTile;

	// Token: 0x040000D8 RID: 216
	public Vector2Int _startSize = new Vector2Int(3, 3);

	// Token: 0x040000D9 RID: 217
	public static Vector2Int _groundSize;

	// Token: 0x040000DA RID: 218
	[SerializeField]
	private PathArrow pathArrowPrefab;

	// Token: 0x040000DB RID: 219
	private List<PathArrow> pathArrows = new List<PathArrow>();

	// Token: 0x040000DC RID: 220
	private List<Vector2Int> tilePoss = new List<Vector2Int>();

	// Token: 0x040000DD RID: 221
	public static List<BasicTile> shortestPath = new List<BasicTile>();

	// Token: 0x040000DE RID: 222
	public static List<PathPoint> shortestPoints = new List<PathPoint>();

	// Token: 0x040000DF RID: 223
	private List<PathPoint> arrowPoints = new List<PathPoint>();

	// Token: 0x040000E0 RID: 224
	public static Path path;

	// Token: 0x040000E1 RID: 225
	private GameTile spawnPoint;

	// Token: 0x040000E2 RID: 226
	private GameTile destinationPoint;

	// Token: 0x040000E3 RID: 227
	private List<TrapContent> battleTraps = new List<TrapContent>();

	// Token: 0x040000E4 RID: 228
	private List<GameObject> tutorialObjs = new List<GameObject>();

	// Token: 0x040000E5 RID: 229
	[SerializeField]
	private Material pathArrowMat;

	// Token: 0x040000E6 RID: 230
	[SerializeField]
	private SpriteRenderer groundBg;

	// Token: 0x040000E7 RID: 231
	[SerializeField]
	private IntentLine intentLinePrefab;

	// Token: 0x040000E9 RID: 233
	public static TrapContent LastTrap;

	// Token: 0x040000EA RID: 234
	private float pathProgress;

	// Token: 0x040000EB RID: 235
	private List<GameTile> previewSlotTiles = new List<GameTile>();
}
