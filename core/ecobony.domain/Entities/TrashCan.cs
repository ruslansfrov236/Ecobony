using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ecobony.domain.Entities
{
    public class TrashCan:BaseEntity
    {
       public AppUser User { get; set; }
        public string UserName { get; set; }
       
        public string? EntityName { get; set; }   // Məs: "Product", "Category"
        public Guid EntityId { get; set; }        // Hansı obyektin ID-si
   
        public DateTime DeletedAt { get; set; }
    }
}
