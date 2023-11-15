using System;
using UnityEngine;

// Token: 0x02000B6E RID: 2926
public class DividerColumn : TableColumn
{
	// Token: 0x06005AB5 RID: 23221 RVA: 0x00214F68 File Offset: 0x00213168
	public DividerColumn(Func<bool> revealed = null, string scrollerID = "") : base(delegate(IAssignableIdentity minion, GameObject widget_go)
	{
		if (revealed != null)
		{
			if (revealed())
			{
				if (!widget_go.activeSelf)
				{
					widget_go.SetActive(true);
					return;
				}
			}
			else if (widget_go.activeSelf)
			{
				widget_go.SetActive(false);
				return;
			}
		}
		else
		{
			widget_go.SetActive(true);
		}
	}, null, null, null, revealed, false, scrollerID)
	{
	}

	// Token: 0x06005AB6 RID: 23222 RVA: 0x00214F9F File Offset: 0x0021319F
	public override GameObject GetDefaultWidget(GameObject parent)
	{
		return Util.KInstantiateUI(Assets.UIPrefabs.TableScreenWidgets.Spacer, parent, true);
	}

	// Token: 0x06005AB7 RID: 23223 RVA: 0x00214FB7 File Offset: 0x002131B7
	public override GameObject GetMinionWidget(GameObject parent)
	{
		return Util.KInstantiateUI(Assets.UIPrefabs.TableScreenWidgets.Spacer, parent, true);
	}

	// Token: 0x06005AB8 RID: 23224 RVA: 0x00214FCF File Offset: 0x002131CF
	public override GameObject GetHeaderWidget(GameObject parent)
	{
		return Util.KInstantiateUI(Assets.UIPrefabs.TableScreenWidgets.Spacer, parent, true);
	}
}
