using SmartQA.DTO;
using SmartQA.Helpers;
using SmartQA.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using SmartQA.TextExtraction;
using Newtonsoft.Json;
using QuestionAnalisys;
namespace SmartQA.Controllers
{
    public class HomeController : Controller
    {
        private string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

        List<TopicModels> TopicList = new List<TopicModels>();
        List<GenericType> AnswerTypeList = new List<GenericType>();
        List<TestModels> Tests = new List<TestModels>();
        public ActionResult Index()
        {
         
            DirectoryInfo dir = new DirectoryInfo(Server.MapPath("~/UsersFiles/"));
            if (!dir.Exists)
            {
                Directory.CreateDirectory(Server.MapPath("~/UsersFiles/"));
            }
            return View();
        }
        public ActionResult About()
        {
            return View();
        }
        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        [HttpPost]
        public ActionResult Contact(ContactModels contact)
        {
            contact.Subject = 0;
           if(Request.Form["subject"].ToLower() == "General Customer Service".ToLower())
           {
               contact.Subject = 1;
           }
           if (Request.Form["subject"].ToLower() == "Suggestions".ToLower())
           {
               contact.Subject = 2;
           }
           if (Request.Form["subject"].ToLower() == "Product Support".ToLower())
           {
               contact.Subject = 3;
           }

            DataAccess dbWork = new DataAccess(connectionString);
            dbWork.AddContact(contact);

            return View();
        }

        public ActionResult EditTopic(string TopicName)
        {
            DataAccess dbWork = new DataAccess(connectionString);
            TopicList = dbWork.GetAllTopics();
            ViewBag.Topics = TopicList;
            TopicModels Topic = TopicList.Where(x => x.TopicName == TopicName).FirstOrDefault();
            return View(Topic);
            

        }

        [HttpPost]
        public ActionResult EditTopic(TopicModels topic)
        {
            DataAccess dbWork = new DataAccess(connectionString);
            HttpPostedFileBase file = Request.Files[0];
            if (file.ContentLength > 0)
            {
                var extension = Path.GetExtension(file.FileName);
                var fileName = topic.TopicName + extension;
                topic.PhotoName = fileName;
                var path = Path.Combine(Server.MapPath("~/Content/Images/"), fileName);
                file.SaveAs(path);
            }
            dbWork.UpdateTopic(topic);
            return RedirectToAction("Topics");


        }

        public ActionResult Topics()
        {
            DataAccess dbWork = new DataAccess(connectionString);
            ViewBag.Message = "Add topic";
            int PageCount = dbWork.GetTopcCountNR();


            SortingPagingInfo info = new SortingPagingInfo();
            info.SortField = "TopicName";
            info.SortDirection = "ascending";
            info.PageSize = 8;
            info.PageCount = Convert.ToInt32(Math.Ceiling((double)(PageCount / info.PageSize))) + 1;
            info.CurrentPageIndex = 1;
            TopicList = dbWork.GetTopics(info.PageSize, info.CurrentPageIndex);
            ViewBag.UserId = User.Identity.GetUserId();
            ViewBag.SortingPagingInfo = info;
            return View(TopicList);
        }

   
        public ActionResult DeleteTopic(int topicId)
        {
            DataAccess dbWork = new DataAccess(connectionString);
            dbWork.DeleteObject(topicId, "Topic");

            return RedirectToAction("Topics");
        }

        [HttpPost]
        public ActionResult Topics(SortingPagingInfo info)
        {
            ViewBag.Message = "Add Topic";
            DataAccess dbWork = new DataAccess(connectionString);
            int offset = info.CurrentPageIndex;
            TopicList = dbWork.GetTopics(info.PageSize, offset);

            ViewBag.SortingPagingInfo = info;
            ViewBag.UserId = User.Identity.GetUserId();
            return View(TopicList);

        }


        public ActionResult Recent()
        {
            DataAccess dbWork = new DataAccess(connectionString);
            int totalTestCount = dbWork.GetQuizCount();
            ViewBag.TestCount = totalTestCount;

            SortingPagingInfo info = new SortingPagingInfo();
            info.SortField = "Tests";
            info.SortDirection = "ascending";
            info.PageSize = 3;
            info.PageCount = Convert.ToInt32(Math.Ceiling((double)(totalTestCount / info.PageSize))) + 1;
            info.CurrentPageIndex = 1;
            Tests = dbWork.GetTests(info.PageSize);
            ViewBag.SortingPagingInfo = info;
            ViewBag.userId = User.Identity.GetUserId();
            return View(Tests);
        }

