using System;

// Token: 0x02000264 RID: 612
public class CircleArcSet
{
	// Token: 0x06000B45 RID: 2885 RVA: 0x0003C6AA File Offset: 0x0003AAAA
	public CircleArcSet()
	{
	}

	// Token: 0x06000B46 RID: 2886 RVA: 0x0003C6BE File Offset: 0x0003AABE
	public CircleArcSet(CircleArc[] _arcs)
	{
		this.Arcs = _arcs;
	}

	// Token: 0x06000B47 RID: 2887 RVA: 0x0003C6DC File Offset: 0x0003AADC
	public static CircleArcSet operator -(CircleArcSet _a, CircleArc _b)
	{
		CircleArcSet circleArcSet = new CircleArcSet();
		for (int i = 0; i < _a.Arcs.Length; i++)
		{
			circleArcSet.Arcs = circleArcSet.Arcs.Union((_a.Arcs[i] - _b).Arcs);
		}
		return circleArcSet;
	}

	// Token: 0x06000B48 RID: 2888 RVA: 0x0003C730 File Offset: 0x0003AB30
	public static CircleArcSet operator -(CircleArcSet _a, CircleArcSet _b)
	{
		for (int i = 0; i < _b.Arcs.Length; i++)
		{
			_a -= _b.Arcs[i];
		}
		return _a;
	}

	// Token: 0x040008D4 RID: 2260
	public CircleArc[] Arcs = new CircleArc[0];
}
