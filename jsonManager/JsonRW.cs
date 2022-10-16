namespace JsonRW;

using System;
using System.Text.Json;


/// <summary>
/// Lector/escritor simple de Json's
/// siempre lee archivos que tengan formato Lista
/// </summary>
/// <typeparam name="T">tipo de dato de lectura para cada elemento</typeparam>
public class JsonRW<T>
{
    public List<T>? Content { get; set; }
    private string filePath { get; set; }

    /// <summary>
    /// Constructor: Lee y guarda en memoria el contenido de un archivo json
    /// </summary>
    /// <param name="filePath">Direccion del archivo</param>
    public JsonRW(string filePath)
    {
        this.filePath = filePath;

        string text = File.ReadAllText(filePath);

        try{
            this.Content = JsonSerializer.Deserialize<List<T>>(text);
        }
        catch (System.Text.Json.JsonException){
            this.Content = new List<T>();
        }
    }


    /// <summary>
    /// Agrega un nuevo elemento al archivo json
    /// </summary>
    /// <param name="value">El item a agregar, debe ser del 
    /// mismo tipo que se provee al constructor</param>
    public void Add(object? value)
    {
        if (this.Content is null || value is null) return;

        foreach (var r in this.Content){
            if (r != null && r.ToString() == value.ToString())return;
        }

        this.Content.Add((T)value);

        Save();
    }

    private void Save()
    {
        if (this.Content is null) return;

        File.WriteAllText(this.filePath, JsonSerializer.Serialize<List<T>>(this.Content));
    }
}
