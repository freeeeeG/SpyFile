using System;
using UnityEngine;

// Token: 0x02000406 RID: 1030
[RequireComponent(typeof(Interactable))]
public class ToggleSwitchCosmeticDecisions : MonoBehaviour
{
	// Token: 0x04000EAF RID: 3759
	[SerializeField]
	[AssignChildComponent(Editorbility.NonEditable)]
	public Animator m_animator;

	// Token: 0x04000EB0 RID: 3760
	[SerializeField]
	[ReadOnly]
	public string m_stateParameter = "On";
}
