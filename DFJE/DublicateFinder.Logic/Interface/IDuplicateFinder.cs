using System.Collections.Generic;
using DuplicateFinder.Logic.Model;

namespace DuplicateFinder.Logic.Interface
{
    public interface IDuplicateFinder
    {

        IEnumerable<IDuplicate> CollectCandidates(string p, CompareMode m);
        
        IEnumerable<IDuplicate> CheckCandidates(IEnumerable<IDuplicate> dups);
    }
}