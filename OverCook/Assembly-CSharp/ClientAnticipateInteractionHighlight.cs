using System;
using System.Collections;
using System.Collections.Generic;
using Team17.Online.Multiplayer.Messaging;
using UnityEngine;

// Token: 0x02000570 RID: 1392
public class ClientAnticipateInteractionHighlight : ClientSynchroniserBase, IAnticipateInteractionNotifications
{
	// Token: 0x06001A2E RID: 6702 RVA: 0x00083180 File Offset: 0x00081580
	public override void StartSynchronising(Component synchronisedObject)
	{
		this.m_highlight = (AnticipateInteractionHighlight)synchronisedObject;
		this.RebuildHighlightMaterials();
		ClientAttachStation clientAttachStation = base.gameObject.RequestComponent<ClientAttachStation>();
		if (clientAttachStation != null)
		{
			clientAttachStation.RegisterOnItemAdded(new ClientAttachStation.OnItemAdded(this.OnItemAttachmentAdded));
			clientAttachStation.RegisterOnItemRemoved(new ClientAttachStation.OnItemRemoved(this.OnItemAttachmentRemoved));
		}
		ClientPlateStackBase clientPlateStackBase = base.gameObject.RequestComponent<ClientPlateStackBase>();
		if (clientPlateStackBase != null)
		{
			clientPlateStackBase.RegisterOnPlateRemoved(new GenericVoid<GameObject>(this.OnPlateRemoved));
		}
		ClientIngredientContainer clientIngredientContainer = base.gameObject.RequestComponent<ClientIngredientContainer>();
		if (clientIngredientContainer != null)
		{
			clientIngredientContainer.RegisterContentsChangedCallback(new ContentsChangedCallback(this.OnContentsChanged));
		}
	}

	// Token: 0x06001A2F RID: 6703 RVA: 0x00083230 File Offset: 0x00081630
	protected override void OnDestroy()
	{
		ClientAttachStation clientAttachStation = base.gameObject.RequestComponent<ClientAttachStation>();
		if (clientAttachStation != null)
		{
			clientAttachStation.UnregisterOnItemAdded(new ClientAttachStation.OnItemAdded(this.OnItemAttachmentAdded));
			clientAttachStation.UnregisterOnItemRemoved(new ClientAttachStation.OnItemRemoved(this.OnItemAttachmentRemoved));
		}
		ClientPlateStackBase clientPlateStackBase = base.gameObject.RequestComponent<ClientPlateStackBase>();
		if (clientPlateStackBase != null)
		{
			clientPlateStackBase.UnregisterOnPlateRemoved(new GenericVoid<GameObject>(this.OnPlateRemoved));
		}
		ClientIngredientContainer clientIngredientContainer = base.gameObject.RequestComponent<ClientIngredientContainer>();
		if (clientIngredientContainer != null)
		{
			clientIngredientContainer.UnregisterContentsChangedCallback(new ContentsChangedCallback(this.OnContentsChanged));
		}
		base.OnDestroy();
	}

	// Token: 0x06001A30 RID: 6704 RVA: 0x000832D4 File Offset: 0x000816D4
	public void RebuildHighlightMaterials()
	{
		bool flag = this.IsHighlighted();
		if (flag)
		{
			this.Unhighlight();
		}
		this.FindRenderers();
		this.SaveRenderersStartingBrightness();
		if (flag)
		{
			this.Highlight();
		}
	}

	// Token: 0x06001A31 RID: 6705 RVA: 0x0008330C File Offset: 0x0008170C
	private void FindRenderers()
	{
		GameObject gameObject = this.m_highlight.m_highlightObjectOverride;
		if (gameObject == null)
		{
			gameObject = base.gameObject;
		}
		this.m_renderers.Clear();
		this.FindRenderersRecursive(gameObject);
	}

