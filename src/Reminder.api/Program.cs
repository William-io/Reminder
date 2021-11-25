using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSingleton<ItemRepository>();

var app = builder.Build();

app.MapGet("/items", ([FromServices] ItemRepository items) =>
{
    return items.Getall();
});

app.MapGet("/", () => "Iniciando API Reminder");
app.Run();


record Item(Guid id, string title, DateTime data, bool IsCompleted);

class ItemRepository
{
    private Dictionary<Guid, Item> items = new Dictionary<Guid, Item>();
    public ItemRepository()
    {

        var firstItem = new Item(Guid.NewGuid(), "First item", DateTime.Now, false);
        var secondItem = new Item(Guid.NewGuid(), "Second item", DateTime.Now, false);
        var thirdItem = new Item(Guid.NewGuid(), "Third item", DateTime.Now, false);

        items.Add(firstItem.id, firstItem);
        items.Add(secondItem.id, secondItem);
        items.Add(thirdItem.id, thirdItem);
    }

    public IEnumerable<Item> Getall() => items.Values;
    public Item GetById(Guid id) => items[id];
    public void Add(Item item) => items.Add(item.id, item);
    public void Update(Item item) => items[item.id] = item;
    public void Delete(Guid id) => items.Remove(id);
}
