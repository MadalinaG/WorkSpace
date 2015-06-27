using SmartQA.DTO;
using SmartQA.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace SmartQA.Helpers
{
    public class DataAccess
    {
        private string _connectionString;
        public DataAccess(string connectionString)
        {
            _connectionString = connectionString;
        }

        public List<TopicModels> GetTopics(int pageCount, int offset)
        {
            List<TopicModels> Topics = new List<TopicModels>();
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
                                        TopicModels topic = new TopicModels();
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

        public List<TopicModels> GetAllTopics()
        {
            List<TopicModels> Topics = new List<TopicModels>();
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
                                        TopicModels topic = new TopicModels();
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
        public List<TestModels> GetTestsByUser(string userId, int pageCount, int offset)
        {
            List<TestModels> Tests = new List<TestModels>();
            if (_connectionString != string.Empty)
            {
                try
                {
                    using (SqlConnection connection = new SqlConnection(_connectionString))
                    {
                        connection.Open();

                        using (SqlTransaction transaction = connection.BeginTransaction())
                        {
                            string sql;
                            sql = string.Format(getTestByUser,userId, offset,pageCount,pageCount );
                            using (SqlCommand command = new SqlCommand(sql, connection, transaction))
                            {
                                using (SqlDataReader reader = command.ExecuteReader())
                                {
                                    
                                    while (reader.Read())
                                    {
                                        TestModels test = new TestModels();
                                        test.AddedByID = userId;
                                        for (int i = 0; i < reader.FieldCount; i++)
                                        {
                                            test.ID = Convert.ToInt32(reader["ID"]);
                                            test.Title = reader["Title"].ToString();
                                            test.TopicID = Convert.ToInt32(reader["TopicID"]);
                                            DateTime date;
                                            DateTime.TryParse(reader["AddedTime"].ToString(), out date);
                                            test.AddedTime = date;
                                            test.QuestionsNumber = Convert.ToInt32(reader["QuestionsNumber"]);
                                            test.TopicName = reader["TopicName"].ToString();
                                            test.Solved = Convert.ToBoolean(reader["Solved"]);
                                        }
                                        Tests.Add(test);
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

            return Tests;
        }
        public List<TestModels> GetTestByTopic(int topicId, int pageCount, int offset)
        {
            List<TestModels> Tests = new List<TestModels>();
            if (_connectionString != string.Empty)
            {
                try
                {
                    using (SqlConnection connection = new SqlConnection(_connectionString))
                    {
                        connection.Open();

                        using (SqlTransaction transaction = connection.BeginTransaction())
                        {
                            string sql;
                            sql = string.Format(getTestByTopic, topicId, offset, pageCount, pageCount);
                            using (SqlCommand command = new SqlCommand(sql, connection, transaction))
                            {
                               
                                using (SqlDataReader reader = command.ExecuteReader())
                                {

                                    while (reader.Read())
                                    {
                                        TestModels test = new TestModels();
                                        for (int i = 0; i < reader.FieldCount; i++)
                                        {
                                            test.ID = Convert.ToInt32(reader["ID"]);
                                            test.Title = reader["Title"].ToString();
                                            test.AddedByID = reader["AddedByID"].ToString();
                                            test.TopicID = Convert.ToInt32(reader["TopicID"]);
                                            DateTime date;
                                            DateTime.TryParse(reader["AddedTime"].ToString(), out date);
                                            test.AddedTime = date;
                                            test.QuestionsNumber = Convert.ToInt32(reader["QuestionsNumber"]);
                                            test.TopicName = reader["TopicName"].ToString();
                                            test.Solved = Convert.ToBoolean(reader["Solved"]);
                                        }
                                        Tests.Add(test);
                                    }
                                }
                            }
                        }
                        connection.Close();
                    }
                }
                catch (Exception ex)
                {
                    throw;
                    WriteErrorToDb(ex.Message);
                }
            }

            return Tests;
        }
        public List<TestModels> GetTests(int pag)
        {
            List<TestModels> Tests = new List<TestModels>();
            if (_connectionString != string.Empty)
            {
                try
                {
                    using (SqlConnection connection = new SqlConnection(_connectionString))
                    {
                        connection.Open();

                        using (SqlTransaction transaction = connection.BeginTransaction())
                        {
                            string sql;
                            sql = string.Format(getTests, pag);
                            using (SqlCommand command = new SqlCommand(sql, connection, transaction))
                            {
                                using (SqlDataReader reader = command.ExecuteReader())
                                {

                                    while (reader.Read())
                                    {
                                        TestModels test = new TestModels();
                                        for (int i = 0; i < reader.FieldCount; i++)
                                        {
                                            test.ID = Convert.ToInt32(reader["ID"]);
                                            test.Title = reader["Title"].ToString();
                                            test.TopicID = Convert.ToInt32(reader["TopicID"]);
                                            DateTime date;
                                            DateTime.TryParse(reader["AddedTime"].ToString(), out date);
                                            test.AddedTime = date;
                                            test.QuestionsNumber = Convert.ToInt32(reader["QuestionsNumber"]);
                                            test.TopicName = reader["TopicName"].ToString();
                                            test.Solved = Convert.ToBoolean(reader["Solved"]);
                                            test.PathTopicPicture = reader["PhotoName"].ToString();
                                            test.UserName = reader["Username"].ToString();
                                            test.AddedByID = reader["AddedByID"].ToString();
                                        }
                                        Tests.Add(test);
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

            return Tests;
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
        public int GetQuizNR(string UserId)
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

                            using (SqlCommand command = new SqlCommand(getTestNrByUser, connection, transaction))
                            {
                                command.Parameters.Add("@UserId", SqlDbType.NVarChar).Value = UserId;
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
        public int GetQuizCount()
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

                            using (SqlCommand command = new SqlCommand(getTestsCount, connection, transaction))
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
        public int GetTestCountByTopic(int topicId)
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

                            using (SqlCommand command = new SqlCommand(getTestsCountByTopic, connection, transaction))
                            {
                                command.Parameters.Add(@"TopicId", SqlDbType.Int).Value = topicId;
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
        public int SaveQuizDB(TestModels test, List<BGDocument> bgDocuments)
        {
            if (test.ID != null)
            {
                if(GetMAXIdForTable("Test") != test.ID)
                {
                    test.ID = GetMAXIdForTable("Test");
                }
            }
            else
            {
                test.ID = GetMAXIdForTable("Test");
            }

            DateTime myDateTime = DateTime.Now;
            string sqlFormattedDate = myDateTime.ToString("yyyy-MM-dd HH:mm:ss");
            test.AddedTime = DateTime.Now;
           
            if (string.IsNullOrEmpty(test.QuizInstructions))
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
                                command.Parameters.Add("@FileName", SqlDbType.NVarChar, 256).Value = (string.IsNullOrEmpty(test.FileName))? string.Empty : test.FileName;
                                command.Parameters.Add("@TopicID", SqlDbType.Int).Value = test.TopicID;
                                command.Parameters.Add("@AddedById", SqlDbType.NVarChar, 128).Value = (string.IsNullOrEmpty(test.AddedByID))? string.Empty : test.AddedByID;
                                command.Parameters.Add("@QuestionsNumber", SqlDbType.Int).Value = test.QuestionsNumber;
                                command.Parameters.Add("@NumberOfAnswerForQuestion", SqlDbType.Int).Value = test.NumberOfAnswerForQuestion;
                                command.Parameters.Add("@MultipleAnswersForOneQuestion", SqlDbType.Bit).Value = (test.MultipleAnswersForOneQuestion == true) ? 1 : 0;
                                command.Parameters.Add("@Description", SqlDbType.NVarChar, 1000).Value = (string.IsNullOrEmpty(test.Description))?string.Empty :  test.Description;
                                command.Parameters.Add("@QuizInstructions", SqlDbType.NVarChar, 3000).Value = (string.IsNullOrEmpty(test.QuizInstructions))? string.Empty : test.QuizInstructions;
                                command.Parameters.Add("@QuizPathOnServer", SqlDbType.NVarChar, 500).Value = (string.IsNullOrEmpty(test.QuizPathOnServer))? string.Empty : test.QuizPathOnServer;
                                command.Parameters.Add("@StartReadAtPage", SqlDbType.Int).Value = test.StartReadAtPage;
                                command.Parameters.Add("@StopReadAtPage", SqlDbType.Int).Value = test.StopReadAtPage;
                                command.Parameters.Add("@XmlBeforeProcess", SqlDbType.NVarChar, 3000).Value = (string.IsNullOrEmpty(test.XmlBeforeProcess))? string.Empty : test.XmlBeforeProcess;
                                command.Parameters.Add("@Query", SqlDbType.NVarChar, 500).Value = (string.IsNullOrEmpty(test.Query))? string.Empty : test.Query;
                                int rows = command.ExecuteNonQuery();
                                transaction.Commit();
                            }
                        }

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
                        if (bgDocuments != null)
                        {
                            if (bgDocuments.Count() > 0)
                            {
                                foreach (BGDocument bgDoc in bgDocuments)
                                {
                                    using (SqlTransaction transaction = connection.BeginTransaction())
                                    {
                                        using (SqlCommand command = new SqlCommand(insertBGDoc, connection, transaction))
                                        {
                                            command.Parameters.Add("@Title", SqlDbType.NVarChar).Value = bgDoc.Title;
                                            command.Parameters.Add("@TopicID", SqlDbType.Int).Value = bgDoc.TopicID;
                                            command.Parameters.Add("@TestID", SqlDbType.Int).Value = bgDoc.TestID;
                                            command.Parameters.Add("@AddedByID", SqlDbType.NVarChar).Value = bgDoc.AddedByID;
                                            command.Parameters.Add("@FileName", SqlDbType.NVarChar).Value = bgDoc.FileName;
                                            command.Parameters.Add("@Path", SqlDbType.NVarChar).Value = bgDoc.Path;

                                            int rows = command.ExecuteNonQuery();
                                            transaction.Commit();
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
                    throw;
                    WriteErrorToDb(ex.Message);
                }
            }
            return test.ID;
        }
        public void SaveTopicDB(TopicModels topic, string UserName)
        {
            topic.ID = GetMAXIdForTable("Topic");
            topic.AddedBy = GetUserId(UserName);
            if (string.IsNullOrEmpty(topic.Description))
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
        public TestModels GetTest(int id)
        {
            TestModels test = null;
            if (_connectionString != string.Empty)
            {
                
                try
                {
                    using (SqlConnection connection = new SqlConnection(_connectionString))
                    {
                        connection.Open();

                        using (SqlTransaction transaction = connection.BeginTransaction())
                        {
                            using (SqlCommand command = new SqlCommand(getTestByID, connection, transaction))
                            {
                                command.Parameters.Add("@ID", SqlDbType.Int).Value = id;
                                using (SqlDataReader reader = command.ExecuteReader())
                                {
                                    while (reader.Read())
                                    {
                                        test = new TestModels();

                                        test.ID = Convert.ToInt32(reader["ID"]);
                                        test.Title = reader["Title"].ToString();
                                        test.FileName = reader["FileName"].ToString();
                                        test.TopicID = Convert.ToInt32(reader["TopicID"]);
                                        DateTime date;
                                        DateTime.TryParse(reader["AddedTime"].ToString(), out date);
                                        test.AddedTime = date;
                                        test.QuizPathOnServer = reader["QuizPathOnServer"].ToString();
                                        test.QuestionsNumber = Convert.ToInt32(reader["QuestionsNumber"]);
                                        test.NumberOfAnswerForQuestion = Convert.ToInt32(reader["NumberOfAnswerForQuestion"]);
                                        test.Solved = Convert.ToBoolean(reader["Solved"]);
                                        test.StartReadAtPage = Convert.ToInt32(reader["StartReadAtPage"]);
                                        test.StopReadAtPage = Convert.ToInt32(reader["StopReadAtPage"]);
                                        test.AddedByID = reader["AddedByID"].ToString();
                                        test.Query = reader["Query"].ToString();
                                        test.XmlBeforeProcess = reader["XmlBeforeProcess"].ToString();
                                        test.XmlAfterProcess = reader["XmlAfterProcess"].ToString();
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
            return test;
        }
        public int GetMAXIdForTable(string tableName)
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
                            string sql = String.Format(getMAxIdFromTable, tableName);

                            using (SqlCommand command = new SqlCommand(sql, connection, transaction))
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

            if (maxIdFromDb == -1)
            {
                return 1;
            }
            else
            {
                return maxIdFromDb + 1;
            }
        }


        public void UpdateTopic(TopicModels topic)
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
                            if (topic.PhotoName != null)
                            {
                                using (SqlCommand command = new SqlCommand(updateTopic, connection, transaction))
                                {
                                    command.Parameters.Add("@TopicName", SqlDbType.NVarChar).Value = topic.TopicName;
                                    command.Parameters.Add("@Description", SqlDbType.NVarChar).Value = topic.Description;
                                    command.Parameters.Add("@PhotoName", SqlDbType.NVarChar).Value = topic.PhotoName;
                                    int rows = command.ExecuteNonQuery();
                                    transaction.Commit();
                                }
                            }
                            else
                            {

                                using (SqlCommand command = new SqlCommand(updateTopicWithoutPhoto, connection, transaction))
                                {
                                    command.Parameters.Add("@TopicName", SqlDbType.NVarChar).Value = topic.TopicName;
                                    command.Parameters.Add("@Description", SqlDbType.NVarChar).Value = topic.Description;
                                    int rows = command.ExecuteNonQuery();
                                    transaction.Commit();
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
                                command.Parameters.Add("@ErrorMessage", SqlDbType.NVarChar, 4000).Value = ErrorMessage;
                                int rows = command.ExecuteNonQuery();
                                transaction.Commit();
                            }
                        }
                        connection.Close();
                    }
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
        }

        public List<GenericType> GetAnswerType()
        {
            List<GenericType> AnswerTypeList = new List<GenericType>();
            GenericType one = new GenericType();
            one.ID = 1; one.Value = "2 Answers (a, b)";

            GenericType two = new GenericType();
            two.ID = 2; two.Value = "3 Answers (a, b, c)";

            GenericType three = new GenericType();
            three.ID = 3; three.Value = "4 Answers (a, b, c, d)";

            GenericType four = new GenericType();
            four.ID = 4; four.Value = "5 Answers (a, b, c, d, e)";

            
            AnswerTypeList.Add(one);
            AnswerTypeList.Add(two);
            AnswerTypeList.Add(three);
            AnswerTypeList.Add(four);
            

            return AnswerTypeList;

        }

        public List<GenericType> GetTimeZoneList()
        {
            List<GenericType> TimeZoneList = new List<GenericType>();
            GenericType first = new GenericType()
            {
                ID = 1,
                Value = "(GMT-10:00) Hawaii"
            };

            GenericType second = new GenericType()
            {
                ID = 2,
                Value = "(GMT-09:00) Alaska"
            };
            GenericType third = new GenericType()
            {
                ID = 3,
                Value = "(GMT-08:00) Pacific Time (US &amp; Canada)"
            };
            GenericType four = new GenericType()
            {
                ID = 4,
                Value = "(GMT-07:00) Arizona"
            };
            GenericType five = new GenericType()
            {
                ID = 5,
                Value = "(GMT-07:00) Mountain Time (US &amp; Canada)"
            };
            GenericType six = new GenericType()
            {
                ID = 6,
                Value = "(GMT-06:00) Central Time (US &amp; Canada)"
            };
            GenericType seven = new GenericType()
            {
                ID = 7,
                Value = "(GMT-05:00) Eastern Time (US &amp; Canada)"
            };
            GenericType eight = new GenericType()
            {
                ID = 8,
                Value = "(GMT-05:00) Indiana (East)"
            };
            GenericType nine = new GenericType()
            {
                ID = 9,
                Value = "Central European Time (CET) UTC+1"
            };
            TimeZoneList.Add(first);
            TimeZoneList.Add(second);
            TimeZoneList.Add(third);
            TimeZoneList.Add(four);
            TimeZoneList.Add(five);
            TimeZoneList.Add(six);
            TimeZoneList.Add(seven);
            TimeZoneList.Add(eight);
            TimeZoneList.Add(nine);

            return TimeZoneList;

        }
        public string GetUserName(string Email)
        {
            string UserName = string.Empty;
            if (_connectionString != string.Empty)
            {
                try
                {
                    using (SqlConnection connection = new SqlConnection(_connectionString))
                    {
                        connection.Open();

                        using (SqlTransaction transaction = connection.BeginTransaction())
                        {
                            using (SqlCommand command = new SqlCommand(getUserNameByEmailAddres, connection, transaction))
                            {
                                command.Parameters.Add(new SqlParameter("Email", Email));
                                using (SqlDataReader reader = command.ExecuteReader())
                                {
                                    while (reader.Read())
                                    {

                                        for (int i = 0; i < reader.FieldCount; i++)
                                        {
                                            UserName = reader["UserName"].ToString();
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
            return UserName;

        }

        public void DeleteObject(int Id, string tableName)
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
                            string sql = string.Format(deleteObject, tableName, Id);
                            using (SqlCommand command = new SqlCommand(sql, connection, transaction))
                            {
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

        public void AddContact(ContactModels contact)
        {
            contact.ID = GetMAXIdForTable("Contact");
            if (_connectionString != string.Empty)
            {
                try
                {
                    using (SqlConnection connection = new SqlConnection(_connectionString))
                    {
                        connection.Open();
                        using (SqlTransaction transaction = connection.BeginTransaction())
                        {
                            using (SqlCommand command = new SqlCommand(insertContact, connection, transaction))
                            {
                                command.Parameters.Add("@ID", SqlDbType.Int).Value = contact.ID;
                                command.Parameters.Add("@Name", SqlDbType.NVarChar, 256).Value = contact.Name;
                                command.Parameters.Add("@Subject", SqlDbType.Int).Value = contact.Subject;
                                command.Parameters.Add("@Email", SqlDbType.NVarChar, 256).Value = contact.Email;
                                command.Parameters.Add("@Message", SqlDbType.NVarChar, 4000).Value = contact.Message;
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

        public void AddUserCopy(UserViewModels userCopy)
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
                            using (SqlCommand command = new SqlCommand(inserUserCopy, connection, transaction))
                            {
                                command.Parameters.Add("@ID", SqlDbType.NVarChar, 128).Value = userCopy.ID;
                                command.Parameters.Add("@Username", SqlDbType.NVarChar, 256).Value = userCopy.Username;
                                command.Parameters.Add("@Email", SqlDbType.NVarChar, 256).Value = userCopy.Email;
                                command.Parameters.Add("@Phone", SqlDbType.NVarChar, 500).Value = string.Empty;
                                command.Parameters.Add("@Pictures", SqlDbType.NVarChar, 500).Value = string.Empty;
                                command.Parameters.Add("@Company", SqlDbType.NVarChar, 256).Value = string.Empty;
                                command.Parameters.Add("@TimeZone", SqlDbType.Int).Value = 1;
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
        public UserViewModels SelectUser(string userId)
        {
            UserViewModels userCopy = new UserViewModels();
            if (_connectionString != string.Empty)
            {
                try
                {
                    using (SqlConnection connection = new SqlConnection(_connectionString))
                    {
                        connection.Open();

                        using (SqlTransaction transaction = connection.BeginTransaction())
                        {

                            using (SqlCommand command = new SqlCommand(selectUser, connection, transaction))
                            {
                                command.Parameters.Add(new SqlParameter("UserId", userId));
                                using (SqlDataReader reader = command.ExecuteReader())
                                {
                                    while (reader.Read())
                                    {

                                        for (int i = 0; i < reader.FieldCount; i++)
                                        {
                                            userCopy.ID = reader["ID"].ToString();
                                            userCopy.Username = reader["Username"].ToString();
                                            userCopy.Email = reader["Email"].ToString();
                                            userCopy.Phone = reader["Phone"].ToString();
                                            userCopy.Picture = reader["Pictures"].ToString();
                                            userCopy.Company = reader["Company"].ToString();
                                            userCopy.TimeZone = Convert.ToInt32(reader["TimeZone"]);
                                        }

                                    }
                                }
                            }
                        }
                        connection.Close();
                    }
                }
                catch (SqlException ex)
                {

                }
            }
            return userCopy;  
        }

        public void  UpdateUser(UserViewModels user)
        {
            int rows = 0;
            if (_connectionString != string.Empty)
            {
                try
                {
                    using (SqlConnection connection = new SqlConnection(_connectionString))
                    {
                        connection.Open();

                        using (SqlTransaction transaction = connection.BeginTransaction())
                        {
                            using (SqlCommand command = new SqlCommand(updateUsers, connection, transaction))
                            {
                                command.Parameters.Add("@Email", SqlDbType.NVarChar).Value = user.Email;
                                command.Parameters.Add("@Username", SqlDbType.NVarChar).Value = user.Username;
                                command.Parameters.Add("@Phone", SqlDbType.NVarChar).Value =(!string.IsNullOrEmpty(user.Phone))?user.Phone : string.Empty;
                                command.Parameters.Add("@Pictures", SqlDbType.NVarChar).Value = (!string.IsNullOrEmpty(user.Picture))? user.Picture : string.Empty;
                                command.Parameters.Add("@Company", SqlDbType.NVarChar).Value = (!string.IsNullOrEmpty(user.Company))? user.Company : string.Empty;
                                command.Parameters.Add("@TimeZone", SqlDbType.Int).Value = user.TimeZone;
                                command.Parameters.Add("@ID", SqlDbType.NVarChar).Value = user.ID;
                                rows = command.ExecuteNonQuery();
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
            if(rows != 0)
            {
                UpdateUserASP(user);
            }
        }
        private void UpdateUserASP(UserViewModels user)
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
                            using (SqlCommand command = new SqlCommand(updateAspUser, connection, transaction))
                            {
                                command.Parameters.Add("@Email", SqlDbType.NVarChar).Value = user.Email;
                                command.Parameters.Add("@Username", SqlDbType.NVarChar).Value = user.Username;
                                command.Parameters.Add("@Phone", SqlDbType.NVarChar).Value = (!string.IsNullOrEmpty(user.Phone))? user.Phone : string.Empty;
                                command.Parameters.Add("@ID", SqlDbType.NVarChar).Value = user.ID;
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
        public void SaveQuestionAndAnswers(QuestionModels questionModels, List<AnswerModels> ValidAnswerList)
        {
            questionModels.ID = GetMAXIdForTable("Question");
            int rows = 0;
            if (_connectionString != string.Empty)
            {
                try
                {
                    using (SqlConnection connection = new SqlConnection(_connectionString))
                    {
                        connection.Open();
                        using (SqlTransaction transaction = connection.BeginTransaction())
                        {
                            using (SqlCommand command = new SqlCommand(insertQuestion, connection, transaction))
                            {
                                command.Parameters.Add("@QuizID", SqlDbType.Int).Value = questionModels.QuizID;
                                command.Parameters.Add("@TopicID", SqlDbType.Int).Value = questionModels.TopicID;
                                command.Parameters.Add("@NumberOfAnswers", SqlDbType.Int).Value = questionModels.NumberOfAnswers;
                                command.Parameters.Add("@Text", SqlDbType.NVarChar).Value = questionModels.Text;
                                rows = command.ExecuteNonQuery();
                                transaction.Commit();
                            }
                        }

                        if(rows > 0)
                        {
                            using (SqlTransaction transaction = connection.BeginTransaction())
                            {
                                using (SqlCommand command = new SqlCommand(UpdateTestWithQuestionNR, connection, transaction))
                                {
                                    command.Parameters.Add("@TestId", SqlDbType.Int).Value = questionModels.QuizID;
                                    command.ExecuteNonQuery();
                                    transaction.Commit();
                                }
                            }

                            foreach (AnswerModels answer in ValidAnswerList)
                            {
                                answer.ID = GetMAXIdForTable("Answer");
                                answer.QuestionID = questionModels.ID;
                                int rowsAnswer = 0;
                                using (SqlTransaction transaction = connection.BeginTransaction())
                                {
                                    using (SqlCommand command = new SqlCommand(insertAnswer, connection, transaction))
                                    {
                                        command.Parameters.Add("@QuizID", SqlDbType.Int).Value = answer.QuizID;
                                        command.Parameters.Add("@QuestionID", SqlDbType.Int).Value = answer.QuestionID;
                                        command.Parameters.Add("@Text", SqlDbType.NVarChar).Value = answer.Text;
                                        rowsAnswer = command.ExecuteNonQuery();
                                        transaction.Commit();
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
          
        }
        public string GetTitle(int ID)
        {
            string Title = string.Empty;
            if (_connectionString != string.Empty)
            {
                try
                {
                    using (SqlConnection connection = new SqlConnection(_connectionString))
                    {
                        connection.Open();

                        using (SqlTransaction transaction = connection.BeginTransaction())
                        {
                            using (SqlCommand command = new SqlCommand(getTitle, connection, transaction))
                            {
                                command.Parameters.Add(new SqlParameter("ID", ID));
                                using (SqlDataReader reader = command.ExecuteReader())
                                {
                                    while (reader.Read())
                                    {

                                        for (int i = 0; i < reader.FieldCount; i++)
                                        {
                                            Title = reader["Title"].ToString();
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

            return Title;
        }
        public string getTopicName(int ID)
        {
            string Title = string.Empty;
            if (_connectionString != string.Empty)
            {
                try
                {
                    using (SqlConnection connection = new SqlConnection(_connectionString))
                    {
                        connection.Open();

                        using (SqlTransaction transaction = connection.BeginTransaction())
                        {
                            using (SqlCommand command = new SqlCommand(getTopicNameq, connection, transaction))
                            {
                                command.Parameters.Add(new SqlParameter("ID", ID));
                                using (SqlDataReader reader = command.ExecuteReader())
                                {
                                    while (reader.Read())
                                    {

                                        for (int i = 0; i < reader.FieldCount; i++)
                                        {
                                            Title = reader["TopicName"].ToString();
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

            return Title;
        }
        public int CountQuestions(int QuizID)
        {
            int questions = -1;
            if (_connectionString != string.Empty)
            {
                try
                {
                    using (SqlConnection connection = new SqlConnection(_connectionString))
                    {
                        connection.Open();
                        using (SqlTransaction transaction = connection.BeginTransaction())
                        {

                            using (SqlCommand command = new SqlCommand(getQuestionNR, connection, transaction))
                            {
                                command.Parameters.Add("@QuizID", SqlDbType.Int).Value = QuizID;
                                using (SqlDataReader reader = command.ExecuteReader())
                                {
                                   
                                    while (reader.Read())
                                    {
                                        for (int i = 0; i < reader.FieldCount; i++)
                                        {
                                            questions = Convert.ToInt32(reader["QuestionsNumber"]);
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
            return questions;
        }
        public void UpdateTestAndSaveBGDoc(List<BGDocument> bgDocuments, TestModels test)
        {
            if (string.IsNullOrEmpty(test.Description))
            {
                test.Description = string.Empty;

            }
            if (string.IsNullOrEmpty(test.QuizInstructions))
            {
                test.QuizInstructions = string.Empty;
            }
            if (bgDocuments.Count() > 0 || (!string.IsNullOrEmpty(test.Description) && !string.IsNullOrEmpty(test.QuizInstructions)))
            {
                if (_connectionString != string.Empty)
                {
                    try
                    {
                        using (SqlConnection connection = new SqlConnection(_connectionString))
                        {
                            connection.Open();
                            if((!string.IsNullOrEmpty(test.Description) || !string.IsNullOrEmpty(test.QuizInstructions)))
                            {
                                using (SqlTransaction transaction = connection.BeginTransaction())
                                {
                                    using (SqlCommand command = new SqlCommand(updateTest, connection, transaction))
                                    {
                                        command.Parameters.Add("@Description", SqlDbType.NVarChar).Value = test.Description;
                                        command.Parameters.Add("@QuizInstructions", SqlDbType.NVarChar).Value = test.QuizInstructions;
                                        command.Parameters.Add("@ID", SqlDbType.Int).Value = test.ID;
                                        int rows = command.ExecuteNonQuery();
                                        transaction.Commit();
                                    }
                                }
                            }
                            if(bgDocuments.Count() > 0)
                            {
                                foreach(BGDocument bgDoc in bgDocuments)
                                {
                                using (SqlTransaction transaction = connection.BeginTransaction())
                                {
                                    using (SqlCommand command = new SqlCommand(insertBGDoc, connection, transaction))
                                    {
                                        command.Parameters.Add("@Title", SqlDbType.NVarChar).Value = bgDoc.Title;
                                        command.Parameters.Add("@TopicID", SqlDbType.Int).Value = bgDoc.TopicID;
                                        command.Parameters.Add("@TestID", SqlDbType.Int).Value = bgDoc.TestID;
                                        command.Parameters.Add("@AddedByID", SqlDbType.NVarChar).Value = bgDoc.AddedByID;
                                        command.Parameters.Add("@FileName", SqlDbType.NVarChar).Value = bgDoc.FileName;
                                        command.Parameters.Add("@Path", SqlDbType.NVarChar).Value = bgDoc.Path;

                                        int rows = command.ExecuteNonQuery();
                                        transaction.Commit();
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
            }
        }
        public List<QuestionModels> getQuestions(int quizID)
        {
            List<QuestionModels> Questions = new List<QuestionModels>();
            if (_connectionString != string.Empty)
            {
                try
                {
                    using (SqlConnection connection = new SqlConnection(_connectionString))
                    {
                        connection.Open();

                        using (SqlTransaction transaction = connection.BeginTransaction())
                        {

                            using (SqlCommand command = new SqlCommand(getQuestionByTest, connection, transaction))
                            {
                                command.Parameters.Add("@QuizId", SqlDbType.Int).Value = quizID;
                                using (SqlDataReader reader = command.ExecuteReader())
                                {
                                  
                                    while (reader.Read())
                                    {
                                        QuestionModels question = new QuestionModels();
                                        for (int i = 0; i < reader.FieldCount; i++)
                                        {
                                            question.ID = Convert.ToInt32(reader["ID"]);
                                            question.QuizID = Convert.ToInt32(reader["QuizID"]);
                                            question.TopicID = Convert.ToInt32(reader["TopicID"]);
                                            question.DocumentID = Convert.ToInt32(reader["DocumentID"]);
                                            question.MultipleAnswers = Convert.ToBoolean(reader["MultipleAnswers"]);
                                            question.QuestionSolved = Convert.ToBoolean(reader["QuestionSolved"]);
                                            question.NumberOfAnswers = Convert.ToInt32(reader["NumberOfAnswers"]);
                                            question.Text = reader["Text"].ToString();
                                            
                                        }
                                        Questions.Add(question);
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

            return Questions;
        }
        public List<AnswerModels> getAnswersForQuestion(int questionID, int quizID)
        {
            List<AnswerModels> Answers = new List<AnswerModels>();
            if (_connectionString != string.Empty)
            {
                try
                {
                    using (SqlConnection connection = new SqlConnection(_connectionString))
                    {
                        connection.Open();

                        using (SqlTransaction transaction = connection.BeginTransaction())
                        {

                            using (SqlCommand command = new SqlCommand(getAnswers, connection, transaction))
                            {
                                command.Parameters.Add("QuizId", SqlDbType.Int).Value = quizID;
                                command.Parameters.Add("QuestionId", SqlDbType.Int).Value = questionID;
                                using (SqlDataReader reader = command.ExecuteReader())
                                {
                                  
                                    while (reader.Read())
                                    {
                                        AnswerModels answer = new AnswerModels();
                                        for (int i = 0; i < reader.FieldCount; i++)
                                        {
                                            answer.ID = Convert.ToInt32(reader["ID"]);
                                            answer.QuizID = Convert.ToInt32(reader["QuizID"]);
                                            answer.QuestionID = Convert.ToInt32(reader["QuestionID"]);
                                            answer.Text = reader["Text"].ToString();
                                        }
                                        Answers.Add(answer);
                                    }
                                }
                            }
                        }
                        connection.Close();
                    }
                }
                catch (Exception ex)
                {
                    throw;
                    WriteErrorToDb(ex.Message);
                }
            }

            return Answers;
        }
        public void UpdateTes(string query, string xmlBeforeProcess, string xmlAfterProcess, int quizID)
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
                            using (SqlCommand command = new SqlCommand(updateTestXML, connection, transaction))
                            {
                                command.Parameters.Add("@query", SqlDbType.NVarChar).Value = query;
                                command.Parameters.Add("@XmlbeforeProcess", SqlDbType.NVarChar).Value = xmlBeforeProcess;
                                command.Parameters.Add("@XmlAfterProcess", SqlDbType.NVarChar).Value = xmlAfterProcess;
                                command.Parameters.Add("@QuizID", SqlDbType.Int).Value = quizID;
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
        
        private static string getQuestionNR = @"Select QuestionsNumber From Test Where ID = @QuizID";
        private static string getCountNrTopic = @"Select Count('') From Topic";
        private static string getTopicByOffsetNr = @"   SELECT *
                                                        FROM Topic
                                                        ORDER BY AddedTime desc
                                                        OFFSET ((@Offset - 1) * @PageCount) ROWS
                                                        FETCH NEXT @PageCount ROWS ONLY;";

        private static string getTopicsQuery = @" Select * from Topic ORDER BY TopicName";
        private static string getMAxIdFromTable = @"Select Max(ID) AS ID from {0}";
        private static string UpdateTestWithQuestionNR = @"Update Test
                                                             Set QuestionsNumber = [QuestionsNumber] + 1
                                                             Where ID = @TestId";
        private static string getUserIdForCurrentUser = @"Select ID AS ID From AspNetUsers Where UserName= @UserName";
        private static string insertTest = @"Insert INTO Test 
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
                                                        Query,
                                                        Solved)                                   
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
                                                        @Query,
                                                        0
                                                        )";

        private static  string UpdateNrArticlesFromTopic = @"Update Topic
                                                             Set NrOfArticles = [NrOfArticles] + @NrArticles
                                                             Where ID = @TopicID";
        private static string UpdateTopicTableWhenInsertQuiz = @"Update Topic 
                                                                   Set QuizNumber = [QuizNumber] + @NumberOfQuizesForTopic, 
                                                                       NrOfQuestions = [NrOfQuestions] + @NrOfQuestionsForTopic
                                                                 Where ID = @TopicID";
        private static string insertTopic = @" INSERT INTO Topic 
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
        private static string insertContact = @" INSERT INTO Contact 
                                                   (ID,
                                                    Name,
                                                    Subject,
                                                    AddedTime, 
                                                    Email,
                                                    Message
                                                    )
                                            VALUES
                                                   (@ID,
                                                    @Name,
                                                    @Subject,
                                                    GETDATE(),
                                                    @Email,
                                                    @Message
                                                    )";
        private static string inserUserCopy = @"INSERT INTO Users
                                                   (ID,
                                                    Email,
                                                    Username,
                                                    Phone, 
                                                    Pictures,
                                                    Company,
                                                    TimeZone
                                                    )
                                            VALUES
                                                   (@ID,
                                                    @Email,
                                                    @Username,
                                                    @Phone,
                                                    @Pictures,
                                                    @Company,
                                                    @TimeZone
                                                    )";


        private string writeErrorMessageTODB = @"INSERT INTO Logger ([ErrorMessage] , [DateTime] ) VALUES (@ErrorMessage , GETDATE())";

        private static string getUserNameByEmailAddres = @"Select UserName From AspNetUsers Where Email = @Email";
        private static string updateTopic = @"Update Topic Set TopicName = @TopicName, Description = @Description, PhotoName = @PhotoName Where TopicName = @TopicName ";
        private static string updateTopicWithoutPhoto = @"Update Topic Set TopicName = @TopicName, Description = @Description Where TopicName = @TopicName ";
        private static string deleteObject = @"DELETE FROM {0} WHERE ID = {1}";
        private static string selectUser = @"Select ID, Email, Username, Phone, Pictures, Company, TimeZone FROM Users WHERE ID = @UserId";
        private static string updateUsers = @"Update Users SET Email = @Email, Username = @Username,  Phone = @Phone, Pictures = @Pictures,Company = @Company,  TimeZone = @TimeZone WHERE ID = @ID ";
        private static string updateAspUser = @"Update AspNetUsers SET Email = @Email, PhoneNumber = @Phone, UserName = @Username WHERE Id = @ID";
        private static string getTestNrByUser = @"Select Count('') FROM Test WHERE AddedByID = @UserId";

        private static string getTestByUser = @"Select   
                                                            ID, Title,TopicID,AddedTime,QuestionsNumber, 
                                                            (Select TopicName From Topic WHERE ID = TopicID) AS TopicName, Solved 
                                                 From Test
                                                 WHERE AddedByID = '{0}' 
                                                 ORDER BY AddedTime desc
                                                 OFFSET (({1} - 1) * {2}) ROWS
                                                 FETCH NEXT {3} ROWS ONLY";


        private static string getTestsCountByTopic = @"Select Count('') From Test Where TopicID = @TopicId";
        private static string getTestsCount = @"Select Count('') From Test";
        private static string getTests = @"Select Top( {0} )  ID,Title,TopicID,AddedTime,QuestionsNumber, (Select TopicName From Topic WHERE ID = TopicID) AS TopicName, Solved, (Select PhotoName From Topic Where ID = TopicID) AS PhotoName, (Select UserName from AspNetUsers Where Id = AddedByID) AS Username, AddedByID From Test Order By AddedTime DESC";

        private static string insertQuestion = @"Insert Into Question 
                                                (ID, QuizID, TopicID, NumberOfAnswers, Text, DocumentID, MultipleAnswers, QuestionSolved) 
                                                Values ((SELECT ISNULL(MAX (ID),0) FROM Question) + 1, @QuizID, @TopicID, @NumberOfAnswers, @Text, 0, 0, 0)";

        private static string insertAnswer = @"Insert Into Answer (ID, QuizID, QuestionID, Text) Values ((SELECT ISNULL(MAX (ID), 0) FROM Answer) + 1, @QuizID, @QuestionID, @Text)";

        private static string getTitle = @"Select Title From Test Where ID = @ID";
        private static string getTopicNameq = @"Select TopicName From Topic Where ID = @ID";
        private static string updateTest = @"Update Test SET Description = @Description, QuizInstructions = @QuizInstructions WHERE ID = @ID";
        private static string insertBGDoc = @"INSERT INTO BackgroundDocument 
                                                                            (ID, Title, TopicID, TestID, AddedByID, AddedTime, FileName, Path) 
                                                     Values ((select ISNULL(Max(ID),0) + 1 from BackgroundDocument), @Title, @TopicID, @TestID, @AddedByID, GETDATE(), @FileName, @Path)";


        private static string getTestByID = @"SELECT * From Test WHERE ID = @ID";
        private static string getQuestionByTest = @"Select * From Question Where QuizID = @QuizId";
        private static string getAnswers = @"Select * from Answer Where QuizID = @QuizId and QuestionID = @QuestionId";
        private static string updateTestXML= @"Update Test SET Query = @query, XmlBeforeProcess = @XmlbeforeProcess, XmlAfterProcess = @XmlAfterProcess WHERE ID = @QuizID";
        private static string getTestByTopic = @"Select   
                                                            ID, Title,AddedByID,TopicID,AddedTime,QuestionsNumber, 
                                                            (Select TopicName From Topic WHERE ID = TopicID) AS TopicName, Solved 
                                                 From Test
                                                 WHERE TopicId = '{0}' 
                                                 ORDER BY AddedTime desc
                                                 OFFSET (({1} - 1) * {2}) ROWS
                                                 FETCH NEXT {3} ROWS ONLY";



       
    }
}