using System;
using UnityEngine;

// Token: 0x0200065C RID: 1628
[RequireComponent(typeof(HeldItemsMeshVisibility), typeof(HatMeshVisibility), typeof(ChefMeshReplacer))]
public class CloneTargetAppearance : MonoBehaviour
{
	// Token: 0x06001F05 RID: 7941 RVA: 0x00097734 File Offset: 0x00095B34
	private void Start()
	{
		this.CloneTarget();
	}

	// Token: 0x06001F06 RID: 7942 RVA: 0x0009773C File Offset: 0x00095B3C
	private void CloneTarget()
	{
		if (this.m_target == null)
		{
			UnityEngine.Object.Destroy(base.gameObject);
			return;
		}
		int childCount = base.transform.childCount;
		for (int i = childCount - 1; i >= 0; i--)
		{
			UnityEngine.Object.Destroy(base.transform.GetChild(i).gameObject);
		}
		GameSession.SelectedChefData chefData = this.m_target.GetComponent<ChefMeshReplacer>().GetChefData();
		base.gameObject.GetComponent<ChefMeshReplacer>().SetChefData(chefData, false);
		ClientHeldItemsMeshVisibility component = base.gameObject.GetComponent<ClientHeldItemsMeshVisibility>();
		component.ForceSetup();
		ClientHatMeshVisibility component2 = base.gameObject.GetComponent<ClientHatMeshVisibility>();
		component2.ForceSetup();
	}

	// Token: 0x040017B7 RID: 6071
	[SerializeField]
	private GameObject m_target;
}
