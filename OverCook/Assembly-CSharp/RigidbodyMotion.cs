using System;
using UnityEngine;

// Token: 0x0200015A RID: 346
[RequireComponent(typeof(Rigidbody))]
public class RigidbodyMotion : MonoBehaviour
{
	// Token: 0x0600061A RID: 1562 RVA: 0x0002C085 File Offset: 0x0002A485
	private void Awake()
	{
		this.m_rigidbody = base.gameObject.GetComponent<Rigidbody>();
	}

	// Token: 0x0600061B RID: 1563 RVA: 0x0002C098 File Offset: 0x0002A498
	private void OnDisable()
	{
		this.m_rigidbody.velocity = Vector3.zero;
	}

	// Token: 0x0600061C RID: 1564 RVA: 0x0002C0AA File Offset: 0x0002A4AA
	public void SetKinematic(bool _kinematic)
	{
		this.m_rigidbody.isKinematic = _kinematic;
	}

	// Token: 0x0600061D RID: 1565 RVA: 0x0002C0B8 File Offset: 0x0002A4B8
	public void SetVelocity(Vector3 _velocity)
	{
		this.m_rigidbody.velocity = _velocity;
	}

	// Token: 0x0600061E RID: 1566 RVA: 0x0002C0C6 File Offset: 0x0002A4C6
	public void AddVelocity(Vector3 _additionalVelocity)
	{
		this.m_rigidbody.velocity += _additionalVelocity;
	}

	// Token: 0x0600061F RID: 1567 RVA: 0x0002C0DF File Offset: 0x0002A4DF
	public void Accelerate(Vector3 _force)
	{
		this.m_rigidbody.AddForce(_force, ForceMode.Acceleration);
	}

	// Token: 0x06000620 RID: 1568 RVA: 0x0002C0EE File Offset: 0x0002A4EE
	public void Movement(Vector3 _movement)
	{
		this.m_rigidbody.MovePosition(this.m_rigidbody.position + _movement * TimeManager.GetDeltaTime(base.gameObject));
	}

	// Token: 0x06000621 RID: 1569 RVA: 0x0002C11C File Offset: 0x0002A51C
	public void Movement(Vector3 _movement, float _delta)
	{
		this.m_rigidbody.MovePosition(this.m_rigidbody.position + _movement * _delta);
	}

	// Token: 0x06000622 RID: 1570 RVA: 0x0002C140 File Offset: 0x0002A540
	public Vector3 GetVelocity()
	{
		return this.m_rigidbody.velocity;
	}

	// Token: 0x06000623 RID: 1571 RVA: 0x0002C150 File Offset: 0x0002A550
	public Vector3 GetVelocityXZ()
	{
		Vector3 velocity = this.GetVelocity();
		return velocity - Vector3.Dot(velocity, Vector3.up) * Vector3.up;
	}

	// Token: 0x06000624 RID: 1572 RVA: 0x0002C181 File Offset: 0x0002A581
	public void SetPosition(ref Vector3 position)
	{
		this.m_rigidbody.MovePosition(position);
	}

	// Token: 0x06000625 RID: 1573 RVA: 0x0002C194 File Offset: 0x0002A594
	public Vector3 GetPosition()
	{
		return this.m_rigidbody.position;
	}

	// Token: 0x06000626 RID: 1574 RVA: 0x0002C1A1 File Offset: 0x0002A5A1
	public void SetRotation(ref Quaternion rotation)
	{
		this.m_rigidbody.rotation = rotation;
	}

	// Token: 0x06000627 RID: 1575 RVA: 0x0002C1B4 File Offset: 0x0002A5B4
	public Quaternion GetRotation()
	{
		return this.m_rigidbody.rotation;
	}

	// Token: 0x0400051D RID: 1309
	private Rigidbody m_rigidbody;
}
