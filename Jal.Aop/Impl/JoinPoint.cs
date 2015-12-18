using System;
using System.Reflection;
using Jal.Aop.Interface;

namespace Jal.Aop.Impl
{
    public class JoinPoint : IJoinPoint
    {
        private object _returnValue;

        public object[] Arguments { get; set; }

        public MethodInfo MethodInfo { get; set; }

        public object ReturnValue
        {
            get
            {
                return _returnValue; 
                
            }
            set
            {
                _returnValue = value;

                if (SetReturnValueToProxy!=null)
                    SetReturnValueToProxy(value);
            }
        }

        public object TargetObject { get; set; }

        public Type TargetType { get; set; }

        public Func<object> ExecuteMethodFromProxy { get; set; }

        public Action<object> SetReturnValueToProxy { get; set; }

        public void Proceed()
        {
            if (ExecuteMethodFromProxy!=null)
                ReturnValue = ExecuteMethodFromProxy();
        }
    }
}
