using System;
using System.Collections.Generic;
using FX;
using Singletons;
using UnityEngine;

namespace Characters.Operations.Customs
{
	// Token: 0x02000FE3 RID: 4067
	public sealed class RandomPlaySound : CharacterOperation
	{
		// Token: 0x06004E9A RID: 20122 RVA: 0x000EBA2A File Offset: 0x000E9C2A
		private void Awake()
		{
			this._audioClipInfo = this._randomAudioClipsInfo.Random<SoundInfo>();
		}

		// Token: 0x06004E9B RID: 20123 RVA: 0x000EBA40 File Offset: 0x000E9C40
		public override void Run(Character owner)
		{
			RandomPlaySound.<>c__DisplayClass7_0 CS$<>8__locals1 = new RandomPlaySound.<>c__DisplayClass7_0();
			CS$<>8__locals1.<>4__this = this;
			if (!this.randomOnlyAwake)
			{
				this._audioClipInfo = this._randomAudioClipsInfo.Random<SoundInfo>();
			}
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

		// Token: 0x06004E9C RID: 20124 RVA: 0x000EBAF8 File Offset: 0x000E9CF8
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

		// Token: 0x04003EAC RID: 16044
		[SerializeField]
		private SoundInfo _audioClipInfo;

		// Token: 0x04003EAD RID: 16045
		[SerializeField]
		private SoundInfo[] _randomAudioClipsInfo;

		// Token: 0x04003EAE RID: 16046
		[SerializeField]
		private bool randomOnlyAwake = true;

		// Token: 0x04003EAF RID: 16047
		[SerializeField]
		private Transform _position;

		// Token: 0x04003EB0 RID: 16048
		[SerializeField]
		private bool _trackChildren;

		// Token: 0x04003EB1 RID: 16049
		private readonly List<ReusableAudioSource> _children = new List<ReusableAudioSource>();
	}
}
