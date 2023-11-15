using System;

// Token: 0x020009C7 RID: 2503
public class ClusterMapIconFixRotation : KMonoBehaviour
{
	// Token: 0x06004ADD RID: 19165 RVA: 0x001A5614 File Offset: 0x001A3814
	private void Update()
	{
		if (base.transform.parent != null)
		{
			float z = base.transform.parent.rotation.eulerAngles.z;
			this.rotation = -z;
			this.animController.Rotation = this.rotation;
		}
	}

	// Token: 0x0400311A RID: 12570
	[MyCmpGet]
	private KBatchedAnimController animController;

	// Token: 0x0400311B RID: 12571
	private float rotation;
}
