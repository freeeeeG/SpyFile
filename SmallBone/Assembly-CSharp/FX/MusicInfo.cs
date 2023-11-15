using System;
using UnityEngine;

namespace FX
{
	// Token: 0x02000246 RID: 582
	[Serializable]
	public class MusicInfo
	{
		// Token: 0x17000273 RID: 627
		// (get) Token: 0x06000B72 RID: 2930 RVA: 0x0001F8C5 File Offset: 0x0001DAC5
		public AudioClip audioClip
		{
			get
			{
				return this._audioClip;
			}
		}

		// Token: 0x17000274 RID: 628
		// (get) Token: 0x06000B73 RID: 2931 RVA: 0x0001F8CD File Offset: 0x0001DACD
		public float volume
		{
			get
			{
				return this._volume;
			}
		}

		// Token: 0x17000275 RID: 629
		// (get) Token: 0x06000B74 RID: 2932 RVA: 0x0001F8D5 File Offset: 0x0001DAD5
		public bool fade
		{
			get
			{
				return this._fade;
			}
		}

		// Token: 0x17000276 RID: 630
		// (get) Token: 0x06000B75 RID: 2933 RVA: 0x0001F8DD File Offset: 0x0001DADD
		public bool loop
		{
			get
			{
				return this._loop;
			}
		}

		// Token: 0x17000277 RID: 631
		// (get) Token: 0x06000B76 RID: 2934 RVA: 0x0001F8E5 File Offset: 0x0001DAE5
		public bool usePlayHistory
		{
			get
			{
				return this._usePlayHistory;
			}
		}

		// Token: 0x04000987 RID: 2439
		[SerializeField]
		private AudioClip _audioClip;

		// Token: 0x04000988 RID: 2440
		[SerializeField]
		[Range(0f, 1f)]
		private float _volume = 1f;

		// Token: 0x04000989 RID: 2441
		[SerializeField]
		private bool _fade = true;

		// Token: 0x0400098A RID: 2442
		[SerializeField]
		private bool _loop = true;

		// Token: 0x0400098B RID: 2443
		[SerializeField]
		private bool _usePlayHistory = true;
	}
}
