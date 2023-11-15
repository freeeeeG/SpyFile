using System;
using UnityEngine;

// Token: 0x020001F7 RID: 503
public abstract class ElementTurret : TurretContent
{
	// Token: 0x1700047A RID: 1146
	// (get) Token: 0x06000CA0 RID: 3232 RVA: 0x00020D89 File Offset: 0x0001EF89
	public override GameTileContentType ContentType
	{
		get
		{
			return GameTileContentType.ElementTurret;
		}
	}

	// Token: 0x06000CA1 RID: 3233 RVA: 0x00020D8C File Offset: 0x0001EF8C
	public override void OnSwitch()
	{
		base.OnSwitch();
		Singleton<GameManager>.Instance.elementTurrets.Remove(this);
	}

	// Token: 0x06000CA2 RID: 3234 RVA: 0x00020DA4 File Offset: 0x0001EFA4
	public override void ContentLanded()
	{
		Singleton<GameManager>.Instance.elementTurrets.Add(this);
		base.ContentLanded();
		base.m_GameTile.tag = StaticData.UndropablePoint;
		base.IsSwitching = false;
	}

	// Token: 0x06000CA3 RID: 3235 RVA: 0x00020DD4 File Offset: 0x0001EFD4
	public override void SaveContent(out ContentStruct contentStruct)
	{
		base.SaveContent(out contentStruct);
		contentStruct = this.m_ContentStruct;
		this.m_ContentStruct.Element = (int)this.Strategy.Attribute.element;
		this.m_ContentStruct.Quality = this.Strategy.Quality;
	}

	// Token: 0x06000CA4 RID: 3236 RVA: 0x00020E24 File Offset: 0x0001F024
	public override void OnUnSpawn()
	{
		base.OnUnSpawn();
		Collider2D collider2D = StaticData.RaycastCollider(base.transform.position, LayerMask.GetMask(new string[]
		{
			StaticData.TempGroundMask
		}));
		if (collider2D != null)
		{
			collider2D.GetComponent<GroundTile>().IsLanded = true;
		}
		Singleton<GameManager>.Instance.elementTurrets.Remove(this);
	}
}
