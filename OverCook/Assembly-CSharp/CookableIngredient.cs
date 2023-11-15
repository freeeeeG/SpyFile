using System;
using UnityEngine;

// Token: 0x02000473 RID: 1139
[AddComponentMenu("Scripts/Game/Environment/CookableIngredient")]
[RequireComponent(typeof(CookingHandler))]
public class CookableIngredient : MonoBehaviour
{
	// Token: 0x04001045 RID: 4165
	[SerializeField]
	public IngredientOrderNode m_ingredientOrderNode;
}
