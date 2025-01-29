using Microsoft.EntityFrameworkCore;
using TestTask.Data;
using TestTask.Models;
using TestTask.Services.Interfaces;

namespace TestTask.Services.Implementations;

public class AuthorService(ApplicationDbContext applicationDbContext) : IAuthorService
{
    public async Task<Author> GetAuthor()
    {
        // Автора, который написал книгу с самым длинным названием ( в случае если таких авторов окажется несколько, необходимо вернуть автора с наименьшим Id)
        var author = await applicationDbContext.Authors
            .Where(a => a.Books.Any())
            .OrderByDescending(a => a.Books.Max(b => b.Title.Length))
            .ThenBy(a => a.Id)
            .FirstOrDefaultAsync();
        return author;
    }

    public async Task<List<Author>> GetAuthors()
    {
        //Авторов, написавших четное количество книг, изданных после 2015 года
        List<Author> authors = await applicationDbContext.Authors.AsNoTracking().Where(
                a => a.Books.Count(b => DateTime.Compare(b.PublishDate, new DateTime(2015, 0, 0)) > 0) % 2 == 0)
            .ToListAsync();
        return authors;
    }
}