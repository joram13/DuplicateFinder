using System.Collections.Generic;
using DuplicateFinder.Logic.Interface;

namespace DuplicateFinder.Logic.Model
{
    public class Duplicate : IDuplicate
    {
        public IEnumerable<string> FilePaths { get; }
        
        public Duplicate(IEnumerable<string> filePaths)
        {
            this.FilePaths = filePaths;
        }
    }
}