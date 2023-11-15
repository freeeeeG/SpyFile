using System;
using UnityEngine;

// Token: 0x0200000C RID: 12
public class GeneralSequencer : MonoBehaviour
{
	// Token: 0x06000035 RID: 53 RVA: 0x00004D8C File Offset: 0x00002F8C
	public void Start()
	{
		LTSeq ltseq = LeanTween.sequence(true);
		ltseq.append(LeanTween.moveY(this.avatar1, this.avatar1.transform.localPosition.y + 6f, 1f).setEaseOutQuad());
		ltseq.insert(LeanTween.alpha(this.star, 0f, 1f));
		ltseq.insert(LeanTween.scale(this.star, Vector3.one * 3f, 1f));
		ltseq.append(LeanTween.rotateAround(this.avatar1, Vector3.forward, 360f, 0.6f).setEaseInBack());
		ltseq.append(LeanTween.moveY(this.avatar1, this.avatar1.transform.localPosition.y, 1f).setEaseInQuad());
		ltseq.append(delegate()
		{
			int num = 0;
			while ((float)num < 50f)
			{
				GameObject gameObject = Object.Instantiate<GameObject>(this.dustCloudPrefab);
				gameObject.transform.parent = this.avatar1.transform;
				gameObject.transform.localPosition = new Vector3(Random.Range(-2f, 2f), 0f, 0f);
				gameObject.transform.eulerAngles = new Vector3(0f, 0f, Random.Range(0f, 360f));
				Vector3 to = new Vector3(gameObject.transform.localPosition.x, Random.Range(2f, 4f), Random.Range(-10f, 10f));
				LeanTween.moveLocal(gameObject, to, 3f * this.speedScale).setEaseOutCirc();
				LeanTween.rotateAround(gameObject, Vector3.forward, 720f, 3f * this.speedScale).setEaseOutCirc();
				LeanTween.alpha(gameObject, 0f, 3f * this.speedScale).setEaseOutCirc().setDestroyOnComplete(true);
				num++;
			}
		});
		ltseq.setScale(this.speedScale);
	}

	// Token: 0x0400003A RID: 58
	public GameObject avatar1;

	// Token: 0x0400003B RID: 59
	public GameObject star;

	// Token: 0x0400003C RID: 60
	public GameObject dustCloudPrefab;

	// Token: 0x0400003D RID: 61
	public float speedScale = 1f;
}
