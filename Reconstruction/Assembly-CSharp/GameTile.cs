using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020001CF RID: 463
public abstract class GameTile : TileBase
{
	// Token: 0x1700044E RID: 1102
	// (get) Token: 0x06000BBD RID: 3005 RVA: 0x0001EAFC File Offset: 0x0001CCFC
	// (set) Token: 0x06000BBE RID: 3006 RVA: 0x0001EB04 File Offset: 0x0001CD04
	public GameTileContent Content
	{
		get
		{
			return this.content;
		}
		set
		{
			this.content = value;
		}
	}

	// Token: 0x1700044F RID: 1103
	// (get) Token: 0x06000BBF RID: 3007 RVA: 0x0001EB0D File Offset: 0x0001CD0D
	public bool isWalkable
	{
		get
		{
			return this.Content.IsWalkable;
		}
	}

	// Token: 0x17000450 RID: 1104
	// (get) Token: 0x06000BC0 RID: 3008 RVA: 0x0001EB1A File Offset: 0x0001CD1A
	// (set) Token: 0x06000BC1 RID: 3009 RVA: 0x0001EB22 File Offset: 0x0001CD22
	public DraggingShape m_DraggingShape { get; set; }

	// Token: 0x17000451 RID: 1105
	// (get) Token: 0x06000BC2 RID: 3010 RVA: 0x0001EB2B File Offset: 0x0001CD2B
	// (set) Token: 0x06000BC3 RID: 3011 RVA: 0x0001EB33 File Offset: 0x0001CD33
	public List<SpriteRenderer> TileRenderers { get; set; }

	// Token: 0x17000452 RID: 1106
	// (get) Token: 0x06000BC4 RID: 3012 RVA: 0x0001EB3C File Offset: 0x0001CD3C
	// (set) Token: 0x06000BC5 RID: 3013 RVA: 0x0001EB44 File Offset: 0x0001CD44
	public SpriteRenderer PreviewRenderer { get; set; }

	// Token: 0x17000453 RID: 1107
	// (get) Token: 0x06000BC6 RID: 3014 RVA: 0x0001EB4D File Offset: 0x0001CD4D
	// (set) Token: 0x06000BC7 RID: 3015 RVA: 0x0001EB55 File Offset: 0x0001CD55
	public Vector3 ExitPoint { get; set; }

	// Token: 0x17000454 RID: 1108
	// (get) Token: 0x06000BC8 RID: 3016 RVA: 0x0001EB5E File Offset: 0x0001CD5E
	// (set) Token: 0x06000BC9 RID: 3017 RVA: 0x0001EB66 File Offset: 0x0001CD66
	public Direction TileDirection
	{
		get
		{
			return this.tileDirection;
		}
		set
		{
			this.tileDirection = value;
		}
	}

	// Token: 0x17000455 RID: 1109
	// (get) Token: 0x06000BCA RID: 3018 RVA: 0x0001EB6F File Offset: 0x0001CD6F
	// (set) Token: 0x06000BCB RID: 3019 RVA: 0x0001EB77 File Offset: 0x0001CD77
	public override bool IsLanded
	{
		get
		{
			return base.IsLanded;
		}
		set
		{
			base.IsLanded = value;
			base.gameObject.layer = (value ? LayerMask.NameToLayer(StaticData.ConcreteTileMask) : LayerMask.NameToLayer(StaticData.TempTileMask));
		}
	}

	// Token: 0x17000456 RID: 1110
	// (get) Token: 0x06000BCC RID: 3020 RVA: 0x0001EBA4 File Offset: 0x0001CDA4
	// (set) Token: 0x06000BCD RID: 3021 RVA: 0x0001EBAC File Offset: 0x0001CDAC
	public bool Previewing
	{
		get
		{
			return this.previewing;
		}
		set
		{
			this.previewing = value;
			this.previewGlow.SetActive(value);
		}
	}

	// Token: 0x06000BCE RID: 3022 RVA: 0x0001EBC4 File Offset: 0x0001CDC4
	protected virtual void Awake()
	{
		this.previewGlow = base.transform.Find("PreviewGlow").gameObject;
		this.PreviewRenderer = this.previewGlow.GetComponent<SpriteRenderer>();
		this.tileBase = base.transform.Find("TileBase");
	}

