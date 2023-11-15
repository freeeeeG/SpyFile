using System;
using UnityEngine;

namespace Dustyroom
{
	// Token: 0x02000058 RID: 88
	public class LinearMotion : MonoBehaviour
	{
		// Token: 0x1700002E RID: 46
		// (get) Token: 0x06000104 RID: 260 RVA: 0x000052DC File Offset: 0x000034DC
		private Vector3 TranslationVector
		{
			get
			{
				switch (this.translationMode)
				{
				case LinearMotion.TranslationMode.Off:
					return Vector3.zero;
				case LinearMotion.TranslationMode.XAxis:
					return Vector3.right;
				case LinearMotion.TranslationMode.YAxis:
					return Vector3.up;
				case LinearMotion.TranslationMode.ZAxis:
					return Vector3.forward;
				case LinearMotion.TranslationMode.Vector:
					return this.translationVector;
				default:
					throw new ArgumentOutOfRangeException();
				}
			}
		}

		// Token: 0x1700002F RID: 47
		// (get) Token: 0x06000105 RID: 261 RVA: 0x00005330 File Offset: 0x00003530
		private Vector3 RotationVector
		{
			get
			{
				switch (this.rotationMode)
				{
				case LinearMotion.RotationMode.Off:
					return Vector3.zero;
				case LinearMotion.RotationMode.XAxis:
					return Vector3.right;
				case LinearMotion.RotationMode.YAxis:
					return Vector3.up;
				case LinearMotion.RotationMode.ZAxis:
					return Vector3.forward;
				case LinearMotion.RotationMode.Vector:
					return this.rotationAxis;
				default:
					throw new ArgumentOutOfRangeException();
				}
			}
		}

		// Token: 0x06000106 RID: 262 RVA: 0x00005384 File Offset: 0x00003584
		private void Update()
		{
			if (this.translationMode != LinearMotion.TranslationMode.Off)
			{
				Vector3 b = this.TranslationVector * this.translationSpeed * Time.deltaTime;
				if (this.useLocalCoordinate)
				{
					base.transform.localPosition += b;
				}
				else
				{
					base.transform.position += b;
				}
			}
			if (this.rotationMode == LinearMotion.RotationMode.Off)
			{
				return;
			}
			Quaternion lhs = Quaternion.AngleAxis(this.rotationSpeed * Time.deltaTime, this.RotationVector);
			if (this.useLocalCoordinate)
			{
				base.transform.localRotation = lhs * base.transform.localRotation;
				return;
			}
			base.transform.rotation = lhs * base.transform.rotation;
		}

		// Token: 0x06000107 RID: 263 RVA: 0x0000544F File Offset: 0x0000364F
		private void FixedUpdate()
		{
			this.translationSpeed += this.translationAcceleration;
			this.rotationSpeed += this.rotationAcceleration;
		}

		// Token: 0x040000E8 RID: 232
		public LinearMotion.TranslationMode translationMode;

		// Token: 0x040000E9 RID: 233
		public Vector3 translationVector = Vector3.forward;

		// Token: 0x040000EA RID: 234
		public float translationSpeed = 1f;

		// Token: 0x040000EB RID: 235
		public LinearMotion.RotationMode rotationMode;

		// Token: 0x040000EC RID: 236
		public Vector3 rotationAxis = Vector3.up;

		// Token: 0x040000ED RID: 237
		public float rotationSpeed = 50f;

		// Token: 0x040000EE RID: 238
		public bool useLocalCoordinate = true;

		// Token: 0x040000EF RID: 239
		public float translationAcceleration;

		// Token: 0x040000F0 RID: 240
		public float rotationAcceleration;

		// Token: 0x020000F6 RID: 246
		public enum TranslationMode
		{
			// Token: 0x0400038F RID: 911
			Off,
			// Token: 0x04000390 RID: 912
			XAxis,
			// Token: 0x04000391 RID: 913
			YAxis,
			// Token: 0x04000392 RID: 914
			ZAxis,
			// Token: 0x04000393 RID: 915
			Vector
		}

		// Token: 0x020000F7 RID: 247
		public enum RotationMode
		{
			// Token: 0x04000395 RID: 917
			Off,
			// Token: 0x04000396 RID: 918
			XAxis,
			// Token: 0x04000397 RID: 919
			YAxis,
			// Token: 0x04000398 RID: 920
			ZAxis,
			// Token: 0x04000399 RID: 921
			Vector
		}
	}
}
