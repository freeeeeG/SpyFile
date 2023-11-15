using System;
using UnityEngine;

// Token: 0x02000010 RID: 16
public class PathBezier2d : MonoBehaviour
{
	// Token: 0x06000043 RID: 67 RVA: 0x000059FC File Offset: 0x00003BFC
	private void Start()
	{
		Vector3[] array = new Vector3[]
		{
			this.cubes[0].position,
			this.cubes[1].position,
			this.cubes[2].position,
			this.cubes[3].position
		};
		this.visualizePath = new LTBezierPath(array);
		LeanTween.move(this.dude1, array, 10f).setOrientToPath2d(true);
		LeanTween.moveLocal(this.dude2, array, 10f).setOrientToPath2d(true);
	}

	// Token: 0x06000044 RID: 68 RVA: 0x00005A9C File Offset: 0x00003C9C
	private void OnDrawGizmos()
	{
		Gizmos.color = Color.red;
		if (this.visualizePath != null)
		{
			this.visualizePath.gizmoDraw(-1f);
		}
	}

	// Token: 0x04000050 RID: 80
	public Transform[] cubes;

	// Token: 0x04000051 RID: 81
	public GameObject dude1;

	// Token: 0x04000052 RID: 82
	public GameObject dude2;

	// Token: 0x04000053 RID: 83
	private LTBezierPath visualizePath;
}
