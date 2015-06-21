using System;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Xml;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using QuestionAnalisys;
using APIs;
using System.Collections.Generic;
using TextProcessing;
using QuestionAnalisys.DTO;
using System.Text.RegularExpressions;
using IndexerLucene;

using Newtonsoft.Json;
using DeserializeProject;
namespace Tests
{
    [TestClass]
    public class UnitTestQuestionAnalisys
    {
        string connectionString = @"Server=tcp:gyfba7sacb.database.windows.net,1433;Database=SmartQuizZ_db;User ID=SmartQuiz@gyfba7sacb;Password=Madalina1893;Trusted_Connection=False;Encrypt=True;Connection Timeout=30";
        [TestMethod]
        public void TestMethodDeserialize()
        {
            //DOCUMENT doc = new DOCUMENT();
            //doc.Part = new Part();
            //doc.Part.S = new S[1];
            //doc.Part.S[0] = new S();
            //doc.Part.S[0].W = new W[1];
            //doc.Part.S[0].W[0] = new W();
            //doc.Part.S[0].W[0].LEMMA = "word";
            //doc.Part.S[0].W[0].offset = 0;
            //doc.Part.S[0].W[0].POS = "dsds";
            //doc.Part.S[0].W[0].Value = "aaaa";
            //doc.Part.S[0].NP = new NP[1];
            //doc.Part.S[0].NP[0] = new NP();
            //doc.Part.S[0].NP[0].W = new W[1];
            //doc.Part.S[0].NP[0].W[0] = new W();
            //doc.Part.S[0].NP[0].W[0].LEMMA = "hgh";
            //doc.Part.S[0].NP[0].W[0].offset = 0;
            //doc.Part.S[0].NP[0].W[0].POS = "asas";
            //doc.Part.S[0].NP[0].W[0].Value = "hgfhg";
            //doc.Part.S[0].NP[0].Head = new Head();
            //doc.Part.S[0].NP[0].Head.W = new W();
            //doc.Part.S[0].NP[0].Head.W.LEMMA = "sdas";
            //doc.Part.S[0].NP[0].Head.W.offset = 0;
            //doc.Part.S[0].NP[0].Head.W.POS = "ads";
            //doc.Part.S[0].NP[0].Head.W.Value = "sdfd";
            //doc.Part.S[0].NP[0].NPP = new NP[1];
            //doc.Part.S[0].NP[0].NPP[0] = new NP();
            //doc.Part.S[0].NP[0].NPP[0].W = new W[1];
            //doc.Part.S[0].NP[0].NPP[0].W[0] = new W();
            //doc.Part.S[0].NP[0].NPP[0].W[0].LEMMA = "dfssdf";
            //doc.Part.S[0].NP[0].NPP[0].W[0].offset = 0;
            //doc.Part.S[0].NP[0].NPP[0].W[0].POS = "dfd";
            //doc.Part.S[0].NP[0].NPP[0].W[0].Value = "dfdf";
            //doc.Part.S[0].NP[0].NPP[0].Head = new Head();
            //doc.Part.S[0].NP[0].NPP[0].Head.W = new W();
            //doc.Part.S[0].NP[0].NPP[0].Head.W.Value = "adsf";
            //doc.Part.S[0].NP[0].NPP[0].Head.W.POS = "ddd";
            //doc.Part.S[0].NP[0].NPP[0].Head.W.offset = 0;
            //doc.Part.S[0].NP[0].NPP[0].Head.W.LEMMA = "dfd";

            //string xml = HttpHandlers.SerializeToString<DOCUMENT>(doc);

            string xmlToDeserialize = "<DOCUMENT><P ID=\"1\"><S ID=\"1\" offset=\"0\"><NP HEADID=\"1.2\" ID=\"1\" TYPE=\"undefined\"><W Case=\"oblique\" Definiteness=\"no\" EXTRA=\"ParticipleLemma:potrivi(tranzitiv)\" Gender=\"masculine\" ID=\"1.1\" LEMMA=\"potrivit\" MSD=\"Afpmson\" Number=\"singular\" POS=\"ADJECTIVE\" offset=\"0\">Potrivit</W><HEAD><W Case=\"oblique\" Definiteness=\"yes\" Gender=\"masculine\" ID=\"1.2\" LEMMA=\"protocol\" MSD=\"Ncmsoy\" Number=\"singular\" POS=\"NOUN\" Type=\"common\" offset=\"9\">protocolului</W></HEAD><W ID=\"1.3\" LEMMA=\"dintre\" MSD=\"Sp\" POS=\"ADPOSITION\" offset=\"22\">dintre</W><NP HEADID=\"1.4\" ID=\"2\" TYPE=\"ORGANIZATION\"><HEAD><W Case=\"direct\" Definiteness=\"yes\" EXTRA=\"NotInDict\" Gender=\"feminine\" ID=\"1.4\" LEMMA=\"PSD\" MSD=\"Npfsry\" Number=\"singular\" POS=\"NOUN\" Type=\"proper\" offset=\"29\">PSD</W></HEAD></NP><W ID=\"1.5\" LEMMA=\"și\" MSD=\"Cc\" POS=\"CONJUNCTION\" Type=\"coordinating\" offset=\"33\">şi</W><NP HEADID=\"1.6\" ID=\"3\" TYPE=\"ORGANIZATION\"><HEAD><W EXTRA=\"NotInDict\" ID=\"1.6\" LEMMA=\"PNL\" MSD=\"Np\" POS=\"NOUN\" Type=\"proper\" offset=\"36\">PNL</W></HEAD></NP><W ID=\"1.7\" LEMMA=\",\" MSD=\"COMMA\" POS=\"\" offset=\"39\">,</W><W Case=\"direct\" Definiteness=\"no\" EXTRA=\"ParticipleLemma:parafa(tranzitiv)\" Gender=\"masculine\" ID=\"1.8\" LEMMA=\"parafat\" MSD=\"Afpmsrn\" Number=\"singular\" POS=\"ADJECTIVE\" offset=\"41\">parafat</W><NP HEADID=\"1.9\" ID=\"4\" TYPE=\"undefined\"><HEAD><W Case=\"direct\" Definiteness=\"no\" Gender=\"feminine\" ID=\"1.9\" LEMMA=\"sâmbătă\" MSD=\"Ncfsrn\" Number=\"singular\" POS=\"NOUN\" Type=\"common\" offset=\"49\">sâmbătă</W></HEAD></NP></NP><W ID=\"1.10\" LEMMA=\",\" MSD=\"COMMA\" POS=\"\" offset=\"56\">,</W><NP HEADID=\"1.11\" ID=\"5\" TYPE=\"undefined\"><HEAD><W Case=\"direct\" Definiteness=\"yes\" Gender=\"masculine\" ID=\"1.11\" LEMMA=\"social-democrat\" MSD=\"Ncmpry\" Number=\"plural\" POS=\"NOUN\" Type=\"common\" offset=\"58\">social-democraţii</W></HEAD></NP><W EXTRA=\"tranzitiv\" ID=\"1.12\" LEMMA=\"vrea\" MSD=\"Vmip3p\" Mood=\"indicative\" Number=\"plural\" POS=\"VERB\" Person=\"third\" Tense=\"present\" Type=\"predicative\" offset=\"76\">vor</W><W EXTRA=\"tranzitiv\" ID=\"1.13\" LEMMA=\"avea\" MSD=\"Vmn\" Mood=\"infinitive\" POS=\"VERB\" Type=\"predicative\" offset=\"80\">avea</W><NP HEADID=\"1.14\" ID=\"6\" TYPE=\"undefined\"><HEAD><W Case=\"direct\" Definiteness=\"no\" EXTRA=\"ParticipleLemma:candida(intranzitiv)\" Gender=\"masculine\" ID=\"1.14\" LEMMA=\"candidat\" MSD=\"Ncmprn\" Number=\"plural\" POS=\"NOUN\" Type=\"common\" offset=\"85\">candidaţi</W></HEAD><W ID=\"1.15\" LEMMA=\"la\" MSD=\"Sp\" POS=\"ADPOSITION\" offset=\"95\">la</W><NP HEADID=\"1.16\" ID=\"7\" TYPE=\"undefined\"><HEAD><W Case=\"direct\" Definiteness=\"yes\" Gender=\"feminine\" ID=\"1.16\" LEMMA=\"șefie\" MSD=\"Ncfsry\" Number=\"singular\" POS=\"NOUN\" Type=\"common\" offset=\"98\">şefia</W></HEAD></NP></NP><W Case=\"direct\" Gender=\"feminine\" ID=\"1.17\" LEMMA=\"al\" MSD=\"Tsfsr\" Number=\"singular\" POS=\"ARTICLE\" Type=\"possessive\" offset=\"104\">a</W><NP HEADID=\"1.20\" ID=\"8\" TYPE=\"undefined\"><W EXTRA=\"NotInDict\" ID=\"1.18\" LEMMA=\"21\" MSD=\"M\" POS=\"NUMERAL\" offset=\"106\">21</W><W ID=\"1.19\" LEMMA=\"de\" MSD=\"Sp\" POS=\"ADPOSITION\" offset=\"109\">de</W><HEAD><W Case=\"direct\" Definiteness=\"no\" Gender=\"feminine\" ID=\"1.20\" LEMMA=\"consiliu\" MSD=\"Ncfprn\" Number=\"plural\" POS=\"NOUN\" Type=\"common\" offset=\"112\">consilii</W></HEAD><W Case=\"direct\" Definiteness=\"no\" Gender=\"feminine\" ID=\"1.21\" LEMMA=\"județean\" MSD=\"Afpfprn\" Number=\"plural\" POS=\"ADJECTIVE\" offset=\"121\">judeţene</W><W ID=\"1.22\" LEMMA=\",\" MSD=\"COMMA\" POS=\"\" offset=\"129\">,</W><W ID=\"1.23\" LEMMA=\"iar\" MSD=\"Rg\" POS=\"ADVERB\" offset=\"131\">iar</W><NP HEADID=\"1.24\" ID=\"9\" TYPE=\"undefined\"><HEAD><W Case=\"direct\" Definiteness=\"yes\" Gender=\"masculine\" ID=\"1.24\" LEMMA=\"liberal\" MSD=\"Ncmpry\" Number=\"plural\" POS=\"NOUN\" Type=\"common\" offset=\"135\">liberalii</W></HEAD></NP></NP><W ID=\"1.25\" LEMMA=\"la\" MSD=\"Sp\" POS=\"ADPOSITION\" offset=\"145\">la</W><W EXTRA=\"NotInDict\" ID=\"1.26\" LEMMA=\"20\" MSD=\"M\" POS=\"NUMERAL\" offset=\"148\">20</W><W ID=\"1.27\" LEMMA=\".\" MSD=\"PERIOD\" POS=\"\" offset=\"150\">.</W></S></P></DOCUMENT>";
            DOCUMENT d = HttpHandlers.Deserialize<DOCUMENT>(xmlToDeserialize);


        }

 

