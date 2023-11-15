using System;
using UnityEngine;

// Token: 0x020001BC RID: 444
[RequireComponent(typeof(MaterialAccessor))]
[ExecuteInEditMode]
public class AnimatedMaterial : MonoBehaviour
{
	// Token: 0x060007A9 RID: 1961 RVA: 0x0002FEB5 File Offset: 0x0002E2B5
	private void Awake()
	{
		this.OnValidate();
	}

	// Token: 0x060007AA RID: 1962 RVA: 0x0002FEBD File Offset: 0x0002E2BD
	private void LateUpdate()
	{
		this.OnValidate();
	}

	// Token: 0x060007AB RID: 1963 RVA: 0x0002FEC5 File Offset: 0x0002E2C5
	private void OnValidate()
	{
		if (this.m_executeInEditMode || (Application.isPlaying && base.enabled))
		{
			this.m_materialAccessor.AccessMaterial.SetFloat(this.m_propertyName, this.m_floatValue);
		}
	}

	// Token: 0x04000610 RID: 1552
	[SerializeField]
	private string m_propertyName;

	// Token: 0x04000611 RID: 1553
	[SerializeField]
	private float m_floatValue;

	// Token: 0x04000612 RID: 1554
	[SerializeField]
	private bool m_executeInEditMode;

	// Token: 0x04000613 RID: 1555
	[SerializeField]
	[AssignComponent(Editorbility.NonEditable)]
	private MaterialAccessor m_materialAccessor;
}
