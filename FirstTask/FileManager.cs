﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Serilog;

namespace FirstTask
{
    public class FileManager : IFileManager
    {
        private readonly ILogger _logger;

        public FileManager(ILogger logger)
        {
            _logger = logger;
        }

        public void CopyTo(string sDir, string tDir)
        {
            DirectoryInfo dirInfo = new DirectoryInfo(tDir);
            if (!dirInfo.Exists)
            {
                    dirInfo.Create();
                    _logger.Debug("directory " + tDir + " created successfully");
            }

            dirInfo = new DirectoryInfo(sDir);
            foreach (FileInfo file in dirInfo.GetFiles("*.*"))
            {
                try
                {
                    File.Copy(file.FullName, Path.Combine(tDir, file.Name), true);
                    _logger.Debug("File copy " + file.FullName + " successfully completed");
                }

                catch(Exception ex)
                {
                    _logger.Error("The process error: {0}", ex.Message);
                }
            }

            foreach (var dir in Directory.GetDirectories(sDir))
            {
                CopyTo(dir, tDir + "\\" + Path.GetFileName(dir));
            }
        }

        public void Process(string sourceDirectoy, string targetDirectory)
        {
            var dirInfo = new DirectoryInfo(targetDirectory);
            if (!dirInfo.Exists)
            {
                _logger.Information(targetDirectory + " is not exists");
                try
                {
                    dirInfo.Create();
                    _logger.Information("directory " + targetDirectory + " created successfully");
                }
                catch(Exception e)
                {
                    throw e;
                }
            }

            string currentFolder = Path.GetFileName(sourceDirectoy) + DateTime.Now.ToString(" [yyyy-M-dd-H-mm]");

            _logger.Debug("Start backup. Directory name: " + currentFolder);


            dirInfo.CreateSubdirectory(currentFolder);

            _logger.Information("subbdirectory " + currentFolder + " created successfully");


            try
            {
                CopyTo(sourceDirectoy, targetDirectory + "\\" + currentFolder);
            }
            catch(Exception e)
            {

            }
        }
    }
}
