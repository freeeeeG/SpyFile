using System;
using UnityEngine;

// Token: 0x02000C75 RID: 3189
public class SwapUIAnimationController : MonoBehaviour
{
	// Token: 0x060065BB RID: 26043 RVA: 0x0025F08C File Offset: 0x0025D28C
	public void SetState(bool Primary)
	{
		this.AnimationControllerObject_Primary.SetActive(Primary);
		if (!Primary)
		{
			this.AnimationControllerObject_Alternate.GetComponent<KAnimControllerBase>().TintColour = new Color(1f, 1f, 1f, 0.5f);
			this.AnimationControllerObject_Primary.GetComponent<KAnimControllerBase>().TintColour = Color.clear;
		}
		this.AnimationControllerObject_Alternate.SetActive(!Primary);
		if (Primary)
		{
			this.AnimationControllerObject_Primary.GetComponent<KAnimControllerBase>().TintColour = Color.white;
			this.AnimationControllerObject_Alternate.GetComponent<KAnimControllerBase>().TintColour = Color.clear;
		}
	}

	// Token: 0x0400460A RID: 17930
	public GameObject AnimationControllerObject_Primary;

	// Token: 0x0400460B RID: 17931
	public GameObject AnimationControllerObject_Alternate;
}
