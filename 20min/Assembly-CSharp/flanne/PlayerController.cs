using System;
using flanne.Core;
using flanne.PerkSystem;
using flanne.Player;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace flanne
{
	// Token: 0x020000F7 RID: 247
	public class PlayerController : StateMachine
	{
		// Token: 0x17000071 RID: 113
		// (get) Token: 0x06000716 RID: 1814 RVA: 0x0001F3B7 File Offset: 0x0001D5B7
		// (set) Token: 0x06000717 RID: 1815 RVA: 0x0001F3BF File Offset: 0x0001D5BF
		public Vector3 moveVec { get; private set; }

		// Token: 0x17000072 RID: 114
		// (get) Token: 0x06000718 RID: 1816 RVA: 0x0001F3C8 File Offset: 0x0001D5C8
		public float finalMoveSpeed
		{
			get
			{
				return this.stats[StatType.MoveSpeed].Modify(this.movementSpeed);
			}
		}

		// Token: 0x06000719 RID: 1817 RVA: 0x0001F3E4 File Offset: 0x0001D5E4
		private void OnMove(InputAction.CallbackContext obj)
		{
			Vector2 vector = obj.ReadValue<Vector2>();
			this.moveVec = new Vector3(vector.x, vector.y, 0f).normalized;
		}

		// Token: 0x0600071A RID: 1818 RVA: 0x0001F420 File Offset: 0x0001D620
		private void Awake()
		{
			if (PlayerController.Instance != null)
			{
				Object.Destroy(base.gameObject);
				return;
			}
			PlayerController.Instance = this;
			this.disableMove = new BoolToggle(false);
			this.disableAction = new BoolToggle(false);
			this.disableAnimation = new BoolToggle(false);
			this.disableFacing = new BoolToggle(false);
			this.ChangeState<IdleState>();
			this._moveAction = this.playerInput.actions["Move"];
		}

		// Token: 0x0600071B RID: 1819 RVA: 0x0001F49D File Offset: 0x0001D69D
		private void Start()
		{
			this.SC = ShootingCursor.Instance;
		}

		// Token: 0x0600071C RID: 1820 RVA: 0x0001F4AA File Offset: 0x0001D6AA
		private void Update()
		{
			if (PauseController.isPaused || this.playerHealth.hp == 0)
			{
				return;
			}
			this.MovePlayer();
			this.UpdateSprite();
		}

		// Token: 0x0600071D RID: 1821 RVA: 0x0001F4CD File Offset: 0x0001D6CD
		private void OnEnable()
		{
			this._moveAction.performed += this.OnMove;
		}

		// Token: 0x0600071E RID: 1822 RVA: 0x0001F4E6 File Offset: 0x0001D6E6
		private void OnDisable()
		{
			this._moveAction.performed -= this.OnMove;
			this._currentState.Exit();
		}

		// Token: 0x0600071F RID: 1823 RVA: 0x0001F50A File Offset: 0x0001D70A
		private void OnDestroy()
		{
			this.onDestroyed.Invoke();
		}

		// Token: 0x06000720 RID: 1824 RVA: 0x0001F517 File Offset: 0x0001D717
		public void SetPosition(Vector2 pos)
		{
			this.playerSprite.transform.localPosition = Vector3.zero;
			base.transform.position = pos;
		}

		// Token: 0x06000721 RID: 1825 RVA: 0x0001F53F File Offset: 0x0001D73F
		public void KnockbackNearby()
		{
			this.knockbackObject.SetActive(true);
		}

		// Token: 0x06000722 RID: 1826 RVA: 0x0001F550 File Offset: 0x0001D750
		private void MovePlayer()
		{
			if (this.disableMove.value)
			{
				return;
			}
			Vector3 v = this.playerSprite.transform.position + this.moveSpeedMultiplier * this.moveVec * this.stats[StatType.MoveSpeed].Modify(this.movementSpeed) * Time.deltaTime;
			this.SetPosition(v);
		}

		// Token: 0x06000723 RID: 1827 RVA: 0x0001F5C5 File Offset: 0x0001D7C5
		private void UpdateSprite()
		{
			this.UpdateAnimation();
			this.UpdateFacing();
		}

		// Token: 0x06000724 RID: 1828 RVA: 0x0001F5D4 File Offset: 0x0001D7D4
		private void UpdateAnimation()
		{
			if (this.disableAnimation.value)
			{
				return;
			}
			if (this.moveVec == Vector3.zero || this.disableMove.value)
			{
				this.playerAnimator.ResetTrigger("Run");
				this.playerAnimator.ResetTrigger("Walk");
				this.playerAnimator.SetTrigger("Idle");
				return;
			}
			if (this.moveSpeedMultiplier >= 1f)
			{
				this.playerAnimator.ResetTrigger("Idle");
				this.playerAnimator.ResetTrigger("Walk");
				this.playerAnimator.SetTrigger("Run");
				this.timerFootStepSFX += Time.deltaTime;
				if (this.timerFootStepSFX > this.footstepSFXCooldown)
				{
					this.timerFootStepSFX -= this.footstepSFXCooldown;
					this.footstepSFX.Play(null);
					return;
				}
			}
			else
			{
				this.playerAnimator.ResetTrigger("Idle");
				this.playerAnimator.ResetTrigger("Run");
				this.playerAnimator.SetTrigger("Walk");
			}
		}

		// Token: 0x06000725 RID: 1829 RVA: 0x0001F6F0 File Offset: 0x0001D8F0
		private void UpdateFacing()
		{
			if (this.disableFacing.value)
			{
				return;
			}
			if (this.playerSprite != null)
			{
				if (this.faceMouse)
				{
					Vector3 a = Camera.main.ScreenToWorldPoint(this.SC.cursorPosition);
					Vector3 position = base.transform.position;
					Vector3 vector = a - position;
					if (vector.x < 0f)
					{
						this.playerSprite.flipX = true;
						return;
					}
					if (vector.x > 0f)
					{
						this.playerSprite.flipX = false;
						return;
					}
				}
				else
				{
					if (this.moveVec.x < 0f)
					{
						this.playerSprite.flipX = true;
						return;
					}
					if (this.moveVec.x > 0f)
					{
						this.playerSprite.flipX = false;
						return;
					}
				}
			}
			else
			{
				Debug.LogError("No sprite renderer on player");
			}
		}

		// Token: 0x040004E1 RID: 1249
		public static PlayerController Instance;

		// Token: 0x040004E2 RID: 1250
		public UnityEvent onDestroyed;

		// Token: 0x040004E3 RID: 1251
		public PlayerInput playerInput;

		// Token: 0x040004E4 RID: 1252
		public SpriteRenderer playerSprite;

		// Token: 0x040004E5 RID: 1253
		public Animator playerAnimator;

		// Token: 0x040004E6 RID: 1254
		public PlayerHealth playerHealth;

		// Token: 0x040004E7 RID: 1255
		public PlayerBuffs playerBuffs;

		// Token: 0x040004E8 RID: 1256
		public StatsHolder stats;

		// Token: 0x040004E9 RID: 1257
		public Gun gun;

		// Token: 0x040004EA RID: 1258
		public Ammo ammo;

		// Token: 0x040004EB RID: 1259
		public Slider reloadBar;

		// Token: 0x040004EC RID: 1260
		public CharacterData loadedCharacter;

		// Token: 0x040004ED RID: 1261
		public PlayerPerks playerPerks;

		// Token: 0x040004EE RID: 1262
		public GameObject knockbackObject;

		// Token: 0x040004EF RID: 1263
		public SoundEffectSO reloadStartSFX;

		// Token: 0x040004F0 RID: 1264
		public SoundEffectSO reloadEndSFX;

		// Token: 0x040004F1 RID: 1265
		public float movementSpeed = 8f;

		// Token: 0x040004F2 RID: 1266
		[SerializeField]
		private SoundEffectSO footstepSFX;

		// Token: 0x040004F3 RID: 1267
		[SerializeField]
		private float footstepSFXCooldown;

		// Token: 0x040004F4 RID: 1268
		private float timerFootStepSFX;

		// Token: 0x040004F5 RID: 1269
		[NonSerialized]
		public float moveSpeedMultiplier;

		// Token: 0x040004F6 RID: 1270
		[NonSerialized]
		public bool faceMouse;

		// Token: 0x040004F7 RID: 1271
		public BoolToggle disableMove;

		// Token: 0x040004F8 RID: 1272
		public BoolToggle disableAction;

		// Token: 0x040004F9 RID: 1273
		public BoolToggle disableAnimation;

		// Token: 0x040004FA RID: 1274
		public BoolToggle disableFacing;

		// Token: 0x040004FC RID: 1276
		private ShootingCursor SC;

		// Token: 0x040004FD RID: 1277
		private InputAction _moveAction;
	}
}
