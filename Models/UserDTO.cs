using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OpahQueue.Models
{
    public class UserDTO
    {
        public long Id { get; set; }
        public string? Name { get; set; }
        public bool WorkStatus { get; set; }

        public int Position {get; set;}

        // public TodoItemDTO(TodoItem tudoItem)
        // {
        //     this.Id = tudoItem.Id;
        //     this.Name = tudoItem.Name;
        //     this.WorkStatus = tudoItem.WorkStatus;            
        // }
    }
}