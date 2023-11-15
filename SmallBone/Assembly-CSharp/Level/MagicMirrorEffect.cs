using System;
using Characters;
using Characters.Player;
using Services;
using Singletons;
using UnityEngine;

namespace Level
{
	// Token: 0x020004EF RID: 1263
	public sealed class MagicMirrorEffect : MonoBehaviour
	{
		// Token: 0x060018CA RID: 6346 RVA: 0x0004DB10 File Offset: 0x0004BD10
		private void Start()
		{
			this._player = Singleton<Service>.Instance.levelManager.player;
			this._weaponInventory = this._player.playerComponents.inventory.weapon;
			this._playerTransform = this._player.transform;
		}

		// Token: 0x060018CB RID: 6347 RVA: 0x0004DB60 File Offset: 0x0004BD60
		private void Update()
		{
			if (this._weaponInventory == null)
			{
				return;
			}
			this._mirrorSkul.sprite = this._weaponInventory.polymorphOrCurrent.characterAnimation.spriteRenderer.sprite;
			this._mirrorSkul.flipX = (this._player.lookingDirection == Character.LookingDirection.Left);
			float num = Mathf.Abs(base.transform.position.x - this._playerTransform.position.x);
			Vector3 normalized = (this._playerTransform.position - base.transform.position).normalized;
			if (num > this._hideDistance)
			{
				this._mirrorSkul.gameObject.SetActive(false);
				return;
			}
			this._mirrorSkul.gameObject.SetActive(true);
			Vector3 position = base.transform.position + this._movementDelta * num * normalized;
			position.y = this._playerTransform.position.y + 0.26f;
			this._mirrorSkul.transform.position = position;
		}

		// Token: 0x0400159E RID: 5534
		[SerializeField]
		private SpriteRenderer _mirrorSkul;

		// Token: 0x0400159F RID: 5535
		[SerializeField]
		private float _hideDistance = 5f;

		// Token: 0x040015A0 RID: 5536
		[SerializeField]
		private float _movementDelta;

		// Token: 0x040015A1 RID: 5537
		private Character _player;

		// Token: 0x040015A2 RID: 5538
		private Transform _playerTransform;

		// Token: 0x040015A3 RID: 5539
		private WeaponInventory _weaponInventory;
	}
}
