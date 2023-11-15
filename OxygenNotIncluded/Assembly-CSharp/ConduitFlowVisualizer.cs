using System;
using System.Collections.Generic;
using FMOD.Studio;
using FMODUnity;
using UnityEngine;

// Token: 0x02000A5A RID: 2650
public class ConduitFlowVisualizer
{
	// Token: 0x06005029 RID: 20521 RVA: 0x001C66DC File Offset: 0x001C48DC
	public ConduitFlowVisualizer(ConduitFlow flow_manager, Game.ConduitVisInfo vis_info, EventReference overlay_sound, ConduitFlowVisualizer.Tuning tuning)
	{
		this.flowManager = flow_manager;
		this.visInfo = vis_info;
		this.overlaySound = overlay_sound;
		this.tuning = tuning;
		this.movingBallMesh = new ConduitFlowVisualizer.ConduitFlowMesh();
		this.staticBallMesh = new ConduitFlowVisualizer.ConduitFlowMesh();
		ConduitFlowVisualizer.RenderMeshTask.Ball.InitializeResources();
	}

	// Token: 0x0600502A RID: 20522 RVA: 0x001C6768 File Offset: 0x001C4968
	public void FreeResources()
	{
		this.movingBallMesh.Cleanup();
		this.staticBallMesh.Cleanup();
	}

	// Token: 0x0600502B RID: 20523 RVA: 0x001C6780 File Offset: 0x001C4980
	private float CalculateMassScale(float mass)
	{
		float t = (mass - this.visInfo.overlayMassScaleRange.x) / (this.visInfo.overlayMassScaleRange.y - this.visInfo.overlayMassScaleRange.x);
		return Mathf.Lerp(this.visInfo.overlayMassScaleValues.x, this.visInfo.overlayMassScaleValues.y, t);
	}

	// Token: 0x0600502C RID: 20524 RVA: 0x001C67E8 File Offset: 0x001C49E8
	private Color32 GetContentsColor(Element element, Color32 default_color)
	{
		if (element != null)
		{
			Color c = element.substance.conduitColour;
			c.a = 128f;
			return c;
		}
		return default_color;
	}

	// Token: 0x0600502D RID: 20525 RVA: 0x001C681D File Offset: 0x001C4A1D
	private Color32 GetTintColour()
	{
		if (!this.showContents)
		{
			return this.visInfo.tint;
		}
		return GlobalAssets.Instance.colorSet.GetColorByName(this.visInfo.overlayTintName);
	}

	// Token: 0x0600502E RID: 20526 RVA: 0x001C684D File Offset: 0x001C4A4D
	private Color32 GetInsulatedTintColour()
	{
		if (!this.showContents)
		{
			return this.visInfo.insulatedTint;
		}
		return GlobalAssets.Instance.colorSet.GetColorByName(this.visInfo.overlayInsulatedTintName);
	}

	// Token: 0x0600502F RID: 20527 RVA: 0x001C687D File Offset: 0x001C4A7D
	private Color32 GetRadiantTintColour()
	{
		if (!this.showContents)
		{
			return this.visInfo.radiantTint;
		}
		return GlobalAssets.Instance.colorSet.GetColorByName(this.visInfo.overlayRadiantTintName);
	}

	// Token: 0x06005030 RID: 20528 RVA: 0x001C68B0 File Offset: 0x001C4AB0
	private Color32 GetCellTintColour(int cell)
	{
		Color32 result;
		if (this.insulatedCells.Contains(cell))
		{
			result = this.GetInsulatedTintColour();
		}
		else if (this.radiantCells.Contains(cell))
		{
			result = this.GetRadiantTintColour();
		}
		else
		{
			result = this.GetTintColour();
		}
		return result;
	}

