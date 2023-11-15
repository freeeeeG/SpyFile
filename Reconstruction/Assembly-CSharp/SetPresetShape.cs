using System;

// Token: 0x02000253 RID: 595
public class SetPresetShape : GuideEvent
{
	// Token: 0x06000F0D RID: 3853 RVA: 0x00027D70 File Offset: 0x00025F70
	public override void Trigger()
	{
		GameRes.PreSetShape[this.ShapeSlot] = this.PresetShape;
	}

	// Token: 0x0400077A RID: 1914
	public ShapeInfo PresetShape;

	// Token: 0x0400077B RID: 1915
	public int ShapeSlot;
}
