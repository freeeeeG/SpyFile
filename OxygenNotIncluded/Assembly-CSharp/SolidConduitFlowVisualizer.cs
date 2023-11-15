using System;
using System.Collections.Generic;
using FMOD.Studio;
using FMODUnity;
using UnityEngine;

// Token: 0x02000A7E RID: 2686
public class SolidConduitFlowVisualizer
{
	// Token: 0x060051A5 RID: 20901 RVA: 0x001D1E00 File Offset: 0x001D0000
	public SolidConduitFlowVisualizer(SolidConduitFlow flow_manager, Game.ConduitVisInfo vis_info, EventReference overlay_sound, SolidConduitFlowVisualizer.Tuning tuning)
	{
		this.flowManager = flow_manager;
		this.visInfo = vis_info;
		this.overlaySound = overlay_sound;
		this.tuning = tuning;
		this.movingBallMesh = new SolidConduitFlowVisualizer.ConduitFlowMesh();
		this.staticBallMesh = new SolidConduitFlowVisualizer.ConduitFlowMesh();
	}

	// Token: 0x060051A6 RID: 20902 RVA: 0x001D1E7C File Offset: 0x001D007C
	public void FreeResources()
	{
		this.movingBallMesh.Cleanup();
		this.staticBallMesh.Cleanup();
	}

	// Token: 0x060051A7 RID: 20903 RVA: 0x001D1E94 File Offset: 0x001D0094
	private float CalculateMassScale(float mass)
	{
		float t = (mass - this.visInfo.overlayMassScaleRange.x) / (this.visInfo.overlayMassScaleRange.y - this.visInfo.overlayMassScaleRange.x);
		return Mathf.Lerp(this.visInfo.overlayMassScaleValues.x, this.visInfo.overlayMassScaleValues.y, t);
	}

	// Token: 0x060051A8 RID: 20904 RVA: 0x001D1EFC File Offset: 0x001D00FC
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

	// Token: 0x060051A9 RID: 20905 RVA: 0x001D1F34 File Offset: 0x001D0134
	private Color32 GetBackgroundColor(float insulation_lerp)
	{
		if (this.showContents)
		{
			return Color32.Lerp(GlobalAssets.Instance.colorSet.GetColorByName(this.visInfo.overlayTintName), GlobalAssets.Instance.colorSet.GetColorByName(this.visInfo.overlayInsulatedTintName), insulation_lerp);
		}
		return Color32.Lerp(this.visInfo.tint, this.visInfo.insulatedTint, insulation_lerp);
	}

