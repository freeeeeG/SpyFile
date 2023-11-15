using System;
using UnityEngine;

// Token: 0x0200000B RID: 11
public class GeneralEventsListeners : MonoBehaviour
{
	// Token: 0x0600002B RID: 43 RVA: 0x00004B65 File Offset: 0x00002D65
	private void Awake()
	{
		LeanTween.LISTENERS_MAX = 100;
		LeanTween.EVENTS_MAX = 2;
		this.fromColor = base.GetComponent<Renderer>().material.color;
	}

	// Token: 0x0600002C RID: 44 RVA: 0x00004B8A File Offset: 0x00002D8A
	private void Start()
	{
		LeanTween.addListener(base.gameObject, 0, new Action<LTEvent>(this.changeColor));
		LeanTween.addListener(base.gameObject, 1, new Action<LTEvent>(this.jumpUp));
	}

	// Token: 0x0600002D RID: 45 RVA: 0x00004BBC File Offset: 0x00002DBC
	private void jumpUp(LTEvent e)
	{
		base.GetComponent<Rigidbody>().AddRelativeForce(Vector3.forward * 300f);
	}

	// Token: 0x0600002E RID: 46 RVA: 0x00004BD8 File Offset: 0x00002DD8
	private void changeColor(LTEvent e)
	{
		float num = Vector3.Distance(((Transform)e.data).position, base.transform.position);
		Color to = new Color(Random.Range(0f, 1f), 0f, Random.Range(0f, 1f));
		LeanTween.value(base.gameObject, this.fromColor, to, 0.8f).setLoopPingPong(1).setDelay(num * 0.05f).setOnUpdate(delegate(Color col)
		{
			base.GetComponent<Renderer>().material.color = col;
		});
	}

	// Token: 0x0600002F RID: 47 RVA: 0x00004C6B File Offset: 0x00002E6B
	private void OnCollisionEnter(Collision collision)
	{
		if (collision.gameObject.layer != 2)
		{
			this.towardsRotation = new Vector3(0f, (float)Random.Range(-180, 180), 0f);
		}
	}

	// Token: 0x06000030 RID: 48 RVA: 0x00004CA0 File Offset: 0x00002EA0
	private void OnCollisionStay(Collision collision)
	{
		if (collision.gameObject.layer != 2)
		{
			this.turnForIter = 0f;
			this.turnForLength = Random.Range(0.5f, 1.5f);
		}
	}

	// Token: 0x06000031 RID: 49 RVA: 0x00004CD0 File Offset: 0x00002ED0
	private void FixedUpdate()
	{
		if (this.turnForIter < this.turnForLength)
		{
			base.GetComponent<Rigidbody>().MoveRotation(base.GetComponent<Rigidbody>().rotation * Quaternion.Euler(this.towardsRotation * Time.deltaTime));
			this.turnForIter += Time.deltaTime;
		}
		base.GetComponent<Rigidbody>().AddRelativeForce(Vector3.forward * 4.5f);
	}

	// Token: 0x06000032 RID: 50 RVA: 0x00004D47 File Offset: 0x00002F47
	private void OnMouseDown()
	{
		if (Input.GetKey(KeyCode.J))
		{
			LeanTween.dispatchEvent(1);
			return;
		}
		LeanTween.dispatchEvent(0, base.transform);
	}

	// Token: 0x04000036 RID: 54
	private Vector3 towardsRotation;

	// Token: 0x04000037 RID: 55
	private float turnForLength = 0.5f;

	// Token: 0x04000038 RID: 56
	private float turnForIter;

	// Token: 0x04000039 RID: 57
	private Color fromColor;

	// Token: 0x02000277 RID: 631
	public enum MyEvents
	{
		// Token: 0x040009DC RID: 2524
		CHANGE_COLOR,
		// Token: 0x040009DD RID: 2525
		JUMP,
		// Token: 0x040009DE RID: 2526
		LENGTH
	}
}
