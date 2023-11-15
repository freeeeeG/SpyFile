using System;
using UnityEngine;

// Token: 0x020007F3 RID: 2035
public interface IClientAttachment
{
	// Token: 0x06002714 RID: 10004
	bool IsAttached();

	// Token: 0x06002715 RID: 10005
	GameObject AccessGameObject();

	// Token: 0x06002716 RID: 10006
	Rigidbody AccessRigidbody();

	// Token: 0x06002717 RID: 10007
	void RegisterAttachChangedCallback(AttachChangedCallback _callback);

	// Token: 0x06002718 RID: 10008
	void UnregisterAttachChangedCallback(AttachChangedCallback _callback);

	// Token: 0x06002719 RID: 10009
	IClientSidePredicted GetClientSidePrediction();

	// Token: 0x0600271A RID: 10010
	void SetClientSidePrediction(CreateClientSidePredictionCallback prediction);
}
