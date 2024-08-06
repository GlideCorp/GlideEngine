using System;
using System.Collections.Generic;
using System.Drawing;
using System.Numerics;
using System.Runtime.InteropServices;
using Core.Logs;
using Engine.Rendering;
using ImGuiNET;
using Silk.NET.Assimp;
using Silk.NET.Input;
using Silk.NET.Input.Extensions;
using Silk.NET.Maths;
using Silk.NET.OpenGL;
using Silk.NET.OpenGL.Extensions.ImGui;
using Silk.NET.Windowing;
using Shader = Engine.Rendering.Shader;
using Texture = Engine.Rendering.Texture2D;

namespace Editor.ImGUI;

public class ImGuiController : IDisposable
{
    private GL _gl;

    private IView _view;

    private IInputContext _input;

    private bool _frameBegun;

    private readonly List<char> _pressedChars = new List<char>();

    private IKeyboard _keyboard;

    private int _attribLocationTex;

    private int _attribLocationProjMtx;

    private int _attribLocationVtxPos;

    private int _attribLocationVtxUV;

    private int _attribLocationVtxColor;

    private uint _vboHandle;

    private uint _elementsHandle;

    private uint _vertexArrayObject;

    private Texture _fontTexture;

    private Shader _shader;

    private int _windowWidth;

    private int _windowHeight;

    public IntPtr Context;

    private static Key[] keyEnumArr = (Key[])Enum.GetValues(typeof(Key));

    //
    // Riepilogo:
    //     Constructs a new ImGuiController.
    public ImGuiController(GL gl, IView view, IInputContext input)
        : this(gl, view, input, null, null)
    {
    }

    //
    // Riepilogo:
    //     Constructs a new ImGuiController with font configuration.
    public ImGuiController(GL gl, IView view, IInputContext input, ImGuiFontConfig imGuiFontConfig)
        : this(gl, view, input, imGuiFontConfig, null)
    {
    }

    //
    // Riepilogo:
    //     Constructs a new ImGuiController with an onConfigureIO Action.
    public ImGuiController(GL gl, IView view, IInputContext input, Action onConfigureIO)
        : this(gl, view, input, null, onConfigureIO)
    {
    }

    //
    // Riepilogo:
    //     Constructs a new ImGuiController with font configuration and onConfigure Action.
public ImGuiController(GL gl, IView view, IInputContext input, ImGuiFontConfig? imGuiFontConfig = null, Action onConfigureIO = null)
    {
        Init(gl, view, input);
        ImGuiIOPtr iO = ImGuiNET.ImGui.GetIO();
        if (imGuiFontConfig.HasValue)
        {
            IntPtr glyph_ranges = imGuiFontConfig.Value.GetGlyphRange?.Invoke(iO) ?? ((IntPtr)0);
            iO.Fonts.AddFontFromFileTTF(imGuiFontConfig.Value.FontPath, imGuiFontConfig.Value.FontSize, null, glyph_ranges);
        }

        onConfigureIO?.Invoke();
        iO.BackendFlags |= ImGuiBackendFlags.RendererHasVtxOffset;
        iO.ConfigFlags |= ImGuiConfigFlags.DockingEnable;
        CreateDeviceResources();
        SetKeyMappings();
        SetPerFrameImGuiData(1f / 60f);
        BeginFrame();
    }

    public void MakeCurrent()
    {
        ImGuiNET.ImGui.SetCurrentContext(Context);
    }

    private void Init(GL gl, IView view, IInputContext input)
    {
        _gl = gl;
        _view = view;
        _input = input;
        _windowWidth = view.Size.X;
        _windowHeight = view.Size.Y;
        Context = ImGuiNET.ImGui.CreateContext();
        ImGuiNET.ImGui.SetCurrentContext(Context);
        ImGuiNET.ImGui.StyleColorsDark();
    }

    private void BeginFrame()
    {
        ImGuiNET.ImGui.NewFrame();
        _frameBegun = true;
        _keyboard = _input.Keyboards[0];
        _view.Resize += WindowResized;
        _keyboard.KeyChar += OnKeyChar;
    }

    private void OnKeyChar(IKeyboard arg1, char arg2)
    {
        _pressedChars.Add(arg2);
    }

    private void WindowResized(Vector2D<int> size)
    {
        _windowWidth = size.X;
        _windowHeight = size.Y;
    }

