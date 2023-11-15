using System;
using Characters.Player;
using UnityEngine;

namespace FX
{
	// Token: 0x0200024B RID: 587
	public class PlayerShadowCaster : MonoBehaviour
	{
		// Token: 0x06000B8E RID: 2958 RVA: 0x0001FD28 File Offset: 0x0001DF28
		private void Awake()
		{
			this._weaponInventory = base.GetComponent<WeaponInventory>();
			this._weaponInventory.onSwap += this.UpdateCustomWidth;
			this._collider = base.GetComponent<Collider2D>();
			this._shadowRenderer = new FootShadowRenderer(1, base.transform);
			this._shadowRenderer.spriteRenderer.sortingLayerName = "Player";
			this._shadowRenderer.spriteRenderer.sortingOrder = -10000;
		}

		// Token: 0x06000B8F RID: 2959 RVA: 0x0001FDA0 File Offset: 0x0001DFA0
		private void UpdateCustomWidth()
		{
			this._customWidth = this._weaponInventory.polymorphOrCurrent.customWidth;
		}

		// Token: 0x06000B90 RID: 2960 RVA: 0x0001FDB8 File Offset: 0x0001DFB8
		private void OnDestroy()
		{
			this._weaponInventory.onSwap -= this.UpdateCustomWidth;
		}

		// Token: 0x06000B91 RID: 2961 RVA: 0x0001FDD4 File Offset: 0x0001DFD4
		private void LateUpdate()
		{
			Bounds bounds = this._collider.bounds;
			if (this._customWidth > 0f)
			{
				Vector3 size = bounds.size;
				size.x = this._customWidth;
				bounds.size = size;
			}
			this._shadowRenderer.SetBounds(bounds);
			this._shadowRenderer.Update();
		}

		// Token: 0x04000998 RID: 2456
		private WeaponInventory _weaponInventory;

		// Token: 0x04000999 RID: 2457
		private FootShadowRenderer _shadowRenderer;

		// Token: 0x0400099A RID: 2458
		private Collider2D _collider;

		// Token: 0x0400099B RID: 2459
		private float _customWidth;
	}
}
