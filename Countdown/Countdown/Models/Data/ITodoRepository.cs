using System.Collections.Generic;

namespace Countdown.Models.Data
{
    public interface ITodoRepository
    {
        IEnumerable<Todo> Todoes { get; }

        Todo Create(Todo todo);
    }
}