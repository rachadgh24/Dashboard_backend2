using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;
using Test2.DataLayer.Entities;

namespace Test2.DataLayer.DbContexts
{
    public class DotNetTrainingCoreContext : DbContext
    {
        public DotNetTrainingCoreContext(DbContextOptions<DotNetTrainingCoreContext> options)
              : base(options)
        {

        }

        public DbSet<Post> Posts { get; set; }
        //public DbSet<Car> Cars { get; set; }


    }
}
