using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AgeCheckConcept.Models
{
    /// <summary>
    /// Represents an entry in an access log
    /// </summary>
    public class AccessLog
    {
        [Key]
        public long AccessLogId { get; set; }

        [Column(TypeName = "datetime2")]
        [Required]
        [DisplayName("Submitted date-time")]
        public DateTime SubmittedDateTime { get; set; }

        [Column(TypeName = "nvarchar(20)")]
        [Required]
        [DisplayName("User name")]
        public string UserName { get; set; }

        [Column(TypeName = "nvarchar(50)")]
        [Required]
        [DisplayName("Email address")]
        public string EmailAddress { get; set; }

        [Column(TypeName = "date")]
        [Required]
        [DisplayName("Date of birth")]
        public DateTime DOB { get; set; }

        [Column(TypeName = "bit")]
        [Required]
        [DisplayName("Successful")]
        public bool IsSuccess { get; set; }

        [Column(TypeName = "bit")]
        [Required]
        [DisplayName("Lock status")]
        public bool IsLockedOut { get; set; }
    }
}
