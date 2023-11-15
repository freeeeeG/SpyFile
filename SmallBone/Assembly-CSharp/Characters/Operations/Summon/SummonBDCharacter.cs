using System;
using Level;
using UnityEngine;

namespace Characters.Operations.Summon
{
	// Token: 0x02000F36 RID: 3894
	public class SummonBDCharacter : Operation
	{
		// Token: 0x06004BC7 RID: 19399 RVA: 0x000DECF8 File Offset: 0x000DCEF8
		public override void Run()
		{
			Character character;
			if (this._position != null)
			{
				character = UnityEngine.Object.Instantiate<Character>(this._character, this._position.position, Quaternion.identity, Map.Instance.transform);
			}
			else
			{
				character = UnityEngine.Object.Instantiate<Character>(this._character);
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

		// Token: 0x04003AF6 RID: 15094
		[SerializeField]
		private Character _character;

		// Token: 0x04003AF7 RID: 15095
		[SerializeField]
		private Transform _position;

		// Token: 0x04003AF8 RID: 15096
		[SerializeField]
		private bool _containInWave = true;

		// Token: 0x04003AF9 RID: 15097
		[SerializeReference]
		[SubclassSelector]
		private IBDCharacterSetting[] _settings;
	}
}
