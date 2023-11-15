using System;
using System.Collections.Generic;
using System.Linq;
using Characters.Actions;
using Characters.Gear.Weapons;
using UnityEngine;

namespace Characters.Abilities.Triggers
{
	// Token: 0x02000B2D RID: 2861
	[Serializable]
	public sealed class OnCooldown : Trigger
	{
		// Token: 0x060039E0 RID: 14816 RVA: 0x000AAE53 File Offset: 0x000A9053
		public override void Attach(Character character)
		{
			this._character = character;
		}

		// Token: 0x060039E1 RID: 14817 RVA: 0x000AAE5C File Offset: 0x000A905C
		public override void UpdateTime(float deltaTime)
		{
			base.UpdateTime(deltaTime);
			this._elapsed += deltaTime;
			if (this._elapsed < 0.1f)
			{
				return;
			}
			this._elapsed = 0f;
			this.Check();
		}

		// Token: 0x060039E2 RID: 14818 RVA: 0x000AAE94 File Offset: 0x000A9094
		private void Check()
		{
			Weapon current = this._character.playerComponents.inventory.weapon.current;
			List<Characters.Actions.Action> list = current.actionsByType[Characters.Actions.Action.Type.Skill].ToList<Characters.Actions.Action>();
			Weapon next = this._character.playerComponents.inventory.weapon.next;
			if (next != null)
			{
				list.AddRange(next.actionsByType[Characters.Actions.Action.Type.Skill]);
			}
			int num = 0;
			foreach (Characters.Actions.Action action in list)
			{
				if (action.type == Characters.Actions.Action.Type.Skill)
				{
					if (action.cooldown.maxStack > 1 && action.cooldown.stacks < action.cooldown.maxStack)
					{
						num++;
					}
					else if (!action.cooldown.canUse)
					{
						num++;
					}
				}
			}
			if (base.canBeTriggered)
			{
				switch (this._type)
				{
				case OnCooldown.Type.Any:
					if (num >= 1)
					{
						base.Invoke();
						return;
					}
					break;
				case OnCooldown.Type.All:
					if (next == null)
					{
						return;
					}
					if (current.currentSkills.Count + next.currentSkills.Count == num)
					{
						base.Invoke();
						return;
					}
					break;
				case OnCooldown.Type.Count:
					if (num == this._count)
					{
						base.Invoke();
					}
					break;
				default:
					return;
				}
			}
		}

		// Token: 0x060039E3 RID: 14819 RVA: 0x00002191 File Offset: 0x00000391
		public override void Detach()
		{
		}

		// Token: 0x04002DE8 RID: 11752
		private const float _checkInterval = 0.1f;

		// Token: 0x04002DE9 RID: 11753
		[SerializeField]
		private OnCooldown.Type _type;

		// Token: 0x04002DEA RID: 11754
		[SerializeField]
		private int _count;

		// Token: 0x04002DEB RID: 11755
		private Character _character;

		// Token: 0x04002DEC RID: 11756
		private float _elapsed;

		// Token: 0x02000B2E RID: 2862
		private enum Type
		{
			// Token: 0x04002DEE RID: 11758
			Any,
			// Token: 0x04002DEF RID: 11759
			All,
			// Token: 0x04002DF0 RID: 11760
			Count
		}

		// Token: 0x02000B2F RID: 2863
		public enum Timing
		{
			// Token: 0x04002DF2 RID: 11762
			Start,
			// Token: 0x04002DF3 RID: 11763
			End
		}
	}
}