	// Token: 0x060051AA RID: 20906 RVA: 0x001D1FA0 File Offset: 0x001D01A0
	public void Render(float z, int render_layer, float lerp_percent, bool trigger_audio = false)
	{
		GridArea visibleArea = GridVisibleArea.GetVisibleArea();
		Vector2I v = new Vector2I(Mathf.Max(0, visibleArea.Min.x - 1), Mathf.Max(0, visibleArea.Min.y - 1));
		Vector2I v2 = new Vector2I(Mathf.Min(Grid.WidthInCells - 1, visibleArea.Max.x + 1), Mathf.Min(Grid.HeightInCells - 1, visibleArea.Max.y + 1));
		this.animTime += (double)Time.deltaTime;
		if (trigger_audio)
		{
			if (this.audioInfo == null)
			{
				this.audioInfo = new List<SolidConduitFlowVisualizer.AudioInfo>();
			}
			for (int i = 0; i < this.audioInfo.Count; i++)
			{
				SolidConduitFlowVisualizer.AudioInfo audioInfo = this.audioInfo[i];
				audioInfo.distance = float.PositiveInfinity;
				audioInfo.position = Vector3.zero;
				audioInfo.blobCount = (audioInfo.blobCount + 1) % SolidConduitFlowVisualizer.BLOB_SOUND_COUNT;
				this.audioInfo[i] = audioInfo;
			}
		}
		Vector3 position = CameraController.Instance.transform.GetPosition();
		Element element = null;
		if (this.tuning.renderMesh)
		{
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
			for (int j = 0; j < this.flowManager.GetSOAInfo().NumEntries; j++)
			{
				Vector2I u = Grid.CellToXY(this.flowManager.GetSOAInfo().GetCell(j));
				if (!(u < v) && !(u > v2))
				{
					SolidConduitFlow.Conduit conduit = this.flowManager.GetSOAInfo().GetConduit(j);
					SolidConduitFlow.ConduitFlowInfo lastFlowInfo = conduit.GetLastFlowInfo(this.flowManager);
					SolidConduitFlow.ConduitContents initialContents = conduit.GetInitialContents(this.flowManager);
					bool flag = lastFlowInfo.direction > SolidConduitFlow.FlowDirection.None;
					if (flag)
					{
						int cell = conduit.GetCell(this.flowManager);
						int cellFromDirection = SolidConduitFlow.GetCellFromDirection(cell, lastFlowInfo.direction);
						Vector2I vector2I = Grid.CellToXY(cell);
						Vector2I vector2I2 = Grid.CellToXY(cellFromDirection);
						Vector2 vector = vector2I;
						if (cell != -1)
						{
							vector = Vector2.Lerp(new Vector2((float)vector2I.x, (float)vector2I.y), new Vector2((float)vector2I2.x, (float)vector2I2.y), lerp_percent);
						}
						float a = this.insulatedCells.Contains(cell) ? 1f : 0f;
						float b = this.insulatedCells.Contains(cellFromDirection) ? 1f : 0f;
						float insulation_lerp = Mathf.Lerp(a, b, lerp_percent);
						Color c = this.GetBackgroundColor(insulation_lerp);
						Vector2I uvbl = new Vector2I(0, 0);
						Vector2I uvtl = new Vector2I(0, 1);
						Vector2I uvbr = new Vector2I(1, 0);
						Vector2I uvtr = new Vector2I(1, 1);
						float highlight = 0f;
						if (this.showContents)
						{
							if (flag != initialContents.pickupableHandle.IsValid())
							{
								this.movingBallMesh.AddQuad(vector, c, this.tuning.size, 0f, 0f, uvbl, uvtl, uvbr, uvtr);
							}
						}
						else
						{
							element = null;
							if (Grid.PosToCell(new Vector3(vector.x + SolidConduitFlowVisualizer.GRID_OFFSET.x, vector.y + SolidConduitFlowVisualizer.GRID_OFFSET.y, 0f)) == this.highlightedCell)
							{
								highlight = 1f;
							}
						}
						Color32 contentsColor = this.GetContentsColor(element, c);
						float num = 1f;
						this.movingBallMesh.AddQuad(vector, contentsColor, this.tuning.size * num, 1f, highlight, uvbl, uvtl, uvbr, uvtr);
						if (trigger_audio)
						{
							this.AddAudioSource(conduit, position);
						}
					}
					if (initialContents.pickupableHandle.IsValid() && !flag)
					{
						int cell2 = conduit.GetCell(this.flowManager);
						Vector2 pos = Grid.CellToXY(cell2);
						float insulation_lerp2 = this.insulatedCells.Contains(cell2) ? 1f : 0f;
						Vector2I uvbl2 = new Vector2I(0, 0);
						Vector2I uvtl2 = new Vector2I(0, 1);
						Vector2I uvbr2 = new Vector2I(1, 0);
						Vector2I uvtr2 = new Vector2I(1, 1);
						float highlight2 = 0f;
						Color c2 = this.GetBackgroundColor(insulation_lerp2);
						float num2 = 1f;
						if (this.showContents)
						{
							this.staticBallMesh.AddQuad(pos, c2, this.tuning.size * num2, 0f, 0f, uvbl2, uvtl2, uvbr2, uvtr2);
						}
						else
						{
							element = null;
							if (cell2 == this.highlightedCell)
							{
								highlight2 = 1f;
							}
						}
						Color32 contentsColor2 = this.GetContentsColor(element, c2);
						this.staticBallMesh.AddQuad(pos, contentsColor2, this.tuning.size * num2, 1f, highlight2, uvbl2, uvtl2, uvbr2, uvtr2);
					}
				}
			}
			this.movingBallMesh.End(z, this.layer);
			this.staticBallMesh.End(z, this.layer);
		}
		if (trigger_audio)
		{
			this.TriggerAudio();
		}
	}

