using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace role_management_user.Dtos
{
    public class MenuReadDto
    {
        public int id {get; set;}
        public string name {get; set;}
        [NotMapped]
        public IEnumerable<SubMenuReadDto> subMenus {get; set;}
    }
}