	// Token: 0x06005031 RID: 20529 RVA: 0x001C68F4 File Offset: 0x001C4AF4
	public void Render(float z, int render_layer, float lerp_percent, bool trigger_audio = false)
	{
		this.animTime += (double)Time.deltaTime;
		if (trigger_audio)
		{
			if (this.audioInfo == null)
			{
				this.audioInfo = new List<ConduitFlowVisualizer.AudioInfo>();
			}
			for (int i = 0; i < this.audioInfo.Count; i++)
			{
				ConduitFlowVisualizer.AudioInfo audioInfo = this.audioInfo[i];
				audioInfo.distance = float.PositiveInfinity;
				audioInfo.position = Vector3.zero;
				audioInfo.blobCount = (audioInfo.blobCount + 1) % 10;
				this.audioInfo[i] = audioInfo;
			}
		}
		if (this.tuning.renderMesh)
		{
			this.RenderMesh(z, render_layer, lerp_percent, trigger_audio);
		}
		if (trigger_audio)
		{
			this.TriggerAudio();
		}
	}

	// Token: 0x06005032 RID: 20530 RVA: 0x001C69A8 File Offset: 0x001C4BA8
	private void RenderMesh(float z, int render_layer, float lerp_percent, bool trigger_audio)
	{
		GridArea visibleArea = GridVisibleArea.GetVisibleArea();
		Vector2I min = new Vector2I(Mathf.Max(0, visibleArea.Min.x - 1), Mathf.Max(0, visibleArea.Min.y - 1));
		Vector2I max = new Vector2I(Mathf.Min(Grid.WidthInCells - 1, visibleArea.Max.x + 1), Mathf.Min(Grid.HeightInCells - 1, visibleArea.Max.y + 1));
		ConduitFlowVisualizer.RenderMeshContext renderMeshContext = new ConduitFlowVisualizer.RenderMeshContext(this, lerp_percent, min, max);
		if (renderMeshContext.visible_conduits.Count == 0)
		{
			renderMeshContext.Finish();
			return;
		}
		ConduitFlowVisualizer.render_mesh_job.Reset(renderMeshContext);
		int num = Mathf.Max(1, (int)((float)(renderMeshContext.visible_conduits.Count / CPUBudget.coreCount) / 1.5f));
		int num2 = Mathf.Max(1, renderMeshContext.visible_conduits.Count / num);
		for (int num3 = 0; num3 != num2; num3++)
		{
			int num4 = num3 * num;
			int end = (num3 == num2 - 1) ? renderMeshContext.visible_conduits.Count : (num4 + num);
			ConduitFlowVisualizer.render_mesh_job.Add(new ConduitFlowVisualizer.RenderMeshTask(num4, end));
		}
		GlobalJobManager.Run(ConduitFlowVisualizer.render_mesh_job);
		float z2 = 0f;
		if (this.showContents)
		{
			z2 = 1f;
		}
		float w = (float)((int)(this.animTime / (1.0 / (double)this.tuning.framesPerSecond)) % (int)this.tuning.spriteCount) * (1f / this.tuning.spriteCount);
		this.movingBallMesh.Begin();
		this.movingBallMesh.SetTexture("_BackgroundTex", this.tuning.backgroundTexture);
		this.movingBallMesh.SetTexture("_ForegroundTex", this.tuning.foregroundTexture);
		this.movingBallMesh.SetVector("_SpriteSettings", new Vector4(1f / this.tuning.spriteCount, 1f, z2, w));
		this.movingBallMesh.SetVector("_Highlight", new Vector4((float)this.highlightColour.r / 255f, (float)this.highlightColour.g / 255f, (float)this.highlightColour.b / 255f, 0f));
		this.staticBallMesh.Begin();
		this.staticBallMesh.SetTexture("_BackgroundTex", this.tuning.backgroundTexture);
		this.staticBallMesh.SetTexture("_ForegroundTex", this.tuning.foregroundTexture);
		this.staticBallMesh.SetVector("_SpriteSettings", new Vector4(1f / this.tuning.spriteCount, 1f, z2, 0f));
		this.staticBallMesh.SetVector("_Highlight", new Vector4((float)this.highlightColour.r / 255f, (float)this.highlightColour.g / 255f, (float)this.highlightColour.b / 255f, 0f));
		Vector3 position = CameraController.Instance.transform.GetPosition();
		ConduitFlowVisualizer visualizer = trigger_audio ? this : null;
		for (int num5 = 0; num5 != ConduitFlowVisualizer.render_mesh_job.Count; num5++)
		{
			ConduitFlowVisualizer.render_mesh_job.GetWorkItem(num5).Finish(this.movingBallMesh, this.staticBallMesh, position, visualizer);
		}
		this.movingBallMesh.End(z, this.layer);
		this.staticBallMesh.End(z, this.layer);
		renderMeshContext.Finish();
		ConduitFlowVisualizer.render_mesh_job.Reset(null);
	}

