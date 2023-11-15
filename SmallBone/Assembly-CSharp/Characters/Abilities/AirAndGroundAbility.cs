using System;
using Characters.Movements;
using UnityEngine;

namespace Characters.Abilities
{
	// Token: 0x020009DA RID: 2522
	public class AirAndGroundAbility : AbilityAttacher
	{
		// Token: 0x06003598 RID: 13720 RVA: 0x0009F3AD File Offset: 0x0009D5AD
		public override void OnIntialize()
		{
			this._abilityComponent.Initialize();
		}

		// Token: 0x06003599 RID: 13721 RVA: 0x0009F3BC File Offset: 0x0009D5BC
		public override void StartAttach()
		{
			base.owner.movement.onJump += this.OnJump;
			base.owner.movement.onFall += this.OnFall;
			base.owner.movement.onGrounded += this.OnGrounded;
		}

		// Token: 0x0600359A RID: 13722 RVA: 0x0009F420 File Offset: 0x0009D620
		public override void StopAttach()
		{
			if (base.owner == null)
			{
				return;
			}
			base.owner.movement.onJump -= this.OnJump;
			base.owner.movement.onFall -= this.OnFall;
			base.owner.movement.onGrounded -= this.OnGrounded;
			this.Detach();
		}

		// Token: 0x0600359B RID: 13723 RVA: 0x0009F496 File Offset: 0x0009D696
		private void OnJump(Movement.JumpType jumpType, float jumpHeight)
		{
			if (jumpType == Movement.JumpType.AirJump)
			{
				return;
			}
			if (this._type == AirAndGroundAbility.Type.Ground)
			{
				this.Detach();
				return;
			}
			this.Attach();
		}

		// Token: 0x0600359C RID: 13724 RVA: 0x0009F4B2 File Offset: 0x0009D6B2
		private void OnFall()
		{
			this.Attach();
		}

		// Token: 0x0600359D RID: 13725 RVA: 0x0009F4BA File Offset: 0x0009D6BA
		private void OnGrounded()
		{
			if (this._type == AirAndGroundAbility.Type.Ground)
			{
				this.Attach();
				return;
			}
			this.Detach();
		}

		// Token: 0x0600359E RID: 13726 RVA: 0x0009F4D1 File Offset: 0x0009D6D1
		private void Attach()
		{
			base.owner.ability.Add(this._abilityComponent.ability);
		}

		// Token: 0x0600359F RID: 13727 RVA: 0x0009F4EF File Offset: 0x0009D6EF
		private void Detach()
		{
			base.owner.ability.Remove(this._abilityComponent.ability);
		}

		// Token: 0x060035A0 RID: 13728 RVA: 0x0001B058 File Offset: 0x00019258
		public override string ToString()
		{
			return this.GetAutoName();
		}

		// Token: 0x04002B25 RID: 11045
		[SerializeField]
		private AirAndGroundAbility.Type _type;

		// Token: 0x04002B26 RID: 11046
		[SerializeField]
		[AbilityComponent.SubcomponentAttribute]
		private AbilityComponent _abilityComponent;

		// Token: 0x020009DB RID: 2523
		private enum Type
		{
			// Token: 0x04002B28 RID: 11048
			Ground,
			// Token: 0x04002B29 RID: 11049
			Air
		}
	}
}
