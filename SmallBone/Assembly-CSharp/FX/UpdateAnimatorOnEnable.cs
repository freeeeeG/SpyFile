using System;
using UnityEngine;

namespace FX
{
	// Token: 0x0200025F RID: 607
	public class UpdateAnimatorOnEnable : MonoBehaviour
	{
		// Token: 0x06000BED RID: 3053 RVA: 0x00020BFA File Offset: 0x0001EDFA
		private void OnEnable()
		{
			this._animator.Update(this._deltaTime.value);
		}

		// Token: 0x040009F2 RID: 2546
		[SerializeField]
		private Animator _animator;

		// Token: 0x040009F3 RID: 2547
		[SerializeField]
		private CustomFloat _deltaTime;
	}
}
