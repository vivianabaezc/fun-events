using System.Net.Http.Json;
using System.Text.Json;

var jsonOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

var apiBaseUrl = args.Length > 3 ? args[3] : "http://localhost:5112";

Guid eventId;
Guid userId;
int quantity;

if (args.Length >= 2)
{
    eventId = Guid.Parse(args[0]);
    userId = Guid.Parse(args[1]);
    quantity = args.Length > 2 ? int.Parse(args[2]) : 1;
}
else
{
    eventId = ReadGuid("Codigo de evento (Guid): ");
    userId = ReadGuid("Codigo de usuario (Guid): ");
    quantity = ReadQuantity("Cantidad de entradas [1]: ");
}

using var httpClient = new HttpClient { BaseAddress = new Uri(apiBaseUrl) };

var request = new ReservationRequest(eventId, userId, quantity);

Console.WriteLine();
Console.WriteLine($"Reservando {quantity} entrada(s) para el evento {eventId} a nombre del usuario {userId}...");

HttpResponseMessage response;

try
{
    response = await httpClient.PostAsJsonAsync("api/reservations", request);
}
catch (HttpRequestException ex)
{
    Console.WriteLine($"No se pudo contactar la API en {apiBaseUrl}: {ex.Message}");
    return 1;
}

if (response.IsSuccessStatusCode)
{
    var reservation = await response.Content.ReadFromJsonAsync<ReservationResponse>(jsonOptions);

    Console.WriteLine("Reserva creada.");
    Console.WriteLine($"  Id:      {reservation?.Id}");
    Console.WriteLine($"  Evento:  {reservation?.EventName}");
    Console.WriteLine($"  Usuario: {reservation?.UserName}");
    Console.WriteLine($"  Cantidad:{reservation?.Quantity}");
    Console.WriteLine($"  Total:   {reservation?.TotalPrice}");
    Console.WriteLine($"  Estado:  {reservation?.Status}");

    return 0;
}

var error = await response.Content.ReadFromJsonAsync<ErrorResponse>(jsonOptions);
Console.WriteLine($"No se pudo crear la reserva ({(int)response.StatusCode}): {error?.Message ?? response.ReasonPhrase}");

return 1;

static Guid ReadGuid(string prompt)
{
    while (true)
    {
        Console.Write(prompt);

        if (Guid.TryParse(Console.ReadLine(), out var value))
            return value;

        Console.WriteLine("Valor invalido, se espera un Guid.");
    }
}

static int ReadQuantity(string prompt)
{
    Console.Write(prompt);
    var input = Console.ReadLine();

    if (string.IsNullOrWhiteSpace(input))
        return 1;

    return int.TryParse(input, out var value) ? value : 1;
}

internal record ReservationRequest(Guid EventId, Guid UserId, int Quantity);

internal record ReservationResponse(
    Guid Id,
    Guid EventId,
    string EventName,
    Guid UserId,
    string UserName,
    int Quantity,
    decimal TotalPrice,
    string Status);

internal record ErrorResponse(string Message);