	// Token: 0x06000BCF RID: 3023 RVA: 0x0001EC14 File Offset: 0x0001CE14
	public void SetTileColor(Color colorToSet)
	{
		foreach (SpriteRenderer spriteRenderer in this.TileRenderers)
		{
			spriteRenderer.color = colorToSet;
		}
	}

	// Token: 0x06000BD0 RID: 3024 RVA: 0x0001EC68 File Offset: 0x0001CE68
	public virtual void TileLanded()
	{
		base.SetBackToParent();
		base.transform.position = new Vector3(base.transform.position.x, base.transform.position.y, 0f);
		StaticData.CorrectTileCoord(this);
		this.Previewing = false;
		this.GetTileDirection();
		this.Content.ContentLanded();
		this.IsLanded = true;
	}

	// Token: 0x06000BD1 RID: 3025 RVA: 0x0001ECD8 File Offset: 0x0001CED8
	public virtual void SetContent(GameTileContent content)
	{
		content.transform.SetParent(base.transform);
		content.transform.position = base.transform.position + Vector3.forward * 0.01f;
		content.transform.localRotation = Quaternion.identity;
		content.m_GameTile = this;
		this.Content = content;
	}

	// Token: 0x06000BD2 RID: 3026 RVA: 0x0001ED3E File Offset: 0x0001CF3E
	public virtual void OnTilePass(Enemy enemy)
	{
		this.Content.OnContentPass(enemy, null, 0);
	}

	// Token: 0x06000BD3 RID: 3027 RVA: 0x0001ED4E File Offset: 0x0001CF4E
	public override void TileDown()
	{
		base.TileDown();
		if (this.m_DraggingShape != null)
		{
			this.m_DraggingShape.StartDragging();
		}
	}

	// Token: 0x06000BD4 RID: 3028 RVA: 0x0001ED6F File Offset: 0x0001CF6F
	public override void OnSpawn()
	{
		base.OnSpawn();
		this.IsLanded = false;
	}

	// Token: 0x06000BD5 RID: 3029 RVA: 0x0001ED80 File Offset: 0x0001CF80
	public override void OnUnSpawn()
	{
		base.OnUnSpawn();
		if (BoardSystem.SelectingTile == this)
		{
			BoardSystem.SelectingTile = null;
		}
		Singleton<ObjectPool>.Instance.UnSpawn(this.Content);
		base.gameObject.tag = StaticData.Untagged;
		this.m_DraggingShape = null;
		this.Previewing = false;
		this.SetTileColor(Color.white);
		this.Content = null;
		base.transform.rotation = Quaternion.identity;
	}

	// Token: 0x06000BD6 RID: 3030 RVA: 0x0001EDF8 File Offset: 0x0001CFF8
	public void SetRandomRotation(int dir = -1)
	{
		int index = (dir == -1) ? Random.Range(0, 4) : dir;
		base.transform.rotation = DirectionExtensions.GetDirection(index).GetRotation();
		this.CorrectRotation();
	}

	// Token: 0x06000BD7 RID: 3031 RVA: 0x0001EE30 File Offset: 0x0001D030
	public void SetRotation(int direction)
	{
		base.transform.rotation = DirectionExtensions.GetDirection(direction).GetRotation();
		this.CorrectRotation();
	}

	// Token: 0x06000BD8 RID: 3032 RVA: 0x0001EE4E File Offset: 0x0001D04E
	private void GetTileDirection()
	{
		this.TileDirection = DirectionExtensions.GetDirection(base.transform.position, base.transform.position + base.transform.up);
	}

	// Token: 0x06000BD9 RID: 3033 RVA: 0x0001EE81 File Offset: 0x0001D081
	public void CorrectRotation()
	{
		this.tileBase.rotation = Quaternion.identity;
		this.Content.CorretRotation();
	}

	// Token: 0x06000BDA RID: 3034 RVA: 0x0001EE9E File Offset: 0x0001D09E
	public void Highlight(bool value)
	{
		this.hightLightEffect.SetActive(value);
	}

	// Token: 0x040005E9 RID: 1513
	private GameTileContent content;

	// Token: 0x040005EA RID: 1514
	private GameObject previewGlow;

	// Token: 0x040005EB RID: 1515
	private Transform tileBase;

	// Token: 0x040005EC RID: 1516
	[SerializeField]
	private GameObject hightLightEffect;

	// Token: 0x040005F1 RID: 1521
	private Direction tileDirection;

	// Token: 0x040005F2 RID: 1522
	private bool previewing;
}
