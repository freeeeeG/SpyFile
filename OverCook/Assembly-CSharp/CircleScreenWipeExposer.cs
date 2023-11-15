using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000A84 RID: 2692
[ExecuteInEditMode]
[RequireComponent(typeof(Image))]
public class CircleScreenWipeExposer : MonoBehaviour
{
	// Token: 0x170003B9 RID: 953
	// (get) Token: 0x06003535 RID: 13621 RVA: 0x000F8321 File Offset: 0x000F6721
	// (set) Token: 0x06003536 RID: 13622 RVA: 0x000F8329 File Offset: 0x000F6729
	public float Prop
	{
		get
		{
			return this.m_prop;
		}
		set
		{
			this.m_prop = value;
		}
	}

	// Token: 0x06003537 RID: 13623 RVA: 0x000F8332 File Offset: 0x000F6732
	private void Awake()
	{
		this.OnValidate();
	}

	// Token: 0x06003538 RID: 13624 RVA: 0x000F833A File Offset: 0x000F673A
	private void Update()
	{
		this.OnValidate();
	}

	// Token: 0x06003539 RID: 13625 RVA: 0x000F8344 File Offset: 0x000F6744
	private void OnValidate()
	{
		if (this.m_executeInEditMode || Application.isPlaying)
		{
			this.ImageProp = this.m_prop;
		}
		else if (!Application.isPlaying && !this.m_executeInEditMode && this.ImageProp != 1f)
		{
			this.ImageProp = 1f;
		}
	}

	// Token: 0x170003BA RID: 954
	// (get) Token: 0x0600353B RID: 13627 RVA: 0x000F8400 File Offset: 0x000F6800
	// (set) Token: 0x0600353A RID: 13626 RVA: 0x000F83A8 File Offset: 0x000F67A8
	private float ImageProp
	{
		get
		{
			Image image = base.gameObject.RequireComponent<Image>();
			return image.material.GetFloat("_Prop");
		}
		set
		{
			Image image = base.gameObject.RequireComponent<Image>();
			image.material.SetFloat("_Prop", value);
			image.SetMaterialDirty();
			Mask mask = base.gameObject.RequestComponent<Mask>();
			if (mask != null)
			{
				mask.enabled = false;
				mask.enabled = true;
			}
		}
	}

	// Token: 0x04002AAF RID: 10927
	[SerializeField]
	[Range(0f, 1f)]
	private float m_prop;

	// Token: 0x04002AB0 RID: 10928
	[SerializeField]
	private bool m_executeInEditMode;
}
