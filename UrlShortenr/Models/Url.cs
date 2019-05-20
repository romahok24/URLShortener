using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UrlShortenr.Models
{
    [Table("Urls")]
    public class Url
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(1000)]
        public string LongUrl { get; set; }

        [Required]
        [StringLength(20)]
        public string ShortUrl { get; set; }

        [Required]
        public DateTime Added { get; set; }

        [Required]
        [StringLength(50)]
        public string Ip { get; set; }

        [Required]
        public int ClicksCount { get; set; }

        public List<Statistic> Statistics { get; set; }
    }
}
