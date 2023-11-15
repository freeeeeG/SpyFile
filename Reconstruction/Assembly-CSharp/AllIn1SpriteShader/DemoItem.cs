using System;
using UnityEngine;

namespace AllIn1SpriteShader
{
	// Token: 0x020002C9 RID: 713
	public class DemoItem : MonoBehaviour
	{
		// Token: 0x0600115B RID: 4443 RVA: 0x00031CBA File Offset: 0x0002FEBA
		private void Update()
		{
			base.transform.LookAt(base.transform.position + DemoItem.lookAtZ);
		}

		// Token: 0x040009B2 RID: 2482
		private static Vector3 lookAtZ = new Vector3(0f, 0f, 1f);
	}
}
