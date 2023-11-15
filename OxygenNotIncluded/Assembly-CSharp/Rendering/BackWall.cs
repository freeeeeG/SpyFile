using System;
using UnityEngine;

namespace rendering
{
	// Token: 0x02000CBF RID: 3263
	public class BackWall : MonoBehaviour
	{
		// Token: 0x0600685E RID: 26718 RVA: 0x002779CF File Offset: 0x00275BCF
		private void Awake()
		{
			this.backwallMaterial.SetTexture("images", this.array);
		}

		// Token: 0x04004818 RID: 18456
		[SerializeField]
		public Material backwallMaterial;

		// Token: 0x04004819 RID: 18457
		[SerializeField]
		public Texture2DArray array;
	}
}
