using tik4net;
using tik4net.Objects;
using tik4net.Objects.CapsMan;


namespace tik4net.Objects.CapsMan
{
    [TikEntity("/caps-man/security")]
    public class CapsManSecurity
    {
        [TikProperty(".id", IsReadOnly = true, IsMandatory = true)]
        public string Id { get; private set; }

        [TikProperty("comment")]
        public string Comment { get; set; }

        [TikProperty("passphrase")]
        public string Passphrase { get; set; }
    }
}