        [HttpPost]
        public ActionResult Recent(SortingPagingInfo info)
        {
            DataAccess dbWork = new DataAccess(connectionString);
            int totalTestCount = dbWork.GetQuizCount();
            ViewBag.TestCount = totalTestCount;

            info.PageSize = 3 * info.CurrentPageIndex;
            Tests = dbWork.GetTests(info.PageSize);
            ViewBag.userId = User.Identity.GetUserId();
            ViewBag.SortingPagingInfo = info;
            return View(Tests);
        }

        public ActionResult CreateTopic()
        {
            DataAccess dbWork = new DataAccess(connectionString);
            TopicList = dbWork.GetAllTopics();
            ViewBag.Topics = TopicList;
            return View();
        }

        [HttpPost]
        public ActionResult CreateTopic(TopicModels topic)
        {
            HttpPostedFileBase file = Request.Files[0];
            if (file.ContentLength > 0)
            {
                var extension = Path.GetExtension(file.FileName);
                var fileName = topic.TopicName + extension;
                topic.PhotoName = fileName;
                var path = Path.Combine(Server.MapPath("~/Content/Images/"), fileName);
                file.SaveAs(path);
            }
            else
            {
                topic.PhotoName = "default.jpg";
            }

            DataAccess dbWork = new DataAccess(connectionString);
            dbWork.SaveTopicDB(topic, User.Identity.Name);

            return RedirectToAction("Topics");
        }
        public ActionResult CreateQuiz()
        {
            DataAccess dbWork = new DataAccess(connectionString);
            TopicList = dbWork.GetAllTopics();
            AnswerTypeList = dbWork.GetAnswerType();

            ViewBag.Topics = new SelectList(TopicList, "ID", "TopicName");
            ViewBag.AnswersNumbers = new SelectList(AnswerTypeList, "ID", "Value");
            if (TempData["CurrentQuizZ"] != null)
            {
                TestModels test = TempData["CurrentQuizZ"] as TestModels;
                ViewData["error"] = "Insert a valid Quiz.";

                return View(test);
            }

            return View();
        }

        [HttpPost]
        public ActionResult CreateQuiz(TestModels test)
        {
            test.AddedByID = User.Identity.GetUserId();
            test.NumberOfAnswerForQuestion += 1;

            List<BGDocument> bgDocuments = new List<BGDocument>();
            DataAccess dbWork = new DataAccess(connectionString);
            string topicName = dbWork.getTopicName(test.TopicID);
            string pathBGDOC = Path.Combine(Server.MapPath("~/UserFiles/"), User.Identity.GetUserId(), "BackgroundDocuments",topicName);
            string pathQuiz = Path.Combine(Server.MapPath("~/UserFiles/"), User.Identity.GetUserId(), "Tests", topicName);
            DirectoryInfo dir1 = new DirectoryInfo(pathBGDOC);
            DirectoryInfo dir2 = new DirectoryInfo(pathQuiz);
            if (!dir1.Exists)
            {
                Directory.CreateDirectory(pathBGDOC);
            }
            if (!dir2.Exists)
            {
                Directory.CreateDirectory(pathQuiz);
            }

            for (int i = 0; i < Request.Files.Count; i++)
            {
                HttpPostedFileBase hpf = Request.Files[i] as HttpPostedFileBase;
                if (hpf.ContentLength > 0 && i == 0)
                {
                    var fileName = Path.GetFileName(hpf.FileName);
                    test.FileName = hpf.FileName;

                    var extension = Path.GetExtension(hpf.FileName);
                    var onlyFileName = Path.GetFileNameWithoutExtension(hpf.FileName);

                    var newfilename = onlyFileName + "_" + test.ID.ToString() + "_" + test.TopicID.ToString() + extension;

                    var pathDoc = Path.Combine(pathQuiz, newfilename);
                    test.QuizPathOnServer = pathDoc;

                    hpf.SaveAs(pathDoc);
                    
                }
                else
                {
                    BGDocument bgDocument = new BGDocument();
                   
                    bgDocument.TestID = test.ID;
                    bgDocument.TopicID = test.TopicID;
                    bgDocument.AddedByID = User.Identity.GetUserId();
                    if (hpf.ContentLength > 0)
                    {
                        var fileName = Path.GetFileName(hpf.FileName);
                        bgDocument.FileName = hpf.FileName;
                        bgDocument.Title = fileName;

                        var extension = Path.GetExtension(hpf.FileName);
                        var onlyFileName = Path.GetFileNameWithoutExtension(hpf.FileName);

                        var newfilename = onlyFileName + "_" + test.ID.ToString() + "_" + test.TopicID.ToString() + extension;

                        var pathDoc = Path.Combine(pathBGDOC, newfilename);
                        bgDocument.Path = pathDoc;

                        hpf.SaveAs(pathDoc);
                        bgDocuments.Add(bgDocument);
                    }
                }
            }
          test.ID =   dbWork.SaveQuizDB( test, bgDocuments);

            return RedirectToAction("QuestionsAnalisys", new { quizId = test.ID });
        }

