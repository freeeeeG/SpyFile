using System;
using Team17.Online.Multiplayer.Messaging;
using UnityEngine;

// Token: 0x02000A10 RID: 2576
public static class PlayerControlsHelper
{
	// Token: 0x060032AF RID: 12975 RVA: 0x000ED24C File Offset: 0x000EB64C
	public static Vector3 GetControlAxis(PlayerControls _controls, ref PlayerControlsHelper.ControlAxisData controlAxisData)
	{
		PlayerControlsHelper.BuildControlAxisData(_controls, ref controlAxisData);
		return PlayerControlsHelper.GetControlAxis(ref controlAxisData);
	}

	// Token: 0x060032B0 RID: 12976 RVA: 0x000ED25B File Offset: 0x000EB65B
	public static Vector3 TurnTowardsControlAxis(ref PlayerControlsHelper.ControlAxisData controlData, PlayerControls _controls, GameObject _gameObject, float _deltaTime)
	{
		PlayerControlsHelper.BuildControlAxisData(_controls, ref controlData);
		return PlayerControlsHelper.TurnTowardsControlAxis(ref controlData, _gameObject, _deltaTime);
	}

	// Token: 0x060032B1 RID: 12977 RVA: 0x000ED26C File Offset: 0x000EB66C
	public static void BuildControlAxisData(PlayerControls _controls, ref PlayerControlsHelper.ControlAxisData controlAxisData)
	{
		controlAxisData.XAxisAllignment = _controls.Movement.XAxisAllignment;
		controlAxisData.YAxisAllignment = _controls.Movement.YAxisAllignment;
		controlAxisData.MoveX = _controls.ControlScheme.m_moveX;
		controlAxisData.MoveY = _controls.ControlScheme.m_moveY;
		controlAxisData.TurnSpeed = _controls.Movement.TurnSpeed;
		controlAxisData.QuantiseDirection = false;
	}

	// Token: 0x060032B2 RID: 12978 RVA: 0x000ED2D8 File Offset: 0x000EB6D8
	public static Vector3 GetControlAxis(ref PlayerControlsHelper.ControlAxisData _axisData)
	{
		float num = (_axisData.XAxisAllignment != PlayerControls.InversionType.Normal) ? -1f : 1f;
		float num2 = (_axisData.YAxisAllignment != PlayerControls.InversionType.Normal) ? -1f : 1f;
		float x = num * _axisData.MoveX.GetValue();
		float z = num2 * -_axisData.MoveY.GetValue();
		Vector3 vector = new Vector3(x, 0f, z);
		return vector.normalized;
	}

	// Token: 0x060032B3 RID: 12979 RVA: 0x000ED350 File Offset: 0x000EB750
	public static Vector3 GetControlAxis(float xAxis, float yAxis, PlayerControls.InversionType xAxisAllignment, PlayerControls.InversionType yAxisAllignment)
	{
		float num = (xAxisAllignment != PlayerControls.InversionType.Normal) ? -1f : 1f;
		float num2 = (yAxisAllignment != PlayerControls.InversionType.Normal) ? -1f : 1f;
		float x = num * xAxis;
		float z = num2 * -yAxis;
		Vector3 vector = new Vector3(x, 0f, z);
		return vector.normalized;
	}

	// Token: 0x060032B4 RID: 12980 RVA: 0x000ED3A8 File Offset: 0x000EB7A8
	public static Vector3 TurnTowardsControlAxis(float xAxis, float yAxis, PlayerControls.InversionType xAxisAllignment, PlayerControls.InversionType yAxisAllignment, float _turnSpeed, GameObject _gameObject, float _deltaTime)
	{
		Vector3 controlAxis = PlayerControlsHelper.GetControlAxis(xAxis, yAxis, xAxisAllignment, yAxisAllignment);
		if (controlAxis != Vector3.zero)
		{
			Vector3 vector = _gameObject.transform.forward;
			vector = Vector3.RotateTowards(vector, controlAxis, _turnSpeed * _deltaTime, 1000f).WithY(0f);
			_gameObject.transform.LookAt(_gameObject.transform.position + vector, Vector3.up);
			return controlAxis;
		}
		return Vector3.zero;
	}

