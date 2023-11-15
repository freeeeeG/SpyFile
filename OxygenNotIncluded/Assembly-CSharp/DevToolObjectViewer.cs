using System;

// Token: 0x02000563 RID: 1379
public class DevToolObjectViewer<T> : DevTool
{
	// Token: 0x06002196 RID: 8598 RVA: 0x000B5DCB File Offset: 0x000B3FCB
	public DevToolObjectViewer(Func<T> getValue)
	{
		this.getValue = getValue;
		this.Name = typeof(T).Name;
	}

	// Token: 0x06002197 RID: 8599 RVA: 0x000B5DF0 File Offset: 0x000B3FF0
	protected override void RenderTo(DevPanel panel)
	{
		T t = this.getValue();
		this.Name = t.GetType().Name;
		ImGuiEx.DrawObject(t, null);
	}

	// Token: 0x0400130C RID: 4876
	private Func<T> getValue;
}
