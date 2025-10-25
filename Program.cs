using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace PokeApiApp
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("=== PokéAPI - Consulta de Pokémon ===");
            Console.Write("Ingresa el nombre de un Pokémon: ");
            string pokemonName = Console.ReadLine()?.ToLower();

            if (string.IsNullOrEmpty(pokemonName))
            {
                Console.WriteLine("❌ No ingresaste ningún nombre.");
                return;
            }

            string apiUrl = $"https://pokeapi.co/api/v2/pokemon/{pokemonName}";

            using (HttpClient client = new HttpClient())
            {
                try
                {
                    Console.WriteLine("\nBuscando información... 🔍");
                    HttpResponseMessage response = await client.GetAsync(apiUrl);

                    if (response.IsSuccessStatusCode)
                    {
                        string json = await response.Content.ReadAsStringAsync();
                        JObject data = JObject.Parse(json);

                        string name = data["name"]?.ToString() ?? "Desconocido";
                        double height = data["height"]?.ToObject<double>() ?? 0;
                        double weight = data["weight"]?.ToObject<double>() ?? 0;

                        Console.WriteLine($"\n=== Información del Pokémon ===");
                        Console.WriteLine($"Nombre: {name}");
                        Console.WriteLine($"Altura: {height / 10} m");
                        Console.WriteLine($"Peso: {weight / 10} kg");

                        Console.WriteLine("\nTipos:");
                        foreach (var type in data["types"])
                        {
                            string typeName = type["type"]?["name"]?.ToString();
                            Console.WriteLine($" - {typeName}");
                        }

                        Console.WriteLine("\n Consulta completada con éxito.");
                    }
                    else
                    {
                        Console.WriteLine($" Error: No se encontró el Pokémon '{pokemonName}'.");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Ocurrió un error: {ex.Message}");
                }
            }
        }
    }
}
