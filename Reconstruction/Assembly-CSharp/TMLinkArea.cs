using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

// Token: 0x02000297 RID: 663
public class TMLinkArea : MonoBehaviour, IPointerClickHandler, IEventSystemHandler
{
	// Token: 0x06001043 RID: 4163 RVA: 0x0002BE64 File Offset: 0x0002A064
	private void Start()
	{
		this.m_Event.AddListener(delegate()
		{
			Singleton<GuideGirlSystem>.Instance.ShowGuideBook(8);
		});
	}

	// Token: 0x06001044 RID: 4164 RVA: 0x0002BE90 File Offset: 0x0002A090
	public void OnPointerClick(PointerEventData eventData)
	{
		if (this.m_Ecom.AreaID != 0)
		{
			return;
		}
		Vector3 position = new Vector3(eventData.position.x, eventData.position.y, 0f);
		if (TMP_TextUtilities.FindIntersectingLink(this.TxtArea, position, Camera.main) > -1)
		{
			this.m_Event.Invoke();
		}
	}

	// Token: 0x04000889 RID: 2185
	[SerializeField]
	private TextMeshProUGUI TxtArea;

	// Token: 0x0400088A RID: 2186
	[SerializeField]
	private UnityEvent m_Event;

	// Token: 0x0400088B RID: 2187
	[SerializeField]
	private TipsElementConstruct m_Ecom;
}
