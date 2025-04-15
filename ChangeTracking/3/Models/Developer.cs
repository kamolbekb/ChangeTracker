namespace _3.Models;

public class Developer : IEntity
{
    public int Id { get; set; }            
    public string FullName { get; set; }  
    public string Stack { get; set; }
    public string Position { get; set; }
}
