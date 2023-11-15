using System;
using UnityEngine;
using UnityEngine.Rendering;

// Token: 0x020009D5 RID: 2517
public class CarryItemShadowDisable : MonoBehaviour
{
	// Token: 0x06003144 RID: 12612 RVA: 0x000E6D7C File Offset: 0x000E517C
	private void Awake()
	{
		this.m_attachmentCarrier.RegisterCarriedItemChangeCallback(new VoidGeneric<GameObject, GameObject>(this.OnItemChanged));
	}

	// Token: 0x06003145 RID: 12613 RVA: 0x000E6D95 File Offset: 0x000E5195
	private void OnItemChanged(GameObject _before, GameObject _after)
	{
		if (_before != null)
		{
			this.SetShadowCastingState(_before, ShadowCastingMode.On);
		}
		if (_after != null)
		{
			this.SetShadowCastingState(_after, ShadowCastingMode.Off);
		}
	}

	// Token: 0x06003146 RID: 12614 RVA: 0x000E6DC0 File Offset: 0x000E51C0
	private void SetShadowCastingState(GameObject _obj, ShadowCastingMode _shadowCasting)
	{
		Renderer[] array = _obj.RequestComponentsRecursive<Renderer>();
		for (int i = 0; i < array.Length; i++)
		{
			array[i].shadowCastingMode = _shadowCasting;
		}
	}

	// Token: 0x04002775 RID: 10101
	[SerializeField]
	[AssignComponent(Editorbility.NonEditable)]
	private ServerPlayerAttachmentCarrier m_attachmentCarrier;
}
