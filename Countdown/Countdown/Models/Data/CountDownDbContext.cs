using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Countdown.Models.Data
{
    public class CountDownDbContext: ApplicationDbContext
    {
        public DbSet<Todo> Todoes { get; set; }
    }
}