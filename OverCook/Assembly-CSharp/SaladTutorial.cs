using System;
using UnityEngine;

// Token: 0x02000782 RID: 1922
[AddComponentMenu("Scripts/Game/Flow/Tutorials/Salad Tutorial")]
public class SaladTutorial : IconTutorialBase
{
	// Token: 0x04001CA3 RID: 7331
	[SerializeField]
	protected Transform m_tomatoCrate;

	// Token: 0x04001CA4 RID: 7332
	[SerializeField]
	protected Transform m_lettuceCrate;

	// Token: 0x04001CA5 RID: 7333
	[SerializeField]
	protected Transform[] m_choppingboards;

	// Token: 0x04001CA6 RID: 7334
	[SerializeField]
	protected Transform m_plate;

	// Token: 0x04001CA7 RID: 7335
	[SerializeField]
	protected Transform m_plateStation;

	// Token: 0x04001CA8 RID: 7336
	[SerializeField]
	protected IngredientOrderNode m_tomato;

	// Token: 0x04001CA9 RID: 7337
	[SerializeField]
	protected IngredientOrderNode m_lettuce;

	// Token: 0x04001CAA RID: 7338
	[SerializeField]
	protected Sprite m_saladIcon;

	// Token: 0x04001CAB RID: 7339
	[SerializeField]
	[AssignComponent(Editorbility.NonEditable)]
	private Animator m_dialogueAnimator;
}
