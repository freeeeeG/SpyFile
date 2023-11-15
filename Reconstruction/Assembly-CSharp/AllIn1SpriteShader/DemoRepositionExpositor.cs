using System;
using UnityEngine;

namespace AllIn1SpriteShader
{
	// Token: 0x020002CB RID: 715
	public class DemoRepositionExpositor : MonoBehaviour
	{
		// Token: 0x06001162 RID: 4450 RVA: 0x00031DF0 File Offset: 0x0002FFF0
		[ContextMenu("RepositionExpositor")]
		private void RepositionExpositor()
		{
			int num = 0;
			Vector3 zero = Vector3.zero;
			foreach (object obj in base.transform)
			{
				Transform transform = (Transform)obj;
				zero.x = (float)num * this.paddingX;
				transform.localPosition = zero;
				num++;
			}
		}

		// Token: 0x040009B4 RID: 2484
		[SerializeField]
		private float paddingX = 10f;
	}
}
