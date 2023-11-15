using System;
using UnityEngine;

// Token: 0x0200046D RID: 1133
public interface IMovingSurface
{
	// Token: 0x0600150C RID: 5388
	Vector3 CalculateVelocityAtPoint(Vector3 _point, IMovingSurface _prevSurface);

	// Token: 0x0600150D RID: 5389
	Quaternion CalculateRotationAtPoint(Vector3 _point, Quaternion _prevRotation);
}
