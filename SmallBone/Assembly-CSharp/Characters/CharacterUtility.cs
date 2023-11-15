using System;
using UnityEngine;

namespace Characters
{
	// Token: 0x020006DA RID: 1754
	public static class CharacterUtility
	{
		// Token: 0x06002397 RID: 9111 RVA: 0x0006AC3C File Offset: 0x00068E3C
		public static bool TryFindCharacterComponent(this GameObject gameObject, out Character character)
		{
			if (gameObject.TryGetComponent<Character>(out character))
			{
				return true;
			}
			Target target;
			if (gameObject.TryGetComponent<Target>(out target))
			{
				character = target.character;
				return true;
			}
			character = null;
			return false;
		}

		// Token: 0x06002398 RID: 9112 RVA: 0x0006AC6C File Offset: 0x00068E6C
		public static bool TryFindCharacterComponent(this Component component, out Character character)
		{
			return component.gameObject.TryFindCharacterComponent(out character);
		}
	}
}
