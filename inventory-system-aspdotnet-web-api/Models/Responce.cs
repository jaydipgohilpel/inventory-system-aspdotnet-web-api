using System.Collections.Generic;
using System.Net;
using System.Runtime.Serialization;

namespace inventory_system_aspdotnet_web_api.Models
{
    [DataContract(Name = "ResponseHelper")]
    public class ResponseHelper
    {
        public string? Message { get; set; } = " ";

        public Boolean? Ok { get; set; } = false;

        public string? Headers { get; set; } = "HttpResponce";
        public Boolean? Success { get; set; } = Constants.success;


    }

    public class ObjectResponseHelper : GetDataResponseHelper
    {
        public object Data { get; set; } // Property for additional object
    }

    public class LoginResponseHelper : GetDataResponseHelper
    {
        public object token { get; set; } // Property for additional object
    }
    public class GetDataResponseHelper
    {
        public string? Message { get; set; } = "Record Featched successfully";

        public System.Collections.IList? Data { get; set; }

        public Boolean? Ok { get; set; } = true;
        public Boolean? Success { get; set; } = Constants.success;

    }

}
