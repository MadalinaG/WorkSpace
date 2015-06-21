using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Configuration;
using SmartQuizZN.Helpers;
using SmartQuizZN.Models;
using System.IO;
using System.Web.Security;
using TextProcessing;
using QuestionAnalisys;
using Newtonsoft.Json;
using System.Web.Routing;
namespace SmartQuizZN.Controllers
{
    public class HomeController : Controller
    {
        private string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        List<Topic> Topicss = new List<Topic>();
        List<AnswersType> AnswersNumber = new List<AnswersType>();
        
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Topics()
        {
           
            ViewBag.Message = "Create new topic";
            GetDatasDB dbWork = new GetDatasDB(connectionString);
            int PageCount = dbWork.GetTopcCountNR();
            

            SortingPagingInfo info = new SortingPagingInfo();
            info.SortField = "TopicName";
            info.SortDirection = "ascending";
            info.PageSize = 8;
            info.PageCount = Convert.ToInt32(Math.Ceiling((double)(PageCount / info.PageSize))) + 1;
            info.CurrentPageIndex = 1;
            Topicss = dbWork.GetTopics(info.PageSize,info.CurrentPageIndex);

            ViewBag.SortingPagingInfo = info;
            return View(Topicss);
        }

        [HttpPost]
        public ActionResult Topics(SortingPagingInfo info)
        {
            ViewBag.Message = "Create new topic";
            GetDatasDB dbWork = new GetDatasDB(connectionString);
            int offset = ((info.CurrentPageIndex - 1) * info.PageSize) + 1;
            Topicss = dbWork.GetTopics(info.PageSize, offset);

            ViewBag.SortingPagingInfo = info;
            return View(Topicss);

        }


        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult CreateQuiz()
        {


            GetDatasDB dbWork = new GetDatasDB(connectionString);
            Topicss = dbWork.GetAllTopics();

            AnswersType one = new AnswersType();
            one.ID = 1; one.Value = "2 Answers (a, b)";

            AnswersType two = new AnswersType();
            two.ID = 2; two.Value = "3 Answers (a, b, c)";

            AnswersType three = new AnswersType();
            three.ID = 3; three.Value = "4 Answers (a, b, c, d)";

            AnswersType four = new AnswersType();
            four.ID = 4; four.Value = "5 Answers (a, b, c, d, e)";

            AnswersType five = new AnswersType();
            five.ID = 5; five.Value = "More than 5 answers";
            AnswersNumber.Add(one);
            AnswersNumber.Add(two);
            AnswersNumber.Add(three);
            AnswersNumber.Add(four);
            AnswersNumber.Add(five);

            ViewBag.Topics = new SelectList(Topicss, "ID", "TopicName");
            ViewBag.AnswersNumbers = new SelectList(AnswersNumber, "ID", "Value");
            if (TempData["CurrentQuizZ"] != null)
            {
                Test test = TempData["CurrentQuizZ"] as Test;
                ViewData["error"] = "Insert a valid Quiz.";

                return View(test);
            }

            return View();

        }
        

