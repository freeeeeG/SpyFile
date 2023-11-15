using System;
using UnityEngine;

// Token: 0x0200047B RID: 1147
public class CookingHandler : MonoBehaviour
{
	// Token: 0x06001541 RID: 5441 RVA: 0x00073D7D File Offset: 0x0007217D
	public CookedCompositeOrderNode.CookingProgress GetCookedOrderState(float _cookingProgress)
	{
		if (_cookingProgress > 2f * this.m_cookingtime)
		{
			return CookedCompositeOrderNode.CookingProgress.Burnt;
		}
		if (_cookingProgress > this.m_cookingtime)
		{
			return CookedCompositeOrderNode.CookingProgress.Cooked;
		}
		return CookedCompositeOrderNode.CookingProgress.Raw;
	}

	// Token: 0x0400104E RID: 4174
	[SerializeField]
	public float m_cookingtime = 10f;

	// Token: 0x0400104F RID: 4175
	[SerializeField]
	public CookingStepData m_cookingType;

	// Token: 0x04001050 RID: 4176
	[SerializeField]
	public CookingStationType m_stationType;

	// Token: 0x04001051 RID: 4177
	[SerializeField]
	public CookingUIController m_cookingUIPrefab;

	// Token: 0x04001052 RID: 4178
	[SerializeField]
	public Vector3 m_cookingUIPrefabOffset = Vector3.zero;
}
