using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200017C RID: 380
[AddComponentMenu("Scripts/Core/Components/TriggerCreateObject")]
public class TriggerCreateObject : MonoBehaviour
{
	// Token: 0x060006A1 RID: 1697 RVA: 0x0002D548 File Offset: 0x0002B948
	private void OnTrigger(string _trigger)
	{
		if (this.m_trigger == _trigger)
		{
			this.m_spawned.RemoveAll((GameObject obj) => obj == null);
			if (this.m_spawned.Count < this.m_maxNumber)
			{
				GameObject gameObject = this.m_objectToCreate.Instantiate(base.transform.position, base.transform.rotation);
				gameObject.name = this.m_objectToCreate.name;
				this.m_spawned.Add(gameObject);
			}
		}
	}

	// Token: 0x04000587 RID: 1415
	[SerializeField]
	private GameObject m_objectToCreate;

	// Token: 0x04000588 RID: 1416
	[SerializeField]
	private string m_trigger;

	// Token: 0x04000589 RID: 1417
	[SerializeField]
	private int m_maxNumber = 100;

	// Token: 0x0400058A RID: 1418
	private List<GameObject> m_spawned = new List<GameObject>();
}
