using System;
using UnityEngine;

// Token: 0x020009A9 RID: 2473
[RequireComponent(typeof(IOrderDefinition))]
public class IngredientContentGUI : MonoBehaviour
{
	// Token: 0x040026DC RID: 9948
	[SerializeField]
	public IngredientContentsUIContainer m_ingredientContentUIPrefab;

	// Token: 0x040026DD RID: 9949
	[SerializeField]
	public bool m_displayEmptyElements;

	// Token: 0x040026DE RID: 9950
	[SerializeField]
	public Vector3 m_Offset = Vector3.zero;
}
