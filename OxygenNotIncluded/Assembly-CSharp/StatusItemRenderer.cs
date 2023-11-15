using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200050C RID: 1292
public class StatusItemRenderer
{
	// Token: 0x17000141 RID: 321
	// (get) Token: 0x06001E74 RID: 7796 RVA: 0x000A2934 File Offset: 0x000A0B34
	// (set) Token: 0x06001E75 RID: 7797 RVA: 0x000A293C File Offset: 0x000A0B3C
	public int layer { get; private set; }

	// Token: 0x17000142 RID: 322
	// (get) Token: 0x06001E76 RID: 7798 RVA: 0x000A2945 File Offset: 0x000A0B45
	// (set) Token: 0x06001E77 RID: 7799 RVA: 0x000A294D File Offset: 0x000A0B4D
	public int selectedHandle { get; private set; }

	// Token: 0x17000143 RID: 323
	// (get) Token: 0x06001E78 RID: 7800 RVA: 0x000A2956 File Offset: 0x000A0B56
	// (set) Token: 0x06001E79 RID: 7801 RVA: 0x000A295E File Offset: 0x000A0B5E
	public int highlightHandle { get; private set; }

	// Token: 0x17000144 RID: 324
	// (get) Token: 0x06001E7A RID: 7802 RVA: 0x000A2967 File Offset: 0x000A0B67
	// (set) Token: 0x06001E7B RID: 7803 RVA: 0x000A296F File Offset: 0x000A0B6F
	public Color32 backgroundColor { get; private set; }

	// Token: 0x17000145 RID: 325
	// (get) Token: 0x06001E7C RID: 7804 RVA: 0x000A2978 File Offset: 0x000A0B78
	// (set) Token: 0x06001E7D RID: 7805 RVA: 0x000A2980 File Offset: 0x000A0B80
	public Color32 selectedColor { get; private set; }

	// Token: 0x17000146 RID: 326
	// (get) Token: 0x06001E7E RID: 7806 RVA: 0x000A2989 File Offset: 0x000A0B89
	// (set) Token: 0x06001E7F RID: 7807 RVA: 0x000A2991 File Offset: 0x000A0B91
	public Color32 neutralColor { get; private set; }

	// Token: 0x17000147 RID: 327
	// (get) Token: 0x06001E80 RID: 7808 RVA: 0x000A299A File Offset: 0x000A0B9A
	// (set) Token: 0x06001E81 RID: 7809 RVA: 0x000A29A2 File Offset: 0x000A0BA2
	public Sprite arrowSprite { get; private set; }

	// Token: 0x17000148 RID: 328
	// (get) Token: 0x06001E82 RID: 7810 RVA: 0x000A29AB File Offset: 0x000A0BAB
	// (set) Token: 0x06001E83 RID: 7811 RVA: 0x000A29B3 File Offset: 0x000A0BB3
	public Sprite backgroundSprite { get; private set; }

	// Token: 0x17000149 RID: 329
	// (get) Token: 0x06001E84 RID: 7812 RVA: 0x000A29BC File Offset: 0x000A0BBC
	// (set) Token: 0x06001E85 RID: 7813 RVA: 0x000A29C4 File Offset: 0x000A0BC4
	public float scale { get; private set; }

