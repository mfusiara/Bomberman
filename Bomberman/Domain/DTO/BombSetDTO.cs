using System.Collections.Generic;

namespace Domain.DTO
{
    public class BombSetDTO : WorldElementDTO
    {
        public List<BombDTO> Bombs { get; set; }  

    }
}