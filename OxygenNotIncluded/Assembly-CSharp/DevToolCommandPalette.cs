using System;
using System.Collections.Generic;
using System.Linq;
using ImGuiNET;
using UnityEngine;

// Token: 0x02000553 RID: 1363
public class DevToolCommandPalette : DevTool
{
	// Token: 0x06002138 RID: 8504 RVA: 0x000B1BAD File Offset: 0x000AFDAD
	public DevToolCommandPalette() : this(null)
	{
	}

	// Token: 0x06002139 RID: 8505 RVA: 0x000B1BB8 File Offset: 0x000AFDB8
	public DevToolCommandPalette(List<DevToolCommandPalette.Command> commands = null)
	{
		this.drawFlags |= ImGuiWindowFlags.NoResize;
		this.drawFlags |= ImGuiWindowFlags.NoScrollbar;
		this.drawFlags |= ImGuiWindowFlags.NoScrollWithMouse;
		if (commands == null)
		{
			this.commands.allValues = DevToolCommandPaletteUtil.GenerateDefaultCommandPalette();
			return;
		}
		this.commands.allValues = commands;
	}

	// Token: 0x0600213A RID: 8506 RVA: 0x000B1C47 File Offset: 0x000AFE47
	public static void Init()
	{
		DevToolCommandPalette.InitWithCommands(DevToolCommandPaletteUtil.GenerateDefaultCommandPalette());
	}

	// Token: 0x0600213B RID: 8507 RVA: 0x000B1C53 File Offset: 0x000AFE53
	public static void InitWithCommands(List<DevToolCommandPalette.Command> commands)
	{
		DevToolManager.Instance.panels.AddPanelFor(new DevToolCommandPalette(commands));
	}

