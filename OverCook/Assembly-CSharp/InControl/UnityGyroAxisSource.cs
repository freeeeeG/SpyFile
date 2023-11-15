using System;
using UnityEngine;

namespace InControl
{
	// Token: 0x020002D3 RID: 723
	public class UnityGyroAxisSource : InputControlSource
	{
		// Token: 0x06000F03 RID: 3843 RVA: 0x000486FE File Offset: 0x00046AFE
		public UnityGyroAxisSource()
		{
			UnityGyroAxisSource.Calibrate();
		}

		// Token: 0x06000F04 RID: 3844 RVA: 0x0004870B File Offset: 0x00046B0B
		public UnityGyroAxisSource(UnityGyroAxisSource.GyroAxis axis)
		{
			this.Axis = (int)axis;
			UnityGyroAxisSource.Calibrate();
		}

		// Token: 0x06000F05 RID: 3845 RVA: 0x00048720 File Offset: 0x00046B20
		public float GetValue(InputDevice inputDevice)
		{
			return UnityGyroAxisSource.GetAxis()[this.Axis];
		}

		// Token: 0x06000F06 RID: 3846 RVA: 0x00048740 File Offset: 0x00046B40
		public bool GetState(InputDevice inputDevice)
		{
			return Utility.IsNotZero(this.GetValue(inputDevice));
		}

		// Token: 0x06000F07 RID: 3847 RVA: 0x0004874E File Offset: 0x00046B4E
		private static Quaternion GetAttitude()
		{
			return Quaternion.Inverse(UnityGyroAxisSource.zeroAttitude) * Input.gyro.attitude;
		}

		// Token: 0x06000F08 RID: 3848 RVA: 0x0004876C File Offset: 0x00046B6C
		private static Vector3 GetAxis()
		{
			Vector3 vector = UnityGyroAxisSource.GetAttitude() * Vector3.forward;
			float x = UnityGyroAxisSource.ApplyDeadZone(Mathf.Clamp(vector.x, -1f, 1f));
			float y = UnityGyroAxisSource.ApplyDeadZone(Mathf.Clamp(vector.y, -1f, 1f));
			return new Vector3(x, y);
		}

		// Token: 0x06000F09 RID: 3849 RVA: 0x000487C8 File Offset: 0x00046BC8
		private static float ApplyDeadZone(float value)
		{
			return Mathf.InverseLerp(0.05f, 1f, Utility.Abs(value)) * Mathf.Sign(value);
		}

		// Token: 0x06000F0A RID: 3850 RVA: 0x000487E6 File Offset: 0x00046BE6
		public static void Calibrate()
		{
			UnityGyroAxisSource.zeroAttitude = Input.gyro.attitude;
		}

		// Token: 0x04000BB9 RID: 3001
		private static Quaternion zeroAttitude;

		// Token: 0x04000BBA RID: 3002
		public int Axis;

		// Token: 0x020002D4 RID: 724
		public enum GyroAxis
		{
			// Token: 0x04000BBC RID: 3004
			X,
			// Token: 0x04000BBD RID: 3005
			Y
		}
	}
}
