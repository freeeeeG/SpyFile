using System;
using Characters.AI;
using Level;
using Services;
using Singletons;
using UnityEngine;

namespace Characters.Operations
{
	// Token: 0x02000E39 RID: 3641
	public sealed class SpawnRandomCharacter : CharacterOperation
	{
		// Token: 0x06004889 RID: 18569 RVA: 0x000D2EDC File Offset: 0x000D10DC
		public override void Run(Character owner)
		{
			if (this._characterPrefabs.Length == 0)
			{
				return;
			}
			Character original = this._characterPrefabs.Random<Character>();
			Character character;
			if (this._containInWave)
			{
				if (this._position != null)
				{
					character = UnityEngine.Object.Instantiate<Character>(original, this._position.position, Quaternion.identity);
				}
				else
				{
					character = UnityEngine.Object.Instantiate<Character>(original);
				}
				Map.Instance.waveContainer.Attach(character);
			}
			else
			{
				character = UnityEngine.Object.Instantiate<Character>(original, this._position.position, Quaternion.identity);
				character.transform.parent = Map.Instance.transform;
			}
			if (this._setPlayerAsTarget)
			{
				AIController componentInChildren = character.GetComponentInChildren<AIController>();
				componentInChildren.target = Singleton<Service>.Instance.levelManager.player;
				componentInChildren.character.ForceToLookAt(componentInChildren.target.transform.position.x);
			}
		}

		// Token: 0x040037A1 RID: 14241
		[SerializeField]
		private Character[] _characterPrefabs;

		// Token: 0x040037A2 RID: 14242
		[SerializeField]
		private Transform _position;

		// Token: 0x040037A3 RID: 14243
		[SerializeField]
		private bool _setPlayerAsTarget;

		// Token: 0x040037A4 RID: 14244
		[SerializeField]
		private bool _containInWave;
	}
}