        [TestMethod]
        public void TestSerivces()
        {
            string questionText = "Cât de mare este Galaxia noastră în diametru";
            string qf = "unde nascut poet contemporan pretutindeni . Este munte romania fost Constantin";
            Languages l = Languages.Romanian;
            Dictionary<int, string> Questions = new Dictionary<int, string>();

            //fosrte important remove semenele de punctuatie

            Questions.Add(1, "Cât de mare este Galaxia noastră în diametru");
            Questions.Add(2, "În ce zi este sărbătorită Ziua Boxului");
            Questions.Add(3, "Este legal să mergi la culoarea roşie a semaforului");
            Questions.Add(4, "Cum pot să aplic pentru un paşaport");
            Questions.Add(5, "What could be a consequence of a reduction in Arctic ice");
            //Analyser a = new Analyser(Questions, Languages.Romanian);
            //a.CreateQuestionList();
            //QuestionAnalisys.NpChunkerRoWS.NpChunkerRoWS s = new QuestionAnalisys.NpChunkerRoWS.NpChunkerRoWS();

            //string xml2 = a.PosTaggerRoWS(questionText);
  
        }

      
        internal string xml = "<?xml version=\"1.0\" encoding=\"UTF-8\"?><S id=\"1\" offset=\"0\"><NP><HEAD><W Case=\"direct\" Definiteness=\"yes\" Gender=\"feminine\" LEMMA=\"stradă\" MSD=\"Ncfsry\" Number=\"singular\" POS=\"NOUN\" Type=\"common\" id=\"1.1\" offset=\"0\">Strada</W></HEAD></NP></S>";

