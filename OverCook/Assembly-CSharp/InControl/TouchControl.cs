using System;
using UnityEngine;

namespace InControl
{
	// Token: 0x020002C6 RID: 710
	public abstract class TouchControl : MonoBehaviour
	{
		// Token: 0x06000E86 RID: 3718
		public abstract void CreateControl();

		// Token: 0x06000E87 RID: 3719
		public abstract void DestroyControl();

		// Token: 0x06000E88 RID: 3720
		public abstract void ConfigureControl();

		// Token: 0x06000E89 RID: 3721
		public abstract void SubmitControlState(ulong updateTick, float deltaTime);

		// Token: 0x06000E8A RID: 3722
		public abstract void CommitControlState(ulong updateTick, float deltaTime);

		// Token: 0x06000E8B RID: 3723
		public abstract void TouchBegan(Touch touch);

		// Token: 0x06000E8C RID: 3724
		public abstract void TouchMoved(Touch touch);

		// Token: 0x06000E8D RID: 3725
		public abstract void TouchEnded(Touch touch);

		// Token: 0x06000E8E RID: 3726
		public abstract void DrawGizmos();

		// Token: 0x06000E8F RID: 3727 RVA: 0x00045663 File Offset: 0x00043A63
		private void OnEnable()
		{
			TouchManager.OnSetup += this.Setup;
		}

		// Token: 0x06000E90 RID: 3728 RVA: 0x00045676 File Offset: 0x00043A76
		private void OnDisable()
		{
			this.DestroyControl();
			Resources.UnloadUnusedAssets();
		}

		// Token: 0x06000E91 RID: 3729 RVA: 0x00045684 File Offset: 0x00043A84
		private void Setup()
		{
			if (!base.enabled)
			{
				return;
			}
			this.CreateControl();
			this.ConfigureControl();
		}

		// Token: 0x06000E92 RID: 3730 RVA: 0x000456A0 File Offset: 0x00043AA0
		protected Vector3 OffsetToWorldPosition(TouchControlAnchor anchor, Vector2 offset, TouchUnitType offsetUnitType, bool lockAspectRatio)
		{
			Vector3 b;
			if (offsetUnitType == TouchUnitType.Pixels)
			{
				b = TouchUtility.RoundVector(offset) * TouchManager.PixelToWorld;
			}
			else if (lockAspectRatio)
			{
				b = offset * TouchManager.PercentToWorld;
			}
			else
			{
				b = Vector3.Scale(offset, TouchManager.ViewSize);
			}
			return TouchManager.ViewToWorldPoint(TouchUtility.AnchorToViewPoint(anchor)) + b;
		}

		// Token: 0x06000E93 RID: 3731 RVA: 0x00045710 File Offset: 0x00043B10
		protected void SubmitButtonState(TouchControl.ButtonTarget target, bool state, ulong updateTick, float deltaTime)
		{
			if (TouchManager.Device == null || target == TouchControl.ButtonTarget.None)
			{
				return;
			}
			InputControl control = TouchManager.Device.GetControl((InputControlType)target);
			if (control != null)
			{
				control.UpdateWithState(state, updateTick, deltaTime);
			}
		}

		// Token: 0x06000E94 RID: 3732 RVA: 0x0004574C File Offset: 0x00043B4C
		protected void CommitButton(TouchControl.ButtonTarget target)
		{
			if (TouchManager.Device == null || target == TouchControl.ButtonTarget.None)
			{
				return;
			}
			InputControl control = TouchManager.Device.GetControl((InputControlType)target);
			if (control != null)
			{
				control.Commit();
			}
		}

