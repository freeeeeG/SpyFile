using System;
using System.Collections.Generic;
using Characters.AI;
using UnityEngine;

namespace Characters.Operations.SetPosition
{
	// Token: 0x02000ED3 RID: 3795
	public class ToLowHealthEnemy : Policy
	{
		// Token: 0x06004A87 RID: 19079 RVA: 0x000D950E File Offset: 0x000D770E
		public override Vector2 GetPosition(Character owner)
		{
			return this.GetPosition();
		}

		// Token: 0x06004A88 RID: 19080 RVA: 0x000D9674 File Offset: 0x000D7874
		public override Vector2 GetPosition()
		{
			Character character = this.GetLowHealthCharacter();
			if (character == null)
			{
				character = this._controller.character;
			}
			return character.transform.position;
		}

		// Token: 0x06004A89 RID: 19081 RVA: 0x000D96B0 File Offset: 0x000D78B0
		private Character GetLowHealthCharacter()
		{
			List<Character> list = this._controller.FindEnemiesInRange(this._findRange);
			double num = 1.0;
			Character result = null;
			foreach (Character character in list)
			{
				if (character.liveAndActive && (this._includeSelf || !(character == this._controller.character)) && character.health.percent < num)
				{
					num = character.health.percent;
					result = character;
				}
			}
			return result;
		}

		// Token: 0x040039A7 RID: 14759
		[SerializeField]
		private AIController _controller;

		// Token: 0x040039A8 RID: 14760
		[SerializeField]
		private Collider2D _findRange;

		// Token: 0x040039A9 RID: 14761
		[SerializeField]
		private bool _includeSelf = true;
	}
}
