using System;

namespace Characters.Abilities.Weapons.DavyJones
{
	// Token: 0x02000C1D RID: 3101
	public sealed class DavyJonesPassiveComponent : AbilityComponent<DavyJonesPassive>
	{
		// Token: 0x17000D77 RID: 3447
		// (get) Token: 0x06003FAB RID: 16299 RVA: 0x000B8CF2 File Offset: 0x000B6EF2
		// (set) Token: 0x06003FAC RID: 16300 RVA: 0x000B8CFF File Offset: 0x000B6EFF
		public float stack
		{
			get
			{
				return base.baseAbility.MakeSaveData();
			}
			set
			{
				base.baseAbility.Load(value);
			}
		}

		// Token: 0x06003FAD RID: 16301 RVA: 0x000B8D10 File Offset: 0x000B6F10
		public bool IsTop(CannonBallType type)
		{
			CannonBallType? cannonBallType = base.baseAbility.Top();
			return cannonBallType != null && type == cannonBallType.Value;
		}

		// Token: 0x06003FAE RID: 16302 RVA: 0x000B8D3E File Offset: 0x000B6F3E
		public bool IsEmpty()
		{
			return base.baseAbility.isEmpty;
		}

		// Token: 0x06003FAF RID: 16303 RVA: 0x000B8D4B File Offset: 0x000B6F4B
		public void Push(CannonBallType type)
		{
			base.baseAbility.Push(type, 1);
		}

		// Token: 0x06003FB0 RID: 16304 RVA: 0x000B8D5A File Offset: 0x000B6F5A
		public void Push(CannonBallType type, int count)
		{
			base.baseAbility.Push(type, count);
		}

		// Token: 0x06003FB1 RID: 16305 RVA: 0x000B8D69 File Offset: 0x000B6F69
		public void Pop()
		{
			base.baseAbility.Pop();
		}
	}
}
