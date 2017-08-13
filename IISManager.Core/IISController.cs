using IISManager.Core.Helper;

namespace IISManager.Core
{
    public class IISController
    {
        public static IController GetControl()
        {
            if (IISHelper.IsIIS7AndHigher())
            {
                return new IIS7Controller();
            }
            return new IIS6Controller();
        }
        public static IController GetControl(bool IsIIS7AndHigher)
        {
            if (IsIIS7AndHigher)
            {
                return new IIS7Controller();
            }
            return new IIS6Controller();
        }
    }
}
