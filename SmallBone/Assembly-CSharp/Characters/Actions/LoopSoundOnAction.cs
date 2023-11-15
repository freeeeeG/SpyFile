using System;
using System.Collections;
using Characters.Operations.Fx;
using UnityEditor;
using UnityEngine;

namespace Characters.Actions
{
	// Token: 0x02000941 RID: 2369
	[RequireComponent(typeof(Action))]
	public class LoopSoundOnAction : MonoBehaviour
	{
		// Token: 0x060032E3 RID: 13027 RVA: 0x00097343 File Offset: 0x00095543
		private void Awake()
		{
			this._sound.Initialize();
			this._action.onStart += this.StartLoop;
		}

		// Token: 0x060032E4 RID: 13028 RVA: 0x00097367 File Offset: 0x00095567
		private void OnDisable()
		{
			if (this._playLoopSound != null)
			{
				base.StopCoroutine(this._playLoopSound);
				this.StopLoop();
			}
		}

		// Token: 0x060032E5 RID: 13029 RVA: 0x00097383 File Offset: 0x00095583
		private void StartLoop()
		{
			if (this._running)
			{
				return;
			}
			if (this._playLoopSound != null)
			{
				base.StopCoroutine(this._playLoopSound);
			}
			this._running = true;
			this._playLoopSound = base.StartCoroutine(this.CPlayLoopSound());
		}

		// Token: 0x060032E6 RID: 13030 RVA: 0x000973BB File Offset: 0x000955BB
		private void StopLoop()
		{
			this._sound.Stop();
			this._running = false;
		}

		// Token: 0x060032E7 RID: 13031 RVA: 0x000973CF File Offset: 0x000955CF
		private IEnumerator CPlayLoopSound()
		{
			float delay = 0f;
			float time = Time.time;
			while (this._action.running)
			{
				if (delay <= 0f)
				{
					this._sound.Run(this._action.owner);
					delay += this._repeatInterval;
				}
				delay -= Time.time - time;
				time = Time.time;
				yield return null;
			}
			this.StopLoop();
			yield break;
		}

		// Token: 0x0400297A RID: 10618
		[GetComponent]
		[SerializeField]
		private Action _action;

		// Token: 0x0400297B RID: 10619
		[Subcomponent(typeof(PlaySound))]
		[SerializeField]
		private PlaySound _sound;

		// Token: 0x0400297C RID: 10620
		[Tooltip("사운드를 반복 재생하는 인터벌 값입니다.\n실제 값 보다 0.05~0.1초 가량 더 적게 설정하면 자연스럽게 Loop 됩니다.")]
		[SerializeField]
		private float _repeatInterval = 1f;

		// Token: 0x0400297D RID: 10621
		private bool _running;

		// Token: 0x0400297E RID: 10622
		private Coroutine _playLoopSound;
	}
}
