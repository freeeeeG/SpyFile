using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000002 RID: 2
[RequireComponent(typeof(Text))]
public class FadeText : MonoBehaviour
{
	// Token: 0x06000001 RID: 1 RVA: 0x00002050 File Offset: 0x00000250
	private void Start()
	{
		this.textRef = base.GetComponent<Text>();
	}

	// Token: 0x06000002 RID: 2 RVA: 0x00002060 File Offset: 0x00000260
	private void Update()
	{
		this.alpha = this.textRef.color.a;
		if (this.alpha > 0f)
		{
			this.alpha -= 0.01f;
			Color color = new Color(1f, 1f, 1f, this.alpha);
			this.textRef.color = color;
		}
	}

	// Token: 0x04000001 RID: 1
	private Text textRef;

	// Token: 0x04000002 RID: 2
	public float alpha;
}
