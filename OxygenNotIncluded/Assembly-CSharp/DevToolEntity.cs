using System;
using System.Runtime.CompilerServices;
using ImGuiNET;
using STRINGS;
using UnityEngine;

// Token: 0x02000555 RID: 1365
public class DevToolEntity : DevTool
{
	// Token: 0x06002140 RID: 8512 RVA: 0x000B20C4 File Offset: 0x000B02C4
	protected override void RenderTo(DevPanel panel)
	{
		if (ImGui.BeginMenuBar())
		{
			if (ImGui.MenuItem("New Window"))
			{
				DevToolUtil.Open(new DevToolEntity());
			}
			ImGui.EndMenuBar();
		}
		ImGui.Text(this.currentTargetOpt.IsNone() ? "Pick target:" : "Change target:");
		ImGui.SameLine();
		if (ImGui.Button("Eyedrop"))
		{
			panel.PushDevTool(new DevToolEntity_EyeDrop(delegate(DevToolEntityTarget result)
			{
				this.currentTargetOpt = result;
			}, null));
		}
		ImGui.SameLine();
		if (ImGui.Button("Search GameObjects (NOT implemented)"))
		{
			panel.PushDevTool(new DevToolEntity_SearchGameObjects(delegate(DevToolEntityTarget result)
			{
				this.currentTargetOpt = result;
			}));
		}
		if (this.GetInGameSelectedEntity().IsSome())
		{
			ImGui.SameLine();
			if (ImGui.Button("\"" + this.GetInGameSelectedEntity().Unwrap().name + "\""))
			{
				this.currentTargetOpt = new DevToolEntityTarget.ForWorldGameObject(this.GetInGameSelectedEntity().Unwrap());
			}
		}
		ImGui.Separator();
		ImGui.Spacing();
		if (this.currentTargetOpt.IsNone())
		{
			this.Name = "Entity";
			ImGui.Text("<nothing selected>");
		}
		else
		{
			this.Name = "Entity: " + this.currentTargetOpt.Unwrap().ToString();
			this.Name = "EntityType: " + this.currentTargetOpt.Unwrap().GetType().FullName.Substring("For".Length);
			ImGuiEx.SimpleField("Entity Name", this.currentTargetOpt.Unwrap().ToString());
		}
		ImGui.Spacing();
		ImGui.Separator();
		ImGui.Spacing();
		if (this.currentTargetOpt.IsNone())
		{
			return;
		}
		DevToolEntityTarget devToolEntityTarget = this.currentTargetOpt.Unwrap();
		DevToolEntityTarget.ForUIGameObject forUIGameObject = devToolEntityTarget as DevToolEntityTarget.ForUIGameObject;
		Option<GameObject> option;
		if (forUIGameObject != null)
		{
			option = forUIGameObject.gameObject;
		}
		else
		{
			DevToolEntityTarget.ForWorldGameObject forWorldGameObject = devToolEntityTarget as DevToolEntityTarget.ForWorldGameObject;
			if (forWorldGameObject != null)
			{
				option = forWorldGameObject.gameObject;
			}
			else
			{
				option = Option.None;
			}
		}
		if (ImGui.CollapsingHeader("Actions", ImGuiTreeNodeFlags.DefaultOpen))
		{
			ImGui.Indent();
			ImGui.Checkbox("Draw Bounding Box", ref this.shouldDrawBoundingBox);
			if (option.IsSome())
			{
				GameObject gameObject = option.Unwrap();
				if (ImGui.Button(string.Format("Inspect GameObject in DevTools###ID_InspectInGame_{0}", gameObject.GetInstanceID())))
				{
					DevToolSceneInspector.Inspect(gameObject);
				}
				WildnessMonitor.Instance smi = gameObject.GetSMI<WildnessMonitor.Instance>();
				if (smi.IsNullOrDestroyed())
				{
					ImGuiEx.Button("Taming: Covert to Tamed", "No WildnessMonitor.Instance found on the selected GameObject");
				}
				else
				{
					WildnessMonitor wildnessMonitor = (WildnessMonitor)smi.GetStateMachine();
					if (smi.GetCurrentState() != wildnessMonitor.tame)
					{
						if (ImGui.Button("Taming: Convert to Tamed"))
						{
							smi.wildness.SetValue(0f);
							smi.GoTo(wildnessMonitor.tame);
						}
					}
					else if (ImGui.Button("Taming: Convert to Untamed"))
					{
						smi.wildness.value = smi.wildness.GetMax();
						smi.GoTo(wildnessMonitor.wild);
					}
				}
			}
			ImGui.Unindent();
		}
		ImGui.Spacing();
		if (ImGui.CollapsingHeader("Related DevTools", ImGuiTreeNodeFlags.DefaultOpen))
		{
			ImGui.Indent();
			if (ImGuiEx.Button("Debug Status Items", DevToolStatusItems.GetErrorForCandidateTarget(devToolEntityTarget).UnwrapOrDefault()))
			{
				panel.PushDevTool(new DevToolStatusItems((DevToolEntityTarget.ForWorldGameObject)devToolEntityTarget));
			}
			if (ImGuiEx.Button("Debug Cavity", DevToolCavity.GetErrorForCandidateTarget(devToolEntityTarget).UnwrapOrDefault()))
			{
				panel.PushDevTool(new DevToolCavity((DevToolEntityTarget.ForSimCell)devToolEntityTarget));
			}
			if (ImGuiEx.Button("Debug GoTo", DevToolEntity_DebugGoTo.GetErrorForCandidateTarget(devToolEntityTarget).UnwrapOrDefault()))
			{
				panel.PushDevTool(new DevToolEntity_DebugGoTo((DevToolEntityTarget.ForWorldGameObject)devToolEntityTarget));
			}
			if (ImGuiEx.Button("Debug RanchStation", DevToolEntity_RanchStation.GetErrorForCandidateTarget(devToolEntityTarget).UnwrapOrDefault()))
			{
				panel.PushDevTool(new DevToolEntity_RanchStation((DevToolEntityTarget.ForWorldGameObject)devToolEntityTarget));
			}
			ImGui.Unindent();
		}
		if (this.shouldDrawBoundingBox)
		{
			Option<ValueTuple<Vector2, Vector2>> screenRect = devToolEntityTarget.GetScreenRect();
			if (screenRect.IsSome())
			{
				DevToolEntity.DrawBoundingBox(screenRect.Unwrap(), devToolEntityTarget.GetDebugName(), ImGui.IsWindowFocused());
			}
		}
	}

