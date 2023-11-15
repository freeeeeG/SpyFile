using System;
using UnityEngine;

// Token: 0x02000BD0 RID: 3024
[AddComponentMenu("KMonoBehaviour/scripts/ReportScreenHeader")]
public class ReportScreenHeader : KMonoBehaviour
{
	// Token: 0x06005F12 RID: 24338 RVA: 0x0022E87A File Offset: 0x0022CA7A
	public void SetMainEntry(ReportManager.ReportGroup reportGroup)
	{
		if (this.mainRow == null)
		{
			this.mainRow = Util.KInstantiateUI(this.rowTemplate.gameObject, base.gameObject, true).GetComponent<ReportScreenHeaderRow>();
		}
		this.mainRow.SetLine(reportGroup);
	}

	// Token: 0x04004048 RID: 16456
	[SerializeField]
	private ReportScreenHeaderRow rowTemplate;

	// Token: 0x04004049 RID: 16457
	private ReportScreenHeaderRow mainRow;
}
