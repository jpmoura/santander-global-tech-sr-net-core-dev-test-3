#addin "nuget:?package=Cake.Coverlet"
#addin nuget:?package=Cake.Sonar
#tool nuget:?package=MSBuild.SonarQube.Runner.Tool
#tool "nuget:?package=ReportGenerator"

var target = Argument("target", "Run");

string projectBaseNamespace = "SantanderGlobalTech.HackerNews";

/*  Test projects relative paths */
string[] testProjectsRelativePaths = new string[]
{
    $"{projectBaseNamespace}.Application.Test",
    $"{projectBaseNamespace}.Api.Test"
};

/*  Test runner configuration */
ConvertableDirectoryPath parentDirectory = Directory(".");
var coverageDirectory = parentDirectory + Directory("Coverage");
var coverageFilename = "coverage_results";
var coverageFileExtension = ".opencover.xml"; // Must be OpenCover format due SonarCloud/SonarQube restrictions
var reportTypes = "HtmlInline_AzurePipelines";
var coverageFilePath = coverageDirectory + File(coverageFilename + coverageFileExtension);
var jsonFilePath = coverageDirectory + File(coverageFilename + ".json");

/*  Sonar configuration */
string sonarAuthToken = Argument<string>("token", "");;

Task("Build")
    .Does(() => {
        DotNetCoreBuild($"{projectBaseNamespace}.sln");
    });

Task("Test")
    .IsDependentOn("Build")
    .Does(() => {
        CoverletSettings coverletSettings = new CoverletSettings
        {
            CollectCoverage = true,
            CoverletOutputDirectory = coverageDirectory,
            CoverletOutputName = coverageFilename
        };

        // In case of only one test project, the final result is already in OpenCover format
        if (testProjectsRelativePaths.Length == 1)
        {
            coverletSettings.CoverletOutputFormat  = CoverletOutputFormat.opencover;
            Coverlet(Directory(testProjectsRelativePaths[0]), coverletSettings);
        }
        else
        {
            // In case of more than one test project then is required to let the intermediary files in the Coverlet JSON custom format so it can be merged automatically
            Coverlet(Directory(testProjectsRelativePaths[0]), coverletSettings);
            coverletSettings.MergeWithFile = jsonFilePath;

            for (int i = 1; i < testProjectsRelativePaths.Length; i++)
            {
                if (i == testProjectsRelativePaths.Length - 1)
                {
                    coverletSettings.CoverletOutputFormat  = CoverletOutputFormat.opencover;
                }

                Coverlet(Directory(testProjectsRelativePaths[i]), coverletSettings);
            }
        }
    });

Task("Report")
    .IsDependentOn("Test")
    .Does(() => {
        var reportSettings = new ReportGeneratorSettings
        {
            ArgumentCustomization = args => args.Append($"-reportTypes:{reportTypes}")
        };

        ReportGenerator(coverageFilePath, coverageDirectory, reportSettings);
    });

Task("Sonar")
    .IsDependentOn("SonarBegin")
    .IsDependentOn("Test")
    .IsDependentOn("SonarEnd");

Task("SonarBegin")
    .Does(() => {
        SonarBegin(new SonarBeginSettings
        {
            // Required params
            Key = "jpmoura_SantanderGlobalTech_NetCoreSenior_Test3",
            Url = "https://sonarcloud.io",
            Login = sonarAuthToken,
            Verbose = true,

            // Custom params
            ArgumentCustomization = args => args
                .Append("/o:jpmoura-github")
                .Append($"/d:sonar.cs.opencover.reportsPaths={coverageDirectory}\\{coverageFilename}{coverageFileExtension}")
        });
    });

Task("SonarEnd")
    .Does(() => {
        SonarEnd(new SonarEndSettings
        {
            Login = sonarAuthToken,
        });
    });

Task("Run")
    .IsDependentOn("Test")
    .Does(() => {
        DotNetCoreRun($"{projectBaseNamespace}.Api.csproj");
    });

RunTarget(target);
