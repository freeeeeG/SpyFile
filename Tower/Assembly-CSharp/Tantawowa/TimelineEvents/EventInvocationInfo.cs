using System;
using System.Linq;
using System.Reflection;
using Tantawowa.Extensions;
using UnityEngine;

namespace Tantawowa.TimelineEvents
{
	// Token: 0x02000072 RID: 114
	public class EventInvocationInfo
	{
		// Token: 0x060001A8 RID: 424 RVA: 0x00007800 File Offset: 0x00005A00
		public EventInvocationInfo(string key, Behaviour targetBehaviour, MethodInfo methodInfo)
		{
			this.Key = key;
			this.MethodInfo = methodInfo;
			this.TargetBehaviour = targetBehaviour;
		}

		// Token: 0x060001A9 RID: 425 RVA: 0x0000781D File Offset: 0x00005A1D
		public void Invoke(object value)
		{
			if (this.MethodInfo != null)
			{
				this.MethodInfo.Invoke(this.TargetBehaviour, new object[]
				{
					value
				});
			}
		}

		// Token: 0x060001AA RID: 426 RVA: 0x0000784C File Offset: 0x00005A4C
		public void InvokEnum(int value)
		{
			object obj = Enum.ToObject(this.MethodInfo.GetParameters()[0].ParameterType, value);
			if (this.MethodInfo != null)
			{
				this.MethodInfo.Invoke(this.TargetBehaviour, new object[]
				{
					obj
				});
			}
		}

		// Token: 0x060001AB RID: 427 RVA: 0x0000789C File Offset: 0x00005A9C
		public void InvokeNoArgs()
		{
			if (this.MethodInfo != null)
			{
				this.MethodInfo.Invoke(this.TargetBehaviour, null);
			}
		}

		// Token: 0x060001AC RID: 428 RVA: 0x000078C0 File Offset: 0x00005AC0
		public void Invoke(bool isSingleArg, string value)
		{
			try
			{
				if (isSingleArg)
				{
					Type parameterType = this.MethodInfo.GetParameters()[0].ParameterType;
					if (parameterType.IsEnum)
					{
						this.Invoke(value.ConvertToType<int>());
					}
					else if (EventInvocationInfo.SupportedTypes.Contains(parameterType))
					{
						if (parameterType == typeof(string))
						{
							this.Invoke(value);
						}
						else if (parameterType == typeof(int))
						{
							this.Invoke(value.ConvertToType<int>());
						}
						else if (parameterType == typeof(float))
						{
							this.Invoke(value.ConvertToType<float>());
						}
						else if (parameterType == typeof(bool))
						{
							this.Invoke(value.ConvertToType<bool>());
						}
						else
						{
							Debug.Log(string.Concat(new string[]
							{
								"Could not parse argument value ",
								value,
								" for method ",
								this.MethodInfo.Name,
								". Ignoring"
							}));
						}
					}
					else
					{
						Debug.Log(string.Concat(new string[]
						{
							"Could not parse argument value ",
							value,
							" for method ",
							this.MethodInfo.Name,
							". Ignoring"
						}));
					}
				}
				else
				{
					this.InvokeNoArgs();
				}
			}
			catch (Exception ex)
			{
				string str = "Exception while executing timeline event ";
				string name = this.MethodInfo.Name;
				string str2 = " ";
				Exception ex2 = ex;
				Debug.Log(str + name + str2 + ((ex2 != null) ? ex2.ToString() : null));
			}
		}

		// Token: 0x04000196 RID: 406
		public Behaviour TargetBehaviour;

		// Token: 0x04000197 RID: 407
		public MethodInfo MethodInfo;

		// Token: 0x04000198 RID: 408
		public static Type[] SupportedTypes = new Type[]
		{
			typeof(string),
			typeof(float),
			typeof(int),
			typeof(bool)
		};

		// Token: 0x04000199 RID: 409
		public string Key;
	}
}
