﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Olaeriu_Teodora_Lab2.Data;
using Olaeriu_Teodora_Lab2.Models;

namespace Olaeriu_Teodora_Lab2.Pages.Books
{
    [Authorize(Roles = "Admin")]
    public class EditModel : BookCategoriesPageModel
    {
        private readonly Olaeriu_Teodora_Lab2.Data.Olaeriu_Teodora_Lab2Context _context;

        public EditModel(Olaeriu_Teodora_Lab2.Data.Olaeriu_Teodora_Lab2Context context)
        {
            _context = context;
        }

        [BindProperty]
        public Book Book { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.Book == null)
            {
                return NotFound();
            }
            Book = await _context.Book.Include(b => b.Publisher).Include(b => b.BookCategories).ThenInclude(b => b.Category).AsNoTracking().FirstOrDefaultAsync(m => m.ID == id);
            var book =  await _context.Book.FirstOrDefaultAsync(m => m.ID == id);
            if (book == null)
            {
                return NotFound();
            }
            //apelam PopulateAssignedCategoryData pentru o obtine informatiile necesare checkbox-
            //urilor folosind clasa AssignedCategoryData
            PopulateAssignedCategoryData(_context, Book);
            Book = book;
            var authorList = _context.Author.Select(x => new
            {
                x.ID,
                FullName = x.FirstName + " " + x.LastName
            });
            ViewData["PublisherID"] = new SelectList(_context.Set<Publisher>(), "ID", "PublisherName");
            ViewData["AuthorID"] = new SelectList(authorList, "ID", "FullName");
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync(int? id, string[] selectedCategories)
        {
            if (id == null)
            {
                return NotFound();
            }
            var bookToUpdate = await _context.Book
            .Include(i => i.Publisher)
            .Include(i => i.BookCategories)
            .ThenInclude(i => i.Category)
            .FirstOrDefaultAsync(s => s.ID == id);
            if (bookToUpdate == null)
            {
                return NotFound();
            }
            if (await TryUpdateModelAsync<Book>(
            bookToUpdate,
            "Book",
            i => i.Title, i => i.Author,
            i => i.Price, i => i.PublishingDate, i => i.Publisher))
            {
                UpdateBookCategories(_context, selectedCategories, bookToUpdate);
                await _context.SaveChangesAsync();
                return RedirectToPage("./Index");
            }
            //Apelam UpdateBookCategories pentru a aplica informatiile din checkboxuri la entitatea Books care
            //este editata
            UpdateBookCategories(_context, selectedCategories, bookToUpdate);
            PopulateAssignedCategoryData(_context, bookToUpdate);
            return Page();
        }

        private bool BookExists(int id)
        {
          return _context.Book.Any(e => e.ID == id);
        }
    }
}