	// Token: 0x06005033 RID: 20531 RVA: 0x001C6D3D File Offset: 0x001C4F3D
	public void ColourizePipeContents(bool show_contents, bool move_to_overlay_layer)
	{
		this.showContents = show_contents;
		this.layer = ((show_contents && move_to_overlay_layer) ? LayerMask.NameToLayer("MaskedOverlay") : 0);
	}

	// Token: 0x06005034 RID: 20532 RVA: 0x001C6D60 File Offset: 0x001C4F60
	private void AddAudioSource(ConduitFlow.Conduit conduit, Vector3 camera_pos)
	{
		using (new KProfiler.Region("AddAudioSource", null))
		{
			UtilityNetwork network = this.flowManager.GetNetwork(conduit);
			if (network != null)
			{
				Vector3 vector = Grid.CellToPosCCC(conduit.GetCell(this.flowManager), Grid.SceneLayer.Building);
				float num = Vector3.SqrMagnitude(vector - camera_pos);
				bool flag = false;
				for (int i = 0; i < this.audioInfo.Count; i++)
				{
					ConduitFlowVisualizer.AudioInfo audioInfo = this.audioInfo[i];
					if (audioInfo.networkID == network.id)
					{
						if (num < audioInfo.distance)
						{
							audioInfo.distance = num;
							audioInfo.position = vector;
							this.audioInfo[i] = audioInfo;
						}
						flag = true;
						break;
					}
				}
				if (!flag)
				{
					ConduitFlowVisualizer.AudioInfo item = default(ConduitFlowVisualizer.AudioInfo);
					item.networkID = network.id;
					item.position = vector;
					item.distance = num;
					item.blobCount = 0;
					this.audioInfo.Add(item);
				}
			}
		}
	}

	// Token: 0x06005035 RID: 20533 RVA: 0x001C6E78 File Offset: 0x001C5078
	private void TriggerAudio()
	{
		if (SpeedControlScreen.Instance.IsPaused)
		{
			return;
		}
		CameraController instance = CameraController.Instance;
		int num = 0;
		List<ConduitFlowVisualizer.AudioInfo> list = new List<ConduitFlowVisualizer.AudioInfo>();
		for (int i = 0; i < this.audioInfo.Count; i++)
		{
			if (instance.IsVisiblePos(this.audioInfo[i].position))
			{
				list.Add(this.audioInfo[i]);
				num++;
			}
		}
		for (int j = 0; j < list.Count; j++)
		{
			ConduitFlowVisualizer.AudioInfo audioInfo = list[j];
			if (audioInfo.distance != float.PositiveInfinity)
			{
				Vector3 position = audioInfo.position;
				position.z = 0f;
				EventInstance instance2 = SoundEvent.BeginOneShot(this.overlaySound, position, 1f, false);
				instance2.setParameterByName("blobCount", (float)audioInfo.blobCount, false);
				instance2.setParameterByName("networkCount", (float)num, false);
				SoundEvent.EndOneShot(instance2);
			}
		}
	}

	// Token: 0x06005036 RID: 20534 RVA: 0x001C6F6A File Offset: 0x001C516A
	public void AddThermalConductivity(int cell, float conductivity)
	{
		if (conductivity < 1f)
		{
			this.insulatedCells.Add(cell);
			return;
		}
		if (conductivity > 1f)
		{
			this.radiantCells.Add(cell);
		}
	}

	// Token: 0x06005037 RID: 20535 RVA: 0x001C6F97 File Offset: 0x001C5197
	public void RemoveThermalConductivity(int cell, float conductivity)
	{
		if (conductivity < 1f)
		{
			this.insulatedCells.Remove(cell);
			return;
		}
		if (conductivity > 1f)
		{
			this.radiantCells.Remove(cell);
		}
	}

