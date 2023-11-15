using System;
using System.Collections.Generic;
using Team17.Online.Multiplayer.Messaging;
using UnityEngine;

// Token: 0x02000883 RID: 2179
internal class EntityIdDebugDisplay : DebugDisplay
{
	// Token: 0x06002A42 RID: 10818 RVA: 0x000C550C File Offset: 0x000C390C
	public override void OnSetUp()
	{
		this.m_resolutionWidth = Screen.width;
		this.m_resolutionHeight = Screen.height;
		EntitySerialisationRegistry.OnEntryAdded = (GenericVoid<EntitySerialisationEntry>)Delegate.Combine(EntitySerialisationRegistry.OnEntryAdded, new GenericVoid<EntitySerialisationEntry>(this.OnEntityEntryAdded));
		EntitySerialisationRegistry.OnEntryRemoved = (GenericVoid<EntitySerialisationEntry>)Delegate.Combine(EntitySerialisationRegistry.OnEntryRemoved, new GenericVoid<EntitySerialisationEntry>(this.OnEntityEntryRemoved));
	}

	// Token: 0x06002A43 RID: 10819 RVA: 0x000C5570 File Offset: 0x000C3970
	public override void OnDestroy()
	{
		EntitySerialisationRegistry.OnEntryAdded = (GenericVoid<EntitySerialisationEntry>)Delegate.Remove(EntitySerialisationRegistry.OnEntryAdded, new GenericVoid<EntitySerialisationEntry>(this.OnEntityEntryAdded));
		EntitySerialisationRegistry.OnEntryRemoved = (GenericVoid<EntitySerialisationEntry>)Delegate.Remove(EntitySerialisationRegistry.OnEntryRemoved, new GenericVoid<EntitySerialisationEntry>(this.OnEntityEntryRemoved));
	}

	// Token: 0x06002A44 RID: 10820 RVA: 0x000C55C0 File Offset: 0x000C39C0
	private Camera GetCamera()
	{
		if (this.m_camera == null)
		{
			Camera[] allCameras = Camera.allCameras;
			for (int i = 0; i < allCameras.Length; i++)
			{
				if (allCameras[i] != null)
				{
					this.m_camera = allCameras[i];
					return this.m_camera;
				}
			}
		}
		return this.m_camera;
	}

	// Token: 0x06002A45 RID: 10821 RVA: 0x000C561C File Offset: 0x000C3A1C
	private void OnEntityEntryAdded(EntitySerialisationEntry entry)
	{
		if (entry != null)
		{
			EntityIdDebugDisplay.EntityInfo item = new EntityIdDebugDisplay.EntityInfo
			{
				m_id = ((entry.m_Header != null) ? entry.m_Header.m_uEntityID : 0U),
				m_worldPos = ((!(entry.m_GameObject == null)) ? entry.m_GameObject.transform.position : Vector3.zero),
				m_name = ((!(entry.m_GameObject == null)) ? entry.m_GameObject.name : "<unknown>")
			};
			Camera camera = this.GetCamera();
			if (camera != null)
			{
				string text = item.m_id + " - " + item.m_name;
				Vector2 position = camera.WorldToScreenPoint(item.m_worldPos);
				position.y = (float)this.m_resolutionHeight - position.y;
				Rect rect = new Rect(position, new Vector2((float)(text.Length * this.m_smallLabelFontSize), (float)this.m_smallLabelFontSize));
				item.m_rect = this.LayoutRect(rect);
				this.m_entityInfos.Add(item);
			}
		}
	}

	// Token: 0x06002A46 RID: 10822 RVA: 0x000C5754 File Offset: 0x000C3B54
	private void OnEntityEntryRemoved(EntitySerialisationEntry entry)
	{
		if (entry != null && entry.m_Header != null)
		{
			for (int i = this.m_entityInfos.Count - 1; i >= 0; i--)
			{
				if (this.m_entityInfos._items[i].m_id == entry.m_Header.m_uEntityID)
				{
					this.m_entityInfos.RemoveAt(i);
					break;
				}
			}
		}
	}

	// Token: 0x06002A47 RID: 10823 RVA: 0x000C57C8 File Offset: 0x000C3BC8
	public override void OnUpdate()
	{
		if (this.m_resolutionWidth != Screen.width || this.m_resolutionHeight != Screen.height)
		{
			this.m_resolutionWidth = Screen.width;
			this.m_resolutionHeight = Screen.height;
			for (int i = 0; i < this.m_entityInfos.Count; i++)
			{
				EntityIdDebugDisplay.EntityInfo entityInfo = this.m_entityInfos._items[i];
				this.m_entityInfos._items[i].m_rect = this.LayoutRect(this.m_entityInfos._items[i].m_rect);
				this.m_entityInfos._items[i] = entityInfo;
			}
		}
	}

