using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000056 RID: 86
public class MB3_DisableHiddenAnimations : MonoBehaviour
{
	// Token: 0x0600023D RID: 573 RVA: 0x00019F1B File Offset: 0x0001831B
	private void Start()
	{
		if (base.GetComponent<SkinnedMeshRenderer>() == null)
		{
			Debug.LogError("The MB3_CullHiddenAnimations script was placed on and object " + base.name + " which has no SkinnedMeshRenderer attached");
		}
	}

	// Token: 0x0600023E RID: 574 RVA: 0x00019F48 File Offset: 0x00018348
	private void OnBecameVisible()
	{
		for (int i = 0; i < this.animationsToCull.Count; i++)
		{
			if (this.animationsToCull[i] != null)
			{
				this.animationsToCull[i].enabled = true;
			}
		}
	}

	// Token: 0x0600023F RID: 575 RVA: 0x00019F9C File Offset: 0x0001839C
	private void OnBecameInvisible()
	{
		for (int i = 0; i < this.animationsToCull.Count; i++)
		{
			if (this.animationsToCull[i] != null)
			{
				this.animationsToCull[i].enabled = false;
			}
		}
	}

	// Token: 0x04000185 RID: 389
	public List<Animation> animationsToCull = new List<Animation>();
}
