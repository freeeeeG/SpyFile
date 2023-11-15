using System;
using UnityEngine;

// Token: 0x02000571 RID: 1393
[AddComponentMenu("Scripts/Game/Environment/AnticipateInteractionHighlight")]
internal class AnticipateInteractionHighlight : MonoBehaviour
{
	// Token: 0x040014CE RID: 5326
	[SerializeField]
	public float m_brightnessModifier = 2f;

	// Token: 0x040014CF RID: 5327
	[SerializeField]
	public GameObject m_highlightObjectOverride;
}
