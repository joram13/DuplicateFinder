//import neccessary packages
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using DuplicateFinder.Logic.Interface;
using DuplicateFinder.Logic.Model;
using System.Linq;


namespace DuplicateFinder.Logic
{
    public class DuplicateFinder : IDuplicateFinder
    //class used too find duplicates with finding method and helper method to collect files
    {
        public IEnumerable<IDuplicate> CollectCandidates(string filePath, CompareMode m)
        /*method to collect duplicate condidates:
         * takes a file path as a string and a compare method as input and returns the duplicates based on the method declared
         * search uses quick sort and constant interations, so has an average time complexity of O(nlog(n))
         */
        {
            //iniciate duplicate list
            List<Duplicate> dups = new List<Duplicate>();
            //get myFile list with all files in path
            var directInfo = new DirectoryInfo(filePath);
            List<FileInfo> myFiles = DeepDirectorySearch(directInfo);

            //check if Mode is Size
            if (m == CompareMode.Size)
            {
                //Sort files by length
                myFiles.Sort((a, b) => a.Length.CompareTo(b.Length));

                //iterate through sorted list of files
                for (int i = 0; i < myFiles.Count - 1; i++)
                {
                    //start adding duplicates to path list
                    var paths = new List<string>();
                    paths.Add(myFiles[i].FullName);

                    //while we have duplicates we add them to path list and increase count
                    while (i + 1 <= myFiles.Count - 1 && myFiles[i].Length == myFiles[i + 1].Length)
                    {
                        paths.Add(myFiles[i + 1].FullName);
                        i += 1;


                    }

                    //if we have duplicates in paths list we add them to Duplicate list
                    if (paths.Count > 1)
                    {
                        dups.Add(new Duplicate(paths));
                    }
                }
            }

            //check if Mode is Name
            if (m == CompareMode.Name)
            {
                //Sort files by Name
                myFiles.Sort((a, b) => a.Name.CompareTo(b.Name));

                //iterate through sorted list of files
                for (int i = 0; i < myFiles.Count - 1; i++)
                {
                    //start adding duplicates to path list
                    var paths = new List<string>();
                    paths.Add(myFiles[i].FullName);

                    //while we have duplicates we add them to path list and increase count
                    while (i + 1 <= myFiles.Count - 1 && myFiles[i].Name == myFiles[i + 1].Name)
                    {
                        paths.Add(myFiles[i + 1].FullName);
                        i += 1;


                    }

                    //if we have duplicates in paths list we add them to Duplicate list
                    if (paths.Count > 1)
                    {
                        dups.Add(new Duplicate(paths));
                    }
                }
            }

            //check if Mode is Size and Name
            if (m == CompareMode.SizeAndName)
            {
                //Sort files by length
                myFiles.Sort((a, b) => a.Length.CompareTo(b.Length));

                //iterate through sorted list of files
                for (int i = 0; i < myFiles.Count - 1; i++)
                {

                    //create name list that stores duplicates based on size and still has to be checkd for names
                    var name = new List<FileInfo>();
                    name.Add(myFiles[i]);

                    //while we have matches on length we add them to names list and increase count
                    while (i + 1 <= myFiles.Count - 1 && myFiles[i].Length == myFiles[i + 1].Length)
                    {
                        name.Add(myFiles[i + 1]);
                        i += 1;
                    }

                    //we sort the name list after names and iterate through it 
                    name.Sort((a, b) => a.Name.CompareTo(b.Name));

                    for (int j = 0; j < name.Count - 1; j++)
                    {
                        //we create the paths list and start adding to it
                        var paths = new List<string>();
                        paths.Add(name[j].FullName);

                        //we check for matches on name and add them to paths
                        while (j + 1 <= name.Count - 1 && name[j].Name == name[j + 1].Name)
                        {
                            paths.Add(name[j + 1].FullName);
                            j += 1;
                        }

                        //if we have duplicates we add them to Duplicates
                        if (paths.Count > 1)
                        {
                            dups.Add(new Duplicate(paths));


                        }
                    }
                }
            }
            return dups;
        }

        public IEnumerable<IDuplicate> CheckCandidates(IEnumerable<IDuplicate> duplicates)
        /*method to check if potential condidates are duplicates:
         * takes Duplicate object as input and returns Duplicates based on hash
         */
        {
            //create Dictionary for hashes
            var duplicatesByHash = new Dictionary<string, List<string>>();
            //iterate through all potential duplicates set
            foreach (var duplicate in duplicates)
            {
                //iterate throu each filepath in set
                foreach (var filePath in duplicate.FilePaths)
                {
                    //calculate hash 
                    var md5Provider = new MD5CryptoServiceProvider();
                    var hash = BitConverter.ToString(md5Provider.ComputeHash(File.ReadAllBytes(filePath)));
                    //if hash value appears the first time add it as a new key and the file path as value
                    if (duplicatesByHash.ContainsKey(hash))
                        duplicatesByHash[hash].Add(filePath);
                    //otherwise add new file path as new value to hash key
                    else
                    {
                        duplicatesByHash.Add(hash, new List<string>());
                        duplicatesByHash[hash].Add(filePath);
                    }
                }
            }

            //iterate over all hash values and add them to selectedDuplicates if we found them more than once
            var selectedDuplicates = new List<IDuplicate>();
            foreach (var duplicateByHash in duplicatesByHash)
                if (duplicateByHash.Value.Count > 1)
                    selectedDuplicates.Add(new Duplicate(duplicateByHash.Value));
            //return duplicates
            return selectedDuplicates;
        }

            public List<FileInfo> DeepDirectorySearch(DirectoryInfo rootDir)
            /* method takes root directory as input and outputs a list of all files 
             * uses a queue and breadth first search to get all files
             */
        {
            //initiate list of lies and list of direcotries that is used as a queue 
            List<FileInfo> files = new List<FileInfo>();
            List<DirectoryInfo> dirs = new List<DirectoryInfo>() { rootDir };

            //search as long as we have directories
            while (dirs.Count > 0)
            {
                //pop first directory
                DirectoryInfo currentDir = dirs[0];
                dirs.RemoveAt(0);
                //add files of current dirctory to files add new found directories to queue
                files.AddRange(currentDir.GetFiles());
                dirs.AddRange(currentDir.GetDirectories());



            }
            //return fiels
            return files;
        }
    }
}
