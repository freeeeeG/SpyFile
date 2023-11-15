using System;
using UnityEngine;

namespace Characters
{
	// Token: 0x02000704 RID: 1796
	[ExecuteAlways]
	public class LookLeft : MonoBehaviour
	{
		// Token: 0x0600244F RID: 9295 RVA: 0x0006D5C4 File Offset: 0x0006B7C4
		private void Awake()
		{
			if (!Application.isPlaying)
			{
				return;
			}
			if (!base.isActiveAndEnabled)
			{
				return;
			}
			base.transform.localScale = Vector3.one;
			this._character = base.GetComponent<Character>();
			if (this._character == null)
			{
				base.transform.localScale = new Vector3(-1f, 1f, 0f);
			}
			else
			{
				this._character.lookingDirection = Character.LookingDirection.Left;
			}
			UnityEngine.Object.Destroy(this);
		}

		// Token: 0x04001EF4 RID: 7924
		private Character _character;
	}
}
