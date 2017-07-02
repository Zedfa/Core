using Core.Cmn;
using Microsoft.Build.Evaluation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Web.Configuration;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;

namespace Core.WebClientModuleManagement
{
    internal class ResourceManagementBase
    {
        private static BundleCollection _bundles = BundleTable.Bundles;
        private static ResolveConflictsForm _resolveConflictsForm;
        private static List<ResourceInfo> _resourceInfoList;
        public static List<ResourceInfo> _conflictResourceInfoList;
        private static object _lock = new object();
        private static string _webClientModuleManagementValue;
        private static string _filesVersionInfoPath = Path.GetFullPath(AppDomain.CurrentDomain.BaseDirectory + "FilesVersionInfo.txt");
        private static readonly IList<string> LoadablExtensions = new ReadOnlyCollection<string>(new List<string> { ".ts", ".js", ".css", ".jpg", ".jpeg", ".gif", ".png", ".cshtml", ".config",
            ".mrt",".ttf",".otf",".woff",".woff2",".eot",".svg" });
        private static Project _currentProject;
        private static List<SubProjectSpec> _subProjectList = new List<SubProjectSpec>();
        private static List<FileSystemWatcher> _fileWatcherList = new List<FileSystemWatcher>();



        internal static void Start()
        {


            bool enableWebClientModuleManagement = Core.Cmn.ConfigHelper.GetConfigValue<bool>("EnableWebClientModuleManagement");
            if (enableWebClientModuleManagement)
            {
                StartupProjectConfig.DllName = AppBase.StartupProject.GetName().Name; //Core.Cmn.ConfigHelper.GetConfigValue<string>("StartupProjectForWebClient");
                                                                                      // _currentProject = new Project(AppDomain.CurrentDomain.BaseDirectory + "\\" + StartupProjectConfig.DllName + ".csproj"); 


                _webClientModuleManagementValue = Core.Cmn.ConfigHelper.GetConfigValue<string>("WebClientModuleManagement");
                _resourceInfoList = new List<ResourceInfo>();
                _conflictResourceInfoList = new List<ResourceInfo>();


                SetGlobalConfiguration(AppBase.StartupProject);


                var pathesInfo = _webClientModuleManagementValue.Split(';');
                foreach (var item in pathesInfo)
                {
                    if (String.IsNullOrEmpty(item)) { continue; }

                    var path = item.Split(',');

                    var projectSpec = new SubProjectSpec(path[1].Trim(), path[0].Trim());

                    _subProjectList.Add(projectSpec);
                }


                var enableWebClientModuleManagementInDebugTime = StartupProjectConfig.Debug;
                //#if !DEBUG
                if (!enableWebClientModuleManagementInDebugTime)
                {
                    ServerSideWebClientManagementForReleaseTime(_bundles);
                    return;
                }
                //#endif


                //#if DEBUG

                //var enableWebClientModuleManagementInDebugTime = Core.Cmn.ConfigHelper.GetConfigValue<bool>("EnableWebClientModuleManagementInDebugTime");
                //((System.Web.Configuration.CompilationSection)(System.Configuration.ConfigurationManager.GetSection("system.web/compilation"))).Debug
                else
                //  if (enableWebClientModuleManagementInDebugTime)
                {
                    // WebClientManagementForDebugTime(configedPaths, allFiles);
                    WebClientManagementForDebugTime();

                }
                //#endif
            }
        }

        internal static void SyncFile(ResourceInfo resourceInfo, bool overwriteSource)
        {
            if (overwriteSource)
            {
                var content = TryReadAllBytes(resourceInfo.FullPath);
                TryWriteAllBytes(resourceInfo.SourcePath, resourceInfo.CSProjFilePath, content);
            }
            else
            {
                var content = TryReadAllBytes(resourceInfo.SourcePath);
                TryWriteAllBytes(resourceInfo.FullPath, resourceInfo.CSProjFilePath, content);
            }

            UpdateVersionFiles(resourceInfo);
        }
        internal static string TryReadAllText(string path)
        {
            while (true)
            {
                try
                {
                    var result = File.ReadAllText(path);
                    return result;
                }
                catch (Exception ex)
                {
                    if (ex is ApplicationException)
                    {
                        throw;
                    }
                    else
                    {
                        HandeFileException(ex);
                    }
                }
            }
        }

