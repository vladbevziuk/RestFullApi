﻿using System.ComponentModel.DataAnnotations;

namespace RestFullApi.Models.dto
{
    public class VillaDto
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }
        public int Occupancy { get; set; }
        public int Sqft { get; set; }
    }
}
