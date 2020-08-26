using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Countdown.Models.Data
{
    public class CountDownTodoRepository: ITodoRepository
    {
        private CountDownDbContext context = new CountDownDbContext();

        public IEnumerable<Todo> Todoes
        {
            get
            {
                return context.Todoes;
            }
        }

        public Todo Create(Todo todo)
        {
            
            Todo item = context.Todoes.Add(todo);
            context.SaveChanges();
            return item;
        }

        public Todo MarkComplete(int id)
        {
            Todo item = context.Todoes.Where(x => x.Id == id).FirstOrDefault();
            if(item != null)
            {
                item.DueDate = DateTime.Now;
                context.SaveChanges();
            }
            return item;
        }
    }
}