	// Token: 0x06005038 RID: 20536 RVA: 0x001C6FC4 File Offset: 0x001C51C4
	public void SetHighlightedCell(int cell)
	{
		this.highlightedCell = cell;
	}

	// Token: 0x04003476 RID: 13430
	private ConduitFlow flowManager;

	// Token: 0x04003477 RID: 13431
	private EventReference overlaySound;

	// Token: 0x04003478 RID: 13432
	private bool showContents;

	// Token: 0x04003479 RID: 13433
	private double animTime;

	// Token: 0x0400347A RID: 13434
	private int layer;

	// Token: 0x0400347B RID: 13435
	private static Vector2 GRID_OFFSET = new Vector2(0.5f, 0.5f);

	// Token: 0x0400347C RID: 13436
	private List<ConduitFlowVisualizer.AudioInfo> audioInfo;

	// Token: 0x0400347D RID: 13437
	private HashSet<int> insulatedCells = new HashSet<int>();

	// Token: 0x0400347E RID: 13438
	private HashSet<int> radiantCells = new HashSet<int>();

	// Token: 0x0400347F RID: 13439
	private Game.ConduitVisInfo visInfo;

	// Token: 0x04003480 RID: 13440
	private ConduitFlowVisualizer.ConduitFlowMesh movingBallMesh;

	// Token: 0x04003481 RID: 13441
	private ConduitFlowVisualizer.ConduitFlowMesh staticBallMesh;

	// Token: 0x04003482 RID: 13442
	private int highlightedCell = -1;

	// Token: 0x04003483 RID: 13443
	private Color32 highlightColour = new Color(0.2f, 0.2f, 0.2f, 0.2f);

	// Token: 0x04003484 RID: 13444
	private ConduitFlowVisualizer.Tuning tuning;

	// Token: 0x04003485 RID: 13445
	private static WorkItemCollection<ConduitFlowVisualizer.RenderMeshTask, ConduitFlowVisualizer.RenderMeshContext> render_mesh_job = new WorkItemCollection<ConduitFlowVisualizer.RenderMeshTask, ConduitFlowVisualizer.RenderMeshContext>();

	// Token: 0x020018F0 RID: 6384
	[Serializable]
	public class Tuning
	{
		// Token: 0x0400736D RID: 29549
		public bool renderMesh;

		// Token: 0x0400736E RID: 29550
		public float size;

		// Token: 0x0400736F RID: 29551
		public float spriteCount;

		// Token: 0x04007370 RID: 29552
		public float framesPerSecond;

		// Token: 0x04007371 RID: 29553
		public Texture2D backgroundTexture;

		// Token: 0x04007372 RID: 29554
		public Texture2D foregroundTexture;
	}

	// Token: 0x020018F1 RID: 6385
	private class ConduitFlowMesh
	{
		// Token: 0x0600934E RID: 37710 RVA: 0x0032E874 File Offset: 0x0032CA74
		public ConduitFlowMesh()
		{
			this.mesh = new Mesh();
			this.mesh.name = "ConduitMesh";
			this.material = new Material(Shader.Find("Klei/ConduitBall"));
		}

		// Token: 0x0600934F RID: 37711 RVA: 0x0032E8E4 File Offset: 0x0032CAE4
		public void AddQuad(Vector2 pos, Color32 color, float size, float is_foreground, float highlight, Vector2I uvbl, Vector2I uvtl, Vector2I uvbr, Vector2I uvtr)
		{
			float num = size * 0.5f;
			this.positions.Add(new Vector3(pos.x - num, pos.y - num, 0f));
			this.positions.Add(new Vector3(pos.x - num, pos.y + num, 0f));
			this.positions.Add(new Vector3(pos.x + num, pos.y - num, 0f));
			this.positions.Add(new Vector3(pos.x + num, pos.y + num, 0f));
			this.uvs.Add(new Vector4((float)uvbl.x, (float)uvbl.y, is_foreground, highlight));
			this.uvs.Add(new Vector4((float)uvtl.x, (float)uvtl.y, is_foreground, highlight));
			this.uvs.Add(new Vector4((float)uvbr.x, (float)uvbr.y, is_foreground, highlight));
			this.uvs.Add(new Vector4((float)uvtr.x, (float)uvtr.y, is_foreground, highlight));
			this.colors.Add(color);
			this.colors.Add(color);
			this.colors.Add(color);
			this.colors.Add(color);
			this.triangles.Add(this.quadIndex * 4);
			this.triangles.Add(this.quadIndex * 4 + 1);
			this.triangles.Add(this.quadIndex * 4 + 2);
			this.triangles.Add(this.quadIndex * 4 + 2);
			this.triangles.Add(this.quadIndex * 4 + 1);
			this.triangles.Add(this.quadIndex * 4 + 3);
			this.quadIndex++;
		}