        [HttpPost]
        public ActionResult CreateQuiz(Test test)
        {
            string id = User.Identity.Name;
            test.NumberOfAnswerForQuestion += 1;
            //foreach (string file in Request.Files)
            //{
            for(int i = 0; i < Request.Files.Count; i++)
            {
                HttpPostedFileBase hpf = Request.Files[i] as HttpPostedFileBase;
                if (hpf.ContentLength > 0)
                {
                    var fileName = Path.GetFileName(hpf.FileName);
                    test.FileName = fileName;
                    var extension = Path.GetExtension(hpf.FileName);
                    var onlyFileName = Path.GetFileNameWithoutExtension(hpf.FileName);

                    var newfilename = onlyFileName + "_" + id + "_" + test.TopicID.ToString() + extension;

                    var path = Path.Combine(Server.MapPath("~/TestDocuments/"), newfilename);
                    test.QuizPathOnServer = Path.Combine("~/TestDocuments/", newfilename);
                    DirectoryInfo dir = new DirectoryInfo(Server.MapPath("~/TestDocuments/"));
                    if (!dir.Exists)
                    {
                        Directory.CreateDirectory(Server.MapPath("~/TestDocuments/"));
                    }
                    hpf.SaveAs(path);
                }
            }
            if (test.Description == null)
            {
                test.Description = "Description.";
            }
            List<QuestionModels> qModels = new List<QuestionModels>();
           
            //read data from quiz 
            if (System.IO.File.Exists(Server.MapPath(test.QuizPathOnServer)))
            {
                TextProcessing.PdfProcessing text = new TextProcessing.PdfProcessing(Server.MapPath(test.QuizPathOnServer), test.StartReadAtPage, test.StopReadAtPage);
                List<TextDocument> list = text.GetAllText();
                ExtractInfo ex = new ExtractInfo(1, test.TopicID, test.QuestionsNumber, list[0].Text, test.NumberOfAnswerForQuestion);
                ex.Extract();
                List<QuestionAnalisys.Question> questionList = ex.questionList;
                string query = ex.query;
                if (questionList == null || questionList.Count() == 0)
                {

                    TempData["CurrentQuizZ"] = test;
                    return RedirectToAction("CreateQuiz");
                }
                else
                {
                    string obj = JsonConvert.SerializeObject(questionList);
                    test.XmlBeforeProcess = obj;
                    test.Query = query;

                    if (!string.IsNullOrEmpty(test.XmlBeforeProcess))
                    {
                        var listrr = JsonConvert.DeserializeObject<List<SmartQuizZN.Models.QuestionModels>>(test.XmlBeforeProcess);
                    }

                    GetDatasDB dbWork = new GetDatasDB(connectionString);
                    int testId = dbWork.SaveQuizDB(test, id);

                    foreach (Question question in questionList)
                    {
                        QuestionModels qModel = new QuestionModels();
                        qModel.AnswerList = question.AnswerList;
                        qModel.AnswerTypeExpected = question.AnswerTypeExpected;
                        qModel.CorrectAnswers = question.CorrectAnswers;
                        qModel.Focus = question.Focus;
                        qModel.IsAnswered = question.IsAnswered;
                        qModel.IsNegative = question.IsNegative;
                        qModel.KeyWords = question.KeyWords;
                        qModel.LanguageId = question.LanguageId;
                        qModel.QuestionAfterProcess = question.QuestionAfterProcess;
                        qModel.QuestionId = question.QuestionId;
                        qModel.QuestionLemmatized = question.QuestionLemmatized;
                        qModel.QuestionText = question.QuestionText;
                        qModel.QuestionType = question.QuestionType;
                        qModel.QuizId = testId;
                        qModel.SentanceW = question.SentanceW;
                        qModel.TopicId = question.TopicId;

                        qModels.Add(qModel);
                    }
                }
            }

            TempData["qModels"] = qModels;
            TempData["Test"] = test;
            TempData["pathQuiz"] = test.QuizPathOnServer.Remove(0, 1);
            return RedirectToAction("Upload"); 
        }

        public ActionResult Upload()
        {
            List<QuestionModels> questions = new List<QuestionModels>();
            if (TempData["qModels"] != null)
            {
                questions = TempData["qModels"] as List<QuestionModels>;
                ViewBag.path = TempData["pathQuiz"];
                return View(questions);
            }
            else
            {
                if (TempData["qModels"] != null)
                {
                    TempData["CurrentQuizZ"] = TempData["test"];
                }
                return RedirectToAction("CreateQuiz");
            }
        }

        [HttpPost]
        public ActionResult Upload(int QuizId)
        {
            string id = User.Identity.Name;
            foreach (string file in Request.Files)
            {
                HttpPostedFileBase hpf = Request.Files[file] as HttpPostedFileBase;
                if (hpf.ContentLength > 0)
                {
                    //var fileName = Path.GetFileName(hpf.FileName);
                    //test.FileName = fileName;
                    //var extension = Path.GetExtension(hpf.FileName);
                    //var onlyFileName = Path.GetFileNameWithoutExtension(hpf.FileName);

                    //var newfilename = onlyFileName + "_" + id + "_" + test.TopicID.ToString() + extension;

                    //var path = Path.Combine(Server.MapPath("~/BackgroundDocuments/"), newfilename);
                    //test.QuizPathOnServer = Path.Combine("~/BackgroundDocuments/", newfilename);
                    //DirectoryInfo dir = new DirectoryInfo(Server.MapPath("~/BackgroundDocuments/"));
                    //if (!dir.Exists)
                    //{
                    //    Directory.CreateDirectory(Server.MapPath("~/BackgroundDocuments/"));
                    //}
                    //hpf.SaveAs(path);
                }
            }
            return View();
        }
                
        public ActionResult CreateTopic()
        {
            GetDatasDB dbWork = new GetDatasDB(connectionString);
            Topicss = dbWork.GetAllTopics();
            ViewBag.Topics = Topicss;
            return View();
        }

        [HttpPost]
        public ActionResult CreateTopic(Topic topic)
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

            GetDatasDB dbWork = new GetDatasDB(connectionString);
            dbWork.SaveTopicDB(topic, User.Identity.Name);

            return RedirectToAction("Topics");
        }
    }
}