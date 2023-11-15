using System;
using System.Collections.Generic;
using Team17.Online.Multiplayer.Messaging;
using UnityEngine;

// Token: 0x020004CF RID: 1231
public class ServerIngredientContainer : ServerSynchroniserBase, IIngredientContents
{
	// Token: 0x060016B7 RID: 5815 RVA: 0x00076E40 File Offset: 0x00075240
	public static List<ServerIngredientContainer> GetAllIngredientContainers()
	{
		return ServerIngredientContainer.ms_AllIngredientContainers;
	}

	// Token: 0x060016B8 RID: 5816 RVA: 0x00076E47 File Offset: 0x00075247
	protected override void OnEnable()
	{
		base.OnEnable();
		ServerIngredientContainer.ms_AllIngredientContainers.Add(this);
	}

	// Token: 0x060016B9 RID: 5817 RVA: 0x00076E5A File Offset: 0x0007525A
	protected override void OnDisable()
	{
		base.OnDisable();
		ServerIngredientContainer.ms_AllIngredientContainers.Remove(this);
	}

	// Token: 0x060016BA RID: 5818 RVA: 0x00076E6E File Offset: 0x0007526E
	public override void StartSynchronising(Component synchronisedObject)
	{
		this.m_ingredientContainer = (IngredientContainer)synchronisedObject;
		this.m_attachment = base.gameObject.GetComponent<ServerPhysicalAttachment>();
	}

	// Token: 0x060016BB RID: 5819 RVA: 0x00076E8D File Offset: 0x0007528D
	public override EntityType GetEntityType()
	{
		return EntityType.IngredientContainer;
	}

	// Token: 0x060016BC RID: 5820 RVA: 0x00076E91 File Offset: 0x00075291
	public bool HasContents()
	{
		return this.m_contents.Count > 0;
	}

	// Token: 0x060016BD RID: 5821 RVA: 0x00076EA4 File Offset: 0x000752A4
	public bool CanTakeContents(AssembledDefinitionNode[] _contents)
	{
		int num = this.m_ingredientContainer.m_capacity - this.m_contents.Count;
		return num >= _contents.Length;
	}

	// Token: 0x060016BE RID: 5822 RVA: 0x00076ED2 File Offset: 0x000752D2
	public bool CanAddIngredient(AssembledDefinitionNode _orderData)
	{
		return this.m_contents.Count < this.m_ingredientContainer.m_capacity;
	}

	// Token: 0x060016BF RID: 5823 RVA: 0x00076EEC File Offset: 0x000752EC
	public virtual void AddIngredient(AssembledDefinitionNode _orderData)
	{
		this.m_contents.Add(_orderData);
		this.OnContentsChanged();
	}

	// Token: 0x060016C0 RID: 5824 RVA: 0x00076F00 File Offset: 0x00075300
	public AssembledDefinitionNode RemoveIngredient(int i)
	{
		if (this.m_contents.Count > i)
		{
			AssembledDefinitionNode assembledDefinitionNode = this.m_contents[i];
			this.m_contents.Remove(assembledDefinitionNode);
			this.OnContentsChanged();
			return assembledDefinitionNode;
		}
		return null;
	}

	// Token: 0x060016C1 RID: 5825 RVA: 0x00076F41 File Offset: 0x00075341
	public int GetContentsCount()
	{
		return this.m_contents.Count;
	}

	// Token: 0x060016C2 RID: 5826 RVA: 0x00076F4E File Offset: 0x0007534E
	public AssembledDefinitionNode GetContentsElement(int i)
	{
		return this.m_contents[i];
	}

	// Token: 0x060016C3 RID: 5827 RVA: 0x00076F5C File Offset: 0x0007535C
	public void Empty()
	{
		this.m_contents.Clear();
		this.OnContentsChanged();
	}

	// Token: 0x060016C4 RID: 5828 RVA: 0x00076F6F File Offset: 0x0007536F
	public bool CanTakeCopiedContents(ServerIngredientContainer _container)
	{
		return _container.m_contents.Count > 0 && this.CanTakeContents(_container.GetContents());
	}