	// Token: 0x060032B5 RID: 12981 RVA: 0x000ED424 File Offset: 0x000EB824
	public static Vector3 TurnTowardsControlAxis(ref PlayerControlsHelper.ControlAxisData _axisData, GameObject _gameObject, float _deltaTime)
	{
		Vector3 controlAxis = PlayerControlsHelper.GetControlAxis(ref _axisData);
		if (controlAxis != Vector3.zero)
		{
			Vector3 vector = _gameObject.transform.forward;
			float turnSpeed = _axisData.TurnSpeed;
			vector = Vector3.RotateTowards(vector, controlAxis, turnSpeed * _deltaTime, 1000f).WithY(0f);
			_gameObject.transform.LookAt(_gameObject.transform.position + vector, Vector3.up);
			return controlAxis;
		}
		return Vector3.zero;
	}

	// Token: 0x060032B6 RID: 12982 RVA: 0x000ED4A0 File Offset: 0x000EB8A0
	public static void TurnTowardsControlAxis(ref PlayerControlsHelper.ControlAxisData _axisData, ref Vector3 _direction, float _deltaTime)
	{
		Vector3 controlAxis = PlayerControlsHelper.GetControlAxis(ref _axisData);
		if (controlAxis != Vector3.zero)
		{
			float turnSpeed = _axisData.TurnSpeed;
			_direction = Vector3.RotateTowards(_direction, controlAxis, turnSpeed * _deltaTime, 1000f).WithY(0f);
		}
	}

	// Token: 0x060032B7 RID: 12983 RVA: 0x000ED4F0 File Offset: 0x000EB8F0
	public static void TurnTowardsDirection(GameObject _gameObject, Vector3 _direction, float _turnSpeed, float _deltaTime)
	{
		if (_direction != Vector3.zero)
		{
			Vector3 vector = _gameObject.transform.forward;
			vector = Vector3.RotateTowards(vector, _direction, _turnSpeed * _deltaTime, 1000f).WithY(0f);
			_gameObject.transform.LookAt(_gameObject.transform.position + vector, Vector3.up);
		}
	}

	// Token: 0x060032B8 RID: 12984 RVA: 0x000ED554 File Offset: 0x000EB954
	public static void DropHeldItem(PlayerControls _control, Vector2 _directionXZ)
	{
		ICarrier carrier = _control.gameObject.RequireInterface<ICarrier>();
		carrier.TakeItem();
	}

	// Token: 0x060032B9 RID: 12985 RVA: 0x000ED574 File Offset: 0x000EB974
	public static void PlaceHeldItem_Server(PlayerControls _control, GameObject _target)
	{
		ICarrier carrier = _control.gameObject.RequireInterface<ICarrier>();
		GameObject gameObject = carrier.InspectCarriedItem();
		IHandlePlacement handlePlacement = (!(_target != null)) ? null : PlayerControlsHelper.GetControllingPlacementHandler_Server(_target);
		if (handlePlacement != null)
		{
			Vector2 normalized = _control.transform.forward.XZ().normalized;
			if (handlePlacement.CanHandlePlacement(carrier, normalized, new PlacementContext(PlacementContext.Source.Player)))
			{
				handlePlacement.HandlePlacement(carrier, normalized, new PlacementContext(PlacementContext.Source.Player));
			}
			else
			{
				handlePlacement.OnFailedToPlace(carrier.InspectCarriedItem());
			}
		}
		else
		{
			carrier.TakeItem();
		}
	}

	// Token: 0x060032BA RID: 12986 RVA: 0x000ED60C File Offset: 0x000EBA0C
	public static void PlaceHeldItem_Client(PlayerControls _control)
	{
		ICarrier carrier = _control.gameObject.RequireInterface<ICarrier>();
		GameObject x = carrier.InspectCarriedItem();
		IClientHandlePlacement iHandlePlacement = _control.CurrentInteractionObjects.m_iHandlePlacement;
		if (iHandlePlacement != null)
		{
			ClientMessenger.ChefEventMessage(ChefEventMessage.ChefEventType.Place, _control.gameObject, iHandlePlacement as MonoBehaviour);
		}
		else if (x != null)
		{
			ClientMessenger.ChefEventMessage(ChefEventMessage.ChefEventType.Take, _control.gameObject, iHandlePlacement as MonoBehaviour);
		}
	}

