namespace FizzCode.PBIFizz;

public abstract class SettingsWithPathAndFile
{
    protected string Path { get; init; }
    protected string FileName { get; init; }

    protected Settings Settings { get; init; }

    protected SettingsWithPathAndFile(string path, string file, Settings settings)
    {
        Path = path;
        FileName = file;
        Settings = settings;
    }

    public string GetOriginalPathAndFileName()
    {
        return System.IO.Path.Combine(Path, FileName);
    }
}

public abstract class FileBase : SettingsWithPathAndFile
{
    protected FileBase(string path, string file, Settings settings)
        : base(path, file, settings)
    {
    }

    protected string GetToolReportJsonFileName()
    {
        return Path + Settings.ToolPath + FileName;
    }

    protected string GetSectionFileName(string sectionNameUnique)
    {
        var fileName = System.IO.Path.GetFileNameWithoutExtension(FileName);
        var escapedSectionNameUnique = FileNameEscaper.Escape(sectionNameUnique);
        var extension = System.IO.Path.GetExtension(FileName);
        return Path + Settings.ToolPath + fileName + "." + escapedSectionNameUnique + extension;
    }
}
