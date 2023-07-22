﻿using Notif_Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Notif_DLL
{
    public class SqlNotif
    {
        static string connectionString = "Server=DESKTOP-1JJPA47\\SQLEXPRESS;Initial Catalog=notifdatabase;Integrated Security=True";
        static SqlConnection sqlConnection;

        public SqlNotif()
        {
            sqlConnection = new SqlConnection(connectionString);
        }
        public List<Notification> GetSaveNotifications()
            {
                UserRepository userRepository = new UserRepository();
                var insertStatement = "SELECT StudentID, senderName, receiverName, Content, DateTime, IsRead FROM tblNotification";
                SqlCommand insertCommand = new SqlCommand(insertStatement, sqlConnection);
            
            sqlConnection.Open();
                SqlDataReader reader = insertCommand.ExecuteReader();

                List<Notification> storenotifications = new List<Notification>();

                while (reader.Read())
                {
                storenotifications.Add(new Notification
                    {
                        StudentID = reader.GetString(0),
                        senderName = userRepository.GetUserByName(reader.GetString(1)),
                        receiverName = userRepository.GetUserByName(reader.GetString(2)),
                        Content = reader.GetString(3),
                        DateTime = DateTime.Now,

                    });
                }

                sqlConnection.Close();

                return storenotifications;
            }

            public void StoreNotifications(Notification storenotifications)
            {
            var insertStatement = "INSERT INTO Notifications (StudentID, senderName, receiverName, Content, DateTime, IsRead) " +
                      "VALUES (@StudentID, @senderName, @receiverName, @Content, @DateTime, @IsRead)";
            SqlCommand insertCommand = new SqlCommand(insertStatement, sqlConnection);
                insertCommand.Parameters.AddWithValue("@StudentID", storenotifications.StudentID);
                insertCommand.Parameters.AddWithValue("@senderName", storenotifications.senderName.Name);
                insertCommand.Parameters.AddWithValue("@receiverName", storenotifications.receiverName.Name);
                insertCommand.Parameters.AddWithValue("@Content", storenotifications.Content);
                insertCommand.Parameters.AddWithValue("@DateTime", storenotifications.DateTime);
                insertCommand.Parameters.AddWithValue("@IsRead", storenotifications.IsRead);

            sqlConnection.Open();

                insertCommand.ExecuteNonQuery();

                sqlConnection.Close();
            }
        }
    
}