		// Token: 0x06000E95 RID: 3733 RVA: 0x00045784 File Offset: 0x00043B84
		protected void SubmitAnalogValue(TouchControl.AnalogTarget target, Vector2 value, float lowerDeadZone, float upperDeadZone, ulong updateTick, float deltaTime)
		{
			if (TouchManager.Device == null)
			{
				return;
			}
			Vector2 value2 = Utility.ApplyCircularDeadZone(value, lowerDeadZone, upperDeadZone);
			if (target == TouchControl.AnalogTarget.LeftStick || target == TouchControl.AnalogTarget.Both)
			{
				TouchManager.Device.UpdateLeftStickWithValue(value2, updateTick, deltaTime);
			}
			if (target == TouchControl.AnalogTarget.RightStick || target == TouchControl.AnalogTarget.Both)
			{
				TouchManager.Device.UpdateRightStickWithValue(value2, updateTick, deltaTime);
			}
		}

		// Token: 0x06000E96 RID: 3734 RVA: 0x000457E0 File Offset: 0x00043BE0
		protected void CommitAnalog(TouchControl.AnalogTarget target)
		{
			if (TouchManager.Device == null)
			{
				return;
			}
			if (target == TouchControl.AnalogTarget.LeftStick || target == TouchControl.AnalogTarget.Both)
			{
				TouchManager.Device.CommitLeftStick();
			}
			if (target == TouchControl.AnalogTarget.RightStick || target == TouchControl.AnalogTarget.Both)
			{
				TouchManager.Device.CommitRightStick();
			}
		}

		// Token: 0x06000E97 RID: 3735 RVA: 0x00045820 File Offset: 0x00043C20
		protected void SubmitRawAnalogValue(TouchControl.AnalogTarget target, Vector2 rawValue, ulong updateTick, float deltaTime)
		{
			if (TouchManager.Device == null)
			{
				return;
			}
			if (target == TouchControl.AnalogTarget.LeftStick || target == TouchControl.AnalogTarget.Both)
			{
				TouchManager.Device.UpdateLeftStickWithRawValue(rawValue, updateTick, deltaTime);
			}
			if (target == TouchControl.AnalogTarget.RightStick || target == TouchControl.AnalogTarget.Both)
			{
				TouchManager.Device.UpdateRightStickWithRawValue(rawValue, updateTick, deltaTime);
			}
		}

		// Token: 0x06000E98 RID: 3736 RVA: 0x00045870 File Offset: 0x00043C70
		protected static Vector2 SnapTo(Vector2 vector, TouchControl.SnapAngles snapAngles)
		{
			if (snapAngles == TouchControl.SnapAngles.None)
			{
				return vector;
			}
			float snapAngle = 360f / (float)snapAngles;
			return TouchControl.SnapTo(vector, snapAngle);
		}

		// Token: 0x06000E99 RID: 3737 RVA: 0x00045898 File Offset: 0x00043C98
		protected static Vector2 SnapTo(Vector2 vector, float snapAngle)
		{
			float num = Vector2.Angle(vector, Vector2.up);
			if (num < snapAngle / 2f)
			{
				return Vector2.up * vector.magnitude;
			}
			if (num > 180f - snapAngle / 2f)
			{
				return -Vector2.up * vector.magnitude;
			}
			float num2 = Mathf.Round(num / snapAngle);
			float angle = num2 * snapAngle - num;
			Vector3 axis = Vector3.Cross(Vector2.up, vector);
			Quaternion rotation = Quaternion.AngleAxis(angle, axis);
			return rotation * vector;
		}

		// Token: 0x06000E9A RID: 3738 RVA: 0x0004593C File Offset: 0x00043D3C
		private void OnDrawGizmosSelected()
		{
			if (!base.enabled)
			{
				return;
			}
			if (TouchManager.ControlsShowGizmos != TouchManager.GizmoShowOption.WhenSelected)
			{
				return;
			}
			if (Utility.GameObjectIsCulledOnCurrentCamera(base.gameObject))
			{
				return;
			}
			if (!Application.isPlaying)
			{
				this.ConfigureControl();
			}
			this.DrawGizmos();
		}

