using System;
using UnityEngine;

// Token: 0x0200074D RID: 1869
[CreateAssetMenu(fileName = "StatValidationList", menuName = "Team17/Create StatValidationList")]
[Serializable]
public class StatValidationList : ScriptableObject
{
	// Token: 0x04001B72 RID: 7026
	[SerializeField]
	public int[] m_ids;
}