    //
    // Riepilogo:
    //     Renders the ImGui draw list data. This method requires a GraphicsDevice because
    //     it may create new DeviceBuffers if the size of vertex or index data has increased
    //     beyond the capacity of the existing buffers. A CommandList is needed to submit
    //     drawing and resource update commands.
    public void Render()
    {
        if (_frameBegun)
        {
            IntPtr currentContext = ImGuiNET.ImGui.GetCurrentContext();
            if (currentContext != Context)
            {
                ImGuiNET.ImGui.SetCurrentContext(Context);
            }

            _frameBegun = false;
            ImGuiNET.ImGui.Render();
            RenderImDrawData(ImGuiNET.ImGui.GetDrawData());
            if (currentContext != Context)
            {
                ImGuiNET.ImGui.SetCurrentContext(currentContext);
            }
        }
    }

    //
    // Riepilogo:
    //     Updates ImGui input and IO configuration state.
    public void Update(float deltaSeconds)
    {
        IntPtr currentContext = ImGuiNET.ImGui.GetCurrentContext();
        if (currentContext != Context)
        {
            ImGuiNET.ImGui.SetCurrentContext(Context);
        }

        if (_frameBegun)
        {
            ImGuiNET.ImGui.Render();
        }

        SetPerFrameImGuiData(deltaSeconds);
        UpdateImGuiInput();
        _frameBegun = true;
        ImGuiNET.ImGui.NewFrame();
        if (currentContext != Context)
        {
            ImGuiNET.ImGui.SetCurrentContext(currentContext);
        }
    }

    //
    // Riepilogo:
    //     Sets per-frame data based on the associated window. This is called by Update(float).
    private void SetPerFrameImGuiData(float deltaSeconds)
    {
        ImGuiIOPtr iO = ImGuiNET.ImGui.GetIO();
        iO.DisplaySize = new Vector2(_windowWidth, _windowHeight);
        if (_windowWidth > 0 && _windowHeight > 0)
        {
            iO.DisplayFramebufferScale = new Vector2(_view.FramebufferSize.X / _windowWidth, _view.FramebufferSize.Y / _windowHeight);
        }

        iO.DeltaTime = deltaSeconds;
    }

    private void UpdateImGuiInput()
    {
        ImGuiIOPtr iO = ImGuiNET.ImGui.GetIO();
        MouseState mouseState = _input.Mice[0].CaptureState();
        IKeyboard keyboard = _input.Keyboards[0];
        iO.MouseDown[0] = mouseState.IsButtonPressed(MouseButton.Left);
        iO.MouseDown[1] = mouseState.IsButtonPressed(MouseButton.Right);
        iO.MouseDown[2] = mouseState.IsButtonPressed(MouseButton.Middle);
        Point point = new Point((int)mouseState.Position.X, (int)mouseState.Position.Y);
        iO.MousePos = new Vector2(point.X, point.Y);
        ScrollWheel scrollWheel = mouseState.GetScrollWheels()[0];
        iO.MouseWheel = scrollWheel.Y;
        iO.MouseWheelH = scrollWheel.X;
        Key[] array = keyEnumArr;
        foreach (Key key in array)
        {
            if (key != Key.Unknown)
            {
                iO.KeysDown[(int)key] = keyboard.IsKeyPressed(key);
            }
        }

        foreach (char pressedChar in _pressedChars)
        {
            iO.AddInputCharacter(pressedChar);
        }

        _pressedChars.Clear();
        iO.KeyCtrl = keyboard.IsKeyPressed(Key.ControlLeft) || keyboard.IsKeyPressed(Key.ControlRight);
        iO.KeyAlt = keyboard.IsKeyPressed(Key.AltLeft) || keyboard.IsKeyPressed(Key.AltRight);
        iO.KeyShift = keyboard.IsKeyPressed(Key.ShiftLeft) || keyboard.IsKeyPressed(Key.ShiftRight);
        iO.KeySuper = keyboard.IsKeyPressed(Key.SuperLeft) || keyboard.IsKeyPressed(Key.SuperRight);
    }

    internal void PressChar(char keyChar)
    {
        _pressedChars.Add(keyChar);
    }

