using System;
using UnityEngine;

// Token: 0x02000042 RID: 66
public class PostProcess : MonoBehaviour
{
	// Token: 0x0600029F RID: 671 RVA: 0x0000F8EF File Offset: 0x0000DAEF
	private void Awake()
	{
		PostProcess.inst = this;
		Object.DontDestroyOnLoad(base.gameObject);
	}

	// Token: 0x060002A0 RID: 672 RVA: 0x0000F902 File Offset: 0x0000DB02
	private void OnEnable()
	{
		this.ApplySetting();
	}

	// Token: 0x060002A1 RID: 673 RVA: 0x0000F90A File Offset: 0x0000DB0A
	public void ApplySetting()
	{
		if (Setting.Inst.setInts[0] == 0)
		{
			this.objPPS_Bloom.gameObject.SetActive(false);
			return;
		}
		this.objPPS_Bloom.gameObject.SetActive(true);
	}

	// Token: 0x04000267 RID: 615
	public static PostProcess inst;

	// Token: 0x04000268 RID: 616
	[SerializeField]
	private GameObject objPPS_Bloom;
}
