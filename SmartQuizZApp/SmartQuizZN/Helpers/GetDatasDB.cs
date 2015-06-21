using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using SmartQuizZN.Models;
using System.Data;
namespace SmartQuizZN.Helpers
{
    public class GetDatasDB
    {
        private string _connectionString ;

        public GetDatasDB(string connectionString)
        {
            _connectionString = connectionString;
        }

        public List<Topic> GetTopics(int pageCount, int offset)
        {
            List<Topic> Topics = new List<Topic>();
            if (_connectionString != string.Empty)
            {
                try
                {
                    using (SqlConnection connection = new SqlConnection(_connectionString))
                    {
                        connection.Open();

                        using (SqlTransaction transaction = connection.BeginTransaction())
                        {

                            using (SqlCommand command = new SqlCommand(getTopicByOffsetNr, connection, transaction))
                            {
                                command.Parameters.Add("@PageCount", SqlDbType.Int).Value = pageCount;
                                command.Parameters.Add("@Offset", SqlDbType.Int).Value = offset;

                                using (SqlDataReader reader = command.ExecuteReader())
                                {
                                    while (reader.Read())
                                    {
                                        Topic topic = new Topic();
                                        for (int i = 0; i < reader.FieldCount; i++)
                                        {
                                            topic.ID = Convert.ToInt32(reader["ID"]);
                                            topic.TopicName = reader["TopicName"].ToString();
                                            topic.Description = reader["Description"].ToString();
                                            topic.AddedBy = reader["AddedBy"].ToString();
                                            DateTime date;
                                            DateTime.TryParse(reader["AddedTime"].ToString(), out date);
                                            topic.AddedTime = date;
                                            topic.NrOfQuestions = Convert.ToInt32(reader["NrOfQuestions"]);
                                            topic.NrOfArticles = Convert.ToInt32(reader["NrOfArticles"]);
                                            topic.PhotoName = reader["PhotoName"].ToString();
                                            topic.QuizNumber = Convert.ToInt32(reader["QuizNumber"]);
                                        }
                                        Topics.Add(topic);
                                    }
                                }
                            }
                        }
                        connection.Close();
                    }
                }
                catch (Exception ex)
                {
                    WriteErrorToDb(ex.Message);
                }
            }
          
            return Topics;
        }

        public List<Topic> GetAllTopics()
        {
            List<Topic> Topics = new List<Topic>();
            if (_connectionString != string.Empty)
            {
                try
                {
                    using (SqlConnection connection = new SqlConnection(_connectionString))
                    {
                        connection.Open();

                        using (SqlTransaction transaction = connection.BeginTransaction())
                        {
                            using (SqlCommand command = new SqlCommand(getTopicsQuery, connection, transaction))
                            {
                                using (SqlDataReader reader = command.ExecuteReader())
                                {
                                    while (reader.Read())
                                    {
                                        Topic topic = new Topic();
                                        for (int i = 0; i < reader.FieldCount; i++)
                                        {
                                            topic.ID = Convert.ToInt32(reader["ID"]);
                                            topic.TopicName = reader["TopicName"].ToString();
                                            topic.Description = reader["Description"].ToString();
                                            topic.AddedBy = reader["AddedBy"].ToString();
                                            DateTime date;
                                            DateTime.TryParse(reader["AddedTime"].ToString(), out date);
                                            topic.AddedTime = date;
                                            topic.NrOfQuestions = Convert.ToInt32(reader["NrOfQuestions"]);
                                            topic.NrOfArticles = Convert.ToInt32(reader["NrOfArticles"]);
                                            topic.PhotoName = reader["PhotoName"].ToString();
                                            topic.QuizNumber = Convert.ToInt32(reader["QuizNumber"]);
                                        }
                                        Topics.Add(topic);
                                    }
                                }
                            }
                        }
                        connection.Close();
                    }
                }
                catch (Exception ex)
                {
                    WriteErrorToDb(ex.Message);
                }
            }

            return Topics;
        }

