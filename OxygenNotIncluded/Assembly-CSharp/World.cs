using System;
using System.Collections.Generic;
using Klei;
using Rendering;
using UnityEngine;

// Token: 0x02000A31 RID: 2609
[AddComponentMenu("KMonoBehaviour/scripts/World")]
public class World : KMonoBehaviour
{
	// Token: 0x170005BC RID: 1468
	// (get) Token: 0x06004E35 RID: 20021 RVA: 0x001B6058 File Offset: 0x001B4258
	// (set) Token: 0x06004E36 RID: 20022 RVA: 0x001B605F File Offset: 0x001B425F
	public static World Instance { get; private set; }

	// Token: 0x170005BD RID: 1469
	// (get) Token: 0x06004E37 RID: 20023 RVA: 0x001B6067 File Offset: 0x001B4267
	// (set) Token: 0x06004E38 RID: 20024 RVA: 0x001B606F File Offset: 0x001B426F
	public SubworldZoneRenderData zoneRenderData { get; private set; }

	// Token: 0x06004E39 RID: 20025 RVA: 0x001B6078 File Offset: 0x001B4278
	protected override void OnPrefabInit()
	{
		global::Debug.Assert(World.Instance == null);
		World.Instance = this;
		this.blockTileRenderer = base.GetComponent<BlockTileRenderer>();
	}

	// Token: 0x06004E3A RID: 20026 RVA: 0x001B609C File Offset: 0x001B429C
	protected override void OnSpawn()
	{
		base.GetComponent<SimDebugView>().OnReset();
		base.GetComponent<PropertyTextures>().OnReset(null);
		this.zoneRenderData = base.GetComponent<SubworldZoneRenderData>();
		Grid.OnReveal = (Action<int>)Delegate.Combine(Grid.OnReveal, new Action<int>(this.OnReveal));
	}

	// Token: 0x06004E3B RID: 20027 RVA: 0x001B60EC File Offset: 0x001B42EC
	protected override void OnLoadLevel()
	{
		World.Instance = null;
		if (this.blockTileRenderer != null)
		{
			this.blockTileRenderer.FreeResources();
		}
		this.blockTileRenderer = null;
		if (this.groundRenderer != null)
		{
			this.groundRenderer.FreeResources();
		}
		this.groundRenderer = null;
		this.revealedCells.Clear();
		this.revealedCells = null;
		base.OnLoadLevel();
	}

	// Token: 0x06004E3C RID: 20028 RVA: 0x001B6158 File Offset: 0x001B4358
	public unsafe void UpdateCellInfo(List<SolidInfo> solidInfo, List<CallbackInfo> callbackInfo, int num_solid_substance_change_info, Sim.SolidSubstanceChangeInfo* solid_substance_change_info, int num_liquid_change_info, Sim.LiquidChangeInfo* liquid_change_info)
	{
		int count = solidInfo.Count;
		this.changedCells.Clear();
		for (int i = 0; i < count; i++)
		{
			int cellIdx = solidInfo[i].cellIdx;
			if (!this.changedCells.Contains(cellIdx))
			{
				this.changedCells.Add(cellIdx);
			}
			Pathfinding.Instance.AddDirtyNavGridCell(cellIdx);
			WorldDamage.Instance.OnSolidStateChanged(cellIdx);
			if (this.OnSolidChanged != null)
			{
				this.OnSolidChanged(cellIdx);
			}
		}
		if (this.changedCells.Count != 0)
		{
			SaveGame.Instance.entombedItemManager.OnSolidChanged(this.changedCells);
			GameScenePartitioner.Instance.TriggerEvent(this.changedCells, GameScenePartitioner.Instance.solidChangedLayer, null);
		}
		int count2 = callbackInfo.Count;
		for (int j = 0; j < count2; j++)
		{
			callbackInfo[j].Release();
		}
		for (int k = 0; k < num_solid_substance_change_info; k++)
		{
			int cellIdx2 = solid_substance_change_info[k].cellIdx;
			if (!Grid.IsValidCell(cellIdx2))
			{
				global::Debug.LogError(cellIdx2);
			}
			else
			{
				Grid.RenderedByWorld[cellIdx2] = (Grid.Element[cellIdx2].substance.renderedByWorld && Grid.Objects[cellIdx2, 9] == null);
				this.groundRenderer.MarkDirty(cellIdx2);
			}
		}
		GameScenePartitioner instance = GameScenePartitioner.Instance;
		this.changedCells.Clear();
		for (int l = 0; l < num_liquid_change_info; l++)
		{
			int cellIdx3 = liquid_change_info[l].cellIdx;
			this.changedCells.Add(cellIdx3);
			if (this.OnLiquidChanged != null)
			{
				this.OnLiquidChanged(cellIdx3);
			}
		}
		instance.TriggerEvent(this.changedCells, GameScenePartitioner.Instance.liquidChangedLayer, null);
	}

