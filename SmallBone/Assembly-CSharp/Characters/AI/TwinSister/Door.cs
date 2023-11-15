using System;
using UnityEngine;

namespace Characters.AI.TwinSister
{
	// Token: 0x0200117B RID: 4475
	public class Door : MonoBehaviour
	{
		// Token: 0x060057A3 RID: 22435 RVA: 0x001047F4 File Offset: 0x001029F4
		public void Open()
		{
			this._animator.Play("open");
		}

		// Token: 0x060057A4 RID: 22436 RVA: 0x00104806 File Offset: 0x00102A06
		public void Close()
		{
			this._animator.Play("close");
		}

		// Token: 0x0400468D RID: 18061
		[SerializeField]
		private Animator _animator;
	}
}
