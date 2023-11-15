using System;
using System.IO;
using Microsoft.Win32;
using UnityEngine;

namespace InControl
{
	// Token: 0x02000348 RID: 840
	public static class Utility
	{
		// Token: 0x06000FF5 RID: 4085 RVA: 0x0005C71C File Offset: 0x0005AB1C
		public static void DrawCircleGizmo(Vector2 center, float radius)
		{
			Vector2 v = Utility.circleVertexList[0] * radius + center;
			int num = Utility.circleVertexList.Length;
			for (int i = 1; i < num; i++)
			{
				Gizmos.DrawLine(v, v = Utility.circleVertexList[i] * radius + center);
			}
		}

		// Token: 0x06000FF6 RID: 4086 RVA: 0x0005C78E File Offset: 0x0005AB8E
		public static void DrawCircleGizmo(Vector2 center, float radius, Color color)
		{
			Gizmos.color = color;
			Utility.DrawCircleGizmo(center, radius);
		}

		// Token: 0x06000FF7 RID: 4087 RVA: 0x0005C7A0 File Offset: 0x0005ABA0
		public static void DrawOvalGizmo(Vector2 center, Vector2 size)
		{
			Vector2 b = size / 2f;
			Vector2 v = Vector2.Scale(Utility.circleVertexList[0], b) + center;
			int num = Utility.circleVertexList.Length;
			for (int i = 1; i < num; i++)
			{
				Gizmos.DrawLine(v, v = Vector2.Scale(Utility.circleVertexList[i], b) + center);
			}
		}

		// Token: 0x06000FF8 RID: 4088 RVA: 0x0005C81E File Offset: 0x0005AC1E
		public static void DrawOvalGizmo(Vector2 center, Vector2 size, Color color)
		{
			Gizmos.color = color;
			Utility.DrawOvalGizmo(center, size);
		}

		// Token: 0x06000FF9 RID: 4089 RVA: 0x0005C830 File Offset: 0x0005AC30
		public static void DrawRectGizmo(Rect rect)
		{
			Vector3 vector = new Vector3(rect.xMin, rect.yMin);
			Vector3 vector2 = new Vector3(rect.xMax, rect.yMin);
			Vector3 vector3 = new Vector3(rect.xMax, rect.yMax);
			Vector3 vector4 = new Vector3(rect.xMin, rect.yMax);
			Gizmos.DrawLine(vector, vector2);
			Gizmos.DrawLine(vector2, vector3);
			Gizmos.DrawLine(vector3, vector4);
			Gizmos.DrawLine(vector4, vector);
		}

		// Token: 0x06000FFA RID: 4090 RVA: 0x0005C8AD File Offset: 0x0005ACAD
		public static void DrawRectGizmo(Rect rect, Color color)
		{
			Gizmos.color = color;
			Utility.DrawRectGizmo(rect);
		}

		// Token: 0x06000FFB RID: 4091 RVA: 0x0005C8BC File Offset: 0x0005ACBC
		public static void DrawRectGizmo(Vector2 center, Vector2 size)
		{
			float num = size.x / 2f;
			float num2 = size.y / 2f;
			Vector3 vector = new Vector3(center.x - num, center.y - num2);
			Vector3 vector2 = new Vector3(center.x + num, center.y - num2);
			Vector3 vector3 = new Vector3(center.x + num, center.y + num2);
			Vector3 vector4 = new Vector3(center.x - num, center.y + num2);
			Gizmos.DrawLine(vector, vector2);
			Gizmos.DrawLine(vector2, vector3);
			Gizmos.DrawLine(vector3, vector4);
			Gizmos.DrawLine(vector4, vector);
		}

		// Token: 0x06000FFC RID: 4092 RVA: 0x0005C969 File Offset: 0x0005AD69
		public static void DrawRectGizmo(Vector2 center, Vector2 size, Color color)
		{
			Gizmos.color = color;
			Utility.DrawRectGizmo(center, size);
		}

		// Token: 0x06000FFD RID: 4093 RVA: 0x0005C978 File Offset: 0x0005AD78
		public static bool GameObjectIsCulledOnCurrentCamera(GameObject gameObject)
		{
			return (Camera.current.cullingMask & 1 << gameObject.layer) == 0;
		}

		// Token: 0x06000FFE RID: 4094 RVA: 0x0005C994 File Offset: 0x0005AD94
		public static Color MoveColorTowards(Color color0, Color color1, float maxDelta)
		{
			float r = Mathf.MoveTowards(color0.r, color1.r, maxDelta);
			float g = Mathf.MoveTowards(color0.g, color1.g, maxDelta);
			float b = Mathf.MoveTowards(color0.b, color1.b, maxDelta);
			float a = Mathf.MoveTowards(color0.a, color1.a, maxDelta);
			return new Color(r, g, b, a);
		}

