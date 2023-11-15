using System;
using System.Collections;
using Characters;
using Services;
using Singletons;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

namespace Level
{
	// Token: 0x0200051A RID: 1306
	public class RewardLight : MonoBehaviour
	{
		// Token: 0x060019B8 RID: 6584 RVA: 0x00050A50 File Offset: 0x0004EC50
		private void Awake()
		{
			this._player = Singleton<Service>.Instance.levelManager.player;
			this._defaultIntensity = this._map.globalLight.intensity;
			this._defaultGodRayIntensity = this._godRay.intensity;
			this._godRay.intensity = 0f;
			this._godRay.gameObject.SetActive(false);
		}

		// Token: 0x060019B9 RID: 6585 RVA: 0x00050ABC File Offset: 0x0004ECBC
		private void OnTriggerEnter2D(Collider2D collision)
		{
			Character component = collision.GetComponent<Character>();
			if (component == null)
			{
				return;
			}
			if (component != this._player)
			{
				return;
			}
			this.Activate();
		}

		// Token: 0x060019BA RID: 6586 RVA: 0x00050AF0 File Offset: 0x0004ECF0
		private void OnTriggerExit2D(Collider2D collision)
		{
			Character component = collision.GetComponent<Character>();
			if (component == null)
			{
				return;
			}
			if (component != this._player)
			{
				return;
			}
			this.Deactivate();
		}

		// Token: 0x060019BB RID: 6587 RVA: 0x00050B24 File Offset: 0x0004ED24
		private void Activate()
		{
			if (this._fade != null)
			{
				base.StopCoroutine(this._fade);
			}
			this._godRay.gameObject.SetActive(true);
			this._fade = base.StartCoroutine(this.CFade(this._defaultIntensity * this._intensityMultifly, this._defaultGodRayIntensity, this._fadeDuration));
		}

		// Token: 0x060019BC RID: 6588 RVA: 0x00050B81 File Offset: 0x0004ED81
		private void Deactivate()
		{
			if (this._fade != null)
			{
				base.StopCoroutine(this._fade);
			}
			this._fade = base.StartCoroutine(this.CFade(this._defaultIntensity, 0f, this._fadeDuration));
		}

		// Token: 0x060019BD RID: 6589 RVA: 0x00050BBA File Offset: 0x0004EDBA
		private IEnumerator CFade(float targetIntensity, float targetGodRayIntensity, float duration)
		{
			while (this._map.waveContainer.state == EnemyWaveContainer.State.Remain)
			{
				yield return null;
			}
			float start = Time.time;
			float defaultIntensity = this._map.globalLight.intensity;
			float defaultGodRayIntensity = this._godRay.intensity;
			while (Time.time - start < duration)
			{
				yield return null;
				float t = (Time.time - start) / duration;
				this._map.globalLight.intensity = Mathf.Lerp(defaultIntensity, targetIntensity, t);
				this._godRay.intensity = Mathf.Lerp(defaultGodRayIntensity, targetGodRayIntensity, t);
			}
			this._map.globalLight.intensity = targetIntensity;
			this._godRay.intensity = targetGodRayIntensity;
			if (targetGodRayIntensity <= 0f)
			{
				this._godRay.gameObject.SetActive(false);
			}
			yield break;
		}

		// Token: 0x04001684 RID: 5764
		[SerializeField]
		[GetComponentInParent(false)]
		private Map _map;

		// Token: 0x04001685 RID: 5765
		[SerializeField]
		private float _intensityMultifly = 0.7f;

		// Token: 0x04001686 RID: 5766
		[SerializeField]
		private Light2D _godRay;

		// Token: 0x04001687 RID: 5767
		[SerializeField]
		private float _fadeDuration;

		// Token: 0x04001688 RID: 5768
		private Character _player;

		// Token: 0x04001689 RID: 5769
		private Coroutine _fade;

		// Token: 0x0400168A RID: 5770
		private float _defaultIntensity;

		// Token: 0x0400168B RID: 5771
		private float _defaultGodRayIntensity;
	}
}
