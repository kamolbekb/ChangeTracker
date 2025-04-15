namespace _3.Models;

public class Book : IEntity
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public int PageCount { get; set; }
}
