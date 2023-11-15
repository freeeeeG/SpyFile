using System;
using System.IO;
using ImGuiNET;

// Token: 0x0200055E RID: 1374
public class DevToolMenuNodeList
{
	// Token: 0x06002182 RID: 8578 RVA: 0x000B57CC File Offset: 0x000B39CC
	public DevToolMenuNodeParent AddOrGetParentFor(string childPath)
	{
		string[] array = Path.GetDirectoryName(childPath).Split(new char[]
		{
			'/'
		});
		string text = "";
		DevToolMenuNodeParent devToolMenuNodeParent = this.root;
		string[] array2 = array;
		for (int i = 0; i < array2.Length; i++)
		{
			string split = array2[i];
			text += devToolMenuNodeParent.GetName();
			IMenuNode menuNode = devToolMenuNodeParent.children.Find((IMenuNode x) => x.GetName() == split);
			DevToolMenuNodeParent devToolMenuNodeParent3;
			if (menuNode != null)
			{
				DevToolMenuNodeParent devToolMenuNodeParent2 = menuNode as DevToolMenuNodeParent;
				if (devToolMenuNodeParent2 == null)
				{
					throw new Exception("Conflict! Both a leaf and parent node exist at path: " + text);
				}
				devToolMenuNodeParent3 = devToolMenuNodeParent2;
			}
			else
			{
				devToolMenuNodeParent3 = new DevToolMenuNodeParent(split);
				devToolMenuNodeParent.AddChild(devToolMenuNodeParent3);
			}
			devToolMenuNodeParent = devToolMenuNodeParent3;
		}
		return devToolMenuNodeParent;
	}

	// Token: 0x06002183 RID: 8579 RVA: 0x000B5880 File Offset: 0x000B3A80
	public DevToolMenuNodeAction AddAction(string path, System.Action onClickFn)
	{
		DevToolMenuNodeAction devToolMenuNodeAction = new DevToolMenuNodeAction(Path.GetFileName(path), onClickFn);
		this.AddOrGetParentFor(path).AddChild(devToolMenuNodeAction);
		return devToolMenuNodeAction;
	}

	// Token: 0x06002184 RID: 8580 RVA: 0x000B58A8 File Offset: 0x000B3AA8
	public void Draw()
	{
		foreach (IMenuNode menuNode in this.root.children)
		{
			menuNode.Draw();
		}
	}

	// Token: 0x06002185 RID: 8581 RVA: 0x000B5900 File Offset: 0x000B3B00
	public void DrawFull()
	{
		if (ImGui.BeginMainMenuBar())
		{
			this.Draw();
			ImGui.EndMainMenuBar();
		}
	}

	// Token: 0x040012FE RID: 4862
	private DevToolMenuNodeParent root = new DevToolMenuNodeParent("<root>");
}
