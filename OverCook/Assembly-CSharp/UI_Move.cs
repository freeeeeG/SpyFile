using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000B1F RID: 2847
public class UI_Move : MonoBehaviour
{
	// Token: 0x170003EF RID: 1007
	// (get) Token: 0x0600399F RID: 14751 RVA: 0x00111585 File Offset: 0x0010F985
	// (set) Token: 0x060039A0 RID: 14752 RVA: 0x0011158D File Offset: 0x0010F98D
	public Vector2 Offset
	{
		get
		{
			return this.m_Offset;
		}
		set
		{
			this.m_Offset = value;
			if (this.m_Material != null)
			{
				this.m_Material.SetVector(UI_Move.m_OffsetUniform, this.m_Offset);
			}
		}
	}

	// Token: 0x060039A1 RID: 14753 RVA: 0x001115C4 File Offset: 0x0010F9C4
	private void Start()
	{
		if (this.m_UIMaterial == null)
		{
		}
		if (this.m_Material == null && this.m_UIMaterial != null)
		{
			this.m_Material = new Material(this.m_UIMaterial);
		}
		this.UpdateGraphics();
	}

	// Token: 0x060039A2 RID: 14754 RVA: 0x0011161C File Offset: 0x0010FA1C
	public void UpdateGraphics()
	{
		this.m_GraphicsToMove = null;
		if (this.m_UIMaterial == null)
		{
			return;
		}
		if (this.m_Material == null)
		{
			this.m_Material = new Material(this.m_UIMaterial);
		}
		this.m_GraphicsToMove = base.gameObject.RequestComponentsRecursive<Graphic>();
		int num = this.m_GraphicsToMove.Length;
		for (int i = 0; i < num; i++)
		{
			this.m_GraphicsToMove[i].material = this.m_Material;
		}
	}

	// Token: 0x060039A3 RID: 14755 RVA: 0x001116A3 File Offset: 0x0010FAA3
	private void LateUpdate()
	{
		if (this.m_GraphicsToMove == null || this.m_GraphicsToMove.Length == 0)
		{
			this.UpdateGraphics();
		}
		this.m_Material.SetVector(UI_Move.m_OffsetUniform, this.m_Offset);
	}

	// Token: 0x04002E56 RID: 11862
	[SerializeField]
	private Vector2 m_Offset = Vector2.zero;

	// Token: 0x04002E57 RID: 11863
	[SerializeField]
	[AssignResource("UI_Move", Editorbility.Editable)]
	public Material m_UIMaterial;

	// Token: 0x04002E58 RID: 11864
	private Graphic[] m_GraphicsToMove;

	// Token: 0x04002E59 RID: 11865
	private Material m_Material;

	// Token: 0x04002E5A RID: 11866
	private static readonly int m_OffsetUniform = Shader.PropertyToID("_Offset");
}
