using System;
using Team17.Online.Multiplayer.Messaging;
using UnityEngine;

// Token: 0x020007EC RID: 2028
public class ServerCatchableItem : ServerSynchroniserBase, ICatchable
{
	// Token: 0x060026FE RID: 9982 RVA: 0x000B8BC8 File Offset: 0x000B6FC8
	public override void StartSynchronising(Component synchronisedObject)
	{
		base.StartSynchronising(synchronisedObject);
		this.m_throwable = base.gameObject.GetComponent<IThrowable>();
	}

	// Token: 0x060026FF RID: 9983 RVA: 0x000B8BE4 File Offset: 0x000B6FE4
	public bool AllowCatch(IHandleCatch _catcher, Vector2 _directionXZ)
	{
		if (this.m_throwable == null)
		{
			return false;
		}
		GameObject gameObject = (_catcher as MonoBehaviour).gameObject;
		if (_catcher != null && gameObject == null)
		{
			return false;
		}
		if (!(gameObject.RequestComponent<ServerAttachStation>() != null) && !(gameObject.RequestComponent<ServerIngredientContainer>() != null))
		{
			if (!this.m_throwable.IsFlying() || (this.m_throwable.IsFlying() && this.m_throwable.GetFlightTime() < 0.1f))
			{
				return false;
			}
		}
		return true;
	}

	// Token: 0x06002700 RID: 9984 RVA: 0x000B8C7D File Offset: 0x000B707D
	public GameObject AccessGameObject()
	{
		return base.gameObject;
	}

	// Token: 0x04001ECE RID: 7886
	private const float c_minFlightTime = 0.1f;

	// Token: 0x04001ECF RID: 7887
	private IThrowable m_throwable;
}
