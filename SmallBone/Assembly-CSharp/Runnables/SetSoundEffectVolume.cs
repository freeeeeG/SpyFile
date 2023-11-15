using System;
using Data;
using UnityEngine;

namespace Runnables
{
	// Token: 0x0200033A RID: 826
	public class SetSoundEffectVolume : Runnable
	{
		// Token: 0x06000FAF RID: 4015 RVA: 0x0002F6D4 File Offset: 0x0002D8D4
		private void OnEnable()
		{
			this._originalVolume = GameData.Settings.sfxVolume;
		}

		// Token: 0x06000FB0 RID: 4016 RVA: 0x0002F6E1 File Offset: 0x0002D8E1
		public override void Run()
		{
			this._originalVolume = GameData.Settings.sfxVolume;
			GameData.Settings.sfxVolume = (float)this._value;
		}

		// Token: 0x06000FB1 RID: 4017 RVA: 0x0002F6FA File Offset: 0x0002D8FA
		public void SetOriginalVolume()
		{
			GameData.Settings.sfxVolume = this._originalVolume;
		}

		// Token: 0x06000FB2 RID: 4018 RVA: 0x0002F707 File Offset: 0x0002D907
		private void OnDisable()
		{
			this.SetOriginalVolume();
		}

		// Token: 0x04000CE6 RID: 3302
		[SerializeField]
		private int _value;

		// Token: 0x04000CE7 RID: 3303
		private float _originalVolume;
	}
}