		// Token: 0x06009350 RID: 37712 RVA: 0x0032EAD7 File Offset: 0x0032CCD7
		public void SetTexture(string id, Texture2D texture)
		{
			this.material.SetTexture(id, texture);
		}

		// Token: 0x06009351 RID: 37713 RVA: 0x0032EAE6 File Offset: 0x0032CCE6
		public void SetVector(string id, Vector4 data)
		{
			this.material.SetVector(id, data);
		}

		// Token: 0x06009352 RID: 37714 RVA: 0x0032EAF5 File Offset: 0x0032CCF5
		public void Begin()
		{
			this.positions.Clear();
			this.uvs.Clear();
			this.triangles.Clear();
			this.colors.Clear();
			this.quadIndex = 0;
		}

		// Token: 0x06009353 RID: 37715 RVA: 0x0032EB2C File Offset: 0x0032CD2C
		public void End(float z, int layer)
		{
			this.mesh.Clear();
			this.mesh.SetVertices(this.positions);
			this.mesh.SetUVs(0, this.uvs);
			this.mesh.SetColors(this.colors);
			this.mesh.SetTriangles(this.triangles, 0, false);
			Graphics.DrawMesh(this.mesh, new Vector3(ConduitFlowVisualizer.GRID_OFFSET.x, ConduitFlowVisualizer.GRID_OFFSET.y, z - 0.1f), Quaternion.identity, this.material, layer);
		}

		// Token: 0x06009354 RID: 37716 RVA: 0x0032EBC2 File Offset: 0x0032CDC2
		public void Cleanup()
		{
			UnityEngine.Object.Destroy(this.mesh);
			this.mesh = null;
			UnityEngine.Object.Destroy(this.material);
			this.material = null;
		}

		// Token: 0x04007373 RID: 29555
		private Mesh mesh;

		// Token: 0x04007374 RID: 29556
		private Material material;

		// Token: 0x04007375 RID: 29557
		private List<Vector3> positions = new List<Vector3>();

		// Token: 0x04007376 RID: 29558
		private List<Vector4> uvs = new List<Vector4>();

		// Token: 0x04007377 RID: 29559
		private List<int> triangles = new List<int>();

		// Token: 0x04007378 RID: 29560
		private List<Color32> colors = new List<Color32>();

		// Token: 0x04007379 RID: 29561
		private int quadIndex;
	}

	// Token: 0x020018F2 RID: 6386
	private struct AudioInfo
	{
		// Token: 0x0400737A RID: 29562
		public int networkID;

		// Token: 0x0400737B RID: 29563
		public int blobCount;

		// Token: 0x0400737C RID: 29564
		public float distance;

		// Token: 0x0400737D RID: 29565
		public Vector3 position;
	}

	// Token: 0x020018F3 RID: 6387
	private class RenderMeshContext
	{
		// Token: 0x06009355 RID: 37717 RVA: 0x0032EBE8 File Offset: 0x0032CDE8
		public RenderMeshContext(ConduitFlowVisualizer outer, float lerp_percent, Vector2I min, Vector2I max)
		{
			this.outer = outer;
			this.lerp_percent = lerp_percent;
			this.visible_conduits = ListPool<int, ConduitFlowVisualizer>.Allocate();
			this.visible_conduits.Capacity = Math.Max(outer.flowManager.soaInfo.NumEntries, this.visible_conduits.Capacity);
			for (int num = 0; num != outer.flowManager.soaInfo.NumEntries; num++)
			{
				Vector2I vector2I = Grid.CellToXY(outer.flowManager.soaInfo.GetCell(num));
				if (min <= vector2I && vector2I <= max)
				{
					this.visible_conduits.Add(num);
				}
			}
		}

