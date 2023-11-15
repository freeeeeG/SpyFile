using System;
using System.Collections.Generic;
using System.Linq;
using ImGuiNET;
using UnityEngine;

// Token: 0x02000571 RID: 1393
public class DevToolUnlockedIds : DevTool
{
	// Token: 0x060021E7 RID: 8679 RVA: 0x000BA16F File Offset: 0x000B836F
	public DevToolUnlockedIds()
	{
		this.RequiresGameRunning = true;
	}

	// Token: 0x060021E8 RID: 8680 RVA: 0x000BA194 File Offset: 0x000B8394
	protected override void RenderTo(DevPanel panel)
	{
		bool flag;
		DevToolUnlockedIds.UnlocksWrapper unlocksWrapper;
		this.GetUnlocks().Deconstruct(out flag, out unlocksWrapper);
		bool flag2 = flag;
		DevToolUnlockedIds.UnlocksWrapper unlocksWrapper2 = unlocksWrapper;
		if (!flag2)
		{
			ImGui.Text("Couldn't access global unlocks");
			return;
		}
		if (ImGui.TreeNode("Help"))
		{
			ImGui.TextWrapped("This is a list of global unlocks that are persistant across saves. Changes made here will be saved to disk immediately.");
			ImGui.Spacing();
			ImGui.TextWrapped("NOTE: It may be necessary to relaunch the game after modifying unlocks in order for systems to respond.");
			ImGui.TreePop();
		}
		ImGui.Spacing();
		ImGuiEx.InputFilter("Filter", ref this.filterForUnlockIds, 50U);
		ImGuiTableFlags flags = ImGuiTableFlags.RowBg | ImGuiTableFlags.BordersInnerH | ImGuiTableFlags.BordersOuterH | ImGuiTableFlags.BordersInnerV | ImGuiTableFlags.BordersOuterV | ImGuiTableFlags.ScrollY;
		if (ImGui.BeginTable("ID_unlockIds", 2, flags))
		{
			ImGui.TableSetupScrollFreeze(2, 2);
			ImGui.TableSetupColumn("Unlock ID");
			ImGui.TableSetupColumn("Actions", ImGuiTableColumnFlags.WidthFixed);
			ImGui.TableHeadersRow();
			ImGui.PushID("ID_row_add_new");
			ImGui.TableNextRow();
			ImGui.TableSetColumnIndex(0);
			ImGui.InputText("", ref this.unlockIdToAdd, 50U);
			ImGui.TableSetColumnIndex(1);
			if (ImGui.Button("Add"))
			{
				unlocksWrapper2.AddId(this.unlockIdToAdd);
				global::Debug.Log("[Added unlock id] " + this.unlockIdToAdd);
				this.unlockIdToAdd = "";
			}
			ImGui.PopID();
			int num = 0;
			foreach (string text in unlocksWrapper2.GetAllIds())
			{
				string text2 = (text == null) ? "<<null>>" : ("\"" + text + "\"");
				if (text2.ToLower().Contains(this.filterForUnlockIds.ToLower()))
				{
					ImGui.TableNextRow();
					ImGui.PushID(string.Format("ID_row_{0}", num++));
					ImGui.TableSetColumnIndex(0);
					ImGui.Text(text2);
					ImGui.TableSetColumnIndex(1);
					if (ImGui.Button("Copy"))
					{
						GUIUtility.systemCopyBuffer = text;
						global::Debug.Log("[Copied to clipboard] " + text);
					}
					ImGui.SameLine();
					if (ImGui.Button("Remove"))
					{
						unlocksWrapper2.RemoveId(text);
						global::Debug.Log("[Removed unlock id] " + text);
					}
					ImGui.PopID();
				}
			}
			ImGui.EndTable();
		}
	}

	// Token: 0x060021E9 RID: 8681 RVA: 0x000BA3C0 File Offset: 0x000B85C0
	private Option<DevToolUnlockedIds.UnlocksWrapper> GetUnlocks()
	{
		if (App.IsExiting)
		{
			return Option.None;
		}
		if (Game.Instance == null || !Game.Instance)
		{
			return Option.None;
		}
		if (Game.Instance.unlocks == null)
		{
			return Option.None;
		}
		return Option.Some<DevToolUnlockedIds.UnlocksWrapper>(new DevToolUnlockedIds.UnlocksWrapper(Game.Instance.unlocks));
	}

	// Token: 0x04001344 RID: 4932
	private string filterForUnlockIds = "";

	// Token: 0x04001345 RID: 4933
	private string unlockIdToAdd = "";

	// Token: 0x02001205 RID: 4613
	public readonly struct UnlocksWrapper
	{
		// Token: 0x06007B83 RID: 31619 RVA: 0x002DE06B File Offset: 0x002DC26B
		public UnlocksWrapper(Unlocks unlocks)
		{
			this.unlocks = unlocks;
		}

		// Token: 0x06007B84 RID: 31620 RVA: 0x002DE074 File Offset: 0x002DC274
		public void AddId(string unlockId)
		{
			this.unlocks.Unlock(unlockId, true);
		}

		// Token: 0x06007B85 RID: 31621 RVA: 0x002DE083 File Offset: 0x002DC283
		public void RemoveId(string unlockId)
		{
			this.unlocks.Lock(unlockId);
		}

		// Token: 0x06007B86 RID: 31622 RVA: 0x002DE091 File Offset: 0x002DC291
		public IEnumerable<string> GetAllIds()
		{
			return from s in this.unlocks.GetAllUnlockedIds()
			orderby s
			select s;
		}

		// Token: 0x1700084A RID: 2122
		// (get) Token: 0x06007B87 RID: 31623 RVA: 0x002DE0C2 File Offset: 0x002DC2C2
		public int Count
		{
			get
			{
				return this.unlocks.GetAllUnlockedIds().Count;
			}
		}

		// Token: 0x04005E3D RID: 24125
		public readonly Unlocks unlocks;
	}
}
