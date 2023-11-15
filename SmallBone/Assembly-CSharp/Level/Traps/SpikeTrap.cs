using System;
using System.Collections;
using System.Linq;
using Characters;
using Characters.Actions;
using Characters.Gear.Weapons;
using Characters.Player;
using PhysicsUtils;
using UnityEngine;

namespace Level.Traps
{
	// Token: 0x0200067D RID: 1661
	public class SpikeTrap : MonoBehaviour
	{
		// Token: 0x06002140 RID: 8512 RVA: 0x00064158 File Offset: 0x00062358
		private void Awake()
		{
			this._attackAction.Initialize(this._character);
			this._weaponNamesToExclude = (from weapon in this._weaponsToExclude
			select weapon.name).ToArray<string>();
			base.StartCoroutine(this.CAttack());
		}

		// Token: 0x06002141 RID: 8513 RVA: 0x000641B8 File Offset: 0x000623B8
		private IEnumerator CAttack()
		{
			for (;;)
			{
				yield return Chronometer.global.WaitForSeconds(0.1f);
				if (this.FindPlayer())
				{
					this._attackAction.TryStart();
					yield return this._attackAction.CWaitForEndOfRunning();
					yield return Chronometer.global.WaitForSeconds(this._interval);
				}
			}
			yield break;
		}

		// Token: 0x06002142 RID: 8514 RVA: 0x000641C8 File Offset: 0x000623C8
		private bool FindPlayer()
		{
			this._range.enabled = true;
			this._overlapper.contactFilter.SetLayerMask(512);
			this._overlapper.OverlapCollider(this._range);
			this._range.enabled = false;
			Target component = this._overlapper.GetComponent<Target>();
			if (component == null || component.character == null || !component.character.movement.isGrounded)
			{
				return false;
			}
			PlayerComponents playerComponents = component.character.playerComponents;
			return playerComponents == null || !this._weaponNamesToExclude.Any((string name) => name.Equals(playerComponents.inventory.weapon.polymorphOrCurrent.name, StringComparison.OrdinalIgnoreCase));
		}

		// Token: 0x04001C53 RID: 7251
		[SerializeField]
		private Character _character;

		// Token: 0x04001C54 RID: 7252
		[SerializeField]
		private float _interval = 2f;

		// Token: 0x04001C55 RID: 7253
		[SerializeField]
		private Characters.Actions.Action _attackAction;

		// Token: 0x04001C56 RID: 7254
		[SerializeField]
		private Collider2D _range;

		// Token: 0x04001C57 RID: 7255
		[SerializeField]
		private readonly NonAllocOverlapper _overlapper = new NonAllocOverlapper(1);

		// Token: 0x04001C58 RID: 7256
		[SerializeField]
		[Space]
		private Weapon[] _weaponsToExclude;

		// Token: 0x04001C59 RID: 7257
		private string[] _weaponNamesToExclude;
	}
}