	// Token: 0x060016C5 RID: 5829 RVA: 0x00076F91 File Offset: 0x00075391
	public void CopyContents(ServerIngredientContainer _container)
	{
		this.CopyContents(_container.GetContents());
	}

	// Token: 0x060016C6 RID: 5830 RVA: 0x00076F9F File Offset: 0x0007539F
	public void CopyContents(AssembledDefinitionNode[] _contents)
	{
		this.m_contents.AddRange(_contents);
		this.OnContentsChanged();
	}

	// Token: 0x060016C7 RID: 5831 RVA: 0x00076FB3 File Offset: 0x000753B3
	public AssembledDefinitionNode[] GetContents()
	{
		return this.m_contents.ToArray();
	}

	// Token: 0x060016C8 RID: 5832 RVA: 0x00076FC0 File Offset: 0x000753C0
	public void RegisterContentsChangedCallback(ContentsChangedCallback _callback)
	{
		this.m_contentsChangedCallback = (ContentsChangedCallback)Delegate.Combine(this.m_contentsChangedCallback, _callback);
	}

	// Token: 0x060016C9 RID: 5833 RVA: 0x00076FD9 File Offset: 0x000753D9
	public void UnregisterContentsChangedCallback(ContentsChangedCallback _callback)
	{
		this.m_contentsChangedCallback = (ContentsChangedCallback)Delegate.Remove(this.m_contentsChangedCallback, _callback);
	}

	// Token: 0x060016CA RID: 5834 RVA: 0x00076FF2 File Offset: 0x000753F2
	public void InformOfInternalChange()
	{
		this.OnContentsChanged();
	}

	// Token: 0x060016CB RID: 5835 RVA: 0x00076FFC File Offset: 0x000753FC
	private void OnContentsChanged()
	{
		AssembledDefinitionNode[] contents = this.GetContents();
		this.m_contentsChangedCallback(contents);
		this.m_ServerData.Initialise(contents);
		this.SendServerEvent(this.m_ServerData);
	}

	// Token: 0x060016CC RID: 5836 RVA: 0x00077034 File Offset: 0x00075434
	public override void UpdateSynchronising()
	{
		if (this.m_ingredientContainer != null && this.m_ingredientContainer.m_onSurfaceTriggerZone != null && this.m_attachment != null)
		{
			this.m_ingredientContainer.m_onSurfaceTriggerZone.enabled = this.m_attachment.IsAttached();
		}
	}

	// Token: 0x060016CD RID: 5837 RVA: 0x00077094 File Offset: 0x00075494
	public override Serialisable GetServerUpdate()
	{
		if (this.m_bCachedActive == base.gameObject.activeSelf)
		{
			return null;
		}
		this.m_bCachedActive = base.gameObject.activeSelf;
		this.m_ServerData.Initialise(base.gameObject.activeSelf);
		this.SendServerEvent(this.m_ServerData);
		return null;
	}

	// Token: 0x040010FB RID: 4347
	private IngredientContainer m_ingredientContainer;

	// Token: 0x040010FC RID: 4348
	private List<AssembledDefinitionNode> m_contents = new List<AssembledDefinitionNode>();

	// Token: 0x040010FD RID: 4349
	private ContentsChangedCallback m_contentsChangedCallback = delegate(AssembledDefinitionNode[] _contents)
	{
	};

	// Token: 0x040010FE RID: 4350
	private ServerPhysicalAttachment m_attachment;

	// Token: 0x040010FF RID: 4351
	private IngredientContainerMessage m_ServerData = new IngredientContainerMessage();

	// Token: 0x04001100 RID: 4352
	private bool m_bCachedActive;

	// Token: 0x04001101 RID: 4353
	private static List<ServerIngredientContainer> ms_AllIngredientContainers = new List<ServerIngredientContainer>();
}
