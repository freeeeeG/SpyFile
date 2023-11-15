using System;
using UnityEngine;

namespace TwoDLaserPack
{
	// Token: 0x0200165E RID: 5726
	public class PlayerMovement : MonoBehaviour
	{
		// Token: 0x06006D29 RID: 27945 RVA: 0x0013844B File Offset: 0x0013664B
		private void Start()
		{
			if (base.gameObject.GetComponent<Animator>() != null)
			{
				this.playerAnimator = base.gameObject.GetComponent<Animator>();
			}
		}

		// Token: 0x06006D2A RID: 27946 RVA: 0x00138474 File Offset: 0x00136674
		private void moveForward(float amount)
		{
			Vector3 position = new Vector3(base.transform.position.x, base.transform.position.y + amount * Time.deltaTime, base.transform.position.z);
			base.transform.position = position;
		}

		// Token: 0x06006D2B RID: 27947 RVA: 0x001384CC File Offset: 0x001366CC
		private void moveBack(float amount)
		{
			Vector3 position = new Vector3(base.transform.position.x, base.transform.position.y - amount * Time.deltaTime, base.transform.position.z);
			base.transform.position = position;
		}

		// Token: 0x06006D2C RID: 27948 RVA: 0x00138524 File Offset: 0x00136724
		private void moveRight(float amount)
		{
			Vector3 position = new Vector3(base.transform.position.x + amount * Time.deltaTime, base.transform.position.y, base.transform.position.z);
			base.transform.position = position;
		}

		// Token: 0x06006D2D RID: 27949 RVA: 0x0013857C File Offset: 0x0013677C
		private void moveLeft(float amount)
		{
			Vector3 position = new Vector3(base.transform.position.x - amount * Time.deltaTime, base.transform.position.y, base.transform.position.z);
			base.transform.position = position;
		}

		// Token: 0x06006D2E RID: 27950 RVA: 0x00002191 File Offset: 0x00000391
		private void HandlePlayerToggles()
		{
		}

		// Token: 0x06006D2F RID: 27951 RVA: 0x001385D4 File Offset: 0x001367D4
		private void HandlePlayerMovement()
		{
			float axis = Input.GetAxis("Horizontal");
			float axis2 = Input.GetAxis("Vertical");
			if (Mathf.Abs(axis) > 0f || Mathf.Abs(axis2) > 0f)
			{
				this.IsMoving = true;
				if (this.playerAnimator != null)
				{
					this.playerAnimator.SetBool("IsMoving", true);
				}
			}
			else
			{
				this.IsMoving = false;
				if (this.playerAnimator != null)
				{
					this.playerAnimator.SetBool("IsMoving", false);
				}
			}
			Vector2 facingDirection = Vector2.zero;
			PlayerMovement.PlayerMovementType playerMovementType = this.playerMovementType;
			if (playerMovementType != PlayerMovement.PlayerMovementType.Normal)
			{
				if (playerMovementType == PlayerMovement.PlayerMovementType.FreeAim)
				{
					if (axis2 > 0f)
					{
						this.moveForward(this.freeAimMovementSpeed);
					}
					else if (axis2 < 0f)
					{
						this.moveBack(this.freeAimMovementSpeed);
					}
					if (axis > 0f)
					{
						this.moveRight(this.freeAimMovementSpeed);
					}
					else if (axis < 0f)
					{
						this.moveLeft(this.freeAimMovementSpeed);
					}
					facingDirection = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0f)) - base.transform.position;
				}
			}
			else
			{
				if (axis < 0f && this.SmoothSpeedX > -7f)
				{
					this.SmoothSpeedX -= 22f * Time.deltaTime;
				}
				else if (axis > 0f && this.SmoothSpeedX < 7f)
				{
					this.SmoothSpeedX += 22f * Time.deltaTime;
				}
				else if (this.SmoothSpeedX > 33f * Time.deltaTime)
				{
					this.SmoothSpeedX -= 33f * Time.deltaTime;
				}
				else if (this.SmoothSpeedX < -33f * Time.deltaTime)
				{
					this.SmoothSpeedX += 33f * Time.deltaTime;
				}
				else
				{
					this.SmoothSpeedX = 0f;
				}
				if (axis2 < 0f && this.SmoothSpeedY > -7f)
				{
					this.SmoothSpeedY -= 22f * Time.deltaTime;
				}
				else if (axis2 > 0f && this.SmoothSpeedY < 7f)
				{
					this.SmoothSpeedY += 22f * Time.deltaTime;
				}
				else if (this.SmoothSpeedY > 33f * Time.deltaTime)
				{
					this.SmoothSpeedY -= 33f * Time.deltaTime;
				}
				else if (this.SmoothSpeedY < -33f * Time.deltaTime)
				{
					this.SmoothSpeedY += 33f * Time.deltaTime;
				}
				else
				{
					this.SmoothSpeedY = 0f;
				}
				Vector2 v = new Vector2(base.transform.position.x + this.SmoothSpeedX * Time.deltaTime, base.transform.position.y + this.SmoothSpeedY * Time.deltaTime);
				base.transform.position = v;
			}
			this.CalculateAimAndFacingAngles(facingDirection);
			Vector3 vector = Camera.main.WorldToViewportPoint(base.transform.position);
			vector.x = Mathf.Clamp(vector.x, 0.05f, 0.95f);
			vector.y = Mathf.Clamp(vector.y, 0.05f, 0.95f);
			base.transform.position = Camera.main.ViewportToWorldPoint(vector);
		}

		// Token: 0x06006D30 RID: 27952 RVA: 0x00138960 File Offset: 0x00136B60
		private void CalculateAimAndFacingAngles(Vector2 facingDirection)
		{
			this.aimAngle = Mathf.Atan2(facingDirection.y, facingDirection.x);
			if (this.aimAngle < 0f)
			{
				this.aimAngle = 6.2831855f + this.aimAngle;
			}
			base.transform.eulerAngles = new Vector3(0f, 0f, this.aimAngle * 57.29578f);
		}

		// Token: 0x06006D31 RID: 27953 RVA: 0x001389C9 File Offset: 0x00136BC9
		private void Update()
		{
			this.HandlePlayerMovement();
			this.HandlePlayerToggles();
		}

		// Token: 0x040058ED RID: 22765
		public PlayerMovement.PlayerMovementType playerMovementType;

		// Token: 0x040058EE RID: 22766
		public bool IsMoving;

		// Token: 0x040058EF RID: 22767
		public float aimAngle;

		// Token: 0x040058F0 RID: 22768
		[Range(1f, 5f)]
		public float freeAimMovementSpeed = 2f;

		// Token: 0x040058F1 RID: 22769
		private float SmoothSpeedX;

		// Token: 0x040058F2 RID: 22770
		private float SmoothSpeedY;

		// Token: 0x040058F3 RID: 22771
		private const float SmoothMaxSpeedX = 7f;

		// Token: 0x040058F4 RID: 22772
		private const float SmoothMaxSpeedY = 7f;

		// Token: 0x040058F5 RID: 22773
		private const float AccelerationX = 22f;

		// Token: 0x040058F6 RID: 22774
		private const float AccelerationY = 22f;

		// Token: 0x040058F7 RID: 22775
		private const float DecelerationX = 33f;

		// Token: 0x040058F8 RID: 22776
		private const float DecelerationY = 33f;

		// Token: 0x040058F9 RID: 22777
		private Animator playerAnimator;

		// Token: 0x0200165F RID: 5727
		public enum PlayerMovementType
		{
			// Token: 0x040058FB RID: 22779
			Normal,
			// Token: 0x040058FC RID: 22780
			FreeAim
		}
	}
}
