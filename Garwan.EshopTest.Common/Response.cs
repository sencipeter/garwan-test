using System;
using System.Collections.Generic;
using System.Text;

namespace Garwan.EshopTest.Common
{
    public class Response
    {
        public Exception Exception { get; set; }
        public bool HasError => Exception != null;
        public string ErrorMessage => Exception?.Message;
    }

    public class ResponseObject<T> : Response
    {
        public T Result { get; set; }

        public T GetResultOrThrowException()
        {
            if (Exception != null)
                throw Exception;
            return Result;
        }
    }

    public class ResponseList<T> : Response
    {
        public ResponseList()
        {
            Result = new List<T>();
        }
        public IList<T> Result { get; set; }
        public int TotalCount { get; set; }
        public int PageSize { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPage
        {
            get
            {
                if (PageSize == 0)
                    return 0;
                var totalPage = TotalCount / PageSize;
                if (TotalCount % PageSize > 0)
                    totalPage = totalPage + 1;
                return totalPage;
            }
        }

        public IList<T> GetResultOrThrowException()
        {
            if (Exception != null)
                throw Exception;

            return Result;
        }
    }
}
