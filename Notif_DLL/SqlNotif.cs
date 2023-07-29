using Notif_Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace Notif_DLL
{
    public class SqlNotif
    {
        static string connectionString = "Server=DESKTOP-1JJPA47\\SQLEXPRESS;Initial Catalog=notifdatabase;Integrated Security=True;";
        static SqlConnection sqlConnection;

        public SqlNotif()
        {
            sqlConnection = new SqlConnection(connectionString);
        }
        public List<Notification> GetSaveNotifications()
        {
            UserRepository userRepository = new UserRepository();
            var selectStatement = "SELECT StudentID, senderName, receiverName, Content, DateTime, IsRead FROM tblNotification";
            SqlCommand selectCommand = new SqlCommand(selectStatement, sqlConnection);

            sqlConnection.Open();
            SqlDataReader reader = selectCommand.ExecuteReader();

            List<Notification> storedNotifications = new List<Notification>();

            while (reader.Read())
            {
                var studentID = reader.GetString(0);
                var senderName = reader.GetString(1); 
                var receiverName = reader.GetString(2); 
                var content = reader.GetString(3);
                var isRead = reader.GetBoolean(5);

              
                var sender = userRepository.GetUserByName(senderName);
                var receiver = userRepository.GetUserByName(receiverName);

             
                DateTime dateModified;
                if (reader.IsDBNull(4))
                {
                    dateModified = DateTime.MinValue; 
                }
                else
                {
                    dateModified = reader.GetDateTime(4);
                }

                storedNotifications.Add(new Notification
                {
                    StudentID = studentID,
                    senderName = sender, 
                    receiverName = receiver, 
                    Content = content,
                    DateModified = dateModified,
                    IsRead = isRead
                });
            }

            sqlConnection.Close();

            return storedNotifications;
        }

        public void StoreNotifications(Notification storenotifications)
        {
            var insertStatement = "INSERT INTO tblNotification (StudentID, senderName, receiverName, Content, DateTime, IsRead) " +
                                  "VALUES (@StudentID, @senderName, @receiverName, @Content, @DateTime, @IsRead)";
            SqlCommand insertCommand = new SqlCommand(insertStatement, sqlConnection);
            insertCommand.Parameters.AddWithValue("@StudentID", storenotifications.StudentID);
            insertCommand.Parameters.AddWithValue("@senderName", storenotifications.senderName.Name);
            insertCommand.Parameters.AddWithValue("@receiverName", storenotifications.receiverName.Name);
            insertCommand.Parameters.AddWithValue("@Content", storenotifications.Content);
            insertCommand.Parameters.AddWithValue("@DateTime", storenotifications.DateModified);
            insertCommand.Parameters.AddWithValue("@IsRead", storenotifications.IsRead);

            sqlConnection.Open();

            insertCommand.ExecuteNonQuery();

            sqlConnection.Close();
        }
        public void DeleteNotificationAndColumn(string studentId, string columnName)
            {
                var deleteStatement = $"UPDATE tblNotification SET {columnName} = NULL WHERE StudentID = @StudentID";
                SqlCommand deleteCommand = new SqlCommand(deleteStatement, sqlConnection);
                deleteCommand.Parameters.AddWithValue("@StudentID", studentId);

                sqlConnection.Open();
                deleteCommand.ExecuteNonQuery();
                sqlConnection.Close();
            }
    }

}