        public ActionResult PersonalQuizzes(string UserId)
        {
            DataAccess dbWork = new DataAccess(connectionString);
            int PageCount = dbWork.GetQuizNR(User.Identity.GetUserId());


            SortingPagingInfo info = new SortingPagingInfo();
            info.SortField = "QuizName";
            info.SortDirection = "ascending";
            info.PageSize = 6;
            info.PageCount = Convert.ToInt32(Math.Ceiling((double)(PageCount / info.PageSize))) + 1;
            info.CurrentPageIndex = 1;
            string userId = User.Identity.GetUserId();
            Tests = dbWork.GetTestsByUser(userId, info.PageSize, info.CurrentPageIndex);
            ViewBag.UserId = userId;
            ViewBag.SortingPagingInfo = info;
            return View(Tests);
        }

        [HttpPost]
        public ActionResult PersonalQuizzes(SortingPagingInfo info)
        {
            DataAccess dbWork = new DataAccess(connectionString);
            int offset = info.CurrentPageIndex;
            Tests = dbWork.GetTestsByUser(User.Identity.GetUserId(), info.PageSize, offset);

            ViewBag.SortingPagingInfo = info;
            ViewBag.UserId = User.Identity.GetUserId();
            return View(Tests);

        }

        public ActionResult BuildQuiz()
        {
            TestModels test;

            if (TempData["CurrentQ"] != null)
            {
                test = TempData["CurrentQ"] as TestModels;
                if(test.Title == null)
                {
                    DataAccess dbWork = new DataAccess(connectionString);
                    test.Title = dbWork.GetTitle(test.ID);
                }
            }
            else
            {
                 test = new TestModels();

                DataAccess dbWork = new DataAccess(connectionString);
                test.ID = dbWork.GetMAXIdForTable("Test");
                TopicList = dbWork.GetAllTopics();
                test.QuizSaved = false;
            }
                test.Questions = new List<QuestionModels>();

                QuestionModels question = new QuestionModels();
                question.QuizID = test.ID;

                question.Answers = new List<AnswerModels>();
                AnswerModels answerDefault1 = new AnswerModels();
                answerDefault1.QuizID = test.ID;
                AnswerModels answerDefault2 = new AnswerModels();
                answerDefault2.QuizID = test.ID;
                AnswerModels answerDefault3 = new AnswerModels();
                answerDefault3.QuizID = test.ID;
                AnswerModels answerDefault4 = new AnswerModels();
                answerDefault4.QuizID = test.ID;
                AnswerModels answerDefault5 = new AnswerModels();
                answerDefault5.QuizID = test.ID;
                question.Answers.Add(answerDefault1);
                question.Answers.Add(answerDefault2);
                question.Answers.Add(answerDefault3);
                question.Answers.Add(answerDefault4);
                question.Answers.Add(answerDefault5);

                test.Questions.Add(question);
                ViewBag.Index = 0;
                ViewBag.Topics = new SelectList(TopicList, "ID", "TopicName");
                return View(test);
            
        }
        [HttpPost]
        public ActionResult BuildQuiz(TestModels test)
        {
            DataAccess dbWork = new DataAccess(connectionString);
            test.UserName = User.Identity.Name;
            test.AddedByID = User.Identity.GetUserId();
            if (test.QuizSaved == true)
            {
                test.QuestionsNumber = dbWork.CountQuestions(test.ID);
            }
            else
            {
                test.QuestionsNumber = 1;
            }

            test = SetDefaultValues(test);
            int quizId = test.ID;

            if (test.QuizSaved == false)
            {
                quizId = dbWork.SaveQuizDB(test, null);
                test.QuizSaved = true;
            }


            test.Questions[0].QuizID = quizId;
            test.Questions[0].TopicID = test.TopicID;

            List<AnswerModels> ValidAnswerList = new List<AnswerModels>();
            foreach (AnswerModels answer in test.Questions[0].Answers)
            {
                if (!string.IsNullOrEmpty(answer.Text))
                {
                    answer.QuizID = quizId;
                    ValidAnswerList.Add(answer);
                }
            }

            test.Questions[0].NumberOfAnswers = ValidAnswerList.Count();

            dbWork.SaveQuestionAndAnswers(test.Questions[0], ValidAnswerList);
            TempData["CurrentQ"] = test;

                if (Request.Form.AllKeys.Contains("addquestion"))
                {
                    test.Questions = new List<QuestionModels>();
                    QuestionModels question = new QuestionModels();
                    question.QuizID = test.ID;
                    question.Answers = new List<AnswerModels>();
                    AnswerModels answerDefault1 = new AnswerModels();
                    answerDefault1.QuizID = test.ID;
                    AnswerModels answerDefault2 = new AnswerModels();
                    answerDefault2.QuizID = test.ID;
                    AnswerModels answerDefault3 = new AnswerModels();
                    answerDefault3.QuizID = test.ID;
                    AnswerModels answerDefault4 = new AnswerModels();
                    answerDefault4.QuizID = test.ID;
                    AnswerModels answerDefault5 = new AnswerModels();
                    answerDefault5.QuizID = test.ID;

                    question.Answers.Add(answerDefault1);
                    question.Answers.Add(answerDefault2);
                    question.Answers.Add(answerDefault3);
                    question.Answers.Add(answerDefault4);
                    question.Answers.Add(answerDefault5);

                    test.Questions.Add(question);
                    ViewBag.Index = test.QuestionsNumber - 1;
                }
                else if (Request.Form.AllKeys.Contains("savequiz"))
                {
                    return RedirectToAction("SaveQuiz");     
                }

                
            

            //return View(test);
            return RedirectToAction("BuildQuiz");
        }

