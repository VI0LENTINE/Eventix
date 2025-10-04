using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Eventix.Models;

namespace Eventix.Data
{
    public class EventixContext : DbContext
    {
        public EventixContext (DbContextOptions<EventixContext> options)
            : base(options)
        {
        }
        public DbSet<Eventix.Models.Performance> Performance { get; set; } = default!;
        public DbSet<Eventix.Models.Category> Category { get; set; }
    }
}
