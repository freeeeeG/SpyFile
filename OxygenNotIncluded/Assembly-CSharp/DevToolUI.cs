using System;
using System.Collections.Generic;
using ImGuiNET;
using UnityEngine;
using UnityEngine.EventSystems;

// Token: 0x02000570 RID: 1392
public class DevToolUI : DevTool
{
	// Token: 0x060021DF RID: 8671 RVA: 0x000B9EE6 File Offset: 0x000B80E6
	protected override void RenderTo(DevPanel panel)
	{
		this.RepopulateRaycastHits();
		this.DrawPingObject();
		this.DrawRaycastHits();
	}

	// Token: 0x060021E0 RID: 8672 RVA: 0x000B9EFC File Offset: 0x000B80FC
	private void DrawPingObject()
	{
		if (this.m_last_pinged_hit != null)
		{
			GameObject gameObject = this.m_last_pinged_hit.Value.gameObject;
			if (gameObject != null && gameObject)
			{
				ImGui.Text("Last Pinged: \"" + DevToolUI.GetQualifiedName(gameObject) + "\"");
				ImGui.SameLine();
				if (ImGui.Button("Inspect"))
				{
					DevToolSceneInspector.Inspect(gameObject);
				}
				ImGui.Spacing();
				ImGui.Spacing();
			}
			else
			{
				this.m_last_pinged_hit = null;
			}
		}
		ImGui.Text("Press \",\" to ping the top hovered ui object");
		ImGui.Spacing();
		ImGui.Spacing();
	}

	// Token: 0x060021E1 RID: 8673 RVA: 0x000B9F9B File Offset: 0x000B819B
	private void Internal_Ping(RaycastResult raycastResult)
	{
		GameObject gameObject = raycastResult.gameObject;
		this.m_last_pinged_hit = new RaycastResult?(raycastResult);
	}

	// Token: 0x060021E2 RID: 8674 RVA: 0x000B9FB4 File Offset: 0x000B81B4
	public static void PingHoveredObject()
	{
		using (ListPool<RaycastResult, DevToolUI>.PooledList pooledList = PoolsFor<DevToolUI>.AllocateList<RaycastResult>())
		{
			UnityEngine.EventSystems.EventSystem current = UnityEngine.EventSystems.EventSystem.current;
			if (!(current == null) && current)
			{
				current.RaycastAll(new PointerEventData(current)
				{
					position = Input.mousePosition
				}, pooledList);
				DevToolUI devToolUI = DevToolManager.Instance.panels.AddOrGetDevTool<DevToolUI>();
				if (pooledList.Count > 0)
				{
					devToolUI.Internal_Ping(pooledList[0]);
				}
			}
		}
	}

	// Token: 0x060021E3 RID: 8675 RVA: 0x000BA040 File Offset: 0x000B8240
	private void DrawRaycastHits()
	{
		if (this.m_raycast_hits.Count <= 0)
		{
			ImGui.Text("Didn't hit any ui");
			return;
		}
		ImGui.Text("Raycast Hits:");
		ImGui.Indent();
		for (int i = 0; i < this.m_raycast_hits.Count; i++)
		{
			RaycastResult raycastResult = this.m_raycast_hits[i];
			ImGui.BulletText(string.Format("[{0}] {1}", i, DevToolUI.GetQualifiedName(raycastResult.gameObject)));
		}
		ImGui.Unindent();
	}

	// Token: 0x060021E4 RID: 8676 RVA: 0x000BA0C0 File Offset: 0x000B82C0
	private void RepopulateRaycastHits()
	{
		this.m_raycast_hits.Clear();
		UnityEngine.EventSystems.EventSystem current = UnityEngine.EventSystems.EventSystem.current;
		if (current == null || !current)
		{
			return;
		}
		current.RaycastAll(new PointerEventData(current)
		{
			position = Input.mousePosition
		}, this.m_raycast_hits);
	}

	// Token: 0x060021E5 RID: 8677 RVA: 0x000BA114 File Offset: 0x000B8314
	private static string GetQualifiedName(GameObject game_object)
	{
		KScreen componentInParent = game_object.GetComponentInParent<KScreen>();
		if (componentInParent != null)
		{
			return componentInParent.gameObject.name + " :: " + game_object.name;
		}
		return game_object.name ?? "";
	}

	// Token: 0x04001342 RID: 4930
	private List<RaycastResult> m_raycast_hits = new List<RaycastResult>();

	// Token: 0x04001343 RID: 4931
	private RaycastResult? m_last_pinged_hit;
}