	// Token: 0x060051AB RID: 20907 RVA: 0x001D2664 File Offset: 0x001D0864
	public void ColourizePipeContents(bool show_contents, bool move_to_overlay_layer)
	{
		this.showContents = show_contents;
		this.layer = ((show_contents && move_to_overlay_layer) ? LayerMask.NameToLayer("MaskedOverlay") : 0);
	}

	// Token: 0x060051AC RID: 20908 RVA: 0x001D2688 File Offset: 0x001D0888
	private void AddAudioSource(SolidConduitFlow.Conduit conduit, Vector3 camera_pos)
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
					SolidConduitFlowVisualizer.AudioInfo audioInfo = this.audioInfo[i];
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
					SolidConduitFlowVisualizer.AudioInfo item = default(SolidConduitFlowVisualizer.AudioInfo);
					item.networkID = network.id;
					item.position = vector;
					item.distance = num;
					item.blobCount = 0;
					this.audioInfo.Add(item);
				}
			}
		}
	}

	// Token: 0x060051AD RID: 20909 RVA: 0x001D27A0 File Offset: 0x001D09A0
	private void TriggerAudio()
	{
		if (SpeedControlScreen.Instance.IsPaused)
		{
			return;
		}
		CameraController instance = CameraController.Instance;
		int num = 0;
		List<SolidConduitFlowVisualizer.AudioInfo> list = new List<SolidConduitFlowVisualizer.AudioInfo>();
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
			SolidConduitFlowVisualizer.AudioInfo audioInfo = list[j];
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

	// Token: 0x060051AE RID: 20910 RVA: 0x001D2892 File Offset: 0x001D0A92
	public void SetInsulated(int cell, bool insulated)
	{
		if (insulated)
		{
			this.insulatedCells.Add(cell);
			return;
		}
		this.insulatedCells.Remove(cell);
	}

	// Token: 0x060051AF RID: 20911 RVA: 0x001D28B2 File Offset: 0x001D0AB2
	public void SetHighlightedCell(int cell)
	{
		this.highlightedCell = cell;
	}

	// Token: 0x04003594 RID: 13716
	private SolidConduitFlow flowManager;

	// Token: 0x04003595 RID: 13717
	private EventReference overlaySound;

	// Token: 0x04003596 RID: 13718
	private bool showContents;

	// Token: 0x04003597 RID: 13719
	private double animTime;

	// Token: 0x04003598 RID: 13720
	private int layer;

	// Token: 0x04003599 RID: 13721
	private static Vector2 GRID_OFFSET = new Vector2(0.5f, 0.5f);

	// Token: 0x0400359A RID: 13722
	private static int BLOB_SOUND_COUNT = 7;

	// Token: 0x0400359B RID: 13723
	private List<SolidConduitFlowVisualizer.AudioInfo> audioInfo;

	// Token: 0x0400359C RID: 13724
	private HashSet<int> insulatedCells = new HashSet<int>();

	// Token: 0x0400359D RID: 13725
	private Game.ConduitVisInfo visInfo;

	// Token: 0x0400359E RID: 13726
	private SolidConduitFlowVisualizer.ConduitFlowMesh movingBallMesh;

	// Token: 0x0400359F RID: 13727
	private SolidConduitFlowVisualizer.ConduitFlowMesh staticBallMesh;

	// Token: 0x040035A0 RID: 13728
	private int highlightedCell = -1;

	// Token: 0x040035A1 RID: 13729
	private Color32 highlightColour = new Color(0.2f, 0.2f, 0.2f, 0.2f);

	// Token: 0x040035A2 RID: 13730
	private SolidConduitFlowVisualizer.Tuning tuning;

	// Token: 0x02001937 RID: 6455
	[Serializable]
	public class Tuning
	{
		// Token: 0x040074B5 RID: 29877
		public bool renderMesh;

		// Token: 0x040074B6 RID: 29878
		public float size;

		// Token: 0x040074B7 RID: 29879
		public float spriteCount;

		// Token: 0x040074B8 RID: 29880
		public float framesPerSecond;

		// Token: 0x040074B9 RID: 29881
		public Texture2D backgroundTexture;

		// Token: 0x040074BA RID: 29882
		public Texture2D foregroundTexture;
	}

	// Token: 0x02001938 RID: 6456
	private class ConduitFlowMesh
	{
		// Token: 0x060094A3 RID: 38051 RVA: 0x00337108 File Offset: 0x00335308
		public ConduitFlowMesh()
		{
			this.mesh = new Mesh();
			this.mesh.name = "ConduitMesh";
			this.material = new Material(Shader.Find("Klei/ConduitBall"));
		}

		// Token: 0x060094A4 RID: 38052 RVA: 0x00337178 File Offset: 0x00335378
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

		// Token: 0x060094A5 RID: 38053 RVA: 0x0033736B File Offset: 0x0033556B
		public void SetTexture(string id, Texture2D texture)
		{
			this.material.SetTexture(id, texture);
		}

		// Token: 0x060094A6 RID: 38054 RVA: 0x0033737A File Offset: 0x0033557A
		public void SetVector(string id, Vector4 data)
		{
			this.material.SetVector(id, data);
		}

		// Token: 0x060094A7 RID: 38055 RVA: 0x00337389 File Offset: 0x00335589
		public void Begin()
		{
			this.positions.Clear();
			this.uvs.Clear();
			this.triangles.Clear();
			this.colors.Clear();
			this.quadIndex = 0;
		}

		// Token: 0x060094A8 RID: 38056 RVA: 0x003373C0 File Offset: 0x003355C0
		public void End(float z, int layer)
		{
			this.mesh.Clear();
			this.mesh.SetVertices(this.positions);
			this.mesh.SetUVs(0, this.uvs);
			this.mesh.SetColors(this.colors);
			this.mesh.SetTriangles(this.triangles, 0, false);
			Graphics.DrawMesh(this.mesh, new Vector3(SolidConduitFlowVisualizer.GRID_OFFSET.x, SolidConduitFlowVisualizer.GRID_OFFSET.y, z - 0.1f), Quaternion.identity, this.material, layer);
		}

		// Token: 0x060094A9 RID: 38057 RVA: 0x00337456 File Offset: 0x00335656
		public void Cleanup()
		{
			UnityEngine.Object.Destroy(this.mesh);
			this.mesh = null;
			UnityEngine.Object.Destroy(this.material);
			this.material = null;
		}

		// Token: 0x040074BB RID: 29883
		private Mesh mesh;

		// Token: 0x040074BC RID: 29884
		private Material material;

		// Token: 0x040074BD RID: 29885
		private List<Vector3> positions = new List<Vector3>();

		// Token: 0x040074BE RID: 29886
		private List<Vector4> uvs = new List<Vector4>();

		// Token: 0x040074BF RID: 29887
		private List<int> triangles = new List<int>();

		// Token: 0x040074C0 RID: 29888
		private List<Color32> colors = new List<Color32>();

		// Token: 0x040074C1 RID: 29889
		private int quadIndex;
	}

	// Token: 0x02001939 RID: 6457
	private struct AudioInfo
	{
		// Token: 0x040074C2 RID: 29890
		public int networkID;

		// Token: 0x040074C3 RID: 29891
		public int blobCount;

		// Token: 0x040074C4 RID: 29892
		public float distance;

		// Token: 0x040074C5 RID: 29893
		public Vector3 position;
	}
}
