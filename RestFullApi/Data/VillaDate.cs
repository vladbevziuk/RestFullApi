using RestFullApi.Models.dto;

namespace RestFullApi.Data
{
    public static class VillaDate
    {
        public static List<VillaDto> villaList = new List<VillaDto>
        {
            new VillaDto{Id = 1, Name="Vlad", Sqft=100, Occupancy=4}
        };
    }
}