        [TestMethod]
        public void TestApiAlchemyLanguage()
        {
            Language l = new Language("Note: THIS IS NOT PRODUCTON CODE. This SDK is example code to get you started making programmatic calls to AlchemyAPI.");
            string result = l.GetLanguage();
        }

        [TestMethod]
        public void TestApiAlchemyKeywords()
        {
            Keywords  k = new Keywords( "Increasing the quality of the software, by better development methods, will affect the time needed for testing (the test phases) by:");
            string result = k.GetKeywords();

            string text =
              @"Order numbers on a stock control system can range between 10000 and 99999 inclusive. Which
of the following inputs might be a result of designing tests for only valid equivalence classes and
valid boundaries";
            Category c = new Category(text);
            string rresult = c.GetCategory(); // for topics (large text)

            //Concept cp = new Concept(text);
            //result = cp.GetConcept();

            Entity e = new Entity(text);
            string rrresult = e.GetEntity();

            Relations r = new Relations(text);
            string rrrresult = r.GetRelations();
        }

        [TestMethod]
        public void TestStemer()
        {
            TestStemmer(new EnglishStemmer(), "How would you estimate the amount of re-testing likely to be required");
            TestStemmer(new RomanianStemmer(), "bunic", "strabunic", "strabunica", "bunica");
        }
        private static void TestStemmer(IStemmer stemmer, params string[] words)
        {
           
            foreach (string word in words)
            {
                string s = stemmer.Stem(word);
            }
        }

