using Notif_BLL;
using Notif_DLL;
using Notif_Models;
using System;
using System.Configuration;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Reflection.Emit;
using System.Reflection;
using System.Reflection.Metadata;
using System.Text;
using static System.Net.Mime.MediaTypeNames;

namespace NotificationInterface
{
    internal class Program
    {
        static void Main(string[] args)
        {
            SqlNotif sqlNotif = new SqlNotif();

            while (true)
            {
                Console.WriteLine("Choose an option:");
                Console.WriteLine("1. Send Notification");
                Console.WriteLine("2. View List of Notifications");
                Console.WriteLine("3. Delete Notification");
                Console.WriteLine("4. Exit");

                Console.Write("Enter the number of your choice: ");
                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        Console.Write("Enter student ID: ");
                        string studentId = Console.ReadLine();

                        Console.Write("Enter sender name: ");
                        string senderName = Console.ReadLine();

                        Console.Write("Enter receiver name: ");
                        string receiverName = Console.ReadLine();

                        Console.Write("Enter notification content: ");
                        string content = Console.ReadLine();

                        NotificationManagement notificationManagement = new NotificationManagement();
                        notificationManagement.SendNotification(senderName, receiverName, content, studentId);

                        var notification = new Notification
                        {
                            StudentID = studentId,
                            senderName = new User { Name = senderName },
                            receiverName = new User { Name = receiverName },
                            Content = content,
                            DateModified = DateTime.Now,
                            IsRead = false
                        };
                        sqlNotif.StoreNotifications(notification);

                        Console.WriteLine("\nNotification sent and saved successfully!\n");
                        break;

                    case "2":
                        List<Notification> storedNotifications = sqlNotif.GetSaveNotifications();
                        foreach (var notif in storedNotifications)
                        {
                            Console.WriteLine($"Student ID: {notif.StudentID}");
                            Console.WriteLine($"Sender: {notif.senderName.Name}");
                            Console.WriteLine($"Receiver: {notif.receiverName.Name}");
                            Console.WriteLine($"Content: {notif.Content}");
                            Console.WriteLine($"Date Modified: {notif.DateModified}");
                            Console.WriteLine($"Is Read: {notif.IsRead}");
                            Console.WriteLine("-------------------------");
                        }
                        break;
                        
                    case "3":
                        Console.Write("Enter student ID to delete: ");
                        string studentIdToDelete = Console.ReadLine();

                        Console.Write("Enter column name to delete in the database: ");
                        string columnNameToDelete = Console.ReadLine();

                        sqlNotif.DeleteNotificationAndColumn(studentIdToDelete, columnNameToDelete);
                        Console.WriteLine("Notification deleted successfully!\n");
                        break;

                    case "4":

                        Console.WriteLine("Exiting the program...");
                        return;

                    default:
                        Console.WriteLine("Invalid choice. Please try again.");
                        break;
                }
            }
        }
    }
}