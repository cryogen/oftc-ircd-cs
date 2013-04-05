namespace oftc_ircd_cs
{
    public class General
    {
        public static GeneralSection Conf { get; set; }

        public static void Init()
        {
            Conf = new GeneralSection();
            Config.AddSection("general", Conf);
        }
    }
}