		// Token: 0x06009356 RID: 37718 RVA: 0x0032EC90 File Offset: 0x0032CE90
		public void Finish()
		{
			this.visible_conduits.Recycle();
		}

		// Token: 0x0400737E RID: 29566
		public ListPool<int, ConduitFlowVisualizer>.PooledList visible_conduits;

		// Token: 0x0400737F RID: 29567
		public ConduitFlowVisualizer outer;

		// Token: 0x04007380 RID: 29568
		public float lerp_percent;
	}

	// Token: 0x020018F4 RID: 6388
	private struct RenderMeshTask : IWorkItem<ConduitFlowVisualizer.RenderMeshContext>
	{
		// Token: 0x06009357 RID: 37719 RVA: 0x0032ECA0 File Offset: 0x0032CEA0
		public RenderMeshTask(int start, int end)
		{
			this.start = start;
			this.end = end;
			int capacity = end - start;
			this.moving_balls = ListPool<ConduitFlowVisualizer.RenderMeshTask.Ball, ConduitFlowVisualizer.RenderMeshTask>.Allocate();
			this.moving_balls.Capacity = capacity;
			this.static_balls = ListPool<ConduitFlowVisualizer.RenderMeshTask.Ball, ConduitFlowVisualizer.RenderMeshTask>.Allocate();
			this.static_balls.Capacity = capacity;
			this.moving_conduits = ListPool<ConduitFlow.Conduit, ConduitFlowVisualizer.RenderMeshTask>.Allocate();
			this.moving_conduits.Capacity = capacity;
		}

		// Token: 0x06009358 RID: 37720 RVA: 0x0032ED04 File Offset: 0x0032CF04
		public void Run(ConduitFlowVisualizer.RenderMeshContext context)
		{
			Element element = null;
			for (int num = this.start; num != this.end; num++)
			{
				ConduitFlow.Conduit conduit = context.outer.flowManager.soaInfo.GetConduit(context.visible_conduits[num]);
				ConduitFlow.ConduitFlowInfo lastFlowInfo = conduit.GetLastFlowInfo(context.outer.flowManager);
				ConduitFlow.ConduitContents initialContents = conduit.GetInitialContents(context.outer.flowManager);
				if (lastFlowInfo.contents.mass > 0f)
				{
					int cell = conduit.GetCell(context.outer.flowManager);
					int cellFromDirection = ConduitFlow.GetCellFromDirection(cell, lastFlowInfo.direction);
					Vector2I vector2I = Grid.CellToXY(cell);
					Vector2I vector2I2 = Grid.CellToXY(cellFromDirection);
					Vector2 vector = (cell == -1) ? vector2I : Vector2.Lerp(new Vector2((float)vector2I.x, (float)vector2I.y), new Vector2((float)vector2I2.x, (float)vector2I2.y), context.lerp_percent);
					Color32 cellTintColour = context.outer.GetCellTintColour(cell);
					Color32 cellTintColour2 = context.outer.GetCellTintColour(cellFromDirection);
					Color32 color = Color32.Lerp(cellTintColour, cellTintColour2, context.lerp_percent);
					bool highlight = false;
					if (context.outer.showContents)
					{
						if (lastFlowInfo.contents.mass >= initialContents.mass)
						{
							this.moving_balls.Add(new ConduitFlowVisualizer.RenderMeshTask.Ball(lastFlowInfo.direction, vector, color, context.outer.tuning.size, false, false));
						}
						if (element == null || lastFlowInfo.contents.element != element.id)
						{
							element = ElementLoader.FindElementByHash(lastFlowInfo.contents.element);
						}
					}
					else
					{
						element = null;
						highlight = (Grid.PosToCell(new Vector3(vector.x + ConduitFlowVisualizer.GRID_OFFSET.x, vector.y + ConduitFlowVisualizer.GRID_OFFSET.y, 0f)) == context.outer.highlightedCell);
					}
					Color32 contentsColor = context.outer.GetContentsColor(element, color);
					float num2 = 1f;
					if (context.outer.showContents || lastFlowInfo.contents.mass < initialContents.mass)
					{
						num2 = context.outer.CalculateMassScale(lastFlowInfo.contents.mass);
					}
					this.moving_balls.Add(new ConduitFlowVisualizer.RenderMeshTask.Ball(lastFlowInfo.direction, vector, contentsColor, context.outer.tuning.size * num2, true, highlight));
					this.moving_conduits.Add(conduit);
				}
				if (initialContents.mass > lastFlowInfo.contents.mass && initialContents.mass > 0f)
				{
					int cell2 = conduit.GetCell(context.outer.flowManager);
					Vector2 pos = Grid.CellToXY(cell2);
					float mass = initialContents.mass - lastFlowInfo.contents.mass;
					bool highlight2 = false;
					Color32 cellTintColour3 = context.outer.GetCellTintColour(cell2);
					float num3 = context.outer.CalculateMassScale(mass);
					if (context.outer.showContents)
					{
						this.static_balls.Add(new ConduitFlowVisualizer.RenderMeshTask.Ball(ConduitFlow.FlowDirections.None, pos, cellTintColour3, context.outer.tuning.size * num3, false, false));
						if (element == null || initialContents.element != element.id)
						{
							element = ElementLoader.FindElementByHash(initialContents.element);
						}
					}
					else
					{
						element = null;
						highlight2 = (cell2 == context.outer.highlightedCell);
					}
					Color32 contentsColor2 = context.outer.GetContentsColor(element, cellTintColour3);
					this.static_balls.Add(new ConduitFlowVisualizer.RenderMeshTask.Ball(ConduitFlow.FlowDirections.None, pos, contentsColor2, context.outer.tuning.size * num3, true, highlight2));
				}
			}
		}