        #region Private Methods

        static void Changed(object sender, FileSystemEventArgs e)
        {
            var fullPath = e.FullPath.Contains("~") ? e.FullPath.Remove(e.FullPath.IndexOf('~')) : e.FullPath;
            //if (Path.GetExtension(e.FullPath).EndsWith("~") || !File.Exists(fullPath)) { return; }
            if ( !File.Exists(fullPath)) { return; }

            var resourceInfo = _resourceInfoList.FirstOrDefault(r =>
                r.SourcePath.ToLower() == Path.GetFullPath(fullPath).ToLower() ||
                r.FullPath.ToLower() == Path.GetFullPath(fullPath).ToLower());
            if (resourceInfo == null)
            {
                return;
            }
            CheckFiles();
            CheckConflict();
        }
        private static void CheckFiles()
        {
            lock (_lock)
            {
                var versionFile = TryReadAllText(_filesVersionInfoPath);
                Dictionary<string, string> allSourceVersionFiles = new Dictionary<string, string>();
                foreach (var item in _resourceInfoList.GroupBy(r => r.SourceVersionFilePath).Select(r => r.First()))
                {
                    allSourceVersionFiles.Add(item.DllName, TryReadAllText(item.SourceVersionFilePath));
                }

                foreach (var resourceInfo in _resourceInfoList)
                {

                    var sourceVersionFile = allSourceVersionFiles.Where(r => r.Key == resourceInfo.DllName).First().Value;

                    var sourceContent = TryReadAllBytes(resourceInfo.SourcePath);
                    var content = TryReadAllBytes(resourceInfo.FullPath);


                    //پیدا کردن آخرین تاریخ تغییر فایل سورس مورد نظر از فایل ورژن ها
                    FileInfo sourceFileInfo = new FileInfo(resourceInfo.SourcePath);
                    var sourceVersionDate = long.Parse(sourceVersionFile);


                    //پیدا کردن آخرین تاریخ تغییر فایل مورد نظر از فایل ورژن ها
                    FileInfo fileInfo = new FileInfo(resourceInfo.FullPath);
                    var version = Regex.Match(versionFile, String.Format(@"(?i){0}:(\d+)", resourceInfo.DllName)).Groups[1].Value;
                    var versionDate = long.Parse(version);

                    bool sourceFileChanged = sourceFileInfo.LastWriteTime.Ticks > sourceVersionDate;
                    bool fileChanged = fileInfo.LastWriteTime.Ticks > versionDate;
                    bool sourContentMatchAreaFile = sourceContent.SequenceEqual(content);

                    if (!sourceFileChanged && !fileChanged)
                    {

                        if (sourceContent.SequenceEqual(content)) { continue; }

                        //Conflict
                        if (_conflictResourceInfoList.Count(r => r.SourcePath == resourceInfo.SourcePath) == 0)
                        {
                            _conflictResourceInfoList.Add(resourceInfo);
                        }
                    }
                    else if ((sourceFileChanged && !fileChanged) && !sourContentMatchAreaFile)
                    {
                        SyncFile(resourceInfo, false);
                    }
                    else if ((!sourceFileChanged && fileChanged) && !sourContentMatchAreaFile)
                    {
                        SyncFile(resourceInfo, true);
                    }
                    else if (sourceFileChanged && fileChanged)
                    {

                        if (sourceContent.SequenceEqual(content))
                        {
                            UpdateVersionFiles(resourceInfo);
                            continue;
                        }

                        //Conflict
                        if (_conflictResourceInfoList.Count(r => r.SourcePath == resourceInfo.SourcePath) == 0)
                        {
                            _conflictResourceInfoList.Add(resourceInfo);
                        }
                    }

                }

            }
        }
        private static void CheckConflict()
        {
            if (_conflictResourceInfoList.Count > 0 && !_resolveConflictsForm.Visible)
            {
                _resolveConflictsForm.ShowDialog();
            }
        }
        private static byte[] ReadToEnd(System.IO.Stream stream)
        {
            long originalPosition = 0;

            if (stream.CanSeek)
            {
                originalPosition = stream.Position;
                stream.Position = 0;
            }

            try
            {
                byte[] readBuffer = new byte[4096];

                int totalBytesRead = 0;
                int bytesRead;

                while ((bytesRead = stream.Read(readBuffer, totalBytesRead, readBuffer.Length - totalBytesRead)) > 0)
                {
                    totalBytesRead += bytesRead;

                    if (totalBytesRead == readBuffer.Length)
                    {
                        int nextByte = stream.ReadByte();
                        if (nextByte != -1)
                        {
                            byte[] temp = new byte[readBuffer.Length * 2];
                            Buffer.BlockCopy(readBuffer, 0, temp, 0, readBuffer.Length);
                            Buffer.SetByte(temp, totalBytesRead, (byte)nextByte);
                            readBuffer = temp;
                            totalBytesRead++;
                        }
                    }
                }

                byte[] buffer = readBuffer;
                if (readBuffer.Length != totalBytesRead)
                {
                    buffer = new byte[totalBytesRead];
                    Buffer.BlockCopy(readBuffer, 0, buffer, 0, totalBytesRead);
                }
                return buffer;
            }
            finally
            {
                if (stream.CanSeek)
                {
                    stream.Position = originalPosition;
                }
            }
        }
        private static void TryAppendAllText(string path, string content)
        {
            while (true)
            {
                try
                {
                    File.AppendAllText(path, content);
                    break;
                }
                catch (Exception ex)
                {
                    if (ex is ApplicationException)
                    {
                        throw;
                    }
                    else
                    {
                        HandeFileException(ex);
                    }
                }
            }
        }
        private static void TryWriteAllBytes(string path, string projectFilePath, byte[] content)
        {

            while (true)
            {
                try
                {
                    UnAssignWatchersToResources();

                    File.WriteAllBytes(path, content);
                    if (Path.GetExtension(path).Equals(".ts")) {
                        System.Diagnostics.Process.Start(@"C:\Program Files (x86)\Microsoft SDKs\TypeScript\2.2\tsc.exe", path);
                    }
                    foreach (var extension in LoadablExtensions)
                    {


                        if (_currentProject.Items.FirstOrDefault(files => files.EvaluatedInclude == projectFilePath) == null)
                        {

                           
                            switch (Path.GetExtension(path))
                            {
                                case ".ts":

                                    _currentProject.AddItem("TypeScriptCompile", projectFilePath);
                                    break;
                                case ".js":
                                    if ($"{projectFilePath.Split('\\')[2]}\\{projectFilePath.Split('\\')[3]}".ToLower().Equals( "scripts\\lib"))
                                    {
                                        _currentProject.AddItem("Content", projectFilePath);
                                    }
                                    break;
                                case ".dll":
                                    break;
                                default:
                                    _currentProject.AddItem("Content", projectFilePath);
                                    break;
                            }
                        }

                    }

                    AssignWatchersToResources();

                    break;
                }
                catch (Exception ex)
                {
                    if (ex is ApplicationException)
                    {
                        throw;
                    }
                    else
                    {
                        HandeFileException(ex);
                    }
                }
            }
        }
        private static byte[] TryReadAllBytes(string path)
        {
            while (true)
            {
                try
                {
                    var result = File.ReadAllBytes(path);
                    return result;
                }
                catch (Exception ex)
                {
                    if (ex is ApplicationException)
                    {
                        throw;
                    }
                    else
                    {
                        HandeFileException(ex);
                    }
                }
            }
        }
        private static void TryWriteAllText(string path, string content)
        {
            while (true)
            {
                try
                {
                    File.WriteAllText(path, content);
                    break;
                }
                catch (Exception ex)
                {
                    if (ex is ApplicationException)
                    {
                        throw;
                    }
                    else
                    {
                        HandeFileException(ex);
                    }
                }
            }
        }
        private static void HandeFileException(Exception ex)
        {
            if (ex is FileLoadException ||
                ex is IOException ||
                ex is UnauthorizedAccessException)
            {
                System.Threading.Thread.Sleep(500);
            }
            else
            {
                throw ex;
            }
        }
        private static void UpdateVersionFiles(ResourceInfo resourceInfo)
        {
            var sourceFileInfo = new FileInfo(resourceInfo.SourceVersionFilePath);
            var newSourceVersionFile = sourceFileInfo.LastWriteTime.Ticks.ToString();
            TryWriteAllText(resourceInfo.SourceVersionFilePath, newSourceVersionFile);

            var fileInfo = new FileInfo(resourceInfo.FullPath);
            var versionFileContent = TryReadAllText(_filesVersionInfoPath);
            string newValue = Regex.Replace(versionFileContent, String.Format(@"(?i){0}:(\d+)", resourceInfo.DllName),
                                    String.Format(@"{0}:{1}", resourceInfo.DllName, fileInfo.LastWriteTime.Ticks));

            TryWriteAllText(_filesVersionInfoPath, newValue);
        }


