using System;
using Services;
using Singletons;

namespace Characters.Operations.LookAtTargets
{
	// Token: 0x02000F04 RID: 3844
	public sealed class Player : Target
	{
		// Token: 0x06004B2E RID: 19246 RVA: 0x000DD518 File Offset: 0x000DB718
		public override Character.LookingDirection GetDirectionFrom(Character character)
		{
			Character player = Singleton<Service>.Instance.levelManager.player;
			if (player == null)
			{
				return character.lookingDirection;
			}
			if (player.transform.position.x > character.transform.position.x)
			{
				return Character.LookingDirection.Right;
			}
			return Character.LookingDirection.Left;
		}
	}
}
