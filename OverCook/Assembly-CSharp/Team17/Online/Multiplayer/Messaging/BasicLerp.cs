using System;
using UnityEngine;

namespace Team17.Online.Multiplayer.Messaging
{
	// Token: 0x020008F1 RID: 2289
	public class BasicLerp : EmptyLerp
	{
		// Token: 0x06002C93 RID: 11411 RVA: 0x000D260C File Offset: 0x000D0A0C
		public override void StartSynchronising(Component synchronisedObject)
		{
			this.m_bInitialPositionSet = false;
			this.m_Transform = base.transform;
			this.m_PreviousParent = this.m_Transform.parent;
			this.Reset();
			base.StartSynchronising(synchronisedObject);
		}

		// Token: 0x06002C94 RID: 11412 RVA: 0x000D263F File Offset: 0x000D0A3F
		public virtual void ApplyLerpInfo(Vector3 targetPosition, Quaternion targetRotation)
		{
			this.m_TargetLocalPosition = targetPosition;
			this.m_PositionWhenReceived = this.m_Transform.localPosition;
			this.m_fLerpDelta = 0f;
			this.m_TargetLocalRotation = targetRotation;
			this.m_RotationWhenReceived = this.m_Transform.localRotation;
		}

		// Token: 0x06002C95 RID: 11413 RVA: 0x000D267C File Offset: 0x000D0A7C
		public override void Reset()
		{
			this.m_TargetLocalPosition = this.m_Transform.localPosition;
			this.m_PositionWhenReceived = this.m_Transform.localPosition;
			this.m_TargetLocalRotation = this.m_Transform.localRotation;
			this.m_RotationWhenReceived = this.m_Transform.localRotation;
			this.m_fLastMessageTime = Time.time;
			base.Reset();
		}

		// Token: 0x06002C96 RID: 11414 RVA: 0x000D26E0 File Offset: 0x000D0AE0
		public override void Reparented()
		{
			Matrix4x4 rhs = Matrix4x4.identity;
			Matrix4x4 lhs = Matrix4x4.identity;
			if (this.m_PreviousParent != null)
			{
				rhs = this.m_PreviousParent.worldToLocalMatrix.inverse;
			}
			if (this.m_Transform.parent != null)
			{
				lhs = this.m_Transform.parent.worldToLocalMatrix;
			}
			this.m_PositionWhenReceived = lhs * rhs * new Vector4(this.m_PositionWhenReceived.x, this.m_PositionWhenReceived.y, this.m_PositionWhenReceived.z, 1f);
			base.Reparented();
		}

		// Token: 0x06002C97 RID: 11415 RVA: 0x000D2790 File Offset: 0x000D0B90
		public override void UpdateLerp(float delta)
		{
			if (this.m_Transform != null && this.m_fLerpTime != 0f)
			{
				this.m_fLerpDelta += delta;
				float t = this.m_fLerpDelta / this.m_fLerpTime;
				this.m_Transform.localPosition = Vector3.Lerp(this.m_PositionWhenReceived, this.m_TargetLocalPosition, t);
				this.m_Transform.localRotation = Quaternion.Lerp(this.m_RotationWhenReceived, this.m_TargetLocalRotation, t);
			}
		}

		// Token: 0x06002C98 RID: 11416 RVA: 0x000D2814 File Offset: 0x000D0C14
		public override void ReceiveServerUpdate(Vector3 localPosition, Quaternion localRotation)
		{
			if (this.m_bInitialPositionSet)
			{
				float time = Time.time;
				float num = time - this.m_fLastMessageTime;
				if (num < 1f)
				{
					this.m_fLerpTime = Mathf.Lerp(this.m_fLerpTime, num + 0.1f, 0.01f);
				}
				this.m_fLastMessageTime = time;
				this.ApplyLerpInfo(localPosition, localRotation);
			}
			else
			{
				this.m_Transform.localPosition = localPosition;
				this.m_Transform.localRotation = localRotation;
				this.m_TargetLocalPosition = localPosition;
				this.m_TargetLocalRotation = localRotation;
				this.m_bInitialPositionSet = true;
			}
		}

		// Token: 0x06002C99 RID: 11417 RVA: 0x000D28A4 File Offset: 0x000D0CA4
		public override void ReceiveServerEvent(Vector3 localPosition, Quaternion localRotation)
		{
			if (this.m_bInitialPositionSet)
			{
				this.ApplyLerpInfo(localPosition, localRotation);
			}
			else
			{
				this.m_Transform.localPosition = localPosition;
				this.m_Transform.localRotation = localRotation;
				this.m_TargetLocalPosition = localPosition;
				this.m_TargetLocalRotation = localRotation;
				this.m_bInitialPositionSet = true;
			}
		}

		// Token: 0x040023D8 RID: 9176
		private const float kMaxLerpTime = 1f;

		// Token: 0x040023D9 RID: 9177
		private Transform m_PreviousParent;

		// Token: 0x040023DA RID: 9178
		protected Transform m_Transform;

		// Token: 0x040023DB RID: 9179
		protected Vector3 m_TargetLocalPosition = Vector3.zero;

		// Token: 0x040023DC RID: 9180
		protected Vector3 m_PositionWhenReceived = Vector3.zero;

		// Token: 0x040023DD RID: 9181
		protected Quaternion m_TargetLocalRotation = Quaternion.identity;

		// Token: 0x040023DE RID: 9182
		protected Quaternion m_RotationWhenReceived = Quaternion.identity;

		// Token: 0x040023DF RID: 9183
		protected float m_fLerpDelta;

		// Token: 0x040023E0 RID: 9184
		protected float m_fLerpTime = 0.1f;

		// Token: 0x040023E1 RID: 9185
		protected float m_fLastMessageTime;

		// Token: 0x040023E2 RID: 9186
		private bool m_bInitialPositionSet;
	}
}
