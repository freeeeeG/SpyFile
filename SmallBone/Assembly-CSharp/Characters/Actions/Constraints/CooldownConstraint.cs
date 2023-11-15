using System;
using Characters.Actions.Cooldowns;
using Characters.Gear.Weapons;
using Characters.Operations;
using Characters.Player;
using UnityEditor;
using UnityEngine;

namespace Characters.Actions.Constraints
{
	// Token: 0x02000974 RID: 2420
	public class CooldownConstraint : Constraint
	{
		// Token: 0x17000B47 RID: 2887
		// (get) Token: 0x06003419 RID: 13337 RVA: 0x0009A448 File Offset: 0x00098648
		public bool canUse
		{
			get
			{
				return this._cooldown.canUse;
			}
		}

		// Token: 0x0600341A RID: 13338 RVA: 0x0009A458 File Offset: 0x00098658
		public override void Initilaize(Action action)
		{
			base.Initilaize(action);
			this._cooldown.Initialize(action.owner);
			this._inventory = action.owner.GetComponent<WeaponInventory>();
			if (this._inventory == null)
			{
				this._cooldown.onReady += this.RunOperationsWhenReady;
				return;
			}
			this._weapon = base.GetComponentInParent<Weapon>();
			this._cooldown.onReady += this.RunOperationsWhenReadyWithCheckWeapon;
		}

		// Token: 0x0600341B RID: 13339 RVA: 0x0009A4D7 File Offset: 0x000986D7
		private void RunOperationsWhenReady()
		{
			base.StartCoroutine(this._operationsWhenReady.CRun(this._action.owner));
		}

		// Token: 0x0600341C RID: 13340 RVA: 0x0009A4F6 File Offset: 0x000986F6
		private void RunOperationsWhenReadyWithCheckWeapon()
		{
			if (this._inventory.polymorphOrCurrent == this._weapon)
			{
				base.StartCoroutine(this._operationsWhenReady.CRun(this._action.owner));
			}
		}

		// Token: 0x0600341D RID: 13341 RVA: 0x0009A52D File Offset: 0x0009872D
		private void OnDisable()
		{
			this._cooldown.onReady -= this.RunOperationsWhenReady;
			this._cooldown.onReady -= this.RunOperationsWhenReadyWithCheckWeapon;
		}

		// Token: 0x0600341E RID: 13342 RVA: 0x0009A448 File Offset: 0x00098648
		public override bool Pass()
		{
			return this._cooldown.canUse;
		}

		// Token: 0x0600341F RID: 13343 RVA: 0x0009A55D File Offset: 0x0009875D
		public override void Consume()
		{
			this._cooldown.Consume();
		}

		// Token: 0x04002A28 RID: 10792
		[SerializeField]
		[Cooldown.SubcomponentAttribute]
		private Cooldown _cooldown;

		// Token: 0x04002A29 RID: 10793
		[UnityEditor.Subcomponent(typeof(OperationInfo))]
		[SerializeField]
		private OperationInfo.Subcomponents _operationsWhenReady;

		// Token: 0x04002A2A RID: 10794
		private Weapon _weapon;

		// Token: 0x04002A2B RID: 10795
		private WeaponInventory _inventory;
	}
}