		// Token: 0x06000FFF RID: 4095 RVA: 0x0005CA00 File Offset: 0x0005AE00
		public static float ApplyDeadZone(float value, float lowerDeadZone, float upperDeadZone)
		{
			if (value < 0f)
			{
				if (value > -lowerDeadZone)
				{
					return 0f;
				}
				if (value < -upperDeadZone)
				{
					return -1f;
				}
				return (value + lowerDeadZone) / (upperDeadZone - lowerDeadZone);
			}
			else
			{
				if (value < lowerDeadZone)
				{
					return 0f;
				}
				if (value > upperDeadZone)
				{
					return 1f;
				}
				return (value - lowerDeadZone) / (upperDeadZone - lowerDeadZone);
			}
		}

		// Token: 0x06001000 RID: 4096 RVA: 0x0005CA60 File Offset: 0x0005AE60
		public static Vector2 ApplyCircularDeadZone(Vector2 v, float lowerDeadZone, float upperDeadZone)
		{
			float d = Mathf.InverseLerp(lowerDeadZone, upperDeadZone, v.magnitude);
			return v.normalized * d;
		}

		// Token: 0x06001001 RID: 4097 RVA: 0x0005CA89 File Offset: 0x0005AE89
		public static Vector2 ApplyCircularDeadZone(float x, float y, float lowerDeadZone, float upperDeadZone)
		{
			return Utility.ApplyCircularDeadZone(new Vector2(x, y), lowerDeadZone, upperDeadZone);
		}

		// Token: 0x06001002 RID: 4098 RVA: 0x0005CA9C File Offset: 0x0005AE9C
		public static float ApplySmoothing(float thisValue, float lastValue, float deltaTime, float sensitivity)
		{
			if (Utility.Approximately(sensitivity, 1f))
			{
				return thisValue;
			}
			float maxDelta = deltaTime * sensitivity * 100f;
			if (Mathf.Sign(lastValue) != Mathf.Sign(thisValue))
			{
				lastValue = 0f;
			}
			return Mathf.MoveTowards(lastValue, thisValue, maxDelta);
		}

		// Token: 0x06001003 RID: 4099 RVA: 0x0005CAE5 File Offset: 0x0005AEE5
		public static float ApplySnapping(float value, float threshold)
		{
			if (value < -threshold)
			{
				return -1f;
			}
			if (value > threshold)
			{
				return 1f;
			}
			return 0f;
		}

		// Token: 0x06001004 RID: 4100 RVA: 0x0005CB07 File Offset: 0x0005AF07
		internal static bool TargetIsButton(InputControlType target)
		{
			return (target >= InputControlType.Action1 && target <= InputControlType.Action4) || (target >= InputControlType.Button0 && target <= InputControlType.Button19);
		}

		// Token: 0x06001005 RID: 4101 RVA: 0x0005CB2F File Offset: 0x0005AF2F
		internal static bool TargetIsStandard(InputControlType target)
		{
			return target >= InputControlType.LeftStickUp && target <= InputControlType.RightBumper;
		}

		// Token: 0x06001006 RID: 4102 RVA: 0x0005CB44 File Offset: 0x0005AF44
		public static string ReadFromFile(string path)
		{
			StreamReader streamReader = new StreamReader(path);
			string result = streamReader.ReadToEnd();
			streamReader.Close();
			return result;
		}

		// Token: 0x06001007 RID: 4103 RVA: 0x0005CB68 File Offset: 0x0005AF68
		public static void WriteToFile(string path, string data)
		{
			StreamWriter streamWriter = new StreamWriter(path);
			streamWriter.Write(data);
			streamWriter.Flush();
			streamWriter.Close();
		}

		// Token: 0x06001008 RID: 4104 RVA: 0x0005CB8F File Offset: 0x0005AF8F
		public static float Abs(float value)
		{
			return (value >= 0f) ? value : (-value);
		}

		// Token: 0x06001009 RID: 4105 RVA: 0x0005CBA4 File Offset: 0x0005AFA4
		public static bool Approximately(float value1, float value2)
		{
			float num = value1 - value2;
			return num >= -1E-07f && num <= 1E-07f;
		}

		// Token: 0x0600100A RID: 4106 RVA: 0x0005CBCE File Offset: 0x0005AFCE
		public static bool IsNotZero(float value)
		{
			return value < -1E-07f || value > 1E-07f;
		}

		// Token: 0x0600100B RID: 4107 RVA: 0x0005CBE6 File Offset: 0x0005AFE6
		public static bool IsZero(float value)
		{
			return value >= -1E-07f && value <= 1E-07f;
		}

		// Token: 0x0600100C RID: 4108 RVA: 0x0005CC01 File Offset: 0x0005B001
		public static bool AbsoluteIsOverThreshold(float value, float threshold)
		{
			return value < -threshold || value > threshold;
		}

		// Token: 0x0600100D RID: 4109 RVA: 0x0005CC12 File Offset: 0x0005B012
		public static float NormalizeAngle(float angle)
		{
			while (angle < 0f)
			{
				angle += 360f;
			}
			while (angle > 360f)
			{
				angle -= 360f;
			}
			return angle;
		}