	// Token: 0x06001E86 RID: 7814 RVA: 0x000A29D0 File Offset: 0x000A0BD0
	public StatusItemRenderer()
	{
		this.layer = LayerMask.NameToLayer("UI");
		this.entries = new StatusItemRenderer.Entry[100];
		this.shader = Shader.Find("Klei/StatusItem");
		for (int i = 0; i < this.entries.Length; i++)
		{
			StatusItemRenderer.Entry entry = default(StatusItemRenderer.Entry);
			entry.Init(this.shader);
			this.entries[i] = entry;
		}
		this.backgroundColor = new Color32(244, 74, 71, byte.MaxValue);
		this.selectedColor = new Color32(225, 181, 180, byte.MaxValue);
		this.neutralColor = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);
		this.arrowSprite = Assets.GetSprite("StatusBubbleTop");
		this.backgroundSprite = Assets.GetSprite("StatusBubble");
		this.scale = 1f;
		Game.Instance.Subscribe(2095258329, new Action<object>(this.OnHighlightObject));
	}

	// Token: 0x06001E87 RID: 7815 RVA: 0x000A2B04 File Offset: 0x000A0D04
	public int GetIdx(Transform transform)
	{
		int instanceID = transform.GetInstanceID();
		int num = 0;
		if (!this.handleTable.TryGetValue(instanceID, out num))
		{
			int num2 = this.entryCount;
			this.entryCount = num2 + 1;
			num = num2;
			this.handleTable[instanceID] = num;
			StatusItemRenderer.Entry entry = this.entries[num];
			entry.handle = instanceID;
			entry.transform = transform;
			entry.buildingPos = transform.GetPosition();
			entry.building = transform.GetComponent<Building>();
			entry.isBuilding = (entry.building != null);
			entry.selectable = transform.GetComponent<KSelectable>();
			this.entries[num] = entry;
		}
		return num;
	}

	// Token: 0x06001E88 RID: 7816 RVA: 0x000A2BB4 File Offset: 0x000A0DB4
	public void Add(Transform transform, StatusItem status_item)
	{
		if (this.entryCount == this.entries.Length)
		{
			StatusItemRenderer.Entry[] array = new StatusItemRenderer.Entry[this.entries.Length * 2];
			for (int i = 0; i < this.entries.Length; i++)
			{
				array[i] = this.entries[i];
			}
			for (int j = this.entries.Length; j < array.Length; j++)
			{
				array[j].Init(this.shader);
			}
			this.entries = array;
		}
		int idx = this.GetIdx(transform);
		StatusItemRenderer.Entry entry = this.entries[idx];
		entry.Add(status_item);
		this.entries[idx] = entry;
	}

	// Token: 0x06001E89 RID: 7817 RVA: 0x000A2C64 File Offset: 0x000A0E64
	public void Remove(Transform transform, StatusItem status_item)
	{
		int instanceID = transform.GetInstanceID();
		int num = 0;
		if (!this.handleTable.TryGetValue(instanceID, out num))
		{
			return;
		}
		StatusItemRenderer.Entry entry = this.entries[num];
		if (entry.statusItems.Count == 0)
		{
			return;
		}
		entry.Remove(status_item);
		this.entries[num] = entry;
		if (entry.statusItems.Count == 0)
		{
			this.ClearIdx(num);
		}
	}

	// Token: 0x06001E8A RID: 7818 RVA: 0x000A2CD0 File Offset: 0x000A0ED0
	private void ClearIdx(int idx)
	{
		StatusItemRenderer.Entry entry = this.entries[idx];
		this.handleTable.Remove(entry.handle);
		if (idx != this.entryCount - 1)
		{
			entry.Replace(this.entries[this.entryCount - 1]);
			this.entries[idx] = entry;
			this.handleTable[entry.handle] = idx;
		}
		entry = this.entries[this.entryCount - 1];
		entry.Clear();
		this.entries[this.entryCount - 1] = entry;
		this.entryCount--;
	}

	// Token: 0x06001E8B RID: 7819 RVA: 0x000A2D7D File Offset: 0x000A0F7D
	private HashedString GetMode()
	{
		if (OverlayScreen.Instance != null)
		{
			return OverlayScreen.Instance.mode;
		}
		return OverlayModes.None.ID;
	}

	// Token: 0x06001E8C RID: 7820 RVA: 0x000A2D9C File Offset: 0x000A0F9C
	public void MarkAllDirty()
	{
		for (int i = 0; i < this.entryCount; i++)
		{
			this.entries[i].MarkDirty();
		}
	}

	// Token: 0x06001E8D RID: 7821 RVA: 0x000A2DCC File Offset: 0x000A0FCC
	public void RenderEveryTick()
	{
		if (DebugHandler.HideUI)
		{
			return;
		}
		this.scale = 1f + Mathf.Sin(Time.unscaledTime * 8f) * 0.1f;
		Shader.SetGlobalVector("_StatusItemParameters", new Vector4(this.scale, 0f, 0f, 0f));
		Vector3 camera_tr = Camera.main.ViewportToWorldPoint(new Vector3(1f, 1f, Camera.main.transform.GetPosition().z));
		Vector3 camera_bl = Camera.main.ViewportToWorldPoint(new Vector3(0f, 0f, Camera.main.transform.GetPosition().z));
		this.visibleEntries.Clear();
		Camera worldCamera = GameScreenManager.Instance.worldSpaceCanvas.GetComponent<Canvas>().worldCamera;
		for (int i = 0; i < this.entryCount; i++)
		{
			this.entries[i].Render(this, camera_bl, camera_tr, this.GetMode(), worldCamera);
		}
	}

	// Token: 0x06001E8E RID: 7822 RVA: 0x000A2ED0 File Offset: 0x000A10D0
	public void GetIntersections(Vector2 pos, List<InterfaceTool.Intersection> intersections)
	{
		foreach (StatusItemRenderer.Entry entry in this.visibleEntries)
		{
			entry.GetIntersection(pos, intersections, this.scale);
		}
	}

	// Token: 0x06001E8F RID: 7823 RVA: 0x000A2F2C File Offset: 0x000A112C
	public void GetIntersections(Vector2 pos, List<KSelectable> selectables)
	{
		foreach (StatusItemRenderer.Entry entry in this.visibleEntries)
		{
			entry.GetIntersection(pos, selectables, this.scale);
		}
	}

	// Token: 0x06001E90 RID: 7824 RVA: 0x000A2F88 File Offset: 0x000A1188
	public void SetOffset(Transform transform, Vector3 offset)
	{
		int num = 0;
		if (this.handleTable.TryGetValue(transform.GetInstanceID(), out num))
		{
			this.entries[num].offset = offset;
		}
	}

	// Token: 0x06001E91 RID: 7825 RVA: 0x000A2FC0 File Offset: 0x000A11C0
	private void OnSelectObject(object data)
	{
		int num = 0;
		if (this.handleTable.TryGetValue(this.selectedHandle, out num))
		{
			this.entries[num].MarkDirty();
		}
		GameObject gameObject = (GameObject)data;
		if (gameObject != null)
		{
			this.selectedHandle = gameObject.transform.GetInstanceID();
			if (this.handleTable.TryGetValue(this.selectedHandle, out num))
			{
				this.entries[num].MarkDirty();
				return;
			}
		}
		else
		{
			this.highlightHandle = -1;
		}
	}

	// Token: 0x06001E92 RID: 7826 RVA: 0x000A3044 File Offset: 0x000A1244
	private void OnHighlightObject(object data)
	{
		int num = 0;
		if (this.handleTable.TryGetValue(this.highlightHandle, out num))
		{
			StatusItemRenderer.Entry entry = this.entries[num];
			entry.MarkDirty();
			this.entries[num] = entry;
		}
		GameObject gameObject = (GameObject)data;
		if (gameObject != null)
		{
			this.highlightHandle = gameObject.transform.GetInstanceID();
			if (this.handleTable.TryGetValue(this.highlightHandle, out num))
			{
				StatusItemRenderer.Entry entry2 = this.entries[num];
				entry2.MarkDirty();
				this.entries[num] = entry2;
				return;
			}
		}
		else
		{
			this.highlightHandle = -1;
		}
	}

	// Token: 0x06001E93 RID: 7827 RVA: 0x000A30E8 File Offset: 0x000A12E8
	public void Destroy()
	{
		Game.Instance.Unsubscribe(-1503271301, new Action<object>(this.OnSelectObject));
		Game.Instance.Unsubscribe(-1201923725, new Action<object>(this.OnHighlightObject));
		foreach (StatusItemRenderer.Entry entry in this.entries)
		{
			entry.Clear();
			entry.FreeResources();
		}
	}

	// Token: 0x0400111D RID: 4381
	private StatusItemRenderer.Entry[] entries;

	// Token: 0x0400111E RID: 4382
	private int entryCount;

	// Token: 0x0400111F RID: 4383
	private Dictionary<int, int> handleTable = new Dictionary<int, int>();

	// Token: 0x04001129 RID: 4393
	private Shader shader;

	// Token: 0x0400112A RID: 4394
	public List<StatusItemRenderer.Entry> visibleEntries = new List<StatusItemRenderer.Entry>();

	// Token: 0x020011B3 RID: 4531
	public struct Entry
	{
		// Token: 0x06007A82 RID: 31362 RVA: 0x002DC15F File Offset: 0x002DA35F
		public void Init(Shader shader)
		{
			this.statusItems = new List<StatusItem>();
			this.mesh = new Mesh();
			this.mesh.name = "StatusItemRenderer";
			this.dirty = true;
			this.material = new Material(shader);
		}

		// Token: 0x06007A83 RID: 31363 RVA: 0x002DC19C File Offset: 0x002DA39C
		public void Render(StatusItemRenderer renderer, Vector3 camera_bl, Vector3 camera_tr, HashedString overlay, Camera camera)
		{
			if (this.transform == null)
			{
				string text = "Error cleaning up status items:";
				foreach (StatusItem statusItem in this.statusItems)
				{
					text += statusItem.Id;
				}
				global::Debug.LogWarning(text);
				return;
			}
			Vector3 vector = this.isBuilding ? this.buildingPos : this.transform.GetPosition();
			if (this.isBuilding)
			{
				vector.x += (float)((this.building.Def.WidthInCells - 1) % 2) / 2f;
			}
			if (vector.x < camera_bl.x || vector.x > camera_tr.x || vector.y < camera_bl.y || vector.y > camera_tr.y)
			{
				return;
			}
			int num = Grid.PosToCell(vector);
			if (Grid.IsValidCell(num) && (!Grid.IsVisible(num) || (int)Grid.WorldIdx[num] != ClusterManager.Instance.activeWorldId))
			{
				return;
			}
			if (!this.selectable.IsSelectable)
			{
				return;
			}
			renderer.visibleEntries.Add(this);
			if (this.dirty)
			{
				int num2 = 0;
				foreach (StatusItem statusItem2 in this.statusItems)
				{
					if (statusItem2.UseConditionalCallback(overlay, this.transform) || !(overlay != OverlayModes.None.ID) || !(statusItem2.render_overlay != overlay))
					{
						num2++;
					}
				}
				this.hasVisibleStatusItems = (num2 != 0);
				StatusItemRenderer.Entry.MeshBuilder meshBuilder = new StatusItemRenderer.Entry.MeshBuilder(num2 + 6, this.material);
				float num3 = 0.25f;
				float z = -5f;
				Vector2 b = new Vector2(0.05f, -0.05f);
				float num4 = 0.02f;
				Color32 c = new Color32(0, 0, 0, byte.MaxValue);
				Color32 c2 = new Color32(0, 0, 0, 75);
				Color32 c3 = renderer.neutralColor;
				if (renderer.selectedHandle == this.handle || renderer.highlightHandle == this.handle)
				{
					c3 = renderer.selectedColor;
				}
				else
				{
					for (int i = 0; i < this.statusItems.Count; i++)
					{
						if (this.statusItems[i].notificationType != NotificationType.Neutral)
						{
							c3 = renderer.backgroundColor;
							break;
						}
					}
				}
				meshBuilder.AddQuad(new Vector2(0f, 0.29f) + b, new Vector2(0.05f, 0.05f), z, renderer.arrowSprite, c2);
				meshBuilder.AddQuad(new Vector2(0f, 0f) + b, new Vector2(num3 * (float)num2, num3), z, renderer.backgroundSprite, c2);
				meshBuilder.AddQuad(new Vector2(0f, 0f), new Vector2(num3 * (float)num2 + num4, num3 + num4), z, renderer.backgroundSprite, c);
				meshBuilder.AddQuad(new Vector2(0f, 0f), new Vector2(num3 * (float)num2, num3), z, renderer.backgroundSprite, c3);
				int num5 = 0;
				for (int j = 0; j < this.statusItems.Count; j++)
				{
					StatusItem statusItem3 = this.statusItems[j];
					if (statusItem3.UseConditionalCallback(overlay, this.transform) || !(overlay != OverlayModes.None.ID) || !(statusItem3.render_overlay != overlay))
					{
						float x = (float)num5 * num3 * 2f - num3 * (float)(num2 - 1);
						if (this.statusItems[j].sprite == null)
						{
							DebugUtil.DevLogError(string.Concat(new string[]
							{
								"Status Item ",
								this.statusItems[j].Id,
								" has null sprite for icon '",
								this.statusItems[j].iconName,
								"', you need to add the sprite to the TintedSprites list in the GameAssets prefab manually."
							}));
							this.statusItems[j].iconName = "status_item_exclamation";
							this.statusItems[j].sprite = Assets.GetTintedSprite("status_item_exclamation");
						}
						Sprite sprite = this.statusItems[j].sprite.sprite;
						meshBuilder.AddQuad(new Vector2(x, 0f), new Vector2(num3, num3), z, sprite, c);
						num5++;
					}
				}
				meshBuilder.AddQuad(new Vector2(0f, 0.29f + num4), new Vector2(0.05f + num4, 0.05f + num4), z, renderer.arrowSprite, c);
				meshBuilder.AddQuad(new Vector2(0f, 0.29f), new Vector2(0.05f, 0.05f), z, renderer.arrowSprite, c3);
				meshBuilder.End(this.mesh);
				this.dirty = false;
			}
			if (this.hasVisibleStatusItems && GameScreenManager.Instance != null)
			{
				Graphics.DrawMesh(this.mesh, vector + this.offset, Quaternion.identity, this.material, renderer.layer, camera, 0, null, false, false);
			}
		}

		// Token: 0x06007A84 RID: 31364 RVA: 0x002DC728 File Offset: 0x002DA928
		public void Add(StatusItem status_item)
		{
			this.statusItems.Add(status_item);
			this.dirty = true;
		}

		// Token: 0x06007A85 RID: 31365 RVA: 0x002DC73D File Offset: 0x002DA93D
		public void Remove(StatusItem status_item)
		{
			this.statusItems.Remove(status_item);
			this.dirty = true;
		}

		// Token: 0x06007A86 RID: 31366 RVA: 0x002DC754 File Offset: 0x002DA954
		public void Replace(StatusItemRenderer.Entry entry)
		{
			this.handle = entry.handle;
			this.transform = entry.transform;
			this.building = this.transform.GetComponent<Building>();
			this.buildingPos = this.transform.GetPosition();
			this.isBuilding = (this.building != null);
			this.selectable = this.transform.GetComponent<KSelectable>();
			this.offset = entry.offset;
			this.dirty = true;
			this.statusItems.Clear();
			this.statusItems.AddRange(entry.statusItems);
		}

		// Token: 0x06007A87 RID: 31367 RVA: 0x002DC7F0 File Offset: 0x002DA9F0
		private bool Intersects(Vector2 pos, float scale)
		{
			if (this.transform == null)
			{
				return false;
			}
			Bounds bounds = this.mesh.bounds;
			Vector3 vector = this.buildingPos + this.offset + bounds.center;
			Vector2 a = new Vector2(vector.x, vector.y);
			Vector3 size = bounds.size;
			Vector2 b = new Vector2(size.x * scale * 0.5f, size.y * scale * 0.5f);
			Vector2 vector2 = a - b;
			Vector2 vector3 = a + b;
			return pos.x >= vector2.x && pos.x <= vector3.x && pos.y >= vector2.y && pos.y <= vector3.y;
		}

		// Token: 0x06007A88 RID: 31368 RVA: 0x002DC8C8 File Offset: 0x002DAAC8
		public void GetIntersection(Vector2 pos, List<InterfaceTool.Intersection> intersections, float scale)
		{
			if (this.Intersects(pos, scale) && this.selectable.IsSelectable)
			{
				intersections.Add(new InterfaceTool.Intersection
				{
					component = this.selectable,
					distance = -100f
				});
			}
		}

		// Token: 0x06007A89 RID: 31369 RVA: 0x002DC914 File Offset: 0x002DAB14
		public void GetIntersection(Vector2 pos, List<KSelectable> selectables, float scale)
		{
			if (this.Intersects(pos, scale) && this.selectable.IsSelectable && !selectables.Contains(this.selectable))
			{
				selectables.Add(this.selectable);
			}
		}

		// Token: 0x06007A8A RID: 31370 RVA: 0x002DC947 File Offset: 0x002DAB47
		public void Clear()
		{
			this.statusItems.Clear();
			this.offset = Vector3.zero;
			this.dirty = false;
		}

		// Token: 0x06007A8B RID: 31371 RVA: 0x002DC966 File Offset: 0x002DAB66
		public void FreeResources()
		{
			if (this.mesh != null)
			{
				UnityEngine.Object.DestroyImmediate(this.mesh);
				this.mesh = null;
			}
			if (this.material != null)
			{
				UnityEngine.Object.DestroyImmediate(this.material);
			}
		}

		// Token: 0x06007A8C RID: 31372 RVA: 0x002DC9A1 File Offset: 0x002DABA1
		public void MarkDirty()
		{
			this.dirty = true;
		}

		// Token: 0x04005D45 RID: 23877
		public int handle;

		// Token: 0x04005D46 RID: 23878
		public Transform transform;

		// Token: 0x04005D47 RID: 23879
		public Building building;

		// Token: 0x04005D48 RID: 23880
		public Vector3 buildingPos;

		// Token: 0x04005D49 RID: 23881
		public KSelectable selectable;

		// Token: 0x04005D4A RID: 23882
		public List<StatusItem> statusItems;

		// Token: 0x04005D4B RID: 23883
		public Mesh mesh;

		// Token: 0x04005D4C RID: 23884
		public bool dirty;

		// Token: 0x04005D4D RID: 23885
		public int layer;

		// Token: 0x04005D4E RID: 23886
		public Material material;

		// Token: 0x04005D4F RID: 23887
		public Vector3 offset;

		// Token: 0x04005D50 RID: 23888
		public bool hasVisibleStatusItems;

		// Token: 0x04005D51 RID: 23889
		public bool isBuilding;

		// Token: 0x020020A3 RID: 8355
		private struct MeshBuilder
		{
			// Token: 0x0600A64F RID: 42575 RVA: 0x0036DE6C File Offset: 0x0036C06C
			public MeshBuilder(int quad_count, Material material)
			{
				this.vertices = new Vector3[4 * quad_count];
				this.uvs = new Vector2[4 * quad_count];
				this.uv2s = new Vector2[4 * quad_count];
				this.colors = new Color32[4 * quad_count];
				this.triangles = new int[6 * quad_count];
				this.material = material;
				this.quadIdx = 0;
			}

			// Token: 0x0600A650 RID: 42576 RVA: 0x0036DED0 File Offset: 0x0036C0D0
			public void AddQuad(Vector2 center, Vector2 half_size, float z, Sprite sprite, Color color)
			{
				if (this.quadIdx == StatusItemRenderer.Entry.MeshBuilder.textureIds.Length)
				{
					return;
				}
				Rect rect = sprite.rect;
				Rect textureRect = sprite.textureRect;
				float num = textureRect.width / rect.width;
				float num2 = textureRect.height / rect.height;
				int num3 = 4 * this.quadIdx;
				this.vertices[num3] = new Vector3((center.x - half_size.x) * num, (center.y - half_size.y) * num2, z);
				this.vertices[1 + num3] = new Vector3((center.x - half_size.x) * num, (center.y + half_size.y) * num2, z);
				this.vertices[2 + num3] = new Vector3((center.x + half_size.x) * num, (center.y - half_size.y) * num2, z);
				this.vertices[3 + num3] = new Vector3((center.x + half_size.x) * num, (center.y + half_size.y) * num2, z);
				float num4 = textureRect.x / (float)sprite.texture.width;
				float num5 = textureRect.y / (float)sprite.texture.height;
				float num6 = textureRect.width / (float)sprite.texture.width;
				float num7 = textureRect.height / (float)sprite.texture.height;
				this.uvs[num3] = new Vector2(num4, num5);
				this.uvs[1 + num3] = new Vector2(num4, num5 + num7);
				this.uvs[2 + num3] = new Vector2(num4 + num6, num5);
				this.uvs[3 + num3] = new Vector2(num4 + num6, num5 + num7);
				this.colors[num3] = color;
				this.colors[1 + num3] = color;
				this.colors[2 + num3] = color;
				this.colors[3 + num3] = color;
				float x = (float)this.quadIdx + 0.5f;
				this.uv2s[num3] = new Vector2(x, 0f);
				this.uv2s[1 + num3] = new Vector2(x, 0f);
				this.uv2s[2 + num3] = new Vector2(x, 0f);
				this.uv2s[3 + num3] = new Vector2(x, 0f);
				int num8 = 6 * this.quadIdx;
				this.triangles[num8] = num3;
				this.triangles[1 + num8] = num3 + 1;
				this.triangles[2 + num8] = num3 + 2;
				this.triangles[3 + num8] = num3 + 2;
				this.triangles[4 + num8] = num3 + 1;
				this.triangles[5 + num8] = num3 + 3;
				this.material.SetTexture(StatusItemRenderer.Entry.MeshBuilder.textureIds[this.quadIdx], sprite.texture);
				this.quadIdx++;
			}

			// Token: 0x0600A651 RID: 42577 RVA: 0x0036E21C File Offset: 0x0036C41C
			public void End(Mesh mesh)
			{
				mesh.Clear();
				mesh.vertices = this.vertices;
				mesh.uv = this.uvs;
				mesh.uv2 = this.uv2s;
				mesh.colors32 = this.colors;
				mesh.SetTriangles(this.triangles, 0);
				mesh.RecalculateBounds();
			}

			// Token: 0x040091C7 RID: 37319
			private Vector3[] vertices;

			// Token: 0x040091C8 RID: 37320
			private Vector2[] uvs;

			// Token: 0x040091C9 RID: 37321
			private Vector2[] uv2s;

			// Token: 0x040091CA RID: 37322
			private int[] triangles;

			// Token: 0x040091CB RID: 37323
			private Color32[] colors;

			// Token: 0x040091CC RID: 37324
			private int quadIdx;

			// Token: 0x040091CD RID: 37325
			private Material material;

			// Token: 0x040091CE RID: 37326
			private static int[] textureIds = new int[]
			{
				Shader.PropertyToID("_Tex0"),
				Shader.PropertyToID("_Tex1"),
				Shader.PropertyToID("_Tex2"),
				Shader.PropertyToID("_Tex3"),
				Shader.PropertyToID("_Tex4"),
				Shader.PropertyToID("_Tex5"),
				Shader.PropertyToID("_Tex6"),
				Shader.PropertyToID("_Tex7"),
				Shader.PropertyToID("_Tex8"),
				Shader.PropertyToID("_Tex9"),
				Shader.PropertyToID("_Tex10")
			};
		}
	}
}