        private static void WebClientManagementForDebugTime()
        {
            _resolveConflictsForm = new ResolveConflictsForm();
            _currentProject = new Project(AppDomain.CurrentDomain.BaseDirectory + StartupProjectConfig.DllName + ".csproj");

            foreach (var subProject in _subProjectList)
            {

                //----resources find loadable extensions from sub system(like core.mvc) and then add to web Application(like sepehr360.mvc)---------


                var resourcesPath = Directory.GetFiles(subProject.FullPath, "*.*", SearchOption.AllDirectories)

                                       .Where(file => LoadablExtensions.Any((ext) =>
                                       {
                                           var fileInSubProject = subProject.Project.GetItemsByEvaluatedInclude(file.Replace(subProject.FullPath, ""));
                                           if (fileInSubProject.Count > 0)
                                           {
                                               switch (ext)
                                               {
                                                   case ".config":
                                                       {
                                                           return file.ToLower().EndsWith("views\\web.config");
                                                       }
                                                   default:
                                                       return file.EndsWith(ext);
                                               }
                                           }
                                           var fileInStartupProject = _currentProject.GetItemsByEvaluatedInclude($"{subProject.RelativeAreaPath}{file.Replace(subProject.Project.DirectoryPath + "\\", "")}");

                                           _currentProject.RemoveItems(fileInStartupProject);
                                           return false;
                                       }));

                foreach (var fullPath in resourcesPath)
                {

                    var resourceInfo = new ResourceInfo(subProject, fullPath);
                    _resourceInfoList.Add(resourceInfo);

                    AddOrUpdateResourceByFilesVersionInfo(resourceInfo);
                }

                SetGlobalConfiguration(subProject.Assembly);

                AddResourcesToWebAppBundleTable(subProject.Assembly, subProject.RelativeAreaPath);

            }

            if (_resourceInfoList.Count == 0)
            {
                return;
            }

            CheckFiles();
            AssignWatchersToResources();
            CheckConflict();
            _currentProject.Save();
        }
        private static void AssignWatchersToResources()
        {
            var fswCurrent = new FileSystemWatcher(Path.GetFullPath(String.Format("{0}{1}", AppDomain.CurrentDomain.BaseDirectory, "Areas")));
            fswCurrent.IncludeSubdirectories = true;
            fswCurrent.EnableRaisingEvents = true;
            fswCurrent.Changed += Changed;
            fswCurrent.Created += Changed;
            fswCurrent.Deleted += Changed;
            fswCurrent.Renamed += Changed;

            _fileWatcherList.Add(fswCurrent);

            foreach (var project in _subProjectList)
            {
                var fswSource = new FileSystemWatcher(project.FullPath);
                fswSource.IncludeSubdirectories = true;
                fswSource.EnableRaisingEvents = true;
                fswSource.Changed += Changed;
                fswCurrent.Created += Changed;
                fswCurrent.Deleted += Changed;
                fswCurrent.Renamed += Changed;

                _fileWatcherList.Add(fswSource);

            }
        }
        private static void UnAssignWatchersToResources()
        {

            foreach (var watcher in _fileWatcherList.ToList())
            {

                watcher.IncludeSubdirectories = true;
                watcher.EnableRaisingEvents = false;
                watcher.Changed -= Changed;
                watcher.Created -= Changed;
                watcher.Deleted -= Changed;
                watcher.Renamed -= Changed;
                watcher.Dispose();
                _fileWatcherList.Remove(watcher);
            }
        }

