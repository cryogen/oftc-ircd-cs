namespace oftc_ircd_cs
{
    public interface IConfigSection
    {
        void SetDefaults();
        void Process(object o);
        void Verify();
    }
}