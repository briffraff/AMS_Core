using AMS2_core.Core.Interface;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Reflection.Metadata.Ecma335;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AMS2_core.Core
{
    public class Service : IAMService
    {
        public void CheckAndResetWindowSize()
        {
            if (Console.WindowHeight != 45 || Console.WindowWidth != 60)
            {
                Console.SetWindowSize(60, 45);
                //Console.SetBufferSize(60, 45);
            }
        }

        // check where is located the .exe file
        public string CheckExeLocation()
        {
            var exelocation = Assembly.GetEntryAssembly().Location;
            return exelocation;
        }

        // pass the exe location 
        public string GetExeDirectory(string location)
        {
            var directory = Path.GetDirectoryName(location) + "\\";
            return directory;
        }

        //  ENUMERATION OPTIONS
        public EnumerationOptions EnumOptions()
        {
            var options = new EnumerationOptions()
            {
                IgnoreInaccessible = true,
                RecurseSubdirectories = true
            };

            return options;
        }

        public void isApproved(string answer)
        {
            answer = answer.ToLower();

            this.ChangeTextColor("White");

            while (answer != null)
            {
                if (answer == "y")
                {
                    break;
                }
                else if (answer == "n")
                {
                    Environment.Exit(0);
                }
                else
                {
                    this.ChangeTextColor("Red");
                    Console.WriteLine("*You must say 'Y' or 'N' !");

                    this.ChangeTextColor("White");
                    answer = Console.ReadLine().ToLower();
                }
            }

        }

        public void ChangeTextColor(string color)
        {
            Console.ForegroundColor = (ConsoleColor)Enum.Parse(typeof(ConsoleColor), color);
        }

        public void ChangeColor(string color)
        {
            Console.BackgroundColor = (ConsoleColor)Enum.Parse(typeof(ConsoleColor), color);
        }

        public void TransferResultFilesToFolder(string source, string destination)
        {
            //transfer result files to script folder
            if (File.Exists(destination + "[Geometries].txt"))
            {
                File.Delete(destination + "[Geometries].txt");
                File.Copy(source + "[Geometries].txt", destination + "[Geometries].txt", true);
            }
            else
            {
                File.Copy(source + "[Geometries].txt", destination + "[Geometries].txt", true);
            }

            if (File.Exists(destination + "[Assigner-OLD].txt"))
            {
                File.Delete(destination + "[Assigner-OLD].txt");
                File.Copy(source + "[Assigner-OLD].txt", destination + "[Assigner-OLD].txt", true);
            }
            else
            {
                File.Copy(source + "[Assigner-OLD].txt", destination + "[Assigner-OLD].txt", true);
            }
        }

        public class AssignInfo
        {
            internal int SkuCounter;
            internal int SkuCounterNonGeometry;
            internal int MaxSkus;
            //internal int ColorCodeLenght;
            //internal bool IsEnd;
            internal string Sku;
            //internal string Assigner;

            public AssignInfo(int skuCounter, int skuCounterNonGeometry, int maxSkus, /*int colorCodeLenght, bool isEnd,*/ string sku /*string assigner*/)
            {
                SkuCounter = skuCounter;
                SkuCounterNonGeometry = skuCounterNonGeometry;
                MaxSkus = maxSkus;
                //ColorCodeLenght = colorCodeLenght;
                //IsEnd = isEnd;
                Sku = sku;
                //Assigner = assigner;
            }
        }

        //PASTE THE SKUs
        public AssignInfo Assign(Dictionary<string, string> collectionSkuGarment, StreamWriter geometries, StreamWriter OLDs)
        {
            //Here we start with the inputs ,create file and put the results in it
            int skuCounter = 0;
            int skuCounterNonGeometry = 0;
            int maxSkus = 100000; //max number for skus
            int colorCodeLenght = 4;
            bool isEnd;
            string sku = "";
            string assigner = "";

            while (skuCounter < maxSkus)
            {
                this.ChangeTextColor("White");

                sku = Console.ReadLine();
                string skuInput = sku;
                isEnd = sku == "end!" ? true : false;

                //remove that part from sku "-000" 
                if (sku.Length >= colorCodeLenght)
                {
                    sku = sku.Remove(sku.Length - colorCodeLenght);
                }

                //if input is end! then break
                if (isEnd == true) { break; }

                if (collectionSkuGarment.ContainsKey(sku))
                {
                    this.ChangeTextColor("Green");

                    //write a garments
                    Console.WriteLine($"{collectionSkuGarment[sku]}");
                    geometries.WriteLine(collectionSkuGarment[sku].Remove(collectionSkuGarment[sku].Length - 5));
                    //asinger
                    assigner = "OLD";
                    OLDs.WriteLine(assigner);
                    skuCounter += 1;
                }
                else
                {
                    this.ChangeTextColor("Red");

                    Console.WriteLine("n/a");
                    geometries.WriteLine("n/a");
                    assigner = string.Empty;
                    OLDs.WriteLine(assigner);
                    skuCounterNonGeometry += 1;
                }
            }

            //CLOSE STREAMING
            geometries.Flush(); //transfer data from memory buffer to the disk
            geometries.Close(); // close the file

            OLDs.Flush();
            OLDs.Close();

            AssignInfo info = new AssignInfo(skuCounter, skuCounterNonGeometry, maxSkus, sku);

            return info;
        }

        public string SetPose(Match validFile)
        {
            string pose = "0000";
            string footwear = "FW00";
            string equipment = "EQ00";
            string validatePose = (validFile.Groups["garment"]).ToString(); // get the current psd garment code
            string MWBGUPS = validatePose[3].ToString(); // get the gender or Equipment 
            string specialCase = validatePose[2].ToString();
            string specialCaseZero = validatePose[1].ToString();

            //validate gender and set the pose to the proper gender;
            switch (MWBGUPS)
            {
                case "M":
                    pose = "MA-2";
                    break;

                case "W":
                    pose = "WA-1";
                    string additionalPose = "WL-2";

                    if (specialCaseZero == "0" && specialCase == "1" || specialCaseZero == "0" && specialCase == "3")
                    {
                        pose = additionalPose;
                    }
                    break;

                case "C":
                    pose = "MPS-Mike";
                    break;

                case "D":
                    pose = "WPS-Monica";
                    string additionalPosePS = "WPS-Jane";

                    if (specialCaseZero == "0" && specialCase == "1"
                        || specialCase == "0" && specialCase == "2"
                        || specialCaseZero == "0" && specialCase == "3"
                        || specialCase == "0" && specialCase == "4")
                    {
                        pose = additionalPosePS;
                    }
                    break;

                case "L":
                    pose = "LPS-YAG15";
                    break;

                case "F":
                    pose = "FPS-YAB19";
                    break;

                case "B":
                    pose = "YAB-1";
                    break;

                case "G":
                    pose = "YAG-2";
                    break;

                case "P":
                    {
                        pose = footwear;
                    }

                    break;

                case "T":
                    pose = footwear;
                    break;

                case "U":
                    if (specialCaseZero == "8" && specialCase == "0" && validatePose[0] == 'N')
                    {
                        pose = footwear;
                    }
                    else if (specialCaseZero == "9" && specialCase == "8" && validatePose[0] == 'N')
                    {
                        pose = equipment;
                    }

                    break;

                case "Y":
                    pose = footwear;
                    break;
            }

            return pose;
        }

        public void CheckSeasonSetNewest(Match file, string pose, Dictionary<string, string> dict)
        {
            string getSku = file.Groups["sku"].ToString();
            //get and validate season then compare to existing seasonal mark ,num or geometry. if newest overwrite garment
            string getSeason = file.Groups["season"].ToString();
            string getGarment = string.Format("SM_{0}_{1}_0_|{2}", file.Groups["garment"], pose, getSeason);

            if (!dict.ContainsKey(getSku))
            {
                dict[getSku] = getGarment;
            }
            else
            {
                bool isNewestGeometry = false;
                bool isNewestSeasonMark = false;
                bool isNewestSeasonNum = false;

                int HO = 1;
                int SP = 2;
                int SU = 3;
                int FA = 4;

                //get marks and num of season input
                string seasonMarks = getSeason.Substring(0, 2);
                string seasonNum = getSeason.Substring(2, 2);

                //get marks and num of season from dict
                string getSkuSeasonFromDict = dict[getSku].Split("|", StringSplitOptions.RemoveEmptyEntries)[1];
                string currentSeasonMarks = getSkuSeasonFromDict.Substring(0, 2);
                string currentSeasonNum = getSkuSeasonFromDict.Substring(2, 2);

                //transofrm marks to numbers
                int seasonMarksInNum = 0;
                int currentSeasonMarksInNum = 0;
                
                if (seasonMarks == nameof(HO)) { seasonMarksInNum = HO; }
                if (seasonMarks == nameof(SP)) { seasonMarksInNum = SP; }
                if (seasonMarks == nameof(SU)) { seasonMarksInNum = SU; }
                if (seasonMarks == nameof(FA)) { seasonMarksInNum = FA; }

                if (currentSeasonMarks == nameof(HO)) { currentSeasonMarksInNum = HO; }
                if (currentSeasonMarks == nameof(SP)) { currentSeasonMarksInNum = SP; }
                if (currentSeasonMarks == nameof(SU)) { currentSeasonMarksInNum = SU; }
                if (currentSeasonMarks == nameof(FA)) { currentSeasonMarksInNum = FA; }


                string inputGeometry = file.Groups["garment"].ToString();
                string dictGeometry = dict[getSku].Split("_")[1];

                // if geometry is changed
                if (inputGeometry != dictGeometry)
                {
                    isNewestGeometry = true;
                }

                //compare season Marks nums
                if (seasonMarksInNum > currentSeasonMarksInNum)
                {
                    isNewestSeasonMark = true;
                }

                //compare season nums
                if (Int32.Parse(seasonNum) > Int32.Parse(currentSeasonNum))
                {
                    isNewestSeasonNum = true;
                }

                // if some of marks , nums or geometry are equal true then go overwrite to dict
                if (isNewestSeasonMark || isNewestSeasonNum || isNewestGeometry)
                {
                    dict[getSku] = getGarment;
                }
            }

            this.ChangeTextColor("Gray");
            Console.Write("{0}", file.Groups["sku"]);
            this.ChangeTextColor("White");
            Console.WriteLine(" , SM_{0}_{1}_0_     ->{2}", file.Groups["garment"], pose, getSeason);
        }


        public async Task<HashSet<string>> GetAllFilesParralelAsync(Dictionary<string, List<string>> categoriesInfo, EnumerationOptions options, bool printTracksuits)
        {
            HashSet<string> filesCollection = new HashSet<string>();
            List<Task> tasks = new List<Task>() { };

            foreach (var category in categoriesInfo.Keys.Where(x => !x.EndsWith("20") || ((x == "root") || (x == "temp"))))
            {
                var name = categoriesInfo[category][0];
                var path = categoriesInfo[category][1];
                var searchPattern = categoriesInfo[category][2];

                tasks.Add(Task.Run(() => GetFilesParralelAsync(filesCollection, name, path, searchPattern, options, printTracksuits)));
            }
            await Task.WhenAll(tasks.ToArray());


            List<Task> tracksuitsTasks = new List<Task>() { };
            foreach (var category in categoriesInfo.Keys.Where(x => x.EndsWith("20")))
            {
                var name = categoriesInfo[category][0];
                var path = categoriesInfo[category][1];
                var searchPattern = categoriesInfo[category][2];

                tracksuitsTasks.Add(Task.Run(() => GetFilesParralelAsync(filesCollection, name, path, searchPattern, options, printTracksuits)));
            }
            await Task.WhenAll(tracksuitsTasks.ToArray());


            return filesCollection;
        }

        private async Task GetFilesParralelAsync(HashSet<string> filesCollection, string name, string path, string searchPattern, EnumerationOptions options, bool printTracksuits)
        {
            var printAll = !name.EndsWith("20");

            //// print all results if "printTracksuits" is true; if false print only different than tracksuits;
            if (printAll || printTracksuits)
            {
                this.ChangeTextColor("White");
                Console.WriteLine($"-----------> {name}[{0}] - 0% ");
            }

            var getFiles = await Task.Run(() => Directory.GetFiles(path, searchPattern, options));
            int count = getFiles.Length;

            foreach (var file in getFiles)
            {
                filesCollection.Add(file);
            }

            if (printAll || printTracksuits)
            {
                this.ChangeTextColor("White");
                this.ChangeTextColor("Green");
                Console.WriteLine($"-------------> {name}[{count}] - 100%");
            }
        }
    }
}


