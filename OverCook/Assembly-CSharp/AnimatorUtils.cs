using System;
using UnityEngine;

// Token: 0x02000100 RID: 256
public static class AnimatorUtils
{
	// Token: 0x060004D1 RID: 1233 RVA: 0x00028694 File Offset: 0x00026A94
	public static bool HasParameter(this Animator _animator, int _paramHash)
	{
		AnimatorControllerParameter[] parameters = _animator.parameters;
		for (int i = 0; i < parameters.Length; i++)
		{
			if (parameters[i].nameHash == _paramHash)
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x060004D2 RID: 1234 RVA: 0x000286D0 File Offset: 0x00026AD0
	public static bool HasParameter(this Animator _animator, string _paramName)
	{
		AnimatorControllerParameter[] parameters = _animator.parameters;
		for (int i = 0; i < parameters.Length; i++)
		{
			if (parameters[i].name == _paramName)
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x060004D3 RID: 1235 RVA: 0x0002870E File Offset: 0x00026B0E
	public static bool IsActive(this Animator _animator)
	{
		return !(_animator == null) && _animator.gameObject.activeInHierarchy && !(_animator.runtimeAnimatorController == null);
	}

	// Token: 0x060004D4 RID: 1236 RVA: 0x00028744 File Offset: 0x00026B44
	public static object GetValue(Animator _animator, string _animatorVariableName, AnimatorVariableType _valueType)
	{
		switch (_valueType)
		{
		case AnimatorVariableType.Bool:
			return _animator.GetBool(_animatorVariableName);
		case AnimatorVariableType.Int:
			return _animator.GetInteger(_animatorVariableName);
		case AnimatorVariableType.Float:
			return _animator.GetFloat(_animatorVariableName);
		case AnimatorVariableType.Trigger:
			return false;
		default:
			return null;
		}
	}

	// Token: 0x060004D5 RID: 1237 RVA: 0x0002879C File Offset: 0x00026B9C
	public static object GetValue(Animator _animator, int _animatorVariableNameHash, AnimatorVariableType _valueType)
	{
		switch (_valueType)
		{
		case AnimatorVariableType.Bool:
			return _animator.GetBool(_animatorVariableNameHash);
		case AnimatorVariableType.Int:
			return _animator.GetInteger(_animatorVariableNameHash);
		case AnimatorVariableType.Float:
			return _animator.GetFloat(_animatorVariableNameHash);
		case AnimatorVariableType.Trigger:
			return false;
		default:
			return null;
		}
	}

	// Token: 0x060004D6 RID: 1238 RVA: 0x000287F4 File Offset: 0x00026BF4
	public static object SetValue(Animator _animator, string _animatorVariableName, AnimatorVariableType _valueType, object _value)
	{
		switch (_valueType)
		{
		case AnimatorVariableType.Bool:
			_animator.SetBool(_animatorVariableName, (bool)_value);
			break;
		case AnimatorVariableType.Int:
			_animator.SetInteger(_animatorVariableName, (int)_value);
			break;
		case AnimatorVariableType.Float:
			_animator.SetFloat(_animatorVariableName, (float)_value);
			break;
		case AnimatorVariableType.Trigger:
			if ((bool)_value)
			{
				_animator.SetTrigger(_animatorVariableName);
			}
			else
			{
				_animator.ResetTrigger(_animatorVariableName);
			}
			break;
		}
		return null;
	}

	// Token: 0x060004D7 RID: 1239 RVA: 0x00028878 File Offset: 0x00026C78
	public static object SetValue(Animator _animator, int _animatorVariableNameHash, AnimatorVariableType _valueType, object _value)
	{
		switch (_valueType)
		{
		case AnimatorVariableType.Bool:
			_animator.SetBool(_animatorVariableNameHash, (bool)_value);
			break;
		case AnimatorVariableType.Int:
			_animator.SetInteger(_animatorVariableNameHash, (int)_value);
			break;
		case AnimatorVariableType.Float:
			_animator.SetFloat(_animatorVariableNameHash, (float)_value);
			break;
		case AnimatorVariableType.Trigger:
			if ((bool)_value)
			{
				_animator.SetTrigger(_animatorVariableNameHash);
			}
			else
			{
				_animator.ResetTrigger(_animatorVariableNameHash);
			}
			break;
		}
		return null;
	}
}
