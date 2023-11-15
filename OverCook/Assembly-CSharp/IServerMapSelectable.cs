using System;

// Token: 0x02000139 RID: 313
public interface IServerMapSelectable
{
	// Token: 0x060005AD RID: 1453
	void AvatarEnteringSelectable(MapAvatarControls _avatar);

	// Token: 0x060005AE RID: 1454
	void AvatarLeavingSelectable(MapAvatarControls _avatar);

	// Token: 0x060005AF RID: 1455
	void OnSelected(MapAvatarControls _avatar);
}