        public int GetTopcCountNR()
        {
            int countNr = 0;
            if (_connectionString != string.Empty)
            {
                try
                {
                    using (SqlConnection connection = new SqlConnection(_connectionString))
                    {
                        connection.Open();

                        using (SqlTransaction transaction = connection.BeginTransaction())
                        {

                            using (SqlCommand command = new SqlCommand(getCountNrTopic, connection, transaction))
                            {
                                countNr = (Int32)command.ExecuteScalar();
                            }
                        }

                        connection.Close();
                    }
                }
                catch (Exception ex)
                {
                    WriteErrorToDb(ex.Message);
                }
            }
            return countNr;
        }

        public int  SaveQuizDB(Test test, string UserName)
        {
            test.ID = GetMAXIdForTable("Test");
            DateTime myDateTime = DateTime.Now;
            string sqlFormattedDate = myDateTime.ToString("yyyy-MM-dd HH:mm:ss");
            test.AddedTime = DateTime.Now;
            test.AddedByID = GetUserId(UserName);
            if(test.QuizInstructions == null)
            {
                test.QuizInstructions = "Without Instactions";
            }
            if (_connectionString != string.Empty)
            {
                try
                {
                    using (SqlConnection connection = new SqlConnection(_connectionString))
                    {
                        connection.Open();

                        using (SqlTransaction transaction = connection.BeginTransaction())
                        {
                            using (SqlCommand command = new SqlCommand(insertTest, connection, transaction))
                            {

                                command.Parameters.Add("@ID", SqlDbType.Int).Value = test.ID;
                                command.Parameters.Add("@Title", SqlDbType.NVarChar, 256).Value = test.Title;
                                command.Parameters.Add("@FileName", SqlDbType.NVarChar, 256).Value = test.FileName;
                                command.Parameters.Add("@TopicID", SqlDbType.Int).Value = test.TopicID;
                                command.Parameters.Add("@AddedById", SqlDbType.NVarChar, 128).Value = test.AddedByID;
                                command.Parameters.Add("@QuestionsNumber", SqlDbType.Int).Value = test.QuestionsNumber;
                                command.Parameters.Add("@NumberOfAnswerForQuestion", SqlDbType.Int).Value = test.NumberOfAnswerForQuestion;
                                command.Parameters.Add("@MultipleAnswersForOneQuestion", SqlDbType.Bit).Value = (test.MultipleAnswersForOneQuestion == true) ? 1 : 0;
                                command.Parameters.Add("@Description", SqlDbType.NVarChar, 1000).Value = test.Description;
                                command.Parameters.Add("@QuizInstructions", SqlDbType.NVarChar, 3000).Value = test.QuizInstructions;
                                command.Parameters.Add("@QuizPathOnServer", SqlDbType.NVarChar, 500).Value = test.QuizPathOnServer;
                                command.Parameters.Add("@StartReadAtPage", SqlDbType.Int).Value = test.StartReadAtPage;
                                command.Parameters.Add("@StopReadAtPage", SqlDbType.Int).Value = test.StopReadAtPage;
                                command.Parameters.Add("@XmlBeforeProcess", SqlDbType.NVarChar, 3000).Value = test.XmlBeforeProcess;
                                command.Parameters.Add("@Query", SqlDbType.NVarChar, 500).Value = test.Query;
                                int rows = command.ExecuteNonQuery();
                                transaction.Commit();
                            }
                        }
                        connection.Close();
                    }
                    UpdateTopicWhenQuizInserted(test);
                }
                catch (Exception ex)
                {
                    WriteErrorToDb(ex.Message);
                }
            }
            return test.ID;
        }

