using System;
using UnityEngine;

// Token: 0x02000BF9 RID: 3065
[ExecutionDependency(typeof(WorldMapFlipperBase))]
[RequireComponent(typeof(WorldMapTileFlip))]
public class WorldMapTileOptimizer : MonoBehaviour, ITileFlipAnimatorProvider, ITileFlipStaticHandler, ITileFlipStartup
{
	// Token: 0x17000434 RID: 1076
	// (get) Token: 0x06003E92 RID: 16018 RVA: 0x0012B63A File Offset: 0x00129A3A
	// (set) Token: 0x06003E93 RID: 16019 RVA: 0x0012B642 File Offset: 0x00129A42
	public TileCosmetics Cosmetics
	{
		get
		{
			return this.m_cosmetics;
		}
		set
		{
			this.m_cosmetics = value;
		}
	}

	// Token: 0x17000435 RID: 1077
	// (get) Token: 0x06003E94 RID: 16020 RVA: 0x0012B64B File Offset: 0x00129A4B
	public GameObject Tile
	{
		get
		{
			return this.m_tile;
		}
	}

	// Token: 0x06003E95 RID: 16021 RVA: 0x0012B653 File Offset: 0x00129A53
	protected virtual void Awake()
	{
		this.UpdateMeshRenderer();
		WorldMapMaterialPool.RegisterMaterial(this.m_OriginalMaterial);
	}

	// Token: 0x06003E96 RID: 16022 RVA: 0x0012B668 File Offset: 0x00129A68
	public Animator Begin(FlipDirection _direction)
	{
		this.m_tile.transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
		this.m_animator = this.m_tile.AddComponent<Animator>();
		this.m_animator.runtimeAnimatorController = this.m_controller;
		this.m_animatorComs = this.m_tile.AddComponent<AnimatorCommunications>();
		return this.m_animator;
	}

	// Token: 0x06003E97 RID: 16023 RVA: 0x0012B6D4 File Offset: 0x00129AD4
	public void End(FlipDirection _direction)
	{
		if (this.m_animatorComs != null)
		{
			UnityEngine.Object.Destroy(this.m_animatorComs);
			this.m_animatorComs = null;
		}
		if (this.m_animator != null)
		{
			UnityEngine.Object.Destroy(this.m_animator);
			this.m_animator = null;
		}
		this.UpdateMesh();
		this.m_complete = true;
	}

	// Token: 0x06003E98 RID: 16024 RVA: 0x0012B734 File Offset: 0x00129B34
	public bool IsComplete()
	{
		return this.m_complete;
	}

	// Token: 0x06003E99 RID: 16025 RVA: 0x0012B73C File Offset: 0x00129B3C
	public void StartUp()
	{
		if (this.m_debugBreak)
		{
		}
		if (!this.m_complete)
		{
			this.m_tile.transform.localRotation = Quaternion.Euler(180f, 0f, 0f);
			if (this.m_MeshRenderer != null)
			{
				this.m_MeshRenderer.material.SetFloat("_Blend", 0f);
			}
		}
		if (this.m_startStatic)
		{
		}
	}

	// Token: 0x06003E9A RID: 16026 RVA: 0x0012B7BC File Offset: 0x00129BBC
	public void SetAsStatic()
	{
		if (!this.m_complete)
		{
			this.End(FlipDirection.Unfold);
		}
		else
		{
			this.m_tile.transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
			this.UpdateMesh();
		}
	}

	// Token: 0x06003E9B RID: 16027 RVA: 0x0012B80C File Offset: 0x00129C0C
	protected void UpdateMesh()
	{
		if (this.m_MeshRenderer == null)
		{
			this.UpdateMeshRenderer();
		}
		MeshFilter meshFilter = this.m_MeshRenderer.gameObject.RequireComponent<MeshFilter>();
		WorldMapTileFlip worldMapTileFlip = base.gameObject.RequestComponent<WorldMapTileFlip>();
		meshFilter.sharedMesh = ((!worldMapTileFlip.IsFlipped()) ? this.m_unflippedMesh : this.m_flippedMesh);
		if (worldMapTileFlip.IsFlipped())
		{
			this.m_MeshRenderer.sharedMaterial = WorldMapMaterialPool.GetSharedMaterialForState(this.m_OriginalMaterial, WorldMapMaterialPool.MapState.UnFolded);
		}
		else
		{
			this.m_MeshRenderer.sharedMaterial = WorldMapMaterialPool.GetSharedMaterialForState(this.m_OriginalMaterial, WorldMapMaterialPool.MapState.Folded);
		}
	}

	// Token: 0x06003E9C RID: 16028 RVA: 0x0012B8B0 File Offset: 0x00129CB0
	private void UpdateMeshRenderer()
	{
		if (this.m_MeshRenderer == null)
		{
			this.m_MeshRenderer = base.gameObject.RequestComponentInImmediateChildren<MeshRenderer>();
			if (this.m_MeshRenderer == null)
			{
				this.m_MeshRenderer = this.m_tile.RequestComponentInImmediateChildren<MeshRenderer>();
			}
			this.m_OriginalMaterial = this.m_MeshRenderer.material;
		}
	}

	// Token: 0x0400323F RID: 12863
	[SerializeField]
	private Mesh m_flippedMesh;

	// Token: 0x04003240 RID: 12864
	[SerializeField]
	private Mesh m_unflippedMesh;

	// Token: 0x04003241 RID: 12865
	[HideInInspector]
	[SerializeField]
	private TileCosmetics m_cosmetics;

	// Token: 0x04003242 RID: 12866
	[SerializeField]
	private bool m_startStatic;

	// Token: 0x04003243 RID: 12867
	[SerializeField]
	[AssignChild("Tile", Editorbility.NonEditable)]
	private GameObject m_tile;

	// Token: 0x04003244 RID: 12868
	[SerializeField]
	private RuntimeAnimatorController m_controller;

	// Token: 0x04003245 RID: 12869
	[SerializeField]
	private bool m_debugBreak;

	// Token: 0x04003246 RID: 12870
	private MeshRenderer m_MeshRenderer;

	// Token: 0x04003247 RID: 12871
	private Material m_OriginalMaterial;

	// Token: 0x04003248 RID: 12872
	private StaticGridLocation m_staticGridLocation;

	// Token: 0x04003249 RID: 12873
	private Animator m_animator;

	// Token: 0x0400324A RID: 12874
	private AnimatorCommunications m_animatorComs;

	// Token: 0x0400324B RID: 12875
	private bool m_complete;
}
