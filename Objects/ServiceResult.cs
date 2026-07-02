using System.Runtime.Serialization;

namespace Library.DomainService.Objects
{
    [DataContract]
    public class ServiceResult
    {
        [DataMember]
        public bool Success { get; set; }

        [DataMember]
        public string Message { get; set; }

        public static ServiceResult Ok(string message)
        {
            return new ServiceResult
            {
                Success = true,
                Message = message
            };
        }

        public static ServiceResult Error(string message)
        {
            return new ServiceResult
            {
                Success = false,
                Message = message
            };
        }
    }
}