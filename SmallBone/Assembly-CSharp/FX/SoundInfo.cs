using System;
using UnityEngine;

namespace FX
{
	// Token: 0x0200025B RID: 603
	[Serializable]
	public class SoundInfo
	{
		// Token: 0x1700028C RID: 652
		// (get) Token: 0x06000BD0 RID: 3024 RVA: 0x000208BB File Offset: 0x0001EABB
		public AudioClip audioClip
		{
			get
			{
				return this._audioClip;
			}
		}

		// Token: 0x1700028D RID: 653
		// (get) Token: 0x06000BD1 RID: 3025 RVA: 0x000208C3 File Offset: 0x0001EAC3
		public float length
		{
			get
			{
				return this._length;
			}
		}

		// Token: 0x1700028E RID: 654
		// (get) Token: 0x06000BD2 RID: 3026 RVA: 0x000208CB File Offset: 0x0001EACB
		public float volume
		{
			get
			{
				return this._volume;
			}
		}

		// Token: 0x1700028F RID: 655
		// (get) Token: 0x06000BD3 RID: 3027 RVA: 0x000208D3 File Offset: 0x0001EAD3
		public float uniqueTime
		{
			get
			{
				return this._uniqueTime;
			}
		}

		// Token: 0x17000290 RID: 656
		// (get) Token: 0x06000BD4 RID: 3028 RVA: 0x000208DB File Offset: 0x0001EADB
		public int priority
		{
			get
			{
				return this._priority;
			}
		}

		// Token: 0x17000291 RID: 657
		// (get) Token: 0x06000BD5 RID: 3029 RVA: 0x000208E3 File Offset: 0x0001EAE3
		public float stereoPan
		{
			get
			{
				return this._stereoPan;
			}
		}

		// Token: 0x17000292 RID: 658
		// (get) Token: 0x06000BD6 RID: 3030 RVA: 0x000208EB File Offset: 0x0001EAEB
		public bool bypassEffects
		{
			get
			{
				return this._bypassEffects;
			}
		}

		// Token: 0x17000293 RID: 659
		// (get) Token: 0x06000BD7 RID: 3031 RVA: 0x000208F3 File Offset: 0x0001EAF3
		public bool bypassListenerEffects
		{
			get
			{
				return this._bypassListenerEffects;
			}
		}

		// Token: 0x17000294 RID: 660
		// (get) Token: 0x06000BD8 RID: 3032 RVA: 0x000208FB File Offset: 0x0001EAFB
		public bool bypassReverbZones
		{
			get
			{
				return this._bypassReverbZones;
			}
		}

		// Token: 0x17000295 RID: 661
		// (get) Token: 0x06000BD9 RID: 3033 RVA: 0x00020903 File Offset: 0x0001EB03
		public bool loop
		{
			get
			{
				return this._loop;
			}
		}

		// Token: 0x17000296 RID: 662
		// (get) Token: 0x06000BDA RID: 3034 RVA: 0x0002090B File Offset: 0x0001EB0B
		public float spatialBlend
		{
			get
			{
				return this._spatialBlend;
			}
		}

		// Token: 0x06000BDB RID: 3035 RVA: 0x00020913 File Offset: 0x0001EB13
		public SoundInfo(AudioClip clip)
		{
			this._audioClip = clip;
		}

		// Token: 0x06000BDC RID: 3036 RVA: 0x00020944 File Offset: 0x0001EB44
		~SoundInfo()
		{
			this.Dispose();
		}

		// Token: 0x06000BDD RID: 3037 RVA: 0x00020970 File Offset: 0x0001EB70
		public void Dispose()
		{
			this._audioClip = null;
		}

		// Token: 0x040009D5 RID: 2517
		[SerializeField]
		private AudioClip _audioClip;

		// Token: 0x040009D6 RID: 2518
		[Tooltip("0일 경우 AudioClip의 Length")]
		[SerializeField]
		private float _length;

		// Token: 0x040009D7 RID: 2519
		[SerializeField]
		[Range(0f, 1f)]
		private float _volume = 1f;

		// Token: 0x040009D8 RID: 2520
		[SerializeField]
		private float _uniqueTime = 0.1f;

		// Token: 0x040009D9 RID: 2521
		[SerializeField]
		[Range(0f, 256f)]
		private int _priority = 128;

		// Token: 0x040009DA RID: 2522
		[Range(-1f, 1f)]
		[SerializeField]
		private float _stereoPan;

		// Token: 0x040009DB RID: 2523
		[SerializeField]
		private bool _bypassEffects;

		// Token: 0x040009DC RID: 2524
		[SerializeField]
		private bool _bypassListenerEffects;

		// Token: 0x040009DD RID: 2525
		[SerializeField]
		private bool _bypassReverbZones;

		// Token: 0x040009DE RID: 2526
		[SerializeField]
		private bool _loop;

		// Token: 0x040009DF RID: 2527
		[Range(0f, 1f)]
		[SerializeField]
		private float _spatialBlend;
	}
}
