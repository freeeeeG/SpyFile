using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// Token: 0x02000184 RID: 388
public class TileShape : MonoBehaviour
{
	// Token: 0x060009F4 RID: 2548 RVA: 0x0001B140 File Offset: 0x00019340
	private void Awake()
	{
		this.m_Anim = base.GetComponent<Animator>();
		this.tilePos = base.transform.GetComponentsInChildren<TileSlot>().ToList<TileSlot>();
		foreach (TileSlot tileSlot in this.tilePos)
		{
			if (tileSlot.IsTurretPos)
			{
				this.turretPos.Add(tileSlot);
			}
		}
		this.renderCam = base.transform.Find("RenderCam").GetComponent<Camera>();
		this.bgObj = base.transform.Find("BG").gameObject;
		this.draggingShape = base.GetComponent<DraggingShape>();
	}

	// Token: 0x060009F5 RID: 2549 RVA: 0x0001B204 File Offset: 0x00019404
	private void LateUpdate()
	{
	}

	// Token: 0x060009F6 RID: 2550 RVA: 0x0001B208 File Offset: 0x00019408
	public void SetTile(GameTile specialTile, int posID = -1, int dir = -1)
	{
		if (specialTile.Content.ContentType == GameTileContentType.ElementTurret)
		{
			this.m_ElementTurret = (specialTile.Content as ElementTurret);
		}
		if (this.shapeType == ShapeType.D)
		{
			specialTile.transform.position = this.tilePos[0].transform.position;
			specialTile.transform.SetParent(base.transform);
			specialTile.m_DraggingShape = this.draggingShape;
			this.tiles.Add(specialTile);
			this.draggingShape.Initialized(this);
			this.SetPreviewPlace();
			return;
		}
		TileSlot tileSlot = this.tilePos[(posID == -1) ? Random.Range(0, this.tilePos.Count) : posID];
		GameTileContent content = specialTile.Content;
		this.SetTilePos(specialTile, tileSlot.transform.position, dir);
		this.tilePos.Remove(tileSlot);
		for (int i = 0; i < this.tilePos.Count; i++)
		{
			GameTile normalTile = ConstructHelper.GetNormalTile(GameTileContentType.Empty);
			this.SetTilePos(normalTile, this.tilePos[i].transform.position, -1);
		}
		this.draggingShape.Initialized(this);
	}

	// Token: 0x060009F7 RID: 2551 RVA: 0x0001B32E File Offset: 0x0001952E
	private void SetTilePos(GameTile tile, Vector3 pos, int dir = -1)
	{
		tile.transform.position = pos;
		tile.SetRandomRotation(dir);
		tile.transform.SetParent(base.transform);
		tile.m_DraggingShape = this.draggingShape;
		this.tiles.Add(tile);
	}

	// Token: 0x060009F8 RID: 2552 RVA: 0x0001B36C File Offset: 0x0001956C
	public void ReclaimTiles()
	{
		foreach (GameTile go in this.tiles)
		{
			Singleton<ObjectPool>.Instance.UnSpawn(go);
		}
		Object.Destroy(base.gameObject);
	}

	// Token: 0x060009F9 RID: 2553 RVA: 0x0001B3D0 File Offset: 0x000195D0
	public void SetUIDisplay(int displayID, RenderTexture texture)
	{
		this.IsPreviewing = false;
		base.transform.position = new Vector3(1000f + 10f * (float)displayID, 0f, -12f);
		this.renderCam.targetTexture = texture;
		this.renderCam.gameObject.SetActive(true);
		this.bgObj.SetActive(true);
		this.draggingShape.MenuTrans.gameObject.SetActive(false);
	}

	// Token: 0x060009FA RID: 2554 RVA: 0x0001B44C File Offset: 0x0001964C
	public void SetPreviewPlace()
	{
		this.IsPreviewing = true;
		this.m_Anim.SetTrigger("Land");
		this.bgObj.SetActive(false);
		Vector2 vector = Camera.main.transform.position;
		this.renderCam.gameObject.SetActive(false);
		base.transform.position = new Vector3(Mathf.Round(vector.x), Mathf.Round(vector.y), -1f);
		this.draggingShape.ShapeSpawned();
		this.draggingShape.MenuTrans.gameObject.SetActive(true);
		foreach (GameTile gameTile in this.tiles)
		{
			gameTile.Previewing = true;
		}
	}

	// Token: 0x0400053C RID: 1340
	public ShapeInfo m_ShapeInfo;

	// Token: 0x0400053D RID: 1341
	private Animator m_Anim;

	// Token: 0x0400053E RID: 1342
	public bool IsPreviewing;

	// Token: 0x0400053F RID: 1343
	private List<TileSlot> tilePos = new List<TileSlot>();

	// Token: 0x04000540 RID: 1344
	private List<TileSlot> turretPos = new List<TileSlot>();

	// Token: 0x04000541 RID: 1345
	private Camera renderCam;

	// Token: 0x04000542 RID: 1346
	private GameObject bgObj;

	// Token: 0x04000543 RID: 1347
	public DraggingShape draggingShape;

	// Token: 0x04000544 RID: 1348
	public ElementTurret m_ElementTurret;

	// Token: 0x04000545 RID: 1349
	[HideInInspector]
	public ShapeSelectUI m_ShapeSelectUI;

	// Token: 0x04000546 RID: 1350
	public List<GameTile> tiles = new List<GameTile>();

	// Token: 0x04000547 RID: 1351
	public ShapeType shapeType;
}
