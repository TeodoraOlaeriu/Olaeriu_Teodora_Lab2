using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Olaeriu_Teodora_Lab2.Models;

namespace Olaeriu_Teodora_Lab2.Data
{
    public class Olaeriu_Teodora_Lab2Context : DbContext
    {
        public Olaeriu_Teodora_Lab2Context (DbContextOptions<Olaeriu_Teodora_Lab2Context> options)
            : base(options)
        {
        }

        public DbSet<Olaeriu_Teodora_Lab2.Models.Book> Book { get; set; } = default!;

        public DbSet<Olaeriu_Teodora_Lab2.Models.Publisher> Publisher { get; set; }

        public DbSet<Olaeriu_Teodora_Lab2.Models.Author> Author { get; set; }

        public DbSet<Olaeriu_Teodora_Lab2.Models.Category> Category { get; set; }

        public DbSet<Olaeriu_Teodora_Lab2.Models.Member> Member { get; set; }

        public DbSet<Olaeriu_Teodora_Lab2.Models.Borrowing> Borrowing { get; set; }
    }
}
