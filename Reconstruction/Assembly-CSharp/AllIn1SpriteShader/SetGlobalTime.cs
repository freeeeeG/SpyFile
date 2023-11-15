using System;
using UnityEngine;

namespace AllIn1SpriteShader
{
	// Token: 0x020002CF RID: 719
	[ExecuteInEditMode]
	public class SetGlobalTime : MonoBehaviour
	{
		// Token: 0x06001174 RID: 4468 RVA: 0x00032651 File Offset: 0x00030851
		private void Start()
		{
			this.globalTime = Shader.PropertyToID("globalUnscaledTime");
		}

		// Token: 0x06001175 RID: 4469 RVA: 0x00032663 File Offset: 0x00030863
		private void Update()
		{
			Shader.SetGlobalFloat(this.globalTime, Time.time / 20f);
		}

		// Token: 0x040009BF RID: 2495
		private int globalTime;
	}
}
