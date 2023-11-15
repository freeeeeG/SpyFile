using System;
using Services;
using UnityEngine;

namespace Characters.Operations.Summon
{
	// Token: 0x02000F3B RID: 3899
	public class SummonCharacterInPool : CharacterOperation
	{
		// Token: 0x06004BDC RID: 19420 RVA: 0x000DF184 File Offset: 0x000DD384
		private void Awake()
		{
			this._pool = new Character[this._cacheCount];
			for (int i = 0; i < this._cacheCount; i++)
			{
				Character character = UnityEngine.Object.Instantiate<Character>(this._characterToSummon, this._summonTransform.position, Quaternion.identity);
				character.gameObject.SetActive(false);
				this._pool[i] = character;
			}
		}

		// Token: 0x06004BDD RID: 19421 RVA: 0x000DF1E4 File Offset: 0x000DD3E4
		protected override void OnDestroy()
		{
			base.OnDestroy();
			if (Service.quitting)
			{
				return;
			}
			for (int i = 0; i < this._cacheCount; i++)
			{
				UnityEngine.Object.Destroy(this._pool[i].gameObject);
			}
		}

		// Token: 0x06004BDE RID: 19422 RVA: 0x000DF224 File Offset: 0x000DD424
		public override void Run(Character owner)
		{
			if (!MMMaths.PercentChance(this._spawnChance))
			{
				return;
			}
			for (int i = 0; i < this._cacheCount; i++)
			{
				Character character = this._pool[i];
				if (!character.gameObject.activeSelf)
				{
					character.transform.position = this._summonTransform.position;
					character.gameObject.SetActive(true);
					return;
				}
			}
		}

		// Token: 0x04003B09 RID: 15113
		[SerializeField]
		private Character _characterToSummon;

		// Token: 0x04003B0A RID: 15114
		[SerializeField]
		private Transform _summonTransform;

		// Token: 0x04003B0B RID: 15115
		[Range(1f, 10f)]
		[SerializeField]
		private int _cacheCount = 1;

		// Token: 0x04003B0C RID: 15116
		[Range(0f, 100f)]
		[SerializeField]
		private int _spawnChance = 50;

		// Token: 0x04003B0D RID: 15117
		private Character[] _pool;
	}
}
