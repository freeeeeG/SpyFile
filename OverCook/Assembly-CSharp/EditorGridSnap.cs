using System;
using UnityEngine;

// Token: 0x02000468 RID: 1128
[ExecuteInEditMode]
public class EditorGridSnap : MonoBehaviour
{
	// Token: 0x060014FD RID: 5373 RVA: 0x00072803 File Offset: 0x00070C03
	private void Awake()
	{
		if (Application.isPlaying)
		{
			base.enabled = false;
			return;
		}
	}

	// Token: 0x060014FE RID: 5374 RVA: 0x00072817 File Offset: 0x00070C17
	private void OnEnable()
	{
		this.m_gridManager = null;
	}

	// Token: 0x060014FF RID: 5375 RVA: 0x00072820 File Offset: 0x00070C20
	protected virtual void Update()
	{
		if (this.m_gridManager == null)
		{
			this.m_gridManager = GameUtils.GetGridManager(base.transform);
		}
		else
		{
			GridIndex unclampedGridLocationFromPos = this.m_gridManager.GetUnclampedGridLocationFromPos(base.transform.position);
			base.transform.position = VectorUtils.Select(this.m_gridManager.GetPosFromGridLocation(unclampedGridLocationFromPos), base.transform.position, this.m_constrainX, this.m_constrainY, this.m_constrainZ);
		}
	}

	// Token: 0x04001028 RID: 4136
	[SerializeField]
	private bool m_constrainX = true;

	// Token: 0x04001029 RID: 4137
	[SerializeField]
	private bool m_constrainY = true;

	// Token: 0x0400102A RID: 4138
	[SerializeField]
	private bool m_constrainZ = true;

	// Token: 0x0400102B RID: 4139
	protected GridManager m_gridManager;
}
