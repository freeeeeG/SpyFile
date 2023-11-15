using System;
using UnityEngine;

// Token: 0x02000554 RID: 1364
[AddComponentMenu("Scripts/Game/Environment/PlateStation")]
[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(AttachStation))]
public class PlateStation : MonoBehaviour
{
	// Token: 0x0400146B RID: 5227
	[SerializeField]
	[AssignResource("NeedsPlateFloatingUI", Editorbility.NonEditable)]
	public GameObject m_needsPlateFloatingUI;

	// Token: 0x0400146C RID: 5228
	[SerializeField]
	[AssignResource("TipsFloatingNumberUI", Editorbility.NonEditable)]
	public GameObject m_tipsFloatingNumberUI;

	// Token: 0x0400146D RID: 5229
	[SerializeField]
	public PlateReturnStation[] m_returnStations;

	// Token: 0x0400146E RID: 5230
	[SerializeField]
	public GameObject m_platePrefab;

	// Token: 0x0400146F RID: 5231
	[SerializeField]
	public float m_createPlateTime;

	// Token: 0x04001470 RID: 5232
	[SerializeField]
	public TeamID m_teamId;

	// Token: 0x04001471 RID: 5233
	[SerializeField]
	public string m_onFoodDeliveredTrigger;

	// Token: 0x04001472 RID: 5234
	[SerializeField]
	public PlateStation.DeliveryFX m_deliveryEffects = new PlateStation.DeliveryFX();

	// Token: 0x02000555 RID: 1365
	[Serializable]
	public class DeliveryFX
	{
		// Token: 0x04001473 RID: 5235
		public GameObject m_deliverPFXPrefab;

		// Token: 0x04001474 RID: 5236
		public float m_pfxToFadeDelayTime = 0.2f;

		// Token: 0x04001475 RID: 5237
		public Shader m_fadeOutShader;

		// Token: 0x04001476 RID: 5238
		public float m_fadeTime = 0.5f;
	}
}
