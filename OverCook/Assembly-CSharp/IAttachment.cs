using System;
using UnityEngine;

// Token: 0x020007F2 RID: 2034
public interface IAttachment
{
	// Token: 0x0600270C RID: 9996
	void Attach(IParentable _parentable);

	// Token: 0x0600270D RID: 9997
	void Detach();

	// Token: 0x0600270E RID: 9998
	bool IsAttached();

	// Token: 0x0600270F RID: 9999
	GameObject AccessGameObject();

	// Token: 0x06002710 RID: 10000
	Rigidbody AccessRigidbody();

	// Token: 0x06002711 RID: 10001
	RigidbodyMotion AccessMotion();

	// Token: 0x06002712 RID: 10002
	void RegisterAttachChangedCallback(AttachChangedCallback _callback);

	// Token: 0x06002713 RID: 10003
	void UnregisterAttachChangedCallback(AttachChangedCallback _callback);
}
