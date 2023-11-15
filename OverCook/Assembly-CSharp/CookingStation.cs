using System;
using UnityEngine;

// Token: 0x02000463 RID: 1123
[ExecutionDependency(typeof(IFlowController))]
[AddComponentMenu("Scripts/Game/Environment/CookingStation")]
[RequireComponent(typeof(AttachStation))]
public class CookingStation : MonoBehaviour
{
	// Token: 0x060014E4 RID: 5348 RVA: 0x000721B2 File Offset: 0x000705B2
	public void SetCookerOn(bool _isOn)
	{
		if (this.m_flameEffect != null)
		{
			this.m_flameEffect.SetActive(_isOn);
		}
	}

	// Token: 0x060014E5 RID: 5349 RVA: 0x000721D4 File Offset: 0x000705D4
	public virtual bool CanAddItem(GameObject _object, MixedCompositeOrderNode.MixingProgress? mixingProgress)
	{
		if (!this.m_attachRestrictions)
		{
			return true;
		}
		IBaseCookable baseCookable = _object.RequestInterface<IBaseCookable>();
		if (baseCookable == null)
		{
			return false;
		}
		if (baseCookable.GetRequiredStationType() != this.m_stationType)
		{
			return false;
		}
		IIngredientContents ingredientContents = _object.RequestInterface<IIngredientContents>();
		return (ingredientContents != null && !ingredientContents.HasContents()) || mixingProgress == null || mixingProgress.Value == MixedCompositeOrderNode.MixingProgress.Mixed;
	}

	// Token: 0x04001014 RID: 4116
	[SerializeField]
	public GameObject m_flameEffect;

	// Token: 0x04001015 RID: 4117
	[SerializeField]
	public CookingStationType m_stationType;

	// Token: 0x04001016 RID: 4118
	[SerializeField]
	public Collider m_itemBlock;

	// Token: 0x04001017 RID: 4119
	[SerializeField]
	public bool m_attachRestrictions = true;
}
