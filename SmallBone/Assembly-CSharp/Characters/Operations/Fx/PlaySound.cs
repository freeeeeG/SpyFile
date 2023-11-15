using System;
using System.Collections.Generic;
using FX;
using Singletons;
using UnityEngine;

namespace Characters.Operations.Fx
{
	// Token: 0x02000F16 RID: 3862
	public sealed class PlaySound : CharacterOperation
	{
		// Token: 0x06004B63 RID: 19299 RVA: 0x000DDE50 File Offset: 0x000DC050
		public override void Run(Character owner)
		{
			PlaySound.<>c__DisplayClass4_0 CS$<>8__locals1 = new PlaySound.<>c__DisplayClass4_0();
			CS$<>8__locals1.<>4__this = this;
			Vector3 position = (this._position == null) ? base.transform.position : this._position.position;
			CS$<>8__locals1.reusableAudioSource = PersistentSingleton<SoundManager>.Instance.PlaySound(this._audioClipInfo, position);
			if (CS$<>8__locals1.reusableAudioSource == null)
			{
				return;
			}
			if (!this._trackChildren)
			{
				return;
			}
			this._children.Add(CS$<>8__locals1.reusableAudioSource);
			CS$<>8__locals1.reusableAudioSource.reusable.onDespawn += CS$<>8__locals1.<Run>g__RemoveFromList|0;
		}

		// Token: 0x06004B64 RID: 19300 RVA: 0x000DDEF0 File Offset: 0x000DC0F0
		public override void Stop()
		{
			if (!this._trackChildren)
			{
				return;
			}
			for (int i = this._children.Count - 1; i >= 0; i--)
			{
				this._children[i].reusable.Despawn();
			}
		}

		// Token: 0x04003A9B RID: 15003
		[SerializeField]
		private SoundInfo _audioClipInfo;

		// Token: 0x04003A9C RID: 15004
		[SerializeField]
		private Transform _position;

		// Token: 0x04003A9D RID: 15005
		[SerializeField]
		private bool _trackChildren;

		// Token: 0x04003A9E RID: 15006
		private readonly List<ReusableAudioSource> _children = new List<ReusableAudioSource>();
	}
}