        private TestModels SetDefaultValues(TestModels test)
        {
            test.Description = (!string.IsNullOrEmpty(test.Description)) ? test.Description : string.Empty;
            test.FileName = (!string.IsNullOrEmpty(test.FileName)) ? test.FileName : string.Empty;
            test.PathTopicPicture = (!string.IsNullOrEmpty(test.PathTopicPicture)) ? test.PathTopicPicture : string.Empty;
            test.Query = (!string.IsNullOrEmpty(test.Query)) ? test.Query : string.Empty;
            test.QuizInstructions = (!string.IsNullOrEmpty(test.QuizInstructions)) ? test.QuizInstructions : string.Empty;
            test.QuizPathOnServer = (!string.IsNullOrEmpty(test.QuizPathOnServer)) ? test.QuizPathOnServer : string.Empty;
            test.TopicName = (!string.IsNullOrEmpty(test.TopicName)) ? test.TopicName : string.Empty;
            test.XmlAfterProcess = (!string.IsNullOrEmpty(test.XmlAfterProcess)) ? test.XmlAfterProcess : string.Empty;
            test.XmlBeforeProcess = (!string.IsNullOrEmpty(test.XmlBeforeProcess)) ? test.XmlBeforeProcess : string.Empty;

            return test;
        }
        public ActionResult EditQuiz()
        {
            return View();
        }

        public ActionResult SaveQuiz()
        {
            TestModels test = new TestModels();

            if (TempData["CurrentQ"] != null)
            {
                test = TempData["CurrentQ"] as TestModels;
                if (test.Title == null)
                {
                    DataAccess dbWork = new DataAccess(connectionString);
                    test.Title = dbWork.GetTitle(test.ID);
                }
            }
            else
            {
                return RedirectToAction("BuildQuiz");
            }

            return View(test);
        }

        [HttpPost]
        public ActionResult SaveQuiz(TestModels test)
        {
            List<BGDocument> bgDocuments = new List<BGDocument>();
            DataAccess dbWork = new DataAccess(connectionString);
            string topicName = dbWork.getTopicName(test.TopicID);
            string path = Path.Combine(Server.MapPath("~/UserFiles/"),User.Identity.GetUserId(), "BackgroundDocuments", topicName);
            DirectoryInfo dir = new DirectoryInfo(path);
            if (!dir.Exists)
            {
                Directory.CreateDirectory(path);
            }

            for (int i = 0; i < Request.Files.Count; i++)
            {
                BGDocument bgDocument = new BGDocument();
                HttpPostedFileBase hpf = Request.Files[i] as HttpPostedFileBase;
                bgDocument.TestID = test.ID;
                bgDocument.TopicID = test.TopicID;
                bgDocument.AddedByID = User.Identity.GetUserId();
                if (hpf.ContentLength > 0)
                {
                    var fileName = Path.GetFileName(hpf.FileName);
                    bgDocument.FileName = hpf.FileName;
                    bgDocument.Title = fileName;

                    var extension = Path.GetExtension(hpf.FileName);
                    var onlyFileName = Path.GetFileNameWithoutExtension(hpf.FileName);

                    var newfilename = onlyFileName + "_" + test.ID.ToString() + "_" + test.TopicID.ToString() + extension;

                    var pathDoc = Path.Combine(path, newfilename);
                    bgDocument.Path = pathDoc;

                    hpf.SaveAs(pathDoc);
                    bgDocuments.Add(bgDocument);
                }
            }

            dbWork.UpdateTestAndSaveBGDoc(bgDocuments, test);

            return View(test);
        }
        [HttpPost]
        public ActionResult DeleteQuiz(int id)
        {
            DataAccess dbWork = new DataAccess(connectionString);
            dbWork.DeleteObject(id, "Test");

            return RedirectToAction("Recent");
        }
        public ActionResult DeleteQuizFromMyQuiz(int id)
        {
            DataAccess dbWork = new DataAccess(connectionString);
            dbWork.DeleteObject(id, "Test");

            return RedirectToAction("PersonalQuizzes");
        }

