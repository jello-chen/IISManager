using IISManager.Core.Configuration;

namespace IISManager.Core
{
    public interface IOperation
    {
        bool Execute(Operation context);
    }
}
