using System;
using Characters;
using Services;
using Singletons;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

namespace Runnables
{
	// Token: 0x02000309 RID: 777
	public sealed class ChangePlayerLightSetting : Runnable
	{
		// Token: 0x06000F34 RID: 3892 RVA: 0x0002E82C File Offset: 0x0002CA2C
		public override void Run()
		{
			if (this._light == null)
			{
				Character player = Singleton<Service>.Instance.levelManager.player;
				this._light = player.GetComponentInChildren<Light2D>();
			}
			this._cachedIntensity = this._light.intensity;
			this._light.intensity = this._intensity;
		}

		// Token: 0x06000F35 RID: 3893 RVA: 0x0002E885 File Offset: 0x0002CA85
		private void OnDestroy()
		{
			if (!this._rollbackOnDestroyed)
			{
				return;
			}
			this._light.intensity = this._cachedIntensity;
		}

		// Token: 0x04000C87 RID: 3207
		private Light2D _light;

		// Token: 0x04000C88 RID: 3208
		[SerializeField]
		private bool _rollbackOnDestroyed;

		// Token: 0x04000C89 RID: 3209
		[SerializeField]
		private float _intensity;

		// Token: 0x04000C8A RID: 3210
		private float _cachedIntensity;
	}
}
