using System;

namespace RcOnline.Models
{
public class App
{
    private readonly string _path ;

    public App()
    {
        _path = AppDomain.CurrentDomain.BaseDirectory
                    .Replace("bin/Debug/netcoreapp2.0/","")
                    .Replace("bin\\Debug\\netcoreapp2.0\\","") + "base.xlsx";
    }
    public void Start()
        {
            while (true)
            {
                Console.Write("1 - Добавить контору в очередь\n" +
                            "2 - Обновить статистику\n" +
                            "3 - Запустить таски\n" +
                            "4 - Перезапустить таски завершенные с ошибкой\n" +
                            "5 - Доделать успешные таски\n" +
                            "0 - Выход\n" +
                            "Ваш выбор: ");

                string choice = Console.ReadLine().Trim();

                switch (choice)
                {
                    case "0":
                        return;

                    case "1": 
                    {
                        var cm = new CompanyManager(_path);
                        cm.AddCompany();
                        break;
                    }
                    case "2":
                    {
                        var tm = new TaskManager(_path);
                        tm.UpdateTaskStatuses();
                        break;
                    }
                    case "3":
                    {
                        var tm = new TaskManager(_path);
                        tm.StartPendingTasks();
                        break;
                    }
                    case "4":
                    {
                        var tm = new TaskManager(_path);
                        tm.RestartErrorTasks();
                        break;
                    }    
                    case "5":
                    {
                        var tm = new TaskManager(_path);
                        tm.RestartSuccessTasks();
                        break;
                    }                  

                    default:
                        Logger.WriteLineError("Ошибочка...\n");
                        break;
                }                
            }
        }
    }   
}