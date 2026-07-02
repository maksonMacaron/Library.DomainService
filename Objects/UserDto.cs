using System.Runtime.Serialization;

namespace Library.DomainService.Objects
{
    [DataContract]
    public class UserDto
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public string Login { get; set; }

        [DataMember]
        public string Password { get; set; }

        [DataMember]
        public string FullName { get; set; }

        [DataMember]
        public string Role { get; set; }
    }
}