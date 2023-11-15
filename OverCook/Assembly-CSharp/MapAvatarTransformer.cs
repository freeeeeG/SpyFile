using System;
using UnityEngine;

// Token: 0x020009FE RID: 2558
public class MapAvatarTransformer : MonoBehaviour
{
	// Token: 0x1700037A RID: 890
	// (get) Token: 0x06003208 RID: 12808 RVA: 0x000EACA4 File Offset: 0x000E90A4
	public MapAvatarTransformer.VanType CurrentType
	{
		get
		{
			return this.m_currentType;
		}
	}

	// Token: 0x06003209 RID: 12809 RVA: 0x000EACAC File Offset: 0x000E90AC
	private void Awake()
	{
		this.m_controls = base.gameObject.RequireComponent<MapAvatarControls>();
	}

	// Token: 0x0600320A RID: 12810 RVA: 0x000EACBF File Offset: 0x000E90BF
	private void OnEnable()
	{
		this.ChangeVan(MapAvatarTransformer.VanType.LAND);
	}

	// Token: 0x0600320B RID: 12811 RVA: 0x000EACC8 File Offset: 0x000E90C8
	private void Update()
	{
		GameObject gridOccupant = this.m_controls.GridManager.GetGridOccupant(this.m_controls.GridManager.GetUnclampedGridLocationFromPos(base.transform.position));
		if (gridOccupant != null)
		{
			WorldMapTileFlip worldMapTileFlip = gridOccupant.RequestComponent<WorldMapTileFlip>();
			if (worldMapTileFlip != null && worldMapTileFlip.IsFlipped())
			{
				string name = gridOccupant.name;
				if (name.StartsWith("Rapids") || name.StartsWith("Ramp_Rapids"))
				{
					this.ChangeVan(MapAvatarTransformer.VanType.WATER);
				}
				else if (name.StartsWith("Balloon") || name.StartsWith("Ramp_Balloon"))
				{
					this.ChangeVan(MapAvatarTransformer.VanType.FLYING);
				}
				else if (name.StartsWith("Space") || name.StartsWith("Ramp_Space"))
				{
					this.ChangeVan(MapAvatarTransformer.VanType.FLYING);
				}
				else
				{
					this.ChangeVan(MapAvatarTransformer.VanType.LAND);
				}
			}
			else
			{
				this.ChangeVan(MapAvatarTransformer.VanType.LAND);
			}
		}
	}

	// Token: 0x0600320C RID: 12812 RVA: 0x000EADCC File Offset: 0x000E91CC
	private void ChangeVan(MapAvatarTransformer.VanType type)
	{
		if (this.m_currentType != type)
		{
			Animator animator = base.gameObject.RequireComponentRecursive<Animator>();
			if (animator != null)
			{
				animator.SetInteger(MapAvatarTransformer.m_iVanType, (int)type);
			}
			if (this.m_transformParticle != null)
			{
				GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.m_transformParticle);
				gameObject.transform.SetParent(base.transform, false);
			}
			this.m_currentType = type;
		}
	}

	// Token: 0x0400283A RID: 10298
	[SerializeField]
	private GameObject m_transformParticle;

	// Token: 0x0400283B RID: 10299
	private MapAvatarTransformer.VanType m_currentType;

	// Token: 0x0400283C RID: 10300
	private MapAvatarControls m_controls;

	// Token: 0x0400283D RID: 10301
	private static readonly int m_iVanType = Animator.StringToHash("VanType");

	// Token: 0x020009FF RID: 2559
	public enum VanType
	{
		// Token: 0x0400283F RID: 10303
		LAND,
		// Token: 0x04002840 RID: 10304
		WATER,
		// Token: 0x04002841 RID: 10305
		FLYING
	}
}
