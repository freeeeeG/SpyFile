using System;
using Characters;
using Level;
using UnityEngine;

// Token: 0x02000013 RID: 19
public class CameraZoneChanger : MonoBehaviour
{
	// Token: 0x06000046 RID: 70 RVA: 0x00003480 File Offset: 0x00001680
	private void OnTriggerEnter2D(Collider2D collision)
	{
		Character component = collision.GetComponent<Character>();
		if (component == null)
		{
			return;
		}
		if (component.type != Character.Type.Player)
		{
			return;
		}
		Map.Instance.cameraZone = this._cameraZone;
		Map.Instance.SetCameraZoneOrDefault();
	}

	// Token: 0x0400004C RID: 76
	[SerializeField]
	private CameraZone _cameraZone;
}
