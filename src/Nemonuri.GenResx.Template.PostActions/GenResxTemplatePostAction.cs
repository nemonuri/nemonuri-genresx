using System.Diagnostics.CodeAnalysis;

namespace Nemonuri.GenResx.Template.PostActions;

public partial class GenResxTemplatePostAction() : Task
{
    private const string FallbackAppDesignerFolderPathSegment = "Properties";

    private const string DefaultGenResxPropsFileSubPath = "Nemonuri.GenResx.props";

    [Required]
    public string? MSBuildProjectDirectory {get;set;}

    [Required]
    public string? AppDesignerFolder {get;set;}

    public string? GenResxPropsFileSubPath {get;set;}

    public override bool Execute()
    {
        try
        {
            //--- Check project directory exists ---
            if (!IsDirectoryExists(MSBuildProjectDirectory))
            {
                Log.LogError("Project Directory is not exist. ({0})", MSBuildProjectDirectory);
                return false;
            }
            //---|
            
            //--- Create directory if needed ---
            string? filledAppDesignerFolderPathSegment = AppDesignerFolder;
            if (string.IsNullOrEmpty(filledAppDesignerFolderPathSegment))
            {
                filledAppDesignerFolderPathSegment = FallbackAppDesignerFolderPathSegment;
                Log.LogWarning("Cannot find '{0}' property. This task will use fallback value instead. ({1})", 
                    nameof(AppDesignerFolder),
                    FallbackAppDesignerFolderPathSegment
                    );
            }

            string appDesignerFolderPath = Path.GetFullPath(Path.Combine(MSBuildProjectDirectory, AppDesignerFolder));

            if (!Directory.Exists(appDesignerFolderPath))
            {
                Directory.CreateDirectory(appDesignerFolderPath);
            }
            //---|

            //--- Check file already exists ---
            string genResxPropsFilePath = Path.Combine(appDesignerFolderPath, GetFilledGenResxPropsFileSubPath());
            if (File.Exists(genResxPropsFilePath))
            {
                Log.LogMessage("File '{0}' already exists. Skip this task.", genResxPropsFilePath);
                return true;
            }
            //---|

            //--- Create GenResxProps file ---
            File.WriteAllText(genResxPropsFilePath, CreateGenResxPropsText());
            //---|

            return true;
        }
        catch (Exception e)
        {
            Log.LogErrorFromException(e, showStackTrace:true);
            return false;
        }
    }

    private static bool IsDirectoryExists([NotNullWhen(true)] string? directoryPath)
    {
        if (directoryPath == null) {return false;}

        return Directory.Exists(directoryPath);
    }

    private string GetFilledGenResxPropsFileSubPath() =>
        IsNullOrWhiteSpace(GenResxPropsFileSubPath) ? DefaultGenResxPropsFileSubPath : GenResxPropsFileSubPath;

    private static bool IsNullOrWhiteSpace([NotNullWhen(false)] string? value) => string.IsNullOrWhiteSpace(value);
}