        public ActionResult QuestionsAnalisys(int quizId)
        {
            DataAccess dbWork = new DataAccess(connectionString);
            TestModels testSelected = dbWork.GetTest(quizId);
            List<QuestionM> qMList = new List<QuestionM>();
            List<QuestionAnalisys.Question> questionList = new List<Question>(); ;
            string query = string.Empty;
            if (testSelected == null)
            {
                return View("Index");
            }
            else
            {
                if(string.IsNullOrEmpty(testSelected.FileName))
                {
                    List<QuestionModels> questions = dbWork.getQuestions(testSelected.ID);
                   for(int i = 0; i< questions.Count(); i++)
                   {
                       List<QuestionAnalisys.Answer> answers = new List<Answer>();
                       questions[i].Answers = dbWork.getAnswersForQuestion(questions[i].ID, questions[i].QuizID);

                       foreach(AnswerModels am in questions[i].Answers)
                       {
                           QuestionAnalisys.Answer an = new Answer()
                           {
                               ID = am.ID,
                               Text = am.Text

                           };
                           answers.Add(an);
                       }

                       QuestionAnalisys.Question quest = new Question();
                       quest.QuestionId = questions[i].ID;
                       quest.QuizId = questions[i].QuizID;
                       quest.TopicId = questions[i].TopicID;
                       quest.QuestionText = questions[i].Text;
                       quest.AnswerList = answers;
                       questionList.Add(quest);
                   }
                }
                else
                {
                       if(!string.IsNullOrEmpty(testSelected.QuizPathOnServer))
                       {
                           PdfProcessing process = new PdfProcessing(testSelected.QuizPathOnServer, testSelected.StartReadAtPage, testSelected.StopReadAtPage);
                           List<TextDocument> list = process.GetAllText();
                           ExtractInfo ex = new ExtractInfo(testSelected.ID, testSelected.TopicID,testSelected.QuestionsNumber, list[0].Text,testSelected.NumberOfAnswerForQuestion);
                            ex.Extract();
                            questionList = ex.questionList;
                            query = ex.query;
                       }
                }

                string xmlBeforeProcess = string.Empty;
                //string xmlBeforeProcess = HttpHandlers.SerializeToString<List<QuestionAnalisys.Question>>(questionList);
                string xmlAfterProcess = string.Empty;
                if (questionList.Count > 0)
                {
                    Analyser analize = new Analyser(questionList, query);
                    analize.AnalizeQuestions();
                    
                    foreach(QuestionAnalisys.Question qq in questionList)
                    {
                        QuestionM qM = new QuestionM();
                        qM.QuestionText = qq.QuestionText;
                        qM.QuestionLemmatized = qq.QuestionLemmatized;
                        qM.QuestionId = qq.QuestionId;
                        qM.QuestionAfterProcess = qq.QuestionAfterProcess;
                        qM.LanguageId = qq.LanguageId;
                        qM.KeyWords = qq.KeyWords;
                        qM.IsNegative = qq.IsNegative;
                        qM.Focus = qq.Focus;
                        qM.AnswerTypeExpected = qq.AnswerTypeExpected;
                        qM.AnswerList = qq.AnswerList;
                        qM.QuizId = qq.QuizId;
                        qM.TopicId = qq.TopicId;
                        qM.QuestionType = qq.QuestionType;
                        qM.SentanceW = qq.SentanceW;

                        qMList.Add(qM);
                    }
                  
                }

                //dbWork.UpdateTes(query, xmlBeforeProcess, xmlAfterProcess, testSelected.ID);
            }

            return View(qMList);
        }

      
        [HttpPost]
        public ActionResult QuestionsAnalisys()
        {

            return View();
        }
    }
}