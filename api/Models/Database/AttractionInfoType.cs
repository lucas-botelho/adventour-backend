﻿using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Adventour.Api.Models.Database
{
    [Table("Attraction_Info_Type")]
    public class AttractionInfoType
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("type_title")]
        public string TypeTitle { get; set; }
    }
}
