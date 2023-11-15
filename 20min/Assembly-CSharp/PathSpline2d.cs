using System;
using UnityEngine;

// Token: 0x02000012 RID: 18
public class PathSpline2d : MonoBehaviour
{
	// Token: 0x0600004A RID: 74 RVA: 0x00005C90 File Offset: 0x00003E90
	private void Start()
	{
		Vector3[] array = new Vector3[]
		{
			this.cubes[0].position,
			this.cubes[1].position,
			this.cubes[2].position,
			this.cubes[3].position,
			this.cubes[4].position
		};
		this.visualizePath = new LTSpline(array);
		LeanTween.moveSpline(this.dude1, array, 10f).setOrientToPath2d(true).setSpeed(2f);
		LeanTween.moveSplineLocal(this.dude2, array, 10f).setOrientToPath2d(true).setSpeed(2f);
	}

	// Token: 0x0600004B RID: 75 RVA: 0x00005D58 File Offset: 0x00003F58
	private void OnDrawGizmos()
	{
		Gizmos.color = Color.red;
		if (this.visualizePath != null)
		{
			this.visualizePath.gizmoDraw(-1f);
		}
	}

	// Token: 0x04000059 RID: 89
	public Transform[] cubes;

	// Token: 0x0400005A RID: 90
	public GameObject dude1;

	// Token: 0x0400005B RID: 91
	public GameObject dude2;

	// Token: 0x0400005C RID: 92
	private LTSpline visualizePath;
}
