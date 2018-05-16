using IISManager.Core.Configuration;

namespace IISManager.Core
{
    public interface IOperation
    {
        string Execute(Operation context);
    }
}
