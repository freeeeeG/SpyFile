using System;
using System.Collections.Generic;
using Team17.Online;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Token: 0x02000B0D RID: 2829
[RequireComponent(typeof(RectTransform))]
[RequireComponent(typeof(LayoutElement))]
public class SiblingBasedSize : UIBehaviour
{
	// Token: 0x0600393D RID: 14653 RVA: 0x0010FBEE File Offset: 0x0010DFEE
	protected override void Awake()
	{
		ClientUserSystem.usersChanged = (GenericVoid)Delegate.Combine(ClientUserSystem.usersChanged, new GenericVoid(this.OnUsersChanged));
		this.UpdateRectTransform();
	}

	// Token: 0x0600393E RID: 14654 RVA: 0x0010FC18 File Offset: 0x0010E018
	protected SiblingBasedSize.SizeOption GetSizeOption()
	{
		if (base.transform.parent != null)
		{
			int activeSiblings = 0;
			int childCount = base.transform.parent.childCount;
			for (int i = 0; i < childCount; i++)
			{
				Transform child = base.transform.parent.GetChild(i);
				if (child != base.transform && child.gameObject.activeInHierarchy)
				{
					activeSiblings++;
				}
			}
			return this.m_sizeOptions.Find((SiblingBasedSize.SizeOption x) => x.m_numSiblings == activeSiblings);
		}
		return null;
	}

	// Token: 0x0600393F RID: 14655 RVA: 0x0010FCC9 File Offset: 0x0010E0C9
	private void OnUsersChanged()
	{
		this.UpdateRectTransform();
	}

	// Token: 0x06003940 RID: 14656 RVA: 0x0010FCD4 File Offset: 0x0010E0D4
	public void UpdateRectTransform()
	{
		SiblingBasedSize.SizeOption sizeOption = this.GetSizeOption();
		if (sizeOption != null)
		{
			RectTransform rectTransform = base.transform as RectTransform;
			rectTransform.sizeDelta = sizeOption.m_dimensions;
			LayoutRebuilder.MarkLayoutForRebuild(rectTransform);
		}
	}

	// Token: 0x06003941 RID: 14657 RVA: 0x0010FD0C File Offset: 0x0010E10C
	protected override void OnDestroy()
	{
		ClientUserSystem.usersChanged = (GenericVoid)Delegate.Remove(ClientUserSystem.usersChanged, new GenericVoid(this.OnUsersChanged));
	}

	// Token: 0x04002E0A RID: 11786
	private RectTransform m_transform;

	// Token: 0x04002E0B RID: 11787
	[SerializeField]
	private List<SiblingBasedSize.SizeOption> m_sizeOptions;

	// Token: 0x02000B0E RID: 2830
	[Serializable]
	public class SizeOption
	{
		// Token: 0x04002E0C RID: 11788
		[SerializeField]
		public int m_numSiblings;

		// Token: 0x04002E0D RID: 11789
		[SerializeField]
		public Vector2 m_dimensions;
	}
}
