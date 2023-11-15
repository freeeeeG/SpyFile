using System;
using UnityEngine;

// Token: 0x02000094 RID: 148
[AddComponentMenu("KMonoBehaviour/scripts/LightSymbolTracker")]
public class LightSymbolTracker : KMonoBehaviour, IRenderEveryTick
{
	// Token: 0x060002AD RID: 685 RVA: 0x00014B50 File Offset: 0x00012D50
	public void RenderEveryTick(float dt)
	{
		Vector3 v = Vector3.zero;
		KBatchedAnimController component = base.GetComponent<KBatchedAnimController>();
		bool flag;
		v = (component.GetTransformMatrix() * component.GetSymbolLocalTransform(this.targetSymbol, out flag)).MultiplyPoint(Vector3.zero) - base.transform.GetPosition();
		base.GetComponent<Light2D>().Offset = v;
	}

	// Token: 0x040001B0 RID: 432
	public HashedString targetSymbol;
}
