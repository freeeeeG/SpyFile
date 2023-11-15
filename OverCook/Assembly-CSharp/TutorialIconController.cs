using System;
using UnityEngine;

// Token: 0x02000787 RID: 1927
[RequireComponent(typeof(Animator))]
public class TutorialIconController : IconTutorialBase
{
	// Token: 0x04001CD6 RID: 7382
	[SerializeField]
	public Transform m_plateStation;

	// Token: 0x04001CD7 RID: 7383
	[SerializeField]
	public PlateReturnStation m_plateReturn;

	// Token: 0x04001CD8 RID: 7384
	[SerializeField]
	public Transform[] m_choppingboards;

	// Token: 0x04001CD9 RID: 7385
	[SerializeField]
	public Transform m_lettuceCrate;

	// Token: 0x04001CDA RID: 7386
	[SerializeField]
	public IngredientOrderNode m_lettuce;

	// Token: 0x04001CDB RID: 7387
	[SerializeField]
	public Transform m_tomatoCrate;

	// Token: 0x04001CDC RID: 7388
	[SerializeField]
	public IngredientOrderNode m_tomato;

	// Token: 0x04001CDD RID: 7389
	[SerializeField]
	public Transform m_cucumberCrate;

	// Token: 0x04001CDE RID: 7390
	[SerializeField]
	public IngredientOrderNode m_cucumber;

	// Token: 0x04001CDF RID: 7391
	[SerializeField]
	public Sprite m_lettuceSaladIcon;

	// Token: 0x04001CE0 RID: 7392
	[SerializeField]
	public Sprite m_tomatoSaladIcon;

	// Token: 0x04001CE1 RID: 7393
	[SerializeField]
	public Sprite m_cucumberSaladIcon;

	// Token: 0x04001CE2 RID: 7394
	[SerializeField]
	[AssignComponent(Editorbility.NonEditable)]
	public Animator m_dialogueAnimator;

	// Token: 0x04001CE3 RID: 7395
	[SerializeField]
	public TutorialPopup m_swapTutorialUI = new TutorialPopup();

	// Token: 0x04001CE4 RID: 7396
	[SerializeField]
	public ControlPadInput.Button m_swapButton;

	// Token: 0x02000788 RID: 1928
	public enum TutorialStage
	{
		// Token: 0x04001CE6 RID: 7398
		Lettuce,
		// Token: 0x04001CE7 RID: 7399
		LettuceTomato,
		// Token: 0x04001CE8 RID: 7400
		LettuceTomatoCucumber
	}
}