	// Token: 0x060032BB RID: 12987 RVA: 0x000ED674 File Offset: 0x000EBA74
	public static IHandlePlacement GetControllingPlacementHandler_Server(GameObject _object)
	{
		ServerHandlePlacementReferral component = ComponentCache<ServerHandlePlacementReferral>.GetComponent(_object);
		if (component != null && component.enabled && component.GetHandlePlacementReferree() != null)
		{
			return component.GetHandlePlacementReferree();
		}
		IHandlePlacement[] components = ComponentCache<IHandlePlacement>.GetComponents(_object);
		return HandlePlacementUtils.GetHighestPriority<IHandlePlacement>(components);
	}

	// Token: 0x060032BC RID: 12988 RVA: 0x000ED6C0 File Offset: 0x000EBAC0
	public static IClientHandlePlacement GetControllingPlacementHandler_Client(GameObject _object)
	{
		ClientHandlePlacementReferral component = ComponentCache<ClientHandlePlacementReferral>.GetComponent(_object);
		if (component != null && component.enabled && component.GetHandlePlacementReferree() != null)
		{
			return component.GetHandlePlacementReferree();
		}
		IClientHandlePlacement[] components = ComponentCache<IClientHandlePlacement>.GetComponents(_object);
		IClientHandlePlacement clientHandlePlacement = null;
		foreach (IClientHandlePlacement clientHandlePlacement2 in components)
		{
			if ((clientHandlePlacement2 as MonoBehaviour).enabled && (clientHandlePlacement == null || clientHandlePlacement2.GetPlacementPriority() > clientHandlePlacement.GetPlacementPriority()))
			{
				clientHandlePlacement = clientHandlePlacement2;
			}
		}
		return clientHandlePlacement;
	}

	// Token: 0x060032BD RID: 12989 RVA: 0x000ED74C File Offset: 0x000EBB4C
	public static IClientHandlePickup GetControllingPickupHandler_Client(GameObject _object)
	{
		ClientHandlePickupReferral component = ComponentCache<ClientHandlePickupReferral>.GetComponent(_object);
		if (component != null && component.enabled && component.GetHandlePickupReferree() != null)
		{
			return component.GetHandlePickupReferree();
		}
		IClientHandlePickup[] components = ComponentCache<IClientHandlePickup>.GetComponents(_object);
		IClientHandlePickup clientHandlePickup = null;
		foreach (IClientHandlePickup clientHandlePickup2 in components)
		{
			if ((clientHandlePickup2 as MonoBehaviour).enabled && (clientHandlePickup == null || clientHandlePickup2.GetPickupPriority() > clientHandlePickup.GetPickupPriority()))
			{
				clientHandlePickup = clientHandlePickup2;
			}
		}
		return clientHandlePickup;
	}

	// Token: 0x060032BE RID: 12990 RVA: 0x000ED7D8 File Offset: 0x000EBBD8
	public static IHandlePickup GetControllingPickupHandler_Server(GameObject _object)
	{
		ServerHandlePickupReferral serverHandlePickupReferral = _object.RequestComponent<ServerHandlePickupReferral>();
		if (serverHandlePickupReferral != null && serverHandlePickupReferral.enabled && serverHandlePickupReferral.GetHandlePickupReferree() != null)
		{
			return serverHandlePickupReferral.GetHandlePickupReferree();
		}
		IHandlePickup[] array = _object.RequestInterfaces<IHandlePickup>();
		IHandlePickup handlePickup = null;
		foreach (IHandlePickup handlePickup2 in array)
		{
			if ((handlePickup2 as MonoBehaviour).enabled && (handlePickup == null || handlePickup2.GetPickupPriority() > handlePickup.GetPickupPriority()))
			{
				handlePickup = handlePickup2;
			}
		}
		return handlePickup;
	}

