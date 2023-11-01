using HPScreen.Admin;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HPScreen.Entities
{
    public class QuoteDisplay
    {
        #region Singleton Implementation
        private static QuoteDisplay instance;
        private static object _lock = new object();
        public static QuoteDisplay Current
        {
            get
            {
                if (instance == null)
                {
                    lock (_lock)
                    {
                        if (instance == null)
                        {
                            instance = new QuoteDisplay();
                        }
                    }
                }
                return instance;
            }
        }
        #endregion
        public List<Tuple<string, string>> Quotes { get; set; }
        public Font QuoteFont { get; set; }
        public Font AuthorFont { get; set; }
        public string CurrentQuote { get; set; }
        public List<string> ParsedQuotes { get; set; }
        public string CurrentAuthor { get; set; }
        public float PosX { get; set; }
        public float PosY { get; set; }
        public const int LINE_HEIGHT_PAD = 8;
        public float CurrentLineHeight { get { return Graphics.Current.StringHeight(ParsedQuotes[0], QuoteFont) + LINE_HEIGHT_PAD; } }
        public enum ModeValue
        {
            Invisible,
            FadeIn,
            Display,
            FadeOut
        }
        public QuoteDisplay()
        {
            PosX = Graphics.Current.ScreenMidX;
            PosY = Graphics.Current.ScreenHeight * (0.75f);

            QuoteFont = new Font(Color.Blue, Font.Type.arial, Font.Size.SIZE_L, true);
            AuthorFont = new Font(Color.White, Font.Type.arial, Font.Size.SIZE_M, true);
            Quotes = new List<Tuple<string, string>>();
            ParsedQuotes = new List<string>();
            InitQuotes();
            SetCurrentQuote();

            InvisibleTimer = Ran.Current.Next(180, 280); // first quote time
            ResetFadeInTimer();
            ResetDisplayTimer();
            ResetFadeOutTimer();
            ResetDisplayAuthorTimer();


            SetMode(ModeValue.Invisible);

            // First display the quote
            // Then display the person who said it with a delay

            // Bonus: It would be neat if it not only faded in, but ever so slightly spun in a circle too during transition (and then settled)
        }

        public void Update()
        {
            if (Mode == ModeValue.Invisible)
            {
                InvisibleTimer--;

                if (InvisibleTimer < 0)
                {
                    SetMode(ModeValue.FadeIn);
                    ResetInvisibleTimer();
                    SetCurrentQuote();
                }
            }
            else if (Mode == ModeValue.FadeIn)
            {
                FadeInTimer--;
                if (FadeInTimer < 0)
                {
                    SetMode(ModeValue.Display);
                    ResetFadeInTimer();
                }
            }
            else if (Mode == ModeValue.Display)
            {
                DisplayTimer--;
                DisplayAuthorTimer--;
                if (DisplayTimer < 0)
                {
                    SetMode(ModeValue.FadeOut);
                    ResetDisplayTimer();
                    ResetDisplayAuthorTimer();
                }
            }
            else if (Mode == ModeValue.FadeOut)
            {
                FadeOutTimer--;
                if (FadeOutTimer < 0)
                {
                    SetMode(ModeValue.Invisible);
                    ResetFadeOutTimer();
                }
            }
        }
        public void Draw()
        {
            if (Mode == ModeValue.Invisible) return;

            Graphics.Current.SpriteB.Begin();
            if (Mode == ModeValue.FadeIn)
            {
                float alpha = 1 - ((float)FadeInTimer / (float)FADE_TIME);
                for (int i = 0; i < ParsedQuotes.Count; i++)
                {
                    Graphics.Current.DrawString(ParsedQuotes[i], new Vector2(PosX, PosY + CurrentLineHeight * i), QuoteFont, true, false, false, QuoteFont.Color * alpha);
                }
            }
            else if (Mode == ModeValue.Display)
            {
                for (int i = 0; i < ParsedQuotes.Count; i++)
                {
                    Graphics.Current.DrawString(ParsedQuotes[i], new Vector2(PosX, PosY + CurrentLineHeight * i), QuoteFont, true, false);
                }
                if (DisplayAuthor)
                {
                    string author = $"- {CurrentAuthor}";
                    float authposx = Graphics.Current.RightAlignStringX((int)PosX, author, QuoteFont);
                    Graphics.Current.DrawString(author, new Vector2(authposx, PosY + CurrentLineHeight * ParsedQuotes.Count), AuthorFont, true, false);
                }
            }
            else if (Mode == ModeValue.FadeOut)
            {
                float alpha = (float)FadeOutTimer / (float)FADE_TIME;
                for (int i = 0; i < ParsedQuotes.Count; i++)
                {
                    Graphics.Current.DrawString(ParsedQuotes[i], new Vector2(PosX, PosY + CurrentLineHeight * i), QuoteFont, true, false, false, QuoteFont.Color * alpha);
                }
                string author = $"- {CurrentAuthor}";
                float authposx = Graphics.Current.RightAlignStringX((int)PosX, author, QuoteFont);
                Graphics.Current.DrawString(author, new Vector2(authposx, PosY + CurrentLineHeight * ParsedQuotes.Count), AuthorFont, true, false, false, AuthorFont.Color * alpha);
            }
            Graphics.Current.SpriteB.End();
        }

        #region Modes
        protected int InvisibleTimer { get; set; }
        protected int FadeInTimer { get; set; }
        protected const int FADE_TIME = 140;
        protected int FadeOutTimer { get; set; }
        protected int DisplayTimer { get; set; }
        protected int DisplayAuthorTimer { get; set; }
        protected bool DisplayAuthor { get { return DisplayAuthorTimer <= 0; } }
        protected const int FRAMES_PER_WORD = 15;
        protected const int FRAMES_FOR_AUTHOR = 300;
        protected ModeValue Mode { get; set; }
        protected void SetMode(ModeValue mode)
        {
            Mode = mode;
            switch (mode)
            {
                case ModeValue.Invisible:
                    break;

                case ModeValue.FadeIn:
                    break;

                case ModeValue.Display:
                    break;

                case ModeValue.FadeOut:
                    break;
            }
        }
        #endregion

        protected void SetCurrentQuote()
        {
            int quotenum = Ran.Current.Next(0, Quotes.Count - 1);
            CurrentAuthor = CleanString(Quotes[quotenum].Item1);
            CurrentQuote = CleanString(Quotes[quotenum].Item2);

            // Format Quote:
            ParsedQuotes.Clear();
            int len = CurrentQuote.Length;
            if (len < 20)
            {
                QuoteFont.SetFontSize(Font.Size.SIZE_XL);
                ParsedQuotes.Add(CurrentQuote);
            }
            else if (len < 42)
            {
                QuoteFont.SetFontSize(Font.Size.SIZE_L);
                ParsedQuotes.Add(CurrentQuote);
            }
            else // Chop into lines:
            {
                QuoteFont.SetFontSize(Font.Size.SIZE_M);
                int maxcharsperline = 42;
                string[] words = CurrentQuote.Split(' ');
                StringBuilder curline = new StringBuilder();
                for (int i = 0; i < words.Length; i++)
                {
                    if (curline.Length > maxcharsperline)
                    {
                        ParsedQuotes.Add(curline.ToString());
                        curline.Clear();
                    }
                    curline.Append(words[i]);
                    curline.Append(" ");
                }

                // Final line:
                if (curline.Length > 0)
                    ParsedQuotes.Add(curline.ToString());
            }
            AuthorFont.SetFontSize(QuoteFont.GetFontSize());
            AuthorFont.DecreaseFontSize();

            float baseY = Graphics.Current.ScreenHeight * 0.75f;
            PosY = baseY - ((ParsedQuotes.Count - 1) * CurrentLineHeight);
        }
        protected void ResetInvisibleTimer()
        {
            InvisibleTimer = Ran.Current.Next(360, 860);
        }
        protected void ResetFadeInTimer()
        {
            FadeInTimer = FADE_TIME;
        }
        protected void ResetFadeOutTimer()
        {
            FadeOutTimer = FADE_TIME;
        }
        protected void ResetDisplayTimer()
        {
            int numwords = CurrentQuote?.Split(' ').Length ?? 0;
            DisplayTimer = (numwords * FRAMES_PER_WORD) + FRAMES_FOR_AUTHOR + 480;
        }
        protected void ResetDisplayAuthorTimer()
        {
            int numwords = CurrentQuote?.Split(' ').Length ?? 0;
            DisplayAuthorTimer = (numwords * FRAMES_PER_WORD);
        }
        protected void InitQuotes()
        {
            AddQ("Rubeus Hagrid", "Yer a wizard Harry.");
            AddQ("Hermione Granger", "I hope you're pleased with yourselves. We could all have been killed — or worse, expelled");
            AddQ("Albus Dumbledore", "It does not do well to dwell on dreams and forget to live");
            AddQ("Albus Dumbledore", "To the well-organized mind, death is but the next great adventure");
            AddQ("Ron Weasley", "You're a little scary sometimes, you know that? Brilliant... but scary");
            AddQ("Severus Snape", "For those select few who possess the predisposition, I can teach you how to bewitch the mind and ensnare the senses.");
            AddQ("Albus Dumbledore", "It takes a great deal of bravery to stand up to our enemies, but just as much to stand up to our friends");
            AddQ("Albus Dumbledore", "Not a scar, no visible sign… to have been loved so deeply, even though the person who loved us is gone, will give us some protection forever");
            AddQ("Albus Dumbledore", "...the trouble is, humans do have a knack of choosing precisely those things that are worst for them.");
            AddQ("Albus Dumbledore", "The truth. It is a beautiful and terrible thing, and should therefore be treated with great caution");
            AddQ("Albus Dumbledore", "Ah, music. A magic beyond all we do here!");
            AddQ("Hermione Granger", "Fear of a name only increases fear of the thing itself");
            AddQ("Harry Potter", "I’ll be in my bedroom, making no noise and pretending I’m not there");
            AddQ("Ron Weasley", "When in doubt, go to the library");
            AddQ("Gilderoy Lockhart", "Fame is a fickle friend, Harry");
            AddQ("Gilderoy Lockhart", "Celebrity is as celebrity does. Remember that");
            AddQ("Draco Malfoy", "Honestly, if you were any slower, you’d be going backward");
            AddQ("Harry Potter", "I solemnly swear I am up to no good");
            AddQ("Albus Dumbledore", "Happiness can be found, even in the darkest of times, if one only remembers to turn on the light");
            AddQ("Ron Weasley", "Don’t let the muggles get you down");
            AddQ("Sirius Black", "I want to commit the murder I was imprisoned for");
            AddQ("Cornelius Fudge", "Why, dear boy, we don’t send wizards to Azkaban just for blowing up their aunts");
            AddQ("Sirius Black", "If you want to know what a man’s like, take a good look at how he treats his inferiors, not his equals");
            AddQ("Rubeus Hagrid", "I am what I am, an' I'm not ashamed. 'Never be ashamed,' my ol' dad used ter say");
            AddQ("Albus Dumbledore", "It matters not what someone is born, but what they grow to be");
            AddQ("Albus Dumbledore", "Numbing the pain for a while will make it worse when you finally feel it");
            AddQ("Fred Weasley", "Anyone can speak Troll. All you have to do is point and grunt");
            AddQ("Albus Dumbledore", "We are only as strong as we are united, as weak as we are divided");
            AddQ("Hermione Granger", "Just because it’s taken you three years to notice, Ron, doesn’t mean no one else has spotted I’m a girl!");
            AddQ("Ron Weasley", "Accio Brain!");
            AddQ("Fred Weasley", "I think we've outgrown full-time education ... Time to test our talents in the real world, d'you reckon?");
            AddQ("Severus Snape", "The mind is not a book, to be opened at will and examined at leisure");
            AddQ("Severus Snape", "The mind is a complex and many-layered thing");
            AddQ("Luna Lovegood", "I think I'll just go down and have some pudding and wait for it all to turn up — it always does in the end");
            AddQ("Hermione Granger", "Just because you have the emotional range of a teaspoon doesn't mean we all have");
            AddQ("Luna Lovegood", "People used to believe there were no such things as the Blibbering Humdinger or the Crumple-Horned Snorkack!");
            AddQ("Hermione Granger", "I mean, it's sort of exciting, isn't it, breaking the rules?");
            AddQ("Sirius Black", "We’ve all got both light and dark inside us. What matters is the part we choose to act on. That’s who we really are");
            AddQ("Albus Dumbledore", "Indifference and neglect often do much more damage than outright dislike");
            AddQ("Luna Lovegood", "Things we lose have a way of coming back to us in the end, if not always in the way we expect");
            AddQ("Albus Dumbledore", "Youth can not know how age thinks and feels. But old men are guilty if they forget what it was to be young");
            AddQ("Ginny Weasley", "The thing about growing up with Fred and George is that you sort of start thinking anything's possible if you've got enough nerve");
            AddQ("Hermione Granger", "Dumbledore says people find it far easier to forgive others for being wrong than being right");
            AddQ("Luna Lovegood", "You’re just as sane as I am");
            AddQ("Seamus Finnigan", "I am a wizard, not a baboon brandishing a stick");
            AddQ("Nearly Headless Nick", "Once again, you show all the sensitivity of a blunt axe");
            AddQ("Albus Dumbledore", "It is the unknown we fear when we look upon death and darkness, nothing more");
            AddQ("Remus Lupin", "It is the quality of one’s convictions that determines success, not the number of followers");
            AddQ("Harry Potter", "I’ve had enough trouble for a lifetime");
            AddQ("Kingsley Shacklebolt", "We’re all human, aren’t we? Every human life is worth the same, and worth saving");
            AddQ("Fred Weasley", "He can run faster than Severus Snape confronted with shampoo");
            AddQ("Albus Dumbledore", "Words are, in my not-so-humble opinion, our most inexhaustible source of magic. Capable of both inflicting injury, and remedying it");
            AddQ("Minerva McGonagall", "I've always wanted to use that spell");
            AddQ("Albus Dumbledore", "Of course it is happening inside your head, Harry, but why on earth should that mean that it is not real?");
            AddQ("Albus Dumbledore", "Do not pity the dead, Harry. Pity the living, and, above all, those who live without love");
            AddQ("Dumbledore and Snape", "‘After all this time?’ ‘Always’");
            AddQ("Severus Snape", "Obviously");
            AddQ("Severus Snape", "I Can Teach You How To bottle fame, brew glory, even stop death!");
            AddQ("Severus Snape", "Are You Incapable Of Restraining Yourself, Or Do You Take Pride In Being An Insufferable Know-It-All?");
            AddQ("Severus Snape", "You Dare Use My Own Spells Against Me, Potter?");
            AddQ("Ron Weasley", "Why Spiders? Why Couldn't It Be 'Follow The Butterflies'?");
            AddQ("Ron Weasley", "Harry, It's You That Has To Go On. Not Me, Not Hermione. You!");
            AddQ("Cornelius Fudge", "I must say, you're taking it a lot better than your predecessor. He tried to throw me out of the window");
            AddQ("Cornelius Fudge", "A finger. That's all that was left, a finger. Nothing else");
            AddQ("Cornelius Fudge", "It gives me great pleasure to welcome each and every one of you to the Finals of the 422nd Quidditch World Cup. Let the match begin!");
            AddQ("Draco Malfoy", "Reading? I Didn't Know You Could Read");
            AddQ("Draco Malfoy", "My Father Will Hear About This!");
            AddQ("Draco Malfoy", "You're Gonna Regret This! You And Your Bloody Chicken!");
            AddQ("Draco Malfoy", "If Brains Were Gold, You'd Be Poorer Than Weasley");
            AddQ("Minerva McGonagall", "Five points will be awarded to each of you for sheer dumb luck");
            AddQ("Minerva McGonagall", "Why is it when something happens, it is always you three?");
            AddQ("Minerva McGonagall", "Why don’t you confer with Mr. Finnigan? As I recall, he has a particular proclivity for pyrotechnics");
            AddQ("Minerva McGonagall", "Perhaps it would be more useful if I were to transfigure Mr. Potter and yourself into a pocket watch?");
            AddQ("Minerva McGonagall", "Wood, I have found you a Seeker!");
            AddQ("Minerva McGonagall", "Have a Biscuit, Potter.");
            AddQ("Albus Dumbledore", "Merlin’s beard, no!");
            AddQ("Cedric Diggory", "Go on, take it! You saved me, take it!");
            AddQ("Cedric Diggory", "Just take your egg and... mull things over in the hot water");
            AddQ("Dolores Umbridge", "Things at Hogwarts are far worse than I feared");
            AddQ("Dolores Umbridge", "Deep down, you know that you deserve to be punished");
            AddQ("Dolores Umbridge", "You know, I really hate children");
            AddQ("Dolores Umbridge", "I will have order!");
            AddQ("Dolores Umbridge", "Boys and girls are not to be within eight inches of each other");
            AddQ("Remus Lupin", "The Dementors Affect You Worst Of All Because You Have True Horrors In Your Past");
            AddQ("Remus Lupin", "That Suggests That What You Fear The Most Is Fear Itself. That Is Very Wise");
            AddQ("Remus Lupin", "Just Remember Fleur, Bill Takes His Steaks On The Raw Side Now");
            AddQ("Ginny Weasley", "I never really gave up on you. Not really");
            AddQ("Ginny Weasley", "Shut it");
            AddQ("Firenze", "This is where I leave you. You are safe now");
            AddQ("Firenze", "The planets have been read wrongly before now, even by centaurs. I hope this is one of those times");
            AddQ("Sybill Trelawney", "How nice to see you in the physical world at last");
            AddQ("Sybill Trelawney", "Sit, my children, sit");
            AddQ("Sybill Trelawney", "I find that descending too often into the hustle and bustle of the main school\r\nclouds my Inner Eye");
            AddQ("Sybill Trelawney", "I wouldn’t be so sure if I were you, dear");
            AddQ("Sybill Trelawney", "You’ll forgive me for saying so, my dear, but I perceive very little aura around you. Very little receptivity to the resonances of the future");
            AddQ("Moaning Myrtle", "I do have feelings, you know, even if I am dead");
            AddQ("Moaning Myrtle", "Ten points if you can get it through her stomach! Fifty points if it goes through her head!");
            AddQ("Moaning Myrtle", "I’d hidden because Olive Hornby was teasing me about my glasses");
            AddQ("Moaning Myrtle", "I just remember seeing a pair of great big yellow eyes…");
            AddQ("Moaning Myrtle", "I sometimes go down there... sometimes don’t have any choice, if someone flushes my toilet when I’m not expecting it");
        }
        protected void AddQ(string author, string quote)
        {
            Quotes.Add(new Tuple<string, string>(author, quote));
        }
        protected string CleanString(string dirtystring)
        {
            StringBuilder cleanstring = new StringBuilder();
            SpriteFont sf = Graphics.Current.Fonts[QuoteFont.Name];
            for (int i = 0; i < dirtystring.Length; i++)
            {
                if (sf.Characters.Contains(dirtystring[i]))
                {
                    cleanstring.Append(dirtystring[i]);
                }
            }
            return cleanstring.ToString();
        }
    }
}
