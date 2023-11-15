using System;

// Token: 0x02000263 RID: 611
public class CircleArc
{
	// Token: 0x06000B42 RID: 2882 RVA: 0x0003C550 File Offset: 0x0003A950
	public CircleArc(float _min, float _max)
	{
		this.MinAngle = _min;
		this.MaxAngle = _max;
	}

	// Token: 0x06000B43 RID: 2883 RVA: 0x0003C568 File Offset: 0x0003A968
	private bool Contains(float x)
	{
		float minAngle = this.MinAngle;
		float num = this.MaxAngle;
		while (x < minAngle)
		{
			x += 6.2831855f;
		}
		while (num < minAngle)
		{
			num += 6.2831855f;
		}
		return x >= minAngle && x <= num;
	}

	// Token: 0x06000B44 RID: 2884 RVA: 0x0003C5C0 File Offset: 0x0003A9C0
	public static CircleArcSet operator -(CircleArc _a, CircleArc _b)
	{
		bool flag = _b.Contains(_a.MinAngle);
		bool flag2 = _b.Contains(_a.MaxAngle);
		if (flag && flag2)
		{
			return new CircleArcSet();
		}
		if (flag)
		{
			return new CircleArcSet(new CircleArc[]
			{
				new CircleArc(_b.MaxAngle, _a.MaxAngle)
			});
		}
		if (flag2)
		{
			return new CircleArcSet(new CircleArc[]
			{
				new CircleArc(_a.MinAngle, _b.MinAngle)
			});
		}
		if (_a.Contains(_b.MinAngle) && _a.Contains(_b.MaxAngle))
		{
			return new CircleArcSet(new CircleArc[]
			{
				new CircleArc(_a.MinAngle, _b.MinAngle),
				new CircleArc(_b.MaxAngle, _a.MaxAngle)
			});
		}
		return new CircleArcSet(new CircleArc[]
		{
			_a
		});
	}

	// Token: 0x040008D2 RID: 2258
	public float MinAngle;

	// Token: 0x040008D3 RID: 2259
	public float MaxAngle;
}
