using System.IO;

namespace CroquetAustralia.WebApi.Specifications.Configuration
{
    public class SolutionConfiguration
    {
        public string Directory
        {
            get
            {
                var currentDirectory = System.IO.Directory.GetCurrentDirectory();
                var solutionDirectory = Path.GetFullPath(Path.Combine(currentDirectory, @"..\..\..\..\"));

                return solutionDirectory;
            }
        }
    }
}