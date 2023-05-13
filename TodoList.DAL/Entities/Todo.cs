using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoList.DAL.Enums;

namespace TodoList.DAL.Entities
{
    public class Todo : BaseEntity
    {
        public string Title { get; set; }
        public string? Description { get; set; }
        public Priority Priority { get; set; } = Priority.Normal;
        public DateTime DueDate { get; set; }
        public bool IsDone { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
    }


}
