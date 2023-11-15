using System;
using System.Collections;
using Characters.Abilities.Customs;
using Characters.Actions;
using FX;
using Singletons;
using UnityEngine;

namespace Characters.Gear.Weapons.Rockstar
{
	// Token: 0x02000842 RID: 2114
	public class RockstarMusic : MonoBehaviour
	{
		// Token: 0x1700091D RID: 2333
		// (get) Token: 0x06002BFB RID: 11259 RVA: 0x00086D0C File Offset: 0x00084F0C
		private float soloBeat
		{
			get
			{
				return 60f / this._soloBpm;
			}
		}

		// Token: 0x1700091E RID: 2334
		// (get) Token: 0x06002BFC RID: 11260 RVA: 0x00086D1A File Offset: 0x00084F1A
		private float passiveBeat
		{
			get
			{
				return 60f / this._passiveBpm;
			}
		}

		// Token: 0x1700091F RID: 2335
		// (get) Token: 0x06002BFD RID: 11261 RVA: 0x00086D28 File Offset: 0x00084F28
		private bool pause
		{
			get
			{
				return Chronometer.global.timeScale <= 0f;
			}
		}

		// Token: 0x17000920 RID: 2336
		// (set) Token: 0x06002BFE RID: 11262 RVA: 0x00086D40 File Offset: 0x00084F40
		private float currentBpm
		{
			set
			{
				RockstarMusic.AmpMusic[] array = this.amps;
				for (int i = 0; i < array.Length; i++)
				{
					array[i].currentBpm = value;
				}
			}
		}

		// Token: 0x17000921 RID: 2337
		// (set) Token: 0x06002BFF RID: 11263 RVA: 0x00086D6C File Offset: 0x00084F6C
		private float beatStart
		{
			set
			{
				RockstarMusic.AmpMusic[] array = this.amps;
				for (int i = 0; i < array.Length; i++)
				{
					array[i].beatStart = value;
				}
			}
		}

		// Token: 0x17000922 RID: 2338
		// (get) Token: 0x06002C00 RID: 11264 RVA: 0x00086D98 File Offset: 0x00084F98
		private bool soundActionRunning
		{
			get
			{
				for (int i = 0; i < this._soundActions.Length; i++)
				{
					if (this._soundActions[i].running)
					{
						return true;
					}
				}
				return false;
			}
		}

		// Token: 0x17000923 RID: 2339
		// (get) Token: 0x06002C01 RID: 11265 RVA: 0x00086DCA File Offset: 0x00084FCA
		private bool passiveRunning
		{
			get
			{
				return Time.time < this._passiveStart + this._passiveIntro + this._passiveBeatDuration * this.passiveBeat;
			}
		}

		// Token: 0x06002C02 RID: 11266 RVA: 0x00086DF0 File Offset: 0x00084FF0
		private void Awake()
		{
			this._maxVolume = this._solo.volume;
			this.currentBpm = this._soloBpm;
			for (int i = 0; i < this._soundStartActions.Length; i++)
			{
				this._soundStartActions[i].onStart += this.StartSolo;
			}
			RockstarPassive rockstarPassive = (RockstarPassive)this._passive.ability;
			rockstarPassive.onSummon = (System.Action)Delegate.Combine(rockstarPassive.onSummon, new System.Action(this.StartPassive));
			RockstarMusic.AmpMusic[] array = this.amps;
			for (int j = 0; j < array.Length; j++)
			{
				array[j].Initialize(this);
			}
		}

