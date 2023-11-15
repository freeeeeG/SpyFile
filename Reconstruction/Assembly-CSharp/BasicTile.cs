using System;
using System.Linq;
using UnityEngine;

// Token: 0x020001CE RID: 462
public class BasicTile : GameTile
{
	// Token: 0x1700044C RID: 1100
	// (get) Token: 0x06000BB4 RID: 2996 RVA: 0x0001E9E0 File Offset: 0x0001CBE0
	// (set) Token: 0x06000BB5 RID: 2997 RVA: 0x0001E9E8 File Offset: 0x0001CBE8
	public bool isPath { get; set; }

	// Token: 0x1700044D RID: 1101
	// (get) Token: 0x06000BB6 RID: 2998 RVA: 0x0001E9F1 File Offset: 0x0001CBF1
	// (set) Token: 0x06000BB7 RID: 2999 RVA: 0x0001E9F9 File Offset: 0x0001CBF9
	public bool hasMine { get; set; }

	// Token: 0x06000BB8 RID: 3000 RVA: 0x0001EA02 File Offset: 0x0001CC02
	public override void SetContent(GameTileContent content)
	{
		base.SetContent(content);
		base.Highlight(false);
		this.SetBaseSprite(content.ContentType);
	}

	// Token: 0x06000BB9 RID: 3001 RVA: 0x0001EA1E File Offset: 0x0001CC1E
	public override void OnTileSelected(bool value)
	{
		base.OnTileSelected(value);
		base.Content.OnContentSelected(value);
	}

	// Token: 0x06000BBA RID: 3002 RVA: 0x0001EA34 File Offset: 0x0001CC34
	private void SetBaseSprite(GameTileContentType type)
	{
		SpriteRenderer[] componentsInChildren = base.GetComponentsInChildren<SpriteRenderer>();
		base.TileRenderers = componentsInChildren.Take(2).ToList<SpriteRenderer>();
		switch (type)
		{
		default:
			base.TileRenderers[0].sprite = this.sprites[Random.Range(0, this.sprites.Length)];
			return;
		case GameTileContentType.ElementTurret:
		case GameTileContentType.Trap:
			base.TileRenderers[0].sprite = this.basicTurretBase;
			return;
		case GameTileContentType.RefactorTurret:
			this.EquipTurret(((RefactorTurret)base.Content).Strategy.TurretSkills.Count - 1);
			return;
		}
	}

	// Token: 0x06000BBB RID: 3003 RVA: 0x0001EAD9 File Offset: 0x0001CCD9
	public void EquipTurret(int count)
	{
		base.TileRenderers[0].sprite = this.RefactorBase[count];
	}

	// Token: 0x040005E4 RID: 1508
	[SerializeField]
	private Sprite[] sprites;

	// Token: 0x040005E5 RID: 1509
	[SerializeField]
	private Sprite basicTurretBase;

	// Token: 0x040005E6 RID: 1510
	[SerializeField]
	private Sprite[] RefactorBase;
}
