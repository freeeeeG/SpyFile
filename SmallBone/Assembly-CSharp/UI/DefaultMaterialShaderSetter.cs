using System;
using UnityEngine;

namespace UI
{
	// Token: 0x02000398 RID: 920
	public class DefaultMaterialShaderSetter : MonoBehaviour
	{
		// Token: 0x060010D9 RID: 4313 RVA: 0x00031D80 File Offset: 0x0002FF80
		private void Awake()
		{
			Canvas.GetDefaultCanvasMaterial().shader = this._shader;
		}

		// Token: 0x04000DCB RID: 3531
		[SerializeField]
		private Shader _shader;
	}
}