		// Token: 0x06009359 RID: 37721 RVA: 0x0032F0A8 File Offset: 0x0032D2A8
		public void Finish(ConduitFlowVisualizer.ConduitFlowMesh moving_ball_mesh, ConduitFlowVisualizer.ConduitFlowMesh static_ball_mesh, Vector3 camera_pos, ConduitFlowVisualizer visualizer)
		{
			for (int num = 0; num != this.moving_balls.Count; num++)
			{
				this.moving_balls[num].Consume(moving_ball_mesh);
			}
			this.moving_balls.Recycle();
			for (int num2 = 0; num2 != this.static_balls.Count; num2++)
			{
				this.static_balls[num2].Consume(static_ball_mesh);
			}
			this.static_balls.Recycle();
			if (visualizer != null)
			{
				foreach (ConduitFlow.Conduit conduit in this.moving_conduits)
				{
					visualizer.AddAudioSource(conduit, camera_pos);
				}
			}
			this.moving_conduits.Recycle();
		}

		// Token: 0x04007381 RID: 29569
		private ListPool<ConduitFlowVisualizer.RenderMeshTask.Ball, ConduitFlowVisualizer.RenderMeshTask>.PooledList moving_balls;

		// Token: 0x04007382 RID: 29570
		private ListPool<ConduitFlowVisualizer.RenderMeshTask.Ball, ConduitFlowVisualizer.RenderMeshTask>.PooledList static_balls;

		// Token: 0x04007383 RID: 29571
		private ListPool<ConduitFlow.Conduit, ConduitFlowVisualizer.RenderMeshTask>.PooledList moving_conduits;

		// Token: 0x04007384 RID: 29572
		private int start;

		// Token: 0x04007385 RID: 29573
		private int end;

		// Token: 0x0200220E RID: 8718
		public struct Ball
		{
			// Token: 0x0600ACA7 RID: 44199 RVA: 0x00377CC5 File Offset: 0x00375EC5
			public Ball(ConduitFlow.FlowDirections direction, Vector2 pos, Color32 color, float size, bool foreground, bool highlight)
			{
				this.pos = pos;
				this.size = size;
				this.color = color;
				this.direction = direction;
				this.foreground = foreground;
				this.highlight = highlight;
			}

