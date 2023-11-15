using System;
using System.Diagnostics;
using ImGuiNET;
using UnityEngine;

// Token: 0x0200056C RID: 1388
public class DevToolStatusItems : DevTool
{
	// Token: 0x060021C7 RID: 8647 RVA: 0x000B94E3 File Offset: 0x000B76E3
	public DevToolStatusItems() : this(Option.None)
	{
	}

	// Token: 0x060021C8 RID: 8648 RVA: 0x000B94F8 File Offset: 0x000B76F8
	public DevToolStatusItems(Option<DevToolEntityTarget.ForWorldGameObject> target)
	{
		this.targetOpt = target;
		this.tableDrawer = ImGuiObjectTableDrawer<StatusItemGroup.Entry>.New().RemoveFlags(ImGuiTableFlags.SizingFixedFit).AddFlags(ImGuiTableFlags.Resizable).Column("Text", (StatusItemGroup.Entry entry) => entry.GetName()).Column("Id Name", (StatusItemGroup.Entry entry) => entry.item.Id).Column("Notification Type", (StatusItemGroup.Entry entry) => entry.item.notificationType).Column("Category", delegate(StatusItemGroup.Entry entry)
		{
			StatusItemCategory category = entry.category;
			return ((category != null) ? category.Name : null) ?? "<no category>";
		}).Column("OnAdded Callstack", delegate(StatusItemGroup.Entry entry)
		{
			StackTrace stackTrace;
			if (this.statusItemStackTraceWatcher.GetStackTraceForEntry(entry, out stackTrace))
			{
				if (ImGui.Selectable("copy callstack"))
				{
					ImGui.SetClipboardText(stackTrace.ToString());
				}
				ImGuiEx.TooltipForPrevious(stackTrace.ToString());
				return;
			}
			ImGui.Text("<None>");
		}).Build();
		base.OnUninit += delegate()
		{
			this.statusItemStackTraceWatcher.Dispose();
		};
	}

	// Token: 0x060021C9 RID: 8649 RVA: 0x000B9610 File Offset: 0x000B7810
	protected override void RenderTo(DevPanel panel)
	{
		this.statusItemStackTraceWatcher.SetTarget(this.targetOpt.AndThen<GameObject>((DevToolEntityTarget.ForWorldGameObject t) => t.gameObject).AndThen<KSelectable>((GameObject go) => go.GetComponent<KSelectable>()).AndThen<StatusItemGroup>((KSelectable s) => s.GetStatusItemGroup()));
		if (ImGui.BeginMenuBar())
		{
			if (ImGui.MenuItem("Eyedrop New Target"))
			{
				panel.PushDevTool(new DevToolEntity_EyeDrop(delegate(DevToolEntityTarget target)
				{
					this.targetOpt = (DevToolEntityTarget.ForWorldGameObject)target;
				}, new Func<DevToolEntityTarget, Option<string>>(DevToolStatusItems.GetErrorForCandidateTarget)));
			}
			string error = null;
			if (this.targetOpt.IsNone())
			{
				error = "No target selected.";
			}
			else
			{
				Option<string> errorForCandidateTarget = DevToolStatusItems.GetErrorForCandidateTarget(this.targetOpt.Unwrap());
				if (errorForCandidateTarget.IsSome())
				{
					error = errorForCandidateTarget.Unwrap();
				}
			}
			if (ImGuiEx.MenuItem("Debug Target", error))
			{
				panel.PushValue<DevToolEntityTarget.ForWorldGameObject>(this.targetOpt.Unwrap());
			}
			ImGui.EndMenuBar();
		}
		this.Name = "Status Items";
		if (this.targetOpt.IsNone())
		{
			ImGui.TextWrapped("No Target selected");
			return;
		}
		DevToolEntityTarget.ForWorldGameObject forWorldGameObject = this.targetOpt.Unwrap();
		Option<string> errorForCandidateTarget2 = DevToolStatusItems.GetErrorForCandidateTarget(forWorldGameObject);
		if (errorForCandidateTarget2.IsSome())
		{
			ImGui.TextWrapped(errorForCandidateTarget2.Unwrap());
			return;
		}
		this.Name = "Status Items for: " + DevToolEntity.GetNameFor(forWorldGameObject.gameObject);
		bool shouldWatch = this.statusItemStackTraceWatcher.GetShouldWatch();
		if (ImGui.Checkbox("Should Track OnAdded Callstacks", ref shouldWatch))
		{
			this.statusItemStackTraceWatcher.SetShouldWatch(shouldWatch);
		}
		ImGui.Checkbox("Draw Bounding Box", ref this.shouldDrawBoundingBox);
		this.tableDrawer.Draw(forWorldGameObject.gameObject.GetComponent<KSelectable>().GetStatusItemGroup().GetEnumerator());
		if (this.shouldDrawBoundingBox)
		{
			Option<ValueTuple<Vector2, Vector2>> screenRect = forWorldGameObject.GetScreenRect();
			if (screenRect.IsSome())
			{
				DevToolEntity.DrawBoundingBox(screenRect.Unwrap(), forWorldGameObject.GetDebugName(), ImGui.IsWindowFocused());
			}
		}
	}

	// Token: 0x060021CA RID: 8650 RVA: 0x000B982C File Offset: 0x000B7A2C
	public static Option<string> GetErrorForCandidateTarget(DevToolEntityTarget uncastTarget)
	{
		if (!(uncastTarget is DevToolEntityTarget.ForWorldGameObject))
		{
			return "Target must be a world GameObject";
		}
		DevToolEntityTarget.ForWorldGameObject forWorldGameObject = (DevToolEntityTarget.ForWorldGameObject)uncastTarget;
		if (forWorldGameObject.gameObject.IsNullOrDestroyed())
		{
			return "Target GameObject is null or destroyed";
		}
		KSelectable component = forWorldGameObject.gameObject.GetComponent<KSelectable>();
		if (component.IsNullOrDestroyed())
		{
			return "Target GameObject doesn't have a KSelectable";
		}
		if (component.GetStatusItemGroup().IsNullOrDestroyed())
		{
			return "Target GameObject doesn't have a StatusItemGroup";
		}
		return Option.None;
	}

	// Token: 0x04001337 RID: 4919
	private Option<DevToolEntityTarget.ForWorldGameObject> targetOpt;

	// Token: 0x04001338 RID: 4920
	private ImGuiObjectTableDrawer<StatusItemGroup.Entry> tableDrawer;

	// Token: 0x04001339 RID: 4921
	private StatusItemStackTraceWatcher statusItemStackTraceWatcher = new StatusItemStackTraceWatcher();

	// Token: 0x0400133A RID: 4922
	private bool shouldDrawBoundingBox = true;
}
