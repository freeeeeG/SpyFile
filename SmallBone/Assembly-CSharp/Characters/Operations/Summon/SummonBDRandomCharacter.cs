using System;
using Level;
using UnityEngine;

namespace Characters.Operations.Summon
{
	// Token: 0x02000F37 RID: 3895
	public class SummonBDRandomCharacter : Operation
	{
		// Token: 0x06004BC9 RID: 19401 RVA: 0x000DED8C File Offset: 0x000DCF8C
		public override void Run()
		{
			if (this._characterPrefabs.Length == 0)
			{
				return;
			}
			Character character;
			if (this._position != null)
			{
				character = UnityEngine.Object.Instantiate<Character>(this._characterPrefabs.Random<Character>(), this._position.position, Quaternion.identity, Map.Instance.transform);
			}
			else
			{
				character = UnityEngine.Object.Instantiate<Character>(this._characterPrefabs.Random<Character>());
			}
			if (this._containInWave)
			{
				Map.Instance.waveContainer.Attach(character);
			}
			IBDCharacterSetting[] settings = this._settings;
			for (int i = 0; i < settings.Length; i++)
			{
				settings[i].ApplyTo(character);
			}
		}

		// Token: 0x04003AFA RID: 15098
		[SerializeField]
		private Character[] _characterPrefabs;

		// Token: 0x04003AFB RID: 15099
		[SerializeField]
		private Transform _position;

		// Token: 0x04003AFC RID: 15100
		[SerializeField]
		private bool _containInWave = true;

		// Token: 0x04003AFD RID: 15101
		[SubclassSelector]
		[SerializeReference]
		private IBDCharacterSetting[] _settings;
	}
}
