using System;
using UnityEngine;

// Token: 0x020004D5 RID: 1237
[AddComponentMenu("Scripts/Game/Environment/Interactable")]
public class Interactable : MonoBehaviour
{
	// Token: 0x04001116 RID: 4374
	[SerializeField]
	public string m_onInteractStartedTrigger = string.Empty;

	// Token: 0x04001117 RID: 4375
	[SerializeField]
	public string m_onInteractEndedTrigger = string.Empty;

	// Token: 0x04001118 RID: 4376
	[SerializeField]
	public string m_onInteractImpulseTrigger = string.Empty;

	// Token: 0x04001119 RID: 4377
	[SerializeField]
	public bool m_allowMultipleInteracters;

	// Token: 0x0400111A RID: 4378
	[SerializeField]
	public bool m_usePlacementButton;
}