    private static void SetKeyMappings()
    {
        ImGuiIOPtr iO = ImGuiNET.ImGui.GetIO();
        iO.KeyMap[512] = 258;
        iO.KeyMap[513] = 263;
        iO.KeyMap[514] = 262;
        iO.KeyMap[515] = 265;
        iO.KeyMap[516] = 264;
        iO.KeyMap[517] = 266;
        iO.KeyMap[518] = 267;
        iO.KeyMap[519] = 268;
        iO.KeyMap[520] = 269;
        iO.KeyMap[522] = 261;
        iO.KeyMap[523] = 259;
        iO.KeyMap[525] = 257;
        iO.KeyMap[526] = 256;
        iO.KeyMap[546] = 65;
        iO.KeyMap[548] = 67;
        iO.KeyMap[567] = 86;
        iO.KeyMap[569] = 88;
        iO.KeyMap[570] = 89;
        iO.KeyMap[571] = 90;
    }

    private unsafe void SetupRenderState(ImDrawDataPtr drawDataPtr, int framebufferWidth, int framebufferHeight)
    {
        _gl.Enable(GLEnum.Blend);
        _gl.BlendEquation(GLEnum.FuncAdd);
        _gl.BlendFuncSeparate(GLEnum.SrcAlpha, GLEnum.OneMinusSrcAlpha, GLEnum.True, GLEnum.OneMinusSrcAlpha);
        _gl.Disable(GLEnum.CullFace);
        _gl.Disable(GLEnum.DepthTest);
        _gl.Disable(GLEnum.StencilTest);
        _gl.Enable(GLEnum.ScissorTest);
        _gl.Disable(GLEnum.PrimitiveRestart);
        _gl.PolygonMode(GLEnum.FrontAndBack, GLEnum.Fill);
        float x = drawDataPtr.DisplayPos.X;
        float num = drawDataPtr.DisplayPos.X + drawDataPtr.DisplaySize.X;
        float y = drawDataPtr.DisplayPos.Y;
        float num2 = drawDataPtr.DisplayPos.Y + drawDataPtr.DisplaySize.Y;
        Span<float> span = stackalloc float[16]
        {
            2f / (num - x),
            0f,
            0f,
            0f,
            0f,
            2f / (y - num2),
            0f,
            0f,
            0f,
            0f,
            -1f,
            0f,
            (num + x) / (x - num),
            (y + num2) / (num2 - y),
            0f,
            1f
        };
        _shader.Use();
        _gl.Uniform1(_attribLocationTex, 0);
        _gl.UniformMatrix4(_attribLocationProjMtx, 1u, transpose: false, span);
        _gl.BindSampler(0u, 0u);
        _vertexArrayObject = _gl.GenVertexArray();
        _gl.BindVertexArray(_vertexArrayObject);
        _gl.BindBuffer(GLEnum.ArrayBuffer, _vboHandle);
        _gl.BindBuffer(GLEnum.ElementArrayBuffer, _elementsHandle);
        _gl.EnableVertexAttribArray((uint)_attribLocationVtxPos);
        _gl.EnableVertexAttribArray((uint)_attribLocationVtxUV);
        _gl.EnableVertexAttribArray((uint)_attribLocationVtxColor);
        _gl.VertexAttribPointer((uint)_attribLocationVtxPos, 2, GLEnum.Float, normalized: false, (uint)sizeof(ImDrawVert), null);
        _gl.VertexAttribPointer((uint)_attribLocationVtxUV, 2, GLEnum.Float, normalized: false, (uint)sizeof(ImDrawVert), (void*)8);
        _gl.VertexAttribPointer((uint)_attribLocationVtxColor, 4, GLEnum.UnsignedByte, normalized: true, (uint)sizeof(ImDrawVert), (void*)16);
    }

