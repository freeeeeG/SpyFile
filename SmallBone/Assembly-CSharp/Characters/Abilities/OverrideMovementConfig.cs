using System;
using Characters.Movements;
using UnityEngine;

namespace Characters.Abilities
{
	// Token: 0x02000A9A RID: 2714
	[Serializable]
	public class OverrideMovementConfig : Ability
	{
		// Token: 0x06003822 RID: 14370 RVA: 0x00089C49 File Offset: 0x00087E49
		public OverrideMovementConfig()
		{
		}

		// Token: 0x06003823 RID: 14371 RVA: 0x000A5A41 File Offset: 0x000A3C41
		public OverrideMovementConfig(Movement.Config config)
		{
			this._config = config;
		}

		// Token: 0x06003824 RID: 14372 RVA: 0x000A5A50 File Offset: 0x000A3C50
		public override IAbilityInstance CreateInstance(Character owner)
		{
			return new OverrideMovementConfig.Instance(owner, this);
		}

		// Token: 0x04002CC1 RID: 11457
		[SerializeField]
		private Movement.Config _config;

		// Token: 0x04002CC2 RID: 11458
		[SerializeField]
		private int _priority;

		// Token: 0x02000A9B RID: 2715
		public class Instance : AbilityInstance<OverrideMovementConfig>
		{
			// Token: 0x06003825 RID: 14373 RVA: 0x000A5A59 File Offset: 0x000A3C59
			public Instance(Character owner, OverrideMovementConfig ability) : base(owner, ability)
			{
			}

			// Token: 0x06003826 RID: 14374 RVA: 0x000A5A64 File Offset: 0x000A3C64
			protected override void OnAttach()
			{
				this.owner.movement.configs.Add(this.ability._priority, this.ability._config);
				if (this.ability._config.keepMove)
				{
					this.owner.movement.MoveHorizontal((this.owner.lookingDirection == Character.LookingDirection.Right) ? Vector2.right : Vector2.left);
				}
			}

			// Token: 0x06003827 RID: 14375 RVA: 0x000A5AD7 File Offset: 0x000A3CD7
			protected override void OnDetach()
			{
				this.owner.movement.configs.Remove(this.ability._config);
			}
		}
	}
}
