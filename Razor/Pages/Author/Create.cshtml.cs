using Domain.DTOs.Author;
using Infrastructure.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Razor.Pages.Author
{
    public class Create(IAuthorServices authorServices) : PageModel
    {
        [BindProperty]
        public AuthorCreateDto AuthorCreateDto { get; set; }

        public void OnGet()
        {
        }
        public async Task OnPostAsync()
        {
            var result = await authorServices.CreateItem(AuthorCreateDto);
        }
    }
}
