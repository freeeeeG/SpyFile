using System;
using UnityEngine;

namespace FlatKit
{
	// Token: 0x02000051 RID: 81
	public class UvScroller : MonoBehaviour
	{
		// Token: 0x060000DE RID: 222 RVA: 0x000040F0 File Offset: 0x000022F0
		private void Start()
		{
			this.offset = this.targetMaterial.mainTextureOffset;
			this.initOffset = this.targetMaterial.mainTextureOffset;
		}

		// Token: 0x060000DF RID: 223 RVA: 0x00004114 File Offset: 0x00002314
		private void OnDisable()
		{
			this.targetMaterial.mainTextureOffset = this.initOffset;
		}

		// Token: 0x060000E0 RID: 224 RVA: 0x00004128 File Offset: 0x00002328
		private void Update()
		{
			this.offset.x = this.offset.x + this.speedX * Time.deltaTime;
			this.offset.y = this.offset.y + this.speedY * Time.deltaTime;
			this.targetMaterial.mainTextureOffset = this.offset;
		}

		// Token: 0x04000096 RID: 150
		public Material targetMaterial;

		// Token: 0x04000097 RID: 151
		public float speedX;

		// Token: 0x04000098 RID: 152
		public float speedY;

		// Token: 0x04000099 RID: 153
		private Vector2 offset;

		// Token: 0x0400009A RID: 154
		private Vector2 initOffset;
	}
}
