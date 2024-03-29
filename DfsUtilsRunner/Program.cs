﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DHI.DFS.Utilities;

namespace DHI.DFS.Utilities.Runner
{
    class Program
    {
        static void Main(string[] args)
        {
            if (!_VerifyArgs(args))
            {
                _PrintUsage();
                Environment.Exit(-1);
            }

            var tool = _GetTool(args[0]);

            switch (tool)
            {
                case DfsTool.Scale:
                    _RunScaleTool(args);
                    break;
                case DfsTool.AddConstant:
                    _RunAddConstantTool(args);
                    break;
                case DfsTool.Sum:
                    _RunSumTool(args);
                    break;
                case DfsTool.Diff:
                    _RunDiffTool(args);
                    break;
                case DfsTool.ExtractTimeSteps:
                    _RunExtractStepsTool(args);
                    break;
                case DfsTool.Flatten:
                    _RunFlattenTool(args);
                    break;
                default:
                    break;
            }
        }

        private static void _RunScaleTool(string[] args)
        {            
            if (args.Count() < 4)
            {   
                Console.WriteLine(">DfsUtils Scale infile.dfsu 0.9 outfile.dfsu");
                throw new ArgumentException("Scale needs 3 arguments");
            }
            var infile = args[1];
            var scale = Convert.ToDouble(args[2]);
            var outfile = args[3];

            var scaler = new DfsScale();
            scaler.Run(infile, scale, outfile);            
        }

        private static void _RunAddConstantTool(string[] args)
        {
            if (args.Count() < 4)
            {
                Console.WriteLine(">DfsUtils AddConstant infile.dfsu 7 outfile.dfsu");
                throw new ArgumentException("AddConstant needs 3 arguments");
            }
            var infile = args[1];
            var cnst = Convert.ToDouble(args[2]);
            var outfile = args[3];

            var constantAdder = new DfsAddConstant();
            constantAdder.Run(infile, cnst, outfile);
        }

        private static void _RunSumTool(string[] args)
        {
            if (args.Count() < 4)
            {
                Console.WriteLine(">DfsUtils Sum file1.dfsu file2.dfsu outfile.dfsu");
                throw new ArgumentException("Sum needs 3 arguments");
            }
            var file1 = args[1];
            var file2 = args[2];
            var outfile = args[3];

            var adder = new DfsAdd();
            adder.Run(file1, file2, outfile);
        }

        private static void _RunDiffTool(string[] args)
        {
            if (args.Count() < 4)
            {
                Console.WriteLine(">DfsUtils Diff infile.dfsu 0.9 outfile.dfsu");
                throw new ArgumentException("Diff needs 3 arguments");
            }
            var file1 = args[1];
            var file2 = args[2];
            var outfile = args[3];

            var differ = new DfsDiff();
            differ.Run(file1, file2, outfile);
        }

        private static void _RunExtractStepsTool(string[] args)
        {
            if (args.Count() < 5)
            {
                Console.WriteLine(">DfsUtils ExtractSteps infile.dfsu outfile.dfsu start end [stride]");
                Console.WriteLine(">DfsUtils ExtractSteps infile.dfsu outfile.dfsu 10 -1 2");
                throw new ArgumentException("ExtractSteps needs 4 or 5 arguments");
            }
            var infile = args[1];            
            var outfile = args[2];
            var startstep = Convert.ToInt32(args[3]);
            var endstep = Convert.ToInt32(args[4]);
            if (args.Count() == 6)
            {
                var stride = Convert.ToInt32(args[5]);
                DfsTimeStepsExtractor.Extract(infile, outfile, startstep, endstep, stride);
            }
            else
            {
                DfsTimeStepsExtractor.Extract(infile, outfile, startstep, endstep);
            }
        }

        private static void _RunFlattenTool(string[] args)
        {
            throw new NotImplementedException();
        }

        private static bool _VerifyArgs(string[] args)
        {
            if (args.Count() < 3)
            {
                Console.WriteLine("Too few arguments");
                return false;
            }
            DfsTool tool;
            try
            { 
                tool = _GetTool(args[0]);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
            return true;
        }

        private static void _PrintUsage()
        {
            Console.WriteLine("Usage:");
            Console.WriteLine(">DfsUtils [toolname] [args]");
            Console.WriteLine(">DfsUtils Scale infile.dfsu 0.9 outfile.dfsu");
            Console.WriteLine(">DfsUtils Sum file1.dfs2 file2.dfs2 outfile.dfs2");
            Console.WriteLine(">DfsUtils Diff file1.dfs2 file2.dfs2 outfile.dfs2");
        }

        private static DfsTool _GetTool(string arg0)
        {
            var toolName = arg0;
            switch (toolName.ToLower())
            {
                case "scale":
                    return DfsTool.Scale;
                case "addconstant":
                    return DfsTool.AddConstant;                    
                case "add": case "sum":
                    return DfsTool.Sum;
                case "diff":
                    return DfsTool.Diff;
                case "extracttimesteps": case "extractsteps":
                    return DfsTool.ExtractTimeSteps;
                case "flatten":
                case "timeaverage": 
                    return DfsTool.Flatten;                    
                default:
                    throw new Exception("No such tool: " + arg0);
            }
        }

    }

    enum DfsTool { Scale, AddConstant, Sum, Diff, ExtractTimeSteps, Flatten}

}
