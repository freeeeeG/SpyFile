using System;

// Token: 0x020004BF RID: 1215
public struct PlacementContext
{
	// Token: 0x06001686 RID: 5766 RVA: 0x00076B27 File Offset: 0x00074F27
	public PlacementContext(PlacementContext.Source source = PlacementContext.Source.Game)
	{
		this.m_source = source;
	}

	// Token: 0x040010F0 RID: 4336
	public PlacementContext.Source m_source;

	// Token: 0x020004C0 RID: 1216
	public enum Source
	{
		// Token: 0x040010F2 RID: 4338
		Game,
		// Token: 0x040010F3 RID: 4339
		Player
	}
}