    private unsafe void RenderImDrawData(ImDrawDataPtr drawDataPtr)
    {
        int num = (int)(drawDataPtr.DisplaySize.X * drawDataPtr.FramebufferScale.X);
        int num2 = (int)(drawDataPtr.DisplaySize.Y * drawDataPtr.FramebufferScale.Y);
        if (num <= 0 || num2 <= 0)
        {
            return;
        }

        _gl.GetInteger(GLEnum.ActiveTexture, out var data);
        _gl.ActiveTexture(GLEnum.Texture0);
        _gl.GetInteger(GLEnum.CurrentProgram, out var data2);
        _gl.GetInteger(GLEnum.TextureBinding2D, out var data3);
        _gl.GetInteger(GLEnum.SamplerBinding, out var data4);
        _gl.GetInteger(GLEnum.ArrayBufferBinding, out var data5);
        _gl.GetInteger(GLEnum.VertexArrayBinding, out var data6);
        Span<int> data7 = stackalloc int[2];
        _gl.GetInteger(GLEnum.PolygonMode, data7);
        Span<int> data8 = stackalloc int[4];
        _gl.GetInteger(GLEnum.ScissorBox, data8);
        _gl.GetInteger(GLEnum.BlendSrcRgb, out var data9);
        _gl.GetInteger(GLEnum.BlendDstRgb, out var data10);
        _gl.GetInteger(GLEnum.BlendSrcAlpha, out var data11);
        _gl.GetInteger(GLEnum.BlendDstAlpha, out var data12);
        _gl.GetInteger(GLEnum.BlendEquation, out var data13);
        _gl.GetInteger(GLEnum.BlendEquationAlpha, out var data14);
        bool flag = _gl.IsEnabled(GLEnum.Blend);
        bool flag2 = _gl.IsEnabled(GLEnum.CullFace);
        bool flag3 = _gl.IsEnabled(GLEnum.DepthTest);
        bool flag4 = _gl.IsEnabled(GLEnum.StencilTest);
        bool flag5 = _gl.IsEnabled(GLEnum.ScissorTest);
        bool flag6 = _gl.IsEnabled(GLEnum.PrimitiveRestart);
        SetupRenderState(drawDataPtr, num, num2);
        Vector2 displayPos = drawDataPtr.DisplayPos;
        Vector2 framebufferScale = drawDataPtr.FramebufferScale;
        Vector4 vector = default(Vector4);
        for (int i = 0; i < drawDataPtr.CmdListsCount; i++)
        {
            ImDrawListPtr imDrawListPtr = drawDataPtr.CmdLists[i];
            _gl.BufferData(GLEnum.ArrayBuffer, (nuint)(imDrawListPtr.VtxBuffer.Size * sizeof(ImDrawVert)), (void*)imDrawListPtr.VtxBuffer.Data, GLEnum.StreamDraw);
            _gl.BufferData(GLEnum.ElementArrayBuffer, (nuint)(imDrawListPtr.IdxBuffer.Size * 2), (void*)imDrawListPtr.IdxBuffer.Data, GLEnum.StreamDraw);
            for (int j = 0; j < imDrawListPtr.CmdBuffer.Size; j++)
            {
                ImDrawCmdPtr imDrawCmdPtr = imDrawListPtr.CmdBuffer[j];
                if (imDrawCmdPtr.UserCallback != IntPtr.Zero)
                {
                    throw new NotImplementedException();
                }

                vector.X = (imDrawCmdPtr.ClipRect.X - displayPos.X) * framebufferScale.X;
                vector.Y = (imDrawCmdPtr.ClipRect.Y - displayPos.Y) * framebufferScale.Y;
                vector.Z = (imDrawCmdPtr.ClipRect.Z - displayPos.X) * framebufferScale.X;
                vector.W = (imDrawCmdPtr.ClipRect.W - displayPos.Y) * framebufferScale.Y;
                if (vector.X < (float)num && vector.Y < (float)num2 && vector.Z >= 0f && vector.W >= 0f)
                {
                    _gl.Scissor((int)vector.X, (int)((float)num2 - vector.W), (uint)(vector.Z - vector.X), (uint)(vector.W - vector.Y));
                    _gl.BindTexture(GLEnum.Texture2D, (uint)(int)imDrawCmdPtr.TextureId);
                    _gl.DrawElementsBaseVertex(GLEnum.Triangles, imDrawCmdPtr.ElemCount, GLEnum.UnsignedShort, (void*)(imDrawCmdPtr.IdxOffset * 2), (int)imDrawCmdPtr.VtxOffset);
                }
            }
        }

        _gl.DeleteVertexArray(_vertexArrayObject);
        _vertexArrayObject = 0u;
        _gl.UseProgram((uint)data2);
        _gl.BindTexture(GLEnum.Texture2D, (uint)data3);
        _gl.BindSampler(0u, (uint)data4);
        _gl.ActiveTexture((GLEnum)data);
        _gl.BindVertexArray((uint)data6);
        _gl.BindBuffer(GLEnum.ArrayBuffer, (uint)data5);
        _gl.BlendEquationSeparate((GLEnum)data13, (GLEnum)data14);
        _gl.BlendFuncSeparate((GLEnum)data9, (GLEnum)data10, (GLEnum)data11, (GLEnum)data12);
        if (flag)
        {
            _gl.Enable(GLEnum.Blend);
        }
        else
        {
            _gl.Disable(GLEnum.Blend);
        }

        if (flag2)
        {
            _gl.Enable(GLEnum.CullFace);
        }
        else
        {
            _gl.Disable(GLEnum.CullFace);
        }

        if (flag3)
        {
            _gl.Enable(GLEnum.DepthTest);
        }
        else
        {
            _gl.Disable(GLEnum.DepthTest);
        }

        if (flag4)
        {
            _gl.Enable(GLEnum.StencilTest);
        }
        else
        {
            _gl.Disable(GLEnum.StencilTest);
        }

        if (flag5)
        {
            _gl.Enable(GLEnum.ScissorTest);
        }
        else
        {
            _gl.Disable(GLEnum.ScissorTest);
        }

        if (flag6)
        {
            _gl.Enable(GLEnum.PrimitiveRestart);
        }
        else
        {
            _gl.Disable(GLEnum.PrimitiveRestart);
        }

        _gl.PolygonMode(GLEnum.FrontAndBack, (GLEnum)data7[0]);
        _gl.Scissor(data8[0], data8[1], (uint)data8[2], (uint)data8[3]);
    }