        [TestMethod]
        public void TestBigHugeLabsApi()
        {
            BigHugeLabs a = new BigHugeLabs();
            string b = a.CallApi("system");
            Rootobject obj = JsonConvert.DeserializeObject<Rootobject>(b);
        }

        [TestMethod]
        public void ExtractText()
        {
            //TextProcessing.PdfProcessing text = new TextProcessing.PdfProcessing(@"C:\Users\madal_000\Desktop\Quiz\English\Skeletal System.pdf", 1, 2);
            TextProcessing.PdfProcessing text = new TextProcessing.PdfProcessing(@"C:\Users\madal_000\Desktop\Quiz\Biologie\5. Analizatorul vizual.pdf", 1, 12);
            List<TextDocument> list = text.GetAllText();
            ExtractInfo ex = new ExtractInfo(1, 1, 14, list[0].Text,5);
            ex.Extract();
            List<Question> questionList = ex.questionList;
            string query = ex.query;
            string  obj = JsonConvert.SerializeObject(questionList);
            List<Question> questionListDES = JsonConvert.DeserializeObject<List<Question>>(obj);
            if (questionList.Count > 0)
            {
                Analyser analize = new Analyser(questionList, query);
                analize.AnalizeQuestions();
                List<BackgroundDocument> bd = new List<BackgroundDocument>();
                BackgroundDocument b = new BackgroundDocument();
              // b.Path = @"C:\Users\madal_000\Desktop\Quiz\English\Abdominal cavity.pdf";
                b.Path = @"C:\Users\madal_000\Desktop\Quiz\Biologie\Analizatorul_vizual1.pdf";
                b.TestID = 1;
                b.TopicID = 1;
                b.Title = "BD";
                b.FileName = "BD";
                bd.Add(b);

                BackgroundDocument b2 = new BackgroundDocument();
                b2.Path = @"C:\Users\madal_000\Desktop\Quiz\Biologie\ANALIZATORULUI_VIZUAL2.pdf";
                b2.TestID = 2;
                b2.TopicID = 2;
                b2.Title = "AnalizatorulVizual";
                b2.FileName = "Analizatorul";
                bd.Add(b2);


                 AnswerEvaluation.AnswerExtract ae = new AnswerEvaluation.AnswerExtract(bd, analize.QuestionList, false);
                ae.AnswerAnalysis();
            }
            else
            {
                //errr message
            }
          
        }

