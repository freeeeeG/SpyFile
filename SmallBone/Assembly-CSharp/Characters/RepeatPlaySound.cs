using System;
using System.Collections;
using FX;
using Singletons;
using UnityEngine;

namespace Characters
{
	// Token: 0x02000716 RID: 1814
	public class RepeatPlaySound : MonoBehaviour
	{
		// Token: 0x170007B4 RID: 1972
		// (get) Token: 0x060024BB RID: 9403 RVA: 0x0006E837 File Offset: 0x0006CA37
		private float interval
		{
			get
			{
				return UnityEngine.Random.Range(this._interval.x, this._interval.y);
			}
		}

		// Token: 0x060024BC RID: 9404 RVA: 0x0006E854 File Offset: 0x0006CA54
		public void Play()
		{
			this._coroutine = base.StartCoroutine(this.CLoop());
		}

		// Token: 0x060024BD RID: 9405 RVA: 0x0006E868 File Offset: 0x0006CA68
		public void Stop()
		{
			ReusableAudioSource reusableAudioSource = this._reusableAudioSource;
			if (reusableAudioSource != null)
			{
				reusableAudioSource.Stop();
			}
			if (this._coroutine != null)
			{
				base.StopCoroutine(this._coroutine);
			}
		}

		// Token: 0x060024BE RID: 9406 RVA: 0x0006E88F File Offset: 0x0006CA8F
		private void Start()
		{
			if (this._playOnAwake)
			{
				this.Play();
			}
		}

		// Token: 0x060024BF RID: 9407 RVA: 0x0006E89F File Offset: 0x0006CA9F
		private IEnumerator CLoop()
		{
			yield return Chronometer.global.WaitForSeconds(this._startDelay);
			for (;;)
			{
				this._reusableAudioSource = PersistentSingleton<SoundManager>.Instance.PlaySound(this._audioClipInfo, (this._position == null) ? base.transform.position : this._position.position);
				yield return Chronometer.global.WaitForSeconds(this.interval);
			}
			yield break;
		}

		// Token: 0x060024C0 RID: 9408 RVA: 0x0006E8AE File Offset: 0x0006CAAE
		private void OnDisable()
		{
			this.Stop();
		}

		// Token: 0x060024C1 RID: 9409 RVA: 0x0006E8AE File Offset: 0x0006CAAE
		private void OnDestroy()
		{
			this.Stop();
		}

		// Token: 0x04001F35 RID: 7989
		[SerializeField]
		private float _startDelay;

		// Token: 0x04001F36 RID: 7990
		[SerializeField]
		private Vector2 _interval;

		// Token: 0x04001F37 RID: 7991
		[SerializeField]
		private bool _playOnAwake = true;

		// Token: 0x04001F38 RID: 7992
		[SerializeField]
		private SoundInfo _audioClipInfo;

		// Token: 0x04001F39 RID: 7993
		[SerializeField]
		private Transform _position;

		// Token: 0x04001F3A RID: 7994
		private ReusableAudioSource _reusableAudioSource;

		// Token: 0x04001F3B RID: 7995
		private Coroutine _coroutine;

		// Token: 0x04001F3C RID: 7996
		private float _elapsed;
	}
}
