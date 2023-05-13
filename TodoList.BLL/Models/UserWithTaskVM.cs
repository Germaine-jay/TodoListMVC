using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TodoList.BLL.Models
{
    public class UserWithTaskVM
    {
        public int Id { get; set; }
        public string Fullname { get; set; }
        public IEnumerable<TaskVM> Tasks { get; set; }

    }

}
