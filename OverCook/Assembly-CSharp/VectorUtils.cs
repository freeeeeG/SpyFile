using System;
using UnityEngine;

// Token: 0x0200026B RID: 619
public static class VectorUtils
{
	// Token: 0x06000B79 RID: 2937 RVA: 0x0003D279 File Offset: 0x0003B679
	public static Vector2 Splat2(float _input)
	{
		return new Vector2(_input, _input);
	}

	// Token: 0x06000B7A RID: 2938 RVA: 0x0003D282 File Offset: 0x0003B682
	public static Vector3 Splat3(float _input)
	{
		return new Vector3(_input, _input, _input);
	}

	// Token: 0x06000B7B RID: 2939 RVA: 0x0003D28C File Offset: 0x0003B68C
	public static Vector2 XZ(this Vector3 _input)
	{
		return new Vector2(_input.x, _input.z);
	}

	// Token: 0x06000B7C RID: 2940 RVA: 0x0003D2A1 File Offset: 0x0003B6A1
	public static Vector2 XY(this Vector3 _input)
	{
		return new Vector2(_input.x, _input.y);
	}

	// Token: 0x06000B7D RID: 2941 RVA: 0x0003D2B6 File Offset: 0x0003B6B6
	public static Vector2 YZ(this Vector3 _input)
	{
		return new Vector2(_input.y, _input.z);
	}

	// Token: 0x06000B7E RID: 2942 RVA: 0x0003D2CB File Offset: 0x0003B6CB
	public static Vector3 FromXZ(Vector2 _xz, float _y)
	{
		return new Vector3(_xz.x, _y, _xz.y);
	}

	// Token: 0x06000B7F RID: 2943 RVA: 0x0003D2E1 File Offset: 0x0003B6E1
	public static Vector3 FromXY(Vector2 _xy, float _z)
	{
		return new Vector3(_xy.x, _xy.y, _z);
	}

	// Token: 0x06000B80 RID: 2944 RVA: 0x0003D2F7 File Offset: 0x0003B6F7
	public static Vector3 FromYZ(Vector2 _yz, float _x)
	{
		return new Vector3(_x, _yz.x, _yz.y);
	}

	// Token: 0x06000B81 RID: 2945 RVA: 0x0003D30D File Offset: 0x0003B70D
	public static Vector3 WithX(this Vector3 _input, float _x)
	{
		return new Vector3(_x, _input.y, _input.z);
	}

	// Token: 0x06000B82 RID: 2946 RVA: 0x0003D323 File Offset: 0x0003B723
	public static Vector3 WithY(this Vector3 _input, float _y)
	{
		return new Vector3(_input.x, _y, _input.z);
	}

	// Token: 0x06000B83 RID: 2947 RVA: 0x0003D339 File Offset: 0x0003B739
	public static Vector3 WithZ(this Vector3 _input, float _z)
	{
		return new Vector3(_input.x, _input.y, _z);
	}

	// Token: 0x06000B84 RID: 2948 RVA: 0x0003D34F File Offset: 0x0003B74F
	public static Vector2 WithX(this Vector2 _input, float _x)
	{
		return new Vector2(_x, _input.y);
	}

	// Token: 0x06000B85 RID: 2949 RVA: 0x0003D35E File Offset: 0x0003B75E
	public static Vector2 WithY(this Vector2 _input, float _y)
	{
		return new Vector2(_input.x, _y);
	}

	// Token: 0x06000B86 RID: 2950 RVA: 0x0003D36D File Offset: 0x0003B76D
	public static Vector3 AddX(this Vector3 _input, float _x)
	{
		return new Vector3(_x + _input.x, _input.y, _input.z);
	}

	// Token: 0x06000B87 RID: 2951 RVA: 0x0003D38B File Offset: 0x0003B78B
	public static Vector3 AddY(this Vector3 _input, float _y)
	{
		return new Vector3(_input.x, _input.y + _y, _input.z);
	}

	// Token: 0x06000B88 RID: 2952 RVA: 0x0003D3A9 File Offset: 0x0003B7A9
	public static Vector3 AddZ(this Vector3 _input, float _z)
	{
		return new Vector3(_input.x, _input.y, _input.z + _z);
	}