		// Token: 0x0600100E RID: 4110 RVA: 0x0005CC48 File Offset: 0x0005B048
		public static float VectorToAngle(Vector2 vector)
		{
			if (Utility.IsZero(vector.x) && Utility.IsZero(vector.y))
			{
				return 0f;
			}
			return Utility.NormalizeAngle(Mathf.Atan2(vector.x, vector.y) * 57.29578f);
		}

		// Token: 0x0600100F RID: 4111 RVA: 0x0005CC9C File Offset: 0x0005B09C
		public static float Min(float v0, float v1, float v2, float v3)
		{
			float num = (v0 < v1) ? v0 : v1;
			float num2 = (v2 < v3) ? v2 : v3;
			return (num < num2) ? num : num2;
		}

		// Token: 0x06001010 RID: 4112 RVA: 0x0005CCD8 File Offset: 0x0005B0D8
		public static float Max(float v0, float v1, float v2, float v3)
		{
			float num = (v0 > v1) ? v0 : v1;
			float num2 = (v2 > v3) ? v2 : v3;
			return (num > num2) ? num : num2;
		}

		// Token: 0x06001011 RID: 4113 RVA: 0x0005CD14 File Offset: 0x0005B114
		internal static float ValueFromSides(float negativeSide, float positiveSide)
		{
			float num = Utility.Abs(negativeSide);
			float num2 = Utility.Abs(positiveSide);
			if (Utility.Approximately(num, num2))
			{
				return 0f;
			}
			return (num <= num2) ? num2 : (-num);
		}

		// Token: 0x06001012 RID: 4114 RVA: 0x0005CD50 File Offset: 0x0005B150
		internal static float ValueFromSides(float negativeSide, float positiveSide, bool invertSides)
		{
			if (invertSides)
			{
				return Utility.ValueFromSides(positiveSide, negativeSide);
			}
			return Utility.ValueFromSides(negativeSide, positiveSide);
		}

		// Token: 0x17000213 RID: 531
		// (get) Token: 0x06001013 RID: 4115 RVA: 0x0005CD67 File Offset: 0x0005B167
		internal static bool Is32Bit
		{
			get
			{
				return IntPtr.Size == 4;
			}
		}

		// Token: 0x17000214 RID: 532
		// (get) Token: 0x06001014 RID: 4116 RVA: 0x0005CD71 File Offset: 0x0005B171
		internal static bool Is64Bit
		{
			get
			{
				return IntPtr.Size == 8;
			}
		}

		// Token: 0x06001015 RID: 4117 RVA: 0x0005CD7C File Offset: 0x0005B17C
		public static string HKLM_GetString(string path, string key)
		{
			string result;
			try
			{
				RegistryKey registryKey = Registry.LocalMachine.OpenSubKey(path);
				if (registryKey == null)
				{
					result = string.Empty;
				}
				else
				{
					result = (string)registryKey.GetValue(key);
				}
			}
			catch
			{
				result = null;
			}
			return result;
		}

		// Token: 0x06001016 RID: 4118 RVA: 0x0005CDD4 File Offset: 0x0005B1D4
		public static string GetWindowsVersion()
		{
			string text = Utility.HKLM_GetString("SOFTWARE\\Microsoft\\Windows NT\\CurrentVersion", "ProductName");
			if (text != null)
			{
				string text2 = Utility.HKLM_GetString("SOFTWARE\\Microsoft\\Windows NT\\CurrentVersion", "CSDVersion");
				string str = (!Utility.Is32Bit) ? "64Bit" : "32Bit";
				return text + ((text2 == null) ? string.Empty : (" " + text2)) + " " + str;
			}
			return SystemInfo.operatingSystem;
		}

		// Token: 0x04000C1C RID: 3100
		public const float Epsilon = 1E-07f;

		// Token: 0x04000C1D RID: 3101
		private static Vector2[] circleVertexList = new Vector2[]
		{
			new Vector2(0f, 1f),
			new Vector2(0.2588f, 0.9659f),
			new Vector2(0.5f, 0.866f),
			new Vector2(0.7071f, 0.7071f),
			new Vector2(0.866f, 0.5f),
			new Vector2(0.9659f, 0.2588f),
			new Vector2(1f, 0f),
			new Vector2(0.9659f, -0.2588f),
			new Vector2(0.866f, -0.5f),
			new Vector2(0.7071f, -0.7071f),
			new Vector2(0.5f, -0.866f),
			new Vector2(0.2588f, -0.9659f),
			new Vector2(0f, -1f),
			new Vector2(-0.2588f, -0.9659f),
			new Vector2(-0.5f, -0.866f),
			new Vector2(-0.7071f, -0.7071f),
			new Vector2(-0.866f, -0.5f),
			new Vector2(-0.9659f, -0.2588f),
			new Vector2(-1f, --0f),
			new Vector2(-0.9659f, 0.2588f),
			new Vector2(-0.866f, 0.5f),
			new Vector2(-0.7071f, 0.7071f),
			new Vector2(-0.5f, 0.866f),
			new Vector2(-0.2588f, 0.9659f),
			new Vector2(0f, 1f)
		};
	}
}