        [TestMethod]
        public void IndexTextBD()
        {
            //TextProcessing.PdfProcessing textEnglish = new TextProcessing.PdfProcessing(@"C:\Users\madal_000\Dropbox\TESTS\iqtb\ISTQB Certification Exam Study Material.pdf", 1, 157);
            //TextProcessing.PdfProcessing textRomanian = new TextProcessing.PdfProcessing(@"C:\Users\madal_000\Dropbox\TESTS\Drept\ANA-MARIA ICHIM.pdf", 1, 94);
            ////List<TextDocument> listEnglish = textEnglish.GetAllText();
            //List<TextDocument> listRomanian = textRomanian.GetAllText();

            ////string[] paragrphEnglish = Regex.Split(listEnglish[0].Text, @"\r\n");
            //string[] paragrphRomanian = Regex.Split(listRomanian[0].Text, @"\r\n");


            string[] paragrphRomanian = new string[2];
            paragrphRomanian[0] = "persoane care justifică un interes legitim  iar soluţionarea cererilor este de competenţa instanţelor  judecătoreşti de drept comun care au plenitudine de jurisdicţie      acţiunea în nulitate absolută este imprescriptibilă  adică ea poate fi intentată oricând indiferent de  timpul scurs de la data încheierii actului  această regulă este expres prevăzută de art  2 din Decretul  nr  167 1958   Nulitatea unui act poate fi invocată oricând  fie pe cale de acţiune  fie pe cale de  excepţie      nulitatea absolută nu poate fi acoperită prin confirmare  expresă ori tacită  această regulă este  impusă de natura obştească a interesului ocrotit prin norma a cărei încălcare atrage nulitatea  absolută  Inadmisibilitatea confirmării nulităţii absolute nu se trebuie confundată cu validarea actului  prin îndeplinirea ulterioară a cerinţei legale  nerespectată în momentul încheierii actului (ex   obţinerea autorizaţiei administrative  până la anularea actului)  validare care decurge din concepţia  despre nulitate  ca şi din regula prevăzută de art  978 C  Civ   actus interpretandus est potius ut  valeat  quam ut pereat   2  Regimul juridic al nulităţii relative  Acest regim juridic se exprimă în următoarele trei reguli     nulitatea relativă poate fi invocată doar de persoana al cărei interes a fost nesocotit la încheierea  actului juridic  invocarea poate fi făcută personal de cel interesat  dacă are capacitatea necesară  pentru aceasta  dar poate fi făcută şi de reprezentantul legal al celui lipsit de capacitatea de exerciţiu     acţiunea în anulabilitate este prescriptibilă  ceea ce înseamnă că nulitatea relativă trebuie invocată  în termenul de prescripţie extinctivă  începutul prescripţiei acestei acţiuni este reglementată în art  9  din Decretul nr  167 1958     nulitatea relativă poate fi confirmată expres sau tacit  confirmarea expresă se realizează potrivit art   1190 C  Civ    Actul de confirmare sau ratificare a unei obligaţii  în contra căreia legea admite  acţiunea în nulitate nu este valabil decât atunci când cuprinde obiectul  cauza şi natura obligaţiei  şi  când face menţiune de motivul acţiunii în nulitate  precum şi despre intenţia de a repara viciul pe  care se întemeia acea acţiune   confirmarea tacită rezultă fie de executarea actului anulabil fie din  invocarea nulităţii înlăuntrul termenului de prescripţie extinctivă   Între nulitatea absolută şi nulitatea relativă nu există deosebiri de efecte   ci doar deosebiri de regim  juridic  Pe baza celor exprimate mai sus  aceste deosebiri se pot exprima  sintetic  astfel     dacă nulitatea absolută poate fi invocată de oricine are interes  chiar şi din oficiu  nulitatea relativă  poate fi invocată numai de persoana al cărei interes a fost de nesocotit la încheierea actului     dacă nulitatea absolută este imprescriptibilă  nulitatea relativă este prescriptibilă     dacă nulitatea absolută nu poate fi acoperită prin confirmare  nulitatea relativă poate fi confirmată   expres ori tacit   CONCLUZII  In alte cuvinte  nulitatea este sancţiunea ce intervine in cazul in care   la încheierea actului juridic civil  nu se respectă dispoziţiile legale referitoare la condiţiile de fond sau de forma   Pentru o mai buna inţelegere a instituţiei nulitaţii actului juridic civil presupune şi operaţiunea  delimitării acesteia de alte cauze de ineficacitate a actului juridic civil O asemenea delimitare  contribuie şi al evitarea confundârii nulitaţii actului juridic civil cu alte sancţiuni de drept civil  ";
            paragrphRomanian[1] = @"Regula (principiul) capacităţii de a încheia acte juridice şi excepţia incapacităţii în această materie, regula sau principiul îl reprezintă capacitatea de a încheia acte juridice civile, incapacitatea constituind excepţia. Cu caracter general, această regulă se desprinde implicit din art. 29 alin. (1) C.civ., conform căruia, „nimeni nu poate fi îngrădit în capacitatea de folosinţă sau lipsit, în tot sau în parte, de capacitatea de exerciţiu, decât în cazurile şi condiţiile expres prevăzute de lege”. Fragmentar, regula este consacrată şi în art. 1180 C.civ., potrivit căruia, „poate contracta orice persoană care nu este declarată incapabilă de lege şi nici oprită să încheie anumite contracte”, în art. 987 alin. (1) C.civ., care dispune că „orice persoană poate face şi primi liberalităţi, cu respectarea regulilor privind capacitatea”, precum şi în art. 1652 C.civ., care prevede că „pot cumpăra sau vinde toţi cei cărora nu le este interzis prin lege”. În legătură cu regula capacităţii de a încheia acte juridice civile, se impun două precizări. În primul rând, sub aspectul corelaţiei dintre capacitate şi discernământ, este de reţinut că, în timp ce capacitatea specialităţii o stare de drept (de iure), discernământul este o stare de fapt (de facto), care se apreciază de la persoană la persoană, în raport de aptitudinea şi puterea psiho-inte-lectivă ale acesteia; capacitatea izvorăşte numai din lege, pe când discernământul este de natură psihologică. în consecinţă, discernământul poate exista, izolat, chiar la o persoană incapabilă, după cum o persoană capabilă se poate găsi într-o situaţie în care, vremelnic, să nu aibă discernământ. În al doilea rând, pentru persoanele juridice, regula o constituie capacitatea de a dobândi orice drepturi şi obligaţii civile, afară de acelea care, prin natura lor sau potrivit legii, nu pot aparţine decât persoanei fizice [art. 206 alin. (1) C.civ.]. 
Capacitatea de a încheia actul juridic civil -capacitatea de a încheia actul juridic civil, prin capacitatea de a încheia actul juridic civil se înţelege aptitudinea subiectului de drept civil de a deveni titular de drepturi şi obligaţii civile prin încheierea actelor juridice civile. Capacitatea de a încheia acte juridice civile este o condiţie de fond, esenţială, de validitate şi generală a actului juridic civil.Capacitatea de a încheia actul juridic civil este numai o parte a capacităţii civile, reunind, în structura sa, o parte din capacitatea de folosinţă a persoanei fizice sau juridice, precum şi capacitatea de exerciţiu a acesteia.
Regula (principiul) capacităţii de a încheia acte juridice şi excepţia incapacităţii în această materie, regula sau principiul îl reprezintă capacitatea de a încheia acte juridice civile, incapacitatea constituind excepţia. Cu caracter general, această regulă se desprinde implicit din art. 29 alin. (1) C.civ., conform căruia, „nimeni nu poate fi îngrădit în capacitatea de folosinţă sau lipsit, în tot sau în parte, de capacitatea de exerciţiu, decât în cazurile şi condiţiile expres prevăzute de lege”. Fragmentar, regula este consacrată şi în art. 1180 C.civ., potrivit căruia, „poate contracta orice persoană care nu este declarată incapabilă de lege şi nici oprită să încheie anumite contracte”, în art. 987 alin. (1) C.civ., care dispune că „orice persoană poate face şi primi liberalităţi, cu respectarea regulilor privind capacitatea”, precum şi în art. 1652 C.civ., care prevede că „pot cumpăra sau vinde toţi cei cărora nu le este interzis prin lege”. În legătură cu regula capacităţii de a încheia acte juridice civile, se impun două precizări. În primul rând, sub aspectul corelaţiei dintre capacitate şi discernământ, este de reţinut că, în timp ce capacitatea constituie o stare de drept (de iure), discernământul este o stare de fapt (de facto), care se apreciază de la persoană la persoană, în raport de aptitudinea şi puterea psiho-inte-lectivă ale acesteia; capacitatea izvorăşte numai din lege, pe când discernământul este de natură psihologică. în consecinţă, discernământul poate exista, izolat, chiar la o persoană incapabilă, după cum o persoană capabilă se poate găsi într-o situaţie în care, vremelnic, să nu aibă discernământ. În al doilea rând, pentru persoanele juridice, regula o constituie capacitatea de a dobândi orice drepturi şi obligaţii civile, afară de acelea care, prin natura lor sau potrivit legii, nu pot aparţine decât persoanei fizice [art. 206 alin. (1) C.civ.]. în cazul persoanelor juridice fără scop lucrativ, regula capacităţii de a încheia acte juridice civile este subordonată principiului specialităţii, consacrat de art. 206 alin. (2) C.civ.";


            List<BackgroundDocument> bd = new List<BackgroundDocument>();
          

            BackgroundDocument bb = new BackgroundDocument();
            bb.TestID = 2;
            bb.FileName = "Drept";
            bb.Paragraphs = paragrphRomanian;
            bb.ProcessedParagraphs = paragrphRomanian;
            bb.Path = @"C:\Users\madal_000\Dropbox\TESTS\Drept\ANA-MARIA ICHIM.pdf";

            bd.Add(bb);
            TextProcessing.PdfProcessing test = new TextProcessing.PdfProcessing(bd);
            List<TextDocument> tt = test.GetAllText();


            string SearchTerm = "\"act*\" AND \" delimita*\"\"~4";
            string searchField = "ProcessedText";
            LuceneService indexing = new LuceneService();
            indexing.BuildIndex(bd);
            var result1 = indexing.Search(SearchTerm, searchField);

            //string SearchTerm = "contract OR vânzare";
            // string searchField = "Text";
            // var indexing = new LuceneService();
            //indexing.BuildIndex(bd);
            //var  result = indexing.Search(SearchTerm, searchField);

        }



    }
}
