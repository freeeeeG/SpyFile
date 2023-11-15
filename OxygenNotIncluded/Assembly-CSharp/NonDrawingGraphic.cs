using System;
using UnityEngine.UI;

// Token: 0x02000BA4 RID: 2980
public class NonDrawingGraphic : Graphic
{
	// Token: 0x06005CEF RID: 23791 RVA: 0x002207D9 File Offset: 0x0021E9D9
	public override void SetMaterialDirty()
	{
	}

	// Token: 0x06005CF0 RID: 23792 RVA: 0x002207DB File Offset: 0x0021E9DB
	public override void SetVerticesDirty()
	{
	}

	// Token: 0x06005CF1 RID: 23793 RVA: 0x002207DD File Offset: 0x0021E9DD
	protected override void OnPopulateMesh(VertexHelper vh)
	{
		vh.Clear();
	}
}