	// Token: 0x06000B89 RID: 2953 RVA: 0x0003D3C7 File Offset: 0x0003B7C7
	public static Vector3 XZY(this Vector3 _input)
	{
		return new Vector3(_input.x, _input.z, _input.y);
	}

	// Token: 0x06000B8A RID: 2954 RVA: 0x0003D3E3 File Offset: 0x0003B7E3
	public static Vector3 SafeNormalised(this Vector3 _vector, Vector3 _default)
	{
		if (_vector.sqrMagnitude < 0.0001f)
		{
			return _default;
		}
		return _vector.normalized;
	}

	// Token: 0x06000B8B RID: 2955 RVA: 0x0003D3FF File Offset: 0x0003B7FF
	public static Vector3 MultipliedBy(this Vector3 _a, Vector3 _b)
	{
		return new Vector3(_a.x * _b.x, _a.y * _b.y, _a.z * _b.z);
	}

	// Token: 0x06000B8C RID: 2956 RVA: 0x0003D433 File Offset: 0x0003B833
	public static Vector2 MultipliedBy(this Vector2 _a, Vector2 _b)
	{
		return new Vector2(_a.x * _b.x, _a.y * _b.y);
	}

	// Token: 0x06000B8D RID: 2957 RVA: 0x0003D458 File Offset: 0x0003B858
	public static Vector2 MultipliedBy(this Vector2 _a, float x, float y)
	{
		return new Vector2(_a.x * x, _a.y * y);
	}

	// Token: 0x06000B8E RID: 2958 RVA: 0x0003D471 File Offset: 0x0003B871
	public static Vector3 DividedBy(this Vector3 _a, Vector3 _b)
	{
		return new Vector3(_a.x / _b.x, _a.y / _b.y, _a.z / _b.z);
	}

	// Token: 0x06000B8F RID: 2959 RVA: 0x0003D4A5 File Offset: 0x0003B8A5
	public static Vector2 DividedBy(this Vector2 _a, Vector2 _b)
	{
		return new Vector2(_a.x / _b.x, _a.y / _b.y);
	}

	// Token: 0x06000B90 RID: 2960 RVA: 0x0003D4CC File Offset: 0x0003B8CC
	public static Vector3 Select(Vector3 _a, Vector3 _b, bool _x, bool _y, bool _z)
	{
		return new Vector3((!_x) ? _b.x : _a.x, (!_y) ? _b.y : _a.y, (!_z) ? _b.z : _a.z);
	}

	// Token: 0x06000B91 RID: 2961 RVA: 0x0003D52A File Offset: 0x0003B92A
	public static Vector2 Select(Vector2 _a, Vector2 _b, bool _x, bool _y)
	{
		return new Vector2((!_x) ? _b.x : _a.x, (!_y) ? _b.y : _a.y);
	}

	// Token: 0x06000B92 RID: 2962 RVA: 0x0003D563 File Offset: 0x0003B963
	public static Vector2 Clamp(this Vector2 _input, Vector2 _min, Vector2 _max)
	{
		return VectorUtils.Max(VectorUtils.Min(_input, _max), _min);
	}

	// Token: 0x06000B93 RID: 2963 RVA: 0x0003D572 File Offset: 0x0003B972
	public static Vector3 Clamp(this Vector3 _input, Vector3 _min, Vector3 _max)
	{
		return VectorUtils.Max(VectorUtils.Min(_input, _max), _min);
	}

	// Token: 0x06000B94 RID: 2964 RVA: 0x0003D581 File Offset: 0x0003B981
	public static Vector2 Min(Vector2 _a, Vector2 _b)
	{
		return new Vector2(Mathf.Min(_a.x, _b.x), Mathf.Min(_a.y, _b.y));
	}

	// Token: 0x06000B95 RID: 2965 RVA: 0x0003D5B0 File Offset: 0x0003B9B0
	public static Vector2 Min(Vector2[] _a)
	{
		return new Vector2(Mathf.Min(_a.ConvertAll((Vector2 x) => x.x)), Mathf.Min(_a.ConvertAll((Vector2 x) => x.y)));
	}

	// Token: 0x06000B96 RID: 2966 RVA: 0x0003D612 File Offset: 0x0003BA12
	public static Vector3 Min(Vector3 _a, Vector3 _b)
	{
		return new Vector3(Mathf.Min(_a.x, _b.x), Mathf.Min(_a.y, _b.y), Mathf.Min(_a.z, _b.z));
	}

