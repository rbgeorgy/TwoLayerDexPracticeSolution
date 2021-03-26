using System;
using System.Data;
using System.Reflection;

namespace TwoLayerSolution
{
    public class ParametersAndType : IEquatable<ParametersAndType>
    {
        public ParameterInfo[] Parameters;
        public Type ReturnType;

        public ParametersAndType(ParameterInfo[] parameterInfos, Type returnType)
        {
            if (parameterInfos != null)
            {
                Parameters = new ParameterInfo[parameterInfos.Length];
                for (int i = 0; i < parameterInfos.Length; i++)
                {
                    Parameters[i] = parameterInfos[i];
                }
            }
            else throw new ArgumentNullException();
            this.ReturnType = returnType ?? throw new ArgumentNullException(nameof(returnType));
        }

        public void Clear()
        {
            Parameters = null;
            ReturnType = null;
        }

        public bool Equals(ParametersAndType other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Equals(Parameters, other.Parameters) && Equals(ReturnType, other.ReturnType);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((ParametersAndType) obj);
        }
    }
}