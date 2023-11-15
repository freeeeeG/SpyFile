using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020001CB RID: 459
public class MaterialAccessor : MonoBehaviour
{
	// Token: 0x170000C6 RID: 198
	// (get) Token: 0x060007E1 RID: 2017 RVA: 0x00030DF4 File Offset: 0x0002F1F4
	// (set) Token: 0x060007E2 RID: 2018 RVA: 0x00030E48 File Offset: 0x0002F248
	public Material AccessMaterial
	{
		get
		{
			MaterialAccessor.MaterialAccessorType accessorType = this.m_accessorType;
			if (accessorType == MaterialAccessor.MaterialAccessorType.Renderer)
			{
				return this.m_renderer.material;
			}
			if (accessorType == MaterialAccessor.MaterialAccessorType.Projector)
			{
				return this.m_projector.material;
			}
			if (accessorType != MaterialAccessor.MaterialAccessorType.Image)
			{
				return null;
			}
			return this.m_image.material;
		}
		set
		{
			MaterialAccessor.MaterialAccessorType accessorType = this.m_accessorType;
			if (accessorType != MaterialAccessor.MaterialAccessorType.Renderer)
			{
				if (accessorType != MaterialAccessor.MaterialAccessorType.Projector)
				{
					if (accessorType == MaterialAccessor.MaterialAccessorType.Image)
					{
						this.m_image.material = value;
					}
				}
				else
				{
					this.m_projector.material = value;
				}
			}
			else
			{
				this.m_renderer.material = value;
			}
		}
	}

	// Token: 0x04000640 RID: 1600
	[SerializeField]
	private MaterialAccessor.MaterialAccessorType m_accessorType;

	// Token: 0x04000641 RID: 1601
	[SerializeField]
	[AssignComponent(Editorbility.NonEditable)]
	private Renderer m_renderer;

	// Token: 0x04000642 RID: 1602
	[SerializeField]
	[AssignComponent(Editorbility.NonEditable)]
	private Projector m_projector;

	// Token: 0x04000643 RID: 1603
	[SerializeField]
	[AssignComponent(Editorbility.NonEditable)]
	private Image m_image;

	// Token: 0x020001CC RID: 460
	private enum MaterialAccessorType
	{
		// Token: 0x04000645 RID: 1605
		Renderer,
		// Token: 0x04000646 RID: 1606
		Projector,
		// Token: 0x04000647 RID: 1607
		Image
	}
}
