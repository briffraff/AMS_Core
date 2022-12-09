using AMS2_core.Core.Interface;
using AMS2_core.Global;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;

namespace AMS2_core.Core
{
    public class Engine : IEngine
    {
        public void Run()
        {
            ////    INTIALIZING
            Stopwatch watch = new Stopwatch();
            Constants gc = new Constants();
            Service service = new Service();
            Debug debug = new Debug();
            Printing print = new Printing();

            //// Empty hash 
            HashSet<string> allFiles = new HashSet<string>();

            //// Enumeration options
            var options = service.EnumOptions();

            //// Global Constants
            var categoriesInfo = new Dictionary<string, List<string>>()
            {
                //// category name = [category NAME, PATH to folder, EXTENSION TO SEARCH]
                [nameof(gc.root)] = new List<string>() { nameof(gc.root), gc.root, gc.psbPattern },
                [nameof(gc.temp)] = new List<string>() { nameof(gc.temp), gc.temp, gc.psdpattern },
                [gc.MWEQ] = new List<string>() { gc.MWEQ, gc.pathRootMWEQ, gc.psdpattern },
                [gc.MWEQ20] = new List<string>() { gc.MWEQ20, gc.pathRootMWEQ20, gc.jpgpattern },
                [gc.MA] = new List<string>() { gc.MA, gc.pathRootMA, gc.psdpattern },
                [gc.MA20] = new List<string>() { gc.MA20, gc.pathRootMA20, gc.jpgpattern },
                [gc.PS] = new List<string>() { gc.PS, gc.pathRootPlusSize, gc.psdpattern },
                [gc.PS20] = new List<string>() { gc.PS20, gc.pathRootPlusSize20, gc.jpgpattern },
                [gc.YA] = new List<string>() { gc.YA, gc.pathRootYA, gc.psdpattern },
                [gc.YA20] = new List<string>() { gc.YA20, gc.pathRootYA20, gc.jpgpattern },
                [gc.YAPS] = new List<string>() { gc.YAPS, gc.pathRootYAPS, gc.psdpattern },
                [gc.YAPS20] = new List<string>() { gc.YAPS20, gc.pathRootYAPS20, gc.jpgpattern },
                [gc.FTW] = new List<string>() { gc.FTW, gc.pathRootFTW, gc.psdpattern },
            };
            
            ////  DEBUG - check categoriesInfo object
            //debug.CheckCategoriesInfoObject(categoriesInfo);

            string exeLocation = service.CheckExeLocation();
            string exeTemp = service.GetExeDirectory(exeLocation);

            string geometryToSheetPath = gc.geometryToSheetPath;

            service.CheckAndResetWindowSize();

            if (Console.BackgroundColor == ConsoleColor.Black)
            {
                service.ChangeColor("Black");
                service.ChangeTextColor("Gray");
                Console.Clear();
            }

            //// tool header
            service.ChangeTextColor("Red");
            print.ToolHeader(gc.toolName, gc.toolDescription, gc.toolVersion);
            service.ChangeTextColor("White");
            print.NewLine();
            service.ChangeTextColor("DarkRed");
            ////-------------------INTIALIZING ENDS HERE !!!


            ////    SCAN THE FILES
            ////Idle message
            Console.WriteLine("COLLECTING PROCESS ...\n");
            watch.Reset();
            watch.Start();
            print.NewLine();

            ////Parralel async - get files
            var files = service.GetAllFilesParralelAsync(categoriesInfo, options, true);

            while (true)
            {
                var isCompleted = files.IsCompleted;
                if (!isCompleted)
                {
                    continue;
                }
                break;
            }

            allFiles.UnionWith(files.Result);

            print.NewLine();
            Console.WriteLine("--COLLECTING PROCESS FINISHED !");
            ////-------------------SCAN ENDS HERE !!!


            ////    PRINT SCAN
            service.ChangeTextColor("Yellow");
            watch.Stop();
            print.NewLine();
            print.PrintingScanResults(watch, allFiles);
            watch.Reset();
            print.NewLine();
            ////-------------------PRINT SCAN INFO ENDS HERE !!!


            //// Q?=>
            service.ChangeTextColor("Yellow");
            print.NewLine();
            Console.WriteLine("DO YOU WANT TO UPDATE THE 'DATABASE' FILE !? - (y / n)");
            service.ChangeTextColor("White");
            string answerInput = Console.ReadLine();
            service.isApproved(answerInput);
            ////----------Q?=>>


            ////    UPDATE DATABASE
            ////create dictionary ,split rows to indexes and put them to library
            Dictionary<string, string> dict = new Dictionary<string, string>();

            ////work with allFiles 
            foreach (var fileWithPath in allFiles)
            {
                ////regex - the following one will catch all the Valid files (ex:Psd's)
                ////new regex - update whole sku part => color group added ,also -|_ between sku group and color group
                var fileRegex = gc.fileRegex;
                var matchedFiles = Regex.Matches(fileWithPath, fileRegex);

                foreach (Match file in matchedFiles)
                {
                    string pose = service.SetPose(file);
                    service.CheckSeasonSetNewest(file, pose, dict);
                }
            }

            print.NewLine();
            service.ChangeTextColor("Green");
            Console.WriteLine("--DONE ! 'DATABASE' FILE IS UPDATED !");
            ////-------------------DATA UPDATE ENDS HERE !!!


            //// Q?=>
            service.ChangeTextColor("Yellow");
            print.NewLine();
            Console.WriteLine("ARE YOU GOING TO ASSIGN SOME GEOMETRY !? - (y / n)");
            service.ChangeTextColor("White");
            answerInput = Console.ReadLine();
            service.isApproved(answerInput);
            ////----------Q?=>>


            ////    ASSIGN THE GEOMETRIES
            ////PASTE THE SKUs
            Console.WriteLine("-> SKUS :");

            StreamWriter sw = new StreamWriter(exeTemp + "[Geometries].txt");
            StreamWriter OLDs = new StreamWriter(exeTemp + "[Assigner-OLD].txt");

            var assign = service.Assign(dict, sw, OLDs);

            int skuCounter = assign.SkuCounter;
            int skuCounterNonGeometry = assign.SkuCounterNonGeometry;
            int maxSkus = assign.MaxSkus; //max number for skus
            string sku = assign.Sku;
            ////-------------------ASSIGNING ENDS HERE !!!


            ////    TRANSFER result files to script folder
            service.TransferResultFilesToFolder(exeTemp, geometryToSheetPath);
            ////-------------------TRANSFER ENDS HERE !!!


            ////    PRINTING
            if (skuCounter == maxSkus)
            {
                print.ReachOutTheLimits(maxSkus, sku, dict);
            }
            print.PrintingResults(skuCounter, skuCounterNonGeometry);
            ////-------------------PRINTING ENDS HERE !!!

        }
    }
}
