using System;
using UnityEngine;

// Token: 0x02000799 RID: 1945
public interface IEquipmentConfig
{
	// Token: 0x06003628 RID: 13864
	EquipmentDef CreateEquipmentDef();

	// Token: 0x06003629 RID: 13865
	void DoPostConfigure(GameObject go);

	// Token: 0x0600362A RID: 13866
	string[] GetDlcIds();
}
