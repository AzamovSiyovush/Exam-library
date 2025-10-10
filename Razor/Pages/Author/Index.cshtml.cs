using Domain.DTOs.Author;
using Infrastructure.Interfaces;
using Infrastructure.Response;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Razor.Pages.Author
{
    public class Index(IAuthorServices authorServices) : PageModel
    {
        public Response<IEnumerable<AuthorGetDto>> AuthorGetDto { get; set; }
        public async Task<IActionResult> OnGetAsync()
        {
            AuthorGetDto = await authorServices.GetItems();
            return Page();
        }
        
    }
}
