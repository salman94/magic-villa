using MagicVilla.Models.Dto;

namespace MagicVilla.Data;

public static class VillaStore
{
    public static List<VillaDto?> listVilla = new List<VillaDto?>()
    {
        new VillaDto() { Id = 1, Name = "Pool view", Occupancy = 4, Sqft = 1232},
        new VillaDto() { Id = 2, Name = "Beach view", Occupancy = 2, Sqft = 800}
    };
}