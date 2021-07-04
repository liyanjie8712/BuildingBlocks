using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Liyanjie.EnglishPluralization.Internals;

namespace Liyanjie.EnglishPluralization
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class EnglishPluralization
    {
        #region

        readonly BidirectionalDictionary<string, string> userDictionary;

        readonly StringBidirectionalDictionary irregularPluralsPluralizationService;

        readonly StringBidirectionalDictionary assimilatedClassicalInflectionPluralizationService;

        readonly StringBidirectionalDictionary oSuffixPluralizationService;

        readonly StringBidirectionalDictionary classicalInflectionPluralizationService;

        readonly StringBidirectionalDictionary irregularVerbPluralizationService;

        readonly StringBidirectionalDictionary wordsEndingWithSePluralizationService;

        readonly StringBidirectionalDictionary wordsEndingWithSisPluralizationService;

        readonly List<string> knownSingluarWords;

        readonly List<string> knownPluralWords;

        readonly string[] uninflectiveSuffixes = new[]
        {
            #region
            "fish",
            "ois",
            "sheep",
            "deer",
            "pos",
            "itis",
            "ism"
           #endregion
        };

        readonly string[] uninflectiveWords = new[]
        {
            #region
            "bison",
            "flounder",
            "pliers",
            "bream",
            "gallows",
            "proceedings",
            "breeches",
            "graffiti",
            "rabies",
            "britches",
            "headquarters",
            "salmon",
            "carp",
            "herpes",
            "scissors",
            "chassis",
            "high-jinks",
            "sea-bass",
            "clippers",
            "homework",
            "series",
            "cod",
            "innings",
            "shears",
            "contretemps",
            "jackanapes",
            "species",
            "corps",
            "mackerel",
            "swine",
            "debris",
            "measles",
            "trout",
            "diabetes",
            "mews",
            "tuna",
            "djinn",
            "mumps",
            "whiting",
            "eland",
            "news",
            "wildebeest",
            "elk",
            "pincers",
            "police",
            "hair",
            "ice",
            "chaos",
            "milk",
            "cotton",
            "corn",
            "millet",
            "hay",
            "pneumonoultramicroscopicsilicovolcanoconiosis",
            "information",
            "rice",
            "tobacco",
            "aircraft",
            "rabies",
            "scabies",
            "diabetes",
            "traffic",
            "cotton",
            "corn",
            "millet",
            "rice",
            "hay",
            "hemp",
            "tobacco",
            "cabbage",
            "okra",
            "broccoli",
            "asparagus",
            "lettuce",
            "beef",
            "pork",
            "venison",
            "bison",
            "mutton",
            "cattle",
            "offspring",
            "molasses",
            "shambles",
            "shingles"
           #endregion
        };

        readonly Dictionary<string, string> irregularVerbList = new()
        {
            #region
            {
                "am",
                "are"
            },
            {
                "are",
                "are"
            },
            {
                "is",
                "are"
            },
            {
                "was",
                "were"
            },
            {
                "were",
                "were"
            },
            {
                "has",
                "have"
            },
            {
                "have",
                "have"
            }
            #endregion
        };

        readonly List<string> pronounList = new()
        {
            #region
            "I",
            "we",
            "you",
            "he",
            "she",
            "they",
            "it",
            "me",
            "us",
            "him",
            "her",
            "them",
            "myself",
            "ourselves",
            "yourself",
            "himself",
            "herself",
            "itself",
            "oneself",
            "oneselves",
            "my",
            "our",
            "your",
            "his",
            "their",
            "its",
            "mine",
            "yours",
            "hers",
            "theirs",
            "this",
            "that",
            "these",
            "those",
            "all",
            "another",
            "any",
            "anybody",
            "anyone",
            "anything",
            "both",
            "each",
            "other",
            "either",
            "everyone",
            "everybody",
            "everything",
            "most",
            "much",
            "nothing",
            "nobody",
            "none",
            "one",
            "others",
            "some",
            "somebody",
            "someone",
            "something",
            "what",
            "whatever",
            "which",
            "whichever",
            "who",
            "whoever",
            "whom",
            "whomever",
            "whose"
            #endregion
        };

        readonly Dictionary<string, string> irregularPluralsList = new()
        {
            #region
            {
                "brother",
                "brothers"
            },
            {
                "child",
                "children"
            },
            {
                "cow",
                "cows"
            },
            {
                "ephemeris",
                "ephemerides"
            },
            {
                "genie",
                "genies"
            },
            {
                "money",
                "moneys"
            },
            {
                "mongoose",
                "mongooses"
            },
            {
                "mythos",
                "mythoi"
            },
            {
                "octopus",
                "octopuses"
            },
            {
                "ox",
                "oxen"
            },
            {
                "soliloquy",
                "soliloquies"
            },
            {
                "trilby",
                "trilbys"
            },
            {
                "crisis",
                "crises"
            },
            {
                "synopsis",
                "synopses"
            },
            {
                "rose",
                "roses"
            },
            {
                "gas",
                "gases"
            },
            {
                "bus",
                "buses"
            },
            {
                "axis",
                "axes"
            },
            {
                "memo",
                "memos"
            },
            {
                "casino",
                "casinos"
            },
            {
                "silo",
                "silos"
            },
            {
                "stereo",
                "stereos"
            },
            {
                "studio",
                "studios"
            },
            {
                "lens",
                "lenses"
            },
            {
                "alias",
                "aliases"
            },
            {
                "pie",
                "pies"
            },
            {
                "corpus",
                "corpora"
            },
            {
                "viscus",
                "viscera"
            },
            {
                "hippopotamus",
                "hippopotami"
            },
            {
                "trace",
                "traces"
            },
            {
                "person",
                "people"
            },
            {
                "chilli",
                "chillies"
            },
            {
                "analysis",
                "analyses"
            },
            {
                "basis",
                "bases"
            },
            {
                "neurosis",
                "neuroses"
            },
            {
                "oasis",
                "oases"
            },
            {
                "synthesis",
                "syntheses"
            },
            {
                "thesis",
                "theses"
            },
            {
                "pneumonoultramicroscopicsilicovolcanoconiosis",
                "pneumonoultramicroscopicsilicovolcanoconioses"
            },
            {
                "status",
                "statuses"
            },
            {
                "prospectus",
                "prospectuses"
            },
            {
                "change",
                "changes"
            },
            {
                "lie",
                "lies"
            },
            {
                "calorie",
                "calories"
            },
            {
                "freebie",
                "freebies"
            },
            {
                "case",
                "cases"
            },
            {
                "house",
                "houses"
            },
            {
                "valve",
                "valves"
            },
            {
                "cloth",
                "clothes"
            }
            #endregion
        };

        readonly Dictionary<string, string> assimilatedClassicalInflectionList = new()
        {
            #region
            {
                "alumna",
                "alumnae"
            },
            {
                "alga",
                "algae"
            },
            {
                "vertebra",
                "vertebrae"
            },
            {
                "codex",
                "codices"
            },
            {
                "murex",
                "murices"
            },
            {
                "silex",
                "silices"
            },
            {
                "aphelion",
                "aphelia"
            },
            {
                "hyperbaton",
                "hyperbata"
            },
            {
                "perihelion",
                "perihelia"
            },
            {
                "asyndeton",
                "asyndeta"
            },
            {
                "noumenon",
                "noumena"
            },
            {
                "phenomenon",
                "phenomena"
            },
            {
                "criterion",
                "criteria"
            },
            {
                "organon",
                "organa"
            },
            {
                "prolegomenon",
                "prolegomena"
            },
            {
                "agendum",
                "agenda"
            },
            {
                "datum",
                "data"
            },
            {
                "extremum",
                "extrema"
            },
            {
                "bacterium",
                "bacteria"
            },
            {
                "desideratum",
                "desiderata"
            },
            {
                "stratum",
                "strata"
            },
            {
                "candelabrum",
                "candelabra"
            },
            {
                "erratum",
                "errata"
            },
            {
                "ovum",
                "ova"
            },
            {
                "forum",
                "fora"
            },
            {
                "addendum",
                "addenda"
            },
            {
                "stadium",
                "stadia"
            },
            {
                "automaton",
                "automata"
            },
            {
                "polyhedron",
                "polyhedra"
            }
            #endregion
        };

        readonly Dictionary<string, string> oSuffixList = new()
        {
            #region
            {
                "albino",
                "albinos"
            },
            {
                "generalissimo",
                "generalissimos"
            },
            {
                "manifesto",
                "manifestos"
            },
            {
                "archipelago",
                "archipelagos"
            },
            {
                "ghetto",
                "ghettos"
            },
            {
                "medico",
                "medicos"
            },
            {
                "armadillo",
                "armadillos"
            },
            {
                "guano",
                "guanos"
            },
            {
                "octavo",
                "octavos"
            },
            {
                "commando",
                "commandos"
            },
            {
                "inferno",
                "infernos"
            },
            {
                "photo",
                "photos"
            },
            {
                "ditto",
                "dittos"
            },
            {
                "jumbo",
                "jumbos"
            },
            {
                "pro",
                "pros"
            },
            {
                "dynamo",
                "dynamos"
            },
            {
                "lingo",
                "lingos"
            },
            {
                "quarto",
                "quartos"
            },
            {
                "embryo",
                "embryos"
            },
            {
                "lumbago",
                "lumbagos"
            },
            {
                "rhino",
                "rhinos"
            },
            {
                "fiasco",
                "fiascos"
            },
            {
                "magneto",
                "magnetos"
            },
            {
                "stylo",
                "stylos"
            }
            #endregion
        };

        readonly Dictionary<string, string> classicalInflectionList = new()
        {
            #region
            {
                "stamen",
                "stamina"
            },
            {
                "foramen",
                "foramina"
            },
            {
                "lumen",
                "lumina"
            },
            {
                "anathema",
                "anathemata"
            },
            {
                "enema",
                "enemata"
            },
            {
                "oedema",
                "oedemata"
            },
            {
                "bema",
                "bemata"
            },
            {
                "enigma",
                "enigmata"
            },
            {
                "sarcoma",
                "sarcomata"
            },
            {
                "carcinoma",
                "carcinomata"
            },
            {
                "gumma",
                "gummata"
            },
            {
                "schema",
                "schemata"
            },
            {
                "charisma",
                "charismata"
            },
            {
                "lemma",
                "lemmata"
            },
            {
                "soma",
                "somata"
            },
            {
                "diploma",
                "diplomata"
            },
            {
                "lymphoma",
                "lymphomata"
            },
            {
                "stigma",
                "stigmata"
            },
            {
                "dogma",
                "dogmata"
            },
            {
                "magma",
                "magmata"
            },
            {
                "stoma",
                "stomata"
            },
            {
                "drama",
                "dramata"
            },
            {
                "melisma",
                "melismata"
            },
            {
                "trauma",
                "traumata"
            },
            {
                "edema",
                "edemata"
            },
            {
                "miasma",
                "miasmata"
            },
            {
                "abscissa",
                "abscissae"
            },
            {
                "formula",
                "formulae"
            },
            {
                "medusa",
                "medusae"
            },
            {
                "amoeba",
                "amoebae"
            },
            {
                "hydra",
                "hydrae"
            },
            {
                "nebula",
                "nebulae"
            },
            {
                "antenna",
                "antennae"
            },
            {
                "hyperbola",
                "hyperbolae"
            },
            {
                "nova",
                "novae"
            },
            {
                "aurora",
                "aurorae"
            },
            {
                "lacuna",
                "lacunae"
            },
            {
                "parabola",
                "parabolae"
            },
            {
                "apex",
                "apices"
            },
            {
                "latex",
                "latices"
            },
            {
                "vertex",
                "vertices"
            },
            {
                "cortex",
                "cortices"
            },
            {
                "pontifex",
                "pontifices"
            },
            {
                "vortex",
                "vortices"
            },
            {
                "index",
                "indices"
            },
            {
                "simplex",
                "simplices"
            },
            {
                "iris",
                "irides"
            },
            {
                "clitoris",
                "clitorides"
            },
            {
                "alto",
                "alti"
            },
            {
                "contralto",
                "contralti"
            },
            {
                "soprano",
                "soprani"
            },
            {
                "basso",
                "bassi"
            },
            {
                "crescendo",
                "crescendi"
            },
            {
                "tempo",
                "tempi"
            },
            {
                "canto",
                "canti"
            },
            {
                "solo",
                "soli"
            },
            {
                "aquarium",
                "aquaria"
            },
            {
                "interregnum",
                "interregna"
            },
            {
                "quantum",
                "quanta"
            },
            {
                "compendium",
                "compendia"
            },
            {
                "lustrum",
                "lustra"
            },
            {
                "rostrum",
                "rostra"
            },
            {
                "consortium",
                "consortia"
            },
            {
                "maximum",
                "maxima"
            },
            {
                "spectrum",
                "spectra"
            },
            {
                "cranium",
                "crania"
            },
            {
                "medium",
                "media"
            },
            {
                "speculum",
                "specula"
            },
            {
                "curriculum",
                "curricula"
            },
            {
                "memorandum",
                "memoranda"
            },
            {
                "stadium",
                "stadia"
            },
            {
                "dictum",
                "dicta"
            },
            {
                "millenium",
                "millenia"
            },
            {
                "trapezium",
                "trapezia"
            },
            {
                "emporium",
                "emporia"
            },
            {
                "minimum",
                "minima"
            },
            {
                "ultimatum",
                "ultimata"
            },
            {
                "enconium",
                "enconia"
            },
            {
                "momentum",
                "momenta"
            },
            {
                "vacuum",
                "vacua"
            },
            {
                "gymnasium",
                "gymnasia"
            },
            {
                "optimum",
                "optima"
            },
            {
                "velum",
                "vela"
            },
            {
                "honorarium",
                "honoraria"
            },
            {
                "phylum",
                "phyla"
            },
            {
                "focus",
                "foci"
            },
            {
                "nimbus",
                "nimbi"
            },
            {
                "succubus",
                "succubi"
            },
            {
                "fungus",
                "fungi"
            },
            {
                "nucleolus",
                "nucleoli"
            },
            {
                "torus",
                "tori"
            },
            {
                "genius",
                "genii"
            },
            {
                "radius",
                "radii"
            },
            {
                "umbilicus",
                "umbilici"
            },
            {
                "incubus",
                "incubi"
            },
            {
                "stylus",
                "styli"
            },
            {
                "uterus",
                "uteri"
            },
            {
                "stimulus",
                "stimuli"
            },
            {
                "apparatus",
                "apparatus"
            },
            {
                "impetus",
                "impetus"
            },
            {
                "prospectus",
                "prospectus"
            },
            {
                "cantus",
                "cantus"
            },
            {
                "nexus",
                "nexus"
            },
            {
                "sinus",
                "sinus"
            },
            {
                "coitus",
                "coitus"
            },
            {
                "plexus",
                "plexus"
            },
            {
                "status",
                "status"
            },
            {
                "hiatus",
                "hiatus"
            },
            {
                "afreet",
                "afreeti"
            },
            {
                "afrit",
                "afriti"
            },
            {
                "efreet",
                "efreeti"
            },
            {
                "cherub",
                "cherubim"
            },
            {
                "goy",
                "goyim"
            },
            {
                "seraph",
                "seraphim"
            },
            {
                "alumnus",
                "alumni"
            }
            #endregion
        };

        readonly List<string> knownConflictingPluralList = new()
        {
            #region
            "they",
            "them",
            "their",
            "have",
            "were",
            "yourself",
            "are"
            #endregion
        };

        readonly Dictionary<string, string> wordsEndingWithSeList = new()
        {
            #region
            {
                "house",
                "houses"
            },
            {
                "case",
                "cases"
            },
            {
                "enterprise",
                "enterprises"
            },
            {
                "purchase",
                "purchases"
            },
            {
                "surprise",
                "surprises"
            },
            {
                "release",
                "releases"
            },
            {
                "disease",
                "diseases"
            },
            {
                "promise",
                "promises"
            },
            {
                "refuse",
                "refuses"
            },
            {
                "whose",
                "whoses"
            },
            {
                "phase",
                "phases"
            },
            {
                "noise",
                "noises"
            },
            {
                "nurse",
                "nurses"
            },
            {
                "rose",
                "roses"
            },
            {
                "franchise",
                "franchises"
            },
            {
                "supervise",
                "supervises"
            },
            {
                "farmhouse",
                "farmhouses"
            },
            {
                "suitcase",
                "suitcases"
            },
            {
                "recourse",
                "recourses"
            },
            {
                "impulse",
                "impulses"
            },
            {
                "license",
                "licenses"
            },
            {
                "diocese",
                "dioceses"
            },
            {
                "excise",
                "excises"
            },
            {
                "demise",
                "demises"
            },
            {
                "blouse",
                "blouses"
            },
            {
                "bruise",
                "bruises"
            },
            {
                "misuse",
                "misuses"
            },
            {
                "curse",
                "curses"
            },
            {
                "prose",
                "proses"
            },
            {
                "purse",
                "purses"
            },
            {
                "goose",
                "gooses"
            },
            {
                "tease",
                "teases"
            },
            {
                "poise",
                "poises"
            },
            {
                "vase",
                "vases"
            },
            {
                "fuse",
                "fuses"
            },
            {
                "muse",
                "muses"
            },
            {
                "slaughterhouse",
                "slaughterhouses"
            },
            {
                "clearinghouse",
                "clearinghouses"
            },
            {
                "endonuclease",
                "endonucleases"
            },
            {
                "steeplechase",
                "steeplechases"
            },
            {
                "metamorphose",
                "metamorphoses"
            },
            {
                "intercourse",
                "intercourses"
            },
            {
                "commonsense",
                "commonsenses"
            },
            {
                "intersperse",
                "intersperses"
            },
            {
                "merchandise",
                "merchandises"
            },
            {
                "phosphatase",
                "phosphatases"
            },
            {
                "summerhouse",
                "summerhouses"
            },
            {
                "watercourse",
                "watercourses"
            },
            {
                "catchphrase",
                "catchphrases"
            },
            {
                "compromise",
                "compromises"
            },
            {
                "greenhouse",
                "greenhouses"
            },
            {
                "lighthouse",
                "lighthouses"
            },
            {
                "paraphrase",
                "paraphrases"
            },
            {
                "mayonnaise",
                "mayonnaises"
            },
            {
                "racecourse",
                "racecourses"
            },
            {
                "apocalypse",
                "apocalypses"
            },
            {
                "courthouse",
                "courthouses"
            },
            {
                "powerhouse",
                "powerhouses"
            },
            {
                "storehouse",
                "storehouses"
            },
            {
                "glasshouse",
                "glasshouses"
            },
            {
                "hypotenuse",
                "hypotenuses"
            },
            {
                "peroxidase",
                "peroxidases"
            },
            {
                "pillowcase",
                "pillowcases"
            },
            {
                "roundhouse",
                "roundhouses"
            },
            {
                "streetwise",
                "streetwises"
            },
            {
                "expertise",
                "expertises"
            },
            {
                "discourse",
                "discourses"
            },
            {
                "warehouse",
                "warehouses"
            },
            {
                "staircase",
                "staircases"
            },
            {
                "workhouse",
                "workhouses"
            },
            {
                "briefcase",
                "briefcases"
            },
            {
                "clubhouse",
                "clubhouses"
            },
            {
                "clockwise",
                "clockwises"
            },
            {
                "concourse",
                "concourses"
            },
            {
                "playhouse",
                "playhouses"
            },
            {
                "turquoise",
                "turquoises"
            },
            {
                "boathouse",
                "boathouses"
            },
            {
                "cellulose",
                "celluloses"
            },
            {
                "epitomise",
                "epitomises"
            },
            {
                "gatehouse",
                "gatehouses"
            },
            {
                "grandiose",
                "grandioses"
            },
            {
                "menopause",
                "menopauses"
            },
            {
                "penthouse",
                "penthouses"
            },
            {
                "racehorse",
                "racehorses"
            },
            {
                "transpose",
                "transposes"
            },
            {
                "almshouse",
                "almshouses"
            },
            {
                "customise",
                "customises"
            },
            {
                "footloose",
                "footlooses"
            },
            {
                "galvanise",
                "galvanises"
            },
            {
                "princesse",
                "princesses"
            },
            {
                "universe",
                "universes"
            },
            {
                "workhorse",
                "workhorses"
            }
            #endregion
        };

        readonly Dictionary<string, string> wordsEndingWithSisList = new()
        {
            #region
            {
                "analysis",
                "analyses"
            },
            {
                "crisis",
                "crises"
            },
            {
                "basis",
                "bases"
            },
            {
                "atherosclerosis",
                "atheroscleroses"
            },
            {
                "electrophoresis",
                "electrophoreses"
            },
            {
                "psychoanalysis",
                "psychoanalyses"
            },
            {
                "photosynthesis",
                "photosyntheses"
            },
            {
                "amniocentesis",
                "amniocenteses"
            },
            {
                "metamorphosis",
                "metamorphoses"
            },
            {
                "toxoplasmosis",
                "toxoplasmoses"
            },
            {
                "endometriosis",
                "endometrioses"
            },
            {
                "tuberculosis",
                "tuberculoses"
            },
            {
                "pathogenesis",
                "pathogeneses"
            },
            {
                "osteoporosis",
                "osteoporoses"
            },
            {
                "parenthesis",
                "parentheses"
            },
            {
                "anastomosis",
                "anastomoses"
            },
            {
                "peristalsis",
                "peristalses"
            },
            {
                "hypothesis",
                "hypotheses"
            },
            {
                "antithesis",
                "antitheses"
            },
            {
                "apotheosis",
                "apotheoses"
            },
            {
                "thrombosis",
                "thromboses"
            },
            {
                "diagnosis",
                "diagnoses"
            },
            {
                "synthesis",
                "syntheses"
            },
            {
                "paralysis",
                "paralyses"
            },
            {
                "prognosis",
                "prognoses"
            },
            {
                "cirrhosis",
                "cirrhoses"
            },
            {
                "sclerosis",
                "scleroses"
            },
            {
                "psychosis",
                "psychoses"
            },
            {
                "apoptosis",
                "apoptoses"
            },
            {
                "symbiosis",
                "symbioses"
            }
            #endregion
        };

        #endregion

        /// <summary>
        /// Constructs a new instance of default pluralization service
        /// </summary>
        public EnglishPluralization()
        {
            userDictionary = new BidirectionalDictionary<string, string>();
            irregularPluralsPluralizationService = new StringBidirectionalDictionary(irregularPluralsList);
            assimilatedClassicalInflectionPluralizationService = new StringBidirectionalDictionary(assimilatedClassicalInflectionList);
            oSuffixPluralizationService = new StringBidirectionalDictionary(oSuffixList);
            classicalInflectionPluralizationService = new StringBidirectionalDictionary(classicalInflectionList);
            wordsEndingWithSePluralizationService = new StringBidirectionalDictionary(wordsEndingWithSeList);
            wordsEndingWithSisPluralizationService = new StringBidirectionalDictionary(wordsEndingWithSisList);
            irregularVerbPluralizationService = new StringBidirectionalDictionary(irregularVerbList);
            knownSingluarWords = new List<string>(irregularPluralsList.Keys.Concat(assimilatedClassicalInflectionList.Keys).Concat(oSuffixList.Keys).Concat(classicalInflectionList.Keys).Concat(irregularVerbList.Keys).Concat(uninflectiveWords).Except(knownConflictingPluralList));
            knownPluralWords = new List<string>(irregularPluralsList.Values.Concat(assimilatedClassicalInflectionList.Values).Concat(oSuffixList.Values).Concat(classicalInflectionList.Values).Concat(irregularVerbList.Values).Concat(uninflectiveWords));
        }

        /// <summary>
        /// Constructs a new instance of default pluralization service
        /// </summary>
        /// <param name="userDictionaryEntries">A collection of user dictionary entries to be used by this service.These inputs can customize the service according the user needs.</param>
        public EnglishPluralization(IEnumerable<CustomEnglishPluralizationEntry> userDictionaryEntries) : this()
        {
            Check.NotNull<IEnumerable<CustomEnglishPluralizationEntry>>(userDictionaryEntries, "userDictionaryEntries");
            foreach (CustomEnglishPluralizationEntry current in userDictionaryEntries)
            {
                userDictionary.AddValue(current.Singular, current.Plural);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="word"></param>
        /// <returns></returns>
        public string Pluralize(string word)
        {
            return Capitalize(word, new Func<string, string>(InternalPluralize));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="word"></param>
        /// <returns></returns>
        public string Singularize(string word)
        {
            return Capitalize(word, new Func<string, string>(InternalSingularize));
        }

        #region

        string InternalPluralize(string word)
        {
            if (userDictionary.ExistsInFirst(word))
            {
                return userDictionary.GetSecondValue(word);
            }
            if (IsNoOpWord(word))
            {
                return word;
            }
            var suffixWord = GetSuffixWord(word, out var str);
            if (IsNoOpWord(suffixWord))
            {
                return str + suffixWord;
            }
            if (IsUninflective(suffixWord))
            {
                return str + suffixWord;
            }
            if (knownPluralWords.Contains(suffixWord.ToLowerInvariant()) || IsPlural(suffixWord))
            {
                return str + suffixWord;
            }
            if (irregularPluralsPluralizationService.ExistsInFirst(suffixWord))
            {
                return str + irregularPluralsPluralizationService.GetSecondValue(suffixWord);
            }
            if (EnglishPluralizationServiceUtil.TryInflectOnSuffixInWord(suffixWord, new List<string>
            {
                "man"
            }, (string s) => s.Remove(s.Length - 2, 2) + "en", out var str2))
            {
                return str + str2;
            }
            if (EnglishPluralizationServiceUtil.TryInflectOnSuffixInWord(suffixWord, new List<string>
            {
                "louse",
                "mouse"
            }, (string s) => s.Remove(s.Length - 4, 4) + "ice", out str2))
            {
                return str + str2;
            }
            if (EnglishPluralizationServiceUtil.TryInflectOnSuffixInWord(suffixWord, new List<string>
            {
                "tooth"
            }, (string s) => s.Remove(s.Length - 4, 4) + "eeth", out str2))
            {
                return str + str2;
            }
            if (EnglishPluralizationServiceUtil.TryInflectOnSuffixInWord(suffixWord, new List<string>
            {
                "goose"
            }, (string s) => s.Remove(s.Length - 4, 4) + "eese", out str2))
            {
                return str + str2;
            }
            if (EnglishPluralizationServiceUtil.TryInflectOnSuffixInWord(suffixWord, new List<string>
            {
                "foot"
            }, (string s) => s.Remove(s.Length - 3, 3) + "eet", out str2))
            {
                return str + str2;
            }
            if (EnglishPluralizationServiceUtil.TryInflectOnSuffixInWord(suffixWord, new List<string>
            {
                "zoon"
            }, (string s) => s.Remove(s.Length - 3, 3) + "oa", out str2))
            {
                return str + str2;
            }
            if (EnglishPluralizationServiceUtil.TryInflectOnSuffixInWord(suffixWord, new List<string>
            {
                "cis",
                "sis",
                "xis"
            }, (string s) => s.Remove(s.Length - 2, 2) + "es", out str2))
            {
                return str + str2;
            }
            if (assimilatedClassicalInflectionPluralizationService.ExistsInFirst(suffixWord))
            {
                return str + assimilatedClassicalInflectionPluralizationService.GetSecondValue(suffixWord);
            }
            if (classicalInflectionPluralizationService.ExistsInFirst(suffixWord))
            {
                return str + classicalInflectionPluralizationService.GetSecondValue(suffixWord);
            }
            if (EnglishPluralizationServiceUtil.TryInflectOnSuffixInWord(suffixWord, new List<string>
            {
                "trix"
            }, (string s) => s.Remove(s.Length - 1, 1) + "ces", out str2))
            {
                return str + str2;
            }
            if (EnglishPluralizationServiceUtil.TryInflectOnSuffixInWord(suffixWord, new List<string>
            {
                "eau",
                "ieu"
            }, (string s) => s + "x", out str2))
            {
                return str + str2;
            }
            if (EnglishPluralizationServiceUtil.TryInflectOnSuffixInWord(suffixWord, new List<string>
            {
                "inx",
                "anx",
                "ynx"
            }, (string s) => s.Remove(s.Length - 1, 1) + "ges", out str2))
            {
                return str + str2;
            }
            if (EnglishPluralizationServiceUtil.TryInflectOnSuffixInWord(suffixWord, new List<string>
            {
                "ch",
                "sh",
                "ss"
            }, (string s) => s + "es", out str2))
            {
                return str + str2;
            }
            if (EnglishPluralizationServiceUtil.TryInflectOnSuffixInWord(suffixWord, new List<string>
            {
                "alf",
                "elf",
                "olf",
                "eaf",
                "arf"
            }, delegate (string s)
            {
                if (!s.EndsWith("deaf", StringComparison.OrdinalIgnoreCase))
                {
                    return s.Remove(s.Length - 1, 1) + "ves";
                }
                return s;
            }, out str2))
            {
                return str + str2;
            }
            if (EnglishPluralizationServiceUtil.TryInflectOnSuffixInWord(suffixWord, new List<string>
            {
                "nife",
                "life",
                "wife"
            }, (string s) => s.Remove(s.Length - 2, 2) + "ves", out str2))
            {
                return str + str2;
            }
            if (EnglishPluralizationServiceUtil.TryInflectOnSuffixInWord(suffixWord, new List<string>
            {
                "ay",
                "ey",
                "iy",
                "oy",
                "uy"
            }, (string s) => s + "s", out str2))
            {
                return str + str2;
            }
            if (suffixWord.EndsWith("y", StringComparison.OrdinalIgnoreCase))
            {
                return str + suffixWord.Remove(suffixWord.Length - 1, 1) + "ies";
            }
            if (oSuffixPluralizationService.ExistsInFirst(suffixWord))
            {
                return str + oSuffixPluralizationService.GetSecondValue(suffixWord);
            }
            if (EnglishPluralizationServiceUtil.TryInflectOnSuffixInWord(suffixWord, new List<string>
            {
                "ao",
                "eo",
                "io",
                "oo",
                "uo"
            }, (string s) => s + "s", out str2))
            {
                return str + str2;
            }
            if (suffixWord.EndsWith("o", StringComparison.OrdinalIgnoreCase))
            {
                return str + suffixWord + "es";
            }
            if (suffixWord.EndsWith("x", StringComparison.OrdinalIgnoreCase))
            {
                return str + suffixWord + "es";
            }
            return str + suffixWord + "s";
        }

        string InternalSingularize(string word)
        {
            if (userDictionary.ExistsInSecond(word))
            {
                return userDictionary.GetFirstValue(word);
            }
            if (IsNoOpWord(word))
            {
                return word;
            }
            var suffixWord = GetSuffixWord(word, out var str);
            if (IsNoOpWord(suffixWord))
            {
                return str + suffixWord;
            }
            if (IsUninflective(suffixWord))
            {
                return str + suffixWord;
            }
            if (knownSingluarWords.Contains(suffixWord.ToLowerInvariant()))
            {
                return str + suffixWord;
            }
            if (irregularVerbPluralizationService.ExistsInSecond(suffixWord))
            {
                return str + irregularVerbPluralizationService.GetFirstValue(suffixWord);
            }
            if (irregularPluralsPluralizationService.ExistsInSecond(suffixWord))
            {
                return str + irregularPluralsPluralizationService.GetFirstValue(suffixWord);
            }
            if (wordsEndingWithSisPluralizationService.ExistsInSecond(suffixWord))
            {
                return str + wordsEndingWithSisPluralizationService.GetFirstValue(suffixWord);
            }
            if (wordsEndingWithSePluralizationService.ExistsInSecond(suffixWord))
            {
                return str + wordsEndingWithSePluralizationService.GetFirstValue(suffixWord);
            }
            if (EnglishPluralizationServiceUtil.TryInflectOnSuffixInWord(suffixWord, new List<string>
            {
                "men"
            }, (string s) => s.Remove(s.Length - 2, 2) + "an", out var str2))
            {
                return str + str2;
            }
            if (EnglishPluralizationServiceUtil.TryInflectOnSuffixInWord(suffixWord, new List<string>
            {
                "lice",
                "mice"
            }, (string s) => s.Remove(s.Length - 3, 3) + "ouse", out str2))
            {
                return str + str2;
            }
            if (EnglishPluralizationServiceUtil.TryInflectOnSuffixInWord(suffixWord, new List<string>
            {
                "teeth"
            }, (string s) => s.Remove(s.Length - 4, 4) + "ooth", out str2))
            {
                return str + str2;
            }
            if (EnglishPluralizationServiceUtil.TryInflectOnSuffixInWord(suffixWord, new List<string>
            {
                "geese"
            }, (string s) => s.Remove(s.Length - 4, 4) + "oose", out str2))
            {
                return str + str2;
            }
            if (EnglishPluralizationServiceUtil.TryInflectOnSuffixInWord(suffixWord, new List<string>
            {
                "feet"
            }, (string s) => s.Remove(s.Length - 3, 3) + "oot", out str2))
            {
                return str + str2;
            }
            if (EnglishPluralizationServiceUtil.TryInflectOnSuffixInWord(suffixWord, new List<string>
            {
                "zoa"
            }, (string s) => s.Remove(s.Length - 2, 2) + "oon", out str2))
            {
                return str + str2;
            }
            if (EnglishPluralizationServiceUtil.TryInflectOnSuffixInWord(suffixWord, new List<string>
            {
                "ches",
                "shes",
                "sses"
            }, (string s) => s.Remove(s.Length - 2, 2), out str2))
            {
                return str + str2;
            }
            if (assimilatedClassicalInflectionPluralizationService.ExistsInSecond(suffixWord))
            {
                return str + assimilatedClassicalInflectionPluralizationService.GetFirstValue(suffixWord);
            }
            if (classicalInflectionPluralizationService.ExistsInSecond(suffixWord))
            {
                return str + classicalInflectionPluralizationService.GetFirstValue(suffixWord);
            }
            if (EnglishPluralizationServiceUtil.TryInflectOnSuffixInWord(suffixWord, new List<string>
            {
                "trices"
            }, (string s) => s.Remove(s.Length - 3, 3) + "x", out str2))
            {
                return str + str2;
            }
            if (EnglishPluralizationServiceUtil.TryInflectOnSuffixInWord(suffixWord, new List<string>
            {
                "eaux",
                "ieux"
            }, (string s) => s.Remove(s.Length - 1, 1), out str2))
            {
                return str + str2;
            }
            if (EnglishPluralizationServiceUtil.TryInflectOnSuffixInWord(suffixWord, new List<string>
            {
                "inges",
                "anges",
                "ynges"
            }, (string s) => s.Remove(s.Length - 3, 3) + "x", out str2))
            {
                return str + str2;
            }
            if (EnglishPluralizationServiceUtil.TryInflectOnSuffixInWord(suffixWord, new List<string>
            {
                "alves",
                "elves",
                "olves",
                "eaves",
                "arves"
            }, (string s) => s.Remove(s.Length - 3, 3) + "f", out str2))
            {
                return str + str2;
            }
            if (EnglishPluralizationServiceUtil.TryInflectOnSuffixInWord(suffixWord, new List<string>
            {
                "nives",
                "lives",
                "wives"
            }, (string s) => s.Remove(s.Length - 3, 3) + "fe", out str2))
            {
                return str + str2;
            }
            if (EnglishPluralizationServiceUtil.TryInflectOnSuffixInWord(suffixWord, new List<string>
            {
                "ays",
                "eys",
                "iys",
                "oys",
                "uys"
            }, (string s) => s.Remove(s.Length - 1, 1), out str2))
            {
                return str + str2;
            }
            if (suffixWord.EndsWith("ies", StringComparison.OrdinalIgnoreCase))
            {
                return str + suffixWord.Remove(suffixWord.Length - 3, 3) + "y";
            }
            if (oSuffixPluralizationService.ExistsInSecond(suffixWord))
            {
                return str + oSuffixPluralizationService.GetFirstValue(suffixWord);
            }
            if (EnglishPluralizationServiceUtil.TryInflectOnSuffixInWord(suffixWord, new List<string>
            {
                "aos",
                "eos",
                "ios",
                "oos",
                "uos"
            }, (string s) => suffixWord.Remove(suffixWord.Length - 1, 1), out str2))
            {
                return str + str2;
            }
            if (EnglishPluralizationServiceUtil.TryInflectOnSuffixInWord(suffixWord, new List<string>
            {
                "ces"
            }, (string s) => s.Remove(s.Length - 1, 1), out str2))
            {
                return str + str2;
            }
            if (EnglishPluralizationServiceUtil.TryInflectOnSuffixInWord(suffixWord, new List<string>
            {
                "ces",
                "ses",
                "xes"
            }, (string s) => s.Remove(s.Length - 2, 2), out str2))
            {
                return str + str2;
            }
            if (suffixWord.EndsWith("oes", StringComparison.OrdinalIgnoreCase))
            {
                return str + suffixWord.Remove(suffixWord.Length - 2, 2);
            }
            if (suffixWord.EndsWith("ss", StringComparison.OrdinalIgnoreCase))
            {
                return str + suffixWord;
            }
            if (suffixWord.EndsWith("s", StringComparison.OrdinalIgnoreCase))
            {
                return str + suffixWord.Remove(suffixWord.Length - 1, 1);
            }
            return str + suffixWord;
        }

        bool IsPlural(string word)
        {
            return userDictionary.ExistsInSecond(word) || (!userDictionary.ExistsInFirst(word) && (IsUninflective(word) || knownPluralWords.Contains(word.ToLower()) || !Singularize(word).Equals(word)));
        }

        string Capitalize(string word, Func<string, string> action)
        {
            string text = action(word);
            if (!IsCapitalized(word))
            {
                return text;
            }
            if (text.Length == 0)
            {
                return text;
            }
            StringBuilder stringBuilder = new StringBuilder(text.Length);
            stringBuilder.Append(char.ToUpperInvariant(text[0]));
            stringBuilder.Append(text.Substring(1));
            return stringBuilder.ToString();
        }

        string GetSuffixWord(string word, out string prefixWord)
        {
            int num = word.LastIndexOf(' ');
            prefixWord = word.Substring(0, num + 1);
            return word.Substring(num + 1);
        }

        bool IsCapitalized(string word)
        {
            return !string.IsNullOrEmpty(word) && char.IsUpper(word, 0);
        }

        bool IsAlphabets(string word)
        {
            return !string.IsNullOrEmpty(word.Trim()) && word.Equals(word.Trim()) && !Regex.IsMatch(word, "[^a-zA-Z\\s]");
        }

        bool IsUninflective(string word)
        {
            return EnglishPluralizationServiceUtil.DoesWordContainSuffix(word, uninflectiveSuffixes) || (!word.ToLower().Equals(word) && word.EndsWith("ese", StringComparison.OrdinalIgnoreCase)) || uninflectiveWords.Contains(word.ToLowerInvariant());
        }

        bool IsNoOpWord(string word)
        {
            return !IsAlphabets(word) || word.Length <= 1 || pronounList.Contains(word.ToLowerInvariant());
        }

        #endregion
    }
}