	// Token: 0x06001A32 RID: 6706 RVA: 0x0008334C File Offset: 0x0008174C
	private void FindRenderersRecursive(GameObject target)
	{
		Renderer[] collection = target.RequestComponents<Renderer>();
		this.m_renderers.AddRange(collection);
		for (int i = 0; i < target.transform.childCount; i++)
		{
			GameObject gameObject = target.transform.GetChild(i).gameObject;
			ClientAnticipateInteractionHighlight x = gameObject.RequestComponent<ClientAnticipateInteractionHighlight>();
			if (x == null || x == this)
			{
				this.FindRenderersRecursive(gameObject);
			}
		}
	}

	// Token: 0x06001A33 RID: 6707 RVA: 0x000833C0 File Offset: 0x000817C0
	private void SaveRenderersStartingBrightness()
	{
		this.m_startingBrightness = new float[this.m_renderers.Count][];
		for (int i = 0; i < this.m_renderers.Count; i++)
		{
			Material[] sharedMaterials = this.m_renderers[i].sharedMaterials;
			int num = sharedMaterials.Length;
			this.m_startingBrightness[i] = new float[num];
			for (int j = 0; j < num; j++)
			{
				Material material = sharedMaterials[j];
				if (this.CanHighlight(material))
				{
					this.m_startingBrightness[i][j] = material.GetFloat("_Brightness");
				}
			}
		}
	}

	// Token: 0x06001A34 RID: 6708 RVA: 0x00083460 File Offset: 0x00081860
	private void Highlight()
	{
		for (int i = 0; i < this.m_renderers.Count; i++)
		{
			if (this.m_renderers[i] == null)
			{
				this.RebuildHighlightMaterials();
				return;
			}
			foreach (Material material in this.m_renderers[i].materials)
			{
				if (this.CanHighlight(material))
				{
					material.SetFloat("_Brightness", this.m_highlight.m_brightnessModifier);
				}
			}
		}
	}

	// Token: 0x06001A35 RID: 6709 RVA: 0x000834F4 File Offset: 0x000818F4
	private void Unhighlight()
	{
		for (int i = 0; i < this.m_renderers.Count; i++)
		{
			if (!(this.m_renderers[i] == null))
			{
				Material[] materials = this.m_renderers[i].materials;
				for (int j = 0; j < materials.Length; j++)
				{
					Material material = materials[j];
					if (this.CanHighlight(material))
					{
						material.SetFloat("_Brightness", this.m_startingBrightness[i][j]);
					}
				}
			}
		}
	}

	// Token: 0x06001A36 RID: 6710 RVA: 0x00083584 File Offset: 0x00081984
	private void IncrementHighlight()
	{
		this.m_highlightCount++;
		if (this.m_highlightCount != 1)
		{
			return;
		}
		this.Highlight();
		for (int i = 0; i < this.m_children.Count; i++)
		{
			this.m_children[i].IncrementHighlight();
		}
	}

	// Token: 0x06001A37 RID: 6711 RVA: 0x000835E0 File Offset: 0x000819E0
	private void DecrementHighlight()
	{
		this.m_highlightCount--;
		if (this.m_highlightCount != 0)
		{
			return;
		}
		this.Unhighlight();
		for (int i = 0; i < this.m_children.Count; i++)
		{
			this.m_children[i].DecrementHighlight();
		}
	}

	// Token: 0x06001A38 RID: 6712 RVA: 0x0008363A File Offset: 0x00081A3A
	public void AddChild(ClientAnticipateInteractionHighlight child)
	{
		if (this.m_children.Contains(child))
		{
			return;
		}
		this.m_children.Add(child);
		if (this.IsHighlighted())
		{
			child.IncrementHighlight();
		}
	}