	// Token: 0x060032BF RID: 12991 RVA: 0x000ED864 File Offset: 0x000EBC64
	public static bool WouldFallIfHoldingNothing(PlayerControls _control)
	{
		if (_control.GroundCollider != null)
		{
			return false;
		}
		if (PlayerControlsHelper.s_fallingLayerMask == 0)
		{
			PlayerControlsHelper.s_fallingLayerMask = LayerMask.GetMask(new string[]
			{
				"Default",
				"Ground",
				"Walls",
				"Worktops"
			});
		}
		ICarrier carrier = _control.gameObject.RequireInterface<ICarrier>();
		IAttachment attachment = carrier.InspectCarriedItem().RequestInterface<IAttachment>();
		Collider collider = attachment.AccessGameObject().RequestComponent<Collider>();
		Bounds bounds = collider.bounds;
		Quaternion rotation = collider.transform.rotation;
		Vector3 position = collider.transform.position;
		int num = Physics.OverlapBoxNonAlloc(bounds.center, bounds.extents, PlayerControlsHelper.s_collisions, rotation, PlayerControlsHelper.s_fallingLayerMask, QueryTriggerInteraction.Ignore);
		for (int i = 0; i < num; i++)
		{
			Collider collider2 = PlayerControlsHelper.s_collisions[i];
			Transform transform = collider2.transform;
			Vector3 zero = Vector3.zero;
			float num2 = 0f;
			Vector3 b = (transform.position - position).normalized * 0.0001f;
			bool flag = Physics.ComputePenetration(collider, position + b, rotation, collider2, transform.position, transform.rotation, out zero, out num2);
			if (flag)
			{
				float num3 = Mathf.Clamp(Vector3.Dot(zero, Vector3.up), -1f, 1f);
				if (num3 > 0.5f)
				{
					return true;
				}
			}
		}
		return false;
	}

	// Token: 0x060032C0 RID: 12992 RVA: 0x000ED9DC File Offset: 0x000EBDDC
	public static bool IsHeldItemInsideStaticCollision(PlayerControls _control)
	{
		if (PlayerControlsHelper.s_staticCollisionLayerMask == 0)
		{
			PlayerControlsHelper.s_staticCollisionLayerMask = LayerMask.GetMask(new string[]
			{
				"Default",
				"Ground",
				"Walls",
				"Worktops",
				"PlateStationBlock",
				"CookingStationBlock"
			});
		}
		ICarrier carrier = _control.gameObject.RequireInterface<ICarrier>();
		IAttachment attachment = carrier.InspectCarriedItem().RequestInterface<IAttachment>();
		Collider collider = attachment.AccessGameObject().RequestComponent<Collider>();
		Bounds bounds = collider.bounds;
		Quaternion rotation = collider.transform.rotation;
		Vector3 position = collider.transform.position;
		Vector3 halfExtents = bounds.extents * 0.5f;
		int num = Physics.OverlapBoxNonAlloc(bounds.center, halfExtents, PlayerControlsHelper.s_collisions, rotation, PlayerControlsHelper.s_staticCollisionLayerMask, QueryTriggerInteraction.Ignore);
		return num > 0;
	}

	// Token: 0x040028D9 RID: 10457
	private const int c_maxCollisionHits = 8;

	// Token: 0x040028DA RID: 10458
	private static Collider[] s_collisions = new Collider[8];

	// Token: 0x040028DB RID: 10459
	private static int s_fallingLayerMask = 0;

	// Token: 0x040028DC RID: 10460
	private const float c_penetrationTestOffset = 0.0001f;

	// Token: 0x040028DD RID: 10461
	private static int s_staticCollisionLayerMask = 0;

	// Token: 0x040028DE RID: 10462
	private const float c_staticCollisionBoundsMultiplier = 0.5f;

	// Token: 0x02000A11 RID: 2577
	public struct ControlAxisData
	{
		// Token: 0x040028DF RID: 10463
		public PlayerControls.InversionType XAxisAllignment;

		// Token: 0x040028E0 RID: 10464
		public PlayerControls.InversionType YAxisAllignment;

		// Token: 0x040028E1 RID: 10465
		public ILogicalValue MoveX;

		// Token: 0x040028E2 RID: 10466
		public ILogicalValue MoveY;

		// Token: 0x040028E3 RID: 10467
		public float TurnSpeed;

		// Token: 0x040028E4 RID: 10468
		public bool QuantiseDirection;
	}
}