		// Token: 0x06000E9B RID: 3739 RVA: 0x00045988 File Offset: 0x00043D88
		private void OnDrawGizmos()
		{
			if (!base.enabled)
			{
				return;
			}
			if (TouchManager.ControlsShowGizmos == TouchManager.GizmoShowOption.UnlessPlaying)
			{
				if (Application.isPlaying)
				{
					return;
				}
			}
			else if (TouchManager.ControlsShowGizmos != TouchManager.GizmoShowOption.Always)
			{
				return;
			}
			if (Utility.GameObjectIsCulledOnCurrentCamera(base.gameObject))
			{
				return;
			}
			if (!Application.isPlaying)
			{
				this.ConfigureControl();
			}
			this.DrawGizmos();
		}

		// Token: 0x020002C7 RID: 711
		public enum ButtonTarget
		{
			// Token: 0x04000B50 RID: 2896
			None,
			// Token: 0x04000B51 RID: 2897
			Action1 = 15,
			// Token: 0x04000B52 RID: 2898
			Action2,
			// Token: 0x04000B53 RID: 2899
			Action3,
			// Token: 0x04000B54 RID: 2900
			Action4,
			// Token: 0x04000B55 RID: 2901
			LeftTrigger,
			// Token: 0x04000B56 RID: 2902
			RightTrigger,
			// Token: 0x04000B57 RID: 2903
			LeftBumper,
			// Token: 0x04000B58 RID: 2904
			RightBumper,
			// Token: 0x04000B59 RID: 2905
			DPadDown = 12,
			// Token: 0x04000B5A RID: 2906
			DPadLeft,
			// Token: 0x04000B5B RID: 2907
			DPadRight,
			// Token: 0x04000B5C RID: 2908
			DPadUp = 11,
			// Token: 0x04000B5D RID: 2909
			Menu = 30,
			// Token: 0x04000B5E RID: 2910
			Button0 = 62,
			// Token: 0x04000B5F RID: 2911
			Button1,
			// Token: 0x04000B60 RID: 2912
			Button2,
			// Token: 0x04000B61 RID: 2913
			Button3,
			// Token: 0x04000B62 RID: 2914
			Button4,
			// Token: 0x04000B63 RID: 2915
			Button5,
			// Token: 0x04000B64 RID: 2916
			Button6,
			// Token: 0x04000B65 RID: 2917
			Button7,
			// Token: 0x04000B66 RID: 2918
			Button8,
			// Token: 0x04000B67 RID: 2919
			Button9,
			// Token: 0x04000B68 RID: 2920
			Button10,
			// Token: 0x04000B69 RID: 2921
			Button11,
			// Token: 0x04000B6A RID: 2922
			Button12,
			// Token: 0x04000B6B RID: 2923
			Button13,
			// Token: 0x04000B6C RID: 2924
			Button14,
			// Token: 0x04000B6D RID: 2925
			Button15,
			// Token: 0x04000B6E RID: 2926
			Button16,
			// Token: 0x04000B6F RID: 2927
			Button17,
			// Token: 0x04000B70 RID: 2928
			Button18,
			// Token: 0x04000B71 RID: 2929
			Button19
		}

		// Token: 0x020002C8 RID: 712
		public enum AnalogTarget
		{
			// Token: 0x04000B73 RID: 2931
			None,
			// Token: 0x04000B74 RID: 2932
			LeftStick,
			// Token: 0x04000B75 RID: 2933
			RightStick,
			// Token: 0x04000B76 RID: 2934
			Both
		}

		// Token: 0x020002C9 RID: 713
		public enum SnapAngles
		{
			// Token: 0x04000B78 RID: 2936
			None,
			// Token: 0x04000B79 RID: 2937
			Four = 4,
			// Token: 0x04000B7A RID: 2938
			Eight = 8,
			// Token: 0x04000B7B RID: 2939
			Sixteen = 16
		}
	}
}
