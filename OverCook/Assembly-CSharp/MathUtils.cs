using System;
using UnityEngine;

// Token: 0x02000266 RID: 614
public static class MathUtils
{
	// Token: 0x06000B4A RID: 2890 RVA: 0x0003C767 File Offset: 0x0003AB67
	public static void FloorModf(float _dividend, float _divisor, out float quotient, out float remainder)
	{
		quotient = Mathf.Floor(_dividend / _divisor);
		remainder = _dividend - quotient * _divisor;
	}

	// Token: 0x06000B4B RID: 2891 RVA: 0x0003C77B File Offset: 0x0003AB7B
	public static void TruncModf(float _dividend, float _divisor, out float quotient, out float remainder)
	{
		quotient = MathUtils.Truncate(_dividend / _divisor);
		remainder = _dividend - quotient * _divisor;
	}

	// Token: 0x06000B4C RID: 2892 RVA: 0x0003C78F File Offset: 0x0003AB8F
	public static void TruncModf(int _dividend, int _divisor, out int quotient, out int remainder)
	{
		quotient = _dividend / _divisor;
		remainder = _dividend - quotient * _divisor;
	}

	// Token: 0x06000B4D RID: 2893 RVA: 0x0003C79E File Offset: 0x0003AB9E
	public static float Truncate(float _value)
	{
		return _value - _value % 1f;
	}

	// Token: 0x06000B4E RID: 2894 RVA: 0x0003C7A9 File Offset: 0x0003ABA9
	public static float Quantize(float _input, float _gridSize)
	{
		return Mathf.Round(_input / _gridSize) * _gridSize;
	}

	// Token: 0x06000B4F RID: 2895 RVA: 0x0003C7B5 File Offset: 0x0003ABB5
	public static float ClampedRemap(float _value, float _a, float _b, float _newA, float _newB)
	{
		return Mathf.Clamp(MathUtils.Remap(_value, _a, _b, _newA, _newB), Mathf.Min(_newA, _newB), Mathf.Max(_newA, _newB));
	}

	// Token: 0x06000B50 RID: 2896 RVA: 0x0003C7D7 File Offset: 0x0003ABD7
	public static float ClampedRemap01(float value, float min, float max)
	{
		return Mathf.Clamp01((value - min) / (max - min));
	}

	// Token: 0x06000B51 RID: 2897 RVA: 0x0003C7E8 File Offset: 0x0003ABE8
	public static float Wrap(float _value, float _intervalMin, float _intervalMax)
	{
		float num = _intervalMax - _intervalMin;
		float num2 = (_value - _intervalMin) % num;
		num2 = (num2 + num) % num;
		return num2 + _intervalMin;
	}

	// Token: 0x06000B52 RID: 2898 RVA: 0x0003C808 File Offset: 0x0003AC08
	public static float AngleWrap(float _x)
	{
		float num;
		float num2;
		MathUtils.FloorModf(_x + 3.1415927f, 6.2831855f, out num, out num2);
		return num2 - 3.1415927f;
	}

	// Token: 0x06000B53 RID: 2899 RVA: 0x0003C831 File Offset: 0x0003AC31
	public static float AngleDifference(float _a, float _b)
	{
		return MathUtils.AngleWrap(_a - _b);
	}

	// Token: 0x06000B54 RID: 2900 RVA: 0x0003C83C File Offset: 0x0003AC3C
	public static int Wrap(int _value, int _intervalMin, int _intervalMax)
	{
		int num = _intervalMax - _intervalMin;
		int num2 = (_value - _intervalMin) % num;
		num2 = (num2 + num) % num;
		return num2 + _intervalMin;
	}

	// Token: 0x06000B55 RID: 2901 RVA: 0x0003C85C File Offset: 0x0003AC5C
	public static float Remap(float _value, float _a, float _b, float _newA, float _newB)
	{
		float num = (_value - _a) / (_b - _a);
		return _newA + num * (_newB - _newA);
	}

	// Token: 0x06000B56 RID: 2902 RVA: 0x0003C87C File Offset: 0x0003AC7C
	public static float SinusoidalSCurve(float _prop)
	{
		float num = Mathf.Clamp(_prop, 0f, 1f);
		return 0.5f * (1f - Mathf.Cos(3.1415927f * num));
	}

	// Token: 0x06000B57 RID: 2903 RVA: 0x0003C8B2 File Offset: 0x0003ACB2
	public static float Square(float _value)
	{
		return _value * _value;
	}

