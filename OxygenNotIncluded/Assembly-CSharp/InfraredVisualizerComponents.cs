using System;
using UnityEngine;

// Token: 0x020004BE RID: 1214
public class InfraredVisualizerComponents : KGameObjectComponentManager<InfraredVisualizerData>
{
	// Token: 0x06001B8F RID: 7055 RVA: 0x000938BC File Offset: 0x00091ABC
	public HandleVector<int>.Handle Add(GameObject go)
	{
		return base.Add(go, new InfraredVisualizerData(go));
	}

	// Token: 0x06001B90 RID: 7056 RVA: 0x000938CC File Offset: 0x00091ACC
	public void UpdateTemperature()
	{
		GridArea visibleArea = GridVisibleArea.GetVisibleArea();
		for (int i = 0; i < this.data.Count; i++)
		{
			KAnimControllerBase controller = this.data[i].controller;
			if (controller != null)
			{
				Vector3 position = controller.transform.GetPosition();
				if (visibleArea.Min <= position && position <= visibleArea.Max)
				{
					this.data[i].Update();
				}
			}
		}
	}

	// Token: 0x06001B91 RID: 7057 RVA: 0x0009395C File Offset: 0x00091B5C
	public void ClearOverlayColour()
	{
		Color32 c = Color.black;
		for (int i = 0; i < this.data.Count; i++)
		{
			KAnimControllerBase controller = this.data[i].controller;
			if (controller != null)
			{
				controller.OverlayColour = c;
			}
		}
	}

	// Token: 0x06001B92 RID: 7058 RVA: 0x000939B1 File Offset: 0x00091BB1
	public static void ClearOverlayColour(KBatchedAnimController controller)
	{
		if (controller != null)
		{
			controller.OverlayColour = Color.black;
		}
	}
}
