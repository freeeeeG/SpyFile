using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020005D6 RID: 1494
public class Tray : Plate
{
	// Token: 0x06001C83 RID: 7299 RVA: 0x0008B4C0 File Offset: 0x000898C0
	public void GetIngredientContents(AssembledDefinitionNode _node, PlacementContext _context, List<int> _availableSlots)
	{
		_availableSlots.Clear();
		for (int i = 0; i < this.m_slots.Length; i++)
		{
			List<OrderContentRestriction> contentRestrictions = this.m_slots[i].GetContentRestrictions();
			for (int j = 0; j < contentRestrictions.Count; j++)
			{
				if (AssembledDefinitionNode.MatchingAlreadySimple(contentRestrictions[j].m_content.Simpilfy(), _node.Simpilfy()))
				{
					_availableSlots.Add(i);
				}
			}
		}
	}

	// Token: 0x06001C84 RID: 7300 RVA: 0x0008B53C File Offset: 0x0008993C
	public bool AreContentsDifferent(AssembledDefinitionNode[] _contents1, AssembledDefinitionNode[] _contents2)
	{
		if (_contents1 == null || _contents2 == null)
		{
			return true;
		}
		if (_contents1.Length == _contents2.Length)
		{
			for (int i = 0; i < _contents1.Length; i++)
			{
				if (!AssembledDefinitionNode.Matching(_contents1[i], _contents2[i]))
				{
					return true;
				}
			}
			return false;
		}
		return true;
	}

	// Token: 0x0400164B RID: 5707
	[SerializeField]
	public OrderToPrefabLookup[] m_slots;
}