	// Token: 0x06004E3D RID: 20029 RVA: 0x001B632D File Offset: 0x001B452D
	private void OnReveal(int cell)
	{
		this.revealedCells.Add(cell);
	}

	// Token: 0x06004E3E RID: 20030 RVA: 0x001B633C File Offset: 0x001B453C
	private void LateUpdate()
	{
		if (Game.IsQuitting())
		{
			return;
		}
		if (GameUtil.IsCapturingTimeLapse())
		{
			this.groundRenderer.RenderAll();
			KAnimBatchManager.Instance().UpdateActiveArea(new Vector2I(0, 0), new Vector2I(9999, 9999));
			KAnimBatchManager.Instance().UpdateDirty(Time.frameCount);
			KAnimBatchManager.Instance().Render();
		}
		else
		{
			GridArea visibleArea = GridVisibleArea.GetVisibleArea();
			this.groundRenderer.Render(visibleArea.Min, visibleArea.Max, false);
			Vector2I vis_chunk_min;
			Vector2I vis_chunk_max;
			Singleton<KBatchedAnimUpdater>.Instance.GetVisibleArea(out vis_chunk_min, out vis_chunk_max);
			KAnimBatchManager.Instance().UpdateActiveArea(vis_chunk_min, vis_chunk_max);
			KAnimBatchManager.Instance().UpdateDirty(Time.frameCount);
			KAnimBatchManager.Instance().Render();
		}
		if (Camera.main != null)
		{
			Vector3 vector = Camera.main.ScreenToWorldPoint(new Vector3(KInputManager.GetMousePos().x, KInputManager.GetMousePos().y, -Camera.main.transform.GetPosition().z));
			Shader.SetGlobalVector("_CursorPos", new Vector4(vector.x, vector.y, vector.z, 0f));
		}
		FallingWater.instance.UpdateParticles(Time.deltaTime);
		FallingWater.instance.Render();
		if (this.revealedCells.Count > 0)
		{
			GameScenePartitioner.Instance.TriggerEvent(this.revealedCells, GameScenePartitioner.Instance.fogOfWarChangedLayer, null);
			this.revealedCells.Clear();
		}
	}

	// Token: 0x040032F2 RID: 13042
	public Action<int> OnSolidChanged;

	// Token: 0x040032F3 RID: 13043
	public Action<int> OnLiquidChanged;

	// Token: 0x040032F5 RID: 13045
	public BlockTileRenderer blockTileRenderer;

	// Token: 0x040032F6 RID: 13046
	[MyCmpGet]
	[NonSerialized]
	public GroundRenderer groundRenderer;

	// Token: 0x040032F7 RID: 13047
	private List<int> revealedCells = new List<int>();

	// Token: 0x040032F8 RID: 13048
	public static int DebugCellID = -1;

	// Token: 0x040032F9 RID: 13049
	private List<int> changedCells = new List<int>();
}
