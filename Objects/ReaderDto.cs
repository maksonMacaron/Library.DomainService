using System.Runtime.Serialization;

namespace Library.DomainService.Objects
{
    [DataContract]
    public class ReaderDto
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public string LibraryCardNumber { get; set; }

        [DataMember]
        public string FullName { get; set; }

        [DataMember]
        public string Phone { get; set; }

        [DataMember]
        public string Email { get; set; }

        [DataMember]
        public string Status { get; set; }
    }
}