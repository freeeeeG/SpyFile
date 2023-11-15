using System;
using System.Collections.Generic;
using ImGuiNET;

// Token: 0x02000560 RID: 1376
public class DevToolMenuNodeParent : IMenuNode
{
	// Token: 0x06002189 RID: 8585 RVA: 0x000B592C File Offset: 0x000B3B2C
	public DevToolMenuNodeParent(string name)
	{
		this.name = name;
		this.children = new List<IMenuNode>();
	}

	// Token: 0x0600218A RID: 8586 RVA: 0x000B5946 File Offset: 0x000B3B46
	public void AddChild(IMenuNode menuNode)
	{
		this.children.Add(menuNode);
	}

	// Token: 0x0600218B RID: 8587 RVA: 0x000B5954 File Offset: 0x000B3B54
	public string GetName()
	{
		return this.name;
	}

	// Token: 0x0600218C RID: 8588 RVA: 0x000B595C File Offset: 0x000B3B5C
	public void Draw()
	{
		if (ImGui.BeginMenu(this.name))
		{
			foreach (IMenuNode menuNode in this.children)
			{
				menuNode.Draw();
			}
			ImGui.EndMenu();
		}
	}

	// Token: 0x040012FF RID: 4863
	public string name;

	// Token: 0x04001300 RID: 4864
	public List<IMenuNode> children;
}
