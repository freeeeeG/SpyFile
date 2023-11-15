using System;
using UnityEngine;

// Token: 0x02000530 RID: 1328
[RequireComponent(typeof(PickupItemSpawner))]
public class PickupItemSwitcher : MonoBehaviour
{
	// Token: 0x040013FA RID: 5114
	[SerializeField]
	public GameObject[] m_itemPrefabs;

	// Token: 0x040013FB RID: 5115
	[SerializeField]
	public string m_switchTrigger;
}