	// Token: 0x06002141 RID: 8513 RVA: 0x000B24E0 File Offset: 0x000B06E0
	public Option<GameObject> GetInGameSelectedEntity()
	{
		if (SelectTool.Instance == null)
		{
			return Option.None;
		}
		KSelectable selected = SelectTool.Instance.selected;
		if (selected.IsNullOrDestroyed())
		{
			return Option.None;
		}
		return selected.gameObject;
	}

	// Token: 0x06002142 RID: 8514 RVA: 0x000B2530 File Offset: 0x000B0730
	public static string GetNameFor(GameObject gameObject)
	{
		if (gameObject.IsNullOrDestroyed())
		{
			return "<null or destroyed GameObject>";
		}
		return string.Concat(new string[]
		{
			"\"",
			UI.StripLinkFormatting(gameObject.name),
			"\" [0x",
			gameObject.GetInstanceID().ToString("X"),
			"]"
		});
	}

	// Token: 0x06002143 RID: 8515 RVA: 0x000B2594 File Offset: 0x000B0794
	public static void DrawBoundingBox([TupleElementNames(new string[]
	{
		"cornerA",
		"cornerB"
	})] ValueTuple<Vector2, Vector2> screenRect, string name, bool isFocused)
	{
		if (isFocused)
		{
			DevToolEntity.DrawScreenRect(screenRect, name, new Color(1f, 0f, 0f, 1f), new Color(1f, 0f, 0f, 0.3f));
			return;
		}
		DevToolEntity.DrawScreenRect(screenRect, Option.None, new Color(0.9f, 0f, 0f, 0.6f), default(Option<Color>));
	}

	// Token: 0x06002144 RID: 8516 RVA: 0x000B2624 File Offset: 0x000B0824
	public static void DrawScreenRect([TupleElementNames(new string[]
	{
		"cornerA",
		"cornerB"
	})] ValueTuple<Vector2, Vector2> screenRect, Option<string> text = default(Option<string>), Option<Color> outlineColor = default(Option<Color>), Option<Color> fillColor = default(Option<Color>))
	{
		Vector2 vector = Vector2.Min(screenRect.Item1, screenRect.Item2);
		Vector2 vector2 = Vector2.Max(screenRect.Item1, screenRect.Item2);
		ImGui.GetBackgroundDrawList().AddRect(vector, vector2, ImGui.GetColorU32(outlineColor.UnwrapOr(Color.red, null)), 0f, ImDrawFlags.None, 4f);
		ImGui.GetBackgroundDrawList().AddRectFilled(vector, vector2, ImGui.GetColorU32(fillColor.UnwrapOr(Color.clear, null)));
		if (text.IsSome())
		{
			ImGui.GetBackgroundDrawList().AddText(ImGui.GetFont(), 30f, new Vector2(vector2.x, vector.y) + new Vector2(15f, 0f), ImGui.GetColorU32(Color.white), text.Unwrap());
		}
	}

	// Token: 0x040012BF RID: 4799
	private Option<DevToolEntityTarget> currentTargetOpt;

	// Token: 0x040012C0 RID: 4800
	private bool shouldDrawBoundingBox = true;
}
