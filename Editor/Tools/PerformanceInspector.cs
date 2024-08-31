using Editor.Gui;
using Engine.Utilities;
using ImGuiNET;
using System.Numerics;
using System.Timers;
using Timer = System.Timers.Timer;

namespace Editor.Tools
{
    public class PerformanceInspector : Tool
    {
        Timer timer;

        float currentFrameTime;
        float minFrameTime;
        float maxFrameTime;
        float avgFrameTime;
        float avgCount;

        int samplesCount;
        int frameNumbers;
        float[] frameTimeSamples;

        public PerformanceInspector() : base($"{Lucide.Gauge} Performance Inspector")
        {
            minFrameTime = float.MaxValue;
            frameTimeSamples = new float[25];

            timer = new Timer(1000);

            timer.Elapsed += OnTimerTrill;
            timer.AutoReset = true;
            timer.Enabled = true;

            Open = true;
        }

        private void OnTimerTrill(object? sender, ElapsedEventArgs e)
        {
            currentFrameTime = 1000.0f/frameNumbers;
            frameNumbers = 0;

            if(samplesCount < frameTimeSamples.Length)
                frameTimeSamples[samplesCount++] = currentFrameTime;
            else
            {
                Array.Copy(frameTimeSamples, 1, frameTimeSamples, 0, frameTimeSamples.Length-1);
                frameTimeSamples[^1] = currentFrameTime;
            }
        }

        protected override void ToolGui()
        {
            frameNumbers++;
            avgCount++;

            float frameTime = Time.DeltaTime;

            if(frameTime < minFrameTime)
            {
                minFrameTime = frameTime;
            }

            if(frameTime > maxFrameTime)
            {
                maxFrameTime = frameTime;
            }

            avgFrameTime = ((avgFrameTime * (avgCount-1)) + frameTime)/ avgCount;

            ImGui.Text($"Current FrameTime {Time.DeltaTime*1000:N2} ms ({Time.FPS:D} fps)");
            ImGui.Text($"Min FrameTime {minFrameTime * 1000:N2} ms ({(int)(1/minFrameTime):D} fps)");
            ImGui.Text($"Max FrameTime {maxFrameTime * 1000:N2} ms ({(int)(1/maxFrameTime):D} fps)");
            ImGui.Text($"Avg FrameTime {avgFrameTime * 1000:N2} ms ({(int)(1/avgFrameTime):D} fps)");
            
            ImGui.PlotHistogram("", ref frameTimeSamples[0], frameTimeSamples.Length, 0, "", 0.0f, avgFrameTime * 1000 * 2, new Vector2(0, 80));
        }
    }
}
