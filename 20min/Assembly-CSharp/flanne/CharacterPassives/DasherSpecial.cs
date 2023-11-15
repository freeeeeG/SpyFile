using System;
using System.Collections;
using flanne.Core;
using UnityEngine;

namespace flanne.CharacterPassives
{
	// Token: 0x0200025B RID: 603
	public class DasherSpecial : MonoBehaviour
	{
		// Token: 0x17000105 RID: 261
		// (get) Token: 0x06000D11 RID: 3345 RVA: 0x0002F9E7 File Offset: 0x0002DBE7
		public int finalDamage
		{
			get
			{
				return Mathf.FloorToInt(this.player.stats[StatType.SummonDamage].Modify(this.player.stats[StatType.MoveSpeed].Modify((float)this.baseDamage)));
			}
		}

		// Token: 0x17000106 RID: 262
		// (get) Token: 0x06000D12 RID: 3346 RVA: 0x0002FA22 File Offset: 0x0002DC22
		public int finalKnockback
		{
			get
			{
				return Mathf.Max(this.baseKnockback, Mathf.FloorToInt(this.player.stats[StatType.CharacterSize].Modify((float)this.baseKnockback)));
			}
		}

		// Token: 0x06000D13 RID: 3347 RVA: 0x0002FA54 File Offset: 0x0002DC54
		private void Start()
		{
			this.player = PlayerController.Instance;
			this.flasher = this.player.GetComponentInChildren<PlayerFlasher>();
			this.spriteTrail = this.player.playerSprite.GetComponentInChildren<SpriteTrail>();
			this.hitbox = Object.Instantiate<HarmfulOnContact>(this.hitboxPrefab);
			this.SetParentToPlayerSprite(this.hitbox.transform);
			this.hitbox.gameObject.SetActive(false);
			this.knockback = this.hitbox.GetComponent<Knockback>();
			this.transformToDeerAnim = Object.Instantiate<GameObject>(this.transformToDeerAnimPrefab);
			this.SetParentToPlayerSprite(this.transformToDeerAnim.transform);
			this.transformToDeerAnim.gameObject.SetActive(false);
			this.transformToHumanAnim = Object.Instantiate<GameObject>(this.transformToHumanAnimPrefab);
			this.SetParentToPlayerSprite(this.transformToHumanAnim.transform);
			this.transformToHumanAnim.gameObject.SetActive(false);
			this.OP = ObjectPooler.SharedInstance;
			this.OP.AddObject(this.projectilePrefab.name, this.projectilePrefab.gameObject, 100, true);
			this.PC = PauseController.SharedInstance;
		}

		// Token: 0x06000D14 RID: 3348 RVA: 0x0002FB78 File Offset: 0x0002DD78
		private void Update()
		{
			if (this.player.playerHealth.hp == 0)
			{
				return;
			}
			if (!this._isTransformed)
			{
				this._transformTimer += Time.deltaTime;
				if (this._transformTimer > this.timeToActivate)
				{
					this._transformTimer -= this.timeToActivate;
					base.StartCoroutine(this.TransformCR());
					return;
				}
			}
			else
			{
				this.player.playerSprite.transform.position += this.moveSpeedMultiplier * this.player.moveVec * this.player.finalMoveSpeed * Time.deltaTime;
				Vector3 position = this.player.playerSprite.transform.position;
				this.player.playerSprite.transform.localPosition = Vector3.zero;
				this.player.transform.position = position;
				this._shootTimer += Time.deltaTime;
				if (this._shootTimer >= this.shootCD)
				{
					this._shootTimer -= this.shootCD;
					for (int i = 0; i < this.projectiles; i++)
					{
						this.Shoot();
					}
				}
			}
		}

