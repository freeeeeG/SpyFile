using System;
using UnityEngine;

namespace Dustyroom
{
	// Token: 0x02000059 RID: 89
	public class OrbitMotion : MonoBehaviour
	{
		// Token: 0x06000109 RID: 265 RVA: 0x000054B4 File Offset: 0x000036B4
		private void Start()
		{
			Vector3 eulerAngles = base.transform.eulerAngles;
			this._x = eulerAngles.y;
			this._y = eulerAngles.x;
			Rigidbody component = base.GetComponent<Rigidbody>();
			if (component != null)
			{
				component.freezeRotation = true;
			}
			if (this.targetMode == OrbitMotion.TargetMode.Transform)
			{
				if (this.targetTransform != null)
				{
					this.targetPosition = this.targetTransform.position + this.targetOffset;
					return;
				}
				Debug.LogWarning("Reference transform is not set.");
			}
		}

		// Token: 0x0600010A RID: 266 RVA: 0x0000553C File Offset: 0x0000373C
		private void Update()
		{
			if (this.targetMode == OrbitMotion.TargetMode.Transform && this.followTargetTransform)
			{
				if (this.targetTransform != null)
				{
					this.targetPosition = this.targetTransform.position + this.targetOffset;
				}
				else
				{
					Debug.LogWarning("Reference transform is not set.");
				}
			}
			if (Mathf.Abs(Input.GetAxis("Mouse X")) + Mathf.Abs(Input.GetAxis("Mouse Y")) > 0f)
			{
				this._lastMoveTime = Time.time;
			}
			this.timeSinceLastMove = Time.time - this._lastMoveTime;
			if (this.interactive && Input.GetMouseButton(0))
			{
				this._x += Input.GetAxis("Mouse X") * this.xSpeed * 40f * 0.02f;
				this._y -= Input.GetAxis("Mouse Y") * this.ySpeed * 40f * 0.02f;
			}
			else if (this.autoMovement)
			{
				this._x += this.autoSpeedX * 40f * Time.deltaTime * 10f;
				this._y -= this.autoSpeedY * 40f * Time.deltaTime * 10f;
				this.distanceHorizontal += this.autoSpeedDistance;
			}
			if (this.clampAngle)
			{
				this._y = OrbitMotion.ClampAngle(this._y, this.yMinLimit, this.yMaxLimit);
			}
			Quaternion rotation = Quaternion.Slerp(base.transform.rotation, Quaternion.Euler(this._y, this._x, 0f), Time.deltaTime * this.damping);
			if (this.allowZoom)
			{
				this.distanceHorizontal = Mathf.Clamp(this.distanceHorizontal - Input.GetAxis("Mouse ScrollWheel") * 5f, this.distanceMin, this.distanceMax);
			}
			float num = rotation.eulerAngles.x;
			if (num > 90f)
			{
				num -= 360f;
			}
			float num2 = Mathf.Lerp(this.distanceHorizontal, this.distanceVertical, Mathf.Abs(num / 90f));
			Vector3 point = new Vector3(0f, 0f, -num2);
			Vector3 position = rotation * point + this.targetPosition;
			base.transform.rotation = rotation;
			base.transform.position = position;
		}

		// Token: 0x0600010B RID: 267 RVA: 0x000057A8 File Offset: 0x000039A8
		private static float ClampAngle(float angle, float min, float max)
		{
			if (angle < -360f)
			{
				angle += 360f;
			}
			if (angle > 360f)
			{
				angle -= 360f;
			}
			return Mathf.Clamp(angle, min, max);
		}

		// Token: 0x040000F1 RID: 241
		public OrbitMotion.TargetMode targetMode = OrbitMotion.TargetMode.Position;

		// Token: 0x040000F2 RID: 242
		public Transform targetTransform;

		// Token: 0x040000F3 RID: 243
		public bool followTargetTransform = true;

		// Token: 0x040000F4 RID: 244
		public Vector3 targetOffset = Vector3.zero;

		// Token: 0x040000F5 RID: 245
		public Vector3 targetPosition;

		// Token: 0x040000F6 RID: 246
		[Space]
		public float distanceHorizontal = 60f;

		// Token: 0x040000F7 RID: 247
		public float distanceVertical = 60f;

		// Token: 0x040000F8 RID: 248
		public float xSpeed = 120f;

		// Token: 0x040000F9 RID: 249
		public float ySpeed = 120f;

		// Token: 0x040000FA RID: 250
		public float damping = 3f;

		// Token: 0x040000FB RID: 251
		[Space]
		public bool clampAngle;

		// Token: 0x040000FC RID: 252
		public float yMinLimit = -20f;

		// Token: 0x040000FD RID: 253
		public float yMaxLimit = 80f;

		// Token: 0x040000FE RID: 254
		[Space]
		public bool allowZoom;

		// Token: 0x040000FF RID: 255
		public float distanceMin = 0.5f;

		// Token: 0x04000100 RID: 256
		public float distanceMax = 15f;

		// Token: 0x04000101 RID: 257
		private float _x;

		// Token: 0x04000102 RID: 258
		private float _y;

		// Token: 0x04000103 RID: 259
		[Space]
		public bool autoMovement;

		// Token: 0x04000104 RID: 260
		public float autoSpeedX = 0.2f;

		// Token: 0x04000105 RID: 261
		public float autoSpeedY = 0.1f;

		// Token: 0x04000106 RID: 262
		public float autoSpeedDistance = -0.1f;

		// Token: 0x04000107 RID: 263
		[Space]
		public bool interactive = true;

		// Token: 0x04000108 RID: 264
		private float _lastMoveTime;

		// Token: 0x04000109 RID: 265
		[HideInInspector]
		public float timeSinceLastMove;

		// Token: 0x020000F8 RID: 248
		public enum TargetMode
		{
			// Token: 0x0400039B RID: 923
			Transform,
			// Token: 0x0400039C RID: 924
			Position
		}
	}
}
