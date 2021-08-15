using System.Collections.Generic;

namespace DuplicateFinder.Logic.Interface
{
    public interface IDuplicate
    {
        IEnumerable<string> FilePaths { get; }

    }
}