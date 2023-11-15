using System;
using System.Collections.Generic;
using Characters.AI;
using FX;
using UnityEngine;

namespace Characters.Operations
{
	// Token: 0x02000DD1 RID: 3537
	public sealed class CommandToSacrifice : CharacterOperation
	{
		// Token: 0x06004707 RID: 18183 RVA: 0x000CE24C File Offset: 0x000CC44C
		public override void Run(Character owner)
		{
			List<Character> list = this._aiController.FindEnemiesInRange(this._range);
			if (list == null || list.Count <= 0)
			{
				return;
			}
			foreach (Character character in list)
			{
				SacrificeCharacter component = character.GetComponent<SacrificeCharacter>();
				if (!(component == null))
				{
					component.Run(false);
					this._effect.Spawn(component.transform.position, 0f, 1f);
				}
			}
		}

		// Token: 0x040035E5 RID: 13797
		[SerializeField]
		private Collider2D _range;

		// Token: 0x040035E6 RID: 13798
		[SerializeField]
		private AIController _aiController;

		// Token: 0x040035E7 RID: 13799
		[SerializeField]
		private EffectInfo _effect;
	}
}
