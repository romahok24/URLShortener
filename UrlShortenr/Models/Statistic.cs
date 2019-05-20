using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UrlShortenr.Models
{
    [Table("Stats")]
    public class Statistic
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public DateTime ClickDate { get; set; }

        [Required]
        [StringLength(50)]
        public string Ip { get; set; }

        [Required]
        [StringLength(500)]
        public string Referer { get; set; }

        public Url Url { get; set; }
    }
}
