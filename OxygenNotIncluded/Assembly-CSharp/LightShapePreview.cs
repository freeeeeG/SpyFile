using System;
using UnityEngine;

// Token: 0x02000840 RID: 2112
[AddComponentMenu("KMonoBehaviour/scripts/LightShapePreview")]
public class LightShapePreview : KMonoBehaviour
{
	// Token: 0x06003D75 RID: 15733 RVA: 0x001550A4 File Offset: 0x001532A4
	private void Update()
	{
		int num = Grid.PosToCell(base.transform.GetPosition());
		if (num != this.previousCell)
		{
			this.previousCell = num;
			LightGridManager.DestroyPreview();
			LightGridManager.CreatePreview(Grid.OffsetCell(num, this.offset), this.radius, this.shape, this.lux);
		}
	}

	// Token: 0x06003D76 RID: 15734 RVA: 0x001550FA File Offset: 0x001532FA
	protected override void OnCleanUp()
	{
		LightGridManager.DestroyPreview();
	}

	// Token: 0x04002814 RID: 10260
	public float radius;

	// Token: 0x04002815 RID: 10261
	public int lux;

	// Token: 0x04002816 RID: 10262
	public global::LightShape shape;

	// Token: 0x04002817 RID: 10263
	public CellOffset offset;

	// Token: 0x04002818 RID: 10264
	private int previousCell = -1;
}
