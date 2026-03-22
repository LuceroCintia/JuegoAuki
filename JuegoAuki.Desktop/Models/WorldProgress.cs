namespace JuegoAuki.Desktop.Models;

public sealed class WorldProgress
{
    public required string Name { get; init; }
    public required string Summary { get; init; }
    public int Completed { get; private set; }

    public string CompletedLabel => $"{Completed}/3";

    public void Advance()
    {
        if (Completed < 3)
        {
            Completed++;
        }
    }
}
