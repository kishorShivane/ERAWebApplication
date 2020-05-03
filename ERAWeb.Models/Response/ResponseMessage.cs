using System;
using System.Collections.Generic;
using System.Text;

namespace ERAWeb.Models
{
    public class ResponseMessage<T>
    {
        public string Message { get; set; }
        public T Content { get; set; }
    }

}