	// Token: 0x06002A48 RID: 10824 RVA: 0x000C5888 File Offset: 0x000C3C88
	private Rect LayoutRect(Rect rect)
	{
		Rect result = rect;
		bool flag;
		do
		{
			flag = false;
			for (int i = 0; i < this.m_entityInfos.Count; i++)
			{
				Rect rect2 = this.m_entityInfos._items[i].m_rect;
				if (result.Overlaps(rect2))
				{
					result.y += rect2.height + 0.5f;
					flag = true;
				}
			}
		}
		while (flag);
		return result;
	}

	// Token: 0x06002A49 RID: 10825 RVA: 0x000C5900 File Offset: 0x000C3D00
	public override void OnDraw(ref Rect rect, GUIStyle style)
	{
		if (this.m_smallLabel == null)
		{
			this.m_smallLabel = new GUIStyle("Label")
			{
				fontSize = this.m_smallLabelFontSize,
				alignment = TextAnchor.UpperLeft,
				clipping = TextClipping.Overflow
			};
		}
		this.m_reflowList.Clear();
		for (int i = 0; i < this.m_entityInfos.Count; i++)
		{
			EntityIdDebugDisplay.EntityInfo entityInfo = this.m_entityInfos._items[i];
			EntitySerialisationEntry entry = EntitySerialisationRegistry.GetEntry(entityInfo.m_id);
			if (entry != null && entry.m_GameObject != null)
			{
				Rect rect2 = entityInfo.m_rect;
				float num = VectorUtils.Hmax(VectorUtils.Abs(entry.m_GameObject.transform.position - entityInfo.m_worldPos));
				if (num > 1f)
				{
					this.m_reflowList.Add(i);
				}
				GUIUtils.ShadowedLabel(rect2, entityInfo.m_id + " - " + entityInfo.m_name, this.m_smallLabel);
			}
		}
		Camera camera = this.GetCamera();
		if (camera != null)
		{
			for (int j = 0; j < this.m_reflowList.Count; j++)
			{
				int num2 = this.m_reflowList._items[j];
				EntityIdDebugDisplay.EntityInfo entityInfo2 = this.m_entityInfos._items[num2];
				EntitySerialisationEntry entry2 = EntitySerialisationRegistry.GetEntry(entityInfo2.m_id);
				if (entry2 != null && entry2.m_GameObject != null)
				{
					entityInfo2.m_worldPos = entry2.m_GameObject.transform.position;
					Vector2 vector = camera.WorldToScreenPoint(entityInfo2.m_worldPos);
					vector.y = (float)this.m_resolutionHeight - vector.y;
					Rect rect3 = entityInfo2.m_rect;
					rect3.x = vector.x;
					rect3.y = vector.y;
					rect3 = this.LayoutRect(rect3);
					entityInfo2.m_rect = rect3;
					this.m_entityInfos._items[num2] = entityInfo2;
				}
			}
		}
	}

	// Token: 0x04002144 RID: 8516
	private Camera m_camera;

	// Token: 0x04002145 RID: 8517
	private Camera[] m_cameras;

	// Token: 0x04002146 RID: 8518
	private int m_resolutionWidth;

	// Token: 0x04002147 RID: 8519
	private int m_resolutionHeight;

	// Token: 0x04002148 RID: 8520
	private FastList<EntityIdDebugDisplay.EntityInfo> m_entityInfos = new FastList<EntityIdDebugDisplay.EntityInfo>(256);

	// Token: 0x04002149 RID: 8521
	private int m_smallLabelFontSize = 10;

	// Token: 0x0400214A RID: 8522
	private GUIStyle m_smallLabel;

	// Token: 0x0400214B RID: 8523
	private FastList<int> m_reflowList = new FastList<int>(256);

	// Token: 0x02000884 RID: 2180
	private struct EntityInfo
	{
		// Token: 0x0400214C RID: 8524
		public uint m_id;

		// Token: 0x0400214D RID: 8525
		public Vector3 m_worldPos;

		// Token: 0x0400214E RID: 8526
		public Rect m_rect;

		// Token: 0x0400214F RID: 8527
		public string m_name;
	}
}