		// Token: 0x06000D15 RID: 3349 RVA: 0x0002FCBF File Offset: 0x0002DEBF
		private IEnumerator TransformCR()
		{
			this.PC.Pause(0.2f);
			this._isTransformed = true;
			this.player.gun.SetVisible(false);
			this.hitbox.gameObject.SetActive(true);
			this.hitbox.damageAmount = this.finalDamage;
			this.knockback.knockbackForce = (float)this.finalKnockback;
			this.transformToDeerAnim.SetActive(true);
			this.player.playerHealth.isInvincible.Flip();
			this.player.disableAction.Flip();
			this.player.disableMove.Flip();
			this.player.disableAnimation.Flip();
			this.transformToDeerSFX.Play(null);
			yield return new WaitForSeconds(0.2f);
			this.player.playerAnimator.ResetTrigger("Idle");
			this.player.playerAnimator.ResetTrigger("Run");
			this.player.playerAnimator.ResetTrigger("Walk");
			this.player.playerAnimator.SetTrigger("Special");
			this.spriteTrail.SetEnabled(true);
			yield return new WaitForSeconds(this.baseDuration - this.warningDuration);
			this.flasher.Flash();
			this.transformWarningSFX.Play(null);
			yield return new WaitForSeconds(this.warningDuration);
			this.flasher.StopFlash();
			this.player.gun.SetVisible(true);
			this.hitbox.gameObject.SetActive(false);
			this.transformToHumanAnim.SetActive(true);
			this.player.disableAction.UnFlip();
			this.player.disableMove.UnFlip();
			this.player.disableAnimation.UnFlip();
			this.player.playerAnimator.ResetTrigger("Special");
			this.player.playerAnimator.SetTrigger("Idle");
			this.spriteTrail.SetEnabled(false);
			this.transformToHumanSFX.Play(null);
			this.PC.Pause(0.2f);
			this._isTransformed = false;
			yield return new WaitForSeconds(0.5f);
			this.player.playerHealth.isInvincible.UnFlip();
			yield break;
		}

		// Token: 0x06000D16 RID: 3350 RVA: 0x0002FCD0 File Offset: 0x0002DED0
		private void Shoot()
		{
			GameObject pooledObject = this.OP.GetPooledObject(this.projectilePrefab.name);
			pooledObject.transform.position = this.firepoint.position;
			pooledObject.SetActive(true);
			pooledObject.GetComponent<Projectile>().damage = (float)this.finalDamage * this.projDamageMultiplier;
		}

		// Token: 0x06000D17 RID: 3351 RVA: 0x0002FD28 File Offset: 0x0002DF28
		private void SetParentToPlayerSprite(Transform t)
		{
			t.SetParent(this.player.playerSprite.transform);
			t.localPosition = Vector3.zero;
			t.localScale = Vector3.one;
		}

		// Token: 0x0400095E RID: 2398
		[SerializeField]
		private HarmfulOnContact hitboxPrefab;

		// Token: 0x0400095F RID: 2399
		[SerializeField]
		private GameObject transformToDeerAnimPrefab;

		// Token: 0x04000960 RID: 2400
		[SerializeField]
		private GameObject transformToHumanAnimPrefab;

		// Token: 0x04000961 RID: 2401
		[SerializeField]
		private int baseDamage = 100;

		// Token: 0x04000962 RID: 2402
		[SerializeField]
		private int baseKnockback = 10;

		// Token: 0x04000963 RID: 2403
		[SerializeField]
		private float baseDuration = 10f;

		// Token: 0x04000964 RID: 2404
		[SerializeField]
		private float warningDuration = 2f;

		// Token: 0x04000965 RID: 2405
		[SerializeField]
		private float timeToActivate = 10f;

		// Token: 0x04000966 RID: 2406
		[SerializeField]
		private float moveSpeedMultiplier = 1.75f;

		// Token: 0x04000967 RID: 2407
		[SerializeField]
		private SoundEffectSO transformToDeerSFX;

		// Token: 0x04000968 RID: 2408
		[SerializeField]
		private SoundEffectSO transformToHumanSFX;

		// Token: 0x04000969 RID: 2409
		[SerializeField]
		private SoundEffectSO transformWarningSFX;

		// Token: 0x0400096A RID: 2410
		[Header("Projectile")]
		public int projectiles;

		// Token: 0x0400096B RID: 2411
		[SerializeField]
		private Projectile projectilePrefab;

		// Token: 0x0400096C RID: 2412
		[SerializeField]
		private Transform firepoint;

		// Token: 0x0400096D RID: 2413
		[SerializeField]
		private float shootCD;

		// Token: 0x0400096E RID: 2414
		[SerializeField]
		private float projDamageMultiplier;

		// Token: 0x0400096F RID: 2415
		private PlayerController player;

		// Token: 0x04000970 RID: 2416
		private PlayerFlasher flasher;

		// Token: 0x04000971 RID: 2417
		private SpriteTrail spriteTrail;

		// Token: 0x04000972 RID: 2418
		private HarmfulOnContact hitbox;

		// Token: 0x04000973 RID: 2419
		private Knockback knockback;

		// Token: 0x04000974 RID: 2420
		private GameObject transformToDeerAnim;

		// Token: 0x04000975 RID: 2421
		private GameObject transformToHumanAnim;

		// Token: 0x04000976 RID: 2422
		private ObjectPooler OP;

		// Token: 0x04000977 RID: 2423
		private PauseController PC;

		// Token: 0x04000978 RID: 2424
		private bool _isTransformed;

		// Token: 0x04000979 RID: 2425
		private float _transformTimer;

		// Token: 0x0400097A RID: 2426
		private float _shootTimer;
	}
}
