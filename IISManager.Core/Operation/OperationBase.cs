using IISManager.Core.Configuration;
using System;

namespace IISManager.Core
{
    public abstract class OperationBase : IOperation
    {
        public static string RootPath = AppDomain.CurrentDomain.BaseDirectory.Substring(0, AppDomain.CurrentDomain.BaseDirectory.IndexOf("IISManager"));
        protected Publish Publish { get; }
        protected OperationBase() { }
        protected OperationBase(Publish publish) => Publish = publish;
        public abstract string Execute(Operation context);
    }
}
