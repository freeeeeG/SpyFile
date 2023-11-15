using System;
using UnityEngine;

// Token: 0x020003AD RID: 941
public class DirtyPlateStackCosmetics : MonoBehaviour
{
	// Token: 0x0600119D RID: 4509 RVA: 0x00064CF4 File Offset: 0x000630F4
	private void Start()
	{
		if (this.m_washable == null)
		{
			this.SetupWashable();
		}
		if (this.m_plateStack == null)
		{
			this.SetupPlateStack();
		}
		this.m_propertyBlock = new MaterialPropertyBlock();
	}

	// Token: 0x0600119E RID: 4510 RVA: 0x00064D2F File Offset: 0x0006312F
	private void SetupWashable()
	{
		this.m_washable = base.gameObject.RequestComponent<ClientWashable>();
	}

	// Token: 0x0600119F RID: 4511 RVA: 0x00064D44 File Offset: 0x00063144
	private void SetupPlateStack()
	{
		this.m_plateStack = base.gameObject.RequestComponent<ClientDirtyPlateStack>();
		if (this.m_plateStack != null)
		{
			this.m_plateStack.RegisterOnPlateAdded(new GenericVoid<GameObject>(this.PlateChanged));
			this.m_plateStack.RegisterOnPlateRemoved(new GenericVoid<GameObject>(this.PlateChanged));
			this.UpdateRenderers();
		}
	}

	// Token: 0x060011A0 RID: 4512 RVA: 0x00064DA7 File Offset: 0x000631A7
	private void PlateChanged(GameObject _plate)
	{
		this.UpdateRenderers();
	}

	// Token: 0x060011A1 RID: 4513 RVA: 0x00064DAF File Offset: 0x000631AF
	private void UpdateRenderers()
	{
		this.m_renderers = base.gameObject.RequestComponentsRecursive<Renderer>();
		this.m_refreshProgress = true;
	}

	// Token: 0x060011A2 RID: 4514 RVA: 0x00064DCC File Offset: 0x000631CC
	private void Update()
	{
		if (this.m_washable == null)
		{
			this.SetupWashable();
		}
		if (this.m_plateStack == null)
		{
			this.SetupPlateStack();
		}
		if (this.m_washable != null && this.m_renderers != null)
		{
			float progressPercent = this.m_washable.ProgressPercent;
			float num = Mathf.Clamp01(Mathf.SmoothStep(0f, 1f, progressPercent) - 0.5f);
			if (this.m_lastProgress != num || this.m_refreshProgress)
			{
				this.m_propertyBlock.SetFloat(this.m_blendID, num);
				for (int i = 0; i < this.m_renderers.Length; i++)
				{
					Renderer renderer = this.m_renderers[i];
					renderer.SetPropertyBlock(this.m_propertyBlock);
				}
				this.m_lastProgress = num;
				this.m_refreshProgress = false;
			}
		}
	}

	// Token: 0x060011A3 RID: 4515 RVA: 0x00064EB0 File Offset: 0x000632B0
	private void OnDestroy()
	{
		if (this.m_plateStack != null)
		{
			this.m_plateStack.UnregisterOnPlateAdded(new GenericVoid<GameObject>(this.PlateChanged));
			this.m_plateStack.UnregisterOnPlateRemoved(new GenericVoid<GameObject>(this.PlateChanged));
		}
	}

	// Token: 0x04000DB4 RID: 3508
	private int m_blendID = Shader.PropertyToID("_BlendFactor");

	// Token: 0x04000DB5 RID: 3509
	private const float kProgressOffset = 0.5f;

	// Token: 0x04000DB6 RID: 3510
	private ClientWashable m_washable;

	// Token: 0x04000DB7 RID: 3511
	private ClientDirtyPlateStack m_plateStack;

	// Token: 0x04000DB8 RID: 3512
	private MaterialPropertyBlock m_propertyBlock;

	// Token: 0x04000DB9 RID: 3513
	private Renderer[] m_renderers;

	// Token: 0x04000DBA RID: 3514
	private float m_lastProgress;

	// Token: 0x04000DBB RID: 3515
	private bool m_refreshProgress;
}
