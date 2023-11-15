using System;
using UnityEngine;

// Token: 0x02000011 RID: 17
public class ExampleSpline : MonoBehaviour
{
	// Token: 0x06000046 RID: 70 RVA: 0x00005AC0 File Offset: 0x00003CC0
	private void Start()
	{
		this.spline = new LTSpline(new Vector3[]
		{
			this.trans[0].position,
			this.trans[1].position,
			this.trans[2].position,
			this.trans[3].position,
			this.trans[4].position
		});
		this.ltLogo = GameObject.Find("LeanTweenLogo1");
		this.ltLogo2 = GameObject.Find("LeanTweenLogo2");
		LeanTween.moveSpline(this.ltLogo2, this.spline.pts, 1f).setEase(LeanTweenType.easeInOutQuad).setLoopPingPong().setOrientToPath(true);
		LeanTween.moveSpline(this.ltLogo2, new Vector3[]
		{
			Vector3.zero,
			Vector3.zero,
			new Vector3(1f, 1f, 1f),
			new Vector3(2f, 1f, 1f),
			new Vector3(2f, 1f, 1f)
		}, 1.5f).setUseEstimatedTime(true);
	}

	// Token: 0x06000047 RID: 71 RVA: 0x00005C18 File Offset: 0x00003E18
	private void Update()
	{
		this.ltLogo.transform.position = this.spline.point(this.iter);
		this.iter += Time.deltaTime * 0.1f;
		if (this.iter > 1f)
		{
			this.iter = 0f;
		}
	}

	// Token: 0x06000048 RID: 72 RVA: 0x00005C76 File Offset: 0x00003E76
	private void OnDrawGizmos()
	{
		if (this.spline != null)
		{
			this.spline.gizmoDraw(-1f);
		}
	}

	// Token: 0x04000054 RID: 84
	public Transform[] trans;

	// Token: 0x04000055 RID: 85
	private LTSpline spline;

	// Token: 0x04000056 RID: 86
	private GameObject ltLogo;

	// Token: 0x04000057 RID: 87
	private GameObject ltLogo2;

	// Token: 0x04000058 RID: 88
	private float iter;
}
