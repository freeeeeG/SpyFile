using System;
using Singletons;
using UnityEngine;

namespace FX
{
	// Token: 0x02000250 RID: 592
	public class ScreenEffectSpawner : Singleton<ScreenEffectSpawner>
	{
		// Token: 0x06000B9B RID: 2971 RVA: 0x0001FFBC File Offset: 0x0001E1BC
		private void Update()
		{
			float zoom = this._cameraController.zoom;
			if (zoom == this._cachedZoom)
			{
				return;
			}
			this._cachedZoom = zoom;
			base.transform.localScale = Vector3.one * zoom;
		}

		// Token: 0x06000B9C RID: 2972 RVA: 0x0001FFFC File Offset: 0x0001E1FC
		public void Spawn(EffectInfo effect, Vector2 offset)
		{
			Vector3 position = base.transform.position;
			position.x += offset.x;
			position.y += offset.y;
			position.z = 0f;
			EffectPoolInstance effectPoolInstance = effect.Spawn(position, 0f, 1f);
			effectPoolInstance.transform.parent = base.transform;
			Vector3 localScale = Vector3.one * effect.scale.value;
			float value = effect.scaleX.value;
			if (value > 0f)
			{
				localScale.x *= value;
			}
			float value2 = effect.scaleY.value;
			if (value2 > 0f)
			{
				localScale.y *= value2;
			}
			effectPoolInstance.transform.localScale = localScale;
		}

		// Token: 0x040009A5 RID: 2469
		[SerializeField]
		private CameraController _cameraController;

		// Token: 0x040009A6 RID: 2470
		private float _cachedZoom;
	}
}
