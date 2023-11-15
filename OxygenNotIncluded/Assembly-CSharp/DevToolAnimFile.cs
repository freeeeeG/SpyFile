using System;

// Token: 0x0200054E RID: 1358
public class DevToolAnimFile : DevTool
{
	// Token: 0x06002122 RID: 8482 RVA: 0x000B07B4 File Offset: 0x000AE9B4
	public DevToolAnimFile(KAnimFile animFile)
	{
		this.animFile = animFile;
		this.Name = "Anim File: \"" + animFile.name + "\"";
	}

	// Token: 0x06002123 RID: 8483 RVA: 0x000B07E0 File Offset: 0x000AE9E0
	protected override void RenderTo(DevPanel panel)
	{
		ImGuiEx.DrawObject(this.animFile, null);
		ImGuiEx.DrawObject(this.animFile.GetData(), null);
	}

	// Token: 0x040012AD RID: 4781
	private KAnimFile animFile;
}
