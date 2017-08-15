using IISManager.Core.Helper;

namespace IISManager.Core
{
    public class IISController
    {
        public static IController GetController()
        {
            if (IISHelper.IsIIS7AndHigher())
            {
                return new IIS7Controller();
            }
            return new IIS6Controller();
        }
        public static IController GetController(bool IsIIS7AndHigher)
        {
            if (IsIIS7AndHigher)
            {
                return new IIS7Controller();
            }
            return new IIS6Controller();
        }
    }
}
