using System;
using UnityEngine;

// Token: 0x0200057B RID: 1403
[RequireComponent(typeof(TabletopConveyenceReceiver))]
[RequireComponent(typeof(StaticGridLocation))]
[RequireComponent(typeof(AttachStation))]
public class ConveyorStation : MonoBehaviour
{
	// Token: 0x0400150B RID: 5387
	[SerializeField]
	public float m_conveySpeed = 1f;

	// Token: 0x0400150C RID: 5388
	[SerializeField]
	public ConveyorStation.XZDirection m_conveyanceDirectionXZ = ConveyorStation.XZDirection.Rightwards;

	// Token: 0x0400150D RID: 5389
	public MeshRenderer m_targetRenderer;

	// Token: 0x0200057C RID: 1404
	public enum XZDirection
	{
		// Token: 0x0400150F RID: 5391
		Leftwards,
		// Token: 0x04001510 RID: 5392
		Rightwards
	}
}
