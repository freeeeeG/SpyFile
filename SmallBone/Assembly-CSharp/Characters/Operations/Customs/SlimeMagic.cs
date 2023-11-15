using System;
using FX;
using Level;
using Level.Objects.DecorationCharacter;
using Singletons;
using UnityEngine;

namespace Characters.Operations.Customs
{
	// Token: 0x02000FEC RID: 4076
	public sealed class SlimeMagic : TargetedCharacterOperation
	{
		// Token: 0x06004EBE RID: 20158 RVA: 0x000EC3A7 File Offset: 0x000EA5A7
		private void Awake()
		{
			this._offeset = new Vector2(0f, 1f);
			this._soundInfos = new SoundInfo[]
			{
				this._transformSoundInfo2,
				this._transformSoundInfo3
			};
		}

		// Token: 0x06004EBF RID: 20159 RVA: 0x000EC3DC File Offset: 0x000EA5DC
		public override void Run(Character owner, Character target)
		{
			if (!MMMaths.PercentChance(this._chance))
			{
				return;
			}
			Vector2 v = target.collider.bounds.center;
			UnityEngine.Object.Instantiate<DecorationCharacter>(this._transformTargetPrefabs.Random<DecorationCharacter>(), v, Quaternion.identity, Map.Instance.transform).SetRenderSortingOrder(SlimeMagic._sortingOrder++);
			this._transformEffectInfo.Spawn(target.transform.position, 0f, 1f);
			PersistentSingleton<SoundManager>.Instance.PlaySound(this._soundInfos.Random<SoundInfo>(), target.transform.position);
			target.health.Kill();
		}

		// Token: 0x04003ED6 RID: 16086
		private static int _sortingOrder = int.MinValue;

		// Token: 0x04003ED7 RID: 16087
		[SerializeField]
		[Range(0f, 100f)]
		private int _chance;

		// Token: 0x04003ED8 RID: 16088
		[SerializeField]
		private EffectInfo _transformEffectInfo;

		// Token: 0x04003ED9 RID: 16089
		[SerializeField]
		private SoundInfo _transformSoundInfo2;

		// Token: 0x04003EDA RID: 16090
		[SerializeField]
		private SoundInfo _transformSoundInfo3;

		// Token: 0x04003EDB RID: 16091
		[SerializeField]
		private DecorationCharacter[] _transformTargetPrefabs;

		// Token: 0x04003EDC RID: 16092
		private Vector2 _offeset;

		// Token: 0x04003EDD RID: 16093
		private SoundInfo[] _soundInfos;
	}
}
