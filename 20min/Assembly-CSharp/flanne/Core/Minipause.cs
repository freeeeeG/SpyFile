using System;
using UnityEngine;

namespace flanne.Core
{
	// Token: 0x02000200 RID: 512
	public class Minipause : MonoBehaviour
	{
		// Token: 0x06000B82 RID: 2946 RVA: 0x0002B26C File Offset: 0x0002946C
		private void Start()
		{
			this.PC = PauseController.SharedInstance;
			this._cdTimer = this.cd;
		}

		// Token: 0x06000B83 RID: 2947 RVA: 0x0002B285 File Offset: 0x00029485
		private void Update()
		{
			if (this.hasCooldown && this._cdTimer > 0f)
			{
				this._cdTimer -= Time.deltaTime;
			}
		}

		// Token: 0x06000B84 RID: 2948 RVA: 0x0002B2AE File Offset: 0x000294AE
		public void Pause(float duration)
		{
			if (this.hasCooldown)
			{
				if (this._cdTimer <= 0f)
				{
					this.PC.Pause(duration);
				}
				this._cdTimer = this.cd;
				return;
			}
			this.PC.Pause(duration);
		}

		// Token: 0x040007EB RID: 2027
		[SerializeField]
		private bool hasCooldown;

		// Token: 0x040007EC RID: 2028
		[SerializeField]
		private float cd;

		// Token: 0x040007ED RID: 2029
		private PauseController PC;

		// Token: 0x040007EE RID: 2030
		private float _cdTimer;
	}
}
