using System;
using Services;
using Singletons;
using UnityEngine;

// Token: 0x0200002B RID: 43
public class FollowGameObejct : MonoBehaviour
{
	// Token: 0x060000A6 RID: 166 RVA: 0x00004440 File Offset: 0x00002640
	public void Start()
	{
		switch (this._followObejct)
		{
		case FollowGameObejct.Type.Camera:
			this._followGameObject = Camera.main.gameObject;
			return;
		case FollowGameObejct.Type.Player:
			this._followGameObject = Singleton<Service>.Instance.levelManager.player.gameObject;
			break;
		case FollowGameObejct.Type.CustomGameObject:
			break;
		default:
			return;
		}
	}

	// Token: 0x060000A7 RID: 167 RVA: 0x00004492 File Offset: 0x00002692
	public void Update()
	{
		base.transform.position = this._followGameObject.transform.position + this._offset;
	}

	// Token: 0x04000091 RID: 145
	[SerializeField]
	private FollowGameObejct.Type _followObejct;

	// Token: 0x04000092 RID: 146
	[SerializeField]
	private GameObject _followGameObject;

	// Token: 0x04000093 RID: 147
	[SerializeField]
	private Vector3 _offset;

	// Token: 0x0200002C RID: 44
	private enum Type
	{
		// Token: 0x04000095 RID: 149
		Camera,
		// Token: 0x04000096 RID: 150
		Player,
		// Token: 0x04000097 RID: 151
		CustomGameObject
	}
}
