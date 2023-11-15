using System;
using UnityEngine;

// Token: 0x020003E7 RID: 999
public class PreperationContainerCosmeticDecisions : MonoBehaviour
{
	// Token: 0x06001264 RID: 4708 RVA: 0x00067A44 File Offset: 0x00065E44
	private void Awake()
	{
		if (this.m_labelGUI != null)
		{
			this.m_startingText = this.m_labelGUI.GetText();
			this.m_startingRect = this.m_labelGUI.GetGUIRect();
		}
		this.m_iOrderDefinition = base.gameObject.RequireInterface<IOrderDefinition>();
		this.m_iOrderDefinition.RegisterOrderCompositionChangedCallback(new OrderCompositionChangedCallback(this.OnOrderCompositionChanged));
	}

	// Token: 0x06001265 RID: 4709 RVA: 0x00067AAC File Offset: 0x00065EAC
	private void OnOrderCompositionChanged(AssembledDefinitionNode _contents)
	{
		CompositeAssembledNode compositeAssembledNode = _contents as CompositeAssembledNode;
		if (this.m_labelGUI != null)
		{
			string text = string.Empty;
			for (int i = 0; i < compositeAssembledNode.m_composition.Length; i++)
			{
				text = text + "\n+" + compositeAssembledNode.m_composition[i].ToString();
			}
			GUIRect guirect = this.m_startingRect.DeepCopy();
			guirect.m_rect.height = (float)(compositeAssembledNode.m_composition.Length + 1) * guirect.m_rect.height;
			this.m_labelGUI.SetGUIRect(guirect);
			this.m_labelGUI.SetText(this.m_startingText + ":" + text);
		}
	}

	// Token: 0x04000E64 RID: 3684
	[SerializeField]
	private LabelGUI m_labelGUI;

	// Token: 0x04000E65 RID: 3685
	private IOrderDefinition m_iOrderDefinition;

	// Token: 0x04000E66 RID: 3686
	private GUIRect m_startingRect;

	// Token: 0x04000E67 RID: 3687
	private string m_startingText;
}
