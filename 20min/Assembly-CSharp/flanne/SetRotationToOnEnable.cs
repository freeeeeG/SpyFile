using System;
using UnityEngine;

namespace flanne
{
	// Token: 0x0200010E RID: 270
	public class SetRotationToOnEnable : MonoBehaviour
	{
		// Token: 0x06000796 RID: 1942 RVA: 0x00020E7E File Offset: 0x0001F07E
		private void OnEnable()
		{
			base.transform.rotation = this.targetTransform.rotation;
		}

		// Token: 0x04000577 RID: 1399
		[SerializeField]
		private Transform targetTransform;
	}
}
