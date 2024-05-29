using System.Timers;

namespace FizzCode.PBIFizz;

public class PbiFizz : SettingsWithPathAndFile
{
    private System.Timers.Timer? checkTimer;
    private DateTime lastSaved;

    public PbiFizz(string path, string file, Settings settings)
        : base(path, file, settings)
    {
    }

    public void Start()
    {
        InitializeDirectory();
        SetTimer();
    }

    private void SetTimer()
    {
        lastSaved = File.GetLastWriteTime(GetOriginalPathAndFileName());
        checkTimer = new System.Timers.Timer() { Interval = 1000, AutoReset = true };
        checkTimer.Elapsed += (sender, e) => TimerElapsed(sender, e, Path, FileName);
        checkTimer.Start();
    }

    private void TimerElapsed(object sender, ElapsedEventArgs e, string path, string file)
    {
        DateTime lastWriteTime = File.GetLastWriteTime(path + file);
        if (lastWriteTime > lastSaved)
        {
            checkTimer.Stop();
            ProcessFile(path, file);
            lastSaved = lastWriteTime;
            checkTimer.Start();
        }
    }

    public void ProcessFile(string path, string file)
    {
        Console.WriteLine("File changed, processing: " + GetOriginalPathAndFileName());
        var fd = new FileDisjoiner(path, file, Settings);
        fd.Process();
    }

    private void InitializeDirectory()
    {
        Directory.CreateDirectory(Path + Settings.ToolPath);
    }
}
