using Silk.NET.Input;
using Silk.NET.Maths;
using Silk.NET.Windowing;
using Silk.NET.Windowing.Glfw;
using Silk.NET.OpenGL;
public class Program
{
    private static IWindow? _window;
    private static GL? _gl;

    public static void Main(string[] args)
    {
        WindowOptions options = WindowOptions.Default with
        {
            Size = new Vector2D<int>(800, 600),
            Title = "GlideEngine",
            API = new(ContextAPI.OpenGL, ContextProfile.Core, ContextFlags.Debug, new(4, 6))
        };

        _window = Window.Create(options);
        _window.Load += OnLoad;
        _window.Update += OnUpdate;
        _window.Render += OnRender;
        _window.Run();

    }
    private static void OnLoad()
    {
        _gl = _window.CreateOpenGL();
        Console.WriteLine($"{_gl?.GetStringS(GLEnum.Vendor)}\n{_gl?.GetStringS(GLEnum.Version)}\n");
    }

    private static void OnUpdate(double deltaTime) { }

    private static void OnRender(double deltaTime) { }
}
