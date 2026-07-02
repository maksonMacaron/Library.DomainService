using System;
using System.Runtime.Serialization;

namespace Library.DomainService.Objects
{
    [DataContract]
    public class LoanDto
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public int ReaderId { get; set; }

        [DataMember]
        public int BookId { get; set; }

        [DataMember]
        public DateTime IssueDate { get; set; }

        [DataMember]
        public DateTime DueDate { get; set; }

        [DataMember]
        public DateTime? ReturnDate { get; set; }

        [DataMember]
        public decimal FineAmount { get; set; }

        [DataMember]
        public string Status { get; set; }
    }
}