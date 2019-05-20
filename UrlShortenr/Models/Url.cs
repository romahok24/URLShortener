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
        [Display(Name = "Длинный URL")]
        public string LongUrl { get; set; }

        [Required]
        [StringLength(20)]
        [Display(Name = "Короткий URL")]
        public string ShortUrl { get; set; }

        [Required]
        [Display(Name = "Дата добавления")]
        public DateTime Added { get; set; }

        [Required]
        [StringLength(50)]
        public string Ip { get; set; }

        [Required]
        public int ClicksCount { get; set; }

        public List<Statistic> Statistics { get; set; }
    }
}