        private static void AddOrUpdateResourceByFilesVersionInfo(ResourceInfo resourceInfo)
        {

            //ثبت فایل ورژن ها در سورس برای اولین بار
            FileInfo sourceFileInfo = new FileInfo(resourceInfo.SourcePath);
            if (!File.Exists(resourceInfo.SourceVersionFilePath))
            {
                TryAppendAllText(resourceInfo.SourceVersionFilePath, sourceFileInfo.LastWriteTime.Ticks.ToString());
            }
            else
            {
                //پیدا کردن فایل مورد نظر در فایل ورژن
                var sourceVersion = TryReadAllText(resourceInfo.SourceVersionFilePath);
                long lastModifyDate;
                if (long.TryParse(sourceVersion, out lastModifyDate))
                {
                    if (sourceFileInfo.LastWriteTime.Ticks > lastModifyDate)
                    {
                        TryWriteAllText(resourceInfo.SourceVersionFilePath, sourceFileInfo.LastWriteTime.Ticks.ToString());
                    }
                }
                else
                {
                    TryWriteAllText(resourceInfo.SourceVersionFilePath, sourceFileInfo.LastWriteTime.Ticks.ToString());
                }
            }

            FileInfo fileInfo = new FileInfo(resourceInfo.FullPath);
            if (!File.Exists(resourceInfo.FullPath))
            {
                fileInfo.Directory.Create();
                var content = TryReadAllBytes(resourceInfo.SourcePath);
                TryWriteAllBytes(resourceInfo.FullPath, resourceInfo.CSProjFilePath, content);
            }

            //ثبت فایل ورژن ها در پروژه برای اولین بار
            if (!File.Exists(_filesVersionInfoPath))
            {
                TryAppendAllText(_filesVersionInfoPath, String.Format("{0}:{1}{2}", resourceInfo.DllName, fileInfo.LastWriteTime.Ticks, Environment.NewLine));
            }
            else
            {
                //پیدا کردن فایل مورد نظر در فایل ورژن
                var versionFileContent = TryReadAllText(_filesVersionInfoPath);
                var version = Regex.Match(versionFileContent, String.Format(@"(?i){0}:(\d+)", resourceInfo.DllName)).Groups[1].Value;
                long lastModifyDate;
                if (long.TryParse(version, out lastModifyDate))
                {
                    if (fileInfo.LastWriteTime.Ticks > lastModifyDate)
                    {
                        string newValue = Regex.Replace(versionFileContent, String.Format(@"(?i){0}:(\d+)", resourceInfo.DllName),
                            String.Format(@"{0}:{1}", resourceInfo.DllName, fileInfo.LastWriteTime.Ticks));
                        TryWriteAllText(_filesVersionInfoPath, newValue);
                    }
                }
                else
                {
                    TryAppendAllText(_filesVersionInfoPath, String.Format("{0}:{1}{2}", resourceInfo.DllName, fileInfo.LastWriteTime.Ticks, Environment.NewLine));
                }
            }

        }