			// Token: 0x0600ACA8 RID: 44200 RVA: 0x00377CF4 File Offset: 0x00375EF4
			public static void InitializeResources()
			{
				ConduitFlowVisualizer.RenderMeshTask.Ball.uv_packs[ConduitFlow.FlowDirections.None] = new ConduitFlowVisualizer.RenderMeshTask.Ball.UVPack
				{
					bl = new Vector2I(0, 0),
					tl = new Vector2I(0, 1),
					br = new Vector2I(1, 0),
					tr = new Vector2I(1, 1)
				};
				ConduitFlowVisualizer.RenderMeshTask.Ball.uv_packs[ConduitFlow.FlowDirections.Left] = new ConduitFlowVisualizer.RenderMeshTask.Ball.UVPack
				{
					bl = new Vector2I(0, 0),
					tl = new Vector2I(0, 1),
					br = new Vector2I(1, 0),
					tr = new Vector2I(1, 1)
				};
				ConduitFlowVisualizer.RenderMeshTask.Ball.uv_packs[ConduitFlow.FlowDirections.Right] = ConduitFlowVisualizer.RenderMeshTask.Ball.uv_packs[ConduitFlow.FlowDirections.Left];
				ConduitFlowVisualizer.RenderMeshTask.Ball.uv_packs[ConduitFlow.FlowDirections.Up] = new ConduitFlowVisualizer.RenderMeshTask.Ball.UVPack
				{
					bl = new Vector2I(1, 0),
					tl = new Vector2I(0, 0),
					br = new Vector2I(1, 1),
					tr = new Vector2I(0, 1)
				};
				ConduitFlowVisualizer.RenderMeshTask.Ball.uv_packs[ConduitFlow.FlowDirections.Down] = ConduitFlowVisualizer.RenderMeshTask.Ball.uv_packs[ConduitFlow.FlowDirections.Up];
			}

			// Token: 0x0600ACA9 RID: 44201 RVA: 0x00377DF9 File Offset: 0x00375FF9
			private static ConduitFlowVisualizer.RenderMeshTask.Ball.UVPack GetUVPack(ConduitFlow.FlowDirections direction)
			{
				return ConduitFlowVisualizer.RenderMeshTask.Ball.uv_packs[direction];
			}

			// Token: 0x0600ACAA RID: 44202 RVA: 0x00377E08 File Offset: 0x00376008
			public void Consume(ConduitFlowVisualizer.ConduitFlowMesh mesh)
			{
				ConduitFlowVisualizer.RenderMeshTask.Ball.UVPack uvpack = ConduitFlowVisualizer.RenderMeshTask.Ball.GetUVPack(this.direction);
				mesh.AddQuad(this.pos, this.color, this.size, (float)(this.foreground ? 1 : 0), (float)(this.highlight ? 1 : 0), uvpack.bl, uvpack.tl, uvpack.br, uvpack.tr);
			}

			// Token: 0x04009878 RID: 39032
			private Vector2 pos;

			// Token: 0x04009879 RID: 39033
			private float size;

			// Token: 0x0400987A RID: 39034
			private Color32 color;

			// Token: 0x0400987B RID: 39035
			private ConduitFlow.FlowDirections direction;

			// Token: 0x0400987C RID: 39036
			private bool foreground;

			// Token: 0x0400987D RID: 39037
			private bool highlight;

			// Token: 0x0400987E RID: 39038
			private static Dictionary<ConduitFlow.FlowDirections, ConduitFlowVisualizer.RenderMeshTask.Ball.UVPack> uv_packs = new Dictionary<ConduitFlow.FlowDirections, ConduitFlowVisualizer.RenderMeshTask.Ball.UVPack>();

			// Token: 0x02002F69 RID: 12137
			private class UVPack
			{
				// Token: 0x0400C1B6 RID: 49590
				public Vector2I bl;

				// Token: 0x0400C1B7 RID: 49591
				public Vector2I tl;

				// Token: 0x0400C1B8 RID: 49592
				public Vector2I br;

				// Token: 0x0400C1B9 RID: 49593
				public Vector2I tr;
			}
		}
	}
}
