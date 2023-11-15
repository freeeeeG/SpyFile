using System;
using Characters;
using Characters.Abilities;
using UnityEngine;

namespace Level.Traps
{
	// Token: 0x0200067C RID: 1660
	public class ServantPrison : MonoBehaviour
	{
		// Token: 0x0600213D RID: 8509 RVA: 0x000640C6 File Offset: 0x000622C6
		private void Awake()
		{
			this._prop.onDestroy += this.OnPropDestroy;
		}

		// Token: 0x0600213E RID: 8510 RVA: 0x000640E0 File Offset: 0x000622E0
		private void OnPropDestroy()
		{
			for (int i = 0; i < this._characters.Length; i++)
			{
				Character character = this._characters[i];
				Map.Instance.waveContainer.Attach(character);
				character.gameObject.SetActive(true);
				character.ability.Add(this._getInvulnerable);
			}
		}

		// Token: 0x04001C50 RID: 7248
		private readonly GetInvulnerable _getInvulnerable = new GetInvulnerable
		{
			duration = 0.5f
		};

		// Token: 0x04001C51 RID: 7249
		[SerializeField]
		private Prop _prop;

		// Token: 0x04001C52 RID: 7250
		[SerializeField]
		private Character[] _characters;
	}
}
