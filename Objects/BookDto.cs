using System.Runtime.Serialization;

namespace Library.DomainService.Objects
{
    [DataContract]
    public class BookDto
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public string Title { get; set; }

        [DataMember]
        public string Author { get; set; }

        [DataMember]
        public string Isbn { get; set; }

        [DataMember]
        public int PublicationYear { get; set; }

        [DataMember]
        public string InventoryNumber { get; set; }

        [DataMember]
        public string Status { get; set; }
    }
}