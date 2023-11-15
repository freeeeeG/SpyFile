using System;
using UnityEngine;

// Token: 0x02000784 RID: 1924
[AddComponentMenu("Scripts/Game/Flow/Tutorials/Soup Tutorial")]
public class SoupTutorial : IconTutorialBase
{
	// Token: 0x04001CAD RID: 7341
	[SerializeField]
	private Sprite m_potIcon;

	// Token: 0x04001CAE RID: 7342
	[SerializeField]
	private Sprite m_soupIcon;

	// Token: 0x04001CAF RID: 7343
	[SerializeField]
	private IngredientOrderNode m_onion;

	// Token: 0x04001CB0 RID: 7344
	[SerializeField]
	private CookedCompositeOrderNode m_onionSoup;

	// Token: 0x04001CB1 RID: 7345
	[SerializeField]
	private GameObject m_cleanPlatesTutorialUI;
}
