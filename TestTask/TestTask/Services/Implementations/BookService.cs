using System.Runtime.InteropServices.JavaScript;
using Microsoft.EntityFrameworkCore;
using TestTask.Data;
using TestTask.Models;
using TestTask.Services.Interfaces;

namespace TestTask.Services.Implementations;

public class BookService(ApplicationDbContext dbContext): IBookService
{
    private const string СarolusRexReleaseDate =  "2012-05-22";
    public async Task<Book> GetBook()
    {
        //. Книгу с наибольшей стоимостью опубликованного тиража
        Book result =  await dbContext.Books.AsNoTracking().OrderByDescending(b => b.Price * b.QuantityPublished)
            .FirstOrDefaultAsync();
        return result;
    }

    public async Task<List<Book>> GetBooks()
    { 
        //1. Книги, в названии которой содержится "Red" и которые опубликованы после выхода альбома "Carolus Rex" группы Sabaton
        DateTime carolusRexReleaseDate = DateTime.Parse(BookService.СarolusRexReleaseDate);
        List<Book> result = await dbContext.Books.Where(b => b.Title.Contains("Red") && DateTime.Compare(b.PublishDate , carolusRexReleaseDate) > 0)
            .AsNoTracking()
            .ToListAsync();
        return result;
    }
}