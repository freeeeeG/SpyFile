using System;
using System.Collections.Generic;
using Team17.Online.Multiplayer.Messaging;
using UnityEngine;

// Token: 0x020004D0 RID: 1232
public class ClientIngredientContainer : ClientSynchroniserBase, IIngredientContents
{
	// Token: 0x060016D1 RID: 5841 RVA: 0x00077131 File Offset: 0x00075531
	public override void StartSynchronising(Component synchronisedObject)
	{
		this.m_ingredientContainer = base.gameObject.RequireComponent<IngredientContainer>();
	}

	// Token: 0x060016D2 RID: 5842 RVA: 0x00077144 File Offset: 0x00075544
	public override EntityType GetEntityType()
	{
		return EntityType.IngredientContainer;
	}

	// Token: 0x060016D3 RID: 5843 RVA: 0x00077148 File Offset: 0x00075548
	public override void ApplyServerEvent(Serialisable serialisable)
	{
		IngredientContainerMessage ingredientContainerMessage = (IngredientContainerMessage)serialisable;
		IngredientContainerMessage.MessageType type = ingredientContainerMessage.Type;
		if (type != IngredientContainerMessage.MessageType.ActiveState)
		{
			if (type == IngredientContainerMessage.MessageType.ContentsChanged)
			{
				this.m_contents.Clear();
				this.m_contents.AddRange(ingredientContainerMessage.Contents);
				this.m_contentsChangedCallback(this.GetContents());
			}
		}
		else
		{
			base.gameObject.SetActive(ingredientContainerMessage.ActiveState);
		}
	}

	// Token: 0x060016D4 RID: 5844 RVA: 0x000771BD File Offset: 0x000755BD
	public AssembledDefinitionNode[] GetContents()
	{
		return this.m_contents.ToArray();
	}

	// Token: 0x060016D5 RID: 5845 RVA: 0x000771CA File Offset: 0x000755CA
	public void RegisterContentsChangedCallback(ContentsChangedCallback _callback)
	{
		this.m_contentsChangedCallback = (ContentsChangedCallback)Delegate.Combine(this.m_contentsChangedCallback, _callback);
	}

	// Token: 0x060016D6 RID: 5846 RVA: 0x000771E3 File Offset: 0x000755E3
	public void UnregisterContentsChangedCallback(ContentsChangedCallback _callback)
	{
		this.m_contentsChangedCallback = (ContentsChangedCallback)Delegate.Remove(this.m_contentsChangedCallback, _callback);
	}

	// Token: 0x060016D7 RID: 5847 RVA: 0x000771FC File Offset: 0x000755FC
	public bool CanAddIngredient(AssembledDefinitionNode _orderData)
	{
		return this.m_contents.Count < this.m_ingredientContainer.m_capacity;
	}

	// Token: 0x060016D8 RID: 5848 RVA: 0x00077216 File Offset: 0x00075616
	public int GetContentsCount()
	{
		return this.m_contents.Count;
	}

	// Token: 0x060016D9 RID: 5849 RVA: 0x00077224 File Offset: 0x00075624
	public bool CanTakeContents(AssembledDefinitionNode[] _contents)
	{
		int num = this.m_ingredientContainer.m_capacity - this.m_contents.Count;
		return num >= _contents.Length;
	}

	// Token: 0x060016DA RID: 5850 RVA: 0x00077252 File Offset: 0x00075652
	public void Empty()
	{
		throw new NotImplementedException();
	}

	// Token: 0x060016DB RID: 5851 RVA: 0x00077259 File Offset: 0x00075659
	public bool HasContents()
	{
		return this.m_contents.Count > 0;
	}

	// Token: 0x060016DC RID: 5852 RVA: 0x00077269 File Offset: 0x00075669
	public void AddIngredient(AssembledDefinitionNode _orderData)
	{
		throw new NotImplementedException();
	}

	// Token: 0x060016DD RID: 5853 RVA: 0x00077270 File Offset: 0x00075670
	public AssembledDefinitionNode RemoveIngredient(int i)
	{
		throw new NotImplementedException();
	}

	// Token: 0x060016DE RID: 5854 RVA: 0x00077277 File Offset: 0x00075677
	public AssembledDefinitionNode GetContentsElement(int i)
	{
		return this.m_contents[i];
	}

	// Token: 0x04001103 RID: 4355
	private List<AssembledDefinitionNode> m_contents = new List<AssembledDefinitionNode>();

	// Token: 0x04001104 RID: 4356
	private ContentsChangedCallback m_contentsChangedCallback = delegate(AssembledDefinitionNode[] _contents)
	{
	};

	// Token: 0x04001105 RID: 4357
	private IngredientContainer m_ingredientContainer;
}