	// Token: 0x06000B58 RID: 2904 RVA: 0x0003C8B8 File Offset: 0x0003ACB8
	public static void AdvanceToTarget_Sinusoidal(ref float _nCurrentX, ref float _nCurrentGradient, float _nTargetX, float _nGradientLimit, float _nTimeToMax, float _nDeltaTime)
	{
		float num = _nCurrentX;
		float num2 = _nCurrentGradient;
		float num3 = 3.1415927f / (2f * _nTimeToMax);
		float num4 = _nGradientLimit / num3;
		num2 = Mathf.Clamp(num2, -_nGradientLimit, _nGradientLimit);
		float? num5 = null;
		if (Mathf.Abs(_nTargetX - num) <= num4)
		{
			float num6 = (float)((_nTargetX - num <= 0f) ? -1 : 1);
			float num7 = num6 * Mathf.Acos(1f - num3 * Mathf.Abs(_nTargetX - num) / _nGradientLimit) / num3;
			if (num6 > 0f)
			{
				num7 = Mathf.Max(num7 - _nDeltaTime, 0f);
			}
			else
			{
				num7 = Mathf.Min(num7 + _nDeltaTime, 0f);
			}
			num5 = new float?(_nGradientLimit * Mathf.Sin(num3 * num7));
			if (num6 > 0f && num5 > num2)
			{
				num5 = null;
			}
			else if (num6 < 0f && num5 < num2)
			{
				num5 = null;
			}
		}
		if (num5 == null)
		{
			float num8 = 1f / num3 * (1.5707964f - Mathf.Acos(num2 / _nGradientLimit));
			if (_nTargetX > num)
			{
				num8 = Mathf.Min(num8 + _nDeltaTime, 3.1415927f / (2f * num3));
			}
			else
			{
				num8 = Mathf.Max(num8 - _nDeltaTime, -3.1415927f / (2f * num3));
			}
			num5 = new float?(_nGradientLimit * Mathf.Cos(1.5707964f - num3 * num8));
		}
		num2 = num5.Value;
		if (_nTargetX > num)
		{
			num2 = Mathf.Clamp(num2, -_nGradientLimit, (_nTargetX - num) / _nDeltaTime);
		}
		else
		{
			num2 = Mathf.Clamp(num2, (_nTargetX - num) / _nDeltaTime, _nGradientLimit);
		}
		num += num2 * _nDeltaTime;
		_nCurrentX = num;
		_nCurrentGradient = num2;
	}

	// Token: 0x06000B59 RID: 2905 RVA: 0x0003CAB7 File Offset: 0x0003AEB7
	public static string AsHexString(uint _value)
	{
		return "0x" + _value.ToString("x");
	}

	// Token: 0x06000B5A RID: 2906 RVA: 0x0003CACF File Offset: 0x0003AECF
	public static Vector3 MultiplyByMatrix(Matrix4x4 matrix, Vector3 vec)
	{
		return matrix * new Vector4(vec.x, vec.y, vec.z, 1f);
	}

	// Token: 0x06000B5B RID: 2907 RVA: 0x0003CAFC File Offset: 0x0003AEFC
	public static Vector3 CircleNearestPoint(Vector3 centre, float radius, Vector3 point)
	{
		return (point - centre).normalized * radius + centre;
	}

	// Token: 0x06000B5C RID: 2908 RVA: 0x0003CB24 File Offset: 0x0003AF24
	public static void CircleCircleOuterTangents(out Vector3 t0, out Vector3 t1, out Vector3 t2, out Vector3 t3, Vector3 centre0, float radius0, Vector3 centre1, float radius1)
	{
		if (radius0 == radius1)
		{
			Vector3 vector = centre1 - centre0;
			Vector3 vector2 = new Vector3(-vector.z, 0f, vector.x);
			Vector3 normalized = vector2.normalized;
			Vector3 a = normalized * radius0;
			Vector3 a2 = normalized * -radius0;
			t0 = a + centre0;
			t0.y = centre0.y;
			t1 = a2 + centre0;
			t1.y = centre0.y;
			t2 = a + centre1;
			t2.y = centre1.y;
			t3 = a2 + centre1;
			t3.y = centre1.y;
		}
		else
		{
			float num = Vector3.Distance(centre0, centre1);
			Vector3 a3 = new Vector3((centre1.x * radius0 - centre0.x * radius1) / (radius0 - radius1), centre0.y, (centre1.z * radius0 - centre0.z * radius1) / (radius0 - radius1));
			Vector3 vector3 = a3 - centre0;
			Vector3 vector4 = a3 - centre1;
			float num2 = radius0 * radius0;
			float num3 = radius1 * radius1;
			float num4 = Mathf.Sqrt(vector3.x * vector3.x + vector3.z * vector3.z - num2);
			float num5 = Mathf.Sqrt(vector4.x * vector4.x + vector4.z * vector4.z - num3);
			float num6 = vector3.x * vector3.x + vector3.z * vector3.z;
			float num7 = vector4.x * vector4.x + vector4.z * vector4.z;
			t0 = new Vector3((num2 * vector3.x + radius0 * vector3.z * num4) / num6 + centre0.x, centre0.y, (num2 * vector3.z - radius0 * vector3.x * num4) / num6 + centre0.z);
			t1 = new Vector3((num2 * vector3.x - radius0 * vector3.z * num4) / num6 + centre0.x, centre0.y, (num2 * vector3.z + radius0 * vector3.x * num4) / num6 + centre0.z);
			t2 = new Vector3((num3 * vector4.x + radius1 * vector4.z * num5) / num7 + centre1.x, centre1.y, (num3 * vector4.z - radius1 * vector4.x * num5) / num7 + centre1.z);
			t3 = new Vector3((num3 * vector4.x - radius1 * vector4.z * num5) / num7 + centre1.x, centre1.y, (num3 * vector4.z + radius1 * vector4.x * num5) / num7 + centre1.z);
		}
	}
}
