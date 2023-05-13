using System.ComponentModel.DataAnnotations;


namespace TodoList.DAL.Entities
{
    public class User : BaseEntity
    {
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public IList<Todo>? TodoList { get; set; }
    }

}