	// Token: 0x0600213C RID: 8508 RVA: 0x000B1C6C File Offset: 0x000AFE6C
	protected override void RenderTo(DevPanel panel)
	{
		DevToolCommandPalette.Resize(panel);
		if (this.commands.allValues == null)
		{
			ImGui.Text("No commands list given");
			return;
		}
		if (this.commands.allValues.Count == 0)
		{
			ImGui.Text("Given command list is empty, no results to show.");
			return;
		}
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			panel.Close();
			return;
		}
		if (!ImGui.IsWindowFocused(ImGuiFocusedFlags.ChildWindows))
		{
			panel.Close();
			return;
		}
		if (Input.GetKeyDown(KeyCode.UpArrow))
		{
			this.m_selected_index--;
			this.shouldScrollToSelectedCommandFlag = true;
		}
		if (Input.GetKeyDown(KeyCode.DownArrow))
		{
			this.m_selected_index++;
			this.shouldScrollToSelectedCommandFlag = true;
		}
		if (this.commands.filteredValues.Count > 0)
		{
			while (this.m_selected_index < 0)
			{
				this.m_selected_index += this.commands.filteredValues.Count;
			}
			this.m_selected_index %= this.commands.filteredValues.Count;
		}
		else
		{
			this.m_selected_index = 0;
		}
		if ((Input.GetKeyUp(KeyCode.Return) || Input.GetKeyUp(KeyCode.KeypadEnter)) && this.commands.filteredValues.Count > 0)
		{
			this.SelectCommand(this.commands.filteredValues[this.m_selected_index], panel);
			return;
		}
		if (this.m_should_focus_search)
		{
			ImGui.SetKeyboardFocusHere();
		}
		if (ImGui.InputText("Filter", ref this.commands.filter, 30U) || this.m_should_focus_search)
		{
			this.commands.Refilter();
		}
		this.m_should_focus_search = false;
		ImGui.Separator();
		string text = "Up arrow & down arrow to navigate. Enter to select. ";
		if (this.commands.filteredValues.Count > 0 && this.commands.didUseFilter)
		{
			text += string.Format("Found {0} Results", this.commands.filteredValues.Count);
		}
		ImGui.Text(text);
		ImGui.Separator();
		if (ImGui.BeginChild("ID_scroll_region"))
		{
			if (this.commands.filteredValues.Count <= 0)
			{
				ImGui.Text("Couldn't find anything that matches \"" + this.commands.filter + "\", maybe it hasn't been added yet?");
			}
			else
			{
				for (int i = 0; i < this.commands.filteredValues.Count; i++)
				{
					DevToolCommandPalette.Command command = this.commands.filteredValues[i];
					bool flag = i == this.m_selected_index;
					ImGui.PushID(i);
					bool flag2;
					if (flag)
					{
						flag2 = ImGui.Selectable("> " + command.display_name, flag);
					}
					else
					{
						flag2 = ImGui.Selectable("  " + command.display_name, flag);
					}
					ImGui.PopID();
					if (this.shouldScrollToSelectedCommandFlag && flag)
					{
						this.shouldScrollToSelectedCommandFlag = false;
						ImGui.SetScrollHereY(0.5f);
					}
					if (flag2)
					{
						this.SelectCommand(command, panel);
						ImGui.EndChild();
						return;
					}
				}
			}
		}
		ImGui.EndChild();
	}

	// Token: 0x0600213D RID: 8509 RVA: 0x000B1F50 File Offset: 0x000B0150
	private void SelectCommand(DevToolCommandPalette.Command command, DevPanel panel)
	{
		command.Internal_Select();
		panel.Close();
	}

	// Token: 0x0600213E RID: 8510 RVA: 0x000B1F60 File Offset: 0x000B0160
	private static void Resize(DevPanel devToolPanel)
	{
		float num = 800f;
		float num2 = 400f;
		Rect rect = new Rect(0f, 0f, (float)Screen.width, (float)Screen.height);
		Rect rect2 = new Rect
		{
			x = rect.x + rect.width / 2f - num / 2f,
			y = rect.y + rect.height / 2f - num2 / 2f,
			width = num,
			height = num2
		};
		devToolPanel.SetPosition(rect2.position, ImGuiCond.None);
		devToolPanel.SetSize(rect2.size, ImGuiCond.None);
	}

	// Token: 0x040012BB RID: 4795
	private int m_selected_index;

	// Token: 0x040012BC RID: 4796
	private StringSearchableList<DevToolCommandPalette.Command> commands = new StringSearchableList<DevToolCommandPalette.Command>(delegate(DevToolCommandPalette.Command command, in string filter)
	{
		return !StringSearchableListUtil.DoAnyTagsMatchFilter(command.tags, filter);
	});

	// Token: 0x040012BD RID: 4797
	private bool m_should_focus_search = true;

	// Token: 0x040012BE RID: 4798
	private bool shouldScrollToSelectedCommandFlag;

	// Token: 0x020011EC RID: 4588
	public class Command
	{
		// Token: 0x06007B29 RID: 31529 RVA: 0x002DD6A0 File Offset: 0x002DB8A0
		public Command(string primary_tag, System.Action on_select) : this(new string[]
		{
			primary_tag
		}, on_select)
		{
		}

		// Token: 0x06007B2A RID: 31530 RVA: 0x002DD6B3 File Offset: 0x002DB8B3
		public Command(string primary_tag, string tag_a, System.Action on_select) : this(new string[]
		{
			primary_tag,
			tag_a
		}, on_select)
		{
		}

		// Token: 0x06007B2B RID: 31531 RVA: 0x002DD6CA File Offset: 0x002DB8CA
		public Command(string primary_tag, string tag_a, string tag_b, System.Action on_select) : this(new string[]
		{
			primary_tag,
			tag_a,
			tag_b
		}, on_select)
		{
		}

		// Token: 0x06007B2C RID: 31532 RVA: 0x002DD6E6 File Offset: 0x002DB8E6
		public Command(string primary_tag, string tag_a, string tag_b, string tag_c, System.Action on_select) : this(new string[]
		{
			primary_tag,
			tag_a,
			tag_b,
			tag_c
		}, on_select)
		{
		}

		// Token: 0x06007B2D RID: 31533 RVA: 0x002DD707 File Offset: 0x002DB907
		public Command(string primary_tag, string tag_a, string tag_b, string tag_c, string tag_d, System.Action on_select) : this(new string[]
		{
			primary_tag,
			tag_a,
			tag_b,
			tag_c,
			tag_d
		}, on_select)
		{
		}

		// Token: 0x06007B2E RID: 31534 RVA: 0x002DD72D File Offset: 0x002DB92D
		public Command(string primary_tag, string tag_a, string tag_b, string tag_c, string tag_d, string tag_e, System.Action on_select) : this(new string[]
		{
			primary_tag,
			tag_a,
			tag_b,
			tag_c,
			tag_d,
			tag_e
		}, on_select)
		{
		}

		// Token: 0x06007B2F RID: 31535 RVA: 0x002DD758 File Offset: 0x002DB958
		public Command(string primary_tag, string tag_a, string tag_b, string tag_c, string tag_d, string tag_e, string tag_f, System.Action on_select) : this(new string[]
		{
			primary_tag,
			tag_a,
			tag_b,
			tag_c,
			tag_d,
			tag_e,
			tag_f
		}, on_select)
		{
		}

		// Token: 0x06007B30 RID: 31536 RVA: 0x002DD788 File Offset: 0x002DB988
		public Command(string primary_tag, string[] additional_tags, System.Action on_select) : this(new string[]
		{
			primary_tag
		}.Concat(additional_tags).ToArray<string>(), on_select)
		{
		}

		// Token: 0x06007B31 RID: 31537 RVA: 0x002DD7A8 File Offset: 0x002DB9A8
		public Command(string[] tags, System.Action on_select)
		{
			this.display_name = tags[0];
			this.tags = (from t in tags
			select t.ToLowerInvariant()).ToArray<string>();
			this.m_on_select = on_select;
		}

		// Token: 0x06007B32 RID: 31538 RVA: 0x002DD7FB File Offset: 0x002DB9FB
		public void Internal_Select()
		{
			this.m_on_select();
		}

		// Token: 0x04005DFD RID: 24061
		public string display_name;

		// Token: 0x04005DFE RID: 24062
		public string[] tags;

		// Token: 0x04005DFF RID: 24063
		private System.Action m_on_select;
	}
}
