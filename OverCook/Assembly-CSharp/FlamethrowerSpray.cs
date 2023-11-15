using System;
using UnityEngine;

// Token: 0x0200060B RID: 1547
public class FlamethrowerSpray : SprayingUtensil
{
	// Token: 0x040016BD RID: 5821
	[SerializeField]
	public float m_cookingRate = 2f;

	// Token: 0x040016BE RID: 5822
	[SerializeField]
	public float m_smoulderTime = 5f;

	// Token: 0x040016BF RID: 5823
	[SerializeField]
	public GameObject m_smoulderEffect;
}