		// Token: 0x06002C03 RID: 11267 RVA: 0x00086E98 File Offset: 0x00085098
		private void OnDisable()
		{
			this.SetBeat(this._soloBpm, 0f);
			this.StopSolo();
			RockstarMusic.AmpMusic[] array = this.amps;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].StopAmp();
			}
		}

		// Token: 0x06002C04 RID: 11268 RVA: 0x00086ED9 File Offset: 0x000850D9
		private void PlaySolo()
		{
			this._soloSource = PersistentSingleton<SoundManager>.Instance.PlaySound(this._solo, base.transform.position);
		}

		// Token: 0x06002C05 RID: 11269 RVA: 0x00086EFC File Offset: 0x000850FC
		private void SetSoloVolume(float volume)
		{
			if (this._soloSource == null)
			{
				return;
			}
			this._soloSource.audioSource.volume = volume * this._maxVolume * PersistentSingleton<SoundManager>.Instance.masterVolume * PersistentSingleton<SoundManager>.Instance.sfxVolume;
		}

		// Token: 0x06002C06 RID: 11270 RVA: 0x00086F3B File Offset: 0x0008513B
		private void StartSolo()
		{
			if (this._running)
			{
				return;
			}
			if (this.passiveRunning)
			{
				return;
			}
			this.SetBeat(this._soloBpm, 0f);
			this._loopSound = base.StartCoroutine(this.CLoopSoundWhileAction());
		}

		// Token: 0x06002C07 RID: 11271 RVA: 0x00086F72 File Offset: 0x00085172
		private void StopSolo()
		{
			this._running = false;
			ReusableAudioSource soloSource = this._soloSource;
			if (soloSource == null)
			{
				return;
			}
			soloSource.Stop();
		}

		// Token: 0x06002C08 RID: 11272 RVA: 0x00086F8B File Offset: 0x0008518B
		private void StartPassive()
		{
			this._passiveStart = Time.time;
			this.SetBeat(this._passiveBpm, this._passiveIntro);
			base.StartCoroutine(this.CWaitForPassiveFinish());
		}

		// Token: 0x06002C09 RID: 11273 RVA: 0x00086FB7 File Offset: 0x000851B7
		private void SetBeat(float bpm, float beatInterval = 0f)
		{
			this.currentBpm = bpm;
			this.beatStart = Time.time + beatInterval;
		}

		// Token: 0x06002C0A RID: 11274 RVA: 0x00086FCD File Offset: 0x000851CD
		private IEnumerator CLoopSoundWhileAction()
		{
			float repeatDelay = 0f;
			float fadeStart = 0f;
			this._running = true;
			while ((this.soundActionRunning && !this.passiveRunning) || Time.time - fadeStart < this._fadeTime)
			{
				if (this.pause)
				{
					if (!this._paused)
					{
						this._paused = true;
						this._soloSource.Stop();
					}
					yield return null;
				}
				else
				{
					if (this._paused)
					{
						this._paused = false;
					}
					repeatDelay -= Time.deltaTime;
					if (repeatDelay <= 0f)
					{
						this.PlaySolo();
						this.SetSoloVolume(1f);
						repeatDelay += this._intervalBeat * this.soloBeat;
					}
					if (this.soundActionRunning && !this.passiveRunning)
					{
						this.SetSoloVolume(1f);
						fadeStart = Time.time;
					}
					else
					{
						this.SetSoloVolume(Mathf.Lerp(1f, 0f, (Time.time - fadeStart) / this._fadeTime));
					}
					yield return null;
				}
			}
			this.StopSolo();
			yield break;
		}

		// Token: 0x06002C0B RID: 11275 RVA: 0x00086FDC File Offset: 0x000851DC
		private IEnumerator CWaitForPassiveFinish()
		{
			while (this.passiveRunning)
			{
				yield return null;
			}
			this.SetBeat(this._soloBpm, 0f);
			if (this.soundActionRunning)
			{
				this.StartSolo();
			}
			yield break;
		}

		// Token: 0x0400252D RID: 9517
		[GetComponentInParent(false)]
		[SerializeField]
		private Weapon _weapon;

		// Token: 0x0400252E RID: 9518
		[SerializeField]
		[Header("Solo")]
		private SoundInfo _solo;

		// Token: 0x0400252F RID: 9519
		private ReusableAudioSource _soloSource;

		// Token: 0x04002530 RID: 9520
		private float _maxVolume;

		// Token: 0x04002531 RID: 9521
		[SerializeField]
		[Tooltip("기본 공격 사운드의 BPM 값을 넣어주세요.")]
		private float _soloBpm = 95f;

		// Token: 0x04002532 RID: 9522
		[SerializeField]
		[Tooltip("이 액션으로만 Solo 사운드가 시작됩니다.")]
		private Characters.Actions.Action[] _soundStartActions;

		// Token: 0x04002533 RID: 9523
		[SerializeField]
		[Tooltip("패시브 발동 중이 아닐 때, 이 액션 중 단 하나라도 사용중이라면 Solo 사운드가 계속 재생됩니다.")]
		private Characters.Actions.Action[] _soundActions;

		// Token: 0x04002534 RID: 9524
		[SerializeField]
		[Tooltip("Solo 사운드를 반복 재생하는 박자 길이입니다.\nBPM으로 입력한 값에 맞춰 실제 길이가 결정됩니다.")]
		private float _intervalBeat = 8f;

		// Token: 0x04002535 RID: 9525
		[Tooltip("Solo 사운드는 어떤 Sound Action도 사용중이지 않을 때 이 시간 만큼 페이드 되었다가 중지됩니다.")]
		[SerializeField]
		private float _fadeTime = 0.6f;

		// Token: 0x04002536 RID: 9526
		[Space]
		[Header("Passive")]
		[SerializeField]
		[GetComponentInParent(false)]
		private RockstarPassiveComponent _passive;

		// Token: 0x04002537 RID: 9527
		[Tooltip("패시브 사운드의 BPM 값을 넣어주세요.")]
		[SerializeField]
		private float _passiveBpm = 185f;

		// Token: 0x04002538 RID: 9528
		[SerializeField]
		[Tooltip("패시브 사운드의 첫 박자가 시작되기 까지의 시간입니다.")]
		private float _passiveIntro = 0.5f;

		// Token: 0x04002539 RID: 9529
		[SerializeField]
		[Tooltip("패시브로 인해 사운드가 재생되는 박자 길이입니다.\n패시브 발동 이후 이 시간 동안 Solo 사운드가 들리지 않습니다.")]
		private float _passiveBeatDuration = 32f;

		// Token: 0x0400253A RID: 9530
		[Header("Amp")]
		[SerializeField]
		[Space]
		private RockstarMusic.AmpMusic[] amps;

		// Token: 0x0400253B RID: 9531
		private Coroutine _loopSound;

		// Token: 0x0400253C RID: 9532
		private Coroutine _waitPassive;

		// Token: 0x0400253D RID: 9533
		private Coroutine _ampPlayBeat;

		// Token: 0x0400253E RID: 9534
		private bool _running;

		// Token: 0x0400253F RID: 9535
		private bool _paused;

		// Token: 0x04002540 RID: 9536
		private float _passiveStart = -100f;

		// Token: 0x04002541 RID: 9537
		private float _soloVolume = 1f;

		// Token: 0x04002542 RID: 9538
		private bool _ampRunning;

		// Token: 0x02000843 RID: 2115
		[Serializable]
		private class AmpMusic
		{
			// Token: 0x17000924 RID: 2340
			// (get) Token: 0x06002C0D RID: 11277 RVA: 0x00086D28 File Offset: 0x00084F28
			private bool pause
			{
				get
				{
					return Chronometer.global.timeScale <= 0f;
				}
			}

			// Token: 0x17000925 RID: 2341
			// (set) Token: 0x06002C0E RID: 11278 RVA: 0x00087057 File Offset: 0x00085257
			public float currentBpm
			{
				set
				{
					this._currentBeat = 60f / value;
				}
			}

			// Token: 0x17000926 RID: 2342
			// (get) Token: 0x06002C0F RID: 11279 RVA: 0x00087066 File Offset: 0x00085266
			// (set) Token: 0x06002C10 RID: 11280 RVA: 0x0008706E File Offset: 0x0008526E
			public float beatStart { private get; set; }

			// Token: 0x06002C11 RID: 11281 RVA: 0x00087078 File Offset: 0x00085278
			public void Initialize(MonoBehaviour coroutineOwner)
			{
				this._amp.onInstantiate += delegate()
				{
					if (!this._ampRunning)
					{
						coroutineOwner.StartCoroutine(this.CPlayAmpSound());
					}
				};
			}

			// Token: 0x06002C12 RID: 11282 RVA: 0x000870B0 File Offset: 0x000852B0
			public void StopAmp()
			{
				this._ampRunning = false;
			}

			// Token: 0x06002C13 RID: 11283 RVA: 0x000870B9 File Offset: 0x000852B9
			private IEnumerator CPlayAmpSound()
			{
				this._ampRunning = true;
				while (this._amp.ampExists)
				{
					int timingIndex;
					yield return new WaitForSeconds(this.GetNextEffectTime(out timingIndex) - Time.time);
					if (!this.pause)
					{
						this._amp.PlayAmpBeat(timingIndex);
					}
				}
				this.StopAmp();
				yield break;
			}

			// Token: 0x06002C14 RID: 11284 RVA: 0x000870C8 File Offset: 0x000852C8
			private float GetNextEffectTime(out int timingIndex)
			{
				float num = Time.time - this.beatStart;
				float num2 = this._currentBeat * (float)this._amp.beat;
				float num3 = (float)((int)(num / num2)) * num2;
				float[] timings = this._amp.GetTimings();
				for (int i = 0; i < timings.Length; i++)
				{
					float num4 = this.beatStart + num3 + timings[i] * num2;
					if (Time.time < num4)
					{
						timingIndex = i;
						return num4;
					}
				}
				timingIndex = 0;
				return this.beatStart + num3 + num2 + timings[0] * num2;
			}

			// Token: 0x04002543 RID: 9539
			[SerializeField]
			private Amp _amp;

			// Token: 0x04002544 RID: 9540
			private bool _ampRunning;

			// Token: 0x04002545 RID: 9541
			private float _currentBeat;
		}
	}
}
