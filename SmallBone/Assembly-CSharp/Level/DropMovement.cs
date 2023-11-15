using System;
using System.Collections;
using System.Collections.Generic;
using PhysicsUtils;
using UnityEngine;

namespace Level
{
	// Token: 0x020004AF RID: 1199
	public class DropMovement : MonoBehaviour
	{
		// Token: 0x06001717 RID: 5911 RVA: 0x000487F8 File Offset: 0x000469F8
		public static void SetMultiDropHorizontalInterval(IList<DropMovement> dropMovements)
		{
			if (dropMovements.Count > 1)
			{
				int num = dropMovements.Count / 2;
				float num2 = (dropMovements.Count % 2 == 0) ? 0.5f : 0f;
				if (dropMovements.Count % 2 == 0)
				{
					num--;
				}
				for (int i = 0; i <= num; i++)
				{
					DropMovement dropMovement = dropMovements[i];
					DropMovement dropMovement2 = dropMovements[dropMovements.Count - 1 - i];
					if (dropMovement == dropMovement2)
					{
						dropMovement.maxHorizontalDistance = 0f;
					}
					else
					{
						float num3 = 1.5f * ((float)(num - i) + num2);
						float num4 = num3;
						dropMovement.horizontalSpeed = -num4;
						dropMovement.maxHorizontalDistance = num3;
						dropMovement2.horizontalSpeed = num4;
						dropMovement2.maxHorizontalDistance = num3;
					}
				}
			}
		}

		// Token: 0x1400001E RID: 30
		// (add) Token: 0x06001718 RID: 5912 RVA: 0x000488B4 File Offset: 0x00046AB4
		// (remove) Token: 0x06001719 RID: 5913 RVA: 0x000488EC File Offset: 0x00046AEC
		public event Action onGround;

		// Token: 0x1700048F RID: 1167
		// (get) Token: 0x0600171A RID: 5914 RVA: 0x00048921 File Offset: 0x00046B21
		// (set) Token: 0x0600171B RID: 5915 RVA: 0x00048929 File Offset: 0x00046B29
		public bool floating
		{
			get
			{
				return this._floating;
			}
			set
			{
				this._floating = value;
				this._graphic.localPosition = Vector3.zero;
			}
		}

		// Token: 0x0600171C RID: 5916 RVA: 0x00048942 File Offset: 0x00046B42
		public void ResetValue()
		{
			this._minDistanceFromGround = this._cachedMinDistanceFromGround;
			this._floating = true;
		}

		// Token: 0x0600171D RID: 5917 RVA: 0x00048957 File Offset: 0x00046B57
		public void SetMultipleRewardMovement(float mindistanceFromSides)
		{
			this._cachedMinDistanceFromGround = this._minDistanceFromGround;
			this._minDistanceFromGround = mindistanceFromSides;
			this._floating = false;
		}

		// Token: 0x0600171E RID: 5918 RVA: 0x00048973 File Offset: 0x00046B73
		public void Pause()
		{
			base.StopAllCoroutines();
		}

		// Token: 0x0600171F RID: 5919 RVA: 0x0004897B File Offset: 0x00046B7B
		public void Stop()
		{
			base.StopAllCoroutines();
			Action action = this.onGround;
			if (action == null)
			{
				return;
			}
			action();
		}

		// Token: 0x06001720 RID: 5920 RVA: 0x00048993 File Offset: 0x00046B93
		public void Float()
		{
			base.StartCoroutine(this.CFloat());
		}

		// Token: 0x06001721 RID: 5921 RVA: 0x000489A4 File Offset: 0x00046BA4
		private void Awake()
		{
			ContactFilter2D contactFilter = default(ContactFilter2D);
			contactFilter.SetLayerMask(Layers.groundMask);
			this._aboveCaster = new RayCaster
			{
				direction = Vector2.up,
				contactFilter = contactFilter
			};
			this._belowCaster = new RayCaster
			{
				direction = Vector2.down,
				contactFilter = contactFilter
			};
			this._leftCaster = new RayCaster
			{
				direction = Vector2.left,
				contactFilter = contactFilter
			};
			this._rightCaster = new RayCaster
			{
				direction = Vector2.right,
				contactFilter = contactFilter
			};
		}

		// Token: 0x06001722 RID: 5922 RVA: 0x00048A3C File Offset: 0x00046C3C
		private void OnEnable()
		{
			this._speed = new Vector2(0f, this._jumpPower);
			this._movedHorizontalDistance = 0f;
			this.horizontalSpeed = 0f;
			this.maxHorizontalDistance = 0f;
			base.StartCoroutine(this.CMove());
		}

		// Token: 0x06001723 RID: 5923 RVA: 0x00048A90 File Offset: 0x00046C90
		public void Move()
		{
			this._speed = new Vector2(0f, this._jumpPower);
			this._movedHorizontalDistance = 0f;
			this.horizontalSpeed = 0f;
			this.maxHorizontalDistance = 0f;
			base.StopAllCoroutines();
			base.StartCoroutine(this.CMove());
		}

		// Token: 0x06001724 RID: 5924 RVA: 0x00048AE7 File Offset: 0x00046CE7
		public void Move(float speedX, float sppedY)
		{
			this._speed = new Vector2(speedX, sppedY);
			this._movedHorizontalDistance = 0f;
			this.horizontalSpeed = speedX;
			this.maxHorizontalDistance = 2.1474836E+09f;
			base.StopAllCoroutines();
			base.StartCoroutine(this.CMove());
		}

