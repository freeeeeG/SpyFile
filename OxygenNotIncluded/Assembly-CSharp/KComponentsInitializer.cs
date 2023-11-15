using System;

// Token: 0x02000838 RID: 2104
public class KComponentsInitializer : KComponentSpawn
{
	// Token: 0x06003D27 RID: 15655 RVA: 0x001536B3 File Offset: 0x001518B3
	private void Awake()
	{
		KComponentSpawn.instance = this;
		this.comps = new GameComps();
	}
}
