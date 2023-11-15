using System;
using Level;
using UnityEngine;

namespace Characters
{
	// Token: 0x0200072E RID: 1838
	public readonly struct TargetStruct : ITarget
	{
		// Token: 0x170007C6 RID: 1990
		// (get) Token: 0x0600255D RID: 9565 RVA: 0x0007099A File Offset: 0x0006EB9A
		Collider2D ITarget.collider
		{
			get
			{
				return this.collider;
			}
		}

		// Token: 0x170007C7 RID: 1991
		// (get) Token: 0x0600255E RID: 9566 RVA: 0x000709A2 File Offset: 0x0006EBA2
		Character ITarget.character
		{
			get
			{
				return this.character;
			}
		}

		// Token: 0x170007C8 RID: 1992
		// (get) Token: 0x0600255F RID: 9567 RVA: 0x000709AA File Offset: 0x0006EBAA
		DestructibleObject ITarget.damageable
		{
			get
			{
				return this.damageable;
			}
		}

		// Token: 0x170007C9 RID: 1993
		// (get) Token: 0x06002560 RID: 9568 RVA: 0x000709B2 File Offset: 0x0006EBB2
		Transform ITarget.transform
		{
			get
			{
				return this.transform;
			}
		}

		// Token: 0x06002561 RID: 9569 RVA: 0x000709BA File Offset: 0x0006EBBA
		public TargetStruct(Character character)
		{
			this.character = character;
			this.damageable = null;
			this.collider = character.collider;
			this.transform = character.transform;
		}

		// Token: 0x06002562 RID: 9570 RVA: 0x000709E2 File Offset: 0x0006EBE2
		public TargetStruct(DestructibleObject damageable)
		{
			this.character = null;
			this.damageable = damageable;
			this.collider = damageable.collider;
			this.transform = damageable.transform;
		}

		// Token: 0x04001FBB RID: 8123
		public readonly Collider2D collider;

		// Token: 0x04001FBC RID: 8124
		public readonly Transform transform;

		// Token: 0x04001FBD RID: 8125
		public readonly Character character;

		// Token: 0x04001FBE RID: 8126
		public readonly DestructibleObject damageable;
	}
}
