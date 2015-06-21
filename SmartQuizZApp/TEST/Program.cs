
using System;
using System.IO;
using java.util;
using java.io;
using edu.stanford.nlp.pipeline;
using Console = System.Console;

namespace TEST
{
    class Program
    {
        static void Main(string[] args)
        {
            // Path to the folder with models extracted from `stanford-corenlp-3.4-models.jar`
            var jarRoot = @"c:E:\Madalina\Licenta\WorkSpace\SmartQuizZApp\QuestionAnalisys\stanford-corenlp-full-2014-10-31\";

            // Text for processing
            var text = "Kosgi Santosh sent an email to Stanford University. He didn&#39;t get a reply.";

            // Annotation pipeline configuration
            var props = new Properties();
            props.setProperty("annotators", "tokenize, ssplit, pos, lemma, ner, parse, dcoref");
            props.setProperty("sutime.binders", "0");

            // We should change current directory, so StanfordCoreNLP could find all the model files automatically 
            var curDir = Environment.CurrentDirectory;
            Directory.SetCurrentDirectory(jarRoot);
            var pipeline = new StanfordCoreNLP(props);
            Directory.SetCurrentDirectory(curDir);

            // Annotation
            var annotation = new Annotation(text);
            pipeline.annotate(annotation);

            // Result - Pretty Print
            using (var stream = new ByteArrayOutputStream())
            {
                pipeline.prettyPrint(annotation, new PrintWriter(stream));
                Console.WriteLine(stream.toString());
                stream.close();
            }
        }
    }
}
