using System;
using UnityEngine;

// Token: 0x020005AD RID: 1453
[RequireComponent(typeof(Interactable))]
public class Terminal : SessionInteractable
{
	// Token: 0x040015B3 RID: 5555
	[SerializeField]
	public PilotMovement m_pilotableObject;
}
