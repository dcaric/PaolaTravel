﻿
/**
 *This is trip model and it corresponds with the Trip table
 */
namespace Travel.API.Models
{
    public class Trip
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
        public decimal Price { get; set; }
        public int DestinationId { get; set; }
    }
}
