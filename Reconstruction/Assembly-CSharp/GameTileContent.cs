using System;
using UnityEngine;

// Token: 0x020001D6 RID: 470
public abstract class GameTileContent : ReusableObject
{
	// Token: 0x17000462 RID: 1122
	// (get) Token: 0x06000C0D RID: 3085 RVA: 0x0001F59A File Offset: 0x0001D79A
	public virtual bool IsWalkable
	{
		get
		{
			return true;
		}
	}

	// Token: 0x17000463 RID: 1123
	// (get) Token: 0x06000C0E RID: 3086 RVA: 0x0001F59D File Offset: 0x0001D79D
	public virtual GameTileContentType ContentType { get; }

	// Token: 0x17000464 RID: 1124
	// (get) Token: 0x06000C0F RID: 3087 RVA: 0x0001F5A5 File Offset: 0x0001D7A5
	// (set) Token: 0x06000C10 RID: 3088 RVA: 0x0001F5AD File Offset: 0x0001D7AD
	public GameTile m_GameTile
	{
		get
		{
			return this.m_gameTile;
		}
		set
		{
			this.m_gameTile = value;
		}
	}

	// Token: 0x17000465 RID: 1125
	// (get) Token: 0x06000C11 RID: 3089 RVA: 0x0001F5B6 File Offset: 0x0001D7B6
	public virtual bool CanEquip
	{
		get
		{
			return false;
		}
	}

	// Token: 0x17000466 RID: 1126
	// (get) Token: 0x06000C12 RID: 3090 RVA: 0x0001F5B9 File Offset: 0x0001D7B9
	// (set) Token: 0x06000C13 RID: 3091 RVA: 0x0001F5C1 File Offset: 0x0001D7C1
	public bool IsSwitching { get; set; }

	// Token: 0x06000C14 RID: 3092 RVA: 0x0001F5CC File Offset: 0x0001D7CC
	public virtual void ContentLanded()
	{
		Collider2D collider2D = StaticData.RaycastCollider(base.transform.position, LayerMask.GetMask(new string[]
		{
			StaticData.GroundTileMask
		}));
		if (collider2D != null)
		{
			collider2D.GetComponent<GroundTile>().IsLanded = false;
		}
		Singleton<LevelManager>.Instance.GameSaveContents.Add(this);
	}

	// Token: 0x06000C15 RID: 3093 RVA: 0x0001F62C File Offset: 0x0001D82C
	public virtual void OnContentSelected(bool value)
	{
	}

	// Token: 0x06000C16 RID: 3094 RVA: 0x0001F62E File Offset: 0x0001D82E
	protected virtual void ContentLandedCheck(Collider2D col)
	{
	}

	// Token: 0x06000C17 RID: 3095 RVA: 0x0001F630 File Offset: 0x0001D830
	public virtual void CorretRotation()
	{
	}

	// Token: 0x06000C18 RID: 3096 RVA: 0x0001F632 File Offset: 0x0001D832
	public virtual void OnContentPass(Enemy enemy, GameTileContent content = null, int index = 0)
	{
	}

	// Token: 0x06000C19 RID: 3097 RVA: 0x0001F634 File Offset: 0x0001D834
	public virtual void SaveContent(out ContentStruct contentSruct)
	{
		this.m_ContentStruct = new ContentStruct();
		contentSruct = this.m_ContentStruct;
		this.m_ContentStruct.ContentType = (int)this.ContentType;
		this.m_ContentStruct.Pos = new Vector2Int(Mathf.RoundToInt(base.transform.position.x), Mathf.RoundToInt(base.transform.position.y));
		this.m_ContentStruct.Direction = (int)this.m_gameTile.TileDirection;
	}

	// Token: 0x06000C1A RID: 3098 RVA: 0x0001F6B5 File Offset: 0x0001D8B5
	public override void OnUnSpawn()
	{
		base.OnUnSpawn();
		this.IsSwitching = false;
		Singleton<LevelManager>.Instance.GameSaveContents.Remove(this);
	}

	// Token: 0x06000C1B RID: 3099 RVA: 0x0001F6D8 File Offset: 0x0001D8D8
	public virtual void OnSwitch()
	{
		StaticData.SetNodeWalkable(this.m_GameTile, false, true);
		this.IsSwitching = true;
		this.m_GameTile.IsLanded = false;
		Collider2D collider2D = StaticData.RaycastCollider(base.transform.position, LayerMask.GetMask(new string[]
		{
			StaticData.TempGroundMask
		}));
		if (collider2D != null)
		{
			collider2D.GetComponent<GroundTile>().IsLanded = true;
		}
	}

	// Token: 0x06000C1C RID: 3100 RVA: 0x0001F748 File Offset: 0x0001D948
	public virtual void OnUndo()
	{
		if (this.IsSwitching)
		{
			this.UndoSwitching();
			return;
		}
		this.UndoUnSwitching();
	}

	// Token: 0x06000C1D RID: 3101 RVA: 0x0001F760 File Offset: 0x0001D960
	protected virtual void UndoSwitching()
	{
		GameTileContent basicContent = Singleton<StaticData>.Instance.ContentFactory.GetBasicContent(GameTileContentType.Empty);
		this.m_GameTile.Content = basicContent;
		GameTile tileWithContent = ConstructHelper.GetTileWithContent(this);
		tileWithContent.transform.position = (Vector3Int)GameRes.SwitchInfo.InitPos;
		tileWithContent.SetRotation(GameRes.SwitchInfo.InitDir);
		tileWithContent.TileLanded();
		BoardSystem.SelectingTile = this.m_GameTile;
		GameRes.Coin += GameRes.SwitchInfo.SwitchSpend;
		GameRes.SwitchInfo = null;
		Singleton<GameManager>.Instance.TransitionToState(StateName.BuildingState);
	}

	// Token: 0x06000C1E RID: 3102 RVA: 0x0001F7F5 File Offset: 0x0001D9F5
	protected virtual void UndoUnSwitching()
	{
		Singleton<GameManager>.Instance.TransitionToState(StateName.BuildingState);
	}

	// Token: 0x0400060A RID: 1546
	public ContentStruct m_ContentStruct;

	// Token: 0x0400060C RID: 1548
	private GameTile m_gameTile;
}