        public void SaveTopicDB(Topic topic, string UserName)
        {
            topic.ID = GetMAXIdForTable("Topic");
            topic.AddedBy = GetUserId(UserName);
            if(string.IsNullOrEmpty(topic.Description))
            {
                topic.Description = "No desciption for this topic";
            }
            if (_connectionString != string.Empty)
            {
                try
                {
                    using (SqlConnection connection = new SqlConnection(_connectionString))
                    {
                        connection.Open();
                        using (SqlTransaction transaction = connection.BeginTransaction())
                        {
                            using (SqlCommand command = new SqlCommand(insertTopic, connection, transaction))
                            {
                                command.Parameters.Add("@ID", SqlDbType.Int).Value = topic.ID;
                                command.Parameters.Add("@TopicName", SqlDbType.NVarChar, 256).Value = topic.TopicName;
                                command.Parameters.Add("@PhotoName", SqlDbType.NVarChar, 256).Value = topic.PhotoName;
                                command.Parameters.Add("@Description", SqlDbType.NVarChar, 4000).Value = topic.Description;
                                command.Parameters.Add("@AddedBy", SqlDbType.NVarChar, 128).Value = topic.AddedBy;
                                int rows = command.ExecuteNonQuery();
                                transaction.Commit();
                            }
                        }
                        connection.Close();
                    }
                   
                }
                catch (Exception ex)
                {
                    WriteErrorToDb(ex.Message);
                }
            }
        }
        private string GetUserId(string UserName)
        {
            string UserId = String.Empty;
            if (_connectionString != string.Empty)
            {
                try
                {
                    using (SqlConnection connection = new SqlConnection(_connectionString))
                    {
                        connection.Open();

                        using (SqlTransaction transaction = connection.BeginTransaction())
                        {

                            using (SqlCommand command = new SqlCommand(getUserIdForCurrentUser, connection, transaction))
                            {
                                command.Parameters.Add(new SqlParameter("UserName", UserName));
                                using (SqlDataReader reader = command.ExecuteReader())
                                {
                                    while (reader.Read())
                                    {

                                        for (int i = 0; i < reader.FieldCount; i++)
                                        {
                                            UserId = reader["ID"].ToString();
                                        }

                                    }
                                }
                            }
                        }
                        connection.Close();
                    }
                }
                catch (Exception ex)
                {

                }
            }
            return UserId;
        }

        private int GetMAXIdForTable(string tableName)
        {
            int maxIdFromDb = -1;
            if (_connectionString != string.Empty)
            {
                try
                {
                    using (SqlConnection connection = new SqlConnection(_connectionString))
                    {
                        connection.Open();
                        using (SqlTransaction transaction = connection.BeginTransaction())
                        {
                            getMAxIdFromTable = String.Format(getMAxIdFromTable, tableName);
                          using (SqlCommand command = new SqlCommand(getMAxIdFromTable, connection, transaction))
                            {
                                using (SqlDataReader reader = command.ExecuteReader())
                                {
                                    while (reader.Read())
                                    {
                                        for (int i = 0; i < reader.FieldCount; i++)
                                        {
                                            maxIdFromDb = Convert.ToInt32(reader["ID"]);
                                        }
                                    }
                                }
                            }
                        }
                        connection.Close();
                    }
                }
                catch (Exception ex)
                {

                }
            }

            if(maxIdFromDb == -1)
            {
                return 1;
            }
            else
            {
                return maxIdFromDb + 1;
            }
        }

