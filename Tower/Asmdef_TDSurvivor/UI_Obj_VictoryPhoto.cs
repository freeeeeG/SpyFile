using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x0200017E RID: 382
public class UI_Obj_VictoryPhoto : MonoBehaviour
{
	// Token: 0x06000A15 RID: 2581 RVA: 0x00025CD8 File Offset: 0x00023ED8
	private void Start()
	{
	}

	// Token: 0x06000A16 RID: 2582 RVA: 0x00025CDA File Offset: 0x00023EDA
	private void Update()
	{
	}

	// Token: 0x06000A17 RID: 2583 RVA: 0x00025CDC File Offset: 0x00023EDC
	public void SetupContent(RenderTexture tex)
	{
		this.image_RenderTexture.texture = tex;
	}

	// Token: 0x06000A18 RID: 2584 RVA: 0x00025CEA File Offset: 0x00023EEA
	public void Toggle(bool isOn)
	{
		this.animator.SetBool("isOn", isOn);
	}

	// Token: 0x040007D0 RID: 2000
	[SerializeField]
	private Animator animator;

	// Token: 0x040007D1 RID: 2001
	[SerializeField]
	private RawImage image_RenderTexture;
}