        private static void ServerSideWebClientManagementForReleaseTime(BundleCollection bundles)
        {

            foreach (var subProject in _subProjectList)
            {
                SetGlobalConfiguration(subProject.Assembly);

                AddResourcesToWebAppBundleTable(subProject.Assembly, subProject.RelativeAreaPath);
            }
        }
        private static void SetGlobalConfiguration(Assembly assembly)
        {
            FilterApiConfig.RegisterHttpFilters(GlobalConfiguration.Configuration.Filters, assembly);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters, assembly);


        }
        private static void AddResourcesToWebAppBundleTable(Assembly assembly, string relativePath)
        {
            var nameSpace = assembly.GetName().Name;
            var bundleConfigClass = assembly.GetType(string.Format("{0}.BundleConfig", nameSpace));
            if (bundleConfigClass != null)
            {
                var registerBundleInstance = Activator.CreateInstance(bundleConfigClass);
                var registerBundlesMethod = bundleConfigClass.GetMethod("RegisterBundles");
                if (registerBundlesMethod != null)
                {
                    var relativeBundlePath = string.Format("~/{0}", relativePath.Replace("\\", "/"));
                    registerBundlesMethod.Invoke(registerBundleInstance, new object[] { _bundles, relativeBundlePath });
                }
            }
        }


        #endregion
    }
}