	// Token: 0x06001A39 RID: 6713 RVA: 0x0008366C File Offset: 0x00081A6C
	public void RemoveChild(ClientAnticipateInteractionHighlight removedChild)
	{
		for (int i = 0; i < this.m_children.Count; i++)
		{
			ClientAnticipateInteractionHighlight x = this.m_children[i];
			if (!(x != removedChild))
			{
				if (this.IsHighlighted())
				{
					removedChild.DecrementHighlight();
				}
				this.m_children.RemoveAt(i);
				return;
			}
		}
	}

	// Token: 0x06001A3A RID: 6714 RVA: 0x000836D1 File Offset: 0x00081AD1
	private bool CanHighlight(Material _material)
	{
		return _material != null && _material.HasProperty("_Brightness");
	}

	// Token: 0x06001A3B RID: 6715 RVA: 0x000836ED File Offset: 0x00081AED
	private bool IsHighlighted()
	{
		return this.m_highlightCount > 0;
	}

	// Token: 0x06001A3C RID: 6716 RVA: 0x000836F8 File Offset: 0x00081AF8
	private void OnItemAttachmentAdded(IClientAttachment attachment)
	{
		ClientAnticipateInteractionHighlight clientAnticipateInteractionHighlight = attachment.AccessGameObject().RequestComponent<ClientAnticipateInteractionHighlight>();
		if (clientAnticipateInteractionHighlight == null)
		{
			return;
		}
		this.AddChild(clientAnticipateInteractionHighlight);
	}

	// Token: 0x06001A3D RID: 6717 RVA: 0x00083728 File Offset: 0x00081B28
	private void OnItemAttachmentRemoved(IClientAttachment attachment)
	{
		ClientAnticipateInteractionHighlight clientAnticipateInteractionHighlight = attachment.AccessGameObject().RequestComponent<ClientAnticipateInteractionHighlight>();
		if (clientAnticipateInteractionHighlight == null)
		{
			return;
		}
		this.RemoveChild(clientAnticipateInteractionHighlight);
	}

	// Token: 0x06001A3E RID: 6718 RVA: 0x00083755 File Offset: 0x00081B55
	private void OnContentsChanged(AssembledDefinitionNode[] node)
	{
		if (base.isActiveAndEnabled)
		{
			base.StartCoroutine(this.DelayedContentsChanged());
		}
	}

	// Token: 0x06001A3F RID: 6719 RVA: 0x00083770 File Offset: 0x00081B70
	private IEnumerator DelayedContentsChanged()
	{
		yield return null;
		this.RebuildHighlightMaterials();
		yield break;
	}

	// Token: 0x06001A40 RID: 6720 RVA: 0x0008378B File Offset: 0x00081B8B
	public void OnInteractionAnticipationStart(InteractionType _type, GameObject _player)
	{
		this.IncrementHighlight();
	}

	// Token: 0x06001A41 RID: 6721 RVA: 0x00083793 File Offset: 0x00081B93
	public void OnInteractionAnticipationEnded(InteractionType _type, GameObject _player)
	{
		this.DecrementHighlight();
	}

	// Token: 0x06001A42 RID: 6722 RVA: 0x0008379C File Offset: 0x00081B9C
	private void OnPlateRemoved(GameObject _plate)
	{
		ClientAnticipateInteractionHighlight clientAnticipateInteractionHighlight = _plate.RequestComponent<ClientAnticipateInteractionHighlight>();
		if (clientAnticipateInteractionHighlight == null)
		{
			return;
		}
		this.RemoveChild(clientAnticipateInteractionHighlight);
	}

	// Token: 0x040014C9 RID: 5321
	private int m_highlightCount;

	// Token: 0x040014CA RID: 5322
	private List<ClientAnticipateInteractionHighlight> m_children = new List<ClientAnticipateInteractionHighlight>();

	// Token: 0x040014CB RID: 5323
	private List<Renderer> m_renderers = new List<Renderer>();

	// Token: 0x040014CC RID: 5324
	private float[][] m_startingBrightness;

	// Token: 0x040014CD RID: 5325
	private AnticipateInteractionHighlight m_highlight;
}
