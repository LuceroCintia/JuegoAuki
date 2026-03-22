namespace JuegoAuki.Desktop.Models;

public sealed record Challenge(
    string World,
    string Title,
    string Story,
    string Question,
    IReadOnlyList<string> Options,
    string CorrectAnswer,
    string SuccessMessage);