    private void CreateDeviceResources()
    {
        _gl.GetInteger(GLEnum.TextureBinding2D, out var data);
        _gl.GetInteger(GLEnum.ArrayBufferBinding, out var data2);
        _gl.GetInteger(GLEnum.VertexArrayBinding, out var data3);

        //source.VertexSource = "#version 330\n        layout (location = 0) in vec2 Position;\n        layout (location = 1) in vec2 UV;\n        layout (location = 2) in vec4 Color;\n        uniform mat4 ProjMtx;\n        out vec2 Frag_UV;\n        out vec4 Frag_Color;\n        void main()\n        {\n            Frag_UV = UV;\n            Frag_Color = Color;\n            gl_Position = ProjMtx * vec4(Position.xy,0,1);\n        }";
        //source.FragmentSource = "#version 330\n        in vec2 Frag_UV;\n        in vec4 Frag_Color;\n        uniform sampler2D Texture;\n        layout (location = 0) out vec4 Out_Color;\n        void main()\n        {\n            Out_Color = Frag_Color * texture(Texture, Frag_UV.st);\n        }";
        _shader = new Shader("resources\\shaders\\imgui.vs", "resources\\shaders\\imgui.fg");
        _attribLocationTex = _shader.GetLocation("Texture");
        _attribLocationProjMtx = _shader.GetLocation("ProjMtx");
        _attribLocationVtxPos = 0;//_shader.GetLocation("Position");
        _attribLocationVtxUV = 1;//_shader.GetLocation("UV");
        _attribLocationVtxColor = 2;// _shader.GetLocation("Color");
        _vboHandle = _gl.GenBuffer();
        _elementsHandle = _gl.GenBuffer();
        RecreateFontDeviceTexture();
        _gl.BindTexture(GLEnum.Texture2D, (uint)data);
        _gl.BindBuffer(GLEnum.ArrayBuffer, (uint)data2);
        _gl.BindVertexArray((uint)data3);
    }

    //
    // Riepilogo:
    //     Creates the texture used to render text.
    private unsafe void RecreateFontDeviceTexture()
    {
        ImGuiIOPtr iO = ImGuiNET.ImGui.GetIO();
        iO.Fonts.GetTexDataAsRGBA32(out byte* out_pixels, out int out_width, out int out_height, out int bytesPerPixel);

        var pixels = new byte[out_width * out_height * bytesPerPixel];
        Marshal.Copy(new IntPtr(out_pixels), pixels, 0, pixels.Length);

        _gl.GetInteger(GLEnum.TextureBinding2D, out var data);
        _fontTexture = new Texture(out_width, out_height);

        _fontTexture.SetData(pixels);
        _fontTexture.Bind();

        iO.Fonts.SetTexID((IntPtr)_fontTexture.TextureID);
        _gl.BindTexture(GLEnum.Texture2D, (uint)data);
    }

    //
    // Riepilogo:
    //     Frees all graphics resources used by the renderer.
    public void Dispose()
    {
        _view.Resize -= WindowResized;
        _keyboard.KeyChar -= OnKeyChar;
        _gl.DeleteBuffer(_vboHandle);
        _gl.DeleteBuffer(_elementsHandle);
        _gl.DeleteVertexArray(_vertexArrayObject);
        _fontTexture.Dispose();
        _shader.Dispose();
        ImGuiNET.ImGui.DestroyContext(Context);
    }
}