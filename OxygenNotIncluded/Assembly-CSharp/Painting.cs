using System;

// Token: 0x020004E9 RID: 1257
public class Painting : Artable
{
	// Token: 0x06001D30 RID: 7472 RVA: 0x0009B148 File Offset: 0x00099348
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.SetOffsetTable(OffsetGroups.InvertedStandardTable);
		this.multitoolContext = "paint";
		this.multitoolHitEffectTag = "fx_paint_splash";
	}

	// Token: 0x06001D31 RID: 7473 RVA: 0x0009B17B File Offset: 0x0009937B
	protected override void OnSpawn()
	{
		base.OnSpawn();
		Components.Paintings.Add(this);
	}

	// Token: 0x06001D32 RID: 7474 RVA: 0x0009B18E File Offset: 0x0009938E
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
		Components.Paintings.Remove(this);
	}
}
