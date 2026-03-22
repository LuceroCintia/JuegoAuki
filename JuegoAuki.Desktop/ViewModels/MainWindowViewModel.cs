using System.Collections.ObjectModel;
using System.Linq;
using Avalonia.Media;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using JuegoAuki.Desktop.Models;

namespace JuegoAuki.Desktop.ViewModels;

public partial class MainWindowViewModel : ObservableObject
{
    private readonly List<Challenge> _challenges;
    private int _challengeIndex;

    public MainWindowViewModel()
    {
        HairOptions = new ObservableCollection<string> { "Corto aventurero", "Rulos brillantes", "Casco espacial" };
        OutfitOptions = new ObservableCollection<string> { "Explorador azul", "Inventor verde", "Capitana violeta" };
        PetOptions = new ObservableCollection<string> { "Nube feliz", "Robot mini", "Dragón bebé" };
        AvatarColors = new ObservableCollection<AvatarColorOption>
        {
            new("#4F8CFF"),
            new("#FF8A65"),
            new("#7E57C2"),
            new("#26A69A"),
            new("#EC407A")
        };

        WorldProgress = new ObservableCollection<WorldProgress>
        {
            new() { Name = "Pueblo de los Números", Summary = "Sumas y restas simples" },
            new() { Name = "Bosque de las Palabras", Summary = "Lectura y vocabulario" },
            new() { Name = "Montaña de los Enigmas", Summary = "Lógica y resolución" }
        };

        _challenges = new List<Challenge>
        {
            new(
                "Pueblo de los Números",
                "Puente de sumas",
                "El constructor necesita una respuesta correcta para terminar el puente.",
                "¿Cuánto es 4 + 3?",
                new List<string> { "6", "7", "8" },
                "7",
                "¡Muy bien! El puente está listo y ganaste 10 monedas."),
            new(
                "Bosque de las Palabras",
                "Palabra escondida",
                "La bibliotecaria perdió una letra y necesita tu ayuda.",
                "Completa la palabra: C _ S A",
                new List<string> { "A", "O", "U" },
                "A",
                "¡Excelente lectura! La palabra correcta era CASA."),
            new(
                "Montaña de los Enigmas",
                "Camino de patrones",
                "Las piedras mágicas siguen un orden especial.",
                "¿Qué sigue? círculo, cuadrado, círculo, cuadrado, ...",
                new List<string> { "círculo", "triángulo", "estrella" },
                "círculo",
                "¡Genial! Descubriste el patrón secreto del camino.")
        };

        SelectedHair = HairOptions.First();
        SelectedOutfit = OutfitOptions.First();
        SelectedPet = PetOptions.First();
        AvatarName = "Auki Explorador";
        AvatarColor = Brush.Parse(AvatarColors.First().Hex);
        Level = 1;
        Coins = 0;
        Stars = 0;
        FeedbackMessage = "Elige una misión para comenzar tu aventura.";
        CurrentChallenge = _challenges[0];
    }

    public ObservableCollection<string> HairOptions { get; }
    public ObservableCollection<string> OutfitOptions { get; }
    public ObservableCollection<string> PetOptions { get; }
    public ObservableCollection<AvatarColorOption> AvatarColors { get; }
    public ObservableCollection<WorldProgress> WorldProgress { get; }

    [ObservableProperty]
    private string avatarName;

    [ObservableProperty]
    private string selectedHair;

    [ObservableProperty]
    private string selectedOutfit;

    [ObservableProperty]
    private string selectedPet;

    [ObservableProperty]
    private IBrush avatarColor;

    [ObservableProperty]
    private int level;

    [ObservableProperty]
    private int coins;

    [ObservableProperty]
    private int stars;

    [ObservableProperty]
    private Challenge currentChallenge;

    [ObservableProperty]
    private string feedbackMessage;

    [RelayCommand]
    private void SelectColor(string hex) => AvatarColor = Brush.Parse(hex);

    [RelayCommand]
    private void Answer(string answer)
    {
        if (answer == CurrentChallenge.CorrectAnswer)
        {
            Coins += 10;
            Stars += 1;
            Level = 1 + (Stars / 3);
            FeedbackMessage = CurrentChallenge.SuccessMessage;
            AdvanceWorld(CurrentChallenge.World);
        }
        else
        {
            FeedbackMessage = "¡Buen intento! Mira la misión otra vez y prueba con otra opción.";
        }
    }

    [RelayCommand]
    private void NextChallenge()
    {
        _challengeIndex = (_challengeIndex + 1) % _challenges.Count;
        CurrentChallenge = _challenges[_challengeIndex];
        FeedbackMessage = "Nueva misión cargada. ¡Lee con atención y diviértete!";
    }

    private void AdvanceWorld(string worldName)
    {
        var world = WorldProgress.FirstOrDefault(item => item.Name == worldName);
        world?.Advance();
        OnPropertyChanged(nameof(WorldProgress));
    }
}
