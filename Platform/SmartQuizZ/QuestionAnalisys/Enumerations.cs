
namespace QuestionAnalisys
{
    public enum Languages : int
    {
        Romanian = 1,
        English = 2,
        Unrecognized = 3
    }

    public enum AdverbsE 
    {
        Why,
        What, 
        Where, 
        When ,
        Who ,
        Witch, 
        Whom ,
        How 
    }

    public enum AdverbsR : int
    {
        DeCe = 1,
        Ce = 2,
        Unde =3,
        Cand = 4,
        Cine = 5,
        CuCine = 6,
        PeCine = 7,
        Cum = 8
    }

    public enum CausalWordsE : int
    {
        Cause = 1,
        Result = 2,
        Determine = 3
    }

    public enum CausalWordsR : int
    {
        Cauza = 1,
        Rezultat = 2,
        Determina = 3
    }

    public enum QuestionType : int
    {
        FactoidLocation = 1,
        FactoidNumber = 2,
        FactoidPerson = 3,
        FactoidTime = 4,
        WhichIsTrue = 5,
        Method = 6,
        Reason = 7,
        Causal = 8,
        Other = 9
    }

    public enum AnswerTypeExpected : int
    {
        Location = 1,
        Date = 2,
        Person =3,
        Measurement = 4,
        Cause = 5,
        Method = 6,
        Purpose = 7,
        Fact = 8,
        Opinion = 9,
        Experience = 10,
        Reason = 11,
        Definition = 12,
        Possibility = 13,
        Illegal = 14,
        Factorid = 15,
        MultipleTypes = 16,
        Others = 17,
        Organisation = 18,
        Number = 19
    }

    public enum PartOfSpeech
    {
        VERB = 1,
        NOUN = 2,
        ADJECTIVE = 3,
        CONJUNCTION = 4,
        ADPOSITION = 5,
        ARTICLE = 6,
        ADVERB = 7,
        DETERMINER = 8,
        UNRECOGNIZED = 9,
        ABREVIATION = 10,
        INTERJECTION = 11,
        NUMERAL = 12,
        PRONOUN = 13,
        PARTICLE = 14,
        PREPOSITION = 15,
        MODAL = 16,
        PREDETERMINER = 17,
        WHDETERMINER = 18,
        WHPRONOUN = 19,
        WHADVERB = 20
    }
    public enum Services
    {
        NpChunkerRoWS = 1,
        Racai = 2,
        Standford = 3,
        NamedEntityRecognizerWS = 4
    }
    
}