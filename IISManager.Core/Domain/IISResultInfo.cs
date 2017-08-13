using System;

namespace IISManager.Core.Domain
{
    public class IISResultInfo
    {
        public int ErrorCode { get; set; }

        public string Message { get; set; }

        public bool Success => ErrorCode == 0;

        public IISResultInfo SetError(string msg, int errorCode = 1)
        {
            Message = msg;
            ErrorCode = errorCode;
            return this;
        }
        public IISResultInfo SetError(Exception exception, int errorCode = 1)
        {
            Message = exception.Message;
            ErrorCode = errorCode;
            return this;
        }

        public object Data { get; set; }
    }

    public class IISResultInfo<T> : IISResultInfo
    {
        public new T Data { get; set; }
    }
}
