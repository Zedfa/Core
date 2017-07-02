using Core.Mvc.Helpers.CustomWrapper.DataSource;
using Core.Mvc.Helpers.CustomWrapper.Infrastructure;
using Core.Mvc.Helpers.CoreKendoGrid.Infrastructure;
using Core.Mvc.Helpers.CoreKendoGrid.Settings.ColumnConfig;
using Kendo.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Mvc.Helpers.CoreKendoGrid
{
    [Serializable()]
    public class Toolbar : JsonObjectBase
    {
        internal AccessOperation CRUDOperation { get; set; }

        public List<ColumnCommand> Commands { get; set; }

        //internal bool ExportToExcel { get; set; }
        //internal bool ExportToPdf { get; set; }
        public Toolbar()
        {
            CRUDOperation = new AccessOperation();

            Commands = GetDefaultCommandList();

        }
        public Toolbar(List<ColumnCommand> commandColumns)
        {
            CRUDOperation = new AccessOperation();

            Commands = GetDefaultCommandList(commandColumns);

        }



        private List<ColumnCommand> GetDefaultCommandList(List<ColumnCommand> commandColumns = null)
        {
            //var commands = commandColumns ?? new List<ColumnCommand>();
            List<ColumnCommand> customCommands = null;
            var commands = new List<ColumnCommand>();
            if (commandColumns != null)
            {
                customCommands = commandColumns.Where(custom => custom.Name == GCommandCr.Custom).ToList();
                commands = commandColumns.Where(custom => custom.Name != GCommandCr.Custom).ToList() ;

            }



            if (CRUDOperation.ReadOnly)
            {
                //return commands.Count > 0 ? new List<ColumnCommand>() : commands;
                commands = commands.Count > 0 ? new List<ColumnCommand>() : commands;

            }
            else
            {
                if (commands.Count() == 0)
                {
                    //var commandDic = new Dictionary<GCommandCr, string>();

                    if (CRUDOperation.Insertable)
                    {
                        commands.Add(new ColumnCommand { Name = GCommandCr.Create, Text = "جدید" });
                    }
                    if (CRUDOperation.Updatable)
                    {
                        commands.Add(new ColumnCommand { Name = GCommandCr.Edit, Text = "ویرایش" });
                    }
                    if (CRUDOperation.Removable)
                    {
                        commands.Add(new ColumnCommand { Name = GCommandCr.Delete, Text = "حذف" });
                    }


                    if (CRUDOperation.Search)
                    {
                        commands.Add(new ColumnCommand { Name = GCommandCr.Search, Text = "جستجو" });
                        commands.Add(new ColumnCommand { Name = GCommandCr.RemoveFilter, Text = "حذف جستجو" });

                    }
                    //if (ExportToExcel)
                   // {
                        commands.Add(new ColumnCommand { Name = GCommandCr.Excel, Text = "Excel" });

                   // }
                    //if (ExportToPdf)
                   // {
                        commands.Add(new ColumnCommand { Name = GCommandCr.Pdf, Text = "PDF" });

                   // }
                    if (CRUDOperation.Refreshable)
                    {
                        commands.Add(new ColumnCommand { Name = GCommandCr.Refresh, Text = "" });

                    }

                    //if (CRUDOperation.Insertable && CRUDOperation.Updatable && CRUDOperation.Removable)
                    //{
                    //    commands = new List<ColumnCommand> { new ColumnCommand { Name = GCommandCr.Create , Text = "جدید" },
                    //                                         new ColumnCommand { Name = GCommandCr.Edit , Text = "ویرایش" },
                    //                                         new ColumnCommand { Name = GCommandCr.Delete , Text = "حذف"  } ,
                    //                                         new ColumnCommand { Name = GCommandCr.Refresh , Text = ""  } ,
                    //    };
                    //}
                    //else if (CRUDOperation.Insertable && !CRUDOperation.Updatable && CRUDOperation.Removable )
                    //{
                    //    commands = new List<ColumnCommand> { new ColumnCommand { Name = GCommandCr.Create , Text = "جدید" },
                    //                                     new ColumnCommand { Name = GCommandCr.Delete , Text = "حذف"  }};
                    //}
                    //else if (CRUDOperation.Insertable && CRUDOperation.Updatable && !CRUDOperation.Removable)
                    //{
                    //    commands = new List<ColumnCommand> { new ColumnCommand { Name = GCommandCr.Create , Text = "جدید" },
                    //                                     new ColumnCommand { Name = GCommandCr.Edit , Text = "ویرایش" }};
                    //}
                    //else if (!CRUDOperation.Insertable && CRUDOperation.Updatable && CRUDOperation.Removable)
                    //{
                    //    commands = new List<ColumnCommand> { new ColumnCommand { Name = GCommandCr.Edit , Text = "ویرایش" },
                    //                                     new ColumnCommand { Name = GCommandCr.Delete , Text = "حذف"  }};
                    //}
                    //else if (!CRUDOperation.Insertable && !CRUDOperation.Updatable && CRUDOperation.Removable)
                    //{
                    //    commands = new List<ColumnCommand> { new ColumnCommand { Name = GCommandCr.Delete, Text = "حذف" } };
                    //}
                    //else if (!CRUDOperation.Insertable && CRUDOperation.Updatable && !CRUDOperation.Removable)
                    //{
                    //    commands = new List<ColumnCommand> { new ColumnCommand { Name = GCommandCr.Edit, Text = "ویرایش" } };
                    //}
                    //else if (CRUDOperation.Insertable && !CRUDOperation.Updatable && !CRUDOperation.Removable)
                    //{
                    //    commands = new List<ColumnCommand> { new ColumnCommand { Name = GCommandCr.Create, Text = "جدید" } };
                    //}
                    //if (!CRUDOperation.Refreshable)
                    //{
                    //    commands = commands.Where(com => com.Name != GCommandCr.Refresh).ToList();
                    //}
                    ////commands.Add(new ColumnCommand { Name = GCommandCr.Custom, Text = "" });

                    ////commands.Add(new ColumnCommand { Name = GCommandCr.UserGuide, Text = "راهنما" });
                }
                else
                {
                    FilterCommandColumnsBasedOnCRUDOperation(commands);
                }

                // return commands;
            }
            if (CRUDOperation.HasCustomAction && customCommands !=null)
            {

                commands.AddRange(customCommands);
            }

            // commands.Add(new ColumnCommand { Name = GCommandCr.Search, Text = "جستجو" });

            return commands;

        }

        private void FilterCommandColumnsBasedOnCRUDOperation(List<ColumnCommand> commands)
        {
            if (CRUDOperation.Insertable && !CRUDOperation.Updatable && CRUDOperation.Removable)
            {
                commands = commands.Where(com => com.Name == GCommandCr.Create || com.Name == GCommandCr.Delete).ToList();//&& com.Name != GCommandCr.Edit 
            }
            else if (CRUDOperation.Insertable && CRUDOperation.Updatable && !CRUDOperation.Removable)
            {
                commands = commands.Where(com => com.Name == GCommandCr.Create || com.Name == GCommandCr.Edit).ToList();
            }
            else if (!CRUDOperation.Insertable && CRUDOperation.Updatable && CRUDOperation.Removable)
            {
                commands = commands.Where(com => com.Name == GCommandCr.Edit || com.Name == GCommandCr.Delete).ToList();
            }
            else if (!CRUDOperation.Insertable && !CRUDOperation.Updatable && CRUDOperation.Removable)
            {
                commands = commands.Where(com => com.Name == GCommandCr.Delete).ToList();
            }
            else if (!CRUDOperation.Insertable && CRUDOperation.Updatable && !CRUDOperation.Removable)
            {
                commands = commands.Where(com => com.Name == GCommandCr.Edit).ToList();
            }
            else if (CRUDOperation.Insertable && !CRUDOperation.Updatable && !CRUDOperation.Removable)
            {
                commands = commands.Where(com => com.Name == GCommandCr.Create).ToList();
            }
            if (!CRUDOperation.Refreshable)
            {
                commands = commands.Where(com => com.Name != GCommandCr.Refresh).ToList();
            }
            //if (CRUDOperation.HasCustomAction)
            //{

            //    commands.AddRange(commands.Where(c => c.Name.Equals(GCommandCr.Custom)).ToList());
            //}
        }

        protected override void Serialize(IDictionary<string, object> json)
        {

        }

        //public Toolbar(Dictionary<GCommandCr, string> commandItems)
        //{
        //    Commands = new List<ColumnCommand>();
        //    if (commandItems.Any())
        //    {
        //        foreach (var item in commandItems)
        //        {
        //            switch (item.Key)
        //            {
        //                case GCommandCr.Create:
        //                    Commands.Add(MakeToolbarDefaultCommands(item.Key, item.Value, "جدید"));
        //                    break;
        //                case GCommandCr.Edit:
        //                    Commands.Add(MakeToolbarDefaultCommands(item.Key, item.Value, "ویرایش"));
        //                    break;
        //                case GCommandCr.Delete:
        //                    Commands.Add(MakeToolbarDefaultCommands(item.Key, item.Value, "حذف"));
        //                    break;
        //                case GCommandCr.Refresh:
        //                    Commands.Add(MakeToolbarDefaultCommands(item.Key, "", ""));
        //                    break;
        //                case GCommandCr.Custom:
        //                    Commands.Add(MakeToolbarDefaultCommands(item.Key, "", ""));
        //                    break;
        //                case GCommandCr.Search:
        //                    Commands.Add(MakeToolbarDefaultCommands(item.Key, item.Value, "جستجو"));
        //                    break;
        //                case GCommandCr.UserGuide:
        //                    Commands.Add(MakeToolbarDefaultCommands(item.Key, item.Value, "راهنما"));
        //                    break;
        //                case GCommandCr.Excel:
        //                    Commands.Add(MakeToolbarDefaultCommands(item.Key, item.Value, "خروجی اکسل"));
        //                    break;
        //                case GCommandCr.Pdf:
        //                    Commands.Add(MakeToolbarDefaultCommands(item.Key, item.Value, "خروجی PDF"));
        //                    break;
        //                default:
        //                    break;
        //            }
        //        }
        //    }

        //}

        //private ColumnCommand MakeToolbarDefaultCommands(GCommandCr commandType, string commandText, string defaultString)
        //{
        //    return new ColumnCommand { Name = commandType, Text = string.IsNullOrEmpty(commandText) ? defaultString : commandText };
        //}
    }
}
