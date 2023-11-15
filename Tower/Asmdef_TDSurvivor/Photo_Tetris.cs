using System;
using UnityEngine;
using UnityEngine.Playables;

// Token: 0x02000108 RID: 264
public class Photo_Tetris : MonoBehaviour
{
	// Token: 0x060006AF RID: 1711 RVA: 0x00018728 File Offset: 0x00016928
	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Space))
		{
			this.timeline.time = 0.0;
			this.timeline.Play();
		}
	}

	// Token: 0x04000590 RID: 1424
	[SerializeField]
	private PlayableDirector timeline;
}
