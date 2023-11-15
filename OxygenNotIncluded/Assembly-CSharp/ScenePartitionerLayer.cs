using System;

// Token: 0x02000959 RID: 2393
public class ScenePartitionerLayer
{
	// Token: 0x06004650 RID: 18000 RVA: 0x0018DD82 File Offset: 0x0018BF82
	public ScenePartitionerLayer(HashedString name, int layer)
	{
		this.name = name;
		this.layer = layer;
	}

	// Token: 0x04002E9A RID: 11930
	public HashedString name;

	// Token: 0x04002E9B RID: 11931
	public int layer;

	// Token: 0x04002E9C RID: 11932
	public Action<int, object> OnEvent;
}