	// Token: 0x06000B97 RID: 2967 RVA: 0x0003D654 File Offset: 0x0003BA54
	public static Vector3 Min(Vector3[] _a)
	{
		return new Vector3(Mathf.Min(_a.ConvertAll((Vector3 x) => x.x)), Mathf.Min(_a.ConvertAll((Vector3 x) => x.y)), Mathf.Min(_a.ConvertAll((Vector3 x) => x.z)));
	}

	// Token: 0x06000B98 RID: 2968 RVA: 0x0003D6DE File Offset: 0x0003BADE
	public static Vector2 Max(Vector2 _a, Vector2 _b)
	{
		return new Vector2(Mathf.Max(_a.x, _b.x), Mathf.Max(_a.y, _b.y));
	}

	// Token: 0x06000B99 RID: 2969 RVA: 0x0003D70C File Offset: 0x0003BB0C
	public static Vector2 Max(Vector2[] _a)
	{
		return new Vector2(Mathf.Max(_a.ConvertAll((Vector2 x) => x.x)), Mathf.Max(_a.ConvertAll((Vector2 x) => x.y)));
	}

	// Token: 0x06000B9A RID: 2970 RVA: 0x0003D76E File Offset: 0x0003BB6E
	public static Vector3 Max(Vector3 _a, Vector3 _b)
	{
		return new Vector3(Mathf.Max(_a.x, _b.x), Mathf.Max(_a.y, _b.y), Mathf.Max(_a.z, _b.z));
	}

	// Token: 0x06000B9B RID: 2971 RVA: 0x0003D7B0 File Offset: 0x0003BBB0
	public static Vector3 Max(Vector3[] _a)
	{
		return new Vector3(Mathf.Max(_a.ConvertAll((Vector3 x) => x.x)), Mathf.Max(_a.ConvertAll((Vector3 x) => x.y)), Mathf.Max(_a.ConvertAll((Vector3 x) => x.z)));
	}

	// Token: 0x06000B9C RID: 2972 RVA: 0x0003D83A File Offset: 0x0003BC3A
	public static float Hmin(Vector3 _a)
	{
		return Mathf.Min(Mathf.Min(_a.x, _a.y), _a.z);
	}

	// Token: 0x06000B9D RID: 2973 RVA: 0x0003D85B File Offset: 0x0003BC5B
	public static float Hmax(Vector3 _a)
	{
		return Mathf.Max(Mathf.Max(_a.x, _a.y), _a.z);
	}

	// Token: 0x06000B9E RID: 2974 RVA: 0x0003D87C File Offset: 0x0003BC7C
	public static Vector3 Rcp(Vector3 _a)
	{
		return new Vector3(1f / _a.x, 1f / _a.y, 1f / _a.z);
	}

	// Token: 0x06000B9F RID: 2975 RVA: 0x0003D8AA File Offset: 0x0003BCAA
	public static Vector3 Abs(Vector3 _a)
	{
		return new Vector3(Mathf.Abs(_a.x), Mathf.Abs(_a.y), Mathf.Abs(_a.z));
	}

	// Token: 0x06000BA0 RID: 2976 RVA: 0x0003D8D8 File Offset: 0x0003BCD8
	public static float DistanceSq(Vector3 _a, Vector3 _b)
	{
		return (_a - _b).sqrMagnitude;
	}

	// Token: 0x06000BA1 RID: 2977 RVA: 0x0003D8F4 File Offset: 0x0003BCF4
	public static float ProgressUnclamped(Vector3 _a, Vector3 _b, Vector3 _pos)
	{
		Vector3 onNormal = _b - _a;
		Vector3 vector = _pos - _a;
		Vector3 vector2 = Vector3.Project(vector, onNormal);
		float num = vector2.magnitude / onNormal.magnitude;
		float num2 = Vector3.Dot(onNormal.normalized, vector2.normalized);
		return num * num2;
	}

	// Token: 0x06000BA2 RID: 2978 RVA: 0x0003D942 File Offset: 0x0003BD42
	public static float Progress(Vector3 _a, Vector3 _b, Vector3 _pos)
	{
		return Mathf.Clamp01(VectorUtils.ProgressUnclamped(_a, _b, _pos));
	}
}
