using System;
using UnityEngine;

// Token: 0x02000016 RID: 22
public class PathSplines : MonoBehaviour
{
	// Token: 0x0600005F RID: 95 RVA: 0x00006968 File Offset: 0x00004B68
	private void OnEnable()
	{
		this.cr = new LTSpline(new Vector3[]
		{
			this.trans[0].position,
			this.trans[1].position,
			this.trans[2].position,
			this.trans[3].position,
			this.trans[4].position
		});
	}

	// Token: 0x06000060 RID: 96 RVA: 0x000069EC File Offset: 0x00004BEC
	private void Start()
	{
		this.avatar1 = GameObject.Find("Avatar1");
		LeanTween.move(this.avatar1, this.cr, 6.5f).setOrientToPath(true).setRepeat(1).setOnComplete(delegate()
		{
			Vector3[] to = new Vector3[]
			{
				this.trans[4].position,
				this.trans[3].position,
				this.trans[2].position,
				this.trans[1].position,
				this.trans[0].position
			};
			LeanTween.moveSpline(this.avatar1, to, 6.5f);
		}).setEase(LeanTweenType.easeOutQuad);
	}

	// Token: 0x06000061 RID: 97 RVA: 0x00006A43 File Offset: 0x00004C43
	private void Update()
	{
		this.iter += Time.deltaTime * 0.07f;
		if (this.iter > 1f)
		{
			this.iter = 0f;
		}
	}

	// Token: 0x06000062 RID: 98 RVA: 0x00006A75 File Offset: 0x00004C75
	private void OnDrawGizmos()
	{
		if (this.cr == null)
		{
			this.OnEnable();
		}
		Gizmos.color = Color.red;
		if (this.cr != null)
		{
			this.cr.gizmoDraw(-1f);
		}
	}

	// Token: 0x04000081 RID: 129
	public Transform[] trans;

	// Token: 0x04000082 RID: 130
	private LTSpline cr;

	// Token: 0x04000083 RID: 131
	private GameObject avatar1;

	// Token: 0x04000084 RID: 132
	private float iter;
}
