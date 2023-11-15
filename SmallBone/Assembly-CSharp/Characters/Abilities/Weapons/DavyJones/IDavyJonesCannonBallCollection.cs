using System;

namespace Characters.Abilities.Weapons.DavyJones
{
	// Token: 0x02000C19 RID: 3097
	public interface IDavyJonesCannonBallCollection
	{
		// Token: 0x06003F97 RID: 16279
		void Push(CannonBallType cannon, int count);

		// Token: 0x06003F98 RID: 16280
		void Pop();
	}
}
