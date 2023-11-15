using System;
using UnityEngine;

// Token: 0x02000269 RID: 617
public static class RectUtils
{
	// Token: 0x06000B5F RID: 2911 RVA: 0x0003CE74 File Offset: 0x0003B274
	public static Rect Added(this Rect original, float x, float y)
	{
		return new Rect(original.x + x, original.y + y, original.width, original.height);
	}

	// Token: 0x06000B60 RID: 2912 RVA: 0x0003CE9B File Offset: 0x0003B29B
	public static Rect Added(this Rect original, Vector2 offset)
	{
		return new Rect(original.x + offset.x, original.y + offset.y, original.width, original.height);
	}

	// Token: 0x06000B61 RID: 2913 RVA: 0x0003CECE File Offset: 0x0003B2CE
	public static Rect SizeDivided(this Rect original, float x, float y)
	{
		return new Rect(original.x, original.y, original.width / x, original.height / y);
	}

	// Token: 0x06000B62 RID: 2914 RVA: 0x0003CEF5 File Offset: 0x0003B2F5
	public static Rect SizeMultiplied(this Rect original, float x, float y)
	{
		return new Rect(original.x, original.y, original.width * x, original.height * y);
	}

	// Token: 0x06000B63 RID: 2915 RVA: 0x0003CF1C File Offset: 0x0003B31C
	public static Rect FullMultiplied(this Rect original, float x, float y)
	{
		return new Rect(original.x * x, original.y * y, original.width * x, original.height * y);
	}

	// Token: 0x06000B64 RID: 2916 RVA: 0x0003CF48 File Offset: 0x0003B348
	public static Rect ResizesAboutCentre(this Rect original, float width, float height)
	{
		float x = original.x + 0.5f * (original.width - width);
		float y = original.y + 0.5f * (original.height - height);
		return new Rect(x, y, width, height);
	}

	// Token: 0x06000B65 RID: 2917 RVA: 0x0003CF8E File Offset: 0x0003B38E
	public static Rect WithCentre(this Rect original, float x, float y)
	{
		return new Rect(x - 0.5f * original.width, y - 0.5f * original.height, original.width, original.height);
	}

	// Token: 0x06000B66 RID: 2918 RVA: 0x0003CFC4 File Offset: 0x0003B3C4
	public static Rect ToWorld(this Rect _parent, Rect _local)
	{
		float x = MathUtils.Remap(_local.x, 0f, 1f, _parent.x, _parent.x + _parent.width);
		float y = MathUtils.Remap(_local.y, 0f, 1f, _parent.y, _parent.y + _parent.height);
		float width = _local.width * _parent.width;
		float height = _local.height * _parent.height;
		return new Rect(x, y, width, height);
	}

	// Token: 0x06000B67 RID: 2919 RVA: 0x0003D054 File Offset: 0x0003B454
	public static Rect ToLocal(this Rect _parent, Rect _world)
	{
		float x = MathUtils.Remap(_world.x, _parent.x, _parent.x + _parent.width, 0f, 1f);
		float y = MathUtils.Remap(_world.y, _parent.y, _parent.y + _parent.height, 0f, 1f);
		float width = _world.width / _parent.width;
		float height = _world.height / _parent.height;
		return new Rect(x, y, width, height);
	}
}