		// Token: 0x06001725 RID: 5925 RVA: 0x00048B26 File Offset: 0x00046D26
		private IEnumerator CMove()
		{
			yield return null;
			bool moveVertical = true;
			bool moveHorizontal = true;
			for (;;)
			{
				float deltaTime = Chronometer.global.deltaTime;
				if (moveVertical)
				{
					this._speed.y = this._speed.y - this._gravity * deltaTime;
					if (this._speed.y > 0f)
					{
						this._aboveCaster.origin = base.transform.position;
						this._aboveCaster.distance = this._minDistanceFromGround + this._speed.y * Time.deltaTime;
						if (this._aboveCaster.SingleCast())
						{
							this._speed.y = 0f;
						}
					}
					else
					{
						this._belowCaster.origin = base.transform.position;
						this._belowCaster.distance = this._minDistanceFromGround - this._speed.y * Time.deltaTime;
						this._belowCaster.contactFilter.SetLayerMask(Layers.groundMask);
						RaycastHit2D hit = this._belowCaster.SingleCast();
						if (hit)
						{
							base.transform.position = hit.point + new Vector2(0f, this._minDistanceFromGround);
							this._speed.y = 0f;
							moveVertical = false;
							this.Stop();
							this.Float();
						}
					}
				}
				if (moveHorizontal)
				{
					this._leftCaster.origin = base.transform.position;
					this._leftCaster.distance = this._minDistanceFromSides + Mathf.Abs(this._speed.x * Time.deltaTime);
					RaycastHit2D hit2 = this._leftCaster.SingleCast();
					this._rightCaster.origin = base.transform.position;
					this._rightCaster.distance = this._minDistanceFromSides + Mathf.Abs(this._speed.x * Time.deltaTime);
					RaycastHit2D hit3 = this._rightCaster.SingleCast();
					if (hit2 && hit2.distance <= this._minDistanceFromSides)
					{
						this._speed.x = this._speed.x + 2f * deltaTime;
					}
					else if (hit3 && hit3.distance <= this._minDistanceFromSides)
					{
						this._speed.x = this._speed.x - 2f * deltaTime;
					}
					else if (this._movedHorizontalDistance < this.maxHorizontalDistance)
					{
						this._speed.x = this.horizontalSpeed;
						this._movedHorizontalDistance += Mathf.Abs(this._speed.x * deltaTime);
					}
					else
					{
						this._speed.x = 0f;
						moveHorizontal = false;
					}
				}
				if (!moveHorizontal && !moveVertical)
				{
					break;
				}
				if (this._speed.y < -this._maxFallSpeed)
				{
					this._speed.y = -this._maxFallSpeed;
				}
				base.transform.Translate(this._speed * deltaTime);
				yield return null;
			}
			this.Stop();
			this.Float();
			yield break;
			yield break;
		}

		// Token: 0x06001726 RID: 5926 RVA: 0x00048B35 File Offset: 0x00046D35
		private IEnumerator CFloat()
		{
			float t = 0f;
			for (;;)
			{
				yield return null;
				if (this._floating)
				{
					Vector3 zero = Vector3.zero;
					t += Chronometer.global.deltaTime;
					zero.y = Mathf.Sin(t * 3.1415927f * this._floatFrequency) * this._floatAmplitude;
					this._graphic.localPosition = zero;
				}
			}
			yield break;
		}

		// Token: 0x04001434 RID: 5172
		private const float _droppedGearHorizontalInterval = 1.5f;

		// Token: 0x04001435 RID: 5173
		private const float _droppedGearBasicHorizontalSpeed = 0.5f;

		// Token: 0x04001437 RID: 5175
		[SerializeField]
		private Transform _graphic;

		// Token: 0x04001438 RID: 5176
		[SerializeField]
		private float _minDistanceFromGround = 1f;

		// Token: 0x04001439 RID: 5177
		[SerializeField]
		private float _minDistanceFromSides = 1f;

		// Token: 0x0400143A RID: 5178
		[SerializeField]
		private float _jumpPower = 15f;

		// Token: 0x0400143B RID: 5179
		[SerializeField]
		private float _gravity = 40f;

		// Token: 0x0400143C RID: 5180
		[SerializeField]
		private float _maxFallSpeed = 40f;

		// Token: 0x0400143D RID: 5181
		[SerializeField]
		private float _floatAmplitude = 0.5f;

		// Token: 0x0400143E RID: 5182
		[SerializeField]
		private float _floatFrequency = 1f;

		// Token: 0x0400143F RID: 5183
		private float _movedHorizontalDistance;

		// Token: 0x04001440 RID: 5184
		[NonSerialized]
		public float horizontalSpeed;

		// Token: 0x04001441 RID: 5185
		[NonSerialized]
		public float maxHorizontalDistance;

		// Token: 0x04001442 RID: 5186
		private bool _floating = true;

		// Token: 0x04001443 RID: 5187
		private Vector2 _speed;

		// Token: 0x04001444 RID: 5188
		private RayCaster _aboveCaster;

		// Token: 0x04001445 RID: 5189
		private RayCaster _belowCaster;

		// Token: 0x04001446 RID: 5190
		private RayCaster _leftCaster;

		// Token: 0x04001447 RID: 5191
		private RayCaster _rightCaster;

		// Token: 0x04001448 RID: 5192
		private float _cachedMinDistanceFromGround;
	}
}