        private void UpdateTopicWhenQuizInserted(Test test)
        {
            if (_connectionString != string.Empty)
            {
                try
                {
                    using (SqlConnection connection = new SqlConnection(_connectionString))
                    {
                        connection.Open();

                        using (SqlTransaction transaction = connection.BeginTransaction())
                        {
                            using (SqlCommand command = new SqlCommand(UpdateTopicTableWhenInsertQuiz, connection, transaction))
                            {
                                command.Parameters.Add("@NumberOfQuizesForTopic", SqlDbType.Int).Value = 1;
                                command.Parameters.Add("@NrOfQuestionsForTopic", SqlDbType.Int).Value = test.QuestionsNumber;
                                command.Parameters.Add("@TopicID", SqlDbType.Int).Value = test.TopicID;
                                int rows = command.ExecuteNonQuery();
                                transaction.Commit();
                            }
                        }
                        connection.Close();
                    }
                }
                catch (Exception ex)
                {
                    WriteErrorToDb(ex.Message);
                }
            }
        }
        private void WriteErrorToDb(string ErrorMessage)
        {
            if (_connectionString != string.Empty)
            {
                try
                {
                    using (SqlConnection connection = new SqlConnection(_connectionString))
                    {
                        connection.Open();

                        using (SqlTransaction transaction = connection.BeginTransaction())
                        {
                            using (SqlCommand command = new SqlCommand(writeErrorMessageTODB, connection, transaction))
                            {
                                command.Parameters.Add("@NumberOfQuizesForTopic", SqlDbType.NVarChar, 4000).Value = ErrorMessage;
                                int rows = command.ExecuteNonQuery();
                                transaction.Commit();
                            }
                        }
                        connection.Close();
                    }
                }
                catch (Exception ex)
                {
                    
                }
            }
        }

        private string getCountNrTopic = @"Select Count(*) From Topic";
        private string getTopicByOffsetNr = @"Select Top(@PageCount) *  From Topic Where ID >= @Offset";
        private string getTopicsQuery = @" Select * from Topic ORDER BY TopicName";
        private string getMAxIdFromTable = @"Select Max(ID) AS ID from {0}";
        private string getUserIdForCurrentUser = @"Select ID AS ID From AspNetUsers Where UserName= @UserName";
        private string insertTest = @"Insert INTO Test 
                                                       (ID, 
                                                        Title, 
                                                        [FileName], 
                                                        TopicID, 
                                                        AddedByID, 
                                                        AddedTime, 
                                                        QuestionsNumber,    
                                                        NumberOfAnswerForQuestion,
                                                        MultipleAnswersForOneQuestion, 
                                                        [Description],
                                                        QuizInstructions,
                                                        QuizPathOnServer,
                                                        StartReadAtPage,
                                                        StopReadAtPage,
                                                        XmlBeforeProcess,
                                                        Query)                                   
                                                Values (@ID,
                                                        @Title,
                                                        @FileName,
                                                        @TopicID, 
                                                        @AddedById,
                                                        GETDATE(), 
                                                        @QuestionsNumber,
                                                        @NumberOfAnswerForQuestion, 
                                                        @MultipleAnswersForOneQuestion, 
                                                        @Description, 
                                                        @QuizInstructions,
                                                        @QuizPathOnServer,
                                                        @StartReadAtPage,
                                                        @StopReadAtPage,
                                                        @XmlBeforeProcess,
                                                        @Query
                                                        )";

        private string UpdateNrArticlesFromTopic = @"Update Topic
                                                             Set NrOfArticles = [NrOfArticles] + @NrArticles
                                                             Where ID = @TopicID"; 
        private string UpdateTopicTableWhenInsertQuiz = @"Update Topic 
                                                                   Set QuizNumber = [QuizNumber] + @NumberOfQuizesForTopic, 
                                                                       NrOfQuestions = [NrOfQuestions] + @NrOfQuestionsForTopic
                                                                 Where ID = @TopicID";  
        private  string insertTopic = @" INSERT INTO TOPIC 
                                                   (ID,
                                                    TopicName,
                                                    Description,
                                                    AddedBy, 
                                                    AddedTime, 
                                                    NrOfQuestions,
                                                    NrOfArticles,
                                                    PhotoName,
                                                    QuizNumber)
                                            VALUES
                                                   (@ID,
                                                    @TopicName,
                                                    @Description,
                                                    @AddedBy,
                                                    GETDATE(),
                                                    0,
                                                    0,
                                                    @PhotoName,
                                                    0)";

        private string writeErrorMessageTODB = @"INSERT INTO Logger ([ErrorMessage] , [DateTime] ) VALUES (@ErrorMessage , GETDATE())";
    }
}

