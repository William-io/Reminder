using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSingleton<ItemRepository>();

var app = builder.Build();

app.MapGet("/items", ([FromServices] ItemRepository items) =>
{
    return items.Getall();
});

app.MapPost("/items", ([FromServices] ItemRepository items, Item item) =>
{
    if (items.GetById(item.id) != null)
    {
        return Results.BadRequest("Não foi possivel adicionar!!");
    }
    //Dando tudo certo retorna o item sob status OK!
    items.Add(item);
    return Results.Created($"/Items/{item.id}", item);
});

app.MapGet("/items/{id}", ([FromServices] ItemRepository items, int id) =>
{
    var item = items.GetById(id);
    return item == null ? Results.NotFound() : Results.Ok(item);
});

app.MapPut("/items/{id}", ([FromServices] ItemRepository items, int id, Item item) =>
{
    if (items.GetById(id) == null)
    {
        return Results.NotFound();
    }
    items.Update(item);
    return Results.Ok(item);
});


app.MapDelete("/items/{id}", ([FromServices] ItemRepository items, int id) =>
{
    if (items.GetById(id) == null)
    {
        return Results.NotFound();
    }
    items.Delete(id);
    return Results.Ok();
});

app.MapGet("/", () => "Iniciando API Reminder");
app.Run();


record Item(int id, string title, bool IsCompleted);

class ItemRepository
{
    private Dictionary<int, Item> items = new Dictionary<int, Item>();
    // public ItemRepository()
    // {

    //     var firstItem = new Item(1, "First item", DateTime.Now, false);
    //     var secondItem = new Item(Guid.NewGuid(), "Second item", DateTime.Now, false);
    //     var thirdItem = new Item(Guid.NewGuid(), "Third item", DateTime.Now, false);

    //     items.Add(firstItem.id, firstItem);
    //     items.Add(secondItem.id, secondItem);
    //     items.Add(thirdItem.id, thirdItem);
    // }

    public IEnumerable<Item> Getall() => items.Values;
    public Item GetById(int id)
    {
        if (items.ContainsKey(id))
        {
            return items[id];
        }

        return null;
    }
    public void Add(Item item) => items.Add(item.id, item);
    public void Update(Item item) => items[item.id] = item;
    public void Delete(int id) => items.Remove(id);
}
